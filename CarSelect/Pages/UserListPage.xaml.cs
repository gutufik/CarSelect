﻿using CarSelect.Data;
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
    /// Interaction logic for UserListPage.xaml
    /// </summary>
    public partial class UsersListPage : Page
    {
        public List<User> Users { get; set; }
        public UsersListPage()
        {
            InitializeComponent();
            Users = DataAccess.GetUsers();
            DataContext = this;
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as MenuItem).DataContext as User;

            if (user != null)
            {
                NavigationService.Navigate(new UserPage(user));
            }
        }
    }
}
