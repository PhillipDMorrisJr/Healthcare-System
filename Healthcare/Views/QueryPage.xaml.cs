using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Healthcare.Utils;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Healthcare.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QueryPage : Page
    {
        private List<List<object>> o;

        public QueryPage()
        {
            InitializeComponent();
            nameID.Text = AccessValidator.CurrentUser.Username;
            userID.Text = AccessValidator.CurrentUser.Id;
            accessType.Text = AccessValidator.Access;
        }

        private void onLogout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void query_OnClick(object sender, RoutedEventArgs e)
        {
            var collection = new ObservableCollection<object>();
            Results.Columns.Clear();

            try
            {
                var table = CustomQuery.RetrieveResults(query.Text);
                resolveColumns(table);

                resolveRows(table, collection);

                Results.ItemsSource = collection;
                confirmation.Foreground = new SolidColorBrush(Colors.LawnGreen);
                confirmation.Text = "Success";
            }
            catch (Exception exc)
            {
                confirmation.Foreground = new SolidColorBrush(Colors.Yellow);
                confirmation.Text = exc.Message;
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void visitsBetween_Click(object sender, RoutedEventArgs e)
        {
            if (end.Date.Date <= begin.Date.Date)
            {
                confirmation.Foreground = new SolidColorBrush(Colors.Yellow);
                confirmation.Text = "The start date must be prior to the end date";
                return;
            }


            var collection = new ObservableCollection<object>();
            Results.Columns.Clear();
            try
            {
                var table = CustomQuery.RetrieveResultsBetweenDates(begin.Date, end.Date);
                resolveColumns(table);

                resolveRows(table, collection);

                Results.ItemsSource = collection;
                confirmation.Foreground = new SolidColorBrush(Colors.LawnGreen);
                confirmation.Text = "Success";
            }
            catch (Exception exc)
            {
                confirmation.Foreground = new SolidColorBrush(Colors.Yellow);
                confirmation.Text = exc.Message;
            }
        }

        private static void resolveRows(DataTable table, ObservableCollection<object> collection)
        {
            foreach (DataRow row in table.Rows) collection.Add(row.ItemArray);
        }

        private void resolveColumns(DataTable table)
        {
            for (var i = 0; i < table.Columns.Count; i++)
                Results.Columns.Add(new DataGridTextColumn
                {
                    Header = table.Columns[i].ColumnName,
                    Binding = new Binding {Path = new PropertyPath("[" + i + "]")}
                });
        }
    }
}