using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTO;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Renter> GetAllRenters()
        {
            _query = "SELECT * FROM dbo.RENTER";
            var result = new ObservableCollection<Renter>();
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow item in data.Rows)
            {
                result.Add(new Renter(item));
            }
            return result;
        }

        public bool DelRenter(string Id)
        {
            _query = $"DELETE FROM dbo.RENTER WHERE RenterId = {Id}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
        //public bool AddRenter(string name, bool gender, string phoneNum, int typeId, string identityNum, string address)
        //{
        //    _query = $"INSERT RENTER VALUES (N'{name}, {gender}, {phoneNum}, {typeId}, {identityNum}, {address})";
        //    var result = DataProvider.Instance.ExecuteNonQuery(_query);
        //    return result > 0;
        //}
        public bool AddNewRenter(Renter renter)
        {
            _query =
                "INSERT INTO dbo.RENTER (RenterId, Name, Gender, PhoneNum, IdentityNum, Address) " +
                $"VALUES(N'{renter.RenterId}', N'{renter.Name}', {renter.Gender}, {renter.PhoneNum},N'{renter.IdentityNum}', N'{renter.Address}')";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateRenter(Renter renter)
        {
            _query = $"UPDATE dbo.RENTER " +
                        $"SET RenterId = '{renter.RenterId}', Name = {renter.Name}, Gerder = '{renter.Gender}', PhoneNum = {renter.PhoneNum}, IdentityNum={renter.IdentityNum}, Address={renter.Address}" +
                        $"WHERE RenterId = {renter.RenterId}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
        //public bool AddRenter(Renter renter)
        //{
        //    _query = $"INSERT RENTER VALUES (N'{renter.Name}, {renter.Gender}, {renter.PhoneNum}, {renter.RenterId}, {renter.IdentityNum}, {renter.Address})";
        //    var result = DataProvider.Instance.ExecuteNonQuery(_query);
        //    return result > 0;
        //}
    }
}