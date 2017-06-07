using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for RentersInRoomDialog.xaml
    /// </summary>
    public partial class RentersInRoomDialog : INotifyPropertyChanged
    {
        private const int OtherNatPosInComboBox = 6;
        private int _formId;
        private ObservableCollection<Roommate> _roommates;
        private Roommate _roommateBeingAdded = new Roommate();

        public Func<int> PassParameterFunc { get; set; }
        public Roommate RoommateBeingAdded
        {
            get => _roommateBeingAdded;
            set
            {
                _roommateBeingAdded = value; 
                OnPropertyChanged(nameof(RoommateBeingAdded));
            }
        }
        public ObservableCollection<Roommate> Roommates
        {
            get => _roommates;
            set
            {
                _roommates = value; 
                OnPropertyChanged(nameof(Roommates));
            }
        }
        public ObservableCollection<Nationality> Nationalities { get; set; }


        public RentersInRoomDialog()
        {
            InitializeComponent();
            Nationalities = new ObservableCollection<Nationality>(NationalityDAO.Instance.GetAllNationalities());
        }


        private void RentersInRoomDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            var roomId = PassParameterFunc();
            _formId = (int)DataProvider.Instance.ExecuteScalar("EXEC USP_GetSelectedFormId @roomid", new object[] { roomId });
            Roommates = new ObservableCollection<Roommate>(RoommateDAO.Instance.GetRoommates(_formId));
        }

        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var rowBeingDeleted = (Roommate)btn.DataContext;

            if (RoommateDAO.Instance.DelRoomate(rowBeingDeleted.Name, _formId))
                Roommates.Remove(rowBeingDeleted);
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var nat = CbbNationality.SelectedItem as Nationality;
            if (nat != null) RoommateBeingAdded.Nationality.NatName = nat.NatName;

            try
            {
                if (CbbNationality.SelectedIndex == OtherNatPosInComboBox && NationalityDAO.Instance.AddNewNat(TxbOtherNat.Text))
                {
                    var newNatId = (int)DataProvider.Instance.ExecuteScalar("EXEC USP_GetNewestNationalityId");
                    var newNatName = TxbOtherNat.Text;

                    RoommateBeingAdded.Nationality.NatId = newNatId;
                    RoommateBeingAdded.Nationality.NatName = newNatName;

                    Nationalities.Add(new Nationality {NatId = newNatId, NatName = newNatName});
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Quốc tịch này đã có, vui lòng chọn trong danh sách");
            }
            try
            {
                if (RoommateDAO.Instance.AddRoommate(RoommateBeingAdded, _formId))
                {
                    Roommates.Add(RoommateBeingAdded);
                    RoommateBeingAdded = new Roommate();
                }

            }
            catch (SqlException )
            {
                MessageBox.Show("Người này đã có trong phòng, nếu trùng tên hãy thêm số phía sau để phân biệt");
            }
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
        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
