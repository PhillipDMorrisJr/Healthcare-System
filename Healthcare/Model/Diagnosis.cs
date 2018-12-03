using System;

namespace Healthcare.Model
{
    public class Diagnosis
    {
        public Diagnosis(int cuId, string doctorId, DateTime date, TimeSpan time, string diagnosis, bool isFinal)
        {
            CuId = cuId;
            DoctorId = doctorId;
            Date = date;
            Time = time;
            CheckupDiagnosis = diagnosis;
            IsFinalDiagnosis = isFinal;
        }

        public int DiagnosisId { get; set; }

        public int CuId { get; }

        public string DoctorId { get; }

        public DateTime Date { get; }

        public TimeSpan Time { get; }

        public string CheckupDiagnosis { get; }

        public bool IsFinalDiagnosis { get; }
    }
}