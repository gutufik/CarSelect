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
    /// Interaction logic for CarsListPage.xaml
    /// </summary>
    public partial class CarsListPage : Page
    {
        public List<Car> Cars { get; set; }
        public List<Car> CarsForFilters { get; set; }
        public List<Brand> Brands { get; set; }
        public List<BodyType> BodyTypes { get; set; }
        public Dictionary<string, Func<Car, object>> Sortings { get; set; }

        private int page = 0;

        private int pageSize = 10;

        private int pagesCount => (CarsForFilters.Count / pageSize) + (CarsForFilters.Count % pageSize == 0 ? 0 : 1);

        public CarsListPage()
        {
            InitializeComponent();
            Cars = DataAccess.GetCars();
            Brands = DataAccess.GetBrands();
            Brands.Insert(0, new Brand { Name = "Все"});

            BodyTypes = DataAccess.GetBodyTypes();
            BodyTypes.Insert(0, new BodyType { Name = "Все" }); 


            Sortings = new Dictionary<string, Func<Car, object>>() 
            {
                { "Без сортировки", x => x.Id },
                { "Сначала новые", x => x.ReleaseYear },
                { "Сначала старые", x => x.ReleaseYear },
            };
            DataAccess.RefreshList += DataAccess_RefreshList;


            DataContext = this;
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            if (filtersChanged)
                page = 0;

            var search = tbSearch.Text.ToLower().Trim();
            var bodyType = cbBodyType.SelectedItem as BodyType;
            var brand = cbBrand.SelectedItem as Brand;
            var sorting = cbSort.SelectedItem as string;

            if (string.IsNullOrEmpty(sorting) || bodyType == null || brand == null)
                return;

            CarsForFilters = Cars.Where(x => (x.Model.Name.ToLower().Contains(search)
                                            || x.Model.Brand.Name.ToLower().Contains(search))
                                            && (brand.Name == "Все"? true: x.Model.Brand == brand ) 
                                            && (bodyType.Name == "Все" ? true : x.BodyType == bodyType)).ToList();

            CarsForFilters = CarsForFilters.OrderBy(Sortings[sorting]).ToList();

            lvCars.ItemsSource = CarsForFilters.Skip(page * pageSize).Take(pageSize);
            lvCars.Items.Refresh();

            

            SetPageNumbers();
        }

        private void DataAccess_RefreshList()
        {
            Cars = DataAccess.GetCars();
            ApplyFilters();
        }

        private void SetPageNumbers()
        {
            spPagination.Children.Clear();

            spPagination.Children.Add(new TextBlock { Text = "<" });
            
            for (int i = 0; i < pagesCount; i++)
            {
                spPagination.Children.Add(new TextBlock() { Text = $"{i+1}" });
            }

            spPagination.Children.Add(new TextBlock { Text = ">" });
            
            foreach (var child in spPagination.Children)
            {
                (child as UIElement).MouseDown += CarsListPage_MouseDown;
            }
        }

        private void CarsListPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var content = (sender as TextBlock).Text;

            if (content.Contains("<") && page > 0)
                page--;
            else if (content.Contains(">") && page < pagesCount - 1)
                page++;
            else if (int.TryParse(content, out int newPage))
                page = newPage - 1;

            ApplyFilters(false);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void lvCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var car = lvCars.SelectedItem as Car;
            if (car != null)
                NavigationService.Navigate(new CarPage(car));

            lvCars.SelectedIndex = -1;
        }

        private void cbBodyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
