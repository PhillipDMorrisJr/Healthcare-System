using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    class Nurse : User
    {
        public Nurse(string username, string password, int id) : base(username, password, id)
        {
        }
    }
}
