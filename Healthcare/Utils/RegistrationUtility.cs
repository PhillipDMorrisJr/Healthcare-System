using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;

namespace Healthcare.Utils
{
    /// <summary>
    /// This class is used to pass the registered patient from the Registration page to the main page
    /// </summary>
    public static class RegistrationUtility 
    {
        private static Patient currentPatient= null;
        private static List<Patient> patients = new List<Patient>();

        /// <summary>
        /// Adds the registration patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static void AddNewPatient(Patient patient)
        {

            // TODO: Fix Error Accessing database, can't read port.
            // Add Patient to Database
            // DatabaseManager.AddPatient(patient); 

            patients.Add(patient);
            AppointmentManager.Appointments.Add(patient, new List<Appointment>());
        }

        /// <summary>
        /// Gets the previous registration data.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static List<Patient> GetPatients()
        {
            return RegistrationUtility.patients;
        } }
}
