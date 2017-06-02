using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using HotelMng.SubWindows;
using System.Collections.ObjectModel;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for RentersPage.xaml
    /// </summary>
    public partial class RentersPage : Page
    {
        public ObservableCollection<Renter> Renters { get; set; }

        public RentersPage()
        {
            InitializeComponent();
            Renters = new ObservableCollection<Renter>(RenterDAO.Instance.GetAllRenters());
        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var id = ((Renter)btn.DataContext).RenterId;
            var rowBeingEdited = Renters.FirstOrDefault(s => s.RenterId == id);

            var editor = new EditRenterDialog()
            {
                UpdateRenterAction = renter =>
                {
                    if (RenterDAO.Instance.UpdateRenter(rowBeingEdited))
                    {
                        rowBeingEdited = renter;
                    }
                },
                PassParameterToDialogAction = () => rowBeingEdited
            };
            editor.ShowDialog();

        }
        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Chắc chắn xóa mục này ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                var btn = sender as Button;
                var rowBeingDeleted = (Renter)btn?.DataContext;
                if (RenterDAO.Instance.DelRenter(rowBeingDeleted.RenterId))
                    Renters.Remove(rowBeingDeleted);
            }
        }
    }
}
