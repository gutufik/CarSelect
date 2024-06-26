﻿using CarSelect.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        private int ITEMSONPAGE = 6;
        private int _page = 0;
        private int pagesCount => (CarsForFilters.Count / ITEMSONPAGE) + (CarsForFilters.Count % ITEMSONPAGE == 0 ? 0 : 1);

        public List<Car> Cars { get; set; }
        public List<Car> CarsForFilters { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Model> Models { get; set; }
        public List<BodyType> BodyTypes { get; set; }
        public List<DriveType> DriveTypes { get; set; }
        public List<GBType> GBTypes { get; set; }
        public Dictionary<string, Func<Car, object>> Sortings { get; set; }


        public CarsListPage()
        {
            InitializeComponent();
            Cars = DataAccess.GetCars();

            Brands = DataAccess.GetBrands();
            Brands.Insert(0, new Brand { Name = "Все", Models = DataAccess.GetModels() });

            BodyTypes = DataAccess.GetBodyTypes();
            BodyTypes.Insert(0, new BodyType { Name = "Все" });

            DriveTypes = DataAccess.GetDriveTypes();
            DriveTypes.Insert(0, new DriveType { Name = "Все" });

            GBTypes = DataAccess.GetGBTypes();
            GBTypes.Insert(0, new GBType { Name = "Все" });


            Sortings = new Dictionary<string, Func<Car, object>>()
            {
                { "Без сортировки", x => x.Id },
                { "Сначала новые", x => x.ReleaseYear },
                { "Сначала старые", x => x.ReleaseYear },
                { "Сначала дешевые", x => x.Price },
                { "Сначала дорогие", x => x.Price },
            };
            DataAccess.RefreshList += DataAccess_RefreshList;

            if (App.User.Role.Name == "Консультант")
                btnNewCar.Visibility = Visibility.Collapsed;


            DataContext = this;
        }

        private void DataAccess_RefreshList()
        {
            Cars = DataAccess.GetCars();
            Brands[0].Models = DataAccess.GetModels();
            Models[0].Cars = Cars.ToList();
            ApplyFilters();
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            if (filtersChanged)
                _page = 0;

            var search = tbSearch.Text.ToLower().Trim();
            var bodyType = cbBodyType.SelectedItem as BodyType;
            var brand = cbBrand.SelectedItem as Brand;
            var model = cbModel.SelectedItem as Model;
            var driveType = cbDriveType.SelectedItem as DriveType;
            var gbType = cbGBType.SelectedItem as GBType;
            var sorting = cbSort.SelectedItem as string;

            if (string.IsNullOrEmpty(sorting) || bodyType == null || brand == null || model == null || gbType == null || driveType == null)
                return;

            CarsForFilters = model.Cars.Where(x => (x.Model.Name.ToLower().Contains(search) || x.Model.Brand.Name.ToLower().Contains(search))
                                            && (brand.Name == "Все" ? true : x.Model.Brand == brand)
                                            && (driveType.Name == "Все" ? true : x.DriveType == driveType)
                                            && (gbType.Name == "Все" ? true : x.GBType == gbType)
                                            && (bodyType.Name == "Все" ? true : x.BodyType == bodyType)).ToList();

            CarsForFilters = CarsForFilters.OrderBy(Sortings[sorting]).ToList();

            if (sorting.Contains("новые") || sorting.Contains("дорогие"))
                CarsForFilters.Reverse();

            lvCars.ItemsSource = CarsForFilters.Skip(_page * ITEMSONPAGE).Take(ITEMSONPAGE);
            lvCars.Items.Refresh();

            if (lvCars.Items.Count > 0)
                tbNotFound.Visibility = Visibility.Hidden;
            else
                tbNotFound.Visibility = Visibility.Visible;

            GeneratePages();
        }

        private void GeneratePages()
        {
            spPagination.Children.Clear();

            spPagination.Children.Add(new TextBlock
            {
                Text = "<",
                FontSize = 20,
                Margin = new Thickness(2, 0, 2, 0)
            });

            for (int i = 0; i < pagesCount; i++)
            {
                spPagination.Children.Add(new TextBlock()
                {
                    Text = $"{i + 1}",
                    FontSize = 20,
                    Margin = new Thickness(2, 0, 2, 0)
                });
            }

            spPagination.Children.Add(new TextBlock
            {
                Text = ">",
                FontSize = 20,
                Margin = new Thickness(2, 0, 2, 0)
            });

            foreach (var child in spPagination.Children)
            {
                (child as UIElement).MouseDown += Paginator;
            }
            if (spPagination.Children.Count != 0)
                (spPagination.Children[_page + 1] as TextBlock).TextDecorations = TextDecorations.Underline;
        }

        private void Paginator(object sender, MouseButtonEventArgs e)
        {
            var content = (sender as TextBlock).Text;

            if (content.Contains("<") && _page > 0)
                _page--;
            else if (content.Contains(">") && _page < pagesCount - 1)
                _page++;
            else if (int.TryParse(content, out int newPage))
                _page = newPage - 1;

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
                NavigationService.Navigate(new CarPage(car)
                {
                    Title = car.ToString()
                });

            lvCars.SelectedIndex = -1;
        }

        private void cbBodyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var brand = cbBrand.SelectedItem as Brand;
            if (brand == null)
                return;

            Models = brand.Models.ToList();
            Models.Insert(0, new Model { Name = "Все", Cars = Cars });
            cbModel.ItemsSource = Models;
            cbModel.SelectedIndex = 0;
            ApplyFilters();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnNewCar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CarPage(new Car())
            {
                Title = "Новый автомобиль"
            });
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            gridSearch.Visibility = gridSearch.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void cbDriveType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbGBType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
