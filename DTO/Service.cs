using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Service
    {
        #region Properties
        public int ServId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        #endregion
        public Service(System.Data.DataRow row)
        {
            ServId  = (int)row["ServId"];
            Name    = row["Name"].ToString();
            Price   = (int)row["Price"];
        }
    }
}
