using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAO;
using DTO;
using HotelMng.SubWindows;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ObservableCollection<ServiceType> ServiceTypes { get; set; }
        public ObservableCollection<Service> Services { get; set; }
        public ServicesPage()
        {
            InitializeComponent();
            ServiceTypes = ServiceTypeDAO.Instance.GetAllServiceTypes();
            Services = ServiceDAO.Instance.GetAllServices();

            var view = (CollectionView) CollectionViewSource.GetDefaultView(Services);
            view.GroupDescriptions?.Add(new PropertyGroupDescription("SvTypeName"));
        }

        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Chắc chắn xóa mục này ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                var btn = sender as Button;
                var rowBeingDeleted = (Service)btn?.DataContext;
                Services.Remove(rowBeingDeleted);
                ServiceDAO.Instance.DelService(rowBeingDeleted.ServId);
            }
        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var id = ((Service)btn?.DataContext).ServId;
            var rowBeingEdited = Services.FirstOrDefault(s => s.ServId == id);
            if (rowBeingEdited != null)
            {
                var edit = new EditServiceDialog
                {
                    UpdateServiceAction = (name, price, unit) =>
                    {
                        rowBeingEdited.UpdateProperties(name, price, unit);
                        ServiceDAO.Instance.UpdateService(rowBeingEdited);
                    },
                    PassParameterToDialogAction = () => rowBeingEdited
                };
                edit.ShowDialog();
            }
        }

        private void AddNewServiceType_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new SubWindows.AddNewServiceTypeDialog
            {
                AddServiceAction = s =>
                {
                    ServiceTypeDAO.Instance.AddNewType(s);
                    ServiceTypes.Clear();
                    ServiceTypes = ServiceTypeDAO.Instance.GetAllServiceTypes();
                }
            };
            window.ShowDialog();
        }

    }
}
