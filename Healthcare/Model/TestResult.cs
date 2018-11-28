using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class TestResult
    {
        public int PatientId { get; private set; }
        public int Code { get; private set; }
        public TimeSpan Time { get; private set; }
        public int AppointmentId { get; private set; }
        public bool Readings { get; private set; }
        public string Diagnosis { get; private set; }

        public TestResult(int patientId, int appointmentId, int code, TimeSpan time, bool testReadings, string testDiagnosis)
        {
            this.PatientId = patientId;
            this.Code = code;
            this.Time = time;
            this.AppointmentId = appointmentId;
            this.Readings = testReadings;
            this.Diagnosis = testDiagnosis;
        }
    }
}
