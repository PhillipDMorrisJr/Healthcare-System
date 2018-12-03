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
    public static class DiagnosisDAL
    {
        private enum Attributes
        {
            DiagnosisId = 0, CuId = 1, DoctorId = 2, Date = 3, Diagnosis = 5, IsFinalDiagnosis = 6
        }

        public static List<Diagnosis> GetDiagnoses()
        {
            var diagnoses = new List<Diagnosis>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from doctorDiagnosis";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return diagnoses;

                    while (reader.Read())
                    {
                        var diagnosisId = reader.GetInt32((int) Attributes.DiagnosisId);
                        var cuId = reader.GetInt32((int) Attributes.CuId);
                        var doctorId = reader.GetString((int) Attributes.DoctorId);
                        var date = reader.GetDateTime((int) Attributes.Date);
                        var time = (TimeSpan) reader["time"];
                        var diagnosis = reader.GetString((int) Attributes.Diagnosis);
                        var isFinalDiagnosis = reader.GetBoolean((int) Attributes.IsFinalDiagnosis);

                        var newDiagnosis = new Diagnosis(cuId, doctorId, date, time, diagnosis, isFinalDiagnosis)
                            {DiagnosisId = diagnosisId};
                        diagnoses.Add(newDiagnosis);                       
                    }
                }
            }
            return diagnoses;
        }

        public static void AddDiagnosis(Diagnosis diagnosis)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `doctorDiagnosis` (`cuID`, `doctorID`, `date`, `time`, `diagnosis`,`isFinalDiagnosis`) VALUES (@cuID, @doctorID, @date, @time, @diagnosis, @isFinalDiagnosis)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@cuID", diagnosis.CuId);
                        cmd.Parameters.AddWithValue("@doctorID", diagnosis.DoctorId);
                        cmd.Parameters.AddWithValue("@date", diagnosis.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@time", diagnosis.Time);
                        cmd.Parameters.AddWithValue("@diagnosis", diagnosis.CheckupDiagnosis);
                        cmd.Parameters.AddWithValue("@isFinalDiagnosis", diagnosis.IsFinalDiagnosis);
                        cmd.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = selectCommand.ExecuteReader();
                        lastIndexReader.Read();
                        var diagnosisId = lastIndexReader.GetInt32(0);

                        diagnosis.DiagnosisId = diagnosisId;
                        DiagnosisManager.DoctorDiagnoses.Add(diagnosis);
                    }

                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
            }
        }
    }
}
