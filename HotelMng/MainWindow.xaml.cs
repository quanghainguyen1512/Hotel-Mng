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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using DAO;
using DTO;
using MahApps.Metro.Controls;
using HamburgerMenuItem = HamburgerMenu.HamburgerMenuItem;

namespace HotelMng
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public IEnumerable<Room> Rooms { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Rooms = RoomDAO.Instance.GetAllRooms();
            var view = (CollectionView) CollectionViewSource.GetDefaultView(Rooms);
            view.GroupDescriptions.Add(new PropertyGroupDescription("RoomTypeId"));
        }

        private void HamburgerMenuItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var hbgMenuItem = sender as HamburgerMenuItem;
            if (hbgMenuItem == null)
                throw new Exception("Unexpected Error..");
            var targetView = hbgMenuItem.Tag.ToString();
            Frame.Source = new Uri(targetView, UriKind.Relative);
        }

        private void TimePicker_OnSelectedDateChanged(object sender, TimePickerBaseSelectionChangedEventArgs<DateTime?> e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connStr = @"Data Source=DESKTOP-TKMUG3F\SIR;Initial Catalog=HotelManagement;Integrated Security=True";
            SqlConnection cn = new SqlConnection(connStr);

            string query = "EXEC dbo.USP_GetDataForReporting @year";

            var dt = DataProvider.Instance.ExecuteQueries(query, new object[] { 8 });

            ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            _repor.LocalReport.DataSources.Add(ds);
            _repor.LocalReport.ReportEmbeddedResource = "HotelMng.Report1.rdlc";
            _repor.RefreshReport();
        }
    }
}
