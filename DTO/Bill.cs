using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Bill
    {
        #region Properties
        public string BillId { get; set; }
        public int TotalMoney { get; set; }
        public string Company { get; set; }
        #endregion

        public Bill(System.Data.DataRow row)
        {
            BillId      = row["BillId"].ToString();
            TotalMoney  = (int)row["TotalMoney"];
            Company     = row["Company"].ToString();
        }
    }
}
