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

namespace HotelMng.Assets.UserControls
{
    /// <summary>
    /// Interaction logic for RoomStatusItem.xaml
    /// </summary>
    public partial class RoomStatusItem : UserControl
    {
        public RoomStatusItem()
        {
            InitializeComponent();
        }


        public Brush StatusBrush
        {
            get => (Brush)GetValue(StatusBrushProperty);
            set => SetValue(StatusBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for StatusBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusBrushProperty =
            DependencyProperty.Register("StatusBrush", typeof(Brush), typeof(RoomStatusItem), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(100, 150, 112))));

        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(int), typeof(RoomStatusItem), new PropertyMetadata(0));

        public string StatusText
        {
            get => (string)GetValue(StatusTextProperty);
            set => SetValue(StatusTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for StatusText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof(string), typeof(RoomStatusItem), new PropertyMetadata(""));

    }
}
