using CarSelect.Data;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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
    /// Interaction logic for TariffsStatisticPage.xaml
    /// </summary>
    public partial class TariffsStatisticPage : Page
    {
        public ICollection<Tariff> Tariffs { get; set; }
        public ICollection<ISeries> CountSeries { get; set; }
        public ICollection<ISeries> SumSeries { get; set; }

        public TariffsStatisticPage(List<Tariff> tariffs)
        {
            InitializeComponent();

            Tariffs = tariffs;

            CountSeries = new List<ISeries>();
            SumSeries = new List<ISeries>();

            foreach (var tariff in Tariffs)
            {
                CountSeries.Add(new PieSeries<double>
                {
                    Values = new double[] { tariff.Requests.Count },
                    Name = tariff.Name,
                    DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue.Total} ({p.StackedValue.Share:P2})"
                });

                SumSeries.Add(new PieSeries<double>
                {
                    Values = new double[] { (double)tariff.Requests.Sum(r => tariff.Price * (r.Car == null ? 0 : r.Car.Price)) },
                    Name = tariff.Name,
                    DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue.Total} ({p.StackedValue.Share:P2})"
                });
            }

            DataContext = this;
        }
    }
}
