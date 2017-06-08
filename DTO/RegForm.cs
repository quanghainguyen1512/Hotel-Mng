using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RegForm : Base
    {
        #region Fields

        private int _rental;
        private Renter _renter;
        private DateTime? _checkOut;

        #endregion

        #region Properties
        public int FormId { get; set; }
        public DateTime CheckIn { get; set; }

        public DateTime? CheckOut
        {
            get => _checkOut;
            set
            {
                _checkOut = value; 
                OnPropertyChanged(nameof(CheckOut));
            }
        }

        public Renter Renter
        {
            get => _renter;
            set
            {
                _renter = value;
                OnPropertyChanged(nameof(Renter));
            }
        }

        public int RoomId { get; set; }

        //public Bill Bill { get; set; }
        public string BillId { get; set; }

        public int Rental
        {
            get => _rental;
            set
            {
                _rental = value;
                OnPropertyChanged(nameof(Rental));
            }
        }

        public ObservableCollection<Service> Services { get; set; }
        #endregion

        public RegForm(System.Data.DataRow row)
        {
            FormId   = (int)row["FormId"];
            CheckIn  = (DateTime)row["CheckIn"];
            RoomId = (int) row["RoomId"];
        }

        public RegForm()
        {
            Renter = new Renter();
        }
    }
}
