using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DAO;
using DTO;

namespace HotelMng.SubWindows
{
    /// <summary>
    /// Interaction logic for AddFormToBillDialog.xaml
    /// </summary>
    public partial class AddFormToBillDialog
    {
        public Action<RegForm> AddFomrAction { get; set; }
        public ObservableCollection<RegForm> Forms { get; set; }
        public AddFormToBillDialog()
        {
            InitializeComponent();
            Forms = new ObservableCollection<RegForm>(RegFormDAO.Instance.GetUsingForms());
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedForm = DataGrid.SelectedItem as RegForm;
            AddFomrAction(selectedForm);
            Forms.Remove(selectedForm);
        }
    }
}
