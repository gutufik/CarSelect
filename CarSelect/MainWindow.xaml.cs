﻿using System;
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
using CarSelect.Data;
using CarSelect.Pages;

namespace CarSelect
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            frmMain.NavigationService.Navigate(new LoginPage());
            frmMain.Navigated += FrmMain_Navigated;
        }

        private void FrmMain_Navigated(object sender, NavigationEventArgs e)
        {
            var content = frmMain.Content as Page;

            var visibility = content is LoginPage ? Visibility.Collapsed : Visibility.Visible;

            btnUsers.Visibility = App.User != null && App.User.Role.Name == "Администратор"? Visibility.Visible:
                                                                                             Visibility.Collapsed;

            spNavigation.Visibility = visibility;
            spPagination.Visibility = visibility;

            this.Title = $"Автоподбор - {content.Title}";
        }

        private void btnCars_Click(object sender, RoutedEventArgs e)
        {
            frmMain.NavigationService.Navigate(new CarsListPage());
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            frmMain.NavigationService.Navigate(new ClientsListPage());
        }

        private void btnRequests_Click(object sender, RoutedEventArgs e)
        {
            frmMain.NavigationService.Navigate(new RequestsListPage());
        }

        private void btnTariffs_Click(object sender, RoutedEventArgs e)
        {
            frmMain.NavigationService.Navigate(new TariffsListPage());
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            frmMain.NavigationService.Navigate(new UsersListPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (frmMain.CanGoBack)
                frmMain.GoBack();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (frmMain.CanGoForward)
                frmMain.GoForward();
        }

        public void ChangeNavigationVisibility(string roleName)
        {
            if (roleName == "Консультант")
                btnUsers.Visibility = Visibility.Collapsed;
            else
                btnUsers.Visibility = Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Подтверждение",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.User = null;
                frmMain.NavigationService.Navigate(new LoginPage());
            }
        }
    }
}
