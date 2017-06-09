using System;

namespace DTO
{
    public class Bill
    {
        #region Properties
        public string BillId { get; set; }
        public int TotalMoney { get; set; }
        public string PayerName { get; set; }
        public string Company { get; set; }
        #endregion

        public Bill(System.Data.DataRow row)
        {
            BillId      = row["BillId"].ToString();
            TotalMoney  = (int)row["TotalMoney"];
            Company     = row["Company"].ToString();
            PayerName = row["PayerName"].ToString();
        }

        public Bill()
        {
        }

        public static string GenerateBillId()
        {
            var now = DateTime.Now;
            return $"{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}{now.Millisecond}";
        }
    }
}
