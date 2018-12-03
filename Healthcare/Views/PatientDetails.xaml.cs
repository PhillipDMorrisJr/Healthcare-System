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
    public sealed partial class PatientDetails : Page
    {
        public PatientDetails()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            if (PatientManager.CurrentPatient != null)
            {
                var currentAddress = PatientManager.GetAddressById(PatientManager.CurrentPatient.AddressId);

                if (currentAddress != null)
                {
                    this.street.Text = currentAddress.Street;
                    this.state.Text = currentAddress.State;
                    this.zip.Text = currentAddress.Zip.ToString();
                }

                this.fname.Text = PatientManager.CurrentPatient.FirstName;
                this.lname.Text = PatientManager.CurrentPatient.LastName;
                this.bday.Date = PatientManager.CurrentPatient.Dob;
                long.TryParse(PatientManager.CurrentPatient.Phone, out long pNumber);
                this.phone.Text = string.Format("{0:(###) ###-####}", pNumber);
                this.gender.Text = PatientManager.CurrentPatient.Gender;

                var social = "***-**-" + PatientManager.CurrentPatient.Ssn.ToString().Substring(5);
                this.ssn.Text = social;
            }
        }

        private void home_onClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
