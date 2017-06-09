using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DTO;
using DTO.Annotations;
using System.Windows.Controls;
using DAO;
using HotelMng.SubWindows;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for BillWindow.xaml
    /// </summary>
    public partial class BillWindow : INotifyPropertyChanged
    {
        private Bill _bill = new Bill { BillId = Bill.GenerateBillId() };
        public Action<RegForm> CheckOutBillAction { get; set; }
        public ObservableCollection<RegForm> Forms { get; set; } = new ObservableCollection<RegForm>();

        public Bill Bill
        {
            get => _bill;
            set
            {
                _bill = value; 
                OnPropertyChanged(nameof(Bill));
            }
        }

        public BillWindow()
        {
            InitializeComponent();
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
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

        private void ButtonDelItem_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var rowBeingDeleted = (RegForm)btn.DataContext;
            Forms.Remove(rowBeingDeleted);
            Bill.TotalMoney = Forms.Sum(f => f.Total);
            TotalMoneyDisplay.Value = Bill.TotalMoney;
        }

        private void ButtonAddForm_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddFormToBillDialog()
            {
                AddFomrAction = form =>
                {
                    form.CheckOut = DateTime.Now;
                    form.UseServices = UseServiceDAO.Instance.GetServicesBeingUsed(form.FormId);
                    form.ServiceCharge = RegFormDAO.Instance.GetServiceCharge(form.FormId);
                    form.Rental = RegFormDAO.Instance.GetRental(form.FormId);

                    form.Total = form.ServiceCharge + form.Rental;

                    Forms.Add(form);
                    Bill.TotalMoney = Forms.Sum(f => f.Total);
                    TotalMoneyDisplay.Value = Bill.TotalMoney;
                }
            };
            dialog.ShowDialog();
        }

        private void ButtonPay_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxbPayerName.Text) || string.IsNullOrEmpty(TxbCompany.Text) ||
                DataGridForms.Items.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa đầy đủ thông tin, vui lòng thử lại");
                return;
            }
            try
            {
                BillDAO.Instance.AddBill(Bill);
                foreach (var form in Forms)
                {
                    form.BillId = Bill.BillId;
                    CheckOutBillAction(form);
                }
                Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }
    }
}
