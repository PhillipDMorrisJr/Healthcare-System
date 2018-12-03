using System;
using System.Data;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    internal static class UserDAL
    {
        public static User GetUser(string username, string password)
        {
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery =
                        "select * from User WHERE username= @username AND password= ENCRYPT( @password , \"salt\")";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.Add("username", (DbType) MySqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("password", (DbType) MySqlDbType.VarChar).Value = password;
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var id = reader.GetInt32((int) Attributes.Userid);
                            if (reader.GetBoolean((int) Attributes.IsAdmin))
                            {
                                var admin = new Administrator(username, password, id);
                                conn.Close();
                                return admin;
                            }

                            var nurse = new Nurse(username, password, id);
                            conn.Close();
                            return nurse;
                        }

                        conn.Close();
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        private enum Attributes
        {
            Userid = 0,
            IsAdmin = 2
        }
    }
}