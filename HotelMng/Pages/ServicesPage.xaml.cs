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

            var view = (CollectionView)CollectionViewSource.GetDefaultView(Services);
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
                var editor = new EditServiceDialog
                {
                    UpdateServiceAction = service =>
                    {
                        rowBeingEdited = service;
                        ServiceDAO.Instance.UpdateService(rowBeingEdited);
                    },
                    PassParameterToDialogAction = () => rowBeingEdited
                };
                editor.ShowDialog();
            }
        }

        private void AddServiceTypeInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new SubWindows.AddNewServiceTypeDialog
            {
                AddServiceAction = s =>
                {
                    if (ServiceTypeDAO.Instance.AddNewType(s))
                    {
                        ServiceTypes.Add(new ServiceType {SvTypeName = s, SvTypeId = ServiceTypeDAO.Instance.NewestServiceTypeId()});
                    }
                }
            };
            window.ShowDialog();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrEmpty(TxtUnit.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            var serviceType = CbbSvType.SelectedItem as ServiceType;
            if (NumUpDownPrice.Value is null || serviceType is null) return;
            var sv = new Service()
            {
                Name = TxtName.Text,
                Price = (int)NumUpDownPrice.Value,
                SvTypeId = serviceType.SvTypeId,
                Unit =  TxtUnit.Text
            };
            if (ServiceDAO.Instance.AddNewService(sv))
            {
                var data = ServiceDAO.Instance.NewestServiceInfo();
                sv.ServId = data.Item1;
                sv.SvTypeName = data.Item2;
                Services.Add(sv);                             
                MessageBox.Show("Thêm thành công");
            }
        }
    }
}
