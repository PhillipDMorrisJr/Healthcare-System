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


        public uint ID { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is checked in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is checked in; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckedIn { get; private set; }

        public string Description { get; set; }

        public bool TestOrdered { get; private set; }

        public bool TestTaken { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment" /> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        /// <param name="appointmentTime">The appointment time.</param>
        /// <param name="description">The description.</param>
        /// <param name="checkedIn">The status for if the patient is checked in.</param>
        /// <param name="testOrdered">The status for if a test was ordered for a patient.</param>
        /// <param name="testTaken">The status for if the test ordered was taken for the patient by a doctor.</param>
        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDateTime, TimeSpan appointmentTime, string description, bool checkedIn, bool testOrdered, bool testTaken)
        {
            this.Doctor = doctor;
            this.Patient = patient;
            this.AppointmentDateTime = appointmentDateTime;
            this.AppointmentTime = appointmentTime;
            this.IsCheckedIn = checkedIn;
            this.Description = description;
            this.TestOrdered = testOrdered;
            this.TestTaken = testTaken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment" /> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        /// <param name="appointmentTime">The appointment time.</param>
        /// <param name="description">The description.</param
        /// <param name="checkedIn">The status for if the patient is checked in.</param>
        /// <param name="testOrdered">The status for if a test was ordered for a patient.</param>
        /// <param name="testTaken">The status for if the test ordered was taken for the patient by a doctor.</param>
        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDateTime, TimeSpan appointmentTime, string description, bool checkedIn, uint id, bool testOrdered, bool testTaken)
        {
            this.Doctor = doctor;
            this.Patient = patient;
            this.AppointmentDateTime = appointmentDateTime;
            this.AppointmentTime = appointmentTime;
            this.IsCheckedIn = checkedIn;
            this.Description = description;
            this.TestOrdered = testOrdered;
            this.TestTaken = testTaken;
            ID = id;
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
