using System;
using DTO;

namespace DAO
{
    public class RegFormDAO
    {
        private static RegFormDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private RegFormDAO() { }
        public static RegFormDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RegFormDAO();
                }
                return _instance;
            }
        }

        public RegForm GetFormById(int formid)
        {
            _query = $"SELECT * FROM dbo.REG_FORM WHERE FormId = {formid}";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            return new RegForm(data.Rows[0]);
        }

        public bool AddRegForm(RegForm reg)
        {
            _query =
                $"INSERT INTO dbo.REG_FORM (CheckIn, RenterId, RoomId, Rental) VALUES(convert(datetime,'{reg.CheckIn:G}',20),'{reg.Renter.RenterId}', {reg.RoomId}, 0)";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateForm(RegForm reg)
        {
            if (string.IsNullOrEmpty(reg.BillId))
                _query =
                    $"UPDATE dbo.REG_FORM SET CheckOut = GETDATE(), Rental = {reg.Rental} WHERE FormId = {reg.FormId}";
            else
                _query =
                    $"UPDATE dbo.REG_FORM SET CheckOut = GETDATE(), Rental = {reg.Rental}, BillId = {reg.BillId} WHERE FormId = {reg.FormId}";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public int GetServiceCharge(int formid)
        {
            _query = "EXEC USP_ServiceCharge @formid";
            var result = DataProvider.Instance.ExecuteScalar(_query, new object[] { formid });
            return result is DBNull ? 0 : Convert.ToInt32(result);
        }

        public int GetRental(int formid)
        {
            _query = $"SELECT dbo.Rental({formid}, GETDATE())";
            var result = DataProvider.Instance.ExecuteScalar(_query);
            return Convert.ToInt32(result);
        }
    }
}
