using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPatient : Page
    {
        private string phoneNumber;
        private string socialSecurityNumber;

        public NewPatient()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            var genders = new List<string> {"Male", "Female"};
            state.ItemsSource = States.GetStates();
            state.SelectedItem = "AL";
            genderCmbox.ItemsSource = genders;
            genderCmbox.SelectedItem = "Male";
        }


        private void validate()
        {
            validation.Text += "Please Address the following:\n";
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
            if (string.IsNullOrEmpty(zip.Text) || zip.Text.Length != 5)
            {
                validation.Text += "Enter valid 5 digit zip in the following format: xxxxx\n";
                zip.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                zip.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateDate()
        {
            if (bday.Date > DateTimeOffset.Now)
            {
                validation.Text += "Patient's birthday must before the current day\n";
                bday.Background = new SolidColorBrush(Colors.MistyRose);
            }
            else
            {
                bday.Background = new SolidColorBrush(Colors.Azure);
            }
        }

        private void validatePhone()
        {
            var validNumberOfDigits = 10;

            var isNumber = long.TryParse(phoneNumber, out var result);
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != validNumberOfDigits || !isNumber)
            {
                validation.Text += "Please enter a valid 10 digit phone number in the following format: xxx-xxx-xxxx\n";
                phone.BorderBrush = new SolidColorBrush(Colors.Red);
                phone1.BorderBrush = new SolidColorBrush(Colors.Red);
                phone2.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                phone.Text = phoneNumber.Substring(0, 3);
                phone1.Text = phoneNumber.Substring(3, 3);
                phone2.Text = phoneNumber.Substring(6, 4);
                phone.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                phone1.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                phone2.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateSSN()
        {
            var validNumberOfDigits = 9;
            var isNumber = int.TryParse(socialSecurityNumber, out var result);
            if (string.IsNullOrEmpty(socialSecurityNumber) || socialSecurityNumber.Length != validNumberOfDigits ||
                !isNumber)
            {
                validation.Text += "Please enter a valid 9 digit ssn in the following format: xxx-xx-xxxx\n";
                ssn.BorderBrush = new SolidColorBrush(Colors.Red);
                ssn1.BorderBrush = new SolidColorBrush(Colors.Red);
                ssn2.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ssn.Password = socialSecurityNumber.Substring(0, 3);
                ssn1.Password = socialSecurityNumber.Substring(3, 2);
                ssn2.Password = socialSecurityNumber.Substring(5, 4);
                ssn.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                ssn1.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
                ssn2.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateStreet()
        {
            if (string.IsNullOrEmpty(street.Text))
            {
                validation.Text += "Please enter a valid street\n";
                street.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                street.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateLastName()
        {
            if (string.IsNullOrEmpty(lname.Text))
            {
                validation.Text += "Please enter a valid last name\n";
                lname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                lname.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private void validateFirstName()
        {
            if (string.IsNullOrEmpty(fname.Text))
            {
                validation.Text += "Please enter a valid first name\n";
                fname.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                fname.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }
        }

        private bool isValid()
        {
            var validZip = 5;
            var validSSN = 9;
            var validPhone = 10;
            var isPhoneNumber = long.TryParse(phoneNumber, out var result);
            var isSSNNumber = int.TryParse(ssn.Password, out var result1);
            var isZipNumber = int.TryParse(zip.Text, out var result2);
            return !string.IsNullOrEmpty(zip.Text) && isPhoneNumber && isSSNNumber && isZipNumber &&
                   zip.Text.Length == validZip && bday.Date <= DateTimeOffset.Now &&
                   !string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == validPhone &&
                   !string.IsNullOrEmpty(socialSecurityNumber) && socialSecurityNumber.Length == validSSN &&
                   !string.IsNullOrEmpty(street.Text) && !string.IsNullOrEmpty(lname.Text) &&
                   !string.IsNullOrEmpty(fname.Text);
        }

        private void createPatient_onClick(object sender, RoutedEventArgs e)
        {
            phoneNumber = phone.Text + phone1.Text + phone2.Text;
            validation.Text = "";
            socialSecurityNumber = ssn.Password + ssn1.Password + ssn2.Password;
            var firstName = fname.Text;
            var lastName = lname.Text;

            var dateOfBirth = bday.Date.DateTime;

            var gender = string.Empty;

            var genderCmboxSelectedItem = genderCmbox.SelectedItem;
            gender = genderCmboxSelectedItem?.ToString();
            var stateCmboxSelectedItem = this.state.SelectedItem;
            var state = stateCmboxSelectedItem?.ToString();


            var street = this.street.Text;
            var zip = this.zip.Text;

            try
            {
                if (isValid())
                {
                    var fullAddress = street + ", " + state + ", " + zip;
                    RegistrationUtility.CreateNewPatient(Convert.ToInt32(socialSecurityNumber), firstName, lastName,
                        phoneNumber, dateOfBirth,
                        gender, fullAddress);
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    validate();
                }
            }
            catch (Exception)
            {
                validate();
            }
        }

        private void home_onClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}