using System;

namespace DTO
{
    public class Room : Base
    {
        #region Properties

        private string _description;
        private char _roomTypeId;
        private int _capacity;
        private RoomStatus _roomStatus;

        public int RoomId { get; set; }

        public RoomStatus RoomStatus
        {
            get => _roomStatus;
            set
            {
                _roomStatus = value;
                OnPropertyChanged(nameof(RoomStatus));
            }
        }

        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value; 
                OnPropertyChanged(nameof(Capacity));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value; 
                OnPropertyChanged(nameof(Description));
            }
        }


        public char RoomTypeId
        {
            get => _roomTypeId;
            set
            {
                _roomTypeId = value;
                OnPropertyChanged(nameof(RoomTypeId));
            }
        }

        #endregion

        public Room(System.Data.DataRow row)
        {
            RoomId = (int) row["RoomId"];
            Description = row["Description"].ToString();
            RoomTypeId = Convert.ToChar(row["RoomTypeId"]);
            Capacity = Convert.ToInt32(row["Capacity"]);
            RoomStatus = new RoomStatus()
            {
                StatusId = (int) row["StatusId"],
                StatusName = row["StatusName"].ToString()
            };
        }

    }
}
