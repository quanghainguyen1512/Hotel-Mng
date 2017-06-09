using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class ServiceTypeDAO
    {
        private static ServiceTypeDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private ServiceTypeDAO() { }
        public static ServiceTypeDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new ServiceTypeDAO();
                }
                return _instance;
            }
        }

        public IEnumerable<ServiceType> GetAllServiceTypes()
        {
            _query = "SELECT * FROM dbo.SERVICE_TYPE";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (System.Data.DataRow row in data.Rows)
            {
                yield return new ServiceType(row);
            }
        }

        public bool AddNewType(string typeName)
        {
            _query = $"INSERT INTO dbo.SERVICE_TYPE ( SvTypeName ) VALUES  ( N'{typeName}')";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool DelType(int id)
        {
            var result = DataProvider.Instance.ExecuteNonQuery(
                $"UPDATE dbo.SERVICE SET SvTypeId = 4 WHERE SvTypeId = {id}");
            if (result > 0)
            {
                _query = $"DELETE FROM dbo.SERVICE_TYPE WHERE SvTypeId = {id}";
                result = DataProvider.Instance.ExecuteNonQuery(_query);
            }
            return result > 0;
        }

        public int NewestServiceTypeId()
        {
            _query = "SELECT TOP 1 SvTypeId FROM dbo.SERVICE_TYPE ORDER BY SvTypeId DESC";
            var data = DataProvider.Instance.ExecuteScalar(_query);
            return (int)data;
        }

    }
}
