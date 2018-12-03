using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CuId { get; private set; }

        public int Code { get; private set; }

        public int ApptId { get; private set; }

        public DateTime Date { get; private set; }

        public TimeSpan Time { get; private set; }

        public string DoctorId { get; private set; }

        public Order(int cuId, int code, DateTime date, TimeSpan time, string doctorId)
        {
            CuId = cuId;
            Code = code;
            Date = date;
            Time = time;
            DoctorId = doctorId;
        }
    }
}
