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
using Healthcare.Model;
using Healthcare.Utils;

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
            if (!this.hasNullOrEmpty())
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private bool hasNullOrEmpty()
        {
            return string.IsNullOrEmpty(systolic.Text) || string.IsNullOrEmpty(diastolic.Text) || string.IsNullOrEmpty(temperature.Text) ||
                                                                               string.IsNullOrEmpty(pulse.Text) || string.IsNullOrEmpty(weight.Text);
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            if (!this.hasNullOrEmpty())
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            
        }

        private void systolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(systolic.Text))
            {
                systolic.Text = "";
            }
        }

        private bool isNotThreeDigitsNorHasChars(string text)
        {
            return !Regex.IsMatch(text, "^(.*[^0-9]|)(1000|[1-9]\\d{0,2})([^0-9].*|)$") || text.Any(character => char.IsLetter(character));
        }

        private void diastolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(diastolic.Text))
            {
                diastolic.Text = "";
            }
        }

        private void weight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(weight.Text))
            {
                weight.Text = "";
            }
        }

        private void pulse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(pulse.Text))
            {
                pulse.Text = "";
            }
        }

        private void temperature_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNotThreeDigitsNorHasChars(temperature.Text))
            {
                int temp = int.Parse(temperature.Text);
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

        private void knownSymptoms_Loaded(object sender, RoutedEventArgs e)
        {
            List<Symptom> symptoms =  SymptomManager.Symptoms;
            foreach (var symptom in symptoms)
            {
                if (symptom != null)
                {
                    ListViewItem item = new ListViewItem
                    {
                        Tag = symptom, Content = symptom.Name
                    };
                    this.knownSymptoms.Items?.Add(item);
                }
            }
        }
    }
}
