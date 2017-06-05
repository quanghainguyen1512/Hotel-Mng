using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAO
{
    public class UseServiceDAO
    {
        private static UseServiceDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private UseServiceDAO() { }
        public static UseServiceDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new UseServiceDAO();
                }
                return _instance;
            }
        }

        public IEnumerable<UseService> GetServicesBeingUsed(int formid)
        {
            _query = "EXEC USP_GetServicesBeingUsed @formid";
            var data = DataProvider.Instance.ExecuteQueries(_query, new object[]{ formid });
            foreach (DataRow row in data.Rows)
            {
                yield return new UseService(row);
            }
        }

        public bool DeleteItem(int formId, int servId)
        {
            _query = $"DELETE FROM dbo.USE_SERVICES WHERE ServId = {servId} AND FormId = {formId}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool AddItem(UseService useService)
        {
            _query =
                $"INSERT INTO dbo.USE_SERVICES VALUES ({useService.ServId}, {useService.FormId}, GETDATE(), {useService.Quantity})";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
