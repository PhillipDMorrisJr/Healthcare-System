using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Doctor
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Dictionary<DateTime, List<TimeSpan>> AvailableDates { get; }
        
        public Doctor(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AvailableDates = new Dictionary<DateTime, List<TimeSpan>>();
        }

    }
}
