using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Healthcare.Utils;

namespace Healthcare.Model
{
    public class User
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get;}
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string ID { get; set; }


        /// <summary>
        /// Gets or sets the user image.
        /// </summary>
        /// <value>
        /// The user image.
        /// </value>
        public Image UserImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="id"> the id</param>
        public User(string username, string password, int id)
        {
            this.Username = username;
            this.Password = password;
            this.ID = id.ToString().PadLeft(7, '0');;
        }
    }
}
