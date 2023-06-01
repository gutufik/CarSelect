using CarSelect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for CarPage.xaml
    /// </summary>
    public partial class CarPage : Page
    {
        public List<BodyType> BodyTypes { get; set; }
        public List<Brand> Brands { get; set; }

        public List<Model> Models { get; set; }

        public CarPage(Car car)
        {
            InitializeComponent();
            BodyTypes = DataAccess.GetBodyTypes();
            Brands = DataAccess.GetBrands();
            Models = DataAccess.GetModels();


            DataContext = this;
        }
    }
}
