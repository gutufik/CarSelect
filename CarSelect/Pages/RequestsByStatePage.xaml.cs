using CarSelect.Data;
using LiveCharts;
using LiveCharts.Wpf;
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

namespace CarSelect.Pages
{
    /// <summary>
    /// Interaction logic for RequestsByStatePage.xaml
    /// </summary>
    public partial class RequestsByStatePage : Page
    {
        public ICollection<Request> Requests { get; set; }
        public SeriesCollection RequestsCount { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public RequestsByStatePage(ICollection<Request> requests)
        {
            InitializeComponent();

            Requests = requests;

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            var statesCount = requests.GroupBy(r => r.State)
                                      .Select(g => new { State = g.Key, Count = g.Count() });

            RequestsCount = new SeriesCollection();

            foreach (var stateCount in statesCount)
            {

                RequestsCount.Add(new PieSeries
                {
                    Title = stateCount.State.Name,
                    Values = new ChartValues<int> { stateCount.Count },
                    LabelPoint = PointLabel
                });
            }

            DataContext = this;
        }

        private void chartRequests_DataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
