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
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPatient : Page
    {
        public EditPatient()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            List<string> genders = new List<string> {"Male", "Female"};
            this.genderCmbox.ItemsSource = genders;

            if (PatientManager.CurrentPatient != null)
            {
                var currentAddress = PatientManager.GetAddressById(PatientManager.CurrentPatient.AddressId);

                if (currentAddress != null)
                {
                    this.street.Text = currentAddress.Street;
                    this.state.Text = currentAddress.State;
                    this.zip.Text = currentAddress.Zip.ToString();
                }

                this.ssn.Password = PatientManager.CurrentPatient.Ssn.ToString();
                this.fname.Text = PatientManager.CurrentPatient.FirstName;
                this.lname.Text = PatientManager.CurrentPatient.LastName;
                this.bday.Date = PatientManager.CurrentPatient.Dob;
                this.phone.Text = PatientManager.CurrentPatient.Phone;
                this.genderCmbox.SelectedItem = PatientManager.CurrentPatient.Gender;
            }           
        }


        private void updatePatient_onClick(object sender, RoutedEventArgs e)
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
                RegistrationUtility.EditPatient(PatientManager.CurrentPatient.Id, Convert.ToInt32(ssn), firstName, lastName, phone, dateOfBirth, gender, fullAddress, PatientManager.CurrentPatient.AddressId);
            }

            this.Frame.Navigate(typeof(MainPage));
        }

        private void home_onClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
