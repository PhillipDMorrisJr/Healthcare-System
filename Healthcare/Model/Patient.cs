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
        /// Gets the Social Security Number.
        /// </summary>
        /// <value>
        /// The Social Security Number.
        /// </value>
        public int Ssn { get; set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets the addressId.
        /// </summary>
        /// <value>
        /// The addressId.
        /// </value>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets the id
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient" /> class.
        /// </summary>
        /// <param name="ssn">The social security number.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="addressId">The addressId.</param>
        public Patient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob, string gender, int addressId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phoneNumber;
            this.Dob = dob;
            this.Gender = gender;
            this.AddressId = addressId;
            this.Ssn = ssn;
        }

        public string Format()
        {
            return FirstName + " " + LastName ;

        }
    }
}