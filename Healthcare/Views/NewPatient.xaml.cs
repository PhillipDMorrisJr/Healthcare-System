using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

        private void validate()
        {
            this.validation.Text +=  "Please Address the following:\n";
            if (string.IsNullOrEmpty(fname.Text))
            {
                this.validation.Text += "Please enter a valid first name\n";
                this.fname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (string.IsNullOrEmpty(lname.Text))
            {
                this.validation.Text += "Please enter a valid last name\n";
                this.lname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (string.IsNullOrEmpty(street.Text))
            {
                this.validation.Text += "Please enter a valid street\n";
                this.street.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (string.IsNullOrEmpty(this.ssn.Password))
            {
                this.validation.Text += "Please enter a valid ssn in the following format: xxxxxxxxx\n";
                this.street.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            if (string.IsNullOrEmpty(this.phone.Text))
            {
                this.validation.Text += "Please enter a valid phone number in the following format: xxxxxxxxxx\n";
                this.street.BorderBrush = new SolidColorBrush(Colors.Red);

            }

            if (this.bday.Date > DateTimeOffset.Now)
            {
                this.validation.Text += "Patient's birthday must before the current day\n";
                this.bday.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (this.genderCmbox.SelectedItem == null)
            {
                this.validation.Text += "Must select a gender\n";
                this.genderCmbox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void createPatient_onClick(object sender, RoutedEventArgs e)
        {
            this.validation.Text = "";
            string ssn = this.ssn.Password;
            string firstName = this.fname.Text;
            string lastName = this.lname.Text;
            string phone = this.phone.Text;
            DateTime dateOfBirth = this.bday.Date.DateTime;

            string gender = string.Empty;
            
            var genderCmboxSelectedItem = this.genderCmbox.SelectedItem;
            gender = genderCmboxSelectedItem?.ToString();
            

            string street = this.street.Text;
            string state = this.state.Text;
            string zip = this.zip.Text;

            try
            {
                string fullAddress = street + ", " + state + ", " + zip;
                RegistrationUtility.CreateNewPatient(Convert.ToInt32(ssn), firstName, lastName, phone, dateOfBirth,
                    gender, fullAddress);
                this.Frame.Navigate(typeof(MainPage));
            }
            catch (Exception)
            {
                this.validate();
            }
        }

        private void home_onClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
