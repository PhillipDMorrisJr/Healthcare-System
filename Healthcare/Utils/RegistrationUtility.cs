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
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
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
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        public static void EditPatient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob, string gender, string address)
        {
            //Patient patient = PatientDAL.UpdatePatient(ssn, firstName, lastName, phoneNumber, dob, gender, address);
        }

        /// <summary>
        /// Gets the previous registration data.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static List<Patient> GetPatients()
        {
            return RegistrationUtility.Patients;
        }
    }
}
