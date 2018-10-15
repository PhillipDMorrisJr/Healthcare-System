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
        public static Appointment CurrentAppointment = null;
        public static Dictionary<Patient, List<Appointment>> Appointments = new Dictionary<Patient, List<Appointment>>();

        public static void AddAppointment(Appointment appt, Patient patient)
        {
            AppointmentManager.Appointments[patient].Add(appt);
        }
    }
}
