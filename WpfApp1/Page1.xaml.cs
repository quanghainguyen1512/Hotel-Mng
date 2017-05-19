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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public List<Student> Students { get; set; }
        public Page1()
        {
            InitializeComponent();
            Students = new List<Student>()
            {
                new Student(){Id = 1, Name = "aaa"},
                new Student(){Id = 2, Name = "bbb"},
                new Student(){Id = 3, Name = "ccc"},
                new Student(){Id = 4, Name = "ddd"},
            };
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
