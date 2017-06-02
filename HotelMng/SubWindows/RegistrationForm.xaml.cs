using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using DTO.Annotations;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : INotifyPropertyChanged
    {
        public Func<Room> PassParameterFunc;
        public Action<RegForm> UpdateRegFormAction;
        private ObservableCollection<Nationality> _nationalities;
        private RegForm _form;

        public ObservableCollection<Nationality> Nationalities
        {
            get => _nationalities;
            set
            {
                _nationalities = value;
                OnPropertyChanged(nameof(Nationalities));
            }
        }

        public RegForm Form
        {
            get => _form;
            set
            {
                _form = value; 
                OnPropertyChanged(nameof(Form));
            }
        }

        public RegistrationForm()
        {
            InitializeComponent();
            Form = new RegForm();
            Nationalities = new ObservableCollection<Nationality>(NationalityDAO.Instance.GetAllNationalities());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbbNationality.SelectedIndex == 6)
            {
                TxbOtherNat.Visibility = Visibility.Visible;
            }
            else
            {
                TxbOtherNat.Visibility = Visibility.Collapsed;
                TxbOtherNat.Text = "";
            }
        }
        private void RegistrationForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            Form.Room = PassParameterFunc();
        }
        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrEmpty(TxtIdNum.Text) ||
                CbbNationality.SelectedIndex < 0 || CheckInTime.SelectedDate is null)
                MessageBox.Show("Vui lòng điền đủ thông tin bắt buộc");
            UpdateRegFormAction(Form);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
