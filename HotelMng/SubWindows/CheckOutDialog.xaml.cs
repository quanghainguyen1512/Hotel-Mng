using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DAO;
using DTO;
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for CheckOutDialog.xaml
    /// </summary>
    public partial class CheckOutDialog : INotifyPropertyChanged
    {
        #region Fields
        private RegForm _form = new RegForm();
        private IEnumerable<UseService> _useServices;
        #endregion

        #region Properties

        public Func<int> PassParameterFunc { get; set; }
        public Action<RegForm> CheckOutAction { get; set; }
        public RegForm RegForm
        {
            get => _form;
            set
            {
                _form = value; 
                OnPropertyChanged(nameof(RegForm));
            }
        }

        public IEnumerable<UseService> UseServices
        {
            get => _useServices;
            set
            {
                _useServices = value; 
                OnPropertyChanged(nameof(UseServices));
            }
        }

        #endregion

        public CheckOutDialog()
        {
            InitializeComponent();
        }

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void CheckOutDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            var roomId = PassParameterFunc();

            RegForm.RoomId = roomId;
            RegForm.FormId = (int)DataProvider.Instance.ExecuteScalar("EXEC USP_GetSelectedFormId @roomid", new object[] { roomId });
            RegForm = RegFormDAO.Instance.GetFormById(RegForm.FormId);
            RegForm.CheckOut = DateTime.Now;

            RegForm.UseServices = UseServiceDAO.Instance.GetServicesBeingUsed(RegForm.FormId);
            RegForm.ServiceCharge = RegFormDAO.Instance.GetServiceCharge(RegForm.FormId);
            RegForm.Rental = RegFormDAO.Instance.GetRental(RegForm.FormId);

            RegForm.Total = RegForm.ServiceCharge + RegForm.Rental;
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonPay_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckOutAction(RegForm);
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Thanh toán không thành công");
            }
        }
    }
}
