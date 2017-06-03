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
using System.Runtime.CompilerServices;
using DTO;
using DAO;
using DTO.Annotations;
using System.ComponentModel;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : INotifyPropertyChanged
    {
        public Login()
        {
            InitializeComponent();
        }

        bool FLogin(string usrename,string password)
        {
            return LoginDAO.Instance.FLogin(usrename, password);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Logined_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (FLogin(username,password))
            {
                var m = new MainWindow();
                this.Hide();
                m.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai Tên Đăng Nhập hoặc Mật Khẩu");
            }


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Thoát Chương Trình ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                this.Close();
            }

        }
    }
}
