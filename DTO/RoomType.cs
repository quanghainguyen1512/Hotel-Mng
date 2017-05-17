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
        public int Price { get; set; }
        public string Note { get; set; }

        #endregion

        public RoomType(System.Data.DataRow row)
        {
            RoomTypeId = (char) row["RoomTypeId"];
            Price = (int) row["Price"];
            Note = row["Note"].ToString();
        }
    }
}
