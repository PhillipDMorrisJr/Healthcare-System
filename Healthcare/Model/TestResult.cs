using System;

namespace Healthcare.Model
{
    public class TestResult
    {
        public TestResult(int orderId, DateTime date, TimeSpan time, bool testReadings)
        {
            OrderId = orderId;
            Date = date;
            Time = time;
            Readings = testReadings;
        }

        public int ResultId { get; set; }

        public int OrderId { get; }

        public DateTime Date { get; }

        public TimeSpan Time { get; }

        public bool Readings { get; }
    }
}