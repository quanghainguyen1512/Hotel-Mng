namespace DTO
{
    public class ServiceType : Base
    {
        #region Properties
        private int _svTypeId;
        private string _svTypeName;

        public int SvTypeId
        {
            get => _svTypeId;
            set
            {
                _svTypeId = value; 
                OnPropertyChanged(nameof(SvTypeId));
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

        public ServiceType()
        {
            
        }
        public ServiceType(System.Data.DataRow row)
        {
            SvTypeId = (int) row["SvTypeId"];
            SvTypeName = row["SvTypeName"].ToString();
        }
    }
}
