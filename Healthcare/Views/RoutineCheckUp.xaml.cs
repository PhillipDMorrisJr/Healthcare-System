using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoutineCheckUp : Page
    {
        private readonly Patient patient;

        public RoutineCheckUp()
        {
            InitializeComponent();

            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            patient = AppointmentManager.CurrentAppointment.Patient;

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);
            doctor.Text = AppointmentManager.CurrentAppointment.Doctor.FullName;

            checkupBtn.IsEnabled = true;
            doctorDiagnosisBtn.IsEnabled = false;
            homeBtn.IsEnabled = true;

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
            if (!hasNullOrEmpty())
            {
                var systolic = int.Parse(this.systolic.Text);
                var diastolic = int.Parse(this.diastolic.Text);
                var pulse = int.Parse(this.pulse.Text);
                var temp = double.Parse(temperature.Text);
                var weight = double.Parse(this.weight.Text);

                var appointmentDate = AppointmentDate.Date;
                var appointmentTime = AppointmentTime.Time;

                var time = appointmentTime;
                var date = appointmentDate.DateTime;

                var nurse = AccessValidator.CurrentUser as Nurse;
                var appointment = AppointmentManager.CurrentAppointment;
                var symptoms = new List<Symptom>();

                var items = patientSymptoms?.Items;
                if (items != null)
                    foreach (ListViewItem item in items)
                    {
                        var symptom = item.Tag as Symptom;
                        symptoms.Add(symptom);
                    }

                var details = new CheckUp(systolic, diastolic, patient, temp, date, time, nurse, weight, pulse,
                    symptoms, appointment);
                var result = CheckUpManager.Execute(details);

                if (result != null)
                {
                    doctorDiagnosisBtn.IsEnabled = true;
                    homeBtn.IsEnabled = false;
                    checkupBtn.IsEnabled = false;

                    //display complete check-in and checkup dialog

                    this.systolic.IsEnabled = false;
                    this.diastolic.IsEnabled = false;
                    this.pulse.IsEnabled = false;
                    temperature.IsEnabled = false;
                    this.weight.IsEnabled = false;
                    AppointmentDate.IsEnabled = false;
                    AppointmentTime.IsEnabled = false;
                    patientSymptoms.IsEnabled = false;
                    knownSymptoms.IsEnabled = false;
                    addSymptomBtn.IsEnabled = false;
                    removeSymptomBtn.IsEnabled = false;

                    CheckUpManager.CurrentCheckUp = details;
                }
            }
        }

        private bool hasNullOrEmpty()
        {
            return string.IsNullOrEmpty(systolic.Text) || string.IsNullOrEmpty(diastolic.Text) ||
                   string.IsNullOrEmpty(temperature.Text) ||
                   string.IsNullOrEmpty(pulse.Text) || string.IsNullOrEmpty(weight.Text);
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void systolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(systolic.Text)) systolic.Text = "";
        }

        private bool isNotThreeDigitsNorHasChars(string text)
        {
            return !Regex.IsMatch(text, "^(.*[^0-9]|)(1000|[1-9]\\d{0,2})([^0-9].*|)$") ||
                   text.Any(character => char.IsLetter(character));
        }

        private void diastolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(diastolic.Text)) diastolic.Text = "";
        }

        private void weight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(weight.Text)) weight.Text = "";
        }

        private void pulse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(pulse.Text)) pulse.Text = "";
        }

        private void temperature_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(temperature.Text)) temperature.Text = "";
        }

        private void addSymptom_Click(object sender, RoutedEventArgs e)
        {
            var selectedSymptom = knownSymptoms.SelectedItem as ListViewItem;
            var isOnePatientsSymptoms = !patientSymptoms.Items?.Contains(selectedSymptom) ?? false;
            if (isOnePatientsSymptoms)
            {
                knownSymptoms.Items?.Remove(selectedSymptom);
                patientSymptoms.Items?.Add(selectedSymptom);
            }
        }

        private void removeSymptom_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatientSymptom = patientSymptoms.SelectedItem as ListViewItem;
            patientSymptoms.Items?.Remove(selectedPatientSymptom);
            knownSymptoms.Items?.Add(selectedPatientSymptom);
        }

        private void knownSymptoms_Loaded(object sender, RoutedEventArgs e)
        {
            var symptoms = SymptomManager.Symptoms;
            foreach (var symptom in symptoms)
                if (symptom != null)
                {
                    var item = new ListViewItem
                    {
                        Tag = symptom, Content = symptom.Name
                    };

                    knownSymptoms.Items?.Add(item);
                }
        }

        private void doctorDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DoctorDiagnosis));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var checkup = CheckUpManager.CurrentCheckUp;

            var diagnosisResult = e.Parameter != null && (bool) e.Parameter;

            var previousPage = Frame.BackStack.Last();

            if (previousPage?.SourcePageType != typeof(DoctorDiagnosis)) return;

            if (!diagnosisResult)
            {
                if (checkup != null) handleFalsePageReturn(checkup);

                return;
            }


            handleTruePageReturn(checkup);

            base.OnNavigatedTo(e);
        }

        private void handleFalsePageReturn(CheckUp checkup)
        {
            systolic.Text = checkup.Systolic.ToString();
            diastolic.Text = checkup.Diastolic.ToString();
            pulse.Text = checkup.Pulse.ToString();
            temperature.Text = checkup.Temperature.ToString(CultureInfo.InvariantCulture);
            weight.Text = checkup.Weight.ToString(CultureInfo.InvariantCulture);
            AppointmentDate.Date = checkup.ArrivalDate;
            AppointmentTime.Time = checkup.ArrivalTime;

            doctorDiagnosisBtn.IsEnabled = true;
            homeBtn.IsEnabled = true;
            checkupBtn.IsEnabled = false;

            systolic.IsEnabled = false;
            diastolic.IsEnabled = false;
            pulse.IsEnabled = false;
            temperature.IsEnabled = false;
            weight.IsEnabled = false;
            AppointmentDate.IsEnabled = false;
            AppointmentTime.IsEnabled = false;
            patientSymptoms.IsEnabled = false;
            knownSymptoms.IsEnabled = false;
            addSymptomBtn.IsEnabled = false;
            removeSymptomBtn.IsEnabled = false;
        }

        private void handleTruePageReturn(CheckUp checkup)
        {
            systolic.Text = checkup.Systolic.ToString();
            diastolic.Text = checkup.Diastolic.ToString();
            pulse.Text = checkup.Pulse.ToString();
            temperature.Text = checkup.Temperature.ToString(CultureInfo.InvariantCulture);
            weight.Text = checkup.Weight.ToString(CultureInfo.InvariantCulture);
            AppointmentDate.Date = checkup.ArrivalDate;
            AppointmentTime.Time = checkup.ArrivalTime;

            doctorDiagnosisBtn.IsEnabled = false;
            homeBtn.IsEnabled = true;
            checkupBtn.IsEnabled = false;

            systolic.IsEnabled = false;
            diastolic.IsEnabled = false;
            pulse.IsEnabled = false;
            temperature.IsEnabled = false;
            weight.IsEnabled = false;
            AppointmentDate.IsEnabled = false;
            AppointmentTime.IsEnabled = false;
            patientSymptoms.IsEnabled = false;
            knownSymptoms.IsEnabled = false;
            addSymptomBtn.IsEnabled = false;
            removeSymptomBtn.IsEnabled = false;
        }
    }
}