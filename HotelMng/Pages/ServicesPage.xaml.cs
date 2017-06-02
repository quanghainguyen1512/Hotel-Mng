using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DAO;
using DTO;
using HotelMng.SubWindows;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage
    {
        public ObservableCollection<ServiceType> ServiceTypes { get; set; }
        public ObservableCollection<Service> Services { get; set; }
        private readonly CollectionView _view;
        public ServicesPage()
        {
            InitializeComponent();
            ServiceTypes = new ObservableCollection<ServiceType>(ServiceTypeDAO.Instance.GetAllServiceTypes()); 
            Services = new ObservableCollection<Service>(ServiceDAO.Instance.GetAllServices());

            _view = (CollectionView) CollectionViewSource.GetDefaultView(Services);
            _view.GroupDescriptions?.Add(new PropertyGroupDescription("SvType.SvTypeName"));
        }

        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Chắc chắn xóa mục này ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                var btn = sender as Button;
                if (btn is null) return;

                var rowBeingDeleted = (Service)btn.DataContext;

                if (ServiceDAO.Instance.DeleteService(rowBeingDeleted.ServId))
                    Services.Remove(rowBeingDeleted);
            }
        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var rowBeingEdited = Services.FirstOrDefault(s => s.ServId == ((Service)btn.DataContext).ServId);

            var dialog = new EditServiceDialog
            {
                UpdateServiceAction = service =>
                {
                    if (ServiceDAO.Instance.UpdateService(rowBeingEdited))
                    {
                        rowBeingEdited = service;
                    }
                    _view.Refresh();
                },
                PassParameterToDialogFunc = () => rowBeingEdited,
                PassServiceTypeFunc = () => ServiceTypes
            };
            dialog.ShowDialog();
        }

        private void AddServiceTypeInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AddNewServiceTypeDialog
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
            //var sv = new Service
            //{
            //    Name = TxtName.Text,
            //    Price = (int)NumUpDownPrice.Value,
            //    SvTypeId = serviceType.SvTypeId,
            //    Unit = TxtUnit.Text
            //};
            //if (ServiceDAO.Instance.AddNewService(sv))
            //{
            //    var data = ServiceDAO.Instance.NewestServiceInfo();
            //    sv.ServId = data.Item1;
            //    sv.SvTypeName = data.Item2;
            //    Services.Add(sv);
            //    MessageBox.Show("Thêm thành công");
            //}
        }
    }
}
