﻿namespace Healthcare.Model
{
    public class Address
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Address" /> class.
        /// </summary>
        /// <param name="street">The street.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        public Address(string street, string state, int zip)
        {
            Street = street;
            State = state;
            Zip = zip;
        }

        /// <summary>
        ///     Gets or sets the AddressId.
        /// </summary>
        /// <value>
        ///     The AddressId.
        /// </value
        public int AddressId { get; set; }

        /// <summary>
        ///     Gets or sets the Street.
        /// </summary>
        /// <value>
        ///     The Street.
        /// </value>
        public string Street { get; set; }

        /// <summary>
        ///     Gets or sets the State.
        /// </summary>
        /// <value>
        ///     The State.
        /// </value>
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets the Zip.
        /// </summary>
        /// <value>
        ///     The Zip.
        /// </value>
        public int Zip { get; set; }
    }
}