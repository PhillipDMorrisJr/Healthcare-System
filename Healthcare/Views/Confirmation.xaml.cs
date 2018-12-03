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
    public sealed partial class Confirmation : Page
    {
        private readonly Patient patient;

        public Confirmation()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;

            patient = PatientManager.CurrentPatient;

            if (patient != null)
            {
                name.Text = patient.FirstName + " " + patient.LastName;
                id.Text = patient.Id.ToString().PadLeft(4, '0');
                phone.Text = string.Format("{0:(###) ###-####}", patient.Phone);
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}