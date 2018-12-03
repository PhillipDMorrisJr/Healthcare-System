using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Healthcare.DAL;
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorDiagnosis : Page
    {
        private static bool testReading;
        private Test test;

        public DoctorDiagnosis()
        {
            InitializeComponent();

            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            doctor.Text = AppointmentManager.CurrentAppointment.Doctor.FullName;

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            cancelBtn.IsEnabled = true;
            orderTestsBtn.IsEnabled = true;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RoutineCheckUp), false);
        }

        private void NormalDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {
            var doctorId = AppointmentManager.CurrentAppointment.Doctor.Id;
            var cuId = CheckUpManager.CurrentCheckUp.cuID;

            var appointmentDate = AppointmentDate.Date;
            var appointmentTime = AppointmentTime.Time;

            var time = appointmentTime;
            var date = appointmentDate.DateTime;

            var checkupDiagnosis = diagnosisBox.Text;

            var diagnosis = new Diagnosis(cuId, doctorId, date, time, checkupDiagnosis, false);

            DiagnosisDAL.AddDiagnosis(diagnosis);
            Frame.Navigate(typeof(RoutineCheckUp), true);
        }

        private void FinalDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {
            var doctorId = AppointmentManager.CurrentAppointment.Doctor.Id;
            var cuId = CheckUpManager.CurrentCheckUp.cuID;

            var appointmentDate = AppointmentDate.Date;
            var appointmentTime = AppointmentTime.Time;

            var time = appointmentTime;
            var date = appointmentDate.DateTime;

            var checkupDiagnosis = diagnosisBox.Text;

            var diagnosis = new Diagnosis(cuId, doctorId, date, time, checkupDiagnosis, true);
            DiagnosisDAL.AddDiagnosis(diagnosis);
            Frame.Navigate(typeof(RoutineCheckUp), true);
        }

        private void OrderTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderTest));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var orderResult = e.Parameter != null && (bool) e.Parameter;

            var previousPage = Frame.BackStack.Last();

            if (previousPage?.SourcePageType != typeof(OrderTest)) return;

            if (!orderResult) return;

            cancelBtn.IsEnabled = false;
            orderTestsBtn.IsEnabled = false;
            finalDiagnosisBtn.IsEnabled = false;

            base.OnNavigatedTo(e);
        }
    }
}