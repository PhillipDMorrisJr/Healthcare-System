using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Healthcare.DAL;
using Healthcare.Model;
using Healthcare.Utils;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Healthcare.Views
{
    public sealed partial class NewAppointment : Page
    {
        private Doctor doctor;
        private bool isValidTime;
        private readonly Patient patient;
        private TimeSpan time;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NewAppointment" /> class.
        /// </summary>
        public NewAppointment()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            isValidTime = false;
            patient = PatientManager.CurrentPatient;

            if (patient != null)
            {
                name.Text = patient.FirstName + " " + patient.LastName;
                ssn.Text = "***-**-" + patient.Ssn.ToString().Substring(5);
                phone.Text = string.Format("{0:(###) ###-####}", patient.Phone);
            }

            var doctors = DoctorManager.Doctors;

            displayDoctors(doctors);
            displayTimes();
        }

        private void displayTimes()
        {
            if (doctor == null) return;
            AppointmentTimes.Items?.Clear();

            var usedSlots = AppointmentManager.RetrieveUsedTimeSlots(AppointmentDate.Date.Date, doctor, patient);

            if (!(AppointmentDate.Date.DayOfWeek == DayOfWeek.Saturday ||
                  AppointmentDate.Date.DayOfWeek == DayOfWeek.Sunday))
            {
                var slot = new TimeSpan(7, 0, 0);
                var timeSlots = new List<TimeSpan>();
                timeSlots.Add(slot);
                for (var i = 0; i < 20; i++)
                {
                    slot += TimeSpan.FromMinutes(30);
                    timeSlots.Add(slot);
                }

                foreach (var currentSlot in timeSlots)
                    if (!usedSlots.Contains(currentSlot) &&
                        (AppointmentDate.Date.Date == DateTimeOffset.Now.Date.Date &&
                         currentSlot > DateTime.Now.TimeOfDay ||
                         AppointmentDate.Date.Date > DateTimeOffset.Now.Date.Date))
                    {
                        var item = new ListViewItem();
                        item.Tag = currentSlot;
                        var formattedTime = DateTime.Today.Add(currentSlot).ToString("hh:mm tt");

                        item.Content = formattedTime;
                        AppointmentTimes.Items?.Add(item);
                    }
                    else
                    {
                        var item = new ListViewItem();
                        item.Tag = currentSlot;
                        var formattedTime = DateTime.Today.Add(currentSlot).ToString("hh:mm tt");

                        item.Content = formattedTime;
                        AppointmentTimes.Items?.Add(item);
                        item.IsEnabled = false;
                        item.Foreground = new SolidColorBrush(Colors.Gray);
                    }
            }
        }

        private void displayDoctors(List<Doctor> doctors)
        {
            foreach (var aDoctor in doctors)
                if (aDoctor != null)
                {
                    var item = new ListViewItem();
                    item.Tag = aDoctor;
                    item.Content = aDoctor.FullName;
                    Doctors.Items?.Add(item);
                }
        }

        /// <summary>
        ///     Handles the Click event of the schedule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void schedule_Click(object sender, RoutedEventArgs e)
        {
            validation.Text = "";
            var date = AppointmentDate.Date.DateTime;
            var isValid = validate();


            if (isValid)
            {
                var appointment = new Appointment(patient, doctor, date, time, description.Text, false);
                AppointmentDAL.AddAppointment(appointment);
                AppointmentManager.AddAppointment(appointment, patient);
                Frame.Navigate(typeof(Confirmation));
            }
        }

        private bool validate()
        {
            var isValid = true;
            if (!isValidTime || doctor == null)
            {
                validation.Text = "Please address the following: \n";
                isValid = validateTime(isValid);

                isValid = validateDoctor(isValid);

                isValid = validateAppointmentDate(isValid);
            }

            return isValid;
        }

        private bool validateAppointmentDate(bool isValid)
        {
            if (AppointmentDate.Date.Date < DateTime.Now.Date.Date)
            {
                validation.Text += "Please select a valid date \n";
                AppointmentDate.Background = new SolidColorBrush(Colors.MistyRose);
                isValid = false;
            }
            else
            {
                AppointmentDate.Background = new SolidColorBrush(Colors.Azure);
            }

            return isValid;
        }

        private bool validateDoctor(bool isValid)
        {
            if (doctor == null)
            {
                validation.Text += "Please select a doctor \n";
                Doctors.Background = new SolidColorBrush(Colors.MistyRose);
                isValid = false;
            }
            else
            {
                Doctors.Background = new SolidColorBrush(Colors.Azure);
            }

            return isValid;
        }

        private bool validateTime(bool isValid)
        {
            if (!isValidTime)
            {
                validation.Text += "Please select a valid time \n";
                AppointmentTimes.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                AppointmentTimes.BorderBrush = new SolidColorBrush(Colors.Gainsboro);
            }

            return isValid;
        }

        private void Doctors_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            doctor = (Doctors.SelectedItem as ListViewItem)?.Tag as Doctor;
            displayTimes();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AppointmentDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            displayTimes();
        }

        private void Times_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAppointmentTime = AppointmentTimes.SelectedItem as ListViewItem;
            isValidTime = selectedAppointmentTime?.IsEnabled ?? false;
            var appointmentTime = selectedAppointmentTime?.Tag;
            if (appointmentTime != null) time = (TimeSpan) appointmentTime;
        }
    }
}