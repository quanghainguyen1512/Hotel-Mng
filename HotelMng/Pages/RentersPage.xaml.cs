using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using HotelMng.SubWindows;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using DTO.Annotations;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for RentersPage.xaml
    /// </summary>
    public partial class RentersPage : INotifyPropertyChanged
    {
        private IEnumerable<Renter> _renters;

        public IEnumerable<Renter> Renters
        {
            get => _renters;
            set
            {
                _renters = value; 
                OnPropertyChanged(nameof(Renters));
            }
        }

        public RentersPage()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            Renters = RenterDAO.Instance.GetAllRenters();
        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var id = ((Renter)btn.DataContext).RenterId;
            var rowBeingEdited = Renters.FirstOrDefault(s => s.RenterId == id);

            var editor = new EditRenterDialog
            {
                UpdateRenterAction = renter =>
                {
                    if (RenterDAO.Instance.UpdateRenter(rowBeingEdited))
                        LoadData();
                },
                PassParameterToDialogAction = () => rowBeingEdited
            };
            editor.ShowDialog();
        }
        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var rowBeingDeleted = (Renter)btn?.DataContext;

            if (MessageBox.Show("Chắc chắn xóa mục này ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                try
                {
                    if (RenterDAO.Instance.DelRenter(rowBeingDeleted?.RenterId))
                        LoadData();

                }
                catch (SqlException)
                {
                    MessageBox.Show("Không thể xóa khách hàng này");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
