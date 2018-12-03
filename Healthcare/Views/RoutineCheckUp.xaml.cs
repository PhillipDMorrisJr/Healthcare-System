using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class RoutineCheckUp : Page
    {
        private Patient patient;

        public RoutineCheckUp()
        {
            this.InitializeComponent();

            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.patient = AppointmentManager.CurrentAppointment.Patient;

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                                AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);   
            this.doctor.Text = AppointmentManager.CurrentAppointment.Doctor.FullName;

            this.checkupBtn.IsEnabled = true;
            this.doctorDiagnosisBtn.IsEnabled = false;
            this.homeBtn.IsEnabled = true;

/*            var checkups = CheckUpManager.GetRefreshedCheckUps();

            foreach (var checkup in checkups)
            {
                if (checkup != null && checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                {
                    this.systolic.Text = checkup.Systolic.ToString();
                    this.diastolic.Text = checkup.Diastolic.ToString();
                    this.pulse.Text = checkup.Pulse.ToString();
                    this.temperature.Text = checkup.Temperature.ToString(CultureInfo.InvariantCulture);
                    this.weight.Text = checkup.Weight.ToString(CultureInfo.InvariantCulture);
                    this.AppointmentDate.Date = checkup.ArrivalDate;
                    this.AppointmentTime.Time = checkup.ArrivalTime;
                    break;
                }
            }*/
        }

        private void checkup_Click(object sender, RoutedEventArgs e)
        {
            if (!this.hasNullOrEmpty())
            {
                int systolic = int.Parse(this.systolic.Text);
                int diastolic = int.Parse(this.diastolic.Text);
                int pulse = int.Parse(this.pulse.Text);
                double temp = double.Parse(this.temperature.Text);
                double weight = double.Parse(this.weight.Text);

                var appointmentDate = this.AppointmentDate.Date;
                var appointmentTime = this.AppointmentTime.Time;

                var time = appointmentTime;
                var date = appointmentDate.DateTime;

                Nurse nurse = AccessValidator.CurrentUser as Nurse;
                Appointment appointment = AppointmentManager.CurrentAppointment;
                List<Symptom> symptoms = new List<Symptom>();

                ItemCollection items = this.patientSymptoms?.Items;
                if (items != null)
                {
                    foreach (ListViewItem item in items)
                    {
                        Symptom symptom = item.Tag as Symptom;
                        symptoms.Add(symptom);
                    }
                }
                
                CheckUp details = new CheckUp(systolic, diastolic, this.patient, temp, date, time, nurse, weight, pulse, symptoms, appointment);            
                var result = CheckUpManager.Execute(details);

                if (result != null)
                {
                    this.doctorDiagnosisBtn.IsEnabled = true;
                    this.homeBtn.IsEnabled = false;
                    this.checkupBtn.IsEnabled = false;

                    //display complete check-in and checkup dialog

                    this.systolic.IsEnabled = false;
                    this.diastolic.IsEnabled = false;
                    this.pulse.IsEnabled = false;
                    this.temperature.IsEnabled = false;
                    this.weight.IsEnabled = false;
                    this.AppointmentDate.IsEnabled = false;
                    this.AppointmentTime.IsEnabled = false;
                    this.patientSymptoms.IsEnabled = false;
                    this.knownSymptoms.IsEnabled = false;
                    this.addSymptomBtn.IsEnabled = false;
                    this.removeSymptomBtn.IsEnabled = false;

                    CheckUpManager.CurrentCheckUp = details;
                }
                else
                {
                    //display error message dialog
                }
            }
        }

        private bool hasNullOrEmpty()
        {
            return string.IsNullOrEmpty(systolic.Text) || string.IsNullOrEmpty(diastolic.Text) || string.IsNullOrEmpty(temperature.Text) ||
                                                                               string.IsNullOrEmpty(pulse.Text) || string.IsNullOrEmpty(weight.Text);
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void systolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(systolic.Text))
            {
                systolic.Text = "";
            }
        }

        private bool isNotThreeDigitsNorHasChars(string text)
        {
            return !Regex.IsMatch(text, "^(.*[^0-9]|)(1000|[1-9]\\d{0,2})([^0-9].*|)$") || text.Any(character => char.IsLetter(character));
        }

        private void diastolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(diastolic.Text))
            {
                diastolic.Text = "";
            }
        }

        private void weight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(weight.Text))
            {
                weight.Text = "";
            }
        }

        private void pulse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(pulse.Text))
            {
                pulse.Text = "";
            }
        }

        private void temperature_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(temperature.Text))
            {
                    temperature.Text = "";             
            }
        }

        private void addSymptom_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedSymptom = this.knownSymptoms.SelectedItem as ListViewItem;
            bool isOnePatientsSymptoms = !this.patientSymptoms.Items?.Contains(selectedSymptom) ?? false;
            if (isOnePatientsSymptoms)
            {
                this.knownSymptoms.Items?.Remove(selectedSymptom);
                this.patientSymptoms.Items?.Add(selectedSymptom);
            }
        }

        private void removeSymptom_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedPatientSymptom = this.patientSymptoms.SelectedItem as ListViewItem;
            this.patientSymptoms.Items?.Remove(selectedPatientSymptom);
            this.knownSymptoms.Items?.Add(selectedPatientSymptom);
        }

        private void knownSymptoms_Loaded(object sender, RoutedEventArgs e)
        {
            List<Symptom> symptoms =  SymptomManager.Symptoms;
            foreach (var symptom in symptoms)
            {
                if (symptom != null)
                {
                    ListViewItem item = new ListViewItem
                    {                       
                        Tag = symptom, Content = symptom.Name
                    };

                    this.knownSymptoms.Items?.Add(item);
                }
            }
        }

        private void doctorDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DoctorDiagnosis));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var checkup = CheckUpManager.CurrentCheckUp;

            bool diagnosisResult = e.Parameter != null && (bool) e.Parameter;

            var previousPage = Frame.BackStack.Last();

            if (previousPage?.SourcePageType != typeof(DoctorDiagnosis)) return;

            if (!diagnosisResult)
            {
                if (checkup != null)
                {
                    this.handleFalsePageReturn(checkup);
                }

                return;
            }


           this.handleTruePageReturn(checkup);

           base.OnNavigatedTo(e);
        }

        private void handleFalsePageReturn(CheckUp checkup)
        {
            this.systolic.Text = checkup.Systolic.ToString();
            this.diastolic.Text = checkup.Diastolic.ToString();
            this.pulse.Text = checkup.Pulse.ToString();
            this.temperature.Text = checkup.Temperature.ToString(CultureInfo.InvariantCulture);
            this.weight.Text = checkup.Weight.ToString(CultureInfo.InvariantCulture);
            this.AppointmentDate.Date = checkup.ArrivalDate;
            this.AppointmentTime.Time = checkup.ArrivalTime;

            this.doctorDiagnosisBtn.IsEnabled = true;
            this.homeBtn.IsEnabled = true;
            this.checkupBtn.IsEnabled = false;

            this.systolic.IsEnabled = false;
            this.diastolic.IsEnabled = false;
            this.pulse.IsEnabled = false;
            this.temperature.IsEnabled = false;
            this.weight.IsEnabled = false;
            this.AppointmentDate.IsEnabled = false;
            this.AppointmentTime.IsEnabled = false;
            this.patientSymptoms.IsEnabled = false;
            this.knownSymptoms.IsEnabled = false;
            this.addSymptomBtn.IsEnabled = false;
            this.removeSymptomBtn.IsEnabled = false;
        }

        private void handleTruePageReturn(CheckUp checkup)
        {
            this.systolic.Text = checkup.Systolic.ToString();
            this.diastolic.Text = checkup.Diastolic.ToString();
            this.pulse.Text = checkup.Pulse.ToString();
            this.temperature.Text = checkup.Temperature.ToString(CultureInfo.InvariantCulture);
            this.weight.Text = checkup.Weight.ToString(CultureInfo.InvariantCulture);
            this.AppointmentDate.Date = checkup.ArrivalDate;
            this.AppointmentTime.Time = checkup.ArrivalTime;

            this.doctorDiagnosisBtn.IsEnabled = false;
            this.homeBtn.IsEnabled = true;
            this.checkupBtn.IsEnabled = false;

            this.systolic.IsEnabled = false;
            this.diastolic.IsEnabled = false;
            this.pulse.IsEnabled = false;
            this.temperature.IsEnabled = false;
            this.weight.IsEnabled = false;
            this.AppointmentDate.IsEnabled = false;
            this.AppointmentTime.IsEnabled = false;
            this.patientSymptoms.IsEnabled = false;
            this.knownSymptoms.IsEnabled = false;
            this.addSymptomBtn.IsEnabled = false;
            this.removeSymptomBtn.IsEnabled = false;
        }
    }
}
