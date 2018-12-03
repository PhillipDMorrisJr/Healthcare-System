using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewRecordFinalDiagnosis : Page
    {
        private readonly RecordedDiagnosis recordDiagnosis;

        public ViewRecordFinalDiagnosis()
        {
            InitializeComponent();

            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            name.Text = AppointmentManager.CurrentAppointment.Patient.FirstName + " " +
                        AppointmentManager.CurrentAppointment.Patient.LastName;

            phone.Text = string.Format("{0:(###) ###-####}", AppointmentManager.CurrentAppointment.Patient.Phone);
            ssn.Text = "***-**-" + AppointmentManager.CurrentAppointment.Patient.Ssn.ToString().Substring(5);

            recordDiagnosis = findRecordedFinalDiagnosis();

            foreach (var diagnosis in DiagnosisManager.DoctorDiagnoses)
                if (diagnosis.DiagnosisId == recordDiagnosis.DiagnosisId)
                    diagnosisBox.Text = diagnosis.CheckupDiagnosis;

            FinalDiagnosisDate.Date = recordDiagnosis.Date;
            FinalDiagnosisTime.Time = recordDiagnosis.Time;
        }

        private void CancelBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentDetails));
        }

        private RecordedDiagnosis findRecordedFinalDiagnosis()
        {
            var records = RecordDiagnosisManager.GetRefreshedRecordedDiagnoses();

            RecordedDiagnosis recordedDiagnosis = null;

            foreach (var record in records)
                if (record.ApptId == AppointmentManager.CurrentAppointment.ID)
                    recordedDiagnosis = record;

            return recordedDiagnosis;
        }
    }
}