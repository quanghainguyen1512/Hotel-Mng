using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Report
    {
        #region Properties
        public char RoomTypeId { get; set; }
        public int Month { get; set; }
        public int Income { get; set; }
        public float Proportion { get; set; }
        #endregion
        public Report(System.Data.DataRow row)
        {
            RoomTypeId  = (char)row["RoomTypeId"];
            Month       = (int)row["Month"];
            Income      = (int)row["Income"];
            Proportion  = (float)row["Proportion"];
        }
    }
}
