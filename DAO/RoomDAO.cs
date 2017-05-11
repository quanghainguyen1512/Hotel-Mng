using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class RoomDAO
    {
        private static RoomDAO _instance;
        private static readonly object Padlock = new object();
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

        public List<Room> GetAllRoom()
        {
            var query = "SELECT * FROM ROOM";
            var data = DataProvider.Instance.ExecuteQueries(query);
            return (from DataRow row in data.Rows
                    select new Room(row)).ToList();
        }

    }
}
