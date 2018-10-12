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
        public string FirstName { get; set; }
        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }
        /// <summary>
        /// Gets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone { get; set; }


        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime Dob { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient" /> class.
        /// The default constructor has no parameters. The default constructor 
        /// is invoked in the processing of object initializer.
        /// </summary>
        public Patient() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient" /> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The date of birth.</param>
        public Patient(string firstName, string lastName, string phoneNumber, DateTime dob)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.Dob = dob;
            this.Id = string.Empty;
        }


        public string Format()
        {
            return FirstName + " " + LastName ;
        }

    }
}
