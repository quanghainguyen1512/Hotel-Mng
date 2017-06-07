using System.Data;

namespace DTO
{
    public class RoomStatus : Base
    {
        #region Fields

        private int _itemCount;

        #endregion

        #region Properties

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

        #endregion

        public RoomStatus()
        {
            
        }
        public RoomStatus(DataRow row)
        {
            StatusId = (int) row["StatusId"];
            StatusName = row["StatusName"].ToString();
            ItemCount = (int) row["ItemCount"];
        }

        public override bool Equals(object obj)
        {
            var r = obj as RoomStatus;
            if (r is null) return false;

            return StatusId == r.StatusId;
        }
    }
}
