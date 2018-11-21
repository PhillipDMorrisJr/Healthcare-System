using System;
using System.Collections.Generic;
using Windows.UI.Notifications;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    /// <summary>
    /// This utility is responsible for confirming that a user's identification exists within the database
    /// World
    /// Hello
    /// </summary>
    public static class AccessValidator
    {
        
        public static User CurrentUser;
        private static String access;
        public static String Access => access;
        

        /// <summary>
        /// Confirms the user access by checking to see if user exists inside database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool ConfirmUserAccess(String username, string password)
        {
            bool validInput = isInputNullOrEmpty(username, password);
            CurrentUser = UserDAL.GetUser(username, password);

            bool isValid = !IsUserNull() && validInput;

            access = !IsUserNull() ? CurrentUser.GetType().Name : null;

            return isValid;

        }

        private static bool IsUserNull()
        {
            bool isUserNull = CurrentUser == null;
            return isUserNull;
        }

        private static bool isInputNullOrEmpty(string username, string password)
        {
            return !(string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password));
        }





    }
}
