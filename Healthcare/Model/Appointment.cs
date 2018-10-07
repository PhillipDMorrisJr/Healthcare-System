using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Appointment
    {
        /// <summary>
        /// Gets the patient.
        /// </summary>
        /// <value>
        /// The patient.
        /// </value>
        public Patient Patient { get;  }
        /// <summary>
        /// Gets the appointment date time.
        /// </summary>
        /// <value>
        /// The appointment date time.
        /// </value>
        public DateTime AppointmentDateTime { get;  }
        /// <summary>
        /// Gets the appointment time.
        /// </summary>
        /// <value>
        /// The appointment time.
        /// </value>
        public TimeSpan AppointmentTime { get;  }
        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment"/> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        /// <param name="appointmentTime">The appointment time.</param>
        public Appointment(Patient patient, DateTime appointmentDateTime, TimeSpan appointmentTime)
        {
            this.Patient = patient;
            this.AppointmentDateTime = appointmentDateTime;
            this.AppointmentTime = appointmentTime;
        }

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns></returns>
        public string Format()
        {
            DateTime time = DateTime.Today.Add(this.AppointmentTime);

             return AppointmentDateTime.ToString("d") + " " +  time.ToString("hh:mm tt");
        }

    }
}
