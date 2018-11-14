using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoutineCheckUp : Page
    {
        public RoutineCheckUp()
        {
            this.InitializeComponent();
        }

        private void checkup_Click(object sender, RoutedEventArgs e)
        {
            bool hasNull = systolic.Text == null || diastolic.Text == null || temperature.Text == null ||
                           pulse.Text == null || weight.Text == null;
            if (!hasNull)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            bool hasNull = systolic.Text == null || diastolic.Text == null || temperature.Text == null ||
                           pulse.Text == null || weight.Text == null;
            if (!hasNull)
            {
this.Frame.Navigate(typeof(MainPage));
            }
            
        }

        private void systolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isThreeDigits(systolic.Text))
            {
                systolic.Text = "";
            }
        }

        private bool isThreeDigits(string text)
        {
            return !Regex.IsMatch(text, "^(.*[^0-9]|)(1000|[1-9]\\d{0,2})([^0-9].*|)$") || text.Any(character => char.IsLetter(character));
        }

        private void diastolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isThreeDigits(diastolic.Text))
            {
                diastolic.Text = "";
            }
        }

        private void weight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isThreeDigits(weight.Text))
            {
                weight.Text = "";
            }
        }

        private void pulse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isThreeDigits(pulse.Text))
            {
                pulse.Text = "";
            }
        }

        private void temperature_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isThreeDigits(temperature.Text))
            {
                int temp = Int32.Parse(temperature.Text);
                if (temp > 0 && temp < 200)
                {
                    temperature.Text = "";
                }
                
            }
        }

        private void addSymptom_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeSymptom_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
