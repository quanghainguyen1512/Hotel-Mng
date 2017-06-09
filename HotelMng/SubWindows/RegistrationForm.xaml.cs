using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        public Func<int> PassParameterFunc;
        public Action<RegForm> UpdateRegFormAction;
        private RegForm _form;
        public ObservableCollection<Nationality> Nationalities { get; set; }

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
            CheckInTime.SelectedDate = DateTime.Now;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
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
            Form.RoomId = PassParameterFunc();
            Form.CheckIn = DateTime.Now;
        }
        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrEmpty(TxtIdNum.Text) ||
                CbbNationality.SelectedIndex < 0 || CheckInTime.SelectedDate is null)
                MessageBox.Show("Vui lòng điền đủ thông tin bắt buộc");
            UpdateRegFormAction(Form);
            Close();
        }

        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
