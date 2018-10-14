using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.Utils
{
    public static class DatabaseManager
    {
        public static void AddPatient(Patient newPatient)
        {
            const string connStr = "server=160.10.25.16;port=3306;uid=cs3230f18i;pwd=OIfaWXx0jaYtAHMs;database=cs3230f18i;";
 
            using (var connection = new MySqlConnection(connStr))  
            {  
                connection.Open();

                var cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText =
                        "INSERT INTO 'patients' ('patientID', 'firstName', 'lastName', 'birthDate', 'phoneNumber') VALUES (?patientID, ?firstName, ?lastName, ?birthDate, ?phoneNumber)"
                };

                cmd.Parameters.AddWithValue("?firstName", MySqlDbType.VarChar).Value = newPatient.FirstName;
                cmd.Parameters.AddWithValue("?lastName", MySqlDbType.VarChar).Value = newPatient.LastName;
                cmd.Parameters.AddWithValue("?birthDate", MySqlDbType.DateTime).Value = newPatient.Dob;
                cmd.Parameters.AddWithValue("?phoneNumber", MySqlDbType.VarChar).Value = newPatient.Phone;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
