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

        public ObservableCollection<ServiceType> GetAllServiceTypes()
        {
            var result = new ObservableCollection<ServiceType>();
            var query = "SELECT * FROM dbo.SERVICE_TYPE";
            var data = DataProvider.Instance.ExecuteQueries(query);
            foreach (System.Data.DataRow row in data.Rows)
            {
                result.Add(new ServiceType(row));
            }
            return result;
        }

        public bool AddNewType(string typeName)
        {
            var query = $"INSERT INTO dbo.SERVICE_TYPE ( SvTypeName ) VALUES  ( N'{typeName}')";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DelType(int id)
        {
            var result = DataProvider.Instance.ExecuteNonQuery(
                $"UPDATE dbo.SERVICE SET SvTypeId = 4 WHERE SvTypeId = {id}");
            if (result > 0)
            {
                var query = $"DELETE FROM dbo.SERVICE_TYPE WHERE SvTypeId = {id}";
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            return result > 0;
        }
    }
}
