using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomType
    {
        #region Properties

        public char RoomTypeId { get; set; }
        public int PriceByDay { get; set; }
        public int Price1StHour { get; set; }
        public int PricePerHour { get; set; }
        public string Note { get; set; }

        #endregion

        public RoomType(System.Data.DataRow row)
        {
            RoomTypeId = (char) row["RoomTypeId"];
            PriceByDay = (int) row["Price"];
            Price1StHour = (int) row["Price1stHour"];
            PricePerHour = (int) row["PricePerHour"];
            Note = row["Note"].ToString();
        }
    }
}
