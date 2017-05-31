using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using DAO;
using DTO;

namespace HotelMng.Converter
{
    public class RoomTypeConverter : IValueConverter
    {
        private IEnumerable<ServiceType> _data = ServiceTypeDAO.Instance.GetAllServiceTypes();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var st = value as ServiceType;
            if (st is null) return null;

            return st.SvTypeId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            return _data.Where(s => s.SvTypeId == id);
        }
    }
}
