using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CheckupList : Page
    {
        public CheckupList()
        {
            InitializeComponent();

            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            enterTestBtn.IsEnabled = false;
            viewTestBtn.IsEnabled = false;
        }

        private void Checkups_Loaded(object sender, RoutedEventArgs e)
        {
            var checkups = CheckUpManager.GetRefreshedCheckUps();

            var appointmentCheckups = new List<CheckUp>();

            foreach (var checkup in checkups)
                if (checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    appointmentCheckups.Add(checkup);

            foreach (var checkup in appointmentCheckups)
                if (checkup != null)
                {
                    var item = new ListViewItem
                    {
                        Tag = checkup, Content = "Checkup: " + checkup.ArrivalDate.ToString("yyyy-mm-dd")
                    };

                    Checkups.Items?.Add(item);
                }
        }

        private void TestOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TestOrderManager.CurrentTestOrder = (TestOrders.SelectedItem as ListViewItem)?.Tag as Order;

            var testsTaken = TestTakenManager.GetRefreshedTestsTaken();

            foreach (var taken in testsTaken)
                if (TestOrderManager.CurrentTestOrder != null &&
                    taken.OrderId == TestOrderManager.CurrentTestOrder.OrderId)
                    TestTakenManager.CurrentTestTaken = taken;

            enterTestBtn.IsEnabled = !TestTakenManager.CurrentTestTaken.IsTaken;
            viewTestBtn.IsEnabled = TestTakenManager.CurrentTestTaken.IsTaken;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentDetails));
        }

        private void Checkups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TestOrders.Items?.Clear();

            CheckUpManager.CurrentCheckUp = (Checkups.SelectedItem as ListViewItem)?.Tag as CheckUp;

            var diagnoses = DiagnosisManager.GetRefreshedDiagnoses();

            foreach (var diagnosis in diagnoses)
                if (CheckUpManager.CurrentCheckUp != null && diagnosis.CuId == CheckUpManager.CurrentCheckUp.cuID)
                    DiagnosisManager.CurrentDiagnosis = diagnosis;

            foreach (var doctor in DoctorManager.Doctors)
                if (doctor.Id == DiagnosisManager.CurrentDiagnosis.DoctorId)
                    this.doctor.Text = doctor.FullName;

            diagnosisBox.Text = DiagnosisManager.CurrentDiagnosis.CheckupDiagnosis;

            var orders = TestOrderManager.GetRefreshedOrders();
            var currentOrders = new List<Order>();

            foreach (var order in orders)
                if (CheckUpManager.CurrentCheckUp != null && order.CuId == CheckUpManager.CurrentCheckUp.cuID)
                    currentOrders.Add(order);

            foreach (var order in currentOrders)
                if (order != null)
                {
                    var name = getTestName(order);

                    var item = new ListViewItem
                    {
                        Tag = order, Content = name
                    };

                    TestOrders.Items?.Add(item);
                }

            enterTestBtn.IsEnabled = false;
            viewTestBtn.IsEnabled = false;
        }

        private void EnterResultsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EnterTestResult));
        }

        private void ViewResultsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewTestResult));
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