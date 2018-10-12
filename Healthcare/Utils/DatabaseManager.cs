using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class DatabaseManager
    {
        private const string ConnectionString = "server=160.10.25.16;port=3306;database=cs3230f18i;uid=cwilli82;pwd=NPD2i:0wO@;";

        public static void AddPatient(Patient newPatient)
        {
            using (var connection = new SqlConnection(ConnectionString))  
            {  
                connection.Open();

                var queryString =
                    "INSERT INTO patients (patientID, firstName, lastName, birthDate, phoneNumber) VALUES (NULL, " +
                    newPatient.FirstName + ", " + newPatient.LastName + ", " + newPatient.Dob + ", " +
                    newPatient.Phone + ")";

                var command = new SqlCommand(queryString, connection);

                command.ExecuteNonQuery();

                connection.Close();
            }  
        }
    }
}
