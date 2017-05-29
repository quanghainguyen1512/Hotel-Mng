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

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == null) throw new Exception("Unexpected error!");
            switch (rb.Name)
            {
                case nameof(RbtnCheckIn):
                    CheckInTime.SelectedDate = DateTime.Now;
                    break;
                case nameof(RbtnReservation):
                    CheckInTime.SelectedDate = DateTime.Now.AddDays(1);
                    break;
            }
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxbOtherNat.Visibility = CbbNationality.SelectedIndex == CbbNationality.Items.Count - 1
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }
    }
}
