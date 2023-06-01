using CarSelect.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Car Car { get; set; }

        public CarPage(Car car)
        {
            InitializeComponent();
            Car = car;
            BodyTypes = DataAccess.GetBodyTypes();
            Brands = DataAccess.GetBrands();
            Models = Car.Model.Brand.Models.ToList();

            if (Car.Requests.Count != 0)
                this.IsEnabled = false;

            DataContext = this;
        }

        private void btnChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Image|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog().Value)
            {
                Car.Image = File.ReadAllBytes(openFileDialog.FileName);
                imgCar.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.SaveCar(Car);
            NavigationService.GoBack();
        }

        private void cbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var brand = (cbBrand.SelectedItem as Brand);

            if (brand == null)
                return;

            Models = brand.Models.ToList();

            cbModel.ItemsSource = Models;
            cbModel.SelectedItem = null;
        }
    }
}
