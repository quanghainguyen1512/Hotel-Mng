using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTO;

namespace DAO
{
    public class RenterDAO
    {
        private static RenterDAO _instance;
        private static readonly object Padlock;
        private RenterDAO() { }
        public static RenterDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RenterDAO();
                }
                return _instance;
            }
        }

        public List<Renter> GetAllRenters()
        {
            var query = "SELECT * FROM RENTER";
            var data = DataProvider.Instance.ExecuteQueries(query);
            return (from DataRow row in data.Rows
                    select new Renter(row)).ToList();
        }
    }
}