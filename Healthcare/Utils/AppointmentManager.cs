using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class AppointmentManager
    {     
        public static Dictionary<Patient, List<Appointment>> Appointments = new Dictionary<Patient, List<Appointment>>();
        public static Appointment CurrentAppointment = null;
        /// <summary>
        /// Adds the appointment.
        /// </summary>
        /// <param name="appt">The appt.</param>
        /// <param name="patient">The patient.</param>
        public static void AddAppointment(Appointment appt, Patient patient)
       {
           AppointmentManager.Appointments[patient].Add(appt);
       }
        /// <summary>
        /// Retrieves the used time slots.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public static List<TimeSpan> RetrieveUsedTimeSlots(DateTime date, Doctor doctor, Patient patient)
        {
            AppointmentDAL apptDal = new AppointmentDAL();
            return apptDal.GetTimeSlots(date, doctor, patient);
        }

        public static void UpdateAppointment(Appointment originalAppointment, Appointment newAppointment, Patient patient)
        {
            AppointmentDAL.UpdateAppointment(originalAppointment, newAppointment);
            AppointmentManager.Appointments[patient].Remove(originalAppointment);
            AppointmentManager.Appointments[patient].Add(newAppointment);


        }
    }
}
