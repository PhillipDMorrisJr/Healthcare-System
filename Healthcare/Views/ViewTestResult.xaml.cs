using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewTestResult : Page
    {
        public ViewTestResult()
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

            var testResults = TestResultManager.GetRefresheResults();

            foreach (var result in testResults)
                if (result.OrderId == TestOrderManager.CurrentTestOrder.OrderId)
                    TestResultManager.CurrentTestResult = result;

            ResultDate.Date = TestResultManager.CurrentTestResult.Date;
            ResultTime.Time = TestResultManager.CurrentTestResult.Time;
            reading.Text = TestResultManager.CurrentTestResult.Readings ? "Positive" : "Negative";
        }

        private void BackBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentDetails));
        }
    }
}