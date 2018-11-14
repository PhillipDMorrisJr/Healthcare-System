using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    class SymptomDAL
    {
        public static List<Symptom> GetSymptoms()
        {
            var symptoms = new List<Symptom>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from symptoms";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return symptoms;

                    while (reader.Read())
                    {
                        var id = (uint) reader["sid"];
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
