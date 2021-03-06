﻿using System;
using System.Collections.Generic;
using Healthcare.Model;
using Healthcare.Utils;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class TestResultDAL
    {
        public static void AddTestResult(TestResult result)
        {
            try
            {
                var orderId = result.OrderId;
                var reading = result.Readings;
                var date = result.Date;
                var time = result.Time;

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertResultQuery =
                        "INSERT INTO `results` (`orderID`, `date`, `time`, `testReadings`) VALUE (@orderID, @date, @time, @testReadings)";

                    using (var cmd = new MySqlCommand(insertResultQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", orderId);
                        cmd.Parameters.AddWithValue("@testReadings", reading);
                        cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (var selectCommand = new MySqlCommand(selectQuery, conn))
                    {
                        var lastIndexReader = selectCommand.ExecuteReader();
                        lastIndexReader.Read();
                        var resultId = lastIndexReader.GetInt32(0);

                        result.ResultId = resultId;
                        TestResultManager.Results.Add(result);
                    }

                    conn.Close();
                }

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var updateQuery =
                        "UPDATE `testsTaken` SET isTestTaken = @isTestTaken, date = @date, time = @time WHERE orderID = @orderID";
                    using (var cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", orderId);
                        cmd.Parameters.AddWithValue("@isTestTaken", true);
                        cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.ExecuteReader();
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

        public static List<TestResult> GetTestResults()
        {
            var results = new List<TestResult>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var selectQuery = "select * from results";
                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var resultId = reader.GetInt32((int) Attributes.ResultId);
                        var id = reader.GetInt32((int) Attributes.OrderId);
                        var date = reader.GetDateTime((int) Attributes.Date);
                        var time = (TimeSpan) reader["time"];
                        var reading = reader.GetBoolean((int) Attributes.Reading);

                        var result = new TestResult(id, date, time, reading) {ResultId = resultId};
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        private enum Attributes
        {
            ResultId = 0,
            OrderId = 1,
            Date = 2,
            Reading = 4
        }
    }
}