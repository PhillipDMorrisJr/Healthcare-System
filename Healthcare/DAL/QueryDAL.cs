using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class QueryDAL
    {
        public static DataTable GetResults(string query)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    var dataTable = new DataTable();

                    using (var dataAdapter = new MySqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public static DataTable GetResultsBetweenDates(DateTimeOffset beginDate, DateTimeOffset endDate)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var query =
                    "SELECT DATE(`checkup`.arrivalDate) as \"Arrival Date\", `patients`.`patientID` as \"Patient ID\", Concat(`patients`.`firstName`, \" \", `patients`.`lastName`) as Patient, " +
                    "Concat(`doctors`.`firstName`, \" \", `doctors`.`lastName`) as Doctor, `User`.username as Nurse,  doctorDiagnosis.diagnosis as Diagnosis, test.name as \"Test Name\", " +
                    "Case when `results`.`testReadings` THEN \"Positive\" ELSE \"Negative\" END as Reading " +
                    "FROM `patients`, checkup, doctorDiagnosis, results, doctors, `User`, testOrder, test WHERE `patients`.patientID = `checkup`.`pID` AND `checkup`.`nurseID` = `User`.`userID` " +
                    "AND doctorDiagnosis.doctorID = doctors.doctorID AND checkup.cuID = testOrder.cuID AND testOrder.cuID = checkup.cuID AND test.code = testOrder.code AND results.orderID = testOrder.orderID AND" +
                    "`checkup`.arrivalDate > @beginDate AND `checkup`.arrivalDate < @endDate Order by `checkup`.arrivalDate, `patients`.lastName";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@beginDate", beginDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
                    var dataTable = new DataTable();

                    using (var dataAdapter = new MySqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }
    }
}