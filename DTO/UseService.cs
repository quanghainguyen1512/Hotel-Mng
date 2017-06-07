using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UseService
    {
        #region Properties

        public DateTime Time { get; set; }
        public int Quantity { get; set; }
        public int ServId { get; set; }
        public string SvName { get; set; }
        

        #endregion

        #region Constructor

        public UseService(DataRow row)
        {
            Quantity = (int) row["Quantity"];
            Time = (DateTime) row["Time"];
            ServId = (int) row["ServId"];
            SvName = row["Name"].ToString();
        }

        public UseService()
        {
        }


        #endregion
    }
}
