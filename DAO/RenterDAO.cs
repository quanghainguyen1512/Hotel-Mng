using System.Collections.Generic;
using System.Data;
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
            _query = "SELECT * FROM dbo.RENTER JOIN dbo.TABLE_NATIONALITY ON TABLE_NATIONALITY.NatId = RENTER.NatId";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                yield return new Renter(row);
            }
        }

        public bool AddRenter(Renter renter)
        {
            var gender = renter.Gender.Equals(true) ? 1 : 0;
            _query = $"INSERT INTO dbo.RENTER (Name, Gender, PhoneNum, NatId, IdentityNum, Address) " +
                     $"VALUES(N'{renter.Name}', {gender}, '{renter.PhoneNum}', {renter.Nationality.NatId}, '{renter.IdentityNum}', N'{renter.Address}')";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateRenter(Renter renter)
        {
            _query =
                "UPDATE dbo.RENTER " +
                $"SET Name = N'{renter.Name}', Gender = '{renter.Gender}', PhoneNum = '{renter.PhoneNum}', IdentityNum='{renter.IdentityNum}', Address = N'{renter.Address}'" +
                $"WHERE RenterId = '{renter.RenterId}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool DelRenter(string renterId)
        {
            _query = $"DELETE FROM dbo.RENTER WHERE RenterId = '{renterId}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public static string GetRenterId(string identityNum)
        {
            var q = $"SELECT RenterId FROM dbo.RENTER WHERE IdentityNum = {identityNum}";
            return DataProvider.Instance.ExecuteScalar(q).ToString();
        }
    }
}