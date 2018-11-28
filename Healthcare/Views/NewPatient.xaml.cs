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
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            List<string> genders = new List<string> {"Male", "Female"};
            this.genderCmbox.ItemsSource = genders;
            this.genderCmbox.SelectedItem = "Male";
        }

        private void createPatient_onClick(object sender, RoutedEventArgs e)
        {
            string ssn = this.ssn.Password;
            string firstName = this.fname.Text;
            string lastName = this.lname.Text;
            string phone = this.phone.Text;
            DateTime dateOfBirth = this.bday.Date.DateTime;

            string gender = string.Empty;
            
            var genderCmboxSelectedItem = this.genderCmbox.SelectedItem;
            if (genderCmboxSelectedItem != null)
            {
                gender = genderCmboxSelectedItem.ToString();
            }

            string street = this.street.Text;
            string state = this.state.Text;
            string zip = this.zip.Text;

            bool isTenDigit = phone.Length == 10;
            bool isSsnNineDigit = ssn.Length == 9;

            if (!(string.IsNullOrWhiteSpace(street) && string.IsNullOrWhiteSpace(state) && string.IsNullOrWhiteSpace(zip) && string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone)) && isTenDigit && isSsnNineDigit)
            {
                string fullAddress = street + ", " + state + ", " + zip;
                RegistrationUtility.CreateNewPatient(Convert.ToInt32(ssn), firstName, lastName, phone, dateOfBirth, gender, fullAddress);               
            }

            this.Frame.Navigate(typeof(MainPage));
        }

        private void home_onClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
