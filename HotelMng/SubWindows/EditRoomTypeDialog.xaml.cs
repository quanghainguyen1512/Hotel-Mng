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
using DTO;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditRoomTypeDialog.xaml
    /// </summary>
    public partial class EditRoomTypeDialog 
    {
        public Func<RoomType> PassParameterToDialogFunc;
        public Action<RoomType> UpdateRoomTypeAction;
        private RoomType _roomTypeBeingEdited;
        public EditRoomTypeDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            _roomTypeBeingEdited.PriceByDay = (int)NumbUpDownPriceByDay.Value;
            _roomTypeBeingEdited.Price1StHour = (int)NumbUpDownPrice1StHour.Value;
            _roomTypeBeingEdited.PricePerHour = (int)NumbUpDownPricePerHour.Value;
            _roomTypeBeingEdited.Note = TxtNote.Text;

            UpdateRoomTypeAction(_roomTypeBeingEdited);
            this.Close();
        }

        private void EditServiceDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            _roomTypeBeingEdited = PassParameterToDialogFunc();

            NumbUpDownPriceByDay.Value = _roomTypeBeingEdited.PriceByDay;
            NumbUpDownPrice1StHour.Value = _roomTypeBeingEdited.Price1StHour;
            NumbUpDownPricePerHour.Value = _roomTypeBeingEdited.PricePerHour;
            TxtNote.Text = _roomTypeBeingEdited.Note;
        }
    }
}
