using System;
using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class AppointmentManager
    {
        public static Dictionary<Patient, List<Appointment>>
            Appointments = new Dictionary<Patient, List<Appointment>>();

        public static Appointment CurrentAppointment = null;

        /// <summary>
        ///     Adds the appointment.
        /// </summary>
        /// <param name="appt">The appt.</param>
        /// <param name="patient">The patient.</param>
        public static void AddAppointment(Appointment appt, Patient patient)
        {
            Appointments[patient].Add(appt);
        }

        /// <summary>
        ///     Retrieves the used time slots.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static List<TimeSpan> RetrieveUsedTimeSlots(DateTime date, Doctor doctor, Patient patient)
        {
            var apptDal = new AppointmentDAL();
            return apptDal.GetTimeSlots(date, doctor, patient);
        }

        public static void UpdateAppointment(Appointment originalAppointment, Appointment newAppointment,
            Patient patient)
        {
            AppointmentDAL.UpdateAppointment(originalAppointment, newAppointment);
            Appointments[patient].Remove(originalAppointment);
            Appointments[patient].Add(newAppointment);
        }
    }
}