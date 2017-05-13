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
using DTO;

namespace HotelMng.Assets.UserControls
{
    /// <summary>
    /// Interaction logic for RoomItem.xaml
    /// </summary>
    public partial class RoomItem : UserControl
    {
        public RoomItem()
        {
            InitializeComponent();
        }

        public Brush StatusColor
        {
            get => (Brush)GetValue(StatusColorProperty);
            set => SetValue(StatusColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for StatusColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register("StatusColor", typeof(Brush), typeof(RoomItem), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(100,150,112))));

        public string RoomNum
        {
            get => (string)GetValue(RoomNumProperty);
            set => SetValue(RoomNumProperty, value);
        }

        // Using a DependencyProperty as the backing store for RoomNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoomNumProperty =
            DependencyProperty.Register("RoomNum", typeof(string), typeof(RoomItem), new PropertyMetadata(""));



        public string GuessName
        {
            get => (string)GetValue(GuessNameProperty);
            set => SetValue(GuessNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for GuessName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GuessNameProperty =
            DependencyProperty.Register("GuessName", typeof(string), typeof(RoomItem), new PropertyMetadata(""));


    }
}
