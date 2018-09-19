using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class User
    {
        public string Username { get;}
        public string Password { get; }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
