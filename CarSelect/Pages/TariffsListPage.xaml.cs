using CarSelect.Data;
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
    /// Interaction logic for TariffsListPage.xaml
    /// </summary>
    public partial class TariffsListPage : Page
    {
        public List<Tariff> Tariffs { get; set; }
        public TariffsListPage()
        {
            InitializeComponent();
            Tariffs = DataAccess.GetTariffs();
            DataContext = this;
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            var tariff = (sender as MenuItem).DataContext as Tariff;
            if (tariff != null)
                NavigationService.Navigate(new TariffPage(tariff));
        }

        private void btnNewTariff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TariffPage(new Tariff()));
        }

        private void btnTariffsStatistics_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TariffsStatisticPage(Tariffs));
        }
    }
}
