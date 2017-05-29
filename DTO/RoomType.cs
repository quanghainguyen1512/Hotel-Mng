using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DTO.Annotations;

namespace DTO
{
    public class RoomType : INotifyPropertyChanged
    {
        private int _priceByDay;
        private int _price1StHour;
        private int _pricePerHour;
        private string _note;

        #region Properties

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

        public int Price1StHour
        {
            get => _price1StHour;
            set
            {
                _price1StHour = value; 
                OnPropertyChanged(nameof(Price1StHour));
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
            Price1StHour = int.Parse(row["Price1StHour"].ToString(), NumberStyles.Currency);
            PricePerHour = int.Parse(row["PricePerHour"].ToString(), NumberStyles.Currency);
            Note = row["Note"].ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
