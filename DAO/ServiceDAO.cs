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

        public bool AddNewService(Service service)
        {
            var query =
                "INSERT INTO dbo.SERVICE (Name, Price, SvTypeId, Unit) " +
                $"VALUES(N'{service.Name}', {service.Price}, {service.SvTypeId}, N'{service.Unit}')";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DelService(int svId)
        {
            var query = $"DELETE FROM dbo.SERVICE WHERE ServId = {svId}";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateService(Service service)
        {
            var query = $"UPDATE dbo.SERVICE " +
                        $"SET Name = '{service.Name}', Price = {service.Price}, Unit = '{service.Unit}', SvTypeId = {service.SvTypeId}" +
                        $"WHERE ServId = {service.ServId}";
            var result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}