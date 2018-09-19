using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Healthcare.Model;
using Healthcare.Utils;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Healthcare
{
    public sealed partial class RegistrationPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPage"/> class.
        /// </summary>
        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the register control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void register_Click(object sender, RoutedEventArgs e)
        {
            string fname = this.FirstName.Text;
            string lname = this.LastName.Text;
            string phone = this.Phone.Text;
            DateTime date = this.AppointmentDate.Date.DateTime;
            TimeSpan time = this.AppointmentTime.Time;
            
            if(!(string.IsNullOrWhiteSpace(fname) && string.IsNullOrWhiteSpace(lname) && string.IsNullOrWhiteSpace(phone))) {
                Patient patient = new Patient(fname, lname, phone, date, time);
                RegistrationUtility.CreateNewPatient(patient);
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
