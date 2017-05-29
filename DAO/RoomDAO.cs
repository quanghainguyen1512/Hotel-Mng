using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return from DataRow row in data.Rows
                select new Room(row);
        }
    }
}
