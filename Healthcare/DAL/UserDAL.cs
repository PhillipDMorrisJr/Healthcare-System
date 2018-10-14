using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    static class UserDAL
    {
        private enum Attributes
        {
            Userid = 0, Username, Isadmin, Password 
        }
        public static User GetUser(string username, string password)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {

                    conn.Open();
                    var selectQuery = "select * from User WHERE username= @username AND password= @password";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.Add("username", (DbType) MySqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("password", (DbType) MySqlDbType.VarChar).Value = password;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int id  = reader.GetInt32((int)Attributes.Userid);
                            if (reader.GetBoolean((int)Attributes.Isadmin))
                            {
                                Administrator admin = new Administrator(username, password, id);
                                conn.Close();
                                return admin;
                            }
                            else
                            {
                                Nurse nurse = new Nurse(username, password, id);
                                conn.Close();
                                return nurse;
                            }
                            

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
    }
}
