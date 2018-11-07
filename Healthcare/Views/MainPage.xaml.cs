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
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.ID;
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
            List<Patient> patientRegistry =  RegistrationUtility.GetPatients();
            foreach (var patientToRegister in patientRegistry)
            {
                if (patientToRegister != null)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = patientToRegister;
                    item.Content = patientToRegister.Format();
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
        private void onUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditPatient));
        }

        /// <summary>
        /// Handles the Click event of the onPatientDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void onPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientDetails));
        }

        /// <summary>
        /// Handles the Click event of the onAddAppointment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void onAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewAppointment));
        }

        /// <summary>
        /// Handles the Click event of the onUpdateAppointment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void onUpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditAppointment));
        }

        /// <summary>
        /// Handles the Click event of the onAppointmentDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void onAppointmentDetails_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppointmentDetails));
        }

        /// <summary>
        /// Handles the OnSelectionChanged event of the DatabasePatientInformation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void DatabasePatientInformation_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DatabaseAppointmentInformation.Items?.Clear();
            Patient currentPatient = (this.DatabasePatientInformation.SelectedItem as ListViewItem)?.Tag as Patient;
            PatientManager.CurrentPatient = currentPatient;
            List<Appointment> appointments;
            AppointmentManager.Appointments.TryGetValue(currentPatient, out appointments);
            if (appointments == null)
            {
                AppointmentManager.Appointments.Add(currentPatient, new List<Appointment>());
            }
            foreach (var appointment in AppointmentManager.Appointments[currentPatient])
            {
                if (appointment != null)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = appointment;
                    item.Content = appointment.Format();
                    this.DatabaseAppointmentInformation.Items?.Add(item);
                }
            }

        }
    }
}
