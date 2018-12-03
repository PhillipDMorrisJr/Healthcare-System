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
    public sealed partial class ViewRecordFinalDiagnosis : Page
    {

        private RecordedDiagnosis recordDiagnosis;

        public ViewRecordFinalDiagnosis()
        {
            this.InitializeComponent();

            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                             AppointmentManager.CurrentAppointment.Patient.LastName;

            this.phone.Text = String.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            this.ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            recordDiagnosis = this.findRecordedFinalDiagnosis();

            foreach (var diagnosis in DiagnosisManager.DoctorDiagnoses)
            {
                if (diagnosis.DiagnosisId == recordDiagnosis.DiagnosisId)
                {
                    this.diagnosisBox.Text = diagnosis.CheckupDiagnosis;
                }
            }

            this.FinalDiagnosisDate.Date = recordDiagnosis.Date;
            this.FinalDiagnosisTime.Time = recordDiagnosis.Time;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppointmentDetails));
        }           

        private RecordedDiagnosis findRecordedFinalDiagnosis()
        {
            List<RecordedDiagnosis> records = RecordDiagnosisManager.GetRefreshedRecordedDiagnoses();

            RecordedDiagnosis recordedDiagnosis = null;

            foreach (var record in records)
            {
                if (record.ApptId == AppointmentManager.CurrentAppointment.ID)
                {
                    recordedDiagnosis = record;
                }
            }

            return recordedDiagnosis;
        }
    }
}
