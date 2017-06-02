using System.Globalization;

namespace DTO
{
    public class Service : Base
    {
        #region Properties

        private string _name;
        private int _price;
        private string _unit;
        private ServiceType _svType;
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


        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }

        public ServiceType SvType
        {
            get => _svType;
            set
            {
                _svType = value;
                OnPropertyChanged(nameof(SvType));
            }
        }

        #endregion

        public Service()
        {
        }

        public Service(System.Data.DataRow row)
        {
            ServId = (int) row["ServId"];
            Name = row["Name"].ToString();
            Price = int.Parse(row["Price"].ToString(), NumberStyles.Currency);
            Unit = row["Unit"].ToString();
            SvType = new ServiceType(row);
        }
    }
}
