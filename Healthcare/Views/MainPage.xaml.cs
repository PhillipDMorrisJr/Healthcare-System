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
using Healthcare.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Healthcare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            //On load this should get you the patient registered in the registration page
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.ID;
            this.accessType.Text = AccessValidator.Access;

        }

        /// <summary>
        /// Handles the Click event of the onLogout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void onLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        /// <summary>
        /// Handles the Click event of the onRegister control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void onRegister_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {

            List<Patient> patientRegistry =  RegistrationUtility.GetPatients();
            foreach (var patientToRegister in patientRegistry)
            {


                if (patientToRegister != null)
                {
                    //TODO: use patient to populate listview
                    this.DatabasePatientInformation.Items.Add(patientToRegister.Format());

                }
            }
        }

        private void onRegisterAppointment_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void onAddPatient_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegistrationPage));
        }

        private void onUpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onAppointmentDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
