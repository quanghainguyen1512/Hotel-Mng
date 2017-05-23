using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class RoomTypeDAO
    {
        private static RoomTypeDAO _instance;
        private static readonly object Padlock = new object();
        private RoomTypeDAO() { }
        public static RoomTypeDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RoomTypeDAO();
                }
                return _instance;
            }
        }

        public List<RoomType> GetAllRoomTypes()
        {
            var query = "SELECT * FROM ROOM_TYPE";
            var data = DataProvider.Instance.ExecuteQueries(query);
            return (from DataRow row in data.Rows
                select new RoomType(row)).ToList();
        }

        public bool AddRoomTypes(char roomTypeId, int priceByDay, int price1StDay, int pricePerHour, string note = null)
        {
            var query =
                $"INSERT INTO dbo.ROOM_TYPE VALUES ({roomTypeId}, {priceByDay}, {price1StDay}, {pricePerHour}, N'{note}'";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateRoomTypes()
        {

            return false;
        }
    }
}
