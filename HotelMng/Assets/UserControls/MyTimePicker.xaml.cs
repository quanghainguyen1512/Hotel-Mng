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
    /// Interaction logic for MyTimePicker.xaml
    /// </summary>
    public partial class MyTimePicker : UserControl
    {
        public MyTimePicker()
        {
            InitializeComponent();
        }



        public int Hours
        {
            get => (int) GetValue(HoursProperty);
            set => SetValue(HoursProperty, value);
        }

        // Using a DependencyProperty as the backing store for Hours.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(MyTimePicker), new PropertyMetadata(DateTime.Now.Hour));



        public int Minutes
        {
            get => (int) GetValue(MinutesProperty);
            set => SetValue(MinutesProperty, value);
        }

        // Using a DependencyProperty as the backing store for Minutes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(MyTimePicker), new PropertyMetadata(DateTime.Now.Minute));


        public TimeSpan SelectedTime
        {
            get => ( TimeSpan)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(MyTimePicker), new PropertyMetadata());

        private void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
