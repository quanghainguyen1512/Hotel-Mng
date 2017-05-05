using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTO;

namespace DAO
{
    public class ServicesDAO
    {
        private static ServicesDAO _instance;
        private static readonly object Padlock;
        private ServicesDAO() { }
        public static ServicesDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new ServicesDAO();
                }
                return _instance;
            }
        }

        public List<Service> GetAllServiceses()
        {
            var query = "SELECT * FROM SERVICES";
            var data = DataProvider.Instance.ExecuteQueries(query);
            return (from DataRow row in data.Rows
                    select new Service(row)).ToList();
        }

    }
}