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
    public static class TestOrderDAL
    {
        private enum Attributes
        {
            OrderId = 0, CuId = 1, Code = 2, Date = 3, DoctorId = 5
        }


        public static List<Order> GetTestOrders()
        {
            var orders = new List<Order>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from testOrder";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return orders;

                    while (reader.Read())
                    {
                        var orderId = reader.GetInt32((int) Attributes.OrderId);
                        var cuId = reader.GetInt32((int) Attributes.CuId);
                        var code = reader.GetInt32((int) Attributes.Code);
                        var date = reader.GetDateTime((int) Attributes.Date);
                        var doctorId = reader.GetString((int) Attributes.DoctorId);
                        var time = (TimeSpan) reader["time"];

                        var newOrder = new Order(cuId, code, date, time, doctorId) {OrderId = orderId};
                        orders.Add(newOrder);                       
                    }
                }
            }
            return orders;
        }

        public static void AddTestOrders(List<Order> orders)
        {
            foreach (var order in orders)
            {
                handleAddOrder(order);
            }           
        }

        private static void handleAddOrder(Order order)
        {
            try
            {
                int currentOrderId;

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `testOrder` (`cuID`, `code`, `date`, `time`, `doctorID`) VALUES (@cuID, @code, @date, @time, @doctorID)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@cuID", order.CuId);
                        cmd.Parameters.AddWithValue("@code", order.Code);
                        cmd.Parameters.AddWithValue("@date", order.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@time", order.Time);
                        cmd.Parameters.AddWithValue("@doctorID", order.DoctorId);
                        cmd.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = selectCommand.ExecuteReader();
                        lastIndexReader.Read();
                        currentOrderId = lastIndexReader.GetInt32(0);

                        order.OrderId = currentOrderId;
                        TestOrderManager.Orders.Add(order);
                    }

                    conn.Close();               
                }

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    TestTaken taken;

                    var date = new DateTime();
                    var time = new TimeSpan();

                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `testsTaken` (`orderID`, `isTestTaken`, `date`, `time`) VALUES (@orderID, @isTestTaken, @date, @time)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", currentOrderId);
                        cmd.Parameters.AddWithValue("@isTestTaken", false);
                        cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.ExecuteNonQuery();

                        taken = new TestTaken(currentOrderId, false, date, time);
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = selectCommand.ExecuteReader();
                        lastIndexReader.Read();
                        var testTakenId = lastIndexReader.GetInt32(0);

                        taken.TakenId = testTakenId;
                        TestTakenManager.TestsTaken.Add(taken);
                    }

                    conn.Close();               
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
            } 
        }
    }
}
