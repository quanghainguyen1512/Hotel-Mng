using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public abstract class Guest : Base
    {
        protected string _name;
        protected string _identityNum;
        protected Nationality _nationality;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public Nationality Nationality
        {
            get => _nationality;
            set
            {
                _nationality = value;
                OnPropertyChanged(nameof(Nationality));
            }
        }

        public Guest(System.Data.DataRow row)
        {
            _name = row["Name"].ToString();
            _identityNum = row["IdentityNum"].ToString();
            _nationality = new Nationality(row);
        }

        public Guest()
        {
            _nationality = new Nationality();
        }
    }
}
