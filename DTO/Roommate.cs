using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Roommate
    {
        #region Properties

        public string Name { get; set; }
        public string IdentityNum { get; set; }
        public Nationality Nationality { get; set; }

        #endregion

        public Roommate(System.Data.DataRow row)
        {
            Name = row["Name"].ToString();
            IdentityNum = row["IdentityNum"].ToString();
            Nationality = new Nationality(row);
        }

        public Roommate()
        {
            Nationality = new Nationality();
        }
    }
}
