using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAO
{
    public class DataProvider
    {
        private static DataProvider _instance;
        private static readonly object Padlock = new object();

        DataProvider() { }

        private readonly string _connString = "Data Source=DESKTOP-L9JNNSC;Initial Catalog=HotelManagement;Integrated Security=True";

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
            var count = 0;

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
