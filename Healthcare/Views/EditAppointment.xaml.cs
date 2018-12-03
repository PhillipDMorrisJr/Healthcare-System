using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditAppointment : Page
    {
        private Doctor doctor;
        private bool isValidTime;
        private readonly Appointment originalAppointment;
        private readonly Patient patient;
        private TimeSpan time;

        public EditAppointment()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            isValidTime = false;
            patient = PatientManager.CurrentPatient;

            if (patient != null)
            {
                nameTxt.Text = patient.FirstName + " " + patient.LastName;
                ssnTxt.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);
                phoneTxt.Text = string.Format("{0:(###) ###-####}", patient.Phone);
            }


            originalAppointment = AppointmentManager.CurrentAppointment;

            initializeDoctors();

            displayTimes();

            AppointmentDate.Date = originalAppointment.AppointmentDateTime;
            description.Text = originalAppointment.Description;
            initializeTimes();
        }

        private void initializeTimes()
        {
            var selectedTime = AppointmentTimes.Items?.First(item =>
                ((TimeSpan) (item as ListViewItem)?.Tag).Equals(originalAppointment.AppointmentTime));
            var timesSelectedIndex = AppointmentTimes.Items?.IndexOf(selectedTime) ?? -1;
            if (timesSelectedIndex > -1)
            {
                AppointmentTimes.SelectedIndex = timesSelectedIndex;

                AppointmentTimes.IsEnabled = true;
            }
        }

        private void initializeDoctors()
        {
            var doctors = DoctorManager.Doctors;
            displayDoctors(doctors);

            var selectedDoctor = Doctors.Items?.First(item =>
                ((Doctor) (item as ListViewItem)?.Tag).Id.Equals(originalAppointment.Doctor.Id));
            var selectedIndex = Doctors.Items?.IndexOf(selectedDoctor) ?? -1;
            if (selectedIndex > -1)
            {
                Doctors.SelectedIndex = selectedIndex;
                Doctors.IsEnabled = true;
            }
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


        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }


        private void Times_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAppointmentTime = AppointmentTimes.SelectedItem as ListViewItem;
            isValidTime = selectedAppointmentTime?.IsEnabled ?? false;
            var appointmentTime = selectedAppointmentTime?.Tag;
            if (appointmentTime != null) time = (TimeSpan) appointmentTime;
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            validation.Text = "";

            var date = AppointmentDate.Date.DateTime;

            var isValid = validate();


            if (isValid)
            {
                var newAppointment =
                    new Appointment(patient, doctor, date, time, description.Text, false);

                AppointmentManager.UpdateAppointment(originalAppointment, newAppointment, patient);
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

        private void AppointmentDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            displayTimes();
        }

        private void Doctors_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            doctor = (Doctors.SelectedItem as ListViewItem)?.Tag as Doctor;
            displayTimes();
        }
    }
}