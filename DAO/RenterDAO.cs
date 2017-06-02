using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTO;

namespace DAO
{
    public class RenterDAO
    {
        private static RenterDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private RenterDAO() { }
        public static RenterDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RenterDAO();
                }
                return _instance;
            }
        }

        public IEnumerable<Renter> GetAllRenters()
        {
            _query = "SELECT * FROM RENTER";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                yield return new Renter(row);
            }
        }

        public bool AddRenter(string name, bool gender, string phoneNum, string renterId, string identityNum, string address)
        {
            _query = $"INSERT RENTER VALUES ({name}, {gender}, {phoneNum}, {renterId}, {identityNum}, {address})";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateRenter(Renter renter)
        {
            _query =
                "UPDATE dbo.RENTER " +
                $"SET RenterId = '{renter.RenterId}', Name = '{renter.Name}', Gender = '{renter.Gender}', PhoneNum = '{renter.PhoneNum}', IdentityNum='{renter.IdentityNum}', Address='{renter.Address}'" +
                $"WHERE RenterId = '{renter.RenterId}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool DelRenter(string rtId)
        {
            _query = $"DELETE FROM dbo.RENTER WHERE RenterId = {rtId}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}