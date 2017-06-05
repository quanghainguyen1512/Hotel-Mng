using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DTO
{
    public class Renter : Base
    {
        #region Properties
        //public string RenterId { get; set; }
        //public string Name { get; set; }
        //public bool Gender { get; set; }
        //public string PhoneNum { get; set; }
        //public string IdentityNum { get; set; }
        //public string Address { get; set; }
        #endregion

        private string _renterId;
        private string _name;
        private bool _gender;
        private string _phoneNum;
        private string _identityNum;
        private string _address;
        private Nationality _nationality;
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
        public string Name
        {
            get => _name;
            set
            {
                _name =value;
                OnPropertyChanged(nameof(Name));
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
        public string IdentityNum
        {
            get => _identityNum;
            set
            {
                _identityNum = value;
                OnPropertyChanged(nameof(IdentityNum));
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

        public Nationality Nationality
        {
            get => _nationality;
            set
            {
                _nationality = value; 
                OnPropertyChanged(nameof(Nationality));
            }
        }

        public Renter(System.Data.DataRow row)
        {
            RenterId = row["RenterId"].ToString();
            Name = row["Name"].ToString();
            Gender = row["Gender"].Equals(true);
            PhoneNum = row["PhoneNum"].ToString();
            IdentityNum = row["IdentityNum"].ToString();
            Address = row["Address"].ToString();
            Nationality = new Nationality(row);
        }
    }
}
