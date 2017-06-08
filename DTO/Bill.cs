using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<RegForm> RegisterForms{ get; set; }    
        #endregion

        public Bill(System.Data.DataRow row)
        {
            BillId      = row["BillId"].ToString();
            TotalMoney  = (int)row["TotalMoney"];
            Company     = row["Company"].ToString();
        }

        public static string GenerateBillId()
        {
            var now = DateTime.Now;
            return $"{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}{now.Millisecond}";
        }
    }
}
