using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Healthcare.DAL;
using Healthcare.Model;
using Healthcare.Utils;
using Healthcare.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Healthcare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MainPage : Page
    {
        private static int findValue;
        private Appointment currentAppointment;
        private Patient currentPatient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            currentAppointment = null;
            currentPatient = null;
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

        }

        /// <summary>
        /// Handles the Click event of the onLogout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void onLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        /// <summary>
        /// Handles the OnLoaded event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            findValue = 1;

            List<Patient> patientRegistry =  RegistrationUtility.GetPatients();
            foreach (var patientToRegister in patientRegistry)
            {
                if (patientToRegister != null)
                {
                    ListViewItem item = new ListViewItem
                    {
                        Tag = patientToRegister, Content = patientToRegister.Format()
                    };
                    this.DatabasePatientInformation.Items?.Add(item);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the onAddPatient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void onAddPatient_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewPatient));
        }

        /// <summary>
        /// Handles the Click event of the onUpdatePatient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
            {
                this.Frame.Navigate(typeof(EditPatient));
            }

            await InformToSelectPatient();
        }

        private static async Task InformToSelectPatient()
        {
            ContentDialog selectPatient = new ContentDialog()
            {
                Content = "Please select a patient",
                CloseButtonText = "Okay"
            };
            await selectPatient.ShowAsync();
        }
        private static async Task InformToSelectAppointment()
        {
            ContentDialog selectAppointment = new ContentDialog()
            {
                Content = "Please select an appointment",
                CloseButtonText = "Okay"
            };
            await selectAppointment.ShowAsync();
        }

        /// <summary>
        /// Handles the Click event of the onPatientDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
            {
                this.Frame.Navigate(typeof(PatientDetails));
            }
            await InformToSelectPatient();
            
        }

        /// <summary>
        /// Handles the Click event of the onAddAppointment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void onAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
            {
                this.Frame.Navigate(typeof(NewAppointment));
            }
            await InformToSelectPatient();
        }

        /// <summary>
        /// Handles the Click event of the onUpdateAppointment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void onUpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (currentAppointment != null)
            {
                this.Frame.Navigate(typeof(EditAppointment));
            }
            await InformToSelectAppointment();
        }

        /// <summary>
        /// Handles the Click event of the onCheckUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private async void onCheckUp_Click(object sender, RoutedEventArgs e)
        {
            if (currentAppointment != null && AccessValidator.Access.Equals("Nurse"))
            {
                this.Frame.Navigate(typeof(RoutineCheckUp));
            }

            if (currentAppointment == null)
            {
                await InformToSelectAppointment();
            } 
            ContentDialog invalidAccess = new ContentDialog()
            {
                Content = "Please login as a Nurse to perform routine check-up",
                CloseButtonText = "Okay"
            };
            await invalidAccess.ShowAsync();


        }

        /// <summary>
        /// Handles the OnSelectionChanged event of the DatabasePatientInformation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void DatabasePatientInformation_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPatient = (this.DatabasePatientInformation.SelectedItem as ListViewItem)?.Tag as Patient;
            this.DatabaseAppointmentInformation.Items?.Clear();
            if (currentPatient == null)
            {
                return;
            }

            PatientManager.CurrentPatient = currentPatient;
            List<Appointment> appointments = AppointmentDAL.GetAppointments(currentPatient);

            if (appointments != null)
            {
                try
                {
                    AppointmentManager.Appointments[currentPatient] = appointments;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        
            foreach (var appointment in AppointmentManager.Appointments[currentPatient])
            {
                if (appointment == null) continue;
                var item = new ListViewItem {Tag = appointment, Content = appointment.Format()};
                this.DatabaseAppointmentInformation.Items?.Add(item);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var patients = RegistrationUtility.GetRefreshedPatients();
           
            this.RefreshPatientList(patients);
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            switch (findValue)
            {
                case 1:
                    HandleSearchByName();
                    break;
                case 2:
                    HandleSearchByDob();
                    break;
                case 3:
                    HandleSearchByBoth();
                    break;
            }
        }

        private void HandleSearchByName()
        {
            var fName = this.firstName.Text;
            var lName = this.lastName.Text;

            if (string.IsNullOrWhiteSpace(fName) && string.IsNullOrWhiteSpace(lName)) return;

            RegistrationUtility.FindPatientsByName(fName, lName);
            
            var patients = RegistrationUtility.GetPatients();

            this.RefreshPatientList(patients);        
        }

        private void HandleSearchByDob()
        {
            var dob = this.datePicker.Date.DateTime;

            RegistrationUtility.FindPatientsByDob(dob);

            var patients = RegistrationUtility.GetPatients();

            this.RefreshPatientList(patients);
        }

        private void HandleSearchByBoth()
        {
            var dob = this.datePicker.Date.DateTime;
            var fName = this.firstName.Text;
            var lName = this.lastName.Text;

            if (string.IsNullOrWhiteSpace(fName) && string.IsNullOrWhiteSpace(lName)) return;

            RegistrationUtility.FindPatientsByNameAndDob(fName, lName, dob);
            
            var patients = RegistrationUtility.GetPatients();

            this.RefreshPatientList(patients);
        }

        private void SearchNameRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            findValue = 1;
        }

        private void SearchDateRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            findValue = 2;
        }

        private void SearchBothRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            findValue = 3;
        }

        private void DatabaseAppointmentInformation_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAppointment = (this.DatabaseAppointmentInformation.SelectedItem as ListViewItem)?.Tag as Appointment;
            AppointmentManager.CurrentAppointment = currentAppointment;
        }

        private void RefreshPatientList(List<Patient> patients)
        {
            this.DatabasePatientInformation.Items?.Clear();
           
            foreach (var patientToRegister in patients)
            {
                if (patientToRegister != null)
                {
                    ListViewItem item = new ListViewItem
                    {
                        Tag = patientToRegister, Content = patientToRegister.Format()
                    };
                    this.DatabasePatientInformation.Items?.Add(item);
                }
            }
        }
        private async void customQuery_Click(object sender, RoutedEventArgs e)
        {
            if (AccessValidator.Access.Equals("Administrator"))
            {
                Frame.Navigate(typeof(QueryPage));
            }
            ContentDialog invalidAccess = new ContentDialog()
            {
                Content = "Please login as an Administrator to access this page",
                CloseButtonText = "Okay"
            };
            await invalidAccess.ShowAsync();
        }
        /// <summary>
        /// Handles the Click event of the Appointment Details control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void onDetails_Click(object sender, RoutedEventArgs e)
        {
            if (currentAppointment != null)
            {
                this.Frame.Navigate(typeof(AppointmentDetails));
            }
            await InformToSelectAppointment();
        }
    }
}
