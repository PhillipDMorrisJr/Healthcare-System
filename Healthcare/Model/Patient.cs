using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    /// <summary>
    /// Patient
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; }
        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; }
        /// <summary>
        /// Gets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone { get; }
        /// <summary>
        /// Gets the appointment date.
        /// </summary>
        /// <value>
        /// The appointment date.
        /// </value>
        public DateTime AppointmentDate { get; }
        /// <summary>
        /// Gets or sets the appointment time.
        /// </summary>
        /// <value>
        /// The appointment time.
        /// </value>
        public TimeSpan AppointmentTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        public Patient(string firstName, string lastName, string phoneNumber, DateTime appointmentDate, TimeSpan appointmentTime )
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.AppointmentDate = appointmentDate;
            this.AppointmentTime = appointmentTime;
        } 

    }
}
