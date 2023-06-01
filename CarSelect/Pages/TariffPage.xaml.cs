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
    /// Interaction logic for TariffPage.xaml
    /// </summary>
    public partial class TariffPage : Page
    {
        public Tariff Tariff { get; set; }
        public TariffPage(Tariff tariff)
        {
            InitializeComponent();
            Tariff = tariff;
            DataContext = this;
        }
    }
}
