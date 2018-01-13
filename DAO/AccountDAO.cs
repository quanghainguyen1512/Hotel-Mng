using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<bool> LogIn(string username, string password)
        {
            
            _query = "EXEC USP_LogIn @username, @password";
            var result = await Task.Run(() => DataProvider.Instance.ExecuteQueries(_query, new object[] { username, password }));
            //var result = DataProvider.Instance.ExecuteQueries(_query, new object[] {username, password});
            if (!(result.Rows.Count == 1))
                return false;
            IsAdmin = result.Rows[0]["IsAdmin"].Equals(true);
            return true;
            
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
