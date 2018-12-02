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
    public sealed partial class OrderTest : Page
    {

        public OrderTest()
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

            this.orderBtn.IsEnabled = false;
        }

        private void orderBtn_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan time;
            DateTime date;

            var dateTimeOffset = this.orderTime.Date;
            if (dateTimeOffset != null)
            {
                time = dateTimeOffset.Value.TimeOfDay;
                date = dateTimeOffset.Value.Date;
            }

            var patientId = AppointmentManager.CurrentAppointment.Patient.Id;
            var appointmentId = (int) AppointmentManager.CurrentAppointment.ID;

            List<Test> tests = new List<Test>();

            ItemCollection items = this.TestToOrder?.Items;
            if (items != null)
            {
                foreach (ListViewItem item in items)
                {
                    Test aTest = item.Tag as Test;
                    tests.Add(aTest);
                }
            }

            //create order object

            //TestOrderDAL.AddTestOrder(order);
            this.Frame.Navigate(typeof(DoctorDiagnosis), true);
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

        private void addTest_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedTest = this.Tests.SelectedItem as ListViewItem;
            bool isOneTest = !this.TestToOrder.Items?.Contains(selectedTest) ?? false;
            if (isOneTest)
            {
                this.Tests.Items?.Remove(selectedTest);
                this.TestToOrder.Items?.Add(selectedTest);
            }
        }

        private void removeTest_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedTest = this.TestToOrder.SelectedItem as ListViewItem;
            this.TestToOrder.Items?.Remove(selectedTest);
            this.Tests.Items?.Add(selectedTest);
        }

        private void TestsToOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.orderBtn.IsEnabled = this.TestToOrder.Items?.Count > 0;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DoctorDiagnosis), false);
        }
    }
}
