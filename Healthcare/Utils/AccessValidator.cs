using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    /// <summary>
    ///     This utility is responsible for confirming that a user's identification exists within the database
    ///     World
    ///     Hello
    /// </summary>
    public static class AccessValidator
    {
        public static User CurrentUser;
        public static string Access { get; private set; }


        /// <summary>
        ///     Confirms the user access by checking to see if user exists inside database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool ConfirmUserAccess(string username, string password)
        {
            var validInput = isInputNullOrEmpty(username, password);
            CurrentUser = UserDAL.GetUser(username, password);

            var isValid = !IsUserNull() && validInput;

            Access = !IsUserNull() ? CurrentUser.GetType().Name : null;

            return isValid;
        }

        private static bool IsUserNull()
        {
            var isUserNull = CurrentUser == null;
            return isUserNull;
        }

        private static bool isInputNullOrEmpty(string username, string password)
        {
            return !(string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password));
        }
    }
}