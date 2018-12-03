using System;

namespace Healthcare.Model
{
    public class Order
    {
        public Order(int cuId, int code, DateTime date, TimeSpan time, string doctorId)
        {
            CuId = cuId;
            Code = code;
            Date = date;
            Time = time;
            DoctorId = doctorId;
        }

        public int OrderId { get; set; }

        public int CuId { get; }

        public int Code { get; }

        public int ApptId { get; private set; }

        public DateTime Date { get; }

        public TimeSpan Time { get; }

        public string DoctorId { get; }
    }
}