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
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    int cuid;
                    var insertQuery =
                        "INSERT INTO `checkup` (`appointmentID`, `systolic`, `diastolic`, `pID`, `temperature`, `arrivalTime`, `nurseID`, `weight`, `pulse`) VALUES (@apptID, @systolic, @diastolic, @pID, @temp, @arrival, @nurseID, @weight, @pulse)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@apptID", details.Appointment.ID);
                        command.Parameters.AddWithValue("@systolic", details.Systolic);
                        command.Parameters.AddWithValue("@diastolic", details.Diastolic);
                        command.Parameters.AddWithValue("@pID", details.Patient.Id);
                        command.Parameters.AddWithValue("@temp", details.Temperature);
                        command.Parameters.AddWithValue("@nurseID", details.Nurse);
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

                    insertQuery = "INSERT INTO `checkupSymptoms` (`cuid`, `sid`) VALUES (@cuid, @sid)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn))
                    {
                        insertCommand.Parameters.AddWithValue("@cuid", cuid);
                        foreach (Symptom symptom in details.Symptoms)
                        {
                            insertCommand.Parameters.AddWithValue("@sid", symptom.ID);
                            insertCommand.ExecuteNonQuery();
                        }
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
