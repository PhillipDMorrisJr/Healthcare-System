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
    public sealed partial class AppointmentDetails : Page
    {
        public AppointmentDetails()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.AppointmentDate.Date = AppointmentManager.CurrentAppointment.AppointmentDateTime;
            this.description.Text = AppointmentManager.CurrentAppointment.Description;
            this.AppointmentTime.Time = AppointmentManager.CurrentAppointment.AppointmentTime;
           
            this.doctor.Text = AppointmentManager.CurrentAppointment.Doctor.FullName;

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                                AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            this.checkedIn.Text = AppointmentManager.CurrentAppointment.IsCheckedIn ? "Yes" : "No";     

            this.checkupListBtn.IsEnabled = AppointmentManager.CurrentAppointment.IsCheckedIn;

            RecordedDiagnosis recordedDiagnosis = null;

            foreach (var record in RecordDiagnosisManager.GetRefreshedRecordedDiagnoses())
            {
                if (record.ApptId == AppointmentManager.CurrentAppointment.ID)
                {
                    recordedDiagnosis = record;
                }
            }

            if (recordedDiagnosis == null)
            {
                this.finalResultBtn.IsEnabled = true;
                this.viewResultBtn.IsEnabled = false;
            }
            else
            {
                this.finalResultBtn.IsEnabled = false;
                this.viewResultBtn.IsEnabled = true;
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void CheckupListBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckupList));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool recordResult = e.Parameter != null && (bool) e.Parameter;

            var previousPage = Frame.BackStack.Last();

            if (previousPage?.SourcePageType != typeof(RecordFinalDiagnosis)) return;

            if (!recordResult) return;

            this.finalResultBtn.IsEnabled = false;
            this.viewResultBtn.IsEnabled = true;

            base.OnNavigatedTo(e);
        }

        private void FinalResultBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RecordFinalDiagnosis));
        }

        private void ViewResultBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewRecordFinalDiagnosis));
        }
    }
}
