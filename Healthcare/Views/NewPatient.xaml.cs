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
        private string phoneNumber;
        private string socialSecurityNumber;
        public NewPatient()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;
            
            List<string> genders = new List<string> {"Male", "Female"};
            this.state.ItemsSource = States.GetStates();
            this.state.SelectedItem = "AL";
            this.genderCmbox.ItemsSource = genders;
            this.genderCmbox.SelectedItem = "Male";
            
        }



        
        private void validate()
        {
            this.validation.Text +=  "Please Address the following:\n";
            validateFirstName();
            validateLastName();
            validateStreet();
            validateSSN();
            validatePhone();
            validateDate();
            validateZip();
        }

        private void validateZip()
        {
            if (string.IsNullOrEmpty(this.zip.Text) || this.zip.Text.Length != 5)
            {
                this.validation.Text += "Enter valid 5 digit zip in the following format: xxxxx\n";
                this.zip.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.zip.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateDate()
        {
            if (this.bday.Date > DateTimeOffset.Now)
            {
                this.validation.Text += "Patient's birthday must before the current day\n";
                this.bday.Background = new SolidColorBrush(Colors.MistyRose);
            }
            else
            {
                this.bday.Background = new SolidColorBrush(Colors.Azure);
            }
        }

        private void validatePhone()
        {
            int validNumberOfDigits = 10;
            
            bool isNumber = long.TryParse(this.phoneNumber, out long result);
            if (string.IsNullOrEmpty(this.phoneNumber) || this.phoneNumber.Length != validNumberOfDigits || !isNumber)
            {
                this.validation.Text += "Please enter a valid 10 digit phone number in the following format: xxx-xxx-xxxx\n";
                this.phone.BorderBrush = new SolidColorBrush(Colors.Red);
                this.phone1.BorderBrush = new SolidColorBrush(Colors.Red);
                this.phone2.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.phone.Text = this.phoneNumber.Substring(0, 3);
                this.phone1.Text = this.phoneNumber.Substring(3, 3);
                this.phone2.Text = this.phoneNumber.Substring(6, 4);
                this.phone.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                this.phone1.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                this.phone2.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateSSN()
        {
            int validNumberOfDigits = 9;
            bool isNumber = int.TryParse(this.socialSecurityNumber, out int result);
            if (string.IsNullOrEmpty(this.socialSecurityNumber) || this.socialSecurityNumber.Length != validNumberOfDigits || !isNumber)
            {
                this.validation.Text += "Please enter a valid 9 digit ssn in the following format: xxx-xx-xxxx\n";
                this.ssn.BorderBrush = new SolidColorBrush(Colors.Red);
                this.ssn1.BorderBrush = new SolidColorBrush(Colors.Red);
                this.ssn2.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.ssn.Password = this.socialSecurityNumber.Substring(0, 3);
                this.ssn1.Password = this.socialSecurityNumber.Substring(3, 2);
                this.ssn2.Password = this.socialSecurityNumber.Substring(5, 4);
                this.ssn.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                this.ssn1.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                this.ssn2.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateStreet()
        {
            if (string.IsNullOrEmpty(street.Text))
            {
                this.validation.Text += "Please enter a valid street\n";
                this.street.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.street.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateLastName()
        {
            if (string.IsNullOrEmpty(lname.Text))
            {
                this.validation.Text += "Please enter a valid last name\n";
                this.lname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.lname.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateFirstName()
        {
            if (string.IsNullOrEmpty(fname.Text))
            {
                this.validation.Text += "Please enter a valid first name\n";
                this.fname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.fname.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private bool isValid()
        {
            int validZip = 5;
            int validSSN = 9;
            int validPhone = 10;
            bool isPhoneNumber = long.TryParse(this.phoneNumber, out long result);
            bool isSSNNumber = int.TryParse(this.ssn.Password, out int result1);
            bool isZipNumber = int.TryParse(this.zip.Text, out int result2);
            return (!string.IsNullOrEmpty(this.zip.Text) && isPhoneNumber && isSSNNumber && isZipNumber &&
                   this.zip.Text.Length == validZip && this.bday.Date <= DateTimeOffset.Now &&
                   !string.IsNullOrEmpty(this.phoneNumber) && this.phoneNumber.Length == validPhone &&
                   !string.IsNullOrEmpty(this.socialSecurityNumber) && this.socialSecurityNumber.Length == validSSN &&
                   !string.IsNullOrEmpty(street.Text) && !string.IsNullOrEmpty(lname.Text) &&
                   !string.IsNullOrEmpty(fname.Text));
                
        }

        private void createPatient_onClick(object sender, RoutedEventArgs e)
        {
            this.phoneNumber = this.phone.Text + this.phone1.Text + this.phone2.Text;
            this.validation.Text = "";
            this.socialSecurityNumber = this.ssn.Password + ssn1.Password + ssn2.Password;
            string firstName = this.fname.Text;
            string lastName = this.lname.Text;
            
            DateTime dateOfBirth = this.bday.Date.DateTime;

            string gender = string.Empty;
            
            var genderCmboxSelectedItem = this.genderCmbox.SelectedItem;
            gender = genderCmboxSelectedItem?.ToString();
            var stateCmboxSelectedItem = this.state.SelectedItem;
            var state = stateCmboxSelectedItem?.ToString();
            

            string street = this.street.Text;
            string zip = this.zip.Text;

            try
            {
                if (isValid())
                {
                string fullAddress = street + ", " + state + ", " + zip;
                RegistrationUtility.CreateNewPatient(Convert.ToInt32(socialSecurityNumber), firstName, lastName, this.phoneNumber, dateOfBirth,
                    gender, fullAddress);
                this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    validate();
                }

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
