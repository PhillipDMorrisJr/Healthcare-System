using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class CheckupList : Page
    {
        public CheckupList()
        {
            this.InitializeComponent();

            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                             AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            this.enterTestBtn.IsEnabled = false;
            this.viewTestBtn.IsEnabled = false;
        }

        private void Checkups_Loaded(object sender, RoutedEventArgs e)
        {
            List<CheckUp> checkups = CheckUpManager.GetRefreshedCheckUps();

            List<CheckUp> appointmentCheckups = new List<CheckUp>();

            foreach (var checkup in checkups)
            {
                if (checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                {
                    appointmentCheckups.Add(checkup);
                }
            }

            foreach (var checkup in appointmentCheckups)
            {
                if (checkup != null)
                {
                    ListViewItem item = new ListViewItem
                    {                       
                        Tag = checkup, Content = "Checkup: " + checkup.ArrivalDate.ToString("yyyy-mm-dd")
                    };

                    this.Checkups.Items?.Add(item);
                }
            }
        }

        private void TestOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TestOrderManager.CurrentTestOrder = (this.TestOrders.SelectedItem as ListViewItem)?.Tag as Order;

            List<TestTaken> testsTaken = TestTakenManager.GetRefreshedTestsTaken();

            foreach (var taken in testsTaken)
            {
                if (TestOrderManager.CurrentTestOrder != null && taken.OrderId == TestOrderManager.CurrentTestOrder.OrderId)
                {
                    TestTakenManager.CurrentTestTaken = taken;
                }
            }

            this.enterTestBtn.IsEnabled = !TestTakenManager.CurrentTestTaken.IsTaken;
            this.viewTestBtn.IsEnabled = TestTakenManager.CurrentTestTaken.IsTaken;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppointmentDetails));
        }

        private void Checkups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.TestOrders.Items?.Clear();

            CheckUpManager.CurrentCheckUp = (this.Checkups.SelectedItem as ListViewItem)?.Tag as CheckUp;

            List<Diagnosis> diagnoses = DiagnosisManager.GetRefreshedDiagnoses();           

            foreach (var diagnosis in diagnoses)
            {
                if (CheckUpManager.CurrentCheckUp != null && diagnosis.CuId == CheckUpManager.CurrentCheckUp.cuID)
                {
                    DiagnosisManager.CurrentDiagnosis = diagnosis;
                }
            }

            foreach (var doctor in DoctorManager.Doctors)
            {
                if (doctor.Id == DiagnosisManager.CurrentDiagnosis.DoctorId)
                {
                    this.doctor.Text = doctor.FullName;
                }
            }

            this.diagnosisBox.Text = DiagnosisManager.CurrentDiagnosis.CheckupDiagnosis;

            List<Order> orders = TestOrderManager.GetRefreshedOrders();
            List<Order> currentOrders = new List<Order>();

            foreach (var order in orders)
            {
                if (CheckUpManager.CurrentCheckUp != null && order.CuId == CheckUpManager.CurrentCheckUp.cuID)
                {
                    currentOrders.Add(order);
                }
            }

            foreach (var order in currentOrders)
            {
                if (order != null)
                {
                    var name = this.getTestName(order);

                    ListViewItem item = new ListViewItem
                    {                         
                        Tag = order, Content = name
                    };

                    this.TestOrders.Items?.Add(item);
                }
            }

            this.enterTestBtn.IsEnabled = false;
            this.viewTestBtn.IsEnabled = false;
        }

        private void EnterResultsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EnterTestResult));
        }

        private void ViewResultsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewTestResult));
        }

        private string getTestName(Order order)
        {
            string name = string.Empty;

            foreach (var test in TestManager.Tests)
            {
                if (test.Code == order.Code)
                {
                    name = test.Name;
                }
            }

            return name;
        }
    }
}
