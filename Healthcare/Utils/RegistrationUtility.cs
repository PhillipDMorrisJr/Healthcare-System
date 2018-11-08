using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    /// <summary>
    /// This class is used to pass the registered patient from the Registration page to the main page
    /// </summary>
    public static class RegistrationUtility 
    {
        private static readonly List<Patient> Patients = PatientDAL.GetPatients();

        /// <summary>
        /// Sets the registration patient.
        /// </summary>
        /// <param name="ssn"> The ssn.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="gender"> The gender.</param>
        /// <param name="address"> The address.</param>
        public static void CreateNewPatient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob, string gender, string address)
        {
            Patient patient = PatientDAL.AddPatient(ssn, firstName, lastName, phoneNumber, dob, gender, address);

            if (patient != null)
            {
                Patients.Add(patient);
                AppointmentManager.Appointments.Add(patient, new List<Appointment>());
            }          
        }

        /// <summary>
        /// Edits a registered patient.
        /// </summary>
        /// <param name="ssn">The ssn.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="gender"> The gender.</param>
        /// <param name="address">The address.</param>
        public static void EditPatient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob, string gender, string address)
        {
            //Patient patient = PatientDAL.UpdatePatient(ssn, firstName, lastName, phoneNumber, dob, gender, address);
        }

        /// <summary>
        /// Finds a registered patient by first and last name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>List of all found patients.</returns>
        public static List<Patient> FindPatientByName(string firstName, string lastName)
        {
            return PatientDAL.SelectPatientsByName(firstName, lastName);
        }

        /// <summary>
        /// Finds a registered patient by date of birth.
        /// </summary>
        /// <param name="dob">The date.</param>
        /// <returns>List of all found patients.</returns>
        public static List<Patient> FindPatientByDob(DateTime dob)
        {
            return PatientDAL.SelectPatientsByDob(dob);
        }

        /// <summary>
        /// Finds a registered patient by first, last name, and date of birth.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dob">The date.</param>
        /// <returns>List of all found patients.</returns>
        public static List<Patient> FindPatientByNameAndDob(string firstName, string lastName, DateTime dob)
        {
            return PatientDAL.SelectPatientsByNameAndDob(firstName, lastName, dob);
        }

        /// <summary>
        /// Gets the previous registration data.
        /// </summary>
        /// <returns>List of all patients available.</returns>
        public static List<Patient> GetPatients()
        {
            return RegistrationUtility.Patients;
        }
    }
}
