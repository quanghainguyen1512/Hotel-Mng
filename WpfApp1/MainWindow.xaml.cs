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
using MahApps.Metro.Controls;
using HamburgerMenuItem = HamburgerMenu.HamburgerMenuItem;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //public List<Food> Foods { get; set; }
        //public ListCollectionView ListCollectionView { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //Foods = new List<Food>
            //{
            //    new Food {Name = "Food1", Type = 1},
            //    new Food {Name = "Food2", Type = 1},
            //    new Food {Name = "Food3", Type = 1},
            //    new Food {Name = "Food4", Type = 2},
            //    new Food {Name = "Food5", Type = 2},
            //    new Food {Name = "Food6", Type = 3},
            //    new Food {Name = "Food7", Type = 4}
            //};
            //ListCollectionView = new ListCollectionView(Foods);
            //ListCollectionView.GroupDescriptions?.Add(new PropertyGroupDescription("Type"));

        }

        private void ListBoxItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var targetView = ((HamburgerMenuItem)sender).Tag.ToString();
            TheFrame.Source = new Uri(targetView, UriKind.Relative);
        }
    }

    public class Food
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
