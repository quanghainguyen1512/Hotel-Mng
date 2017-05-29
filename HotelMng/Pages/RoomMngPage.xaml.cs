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
    public partial class RoomMngPage : Page
    {
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public RoomMngPage()
        {
            InitializeComponent();
            RoomTypes = RoomTypeDAO.Instance.GetAllRoomTypes();
        }

        private void ButtonEditRoomType_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var id = ((RoomType)btn?.DataContext).RoomTypeId;
            var rowBeingEdited = RoomTypes.FirstOrDefault(r => r.RoomTypeId == id);
            if (rowBeingEdited != null)
            {
                var editor = new EditRoomTypeDialog()
                {
                    UpdateRoomTypeAction = roomtype =>
                    {
                        rowBeingEdited = roomtype;
                        RoomTypeDAO.Instance.UpdateRoomTypes(rowBeingEdited);
                    },
                    PassParameterToDialogFunc = () => rowBeingEdited
                };
                editor.ShowDialog();
            }
        }

        private void ButtonAddRoomType_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtRoomTypeId.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            var rt = new RoomType
            {
                RoomTypeId = Convert.ToChar(TxtNote.Text),
                Price1StHour = (int) NumUpDownPrice1StHour.Value,
                PriceByDay = (int) NumUpDownPriceByDay.Value,
                PricePerHour = (int) NumUpDownPricePerHour.Value,
                Note = TxtNote.Text
            };
            if (RoomTypeDAO.Instance.AddRoomTypes(rt))
            {
                RoomTypes.Add(rt);
                MessageBox.Show("Thêm thành công");
            }
        }
    }
}
