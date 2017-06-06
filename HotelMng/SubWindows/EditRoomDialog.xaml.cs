using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditRoomDialog.xaml
    /// </summary>
    public partial class EditRoomDialog : INotifyPropertyChanged
    {
        public Func<Tuple<Room, IEnumerable<RoomStatus>, IEnumerable<char>>> PassParameterToDialogFunc;
        public Action<Room> UpdateRoomTypeAction;
        private Room _roomBeingEdited;
        private IEnumerable<RoomStatus> _roomStatusIEnumerable;
        private IEnumerable<char> _roomTypeIdEnumerable;

        public Room RoomBeingEdited
        {
            get => _roomBeingEdited;
            set
            {
                _roomBeingEdited = value;
                OnPropertyChanged(nameof(RoomBeingEdited));
            }
        }

        public IEnumerable<RoomStatus> RoomStatusIEnumerable
        {
            get => _roomStatusIEnumerable;
            set
            {
                _roomStatusIEnumerable = value;
                OnPropertyChanged(nameof(RoomStatusIEnumerable));
            }
        }

        public IEnumerable<char> RoomTypeIdEnumerable
        {
            get => _roomTypeIdEnumerable;
            set
            {
                _roomTypeIdEnumerable = value; 
                OnPropertyChanged(nameof(RoomStatusIEnumerable));
            }
        }

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
            RoomBeingEdited.RoomTypeId = Convert.ToChar(CbbRoomType.SelectedItem);
            RoomBeingEdited.RoomStatus = CbbRoomStatus.SelectionBoxItem as RoomStatus;
            UpdateRoomTypeAction(RoomBeingEdited);
            this.Close();
        }

        private void EditRoomDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            RoomBeingEdited = PassParameterToDialogFunc().Item1;
            RoomStatusIEnumerable = PassParameterToDialogFunc().Item2;
            RoomTypeIdEnumerable = PassParameterToDialogFunc().Item3;
            CbbRoomType.ItemsSource = RoomTypeIdEnumerable;

            //for (var i = 0; i < CbbRoomStatus.Items.Count; i++)
            //{
            //    var item = CbbRoomStatus.Items[i] as RoomStatus;
            //    if (item != null && item.StatusId == RoomBeingEdited.RoomStatus.StatusId)
            //    {
            //        CbbRoomStatus.SelectedIndex = i;
            //        break;
            //    }
            //}

            for (var i = 0; i < CbbRoomType.Items.Count; i++)
            {
                var item = Convert.ToChar(CbbRoomType.Items[i]);
                if (item == RoomBeingEdited.RoomTypeId)
                {
                    CbbRoomType.SelectedIndex = i;
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
