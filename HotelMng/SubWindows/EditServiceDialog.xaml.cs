using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DAO;
using DTO;
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditServiceDialog.xaml
    /// </summary>
    public partial class EditServiceDialog : INotifyPropertyChanged
    {
        public Action<Service> UpdateServiceAction;
        public Func<Service> PassParameterToDialogFunc;
        public Func<IEnumerable<ServiceType>> PassServiceTypeFunc;


        private Service _serviceBeingUpdated;
        private IEnumerable<ServiceType> _serviceTypes;

        public IEnumerable<ServiceType> ServiceTypes
        {
            get => _serviceTypes;
            set
            {
                _serviceTypes = value; 
                OnPropertyChanged(nameof(ServiceTypes));
            }
        }

        public Service ServiceBeingUpdated
        {
            get => _serviceBeingUpdated;
            set
            {
                _serviceBeingUpdated = value; 
                OnPropertyChanged(nameof(ServiceBeingUpdated));
            }
        }

        public EditServiceDialog()
        {
            InitializeComponent();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceBeingUpdated.SvType = CbbServType.SelectedItem as ServiceType;
            UpdateServiceAction(ServiceBeingUpdated);
            this.Close();
        }

        private void EditServiceDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ServiceTypes = PassServiceTypeFunc();
            ServiceBeingUpdated = PassParameterToDialogFunc();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
