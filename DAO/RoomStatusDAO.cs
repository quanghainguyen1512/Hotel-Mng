using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class RoomStatusDAO
    {
        private static RoomStatusDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private RoomStatusDAO() { }
        public static RoomStatusDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RoomStatusDAO();
                }
                return _instance;
            }
        }

        public IEnumerable<RoomStatus> GetAllRoomStatus()
        {
            _query = "EXEC USP_GetRoomStatusInfo";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                yield return new RoomStatus(row);
            }
        }
    }
}
