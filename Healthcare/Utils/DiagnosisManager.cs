using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class DiagnosisManager
    {
        public static List<Diagnosis> DoctorDiagnoses = DiagnosisDAL.GetDiagnoses();

        public static Diagnosis CurrentDiagnosis;

        public static List<Diagnosis> GetRefreshedDiagnoses()
        {
            DoctorDiagnoses = DiagnosisDAL.GetDiagnoses();
            return DoctorDiagnoses;
        }
    }
}