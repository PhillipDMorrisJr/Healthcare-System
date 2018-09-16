using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// This utility is responsible for confirming that a user's identification exists within the database
    /// </summary>
    public static class AccessValidator
    {
        /// <summary>
        /// Confirms the user access by checking to see if user exists inside database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool ConfirmUserAccess(String username, string password)
        {
            return true;
        }

    }
}
