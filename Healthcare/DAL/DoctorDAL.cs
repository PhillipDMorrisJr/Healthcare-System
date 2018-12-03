using System.Collections.Generic;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    internal class DoctorDAL
    {
        public static List<Doctor> GetDoctors()
        {
            var doctors = new List<Doctor>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from doctors";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return doctors;

                    while (reader.Read())
                    {
                        var id = (string) reader["doctorID"];
                        var firstName = (string) reader["firstName"];
                        var lastName = (string) reader["lastName"];

                        var newDoctor = new Doctor(id, firstName, lastName);
                        doctors.Add(newDoctor);
                    }
                }
            }

            return doctors;
        }
    }
}