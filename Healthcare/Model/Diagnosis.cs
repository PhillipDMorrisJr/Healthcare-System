using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Diagnosis
    {
        public int DiagnosisId { get; set; }

        public int CuId { get; private set; }

        public string DoctorId { get; private set; }

        public DateTime Date { get; private set; }

        public TimeSpan Time { get; private set; }

        public string CheckupDiagnosis { get; private set; }

        public bool IsFinalDiagnosis { get; private set; }


        public Diagnosis(int cuId, string doctorId, DateTime date, TimeSpan time, string diagnosis, bool isFinal)
        {
            CuId = cuId;
            DoctorId = doctorId;
            Date = date;
            Time = time;
            CheckupDiagnosis = diagnosis;
            IsFinalDiagnosis = isFinal;
        }
    }
}
