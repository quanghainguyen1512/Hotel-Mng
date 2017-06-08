using DTO;

namespace DAO
{
    public class FeeDAO
    {
        private static FeeDAO _instance;
        private static readonly object Padlock = new object();
        private string _query;
        private FeeDAO() { }
        public static FeeDAO Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                        _instance = new FeeDAO();
                }
                return _instance;
            }
        }

        public Fee GetAllFee()
        {
            _query = "SELECT FeeForEachMoreGuest, FeeForForeigner FROM FEE";
            var result = DataProvider.Instance.ExecuteQueries(_query);
            return new Fee(result.Rows[0]);
        }

        public bool UpdateFee(Fee fee)
        {
            _query =
                $"UPDATE FEE SET FeeForEachMoreGuest = {fee.FeeForEachMoreGuest}, FeeForForeigner = {fee.FeeForForeigner} WHERE Id = 1";
            var result = DataProvider.Instance.ExecuteNonQuery(_query);
            return result > 0;
        }
    }
}
