
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAO;
using DTO;
using HotelMng.SubWindows;
using System.Collections.ObjectModel;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for RentersPage.xaml
    /// </summary>
    public partial class RentersPage : Page
    {
        public ObservableCollection<Renter> Renters { get; set; }
        public Renter RenterBeingAdded { get; set; }

        //private readonly CollectionView _view;

        public RentersPage()
        {
            InitializeComponent();
            Renters = new ObservableCollection<Renter>(RenterDAO.Instance.GetAllRenters());

            //var view = (CollectionView)CollectionViewSource.GetDefaultView(Renters);
            //view.GroupDescriptions?.Add(new PropertyGroupDescription("Renters"));

        }

        private void ButtonEditItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            //var rowBeingEdited = Renters.FirstOrDefault(s => s.RenterId == ((Renter)btn.DataContext).RenterId);
            var id = ((Renter)btn?.DataContext).RenterId;
            var rowBeingEdited = Renters.FirstOrDefault(s => s.RenterId == id);

            var editor = new EditRenter()
            {
                UpdateRenterAction = renter =>
                {
                    if (RenterDAO.Instance.UpdateRenter(rowBeingEdited))
                    {
                        rowBeingEdited = renter;
                    }
                    //_view.Refresh();
                },
                PassParameterToDialogAction = () => rowBeingEdited
            };
            editor.ShowDialog();

        }
        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Chắc chắn xóa mục này ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                var btn = sender as Button;
                var rowBeingDeleted = (Renter)btn?.DataContext;
                Renters.Remove(rowBeingDeleted);
                RenterDAO.Instance.DelRenter(rowBeingDeleted.RenterId);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtIdentityNum.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            //var rt = new Renter()
            //{
            //    RenterId = txtID.Text,
            //    Name = txtName.Text,
            //    Gender = txtGender.Text,
            //    PhoneNum = txtPhoneNum.Text,
            //    IdentityNum = txtIdentityNum.Text,
            //    Address = txtAddress.Text
            //};
            if (RenterDAO.Instance.AddRenter(txtName.Text, true, txtPhoneNum.Text, txtID.Text, txtIdentityNum.Text, txtAddress.Text))
            {
                Renters.Add(RenterBeingAdded);
                MessageBox.Show("Thêm thành công");
            }
        }
    }
}
