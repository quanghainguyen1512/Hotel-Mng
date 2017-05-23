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

        public bool UpdateRoomTypes(IEnumerable<RoomType> roomTypes)
        {

            return false;
        }
    }
}
