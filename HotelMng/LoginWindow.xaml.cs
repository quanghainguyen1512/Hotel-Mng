﻿using System.Windows;
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
            if (!AccountDAO.Instance.LogIn(TxbUsername.Text, TxbPassword.Password))
            {
                MessageBox.Show("Đăng nhập thất bại, vui lòng thử lại");
                return;
            }

            var main = new MainWindow();
            main.Show();
            
            Close();
        }

    }
}
