using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class TestTaken
    {

        public int TakenId { get; set; }

        public int OrderId { get; private set; }

        public bool IsTaken { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public TestTaken(int orderId, bool taken, DateTime date, TimeSpan time)
        {
            OrderId = orderId;
            IsTaken = taken;
            Date = date;
            Time = time;
        }
    }
}
