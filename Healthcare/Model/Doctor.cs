namespace Healthcare.Model
{
    /// <summary>
    /// </summary>
    public class Doctor
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Doctor" /> class.
        /// </summary>
        /// <param name="id">The doctor id.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        public Doctor(string id, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
        }

        /// <summary>
        ///     Gets the full name.
        /// </summary>
        /// <value>
        ///     The full name.
        /// </value>
        public string FullName => "Dr. " + FirstName + " " + LastName;

        /// <summary>
        ///     Gets the first name.
        /// </summary>
        /// <value>
        ///     The first name.
        /// </value>
        public string FirstName { get; }

        /// <summary>
        ///     Gets the last name.
        /// </summary>
        /// <value>
        ///     The last name.
        /// </value>
        public string LastName { get; }

        /// <summary>
        ///     Gets the doctor id.
        /// </summary>
        /// <value>
        ///     The doctor id.
        /// </value>
        public string Id { get; }
    }
}