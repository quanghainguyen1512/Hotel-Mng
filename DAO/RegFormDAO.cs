using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class RegFormDAO
    {
        private static RegFormDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private RegFormDAO() { }
        public static RegFormDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RegFormDAO();
                }
                return _instance;
            }
        }

        public bool AddRegForm(RegForm reg)
        {
            _query =
                $"INSERT INTO dbo.REG_FORM (CheckIn, RenterId, RoomId, Rental) VALUES({reg.CheckIn},'{reg.Renter.RenterId}', {reg.Room.RoomId}, 0)";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

    }
}
