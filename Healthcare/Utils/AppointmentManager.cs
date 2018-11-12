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
        public static void AddAppointment(Appointment appt, Patient patient)
       {
           AppointmentManager.Appointments[patient].Add(appt);
       }
        public static List<TimeSpan> retrieveUsedTimeSlots(DateTime date)
        {
            AppointmentDAL apptd = new AppointmentDAL();
            return apptd.GetTimeSlots(date);
        } 
    }
}
