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
        public string RoomId { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public char RoomTypeId { get; set; }
        #endregion

        public Room(System.Data.DataRow row)
        {
            RoomId      = row["RoomId"].ToString();
            Description = row["Description"].ToString();
            StatusId    = (int) row["StatusId"];
            RoomTypeId  = (char) row["RoomTypeId"];
        }       
    }
}
