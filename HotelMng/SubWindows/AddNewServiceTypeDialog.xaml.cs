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

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for AddNewServiceTypeWindow.xaml
    /// </summary>
    public partial class AddNewServiceTypeDialog
    {
        public Action<string> AddServiceAction;
        public AddNewServiceTypeDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxNewService.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại dịch vụ");
                return;
            }
            AddServiceAction(TextBoxNewService.Text);
            this.Close();
        }

        private void TextBoxNewService_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonOk.IsEnabled = !string.IsNullOrEmpty(TextBoxNewService.Text);
        }
    }
}
