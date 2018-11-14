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
    public static class CheckUpDAL
    {
        public static CheckUp AddCheckUp(CheckUp details)
        {
            try
            {
                int cuid;
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    
                    var insertQuery =
                        "INSERT INTO `checkup` (`appointmentID`, `systolic`, `diastolic`, `pID`, `temperature`, `arrivalTime`, `nurseID`, `weight`, `pulse`) VALUES (@apptID, @systolic, @diastolic, @pID, @temp, @arrival, @nurseID, @weight, @pulse)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@apptID", details.Appointment.ID);
                        command.Parameters.AddWithValue("@systolic", details.Systolic);
                        command.Parameters.AddWithValue("@diastolic", details.Diastolic);
                        command.Parameters.AddWithValue("@pID", details.Patient.Id);
                        command.Parameters.AddWithValue("@arrival", details.ArrivalTime.ToString());
                        command.Parameters.AddWithValue("@temp", details.Temperature);
                        command.Parameters.AddWithValue("@nurseID", details.Nurse.Id);
                        command.Parameters.AddWithValue("@weight", details.Weight);
                        command.Parameters.AddWithValue("@pulse", details.Pulse);
                        command.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = selectCommand.ExecuteReader();
                        lastIndexReader.Read();
                        cuid = lastIndexReader.GetInt32(0);

                    }

                    conn.Close();
                }

                foreach (Symptom symptom in details.Symptoms)
                {
                    using (MySqlConnection conn = DbConnection.GetConnection())
                    {
                        conn.Open();
                        var insertQuery = "INSERT INTO `checkupSymptoms` (`cuid`, `sid`) VALUES (@cuid, @sid)";

                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn))
                        {
                            insertCommand.Parameters.AddWithValue("@cuid", cuid);
                            insertCommand.Parameters.AddWithValue("@sid", symptom.ID);
                            insertCommand.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var checkInQuery =
                        "UPDATE `appointments` SET `isCheckedIn` = @status WHERE appointmentID = @aID AND patientID = @pID";
                    using (MySqlCommand checkInCommand = new MySqlCommand(checkInQuery, conn))
                    {

                        checkInCommand.Parameters.AddWithValue("@status", 1);
                        checkInCommand.Parameters.AddWithValue("@aID", details.Appointment.ID);
                        checkInCommand.Parameters.AddWithValue("@pID", details.Patient.Id);
                        checkInCommand.ExecuteNonQuery();

                    }

                    conn.Close();
                }

                return details;

            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
                return null;
            }
        }

    }
}
