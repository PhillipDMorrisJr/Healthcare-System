using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class QueryDAL
    {
        public static DataTable GetResults(string query)
        {

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {

                        DataTable dataTable = new DataTable();

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
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "";
                    using (var cmd = new MySqlCommand(query, conn))
                    {

                        DataTable dataTable = new DataTable();

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