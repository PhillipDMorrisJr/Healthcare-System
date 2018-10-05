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


        public DateTime DOB { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        public Patient(string firstName, string lastName, string phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
        }


        public string Format()
        {
           // DateTime time = DateTime.Today.Add(AppointmentTime);
            return FirstName + " " + LastName + " ";
            //+ string.Format("{0:(###) ###-####}", Convert.ToInt32(Phone)) + " " + AppointmentDate.ToString("d") + " " +
            // time.ToString("hh:mm tt");
        }

    }
}
