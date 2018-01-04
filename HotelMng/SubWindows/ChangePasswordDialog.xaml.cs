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
using DAO;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for ChangePasswordDialog.xaml
    /// </summary>
    public partial class ChangePasswordDialog
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            if (!AccountDAO.Instance.LogIn(TxbUsername.Text, PwbOldPass.Password))
            {
                MessageBox.Show("Vui lòng kiểm tra lại Tên đăng nhập và Mật khẩu hiện tại");
            }
            else if (string.IsNullOrEmpty(PwbNewPass.Password))
            {
                MessageBox.Show("Mật khẩu mới không được để trống");
            }
            else if (PwbNewPass.Password != PwbConfirmNewPass.Password)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp với mật khẩu mới, vui lòng thử lại");
            }
            else
            {
                try
                {
                    if (AccountDAO.Instance.ChangePassword(TxbUsername.Text, PwbNewPass.Password))
                    {
                        MessageBox.Show("Đổi mật khẩu thành công");
                        Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại");
                }
            }
        }
    }
}
