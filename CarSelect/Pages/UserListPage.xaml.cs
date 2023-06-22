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
    /// Interaction logic for UserListPage.xaml
    /// </summary>
    public partial class UsersListPage : Page
    {
        private int ITEMSONPAGE = 10;
        private int _page = 0;
        private int pagesCount => (UsersForFilters.Count / ITEMSONPAGE) + (UsersForFilters.Count % ITEMSONPAGE == 0 ? 0 : 1);

        public List<User> Users { get; set; }
        public List<User> UsersForFilters { get; set; }
        public List<Role> Roles { get; set; }

        public UsersListPage()
        {
            InitializeComponent();

            Users = DataAccess.GetUsers();
            Roles = DataAccess.GetRoles();
            Roles.Insert(0, new Role { Name = "Все роли", Users = Users });

            if (App.User.Role.Name == "Консультант")
                btnUsersStatistic.Visibility = Visibility.Hidden;

            DataAccess.RefreshList += DataAccess_RefreshList;

            DataContext = this;
        }

        private void DataAccess_RefreshList()
        {
            Users = DataAccess.GetUsers();
            Roles[0].Users = Users;
            ApplyFilters();
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as MenuItem).DataContext as User;

            if (user != null)
                NavigationService.Navigate(new UserPage(user)
                {
                    Title = $"{user.LastName} {user.FirstName}"
                });
        }

        private void miRequestsStatistic_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as MenuItem).DataContext as User;

            if (user != null)
                NavigationService.Navigate(new RequestsByStatePage(user.Requests));

        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserPage(new User())
            {
                Title = "Новый пользователь"
            });
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            if (filtersChanged)
                _page = 0;

            var search = tbSearch.Text.ToLower().Trim();
            var role = cbRole.SelectedItem as Role;

            if (role == null)
                return;

            UsersForFilters = role.Users.Where(x => x.FirstName.ToLower().Contains(search) ||
                                              x.LastName.ToLower().Contains(search) ||
                                              (x.Patronymic == null? false: x.Patronymic.ToLower().Contains(search)) ||
                                              x.Login.ToLower().Contains(search)).ToList();

            lvUsers.ItemsSource = UsersForFilters.Skip(_page * ITEMSONPAGE).Take(ITEMSONPAGE);
            lvUsers.Items.Refresh();

            if (lvUsers.Items.Count > 0)
                tbNotFound.Visibility = Visibility.Hidden;
            else
                tbNotFound.Visibility = Visibility.Visible;

            GeneratePages();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
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

        private void btnUsersStatistic_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfitByUsers(Users));
        }

        private void lvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var user = lvUsers.SelectedItem as User;

            if (user != null)
                NavigationService.Navigate(new UserPage(user)
                {
                    Title = $"{user.LastName} {user.FirstName}"
                });

            lvUsers.SelectedIndex = -1;
        }
    }
}
