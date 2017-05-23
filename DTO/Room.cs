using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Room
    {
        #region Properties
        public int RoomId { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public char RoomTypeId { get; set; }
        #endregion

        public Room(System.Data.DataRow row)
        {
            RoomId      = (int) row["RoomId"];
            Description = row["Description"].ToString();
            StatusId    = (int) row["StatusId"];
            RoomTypeId = Convert.ToChar(row["RoomTypeId"]);
            Status = row["StatusName"].ToString();
        }

        public override string ToString()
        {
            if (StatusId != 0)
                return RoomId + $" ({Status})";
            return RoomId.ToString();
        }
    }
}
