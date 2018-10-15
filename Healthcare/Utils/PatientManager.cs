using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;

namespace Healthcare.Utils
{
    class PatientManager
    {
        public static List<Patient> Patients = new List<Patient>();
        public static Patient CurrentPatient;
    }
}
