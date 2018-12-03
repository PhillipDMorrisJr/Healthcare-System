using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.DAL;
using Healthcare.Model;
using Healthcare.Utils;
using Healthcare.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Healthcare
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
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
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            currentAppointment = null;
            currentPatient = null;
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;
        }

        /// <summary>
        ///     Handles the Click event of the onLogout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void onLogout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        /// <summary>
        ///     Handles the OnLoaded event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                findValue = 1;

                var patientRegistry = RegistrationUtility.GetPatients();
                foreach (var patientToRegister in patientRegistry)
                    if (patientToRegister != null)
                    {
                        var item = new ListViewItem
                        {
                            Tag = patientToRegister,
                            Content = patientToRegister.Format()
                        };
                        DatabasePatientInformation.Items?.Add(item);
                    }
            }
            catch (Exception)
            {
                var backHome = new ContentDialog
                {
                    Content = "Page failed to load. Returning back to login page.",
                    CloseButtonText = "Okay"
                };
                await backHome.ShowAsync();
                Frame.Navigate(typeof(LoginPage));
            }
        }

        /// <summary>
        ///     Handles the Click event of the onAddPatient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void onAddPatient_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPatient));
        }

        /// <summary>
        ///     Handles the Click event of the onUpdatePatient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
                Frame.Navigate(typeof(EditPatient));
            else
                await InformToSelectPatient();
        }

        private static async Task InformToSelectPatient()
        {
            var selectPatient = new ContentDialog
            {
                Content = "Please select a patient",
                CloseButtonText = "Okay"
            };
            await selectPatient.ShowAsync();
        }

        private static async Task InformToSelectAppointment()
        {
            var selectAppointment = new ContentDialog
            {
                Content = "Please select an appointment",
                CloseButtonText = "Okay"
            };
            await selectAppointment.ShowAsync();
        }

        /// <summary>
        ///     Handles the Click event of the onPatientDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
                Frame.Navigate(typeof(PatientDetails));
            else
                await InformToSelectPatient();
        }

        /// <summary>
        ///     Handles the Click event of the onAddAppointment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
                Frame.Navigate(typeof(NewAppointment));
            else
                await InformToSelectPatient();
        }

        /// <summary>
        ///     Handles the Click event of the onUpdateAppointment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onUpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (currentAppointment != null)
                Frame.Navigate(typeof(EditAppointment));
            else
                await InformToSelectAppointment();
        }

        /// <summary>
        ///     Handles the Click event of the onCheckUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private async void onCheckUp_Click(object sender, RoutedEventArgs e)
        {
            if (currentAppointment != null && AccessValidator.Access.Equals("Nurse"))
            {
                Frame.Navigate(typeof(RoutineCheckUp));
            }
            else if (currentAppointment == null)
            {
                await InformToSelectAppointment();
            }
            else
            {
                var invalidAccess = new ContentDialog
                {
                    Content = "Please login as a Nurse to perform routine check-up",
                    CloseButtonText = "Okay"
                };
                await invalidAccess.ShowAsync();
            }
        }

        /// <summary>
        ///     Handles the OnSelectionChanged event of the DatabasePatientInformation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void DatabasePatientInformation_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPatient = (DatabasePatientInformation.SelectedItem as ListViewItem)?.Tag as Patient;
            DatabaseAppointmentInformation.Items?.Clear();
            if (currentPatient == null) return;

            PatientManager.CurrentPatient = currentPatient;
            var appointments = AppointmentDAL.GetAppointments(currentPatient);

            AddAPpointmentsToManager(appointments);

            displayAppointments();
        }

        private void displayAppointments()
        {
            try
            {
                foreach (var appointment in AppointmentManager.Appointments[currentPatient])
                {
                    if (appointment == null) continue;
                    var item = new ListViewItem {Tag = appointment, Content = appointment.Format()};
                    DatabaseAppointmentInformation.Items?.Add(item);
                }
            }
            catch (Exception)
            {
            }
        }

        private void AddAPpointmentsToManager(List<Appointment> appointments)
        {
            if (appointments != null)
                try
                {
                    AppointmentManager.Appointments[currentPatient] = appointments;
                }
                catch (Exception)
                {
                    // ignored
                }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var patients = RegistrationUtility.GetRefreshedPatients();

            RefreshPatientList(patients);
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
            var fName = firstName.Text;
            var lName = lastName.Text;

            if (string.IsNullOrWhiteSpace(fName) && string.IsNullOrWhiteSpace(lName)) return;

            RegistrationUtility.FindPatientsByName(fName, lName);

            var patients = RegistrationUtility.GetPatients();

            RefreshPatientList(patients);
        }

        private void HandleSearchByDob()
        {
            var dob = datePicker.Date.DateTime;

            RegistrationUtility.FindPatientsByDob(dob);

            var patients = RegistrationUtility.GetPatients();

            RefreshPatientList(patients);
        }

        private void HandleSearchByBoth()
        {
            var dob = datePicker.Date.DateTime;
            var fName = firstName.Text;
            var lName = lastName.Text;

            if (string.IsNullOrWhiteSpace(fName) && string.IsNullOrWhiteSpace(lName)) return;

            RegistrationUtility.FindPatientsByNameAndDob(fName, lName, dob);

            var patients = RegistrationUtility.GetPatients();

            RefreshPatientList(patients);
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
            currentAppointment = (DatabaseAppointmentInformation.SelectedItem as ListViewItem)?.Tag as Appointment;
            AppointmentManager.CurrentAppointment = currentAppointment;
        }

        private void RefreshPatientList(List<Patient> patients)
        {
            DatabasePatientInformation.Items?.Clear();

            foreach (var patientToRegister in patients)
                if (patientToRegister != null)
                {
                    var item = new ListViewItem
                    {
                        Tag = patientToRegister, Content = patientToRegister.Format()
                    };
                    DatabasePatientInformation.Items?.Add(item);
                }
        }

        private async void customQuery_Click(object sender, RoutedEventArgs e)
        {
            if (AccessValidator.Access.Equals("Administrator"))
            {
                Frame.Navigate(typeof(QueryPage));
            }
            else
            {
                var invalidAccess = new ContentDialog
                {
                    Content = "Please login as an Administrator to access this page",
                    CloseButtonText = "Okay"
                };
                await invalidAccess.ShowAsync();
            }
        }

        /// <summary>
        ///     Handles the Click event of the Appointment Details control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void onDetails_Click(object sender, RoutedEventArgs e)
        {
            if (currentAppointment != null)
                Frame.Navigate(typeof(AppointmentDetails));
            else
                await InformToSelectAppointment();
        }
    }
}