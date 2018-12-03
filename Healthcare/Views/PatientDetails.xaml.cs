using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientDetails : Page
    {
        public PatientDetails()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            if (PatientManager.CurrentPatient != null)
            {
                var currentAddress = PatientManager.GetAddressById(PatientManager.CurrentPatient.AddressId);

                if (currentAddress != null)
                {
                    street.Text = currentAddress.Street;
                    state.Text = currentAddress.State;
                    zip.Text = currentAddress.Zip.ToString();
                }

                fname.Text = PatientManager.CurrentPatient.FirstName;
                lname.Text = PatientManager.CurrentPatient.LastName;
                bday.Date = PatientManager.CurrentPatient.Dob;
                long.TryParse(PatientManager.CurrentPatient.Phone, out var pNumber);
                phone.Text = string.Format("{0:(###) ###-####}", pNumber);
                gender.Text = PatientManager.CurrentPatient.Gender;

                var social = "***-**-" + PatientManager.CurrentPatient.Ssn.ToString().Substring(5);
                ssn.Text = social;
            }
        }

        private void home_onClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}