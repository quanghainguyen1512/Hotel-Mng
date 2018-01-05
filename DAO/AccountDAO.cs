using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class AccountDAO
    {
        public bool IsAdmin { get; private set; } = false;
        private static AccountDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private AccountDAO() { }
        public static AccountDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new AccountDAO();
                }
                return _instance;
            }
        }

        public bool LogIn(string username, string password)
        {
            
             _query = "EXEC USP_LogIn @username, @password";
             var result = DataProvider.Instance.ExecuteQueries(_query, new object[] {username, password});
            IsAdmin = result.Rows[0]["IsAdmin"].Equals(true);
                return result.Rows.Count == 1;
            
        }

        public bool ChangePassword(string username, string newPassword)
        {
            _query =
                $"UPDATE dbo.ACCOUNT SET Password = '{newPassword}' WHERE Username = '{username}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
