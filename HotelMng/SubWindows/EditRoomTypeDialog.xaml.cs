using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DTO;
using DTO.Annotations;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for EditRoomTypeDialog.xaml
    /// </summary>
    public partial class EditRoomTypeDialog : INotifyPropertyChanged
    {
        public Func<RoomType> PassParameterToDialogFunc;
        public Action<RoomType> UpdateRoomTypeAction;

        private RoomType _roomTypeBeingEdited;

        public RoomType RoomTypeBeingEdited
        {
            get => _roomTypeBeingEdited;
            set
            {
                _roomTypeBeingEdited = value;
                OnPropertyChanged(nameof(RoomTypeBeingEdited));
            }
        }

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
            UpdateRoomTypeAction(RoomTypeBeingEdited);
            this.Close();
        }

        private void EditServiceDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            RoomTypeBeingEdited = PassParameterToDialogFunc();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
