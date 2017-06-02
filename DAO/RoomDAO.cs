using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAO
{
    public class RoomDAO
    {
        private static RoomDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private RoomDAO() { }
        public static RoomDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RoomDAO();
                }
                return  _instance;
            }
        }

        public IEnumerable<Room> GetAllRooms()
        {
            _query = "EXEC USP_GetAllRoomInfo";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                yield return new Room(row);
            }
        }

        public bool AddNewRoom(Room room)
        {
            _query = $"INSERT INTO dbo.ROOM VALUES({room.RoomId}, {room.RoomTypeId}, {room.Description}, 1)";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool DeleteRoom(int roomId)
        {
            _query = $"DELETE FROM dbo.ROOM WHERE RoomId = {roomId}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateRoom(Room room)
        {
            _query =
                $"UPDATE dbo.ROOM SET RoomTypeId = '{room.RoomTypeId}', Description = N'{room.Description}', StatusId = {room.RoomStatus.StatusId} " +
                $"WHERE RoomId = {room.RoomId}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
