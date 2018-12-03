using System;
using System.Data;
using Healthcare.DAL;

namespace Healthcare.Utils
{
    public static class CustomQuery
    {
        public static DataTable RetrieveResults(string query)
        {
            var dataTable = QueryDAL.GetResults(query);
            return dataTable;
        }

        public static DataTable RetrieveResultsBetweenDates(DateTimeOffset beginDate, DateTimeOffset endDate)
        {
            var dataTable = QueryDAL.GetResultsBetweenDates(beginDate, endDate);
            return dataTable;
        }
    }
}