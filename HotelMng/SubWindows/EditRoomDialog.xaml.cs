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
    /// Interaction logic for EditRoomDialog.xaml
    /// </summary>
    public partial class EditRoomDialog
    {
        public Func<Room> PassParameterToDialogFunc;
        public Action<Room> UpdateRoomTypeAction;
        private Room _roomBeingEdited;

        public EditRoomDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void EditRoomDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            _roomBeingEdited = PassParameterToDialogFunc();
            TxtDescription.Text = _roomBeingEdited.Description;
            TxtCapacity.Text = _roomBeingEdited.Capacity.ToString();

            var selectedIndex = 0;
            for (int i = 0; i < CbbRoomType.Items.Count; i++)
            {
                var item = CbbRoomType.Items[i];
                if ((item as RoomType).RoomTypeId == _roomBeingEdited.RoomTypeId)
                {
                    selectedIndex = i;
                    break;
                }
            }
            CbbRoomType.SelectedIndex = selectedIndex;

            for (int i = 0; i < CbbRoomStatus.Items.Count; i++)
            {
                var item = CbbRoomStatus.Items[i];
                if ((item as RoomType).RoomTypeId == _roomBeingEdited.RoomTypeId)
                {
                    selectedIndex = i;
                    break;
                }
            }
            CbbRoomStatus.SelectedIndex = selectedIndex;
        }
    }
}
