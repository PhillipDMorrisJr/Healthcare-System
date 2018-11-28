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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditAppointment : Page
    {
        private Patient patient;
        private Doctor doctor;
        private TimeSpan time;
        private bool isValidTime;
        private Appointment originalAppointment;

        public EditAppointment()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.isValidTime = false;
            this.patient = PatientManager.CurrentPatient;

            if (this.patient != null)
            {
                this.nameTxt.Text = this.patient.FirstName + " " + this.patient.LastName;
                this.ssnTxt.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);
                this.phoneTxt.Text = String.Format("{0:(###) ###-####}", this.patient.Phone);
            }

            
            this.originalAppointment = AppointmentManager.CurrentAppointment;
            
            initializeDoctors();

            displayTimes();
            
            this.AppointmentDate.Date = this.originalAppointment.AppointmentDateTime;
            this.description.Text = this.originalAppointment.Description;
            initializeTimes();
        }

        private void initializeTimes()
        {
            object selectedTime = this.AppointmentTimes.Items?.First(item =>
                ((TimeSpan) (item as ListViewItem)?.Tag).Equals(this.originalAppointment.AppointmentTime));
            int timesSelectedIndex = this.AppointmentTimes.Items?.IndexOf(selectedTime) ?? -1;
            if (timesSelectedIndex > -1)
            {
                this.AppointmentTimes.SelectedIndex = timesSelectedIndex;
                this.AppointmentTimes.IsEnabled = true;
            }
        }

        private void initializeDoctors()
        {
            List<Doctor> doctors = DoctorManager.Doctors;
            displayDoctors(doctors);

            object selectedDoctor = this.Doctors.Items?.First(item =>
                ((Doctor) (item as ListViewItem)?.Tag).Id.Equals(this.originalAppointment.Doctor.Id));
            int selectedIndex = this.Doctors.Items?.IndexOf(selectedDoctor) ?? -1;
            if (selectedIndex > -1)
            {
                this.Doctors.SelectedIndex = selectedIndex;
                this.Doctors.IsEnabled = true;
            }
        }

        private void displayTimes()
        {
            if (doctor == null)
            {
                return;
            }

            this.AppointmentTimes.Items?.Clear();

            List<TimeSpan> usedSlots =
                AppointmentManager.RetrieveUsedTimeSlots(this.AppointmentDate.Date.Date, doctor, patient);
           

            if (!(this.AppointmentDate.Date.DayOfWeek == DayOfWeek.Saturday ||
                  this.AppointmentDate.Date.DayOfWeek == DayOfWeek.Sunday))
            {
                TimeSpan slot = new TimeSpan(7, 0, 0);
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




        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
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

        private void update_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = this.AppointmentDate.Date.DateTime;


            if (this.doctor != null && this.isValidTime)
            {
                Appointment newAppointment =
                    new Appointment(this.patient, this.doctor, date, time, description.Text, false, false, false);
                
                AppointmentManager.UpdateAppointment(this.originalAppointment, newAppointment, this.patient);
                this.Frame.Navigate(typeof(Confirmation));
            }
        }

        private void Doctors_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.doctor = (this.Doctors.SelectedItem as ListViewItem)?.Tag as Doctor;
            this.displayTimes();
        }
    }
}