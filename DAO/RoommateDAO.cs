using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class RoommateDAO
    {
        private static RoommateDAO _instance;
        private static readonly object Padlock = new object();
        private RoommateDAO() { }
        public static RoommateDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RoommateDAO();
                }
                return _instance;
            }
        }

        public List<Roommate> GetRoommates(string renterId)
        {
            var query = "EXEC dbo.USP_GetAllRoommatesByRenterId";
            var data = DataProvider.Instance.ExecuteQueries(query, new object[] {renterId});
            return (from System.Data.DataRow row in data.Rows
                select new Roommate(row)).ToList();
        }

    }
}
