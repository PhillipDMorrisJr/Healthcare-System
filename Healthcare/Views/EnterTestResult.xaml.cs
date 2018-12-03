using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.DAL;
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnterTestResult : Page
    {
        private static bool testReading;

        public EnterTestResult()
        {
            InitializeComponent();

            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            foreach (var doctor in DoctorManager.Doctors)
                if (doctor.Id == DiagnosisManager.CurrentDiagnosis.DoctorId)
                    this.doctor.Text = doctor.FullName;

            var orders = TestOrderManager.GetRefreshedOrders();

            foreach (var order in orders) test.Text = getTestName(order);

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);
        }

        private void positiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            testReading = true;
        }

        private void negativeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            testReading = false;
        }

        private void submitResult_Click(object sender, RoutedEventArgs e)
        {
            var appointmentDate = AppointmentDate.Date;
            var appointmentTime = AppointmentTime.Time;

            var time = appointmentTime;
            var date = appointmentDate.DateTime;

            var currentOrder = TestOrderManager.CurrentTestOrder;

            var orderId = currentOrder.OrderId;
            var reading = testReading;

            var result = new TestResult(orderId, date, time, reading);
            TestResultDAL.AddTestResult(result);
            Frame.Navigate(typeof(CheckupList));
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CheckupList));
        }

        private string getTestName(Order order)
        {
            var name = string.Empty;

            foreach (var test in TestManager.Tests)
                if (test.Code == order.Code)
                    name = test.Name;

            return name;
        }
    }
}