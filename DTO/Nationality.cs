using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Nationality : Base
    {
        private int _natId;
        private string _natName;

        public int NatId
        {
            get => _natId;
            set
            {
                _natId = value; 
                OnPropertyChanged(nameof(NatId));
            }
        }

        public string NatName
        {
            get => _natName;
            set
            {
                _natName = value; 
                OnPropertyChanged(nameof(NatName));
            }
        }

        public Nationality(DataRow row)
        {
            NatId = (int) row["NatId"];
            NatName = row["NatName"].ToString();
        }

        public Nationality()
        {
        }

        public override bool Equals(object obj)
        {
            var nat = obj as Nationality;
            if (nat is null) return false;

            return NatId == nat.NatId;
        }
    }
}
