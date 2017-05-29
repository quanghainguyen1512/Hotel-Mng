using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using DTO;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditServiceDialog.xaml
    /// </summary>
    public partial class EditServiceDialog
    {
        public Action<Service> UpdateServiceAction;
        public Func<Service> PassParameterToDialogAction;
        private Service _serviceBeingUpdated;
        public EditServiceDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            var serviceType = CbbServType.SelectedItem as ServiceType;
            if (serviceType != null)
                if (NumbUpDownPrice.Value != null)
                {
                    _serviceBeingUpdated = new Service()
                    {
                        Name = TxbName.Text,
                        Price = (int)NumbUpDownPrice.Value,
                        Unit = TxbUnit.Text,
                        SvTypeId = serviceType.SvTypeId
                    };
                }
            UpdateServiceAction(_serviceBeingUpdated);
        }

        private void EditServiceDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            _serviceBeingUpdated = PassParameterToDialogAction();
            TxbName.Text = _serviceBeingUpdated.Name;
            NumbUpDownPrice.Value = _serviceBeingUpdated.Price;
            TxbUnit.Text = _serviceBeingUpdated.Unit;
        }
    }
}
