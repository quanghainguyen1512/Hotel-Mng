using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DAO;
using DTO;
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for UsingServiceWindow.xaml
    /// </summary>
    public partial class UsingServiceWindow : INotifyPropertyChanged
    {
        private ObservableCollection<UseService> _useServices;
        private int _formId;
        public Func<int> PassParameterFunc { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public ObservableCollection<UseService> UseServices
        {
            get => _useServices;
            set
            {
                _useServices = value;
                OnPropertyChanged(nameof(UseServices));
            }
        }

        public UsingServiceWindow()
        {
            InitializeComponent();
            Services = ServiceDAO.Instance.GetAllServices();

            var view = (CollectionView)CollectionViewSource.GetDefaultView(Services);
            view.GroupDescriptions?.Add(new PropertyGroupDescription("SvType.SvTypeName"));
        }

        private void UsingServiceWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var roomId = PassParameterFunc();
            _formId = (int) DataProvider.Instance.ExecuteScalar("EXEC USP_GetSelectedFormId @roomid", new object[] {roomId});
            UseServices = new ObservableCollection<UseService>(UseServiceDAO.Instance.GetServicesBeingUsed(_formId));
        }

        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var rowBeingDeleted = (UseService) btn.DataContext;

            if (UseServiceDAO.Instance.DeleteItem(_formId, rowBeingDeleted.ServId))
                UseServices.Remove(rowBeingDeleted);
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var s = CbbServices.SelectedItem as Service;
            if (s is null) return;

            var us = new UseService
            {
                Quantity = Convert.ToInt32(NumUpDownQuantity.Value),
                ServId = s.ServId,
                SvName = s.Name,
                Time = DateTime.Now
            };

            try
            {
                if (UseServiceDAO.Instance.AddItem(us, _formId))
                    UseServices.Add(us);
            }
            catch (SqlException)
            {
                MessageBox.Show("Dịch vụ đã có trong danh sách, bạn có thể thay đổi số lượng");
            }
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
