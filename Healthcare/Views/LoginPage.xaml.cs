using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            loginValidator.Visibility = Visibility.Collapsed;
        }

        private void onLogin_Click(object sender, RoutedEventArgs e)
        {
            var access = AccessValidator.ConfirmUserAccess(UserNameBox.Text, PasswordBox.Password);

            if (access)
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                PasswordBox.Password = "";
                loginValidator.Visibility = Visibility.Visible;
            }
        }
    }
}