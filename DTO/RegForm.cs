using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RegForm : Base
    {
        private Room _room;

        #region Properties
        public int FormId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public Renter Renter { get; set; }

        public Room Room
        {
            get => _room;
            set
            {
                _room = value; 
                OnPropertyChanged(nameof(Room));
            }
        }

        public Bill Bill { get; set; }
        public int Rental { get; set; }
        #endregion

        public RegForm(System.Data.DataRow row)
        {
            FormId   = (int)row["FormId"];
            CheckIn  = (DateTime)row["CheckIn"];
            CheckOut = (DateTime?)row["CheckOut"];
        }

        public RegForm()
        {
        }
    }
}
