using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class DoctorManager
    {
        public static List<Doctor> Doctors = new List<Doctor>() {
            new Doctor("Jimmy", "Johnson")
        };
    }
}
