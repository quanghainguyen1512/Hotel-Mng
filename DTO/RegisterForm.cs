using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class RegisterForm
    {
        #region Properties
        public int      FormId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        #endregion

        public RegisterForm(System.Data.DataRow row)
        {
            FormId   = (int)row["FormId"];
            CheckIn  = (DateTime)row["CheckIn"];
            CheckOut = (DateTime?)row["CheckOut"];
        }
    }
}
