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
        private TimeSpan time;
        private bool isValidTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewAppointment"/> class.
        /// </summary>
        public NewAppointment()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;
            this.isValidTime = false;
            this.patient = PatientManager.CurrentPatient;

            if (this.patient != null)
            {
                this.name.Text = this.patient.FirstName + " " + this.patient.LastName;
                this.id.Text = this.patient.Id.ToString();
                this.phone.Text = String.Format("{0:(###) ###-####}", this.patient.Phone);
            }

            List<Doctor> doctors = DoctorManager.Doctors;

            displayDoctors(doctors);
            displayTimes();

        }

        private void displayTimes()
        {            
            if (doctor == null)
            {
                return;
            }
            this.AppointmentTimes.Items?.Clear();

            List<TimeSpan> usedSlots = AppointmentManager.RetrieveUsedTimeSlots(this.AppointmentDate.Date.Date, doctor, patient);

            if (!(this.AppointmentDate.Date.DayOfWeek == DayOfWeek.Saturday ||
                this.AppointmentDate.Date.DayOfWeek == DayOfWeek.Sunday))
            {
                TimeSpan slot = new TimeSpan(7,0,0);
                List<TimeSpan> timeSlots = new List<TimeSpan>();
                timeSlots.Add(slot);
                for (int i = 0; i < 20; i++)
                {
                    slot += TimeSpan.FromMinutes(30);
                    timeSlots.Add(slot);
                }
                foreach (var currentSlot in timeSlots)
                {
                    if (!usedSlots.Contains(currentSlot))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Tag = currentSlot;
                        string formattedTime = DateTime.Today.Add(currentSlot).ToString("hh:mm tt");

                        item.Content = formattedTime;
                        this.AppointmentTimes.Items?.Add(item);
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem();
                        item.Tag = currentSlot;
                        string formattedTime = DateTime.Today.Add(currentSlot).ToString("hh:mm tt");

                        item.Content = formattedTime;
                        this.AppointmentTimes.Items?.Add(item);
                        item.IsEnabled = false;
                        item.Foreground = new SolidColorBrush(Colors.Gray); 
                    }
                }
            }
        }

        private void displayDoctors(List<Doctor> doctors)
        {
            foreach (var aDoctor in doctors)
            {
                if (aDoctor != null)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = aDoctor;
                    item.Content = aDoctor.FullName;
                    this.Doctors.Items?.Add(item);
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

            if (this.doctor != null && this.isValidTime)
            {
                Appointment appointment = new Appointment(this.patient, this.doctor, date, time, description.Text,false);
                AppointmentDAL.AddAppointment(this.patient, this.doctor, date, time,description.Text, false);
                AppointmentManager.AddAppointment(appointment, this.patient);
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void Doctors_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.doctor = (this.Doctors.SelectedItem as ListViewItem)?.Tag as Doctor;
            this.displayTimes();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void AppointmentDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            displayTimes();
        }

        private void Times_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem selectedAppointmentTime = this.AppointmentTimes.SelectedItem as ListViewItem;
            this.isValidTime = selectedAppointmentTime?.IsEnabled ?? false;
            var appointmentTime = (selectedAppointmentTime)?.Tag;
            if (appointmentTime != null)
            {
                this.time = (TimeSpan) appointmentTime;
            }
        }
    }
}
 