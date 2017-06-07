using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditRenterDialog.xaml
    /// </summary>
    public partial class EditRenterDialog : INotifyPropertyChanged
    {
        private const int OtherNatPosInComboBox = 6;
        private Renter _renterBeingUpdated;

        public Action<Renter> UpdateRenterAction;
        public Func<Renter> PassParameterToDialogAction;
        public ObservableCollection<Nationality> Nationalities { get; set; }

        public Renter RenterBeingUpdated
        {
            get => _renterBeingUpdated;
            set
            {
                _renterBeingUpdated = value;
                OnPropertyChanged(nameof(RenterBeingUpdated));
            }
        }


        public EditRenterDialog()
        {
            InitializeComponent();
            Nationalities = new ObservableCollection<Nationality>(NationalityDAO.Instance.GetAllNationalities());
        }

        private void EditRenter_OnLoaded(object sender, RoutedEventArgs e)
        {
            RenterBeingUpdated = PassParameterToDialogAction();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CbbNationality.SelectedIndex == OtherNatPosInComboBox && NationalityDAO.Instance.AddNewNat(TxbOtherNat.Text))
                {
                    var newNatId = (int)DataProvider.Instance.ExecuteScalar("EXEC USP_GetNewestNationalityId");
                    var newNatName = TxbOtherNat.Text;

                    RenterBeingUpdated.Nationality.NatId = newNatId;
                    RenterBeingUpdated.Nationality.NatName = newNatName;

                    Nationalities.Add(new Nationality { NatId = newNatId, NatName = newNatName });
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Quốc tịch này đã có, vui lòng chọn trong danh sách");
            }

            UpdateRenterAction(RenterBeingUpdated);
            Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbbNationality.SelectedIndex == OtherNatPosInComboBox)
            {
                TxbOtherNat.Visibility = Visibility.Visible;
            }
            else
            {
                TxbOtherNat.Visibility = Visibility.Collapsed;
                TxbOtherNat.Text = "";
            }
        }

        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
