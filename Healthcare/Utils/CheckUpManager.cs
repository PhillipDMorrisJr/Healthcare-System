using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class CheckUpManager
    {
        /// <summary>
        /// Executes check up with the specified details.
        /// </summary>
        /// <param name="details">The check up details.</param>
        /// <returns></returns>
        public static CheckUp Execute(CheckUp details)
        {
            return CheckUpDAL.AddCheckUp(details);
        }
    }
}
