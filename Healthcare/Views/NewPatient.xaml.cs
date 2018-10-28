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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPatient : Page
    {
        public NewPatient()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.ID;
            this.accessType.Text = AccessValidator.Access;
        }

        private void createPatient_onClick(object sender, RoutedEventArgs e)
        {
            string fname = this.fname.Text;
            string lname = this.lname.Text;
            string phone = this.phone.Text;
            string ssn = this.ssn.Text;
            string address = this.address.Text + ", " + this.state.Text + " " + this.zip.Text;
            DateTime dateOfBirth = this.bday.Date.DateTime;

            bool isPhoneTenDigit = phone.Length == 10;
            bool isSSNNineDigit = ssn.Length == 9;

            if(!(string.IsNullOrWhiteSpace(fname) && string.IsNullOrWhiteSpace(lname) && string.IsNullOrWhiteSpace(phone)) && isPhoneTenDigit && isSSNNineDigit) {
            RegistrationUtility.CreateNewPatient(fname, lname, phone, dateOfBirth,ssn,address);

            this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
