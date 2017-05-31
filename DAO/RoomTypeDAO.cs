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

        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            _query = "SELECT * FROM ROOM_TYPE";
            var data = DataProvider.Instance.ExecuteQueries(_query);
            foreach (DataRow row in data.Rows)
            {
                yield return new RoomType(row);
            }
        }

        public bool AddRoomTypes(RoomType roomType)
        {
            _query =
                $"INSERT INTO dbo.ROOM_TYPE VALUES ({roomType.RoomTypeId}, {roomType.PriceByDay}, {roomType.PriceFirstHour}, {roomType.PricePerHour}, N'{roomType.Note}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }

        public bool UpdateRoomTypes(RoomType roomType)
        {
            _query =
                "UPDATE dbo.ROOM_TYPE " +
                $"SET PriceByDay = {roomType.PriceByDay}, PriceFirstHour = {roomType.PriceFirstHour}, PricePerHour = {roomType.PricePerHour}, Note = '{roomType.Note}' " +
                $"WHERE RoomTypeId = '{roomType.RoomTypeId}'";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
