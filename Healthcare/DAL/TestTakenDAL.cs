using System;
using System.Collections.Generic;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class TestTakenDAL
    {
        public static List<TestTaken> GetTestTaken()
        {
            var testsTaken = new List<TestTaken>();

            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from testsTaken";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var orderId = reader.GetInt32((int) Attributes.OrderId);
                            var isTaken = reader.GetBoolean((int) Attributes.IsTaken);
                            var date = reader.GetDateTime((int) Attributes.Date);
                            var time = (TimeSpan) reader["time"];

                            var taken = new TestTaken(orderId, isTaken, date, time);
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

        private enum Attributes
        {
            OrderId = 1,
            IsTaken = 2,
            Date = 3
        }
    }
}