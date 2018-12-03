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
    public sealed partial class RecordFinalDiagnosis : Page
    {

        private Diagnosis finalDiagnosis;

        public RecordFinalDiagnosis()
        {
            this.InitializeComponent();

            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                             AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            finalDiagnosis = this.findFinalDiagnosis();

            if (finalDiagnosis != null)
            {
                this.diagnosisBox.Text = finalDiagnosis.CheckupDiagnosis;
                this.FinalDiagnosisDate.Date = finalDiagnosis.Date;
                this.FinalDiagnosisTime.Time = finalDiagnosis.Time;
            }           
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppointmentDetails));
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

        private void Diagnoses_Loaded(object sender, RoutedEventArgs e)
        {
            List<CheckUp> checkups = CheckUpManager.GetRefreshedCheckUps();
            List<Diagnosis> diagnoses = DiagnosisManager.GetRefreshedDiagnoses();

            List<Diagnosis> currentiDiagnoses = new List<Diagnosis>();

            foreach (var checkup in checkups)
            {
                foreach (var diagnosis in diagnoses)
                {
                    if (checkup.cuID == diagnosis.CuId && checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    {
                        currentiDiagnoses.Add(diagnosis);
                    }
                }
            }

            foreach (var diagnosis in currentiDiagnoses)
            {
                if (diagnosis != null)
                {
                    ListViewItem item = new ListViewItem
                    {                       
                        Tag = diagnosis, Content = diagnosis.CheckupDiagnosis
                    };

                    this.Diagnoses.Items?.Add(item);
                }
            }
        }

        private void Tests_Loaded(object sender, RoutedEventArgs e)
        {
            List<CheckUp> checkups = CheckUpManager.GetRefreshedCheckUps();
            List<Order> orders = TestOrderManager.GetRefreshedOrders();

            List<Order> currentOrders = new List<Order>();

            foreach (var checkup in checkups)
            {
                foreach (var order in orders)
                {
                    if (checkup.cuID == order.CuId && checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    {
                        currentOrders.Add(order);
                    }
                }
            }

            foreach (var order in currentOrders)
            {
                if (order != null)
                {
                    ListViewItem item = new ListViewItem
                    {                       
                        Tag = order, Content = this.getTestName(order)
                    };

                    this.Tests.Items?.Add(item);
                }
            }
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var date = this.FinalDiagnosisDate.Date.Date;
                var time = this.FinalDiagnosisTime.Time;
                var apptId = (int) AppointmentManager.CurrentAppointment.ID;
                var cuId = this.finalDiagnosis.CuId;
                var diagnosisId = this.finalDiagnosis.DiagnosisId;

                RecordedDiagnosis finalDiagnosis = new RecordedDiagnosis(diagnosisId, apptId, cuId, date, time);
                RecordedDiagnosesDAL.AddRecordFinalDiagnosis(finalDiagnosis);
                this.Frame.Navigate(typeof(AppointmentDetails) , true);
            }
            catch (Exception)
            {
                this.Frame.Navigate(typeof(AppointmentDetails) , false);
            }
        }

        private Diagnosis findFinalDiagnosis()
        {
            List<CheckUp> checkups = CheckUpManager.GetRefreshedCheckUps();
            List<Diagnosis> diagnoses = DiagnosisManager.GetRefreshedDiagnoses();

            List<Diagnosis> currentiDiagnoses = new List<Diagnosis>();

            foreach (var checkup in checkups)
            {
                foreach (var diagnosis in diagnoses)
                {
                    if (checkup.cuID == diagnosis.CuId && checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    {
                        currentiDiagnoses.Add(diagnosis);
                    }
                }
            }

            Diagnosis finalDiagnosis = null;

            foreach (var diagnosis in currentiDiagnoses)
            {
                if (diagnosis.IsFinalDiagnosis)
                {
                    finalDiagnosis = diagnosis;
                }
            }

            return finalDiagnosis;
        }
    }
}
