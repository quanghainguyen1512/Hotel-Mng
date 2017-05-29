using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using DTO.Annotations;

namespace DTO
{
    public class Service : INotifyPropertyChanged
    {
        #region Properties

        private string _name;
        private int _price;
        private int _svTypeId;
        private string _unit;
        private string _svTypeName;

        public int ServId { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value; 
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                _price = value; 
                OnPropertyChanged(nameof(Price));
            }
        }

        public int SvTypeId
        {
            get => _svTypeId;
            set
            {
                _svTypeId = value; 
                OnPropertyChanged(nameof(SvTypeId));
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value; 
                OnPropertyChanged(nameof(Unit));
            }
        }

        public string SvTypeName
        {
            get => _svTypeName;
            set
            {
                _svTypeName = value;
                OnPropertyChanged(nameof(SvTypeName));
            }
        }

        #endregion

        public Service() { }

        public Service(System.Data.DataRow row)
        {
            ServId = (int) row["ServId"];
            Name = row["Name"].ToString();
            Price = int.Parse(row["Price"].ToString(), NumberStyles.Currency);
            SvTypeId = (int) row["SvTypeId"];
            Unit = row["Unit"].ToString();
            SvTypeName = row["SvTypeName"].ToString();
        }

        public void UpdateProperties(string name, int price, string unit, int svTypeId)
        {
            Name = name;
            Price = price;
            Unit = unit;
            SvTypeId = svTypeId;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
