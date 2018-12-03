using System.Collections.Generic;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    internal class SymptomDAL
    {
        public static List<Symptom> GetSymptoms()
        {
            var symptoms = new List<Symptom>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from symptoms";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return symptoms;

                    while (reader.Read())
                    {
                        var id = (uint) reader["sID"];
                        var name = (string) reader["name"];

                        var newSymptom = new Symptom(id, name);
                        symptoms.Add(newSymptom);
                    }
                }
            }

            return symptoms;
        }
    }
}