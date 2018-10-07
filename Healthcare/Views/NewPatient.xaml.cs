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
            DateTime dateOFBirth = this.bday.Date.DateTime;
            int phoneNumber;
            bool isTenDigit = false;
             if (phone.Length == 10)
             {
            try
                {
                   phoneNumber = Convert.ToInt32(phone);
                   isTenDigit = true;
               }
               catch (Exception)
              {

             }
              if(!(string.IsNullOrWhiteSpace(fname) && string.IsNullOrWhiteSpace(lname) && string.IsNullOrWhiteSpace(phone)) && isTenDigit) {
                 Patient patient = new Patient(fname, lname, phone, dateOFBirth);
               RegistrationUtility.CreateNewPatient(patient);
               
               this.Frame.Navigate(typeof(MainPage));
              }
             }
        }
    }
}
