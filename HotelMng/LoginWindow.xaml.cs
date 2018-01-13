using System;
using System.Windows;
using DAO;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void ButtonLogIn_OnClick(object sender, RoutedEventArgs e)
        {
            DataProvider.Instance.ServerName = TxbServerName.Text;
            try
            {
                LogInBtn.IsEnabled = false;
                LoadingIndicator.Visibility = Visibility.Visible;
                var logged = await AccountDAO.Instance.LogIn(TxbUsername.Text, TxbPassword.Password);
                LogInBtn.IsEnabled = true;
                LoadingIndicator.Visibility = Visibility.Hidden;

                if (!logged)
                {
                    MessageBox.Show("Đăng nhập thất bại, vui lòng thử lại");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sai Server Name, vui lòng thử lại" + ex.Message);
                return;
            }

            var main = new MainWindow();
            main.Show();
            
            Close();
        }

    }
}
