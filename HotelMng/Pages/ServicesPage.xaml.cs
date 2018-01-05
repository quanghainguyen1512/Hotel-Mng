using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DAO;
using DTO;
using DTO.Annotations;
using HotelMng.SubWindows;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : INotifyPropertyChanged
    {
        private CollectionView _view;
        private IEnumerable<Service> _services;

        public ObservableCollection<ServiceType> ServiceTypes { get; set; }

        public IEnumerable<Service> Services
        {
            get => _services;
            set
            {
                _services = value; 
                OnPropertyChanged(nameof(Services));
            }
        }

        public Service ServiceBeingAdded { get; set; } = new Service();

        public ServicesPage()
        {
            InitializeComponent();
            ServiceTypes = new ObservableCollection<ServiceType>(ServiceTypeDAO.Instance.GetAllServiceTypes()); 
            LoadData();

            _view.Filter = ServiceFilter;
        }

        void LoadData()
        {
            Services = ServiceDAO.Instance.GetAllServices();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Services);
            _view.GroupDescriptions?.Add(new PropertyGroupDescription("SvType.SvTypeName"));
        }

        private bool ServiceFilter(object item)
        {
            var str = TxtNameFilter.Text;

            if (string.IsNullOrEmpty(str))
                return true;

            var service = item as Service;
            return service != null && service.Name.IndexOf(str, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!AccountDAO.Instance.IsAdmin)
            {
                MessageBox.Show("Chỉ admin được quyền thực hiện thao tác này");
                return;
            }

            if (MessageBox.Show("Chắc chắn xóa mục này ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                if (!(sender is Button btn)) return;
                

                var rowBeingDeleted = (Service)btn.DataContext;

                if (ServiceDAO.Instance.DeleteService(rowBeingDeleted.ServId))
                {
                    LoadData();
                }
            }
        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn)) return;
            

            var rowBeingEdited = Services.FirstOrDefault(s => s.ServId == ((Service)btn.DataContext).ServId);

            var dialog = new EditServiceDialog
            {
                UpdateServiceAction = service =>
                {
                    if (ServiceDAO.Instance.UpdateService(rowBeingEdited))
                    {
                        LoadData();
                    }
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
            }
            else if (ServiceDAO.Instance.AddNewService(ServiceBeingAdded))
            {
                LoadData();
            }
        }

        private void TxtNameFilter_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _view.Refresh();
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
