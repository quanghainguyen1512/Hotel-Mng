using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class RoomTypeDAO
    {
        private static RoomTypeDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private RoomTypeDAO() { }
        public static RoomTypeDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new RoomTypeDAO();
                }
                return _instance;
            }
        }

        public ObservableCollection<RoomType> GetAllRoomTypes()
        {
            var result = new ObservableCollection<RoomType>();
            _query = "SELECT * FROM ROOM_TYPE";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                result.Add(new RoomType(row));
            }
            return result;
        }

        public bool AddRoomTypes(RoomType roomType)
        {
            _query =
                $"INSERT INTO dbo.ROOM_TYPE VALUES ({roomType.RoomTypeId}, {roomType.PriceByDay}, {roomType.Price1StHour}, {roomType.PricePerHour}, N'{roomType.Note}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateRoomTypes(RoomType roomType)
        {
            _query =
                "UPDATE dbo.ROOM_TYPE " +
                $"SET PriceByDay = {roomType.PriceByDay}, Price1stHour = {roomType.Price1StHour}, PricePerHour = {roomType.PricePerHour}, Note = '{roomType.Note}' " +
                $"WHERE RoomTypeId = '{roomType.RoomTypeId}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
