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
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewTestResult : Page
    {
        public ViewTestResult()
        {
            this.InitializeComponent();

            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.doctor.Text = AppointmentManager.CurrentAppointment.Doctor.FullName;

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                             AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            List<TestResult> testResults = TestResultManager.GetRefresheResults();

            foreach (var result in testResults)
            {
                if (result.OrderId == TestOrderManager.CurrentTestOrder.OrderId)
                {
                    TestResultManager.CurrentTestResult = result;
                }
            }

            this.ResultDate.Date = TestResultManager.CurrentTestResult.Date;
            this.ResultTime.Time = TestResultManager.CurrentTestResult.Time;
            this.reading.Text = TestResultManager.CurrentTestResult.Readings ? "Positive" : "Negative";
        }

        private void BackBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppointmentDetails));
        }
    }
}
