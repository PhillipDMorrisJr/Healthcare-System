using System;

namespace Healthcare.Model
{
    public class RecordedDiagnosis
    {
        public RecordedDiagnosis(int diagnosisId, int apptId, int cuId, DateTime date, TimeSpan time)
        {
            DiagnosisId = diagnosisId;
            ApptId = apptId;
            Date = date;
            Time = time;
            CuId = cuId;
        }

        public int RecordId { get; set; }

        public int DiagnosisId { get; set; }

        public int ApptId { get; set; }

        public int CuId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }
    }
}