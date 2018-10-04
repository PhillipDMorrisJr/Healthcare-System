using System;
using System.Collections.Generic;
using Windows.UI.Notifications;
using Healthcare.Model;

namespace Healthcare.Utils
{
    /// <summary>
    /// This utility is responsible for confirming that a user's identification exists within the database
    /// </summary>
    public static class AccessValidator
    {
        
        private static List<User> users = new List<User>()
        {
            (new User("Phillip", "Phillycheeze101")),
            (new User("Caviaye", "Password0")),
            (new User("Dr. Yang", "Password")),
            (new User("1", "1")),
            (new User("2", "2")),
            (new User("2", "2")),
        };

        public static User CurrentUser;

        /// <summary>
        /// Confirms the user access by checking to see if user exists inside database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool ConfirmUserAccess(String username, string password)
        {
            bool validInput = ValidateThatUsernameAndPasswordAreNotEmpty(username, password);
            CurrentUser = users.Find(user => user.Username == username && user.Password == password);
            bool isValid = CurrentUser != null && validInput;

            return isValid;

        }

        private static bool ValidateThatUsernameAndPasswordAreNotEmpty(string username, string password)
        {
            return !(string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password));
        }
    }
}
