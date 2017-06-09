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

        private int _total;
        private Renter _renter;
        private DateTime? _checkOut;
        private IEnumerable<UseService> _useServices;
        private int _serviceCharge;
        private int _rental;
        private int _roomId;

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

        public int RoomId
        {
            get => _roomId;
            set
            {
                _roomId = value; 
                OnPropertyChanged(nameof(RoomId));
            }
        }

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

        public IEnumerable<UseService> UseServices
        {
            get => _useServices;
            set
            {
                _useServices = value; 
                OnPropertyChanged(nameof(UseServices));
            }
        }

        public int ServiceCharge
        {
            get => _serviceCharge;
            set
            {
                _serviceCharge = value; 
                OnPropertyChanged(nameof(ServiceCharge));
            }
        }

        public int Total
        {
            get => _total;
            set
            {
                _total = value; 
                OnPropertyChanged(nameof(Total));
            }
        }

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
