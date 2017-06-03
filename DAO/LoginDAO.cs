using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DTO;

namespace DAO
{
    public class LoginDAO
    {
        private static LoginDAO _instance;


        public static LoginDAO Instance
        {
            get { if (_instance == null) _instance = new LoginDAO(); return _instance; }
            private set { _instance = value; }
        }

        private LoginDAO() { }

        public bool FLogin(string username,string password)
        {
            string query = "SELECT * FROM dbo.ACCOUNT WHERE UserName = N'" + username + "'AND Password = N'" + password + "' ";
            DataTable resuft = DataProvider.Instance.ExecuteQueries(query);

            return resuft.Rows.Count > 0;
        }
    }
}
