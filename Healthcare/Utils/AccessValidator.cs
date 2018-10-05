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
        
        private static List<Nurse> nurses = new List<Nurse>()
        {
            (new Nurse("Caviaye", "Password0")),
            (new Nurse("Dr. Yang", "Password")),
            (new Nurse("1", "1")),
            (new Nurse("2", "2")),
            (new Nurse("2", "2")),
        };


        private static List<Administrator> admins = new List<Administrator>()
        {
            new Administrator("Phillip", "Phillycheeze101"),
            new Administrator("3", "3"),
        };

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
            bool validInput = ValidateThatUsernameAndPasswordAreNotEmpty(username, password);
            Nurse nurse = nurses.Find(user => user.Username == username && user.Password == password);
            Administrator administrator = admins.Find(user => user.Username == username && user.Password == password);
            CurrentUser = administrator ?? (User) nurse;

            bool isValid = !IsUserNull() && validInput;

            access = !IsUserNull() ? CurrentUser.GetType().Name : null;

            return isValid;

        }

        private static bool IsUserNull()
        {
            bool isUserNull = CurrentUser == null;
            return isUserNull;
        }

        private static bool ValidateThatUsernameAndPasswordAreNotEmpty(string username, string password)
        {
            return !(string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password));
        }
    }
}
