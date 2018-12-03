using System.Linq;
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
    public sealed partial class AppointmentDetails : Page
    {
        public AppointmentDetails()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            AppointmentDate.Date = AppointmentManager.CurrentAppointment.AppointmentDateTime;
            description.Text = AppointmentManager.CurrentAppointment.Description;
            AppointmentTime.Time = AppointmentManager.CurrentAppointment.AppointmentTime;

            doctor.Text = AppointmentManager.CurrentAppointment.Doctor.FullName;

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            checkedIn.Text = AppointmentManager.CurrentAppointment.IsCheckedIn ? "Yes" : "No";

            checkupListBtn.IsEnabled = AppointmentManager.CurrentAppointment.IsCheckedIn;

            RecordedDiagnosis recordedDiagnosis = null;

            foreach (var record in RecordDiagnosisManager.GetRefreshedRecordedDiagnoses())
                if (record.ApptId == AppointmentManager.CurrentAppointment.ID)
                    recordedDiagnosis = record;

            if (recordedDiagnosis == null)
            {
                finalResultBtn.IsEnabled = true;
                viewResultBtn.IsEnabled = false;
            }
            else
            {
                finalResultBtn.IsEnabled = false;
                viewResultBtn.IsEnabled = true;
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void CheckupListBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CheckupList));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var recordResult = e.Parameter != null && (bool) e.Parameter;

            var previousPage = Frame.BackStack.Last();

            if (previousPage?.SourcePageType != typeof(RecordFinalDiagnosis)) return;

            if (!recordResult) return;

            finalResultBtn.IsEnabled = false;
            viewResultBtn.IsEnabled = true;

            base.OnNavigatedTo(e);
        }

        private void FinalResultBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecordFinalDiagnosis));
        }

        private void ViewResultBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewRecordFinalDiagnosis));
        }
    }
}