﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IEnumerable<Roommate> GetRoommates(string renterId)
        {
            _query = "EXEC dbo.USP_GetAllRoommatesByRenterId";
            var data = DataProvider.Instance.ExecuteQueries(_query, new object[] {renterId});
            foreach (DataRow row in data.Rows)
            {
                yield return new Roommate(row);
            }
        }

    }
}
