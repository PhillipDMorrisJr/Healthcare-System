﻿using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Healthcare.Views
{
    public sealed partial class NewAppointment : Page
    {
        private Patient patient;
        /// <summary>
        /// Initializes a new instance of the <see cref="NewAppointment"/> class.
        /// </summary>
        public NewAppointment()
        {
            this.InitializeComponent();
            this.patient = PatientManager.CurrentPatient;
            this.nameID.Text = AccessValidator.CurrentUser.Username;
            this.userID.Text = AccessValidator.CurrentUser.ID;
            this.accessType.Text = AccessValidator.Access;
            this.name.Text = this.patient.FirstName + " " + this.patient.LastName;
            this.id.Text = this.patient.ID;
            this.phone.Text = String.Format("{0:(###) ###-####}", this.patient.Phone); 
        }
        
        /// <summary>
        /// Handles the Click event of the schedule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void schedule_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = this.AppointmentDate.Date.DateTime;
            TimeSpan time = this.AppointmentTime.Time;
            Appointment appt = new Appointment(this.patient, date, time);
            AppointmentManager.Appointments[this.patient].Add(appt);
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
 