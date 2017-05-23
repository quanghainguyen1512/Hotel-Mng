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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public ObservableCollection<Student> Students { get; set; }
        public Page1()
        {
            InitializeComponent();
            //Load();
        }
        //void Load()
        //{
        //    var query = "SELECT * FROM STUDENT";
        //    var data = DataProvider.Instance.ExecuteQueries(query);
        //    foreach (System.Data.DataRow row in data.Rows)
        //    {
        //        Students.Add(new Student(row));
        //    }
        //}

    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }

        //public Student(System.Data.DataRow row)
        //{
        //    Id = (int) row["Id"];
        //    Name = row["Name"].ToString();
        //    Class = (int) row["ClassId"];
        //}
    }
}
