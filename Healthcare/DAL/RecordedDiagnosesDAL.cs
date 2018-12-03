using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using Healthcare.Utils;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    class RecordedDiagnosesDAL
    {
        private enum Attributes
        {
            RecordId = 0, DiagnosisId = 1, CuId = 2, ApptId = 3, Date = 4
        }

        public static List<RecordedDiagnosis> GetRecordedDiagnoses()
        {
            var recordedDiagnoses = new List<RecordedDiagnosis>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from recordedFinalDiagnosis";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return recordedDiagnoses;

                    while (reader.Read())
                    {
                        var recordId = reader.GetInt32((int) Attributes.RecordId);
                        var diagnosisId = reader.GetInt32((int) Attributes.DiagnosisId);
                        var cuId = reader.GetInt32((int) Attributes.CuId);
                        var apptId = reader.GetInt32((int) Attributes.ApptId);
                        var date = reader.GetDateTime((int) Attributes.Date);
                        var time = (TimeSpan) reader["time"];

                        var recordedDiagnosis = new RecordedDiagnosis(diagnosisId, apptId, cuId, date, time) {RecordId = recordId};
                        recordedDiagnoses.Add(recordedDiagnosis);                       
                    }
                }
            }
            return recordedDiagnoses;
        }

        public static void AddRecordFinalDiagnosis(RecordedDiagnosis finalDiagnosis)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                var insertFinalQuery =
                    "INSERT INTO `recordedFinalDiagnosis` (`diagnosisID`, `cuID`, `apptID`, `date`, `time`) VALUE (@diagnosisId, @cuId, @apptId, @date, @time)";

                using (MySqlCommand cmd = new MySqlCommand(insertFinalQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@diagnosisId", finalDiagnosis.DiagnosisId);
                    cmd.Parameters.AddWithValue("@cuId", finalDiagnosis.CuId);
                    cmd.Parameters.AddWithValue("@apptId", finalDiagnosis.ApptId);
                    cmd.Parameters.AddWithValue("@date", finalDiagnosis.Date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@time", finalDiagnosis.Time);
                    cmd.ExecuteNonQuery();
                }

                var selectQuery = "select LAST_INSERT_ID()";

                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                {
                    MySqlDataReader lastIndexReader = selectCommand.ExecuteReader();
                    lastIndexReader.Read();
                    var recordId = lastIndexReader.GetInt32(0);

                    finalDiagnosis.RecordId = recordId;
                    RecordDiagnosisManager.RecordedDiagnoses.Add(finalDiagnosis);
                }

                conn.Close();               
            }
        }
    }
}
