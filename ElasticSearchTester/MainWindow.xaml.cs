using Nest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElasticSearchTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get data to index
                List<Dictionary<string, object>> documents = new DataAccess().GetData();

                ESUtility utility = new ESUtility(esUrlTextBox.Text);
                utility.Index(documents);
                MessageBox.Show("Data added");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ESUtility utility = new ESUtility(esUrlTextBox.Text);
                utility.DropIndex();
                MessageBox.Show("Index deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ESUtility utility = new ESUtility(esUrlTextBox.Text);
                ISearchResponse<Dictionary<string, object>> results = utility.Query(queryTextBox.Text);
                StringBuilder sb = new StringBuilder();

                foreach (Hit<Dictionary<string, object>> result in results.Hits)
                {
                    sb.Append("+++++START RESULT+++++\n\n");

                    foreach (string key in result.Source.Keys)
                    {
                        if (result.Source[key].GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                        {
                            foreach (var value in (Newtonsoft.Json.Linq.JArray)result.Source[key])
                            {
                                sb.AppendFormat("{0}: {1}\n", key, value.ToString());
                            }
                        }
                        else
                        {
                            sb.AppendFormat("{0}: {1}\n", key, result.Source[key]);
                        }
                    }

                    sb.Append("\n+++++END RESULT+++++\n\n");
                }

                resultsTextBox.Text = sb.ToString();
                MessageBox.Show(string.Format("Query submitted. {0} results.", results.HitsMetaData.Total));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ESUtility utility = new ESUtility(esUrlTextBox.Text);
                utility.Delete(queryTextBox.Text);
                MessageBox.Show("Data deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
