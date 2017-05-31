using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomStatus : Base
    {
        private int _itemCount;
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public int ItemCount
        {
            get => _itemCount;
            set
            {
                _itemCount = value; 
                OnPropertyChanged(nameof(ItemCount));
            }
        }

        public RoomStatus()
        {
            
        }
        public RoomStatus(DataRow row)
        {
            StatusId = (int) row["StatusId"];
            StatusName = row["StatusName"].ToString();
            ItemCount = (int) row["ItemCount"];
        }
    }
}
