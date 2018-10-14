using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class DbConnection
    {
        public static MySqlConnection GetConnection()
        {
            string conStr = "server=160.10.25.16; port=3306; uid=cs3230f18i;" +
                            "pwd=OIfaWXx0jaYtAHMs;database=cs3230f18i;";
            return new MySqlConnection(conStr);
        }
    }
}

