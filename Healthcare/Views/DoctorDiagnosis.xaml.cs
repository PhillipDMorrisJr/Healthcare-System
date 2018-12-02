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
    public sealed partial class DoctorDiagnosis : Page
    {
        private static bool testReading;
        private Test test;
        private bool cancelBtOn = true;
        private bool orderBtnOn = true;

        public DoctorDiagnosis()
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

            this.cancelBtn.IsEnabled = cancelBtOn;
            this.orderTestsBtn.IsEnabled = orderBtnOn;

        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RoutineCheckUp), false);
        }

        private void NormalDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: handle normal diagnosis 
            this.Frame.Navigate(typeof(RoutineCheckUp), true);
        }

        private void FinalDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: handle final diagnosis 
            this.Frame.Navigate(typeof(RoutineCheckUp), true);
        }

        private void OrderTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OrderTest));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool orderResult = e.Parameter != null && (bool) e.Parameter;

            var previousPage = Frame.BackStack.Last();

            if (previousPage?.SourcePageType != typeof(OrderTest)) return;

            if (!orderResult) return;

            cancelBtOn = false;
            orderBtnOn = false;

            base.OnNavigatedTo(e);
        }
    }
}
