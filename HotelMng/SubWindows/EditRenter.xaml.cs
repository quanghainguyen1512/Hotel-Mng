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
    /// Interaction logic for EditRenter.xaml
    /// </summary>
    public partial class EditRenter : Window
    {
        public Action<Renter> UpdateRenterAction;
        public Func<Renter> PassParameterToDialogAction;
        private Renter _renterBeingUpdated;
        public EditRenter()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _renterBeingUpdated = PassParameterToDialogAction();


            txtRenterId.Text = _renterBeingUpdated.RenterId;
            txtName.Text = _renterBeingUpdated.Name;
            txtGender.Text = _renterBeingUpdated.Gender;
            txtPhoneNum.Text = _renterBeingUpdated.PhoneNum;
            txtIdentityNum.Text = _renterBeingUpdated.IdentityNum;
            txtAddress.Text = _renterBeingUpdated.Address;

        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            _renterBeingUpdated = new Renter()
            {
                RenterId = txtRenterId.Text,
                Name = txtName.Text,
                Gender = txtGender.Text,
                PhoneNum = txtPhoneNum.Text,
                IdentityNum = txtIdentityNum.Text,
                Address = txtAddress.Text

            };
            UpdateRenterAction(_renterBeingUpdated);

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
