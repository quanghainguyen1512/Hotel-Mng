using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DAO;
using DTO;
using DTO.Annotations;
using HotelMng.SubWindows;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for RoomMngPage.xaml
    /// </summary>
    public partial class RoomMngPage : INotifyPropertyChanged
    {
        #region Fields

        private IEnumerable<RoomType> _roomTypes;

        #endregion

        #region Properties

        public RoomType RoomTypeBeingAdded { get; set; } = new RoomType();

        public IEnumerable<RoomType> RoomTypes
        {
            get => _roomTypes;
            set
            {
                _roomTypes = value; 
                OnPropertyChanged(nameof(RoomTypes));
            }
        }

        #endregion
        public RoomMngPage()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            RoomTypes = RoomTypeDAO.Instance.GetAllRoomTypes();
        }

        #region Event Handlers

        private void ButtonEditRoomType_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;

            var rowBeingEdited = RoomTypes.FirstOrDefault(r => r.RoomTypeId == ((RoomType)btn.DataContext).RoomTypeId);

            var dialog = new EditRoomTypeDialog()
            {
                UpdateRoomTypeAction = roomtype =>
                {
                    if (RoomTypeDAO.Instance.UpdateRoomTypes(rowBeingEdited)) 
                        LoadData();
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
            if (RoomTypeDAO.Instance.AddRoomTypes(RoomTypeBeingAdded))
            {
                LoadData();
            }
        }

        #endregion

        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
