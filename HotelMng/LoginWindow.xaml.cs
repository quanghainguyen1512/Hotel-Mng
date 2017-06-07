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

        private void ButtonLogIn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!LogIn(TxbUsername.Text, TxbPassword.Password)) return;

            var main = new MainWindow();
            main.Show();
            Close();
        }

        bool LogIn(string username, string password)
        {
            var result = DataProvider.Instance.ExecuteQueries("EXEC USP_LogIn @username, @password",
                new object[] {username, password});
            return result.Rows.Count == 1;
        }
    }
}
