using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class RecordDiagnosisManager
    {
        public static List<RecordedDiagnosis> RecordedDiagnoses = RecordedDiagnosesDAL.GetRecordedDiagnoses();

        public static List<RecordedDiagnosis> GetRefreshedRecordedDiagnoses()
        {
            RecordedDiagnoses = RecordedDiagnosesDAL.GetRecordedDiagnoses();
            return RecordedDiagnoses;
        }
    }
}