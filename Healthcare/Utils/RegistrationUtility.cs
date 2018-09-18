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

        /// <summary>
        /// Sets the registration patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static object SetRegistrationPatient(Patient patient)
        {
            Patient data = RegistrationUtility.currentPatient;
            return data;
        }

        /// <summary>
        /// Gets the previous registration data.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static Patient GetPreviousRegistrationData()
        {
            Patient data = RegistrationUtility.currentPatient;
            RegistrationUtility.currentPatient = null;
            return data;
        } }
}
