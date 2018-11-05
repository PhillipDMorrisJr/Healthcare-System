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
        /// Gets the Social Security Number.
        /// </summary>
        /// <value>
        /// The Social Security Number.
        /// </value>
        public int SSN { get; set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; }
        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient" /> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The date of birth.</param>
        public Patient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob, string gender, string address, int id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.ID = id.ToString("D4");
            this.DOB = dob;
            this.SSN = ssn;
            this.Gender = gender;
            this.Address = address;
        }

        public Patient(string firstName, string lastName, string phoneNumber, DateTime dob, string gender, string address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.DOB = dob;
            this.Gender = gender;
            this.Address = address;
        }

        public string Format()
        {
            return FirstName + " " + LastName ;

        }

    }
}
