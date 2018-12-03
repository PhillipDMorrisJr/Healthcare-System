using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class DoctorManager
    {
        public static List<Doctor> Doctors = DoctorDAL.GetDoctors();
    }
}