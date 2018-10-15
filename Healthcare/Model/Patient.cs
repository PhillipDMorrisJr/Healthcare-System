using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Utils;

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
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime DOB { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string ID { get; set; }

        /// <summary>
        /// Gets the identification number.
        /// </summary>
        /// <value>
        /// The identification number.
        /// </value>
        public int IdentificationNumber { get; } 

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient" /> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The date of birth.</param>
        public Patient(string firstName, string lastName, string phoneNumber, DateTime dob, int id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.IdentificationNumber = id;
            this.ID = id.ToString("D4");
            this.DOB = dob;
        }

        public string Format()
        {
            return FirstName + " " + LastName ;

        }

    }
}
