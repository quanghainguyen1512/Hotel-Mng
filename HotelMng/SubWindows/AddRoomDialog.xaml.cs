using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using DTO;
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for AddRoomDialog.xaml
    /// </summary>
    public partial class AddRoomDialog : INotifyPropertyChanged
    {
        private IEnumerable<RoomStatus> _roomStatusIEnumerable;
        private IEnumerable<char> _roomTypeIdEnumerable;

        public Func<IEnumerable<char>> PassParameterToDialogFunc;
        public Action<Room> AddRoomTypeAction;
        public Room RoomBeingAdded { get; set; } = new Room();

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
                OnPropertyChanged(nameof(RoomTypeIdEnumerable));
            }
        }

        public AddRoomDialog()
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

        private void AddRoomDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            RoomTypeIdEnumerable = PassParameterToDialogFunc();
            CbbRoomType.ItemsSource = RoomTypeIdEnumerable;

        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            if (NumUpDownRoomId.Value == null || NumUpDownCapacity.Value == null)
            {
                MessageBox.Show("Vui lòng điền thông tin mã phòng và sức chứa");
                return;
            }
            RoomBeingAdded.RoomTypeId = Convert.ToChar(CbbRoomType.SelectedItem);
            try
            {
                AddRoomTypeAction(RoomBeingAdded);

                Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Mã phòng đã có sẵn, vui lòng thử mã khác");
            }
        }
    }
}
