using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ServiceType
    {
        #region Properties

        public int SvTypeId { get; set; }
        public string SvTypeName { get; set; }

        #endregion

        public ServiceType()
        {
            
        }
        public ServiceType(System.Data.DataRow row)
        {
            SvTypeId = (int) row["SvTypeId"];
            SvTypeName = row["SvTypeName"].ToString();
        }
    }
}
