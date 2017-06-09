using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ReportRow
    {
        public char RoomTypeId { get; set; }
        public int Income { get; set; }
        public double Proportion { get; set; }

        public ReportRow(DataRow row)
        {
            RoomTypeId = Convert.ToChar(row["RoomTypeId"]);
            Income = int.Parse(row["Income"].ToString(), NumberStyles.Currency);
            Proportion = double.Parse(row["Proportion"].ToString());
        }
    }
}
