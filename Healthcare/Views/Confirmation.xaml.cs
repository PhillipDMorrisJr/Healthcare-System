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
using Healthcare.Model;
using Healthcare.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Confirmation : Page
    {
        private Patient patient;
        public Confirmation()
        {
            this.InitializeComponent();
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.Id;
            this.accessType.Text = AccessValidator.Access;

            this.patient = PatientManager.CurrentPatient;

            if (this.patient != null)
            {
                this.name.Text = this.patient.FirstName + " " + this.patient.LastName;
                this.id.Text = this.patient.Id.ToString();
                this.phone.Text = String.Format("{0:(###) ###-####}", this.patient.Phone);
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
