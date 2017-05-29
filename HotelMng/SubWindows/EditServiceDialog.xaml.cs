using System;
using System.Collections.Generic;
using System.Windows;
using DAO;
using DTO;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditServiceDialog.xaml
    /// </summary>
    public partial class EditServiceDialog
    {
        public Action<Service> UpdateServiceAction;
        public Func<Service> PassParameterToDialogFunc;
        public IEnumerable<ServiceType> ServiceTypes { get; set; }
        private Service _serviceBeingUpdated;
        public EditServiceDialog()
        {
            InitializeComponent();
            ServiceTypes = ServiceTypeDAO.Instance.GetAllServiceTypes();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            var serviceType = CbbServType.SelectedItem as ServiceType;

            _serviceBeingUpdated.UpdateProperties(TxbName.Text, (int)NumbUpDownPrice.Value, TxbUnit.Text, serviceType.SvTypeId);
            _serviceBeingUpdated.SvTypeName = DataProvider.Instance.
                ExecuteScalar($"SELECT SvTypeName FROM dbo.SERVICE_TYPE WHERE SvTypeId = '{serviceType.SvTypeId}'").ToString();

            UpdateServiceAction(_serviceBeingUpdated);

            this.Close();
        }

        private void EditServiceDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            _serviceBeingUpdated = PassParameterToDialogFunc();

            TxbName.Text = _serviceBeingUpdated.Name;
            NumbUpDownPrice.Value = _serviceBeingUpdated.Price;
            TxbUnit.Text = _serviceBeingUpdated.Unit;

            var selectedIndex = 0;
            for (var i = 0; i < CbbServType.Items.Count; i++)
            {
                var item = CbbServType.Items[i];
                if ((item as ServiceType).SvTypeId == _serviceBeingUpdated.SvTypeId)
                {
                    selectedIndex = i;
                    break;
                }
            }

            CbbServType.SelectedIndex = selectedIndex;
        }
    }
}
