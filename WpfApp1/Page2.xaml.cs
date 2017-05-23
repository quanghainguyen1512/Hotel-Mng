using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public ObservableCollection<Student> Students { get; set; }
        public CollectionView CollectionView { get; set; }
        public List<int> Classes { get; set; }
        private DataRowView _rowBeingEdited;
        private Student _newStudent;
        public Page2()
        {
            InitializeComponent();

            var data = DataProvider.Instance.ExecuteQueries("SELECT * FROM STUDENT");

            Students = new ObservableCollection<Student>();
            foreach (System.Data.DataRow row in data.Rows)
            {
                Students.Add(new Student()
                {
                    Id = (int) row["Id"],
                    Name = row["Name"].ToString(),
                    Class = (int) row["ClassId"]
                });
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            Students.Remove((Student) btn?.DataContext);
        }

        private void Grid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //_rowBeingEdited = e.Row.Item as DataRowView;
            MessageBox.Show("Cell editing");
        }

        private void Grid_OnCurrentCellChanged(object sender, EventArgs e)
        {
            //var dataGridCellEditEndingEventArgs = e as DataGridCellEditEndingEventArgs;
            //if (dataGridCellEditEndingEventArgs != null)
            //    _newStudent = dataGridCellEditEndingEventArgs.Row.Item as Student;
            ////MessageBox.Show(_newStudent?.Id.ToString());
            MessageBox.Show("Current cell changed");
            _rowBeingEdited?.EndEdit();
        }
    }
}
