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
        private Test test;

        public EnterTestResult()
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

            this.submitBtn.IsEnabled = false;
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
            var time = this.ResultTime.Time;
            var code = this.test.Code;
            var reading = testReading;
            var testDiagnosis = this.diagnosis.Text;
            var patientId = AppointmentManager.CurrentAppointment.Patient.Id;
            var appointmentId = (int) AppointmentManager.CurrentAppointment.ID;

            var result = new TestResult(patientId, appointmentId, code, time, reading, testDiagnosis);
            TestResultDAL.AddTestResult(result);
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Tests_OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Test> tests =  TestManager.Tests;
            foreach (var test in tests)
            {
                if (test != null)
                {
                    ListViewItem item = new ListViewItem
                    {                       
                        Tag = test, Content = test.Name
                    };

                    this.Tests.Items?.Add(item);
                }
            }
        }

        private void Tests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.submitBtn.IsEnabled = true;
            this.test = (this.Tests.SelectedItem as ListViewItem)?.Tag as Test;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppointmentDetails));
        }
    }
}
