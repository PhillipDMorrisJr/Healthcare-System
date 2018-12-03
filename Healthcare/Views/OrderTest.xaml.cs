using System.Collections.Generic;
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
    public sealed partial class OrderTest : Page
    {
        public OrderTest()
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

            orderBtn.IsEnabled = false;
        }

        private void orderBtn_Click(object sender, RoutedEventArgs e)
        {
            var appointmentDate = AppointmentDate.Date;
            var appointmentTime = AppointmentTime.Time;

            var time = appointmentTime;
            var date = appointmentDate.DateTime;

            var doctorId = AppointmentManager.CurrentAppointment.Doctor.Id;
            var cuId = CheckUpManager.CurrentCheckUp.cuID;


            var orders = new List<Order>();

            var items = TestToOrder?.Items;
            if (items != null)
                foreach (ListViewItem item in items)
                {
                    if (!(item.Tag is Test aTest)) continue;

                    var code = aTest.Code;
                    var order = new Order(cuId, code, date, time, doctorId);
                    orders.Add(order);
                }

            TestOrderDAL.AddTestOrders(orders);
            Frame.Navigate(typeof(DoctorDiagnosis), true);
        }

        private void Tests_OnLoaded(object sender, RoutedEventArgs e)
        {
            var tests = TestManager.Tests;
            foreach (var test in tests)
                if (test != null)
                {
                    var item = new ListViewItem
                    {
                        Tag = test, Content = test.Name
                    };

                    Tests.Items?.Add(item);
                }
        }

        private void addTest_Click(object sender, RoutedEventArgs e)
        {
            var selectedTest = Tests.SelectedItem as ListViewItem;
            var isOneTest = !TestToOrder.Items?.Contains(selectedTest) ?? false;
            if (isOneTest)
            {
                Tests.Items?.Remove(selectedTest);
                TestToOrder.Items?.Add(selectedTest);
            }
        }

        private void removeTest_Click(object sender, RoutedEventArgs e)
        {
            var selectedTest = TestToOrder.SelectedItem as ListViewItem;
            TestToOrder.Items?.Remove(selectedTest);
            Tests.Items?.Add(selectedTest);
        }

        private void TestsToOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderBtn.IsEnabled = TestToOrder.Items?.Count > 0;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DoctorDiagnosis), false);
        }
    }
}