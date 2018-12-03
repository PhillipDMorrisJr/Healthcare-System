using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.DAL;

namespace Healthcare.Utils
{
    public static class CustomQuery
    {
        public static DataTable RetrieveResults(string query)
        {
            DataTable dataTable = QueryDAL.GetResults(query);
            return dataTable;
        }

        public static DataTable RetrieveResultsBetweenDates(DateTimeOffset beginDate, DateTimeOffset endDate)
        {
            DataTable dataTable = QueryDAL.GetResultsBetweenDates(beginDate, endDate);
            return dataTable;
        }
    }
}
