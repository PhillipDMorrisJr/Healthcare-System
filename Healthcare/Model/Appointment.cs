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
        /// Gets the doctor.
        /// </summary>
        /// <value>
        /// The doctor.
        /// </value>
        public Doctor Doctor { get;  }
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
        /// Gets a value indicating whether this instance is checked in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is checked in; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckedIn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment" /> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        /// <param name="appointmentTime">The appointment time.</param>
        /// <param name="description">The description.</param>
        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDateTime, TimeSpan appointmentTime, string description, bool checkedIn)
        {
            this.Doctor = doctor;
            this.Patient = patient;
            this.AppointmentDateTime = appointmentDateTime;
            this.AppointmentTime = appointmentTime;
            this.IsCheckedIn = checkedIn;
        }

        public void CheckIn()
        {
            if (!IsCheckedIn)
            {
                this.IsCheckedIn = true;
            }
        }



        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns></returns>
        public string Format()
        {
            DateTime time = DateTime.Today.Add(this.AppointmentTime);

             return this.Doctor.FullName + " on " + this.AppointmentDateTime.ToString("d") + " at " +  time.ToString("hh:mm tt");
        }

    }
}
