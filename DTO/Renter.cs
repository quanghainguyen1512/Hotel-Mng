using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Renter
    {
        #region Properties
        public string   RenterId { get; set; }
        public string   Name { get; set; }
        public bool     Gender { get; set; }
        public string   PhoneNum { get; set; }
        public string   IdentityNum { get; set; }
        public string   Address { get; set; }
        public int      NatId { get; set; }
        #endregion

        public Renter()
        {
            
        }

        public Renter(System.Data.DataRow row)
        {
            RenterId    = row["RenterId"].ToString();
            Name        = row["Name"].ToString();   
            Gender      = row["Gender"].Equals(true);
            PhoneNum    = row["PhoneNum"].ToString();
            IdentityNum = row["IdentityNum"].ToString();
            Address     = row["Address"].ToString();
            NatId       = (int) row["NatId"];
        }
    }
}
