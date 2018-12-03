using System;
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
    public sealed partial class RecordFinalDiagnosis : Page
    {
        private readonly Diagnosis finalDiagnosis;

        public RecordFinalDiagnosis()
        {
            InitializeComponent();

            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            finalDiagnosis = findFinalDiagnosis();

            if (finalDiagnosis != null)
            {
                diagnosisBox.Text = finalDiagnosis.CheckupDiagnosis;
                FinalDiagnosisDate.Date = finalDiagnosis.Date;
                FinalDiagnosisTime.Time = finalDiagnosis.Time;
            }
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentDetails));
        }

        private string getTestName(Order order)
        {
            var name = string.Empty;

            foreach (var test in TestManager.Tests)
                if (test.Code == order.Code)
                    name = test.Name;

            return name;
        }

        private void Diagnoses_Loaded(object sender, RoutedEventArgs e)
        {
            var checkups = CheckUpManager.GetRefreshedCheckUps();
            var diagnoses = DiagnosisManager.GetRefreshedDiagnoses();

            var currentiDiagnoses = new List<Diagnosis>();

            foreach (var checkup in checkups)
            foreach (var diagnosis in diagnoses)
                if (checkup.cuID == diagnosis.CuId &&
                    checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    currentiDiagnoses.Add(diagnosis);

            foreach (var diagnosis in currentiDiagnoses)
                if (diagnosis != null)
                {
                    var item = new ListViewItem
                    {
                        Tag = diagnosis, Content = diagnosis.CheckupDiagnosis
                    };

                    Diagnoses.Items?.Add(item);
                }
        }

        private void Tests_Loaded(object sender, RoutedEventArgs e)
        {
            var checkups = CheckUpManager.GetRefreshedCheckUps();
            var orders = TestOrderManager.GetRefreshedOrders();

            var currentOrders = new List<Order>();

            foreach (var checkup in checkups)
            foreach (var order in orders)
                if (checkup.cuID == order.CuId && checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    currentOrders.Add(order);

            foreach (var order in currentOrders)
                if (order != null)
                {
                    var item = new ListViewItem
                    {
                        Tag = order, Content = getTestName(order)
                    };

                    Tests.Items?.Add(item);
                }
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var date = FinalDiagnosisDate.Date.Date;
                var time = FinalDiagnosisTime.Time;
                var apptId = (int) AppointmentManager.CurrentAppointment.ID;
                var cuId = this.finalDiagnosis.CuId;
                var diagnosisId = this.finalDiagnosis.DiagnosisId;

                var finalDiagnosis = new RecordedDiagnosis(diagnosisId, apptId, cuId, date, time);
                RecordedDiagnosesDAL.AddRecordFinalDiagnosis(finalDiagnosis);
                Frame.Navigate(typeof(AppointmentDetails), true);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof(AppointmentDetails), false);
            }
        }

        private Diagnosis findFinalDiagnosis()
        {
            var checkups = CheckUpManager.GetRefreshedCheckUps();
            var diagnoses = DiagnosisManager.GetRefreshedDiagnoses();

            var currentiDiagnoses = new List<Diagnosis>();

            foreach (var checkup in checkups)
            foreach (var diagnosis in diagnoses)
                if (checkup.cuID == diagnosis.CuId &&
                    checkup.Appointment.ID == AppointmentManager.CurrentAppointment.ID)
                    currentiDiagnoses.Add(diagnosis);

            Diagnosis finalDiagnosis = null;

            foreach (var diagnosis in currentiDiagnoses)
                if (diagnosis.IsFinalDiagnosis)
                    finalDiagnosis = diagnosis;

            return finalDiagnosis;
        }
    }
}