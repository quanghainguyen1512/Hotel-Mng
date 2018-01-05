using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DTO
{
    public class Renter : Guest
    {
        private string _renterId;
        private bool _gender;
        private string _phoneNum;
        private string _address;

        public Renter()
        {
            Nationality = new Nationality();
        }
        public string RenterId
        {
            get => _renterId;
            set
            {
                _renterId = value;
                OnPropertyChanged(nameof(RenterId));
            }
        }
        
        public bool Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        public string PhoneNum
        {
            get => _phoneNum;
            set
            {
                _phoneNum = value;
                OnPropertyChanged(nameof(PhoneNum));
            }
        }
        
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public Renter(System.Data.DataRow row) : base(row)
        {
            RenterId = row["RenterId"].ToString();
            Gender = row["Gender"].Equals(true);
            PhoneNum = row["PhoneNum"].ToString();
            Address = row["Address"].ToString();
        }
    }
}
