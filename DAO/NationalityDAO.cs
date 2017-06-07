using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class NationalityDAO
    {
        private static NationalityDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private NationalityDAO() { }
        public static NationalityDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new NationalityDAO();
                }
                return _instance;
            }
        }

        public IEnumerable<Nationality> GetAllNationalities()
        {
            _query = "SELECT * FROM dbo.TABLE_NATIONALITY ORDER BY NatId";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                yield return new Nationality(row);
            }
        }

        public bool AddNewNat(string natName)
        {
            _query = $"INSERT INTO dbo.TABLE_NATIONALITY ( NatName ) VALUES(N'{natName}')";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
