using System;
using System.Globalization;
using System.Windows.Data;

namespace HotelMng.Converter
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gender = "";
            if(value != null)
                gender = value.Equals(true) ? "Nam" : "Nữ";
            return gender;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
