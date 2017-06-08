using System.Data;

namespace DTO
{
    public class Fee
    {
        public float FeeForEachMoreGuest { get; set; }
        public float FeeForForeigner { get; set; }

        public Fee(DataRow row)
        {
            FeeForEachMoreGuest = float.Parse(row["FeeForEachMoreGuest"].ToString());
            FeeForForeigner = float.Parse(row["FeeForForeigner"].ToString());
        }
    }
}
