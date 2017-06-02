using System;
using System.Globalization;

namespace DTO
{
    public class RoomType : Base
    {
        #region Properties

        private int _priceByDay;
        private int _priceFirstHour;
        private int _pricePerHour;
        private string _note;


        public char RoomTypeId { get; set; }

        public int PriceByDay
        {
            get => _priceByDay;
            set
            {
                _priceByDay = value; 
                OnPropertyChanged(nameof(PriceByDay));
            }
        }

        public int PriceFirstHour
        {
            get => _priceFirstHour;
            set
            {
                _priceFirstHour = value; 
                OnPropertyChanged(nameof(PriceFirstHour));
            }
        }

        public int PricePerHour
        {
            get => _pricePerHour;
            set
            {
                _pricePerHour = value; 
                OnPropertyChanged(nameof(PricePerHour));
            }
        }

        public string Note
        {
            get => _note;
            set
            {
                _note = value; 
                OnPropertyChanged(nameof(Note));
            }
        }

        #endregion

        public RoomType() { }

        public RoomType(System.Data.DataRow row)
        {
            RoomTypeId = Convert.ToChar(row["RoomTypeId"]);
            PriceByDay = int.Parse(row["PriceByDay"].ToString(), NumberStyles.Currency);
            //PriceFirstHour = int.Parse(row["PriceFirstHour"].ToString(), NumberStyles.Currency);
            PricePerHour = int.Parse(row["PricePerHour"].ToString(), NumberStyles.Currency);
            Note = row["Note"].ToString();
        }

    }
}
