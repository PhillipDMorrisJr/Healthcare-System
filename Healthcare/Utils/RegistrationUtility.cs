using System;
using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    /// <summary>
    ///     This class is used to pass the registered patient from the Registration page to the main page
    /// </summary>
    public static class RegistrationUtility
    {
        private static List<Patient> Patients = PatientDAL.GetPatients();

        /// <summary>
        ///     Sets the registration patient.
        /// </summary>
        /// <param name="ssn"> The ssn.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="gender"> The gender.</param>
        /// <param name="address"> The address.</param>
        public static void CreateNewPatient(int ssn, string firstName, string lastName, string phoneNumber,
            DateTime dob, string gender, string address)
        {
            var patient = PatientDAL.AddPatient(ssn, firstName, lastName, phoneNumber, dob, gender, address);

            if (patient != null)
            {
                Patients.Add(patient);
                AppointmentManager.Appointments.Add(patient, new List<Appointment>());
            }
        }

        /// <summary>
        ///     Edits a registered patient.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="ssn">The ssn.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="gender"> The gender.</param>
        /// <param name="address">The address.</param>
        /// <param name="addressId">The id for address.</param>
        public static void EditPatient(int id, int ssn, string firstName, string lastName, string phoneNumber,
            DateTime dob, string gender, string address, int addressId)
        {
            var patient = PatientDAL.UpdatePatient(id, ssn, firstName, lastName, phoneNumber, dob, gender, address,
                addressId);
            Patient matchedPatient = null;
            var appointments = new List<Appointment>();

            if (patient == null) return;

            foreach (var registryPatient in Patients)
            {
                if (registryPatient.Id != patient.Id) continue;
                matchedPatient = registryPatient;
            }

            if (matchedPatient == null) return;

            foreach (var entry in AppointmentManager.Appointments)
                if (entry.Key.Id == matchedPatient.Id)
                    appointments = entry.Value;

            Patients.Remove(matchedPatient);
            Patients.Add(patient);
            AppointmentManager.Appointments[patient] = appointments;
        }

        /// <summary>
        ///     Finds a registered patient by first and last name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>List of all found patients.</returns>
        public static void FindPatientsByName(string firstName, string lastName)
        {
            var foundPatients = PatientDAL.SelectPatientsByName(firstName, lastName);

            if (foundPatients == null || foundPatients.Count == 0) return;

            var appointments = new List<Appointment>();
            Patients.Clear();

            foreach (var patient in foundPatients)
            {
                foreach (var entry in AppointmentManager.Appointments)
                    if (entry.Key.Id == patient.Id)
                        appointments = entry.Value;

                Patients.Add(patient);
                AppointmentManager.Appointments[patient] = appointments;
            }
        }

        /// <summary>
        ///     Finds a registered patient by date of birth.
        /// </summary>
        /// <param name="dob">The date.</param>
        /// <returns>List of all found patients.</returns>
        public static void FindPatientsByDob(DateTime dob)
        {
            var foundPatients = PatientDAL.SelectPatientsByDob(dob);

            if (foundPatients == null || foundPatients.Count == 0) return;

            var appointments = new List<Appointment>();
            Patients.Clear();

            foreach (var patient in foundPatients)
            {
                foreach (var entry in AppointmentManager.Appointments)
                    if (entry.Key.Id == patient.Id)
                        appointments = entry.Value;

                Patients.Add(patient);
                AppointmentManager.Appointments[patient] = appointments;
            }
        }

        /// <summary>
        ///     Finds a registered patient by first, last name, and date of birth.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dob">The date.</param>
        /// <returns>List of all found patients.</returns>
        public static void FindPatientsByNameAndDob(string firstName, string lastName, DateTime dob)
        {
            var foundPatients = PatientDAL.SelectPatientsByNameAndDob(firstName, lastName, dob);

            if (foundPatients == null || foundPatients.Count == 0) return;

            var appointments = new List<Appointment>();
            Patients.Clear();

            foreach (var patient in foundPatients)
            {
                foreach (var entry in AppointmentManager.Appointments)
                    if (entry.Key.Id == patient.Id)
                        appointments = entry.Value;

                Patients.Add(patient);
                AppointmentManager.Appointments[patient] = appointments;
            }
        }

        /// <summary>
        ///     Gets the previous registration data.
        /// </summary>
        /// <returns>List of all patients available.</returns>
        public static List<Patient> GetPatients()
        {
            return Patients;
        }


        public static List<Patient> GetRefreshedPatients()
        {
            Patients = PatientDAL.GetPatients();
            return Patients;
        }
    }
}