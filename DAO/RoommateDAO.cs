using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAO
{
    public class RoommateDAO
    {
        private static RoommateDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
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

        public IEnumerable<Roommate> GetRoommates(int formId)
        {
            _query = "EXEC dbo.USP_GetAllRoommatesByFormId @formid";
            var data = DataProvider.Instance.ExecuteQueries(_query, new object[] {formId});
            foreach (DataRow row in data.Rows)
            {
                yield return new Roommate(row);
            }
        }

        public bool AddRoommate(Roommate roommate, int formid)
        {
            _query =
                $"INSERT INTO dbo.ROOMMATE VALUES (N'{roommate.Name}', '{roommate.IdentityNum}', {roommate.Nationality.NatId}, {formid})";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool DelRoomate(string name, int formid)
        {
            _query = $"DELETE FROM dbo.ROOMMATE WHERE Name = N'{name}' AND FormId = {formid}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
