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
using DAO;
using DTO;

namespace HotelMng.Pages
{
    /// <summary>
    /// Interaction logic for AdjustFeePage.xaml
    /// </summary>
    public partial class AdjustFeePage : Page
    {
        public Fee Fee { get; set; }
        
        public AdjustFeePage()
        {
            InitializeComponent();
            Fee = FeeDAO.Instance.GetAllFee();
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            if (FeeDAO.Instance.UpdateFee(Fee))
                MessageBox.Show("Cập nhật thành công");
        }
    }
}
