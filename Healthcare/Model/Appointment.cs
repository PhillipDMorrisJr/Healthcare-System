using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Appointment
    {
        public Patient Patient { get;  }
        public DateTime AppointmentDateTime { get;  }
        public Appointment(Patient patient, DateTime appointmentDateTime)
        {
            Patient = patient;
            AppointmentDateTime = appointmentDateTime;
        }
    }
}
