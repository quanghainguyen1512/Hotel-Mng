using System;
using System.Collections.Generic;
using System.Globalization;
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
        public int SvTypeId { get; set; }
        public string Unit { get; set; }
        public string SvTypeName { get; set; }
        #endregion
        public Service(System.Data.DataRow row)
        {
            ServId  = (int)row["ServId"];
            Name    = row["Name"].ToString();
            Price = int.Parse(row["Price"].ToString(), NumberStyles.Currency);
            SvTypeId = (int) row["SvTypeId"];
            Unit = row["Unit"].ToString();
            SvTypeName = row["SvTypeName"].ToString();
        }

        public void UpdateProperties(string name, int price, string unit, int svTypeId)
        {
            Name = name;
            Price = price;
            Unit = unit;
            SvTypeId = svTypeId;
        }
    }
}
