using DTO;

namespace DAO
{
    public class BillDAO
    {
        private static BillDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private BillDAO() { }
        public static BillDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new BillDAO();
                }
                return _instance;
            }
        }

        public bool AddBill(Bill bill)
        {
            _query = $"INSERT INTO dbo.BILL (BillId, TotalMoney, PayerName, CompanyName) " +
                     $"VALUES ('{bill.BillId}', {bill.TotalMoney}, N'{bill.PayerName}', N'{bill.Company}')";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
