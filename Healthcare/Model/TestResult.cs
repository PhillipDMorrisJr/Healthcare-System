using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class TestResult
    {
        public int ResultId { get; set; }

        public int OrderId { get; private set; }

        public DateTime Date { get; private set; }

        public TimeSpan Time { get; private set; }

        public bool Readings { get; private set; }

        public TestResult(int orderId, DateTime date, TimeSpan time, bool testReadings)
        {
            OrderId = orderId;
            Date = date;
            Time = time;
            Readings = testReadings;
        }
    }
}
