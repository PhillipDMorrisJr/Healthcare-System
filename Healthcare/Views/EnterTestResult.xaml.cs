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
    public sealed partial class EnterTestResult : Page
    {
        private static bool testReading;

        public EnterTestResult()
        {
            this.InitializeComponent();

            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            foreach (var doctor in DoctorManager.Doctors)
            {
                if (doctor.Id == DiagnosisManager.CurrentDiagnosis.DoctorId)
                {
                    this.doctor.Text = doctor.FullName;
                }
            }

            List<Order> orders = TestOrderManager.GetRefreshedOrders();

            foreach (var order in orders)
            {
                this.test.Text = this.getTestName(order);
            }

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                             AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);
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
            var appointmentDate = this.AppointmentDate.Date;
            var appointmentTime = this.AppointmentTime.Time;

            var time = appointmentTime;
            var date = appointmentDate.DateTime;

            var currentOrder = TestOrderManager.CurrentTestOrder;

            var orderId = currentOrder.OrderId;
            var reading = testReading;

            var result = new TestResult(orderId, date, time, reading);
            TestResultDAL.AddTestResult(result);
            this.Frame.Navigate(typeof(CheckupList));
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckupList));
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
