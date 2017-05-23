using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using DTO;

namespace DAO
{
    public class ServiceDAO
    {
        private static ServiceDAO _instance;
        private static readonly object Padlock = new object();
        private ServiceDAO() { }
        public static ServiceDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new ServiceDAO();
                }
                return _instance;
            }
        }

        public ObservableCollection<Service> GetAllServices()
        {
            var result = new ObservableCollection<Service>();
            var query = "EXEC dbo.USP_GetAllServicesInfo";
            var data = DataProvider.Instance.ExecuteQueries(query);
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Service(row));
            }
            return result;
        }

        public bool AddNewService(string svName, int price, int svTypeId, string unit)
        {
            var query =
                "INSERT INTO dbo.SERVICE (Name, Price, SvTypeId, Unit) " +
                $"VALUES(N'{svName}', {price}, {svTypeId}, N'{unit}')";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DelService(int svId)
        {
            var query = $"DELETE FROM dbo.SERVICE WHERE ServId = {svId}";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}