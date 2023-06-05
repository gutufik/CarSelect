using CarSelect.Data;
using CarSelect.Services;
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
    /// Interaction logic for ProfitByUsers.xaml
    /// </summary>
    public partial class ProfitByUsers : Page
    {
        public ICollection<User> Users { get; set; }
        public ICollection<ISeries> Series { get; set; }

        public ProfitByUsers(ICollection<User> users)
        {
            InitializeComponent();

            Users = users;

            Series = new List<ISeries>();
            foreach (var user in Users)
            {
                Series.Add(new PieSeries<double>
                {
                    Values = new double[] { (double)user.Requests.Sum(r => r.Tariff.Price * (r.Car == null ? 0 : r.Car.Price)) },
                    Name = user.ToString(),
                    DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue.Total} ({p.StackedValue.Share:P2})"
                });
            }

            DataContext = this;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportService.ExportUsersStatistics(Users.ToList());
            MessageBox.Show("Статистика успешно экспортирована.");
        }
    }
}
