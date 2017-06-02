using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using DAO;
using DTO;
using DTO.Annotations;
using HotelMng.SubWindows;
using Microsoft.Reporting.WinForms;
using MahApps.Metro.Controls;
using HamburgerMenuItem = HamburgerMenu.HamburgerMenuItem;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region Properties

        private ObservableCollection<Room> _rooms;
        private readonly CollectionView _view;
        public IEnumerable<RoomStatus> AllRoomStatus { get; set; }
        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value; 
                OnPropertyChanged(nameof(Rooms));
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Rooms = new ObservableCollection<Room>(RoomDAO.Instance.GetAllRooms());

            AllRoomStatus = RoomStatusDAO.Instance.GetAllRoomStatus();

            _view = (CollectionView) CollectionViewSource.GetDefaultView(Rooms);
            _view.GroupDescriptions.Add(new PropertyGroupDescription("RoomTypeId"));
        }

        #region Mng Tab

        private void HamburgerMenuItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var hbgMenuItem = sender as HamburgerMenuItem;
            if (hbgMenuItem is null)
                throw new Exception("Unexpected Error..");
            var targetView = hbgMenuItem.Tag.ToString();
            Frame.Source = new Uri(targetView, UriKind.Relative);
        }
        

        #endregion

        #region Home Tab

        private void TimePicker_OnSelectedDateChanged(object sender, TimePickerBaseSelectionChangedEventArgs<DateTime?> e)
        {

        }

        private void Tile_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenu cm;
            var tile = sender as Tile;
            if (tile is null) return;

            switch (((Room)tile.DataContext).RoomStatus.StatusId)
            {
                case 1:
                    cm = this.FindResource("ContextMenuForAvailableRoom") as ContextMenu;
                    break;
                case 2:
                    cm = this.FindResource("ContextMenuForRentedRoom") as ContextMenu;
                    break;
                default:
                    cm = this.FindResource("DefaultContextMenu") as ContextMenu;
                    break;
            }

            if (cm is null) return;

            cm.PlacementTarget = tile;
            cm.IsOpen = true;
        }
        

        #endregion

        #region MenuItem Click event

        private void MenuItemDelRoom_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;
            var room = (Room)menuItem.DataContext;

            if (MessageBox.Show("Chắc chắn xóa?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                if (RoomDAO.Instance.DeleteRoom(room.RoomId))
                    Rooms.Remove(room);
            }
        }
        private void MenuItemEditRoom_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;

            var itemBeingEdited = Rooms.FirstOrDefault(r => r.RoomId == ((Room) menuItem.DataContext).RoomId);
            var dialog = new EditRoomDialog
            {
                PassParameterToDialogFunc =
                    () => new Tuple<Room, IEnumerable<RoomStatus>, IEnumerable<char>>(itemBeingEdited, AllRoomStatus, Rooms.Select(r => r.RoomTypeId).Distinct()),
                UpdateRoomTypeAction = room =>
                {
                    if (RoomDAO.Instance.UpdateRoom(room))
                    {
                        itemBeingEdited = room;
                    }
                    _view.Refresh();
                }
            };
            dialog.ShowDialog();
        }
        private void MenuItemRegRoom_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;
            var roomParam = (Room)menuItem.DataContext;
            var dialog = new RegistrationForm()
            {
                PassParameterFunc = () => roomParam,
                UpdateRegFormAction = form =>
                {
                    //RegFormDAO.Instance.AddRegForm(form);
                    var temp = Rooms.First(r => r.RoomId == roomParam.RoomId);
                    temp.RoomStatus = AllRoomStatus.FirstOrDefault(s => s.StatusId == 2);
                    RoomDAO.Instance.UpdateRoom(temp);
                }

            };
            dialog.ShowDialog();
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

        #region About Tab

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var query = "EXEC dbo.USP_GetDataForReporting @month, @year";
            var data = DataProvider.Instance.ExecuteQueries(query, new object[] { 8, 2017 });

            var ds = new ReportDataSource("DataSet1", data);
            _repo.LocalReport.DataSources.Add(ds);
            _repo.LocalReport.ReportEmbeddedResource = "HotelMng.Report1.rdlc";
            _repo.RefreshReport();

        }

    }
}
