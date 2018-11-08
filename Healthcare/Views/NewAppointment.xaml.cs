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
using Healthcare.DAL;
using Healthcare.Model;
using Healthcare.Utils;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Healthcare.Views
{
    public sealed partial class NewAppointment : Page
    {
        private Patient patient;
        private Doctor doctor;
        /// <summary>
        /// Initializes a new instance of the <see cref="NewAppointment"/> class.
        /// </summary>
        public NewAppointment()
        {
            this.InitializeComponent();
            this.patient = PatientManager.CurrentPatient;
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.ID;
            this.accessType.Text = AccessValidator.Access;
            this.name.Text = this.patient.FirstName + " " + this.patient.LastName;
            this.id.Text = this.patient.Id.ToString();
            //this.ssn.Text = this.patient.Ssn.ToString();
            this.phone.Text = String.Format("{0:(###) ###-####}", this.patient.Phone);

            DoctorDAL dal = new DoctorDAL();
            List<Doctor> doctors = dal.GetDoctors();

            foreach (var aDoctor in doctors)
            {
                if (aDoctor != null)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = aDoctor;
                    item.Content = aDoctor.FullName;
                    this.databaseInformationDoctors.Items?.Add(item);
                }
            }

        }
        
        /// <summary>
        /// Handles the Click event of the schedule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void schedule_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = this.AppointmentDate.Date.DateTime;
            TimeSpan time = this.AppointmentTime.Time;

            if (this.doctor != null)
            {
                Appointment appt = new Appointment(this.patient, this.doctor, date, time);
                AppointmentManager.Appointments[this.patient].Add(appt);
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void DatabaseInformationDoctors_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.doctor = (this.databaseInformationDoctors.SelectedItem as ListViewItem)?.Tag as Doctor;
        }
    }
}
 