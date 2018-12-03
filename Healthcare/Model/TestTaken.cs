using System;

namespace Healthcare.Model
{
    public class TestTaken
    {
        public TestTaken(int orderId, bool taken, DateTime date, TimeSpan time)
        {
            OrderId = orderId;
            IsTaken = taken;
            Date = date;
            Time = time;
        }

        public int TakenId { get; set; }

        public int OrderId { get; }

        public bool IsTaken { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }
    }
}