using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        #region Fields

        private ObservableCollection<Room> _rooms;
        private CollectionView _view;
        private IEnumerable<RoomStatus> _allRoomStatus;

        #endregion

        #region Properties

        public IEnumerable<RoomStatus> AllRoomStatus
        {
            get => _allRoomStatus;
            set
            {
                _allRoomStatus = value; 
                OnPropertyChanged(nameof(AllRoomStatus));
            }
        }

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
            LoadData();
            LoadRoomStatus(); 
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

        private void LoadData()
        {
            Rooms = new ObservableCollection<Room>(RoomDAO.Instance.GetAllRooms());
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Rooms);
            _view.GroupDescriptions?.Add(new PropertyGroupDescription("RoomTypeId"));

        }

        private void LoadRoomStatus()
        {
            AllRoomStatus = RoomStatusDAO.Instance.GetAllRoomStatus();
        }

        private void ButtonExit_OnCick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.No)
                return;
            Close();
        }

        private void Tile_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenu cm;
            var tile = sender as Tile;
            if (tile is null) return;

            switch (((Room)tile.DataContext).RoomStatus.StatusId)
            {
                case 0:
                    cm = RoomContainer.FindResource("ContextMenuForAvailableRoom") as ContextMenu;
                    break;
                case 1:
                    cm = RoomContainer.FindResource("ContextMenuForRentedRoom") as ContextMenu;
                    break;
                default:
                    cm = RoomContainer.FindResource("DefaultContextMenu") as ContextMenu;
                    break;
            }

            if (cm is null) return;

            cm.PlacementTarget = tile;
            cm.IsOpen = true;
        }
        #endregion

        #region About Tab

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #endregion

        #region Report Tab

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CbbMonth.SelectedIndex < 0 || CbbYear.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn tháng và năm");
                return;
            }

            var query = "EXEC dbo.USP_GetDataForReporting @month, @year";
            var data = DataProvider.Instance.ExecuteQueries(query, new object[] { (int)CbbMonth.SelectedValue, (int) CbbYear.SelectedValue});

            var ds = new ReportDataSource("DataSet1", data);
            Report.LocalReport.DataSources.Add(ds);
            Report.LocalReport.ReportEmbeddedResource = "HotelMng.Report1.rdlc";
            Report.RefreshReport();
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

            var itemBeingEdited = Rooms.First(r => r.RoomId == ((Room)menuItem.DataContext).RoomId);
            var oldStatus = itemBeingEdited.RoomStatus.StatusId;
            var dialog = new EditRoomDialog
            {
                PassParameterToDialogFunc =
                    () => new Tuple<Room, IEnumerable<RoomStatus>, IEnumerable<char>>(itemBeingEdited, AllRoomStatus, Rooms.Select(r => r.RoomTypeId).Distinct()),
                UpdateRoomTypeAction = room =>
                {
                    if (RoomDAO.Instance.UpdateRoom(room))
                    {
                        if (oldStatus != itemBeingEdited.RoomStatus.StatusId)
                            LoadRoomStatus();
                    }
                }
            };
            dialog.ShowDialog();
        }
        private void MenuItemRegRoom_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;

            var roomParam = (Room)menuItem.DataContext;

            var dialog = new RegistrationForm
            {
                PassParameterFunc = () => roomParam.RoomId,
                UpdateRegFormAction = form =>
                {
                    var temp = Rooms.First(r => r.RoomId == roomParam.RoomId);
                    temp.RoomStatus = AllRoomStatus.FirstOrDefault(s => s.StatusId == 1);
                    if (RoomDAO.Instance.UpdateRoom(temp) && RenterDAO.Instance.AddRenter(form.Renter))
                    {
                        form.Renter.RenterId = RenterDAO.GetRenterId(form.Renter.IdentityNum);
                        RegFormDAO.Instance.AddRegForm(form);
                    }
                }
            };
            dialog.ShowDialog();
        }
        private void MenuItemAddService_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;
            var roomParam = (Room)menuItem.DataContext;
            var dialog = new UsingServiceWindow
            {
                PassParameterFunc = () => roomParam.RoomId
            };
            dialog.ShowDialog();
        }
        private void MenuItemRentersInRoom_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;
            var roomParam = (Room)menuItem.DataContext;
            var dialog = new RentersInRoomDialog
            {
                PassParameterFunc = () => roomParam.RoomId
            };
            dialog.ShowDialog();
        }

        private void MenuItemPayBill_OnClick(object sender, RoutedEventArgs e)
        {
        }
        private void MenuItemCheckOut_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null) return;
            var roomParam = (Room)menuItem.DataContext;

            var dialog = new CheckOutDialog()
            {
                PassParameterFunc = () => roomParam.RoomId,
                CheckOutAction = form =>
                {
                    if (!RegFormDAO.Instance.UpdateForm(form)) return;

                    if (RoomDAO.Instance.UpdateRoomAfterPay(roomParam.RoomId))
                        LoadData();

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

        private void ButtonChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
