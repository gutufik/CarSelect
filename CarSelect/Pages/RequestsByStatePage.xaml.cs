using CarSelect.Data;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
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
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.WPF;
using CarSelect.Services;

namespace CarSelect.Pages
{
    /// <summary>
    /// Interaction logic for RequestsByStatePage.xaml
    /// </summary>
    public partial class RequestsByStatePage : Page
    {
        public ICollection<Request> Requests { get; set; }
        public ICollection<ISeries> Series { get; set; }

        public RequestsByStatePage(ICollection<Request> requests)
        {
            InitializeComponent();

            Requests = requests;

            var statesCount = requests.GroupBy(r => r.State)
                                      .Select(g => new { StateName = g.Key.Name, Count = g.Count() });

            Series = new List<ISeries>();
            foreach (var stateCount in statesCount)
            {
                Series.Add(new PieSeries<double>
                {
                    Values = new double[] { stateCount.Count },
                    Name = stateCount.StateName,
                    DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue.Total} ({p.StackedValue.Share:P2})"
                });
            }

            DataContext = this;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportService.ExportUserStatistics(Requests.ToList());
            MessageBox.Show("Статистика успешно экспортирована.");
        }
    }
}
