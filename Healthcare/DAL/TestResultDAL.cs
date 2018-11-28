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
    public static class TestResultDAL
    {
        private enum Attributes
        {
            PatientId = 0, Code = 2, TestReadings = 4, Diagnosis = 5
        }

        public static void AddTestResult(TestResult result)
        {
            try
            {
                var pId = result.PatientId;
                var apptId = result.AppointmentId;
                var code = result.Code;
                var time = result.Time;
                var testReadings = result.Readings;
                var diagnosis = result.Diagnosis;

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertResultQuery =
                        "INSERT INTO `results` (`patientID`, `apptID`, `code`, `time`, `testReadings`, `diagnosis`) VALUE (@patientID, @apptID, @code, @time, @testReadings, @diagnosis)";

                    using (MySqlCommand cmd = new MySqlCommand(insertResultQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@patientID", pId);
                        cmd.Parameters.AddWithValue("@apptID", apptId);
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.Parameters.AddWithValue("@testReadings", testReadings);
                        cmd.Parameters.AddWithValue("@diagnosis", diagnosis);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var updateQuery =
                        "UPDATE `appointments` SET testTaken = @testTaken WHERE appointmentID = @apptID";
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@testTaken", 1);
                        cmd.Parameters.AddWithValue("@apptID", apptId);
                        cmd.ExecuteNonQuery();
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

        public static TestResult GetTestResult(int apptId)
        {
            try
            {
                TestResult result = null;

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from results WHERE apptID = @apptID";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@apptID", apptId);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {                         
                            var pId = reader.GetInt32((int) Attributes.PatientId);
                            var code = reader.GetInt32((int) Attributes.Code);
                            var time = (TimeSpan) reader["time"];
                            var testReadings = reader.GetBoolean((int) Attributes.TestReadings);
                            var diagnosis = reader.GetString((int) Attributes.Diagnosis);

                            result = new TestResult(pId, apptId, code, time, testReadings, diagnosis);                          
                        }
                        conn.Close();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }
    }
}
