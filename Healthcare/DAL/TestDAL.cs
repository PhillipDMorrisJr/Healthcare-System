using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class TestDAL
    {
        public static List<Test> GetTests()
        {
            var tests = new List<Test>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from test";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return tests;

                    while (reader.Read())
                    {
                        var code = Convert.ToInt32(reader["code"]);
                        var name = (string) reader["name"];

                        var newTest = new Test(code, name);
                        tests.Add(newTest);                       
                    }
                }
            }
            return tests;
        }
    }
}
