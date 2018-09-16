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
        public string FirstName { get; }
        public string LastName { get; }
        public string Phone { get; }
        public DateTime AppointmentDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        public Patient(string firstName, string lastName, string phoneNumber, DateTime appointmentDateTime)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.AppointmentDate = appointmentDateTime;
        } 

    }
}
