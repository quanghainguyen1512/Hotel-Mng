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
        public Action<string, int, string, int> UpdateServiceAction;
        public Func<Service> PassParameterToDialogAction;
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
                    UpdateServiceAction(TxbName.Text, (int) NumbUpDownPrice.Value, TxbUnit.Text, serviceType.SvTypeId);
        }

        private void EditServiceDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            var parameter = PassParameterToDialogAction();
            TxbName.Text = parameter.Name;
            NumbUpDownPrice.Value = parameter.Price;
            TxbUnit.Text = parameter.Unit;
        }
    }
}
