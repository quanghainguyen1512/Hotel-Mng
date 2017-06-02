using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DAO;
using DTO;
using DTO.Annotations;
using HotelMng.SubWindows;
using MahApps.Metro.Controls;
using Microsoft.Reporting.WinForms;
using HamburgerMenuItem = HamburgerMenu.HamburgerMenuItem;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
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

        public MainWindow()
        {
            InitializeComponent();
            Rooms = new ObservableCollection<Room>(RoomDAO.Instance.GetAllRooms());

            AllRoomStatus = RoomStatusDAO.Instance.GetAllRoomStatus();

            _view = (CollectionView)CollectionViewSource.GetDefaultView(Rooms);
            _view.GroupDescriptions.Add(new PropertyGroupDescription("RoomTypeId"));

            //for (int i = 1; i <= 12; i++)
            //{
            //    ComboBoxItem item = new ComboBoxItem();
            //    item.Content = i;
            //    cbxMonth.Items.Add(item);
            //}

            //for (int i = 2000; i <= DateTime.UtcNow.Year; i++)
            //{
            //    ComboBoxItem item = new ComboBoxItem();
            //    item.Content = i;
            //    cbxYear.Items.Add(item);
            //}
        }

        private void HamburgerMenuItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var hbgMenuItem = sender as HamburgerMenuItem;
            if (hbgMenuItem is null)
                throw new Exception("Unexpected Error..");
            var targetView = hbgMenuItem.Tag.ToString();
            Frame.Source = new Uri(targetView, UriKind.Relative);
        }

        private void TimePicker_OnSelectedDateChanged(object sender, TimePickerBaseSelectionChangedEventArgs<DateTime?> e)
        {

        }

        private void Tile_OnClick(object sender, RoutedEventArgs e)
        {
            var cm = this.FindResource("TileCMenu") as ContextMenu;
            if (cm is null) return;

            cm.PlacementTarget = sender as Tile;
            cm.IsOpen = true;
        }

        private void MenuItemDelRoom_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Chắc chắn xóa?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                var menuItem = sender as MenuItem;
                if (menuItem is null) return;

                var room = (Room)menuItem.DataContext;

                if (RoomDAO.Instance.DeleteRoom(room.RoomId))
                    Rooms.Remove(room);
            }
        }

        private void MenuItemEditRoom_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;

            var itemBeingEdited = Rooms.FirstOrDefault(r => r.RoomId == ((Room)menuItem.DataContext).RoomId);

            var dialog = new EditRoomDialog
            {
                PassParameterToDialogFunc = () => itemBeingEdited,
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var query = "EXEC dbo.USP_GetDataForReporting @month, @year";
            var data = DataProvider.Instance.ExecuteQueries(query, new object[] { (int.Parse(cbxMonth.Text)), (int.Parse(cbxYear.Text)) });
            var ds = new ReportDataSource("DataSet1", data);
            _repo.LocalReport.DataSources.Add(ds);
            _repo.LocalReport.ReportEmbeddedResource = "HotelMng.Report1.rdlc";
            _repo.RefreshReport();

        }
    }
}
