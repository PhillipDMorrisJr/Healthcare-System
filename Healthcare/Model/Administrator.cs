using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    class Administrator : User
    {
        
        public Administrator(string username, string password, int id) : base(username, password, id)
        {

        }
    }
}
