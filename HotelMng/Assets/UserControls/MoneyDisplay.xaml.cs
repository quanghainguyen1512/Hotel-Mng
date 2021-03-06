﻿using System;
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
    /// Interaction logic for MoneyDisplay.xaml
    /// </summary>
    public partial class MoneyDisplay : UserControl
    {
        public MoneyDisplay()
        {
            InitializeComponent();
        }


        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(MoneyDisplay), new PropertyMetadata(0));



        public string CurrencyUnit
        {
            get => (string)GetValue(CurrencyUnitProperty);
            set => SetValue(CurrencyUnitProperty, value);
        }

        // Using a DependencyProperty as the backing store for CurrencyUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrencyUnitProperty =
            DependencyProperty.Register("CurrencyUnit", typeof(string), typeof(MoneyDisplay), new PropertyMetadata("VND"));



        public SolidColorBrush ItemForeground
        {
            get => (SolidColorBrush)GetValue(ItemForegroundProperty);
            set => SetValue(ItemForegroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.Register("ItemForeground", typeof(SolidColorBrush), typeof(MoneyDisplay), new PropertyMetadata());


    }
}
