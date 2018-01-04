using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAO
{
    public class DataProvider
    {
        private static DataProvider _instance;
        private static readonly object Padlock = new object();
        public string ServerName { private get; set; }

        private DataProvider() { }

        private string _connString = ConfigurationManager.ConnectionStrings["HotelMng.Properties.Settings.HotelManagementConnectionString"].ConnectionString;
        
        public static DataProvider Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new DataProvider();
                }
                return _instance;
            }
        }
        #region Execute Queries
        /// <summary>
        ///     Execute normal queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteQueries(string query, object[] parameters = null)
        {
            _connString = _connString.Replace("{server_name}", ServerName);
            var dataTable = new DataTable();

            using (var sqlConn = new SqlConnection(_connString))
            {
                sqlConn.Open();

                var command = new SqlCommand(query, sqlConn);

                if (parameters != null)
                {
                    var param = query.Replace(',', ' ').Split(' ');
                    var i = 0;
                    foreach (var item in param)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }

                var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                sqlConn.Close();
            }
            return dataTable;
        }

        #endregion

        #region Execute Non-queries
        /// <summary>
        /// Returns number of rows affected (for Insert, Update, Delete).
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, object[] parameters = null)
        {
            _connString = _connString.Replace("{server_name}", ServerName);
            int count;
            using (var sqlConnection = new SqlConnection(_connString))
            {
                sqlConnection.Open();
                var command = new SqlCommand(query, sqlConnection);

                if (parameters != null)
                {
                    var param = query.Replace(',', ' ').Split(' ');
                    var i = 0;
                    foreach (var item in param)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                count = command.ExecuteNonQuery();

                sqlConnection.Close();
            }
            return count;
        }
        #endregion

        #region Execute Scalar
        /// <summary>
        /// Executes the query and returns the first column of the first row in the result
        /// set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string query, object[] parameters = null)
        {
            _connString = _connString.Replace("{server_name}", ServerName);
            object data;
            using (var sqlConnection = new SqlConnection(_connString))
            {
                sqlConnection.Open();
                var command = new SqlCommand(query, sqlConnection);

                if (parameters != null)
                {
                    var param = query.Replace(',', ' ').Split(' ');
                    var i = 0;
                    foreach (var item in param)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();

                sqlConnection.Close();
            }
            return data;
        }

        #endregion

    }
}
