using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class TestTakenDAL
    {
        private enum Attributes
        {
            OrderId = 1, IsTaken = 2, Date = 3
        }

        public static List<TestTaken> GetTestTaken()
        {
            List<TestTaken> testsTaken = new List<TestTaken>();

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from testsTaken";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var orderId = reader.GetInt32((int)Attributes.OrderId);
                            var isTaken = reader.GetBoolean((int)Attributes.IsTaken);
                            var date = reader.GetDateTime((int) Attributes.Date);
                            var time = (TimeSpan) reader["time"];

                            TestTaken taken = new TestTaken(orderId, isTaken, date, time);
                            testsTaken.Add(taken);
                        }
                        conn.Close();
                        return testsTaken;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return testsTaken;
            }
        }
    }
}
