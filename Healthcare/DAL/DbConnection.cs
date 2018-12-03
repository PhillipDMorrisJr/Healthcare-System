using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class DbConnection
    {
        public static MySqlConnection GetConnection()
        {
            var conStr = "server=160.10.25.16; port=3306; uid=cs3230f18i;" +
                         "pwd=OIfaWXx0jaYtAHMs;database=cs3230f18i;";
            return new MySqlConnection(conStr);
        }
    }
}