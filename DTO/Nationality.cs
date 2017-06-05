using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Nationality
    {
        public int NatId { get; set; }
        public string NatName { get; set; }

        public Nationality(DataRow row)
        {
            NatId = (int) row["NatId"];
            NatName = row["NatName"].ToString();
        }

        public Nationality()
        {
        }
    }
}
