using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using HotelMng.SubWindows;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for RoomMngPage.xaml
    /// </summary>
    public partial class RoomMngPage
    {
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public RoomType RoomTypeBeingAdded { get; set; }
        public RoomMngPage()
        {
            InitializeComponent();
            RoomTypes = new ObservableCollection<RoomType>(RoomTypeDAO.Instance.GetAllRoomTypes()); 
        }

        private void ButtonEditRoomType_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var rowBeingEdited = RoomTypes.FirstOrDefault(r => r.RoomTypeId == ((RoomType)btn.DataContext).RoomTypeId);

            var dialog = new EditRoomTypeDialog()
            {
                UpdateRoomTypeAction = roomtype =>
                {
                    rowBeingEdited = roomtype;
                    RoomTypeDAO.Instance.UpdateRoomTypes(rowBeingEdited);
                },
                PassParameterToDialogFunc = () => rowBeingEdited
            };
            dialog.ShowDialog();
        }

        private void ButtonAddRoomType_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtRoomTypeId.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            //var rt = new RoomType
            //{
            //    RoomTypeId = Convert.ToChar(TxtNote.Text),
            //    PriceFirstHour = (int) NumUpDownPrice1StHour.Value,
            //    PriceByDay = (int) NumUpDownPriceByDay.Value,
            //    PricePerHour = (int) NumUpDownPricePerHour.Value,
            //    Note = TxtNote.Text
            //};
            if (RoomTypeDAO.Instance.AddRoomTypes(RoomTypeBeingAdded))
            {
                RoomTypes.Add(RoomTypeBeingAdded);
                MessageBox.Show("Thêm thành công");
            }
        }

    }
}
