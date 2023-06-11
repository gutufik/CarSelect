using CarSelect.Data;
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
    /// Interaction logic for ClientsListPage.xaml
    /// </summary>
    public partial class ClientsListPage : Page
    {
        private int ITEMSONPAGE = 10;
        private int _page = 0;
        private int pagesCount => (ClientsForFilters.Count / ITEMSONPAGE) + (ClientsForFilters.Count % ITEMSONPAGE == 0 ? 0 : 1);

        public List<Client> Clients { get; set; }
        public List<Client> ClientsForFilters { get; set; }
        public Dictionary<string, Func<Client, object>> Sortings { get; set; }
        public ClientsListPage()
        {
            InitializeComponent();
            Clients = DataAccess.GetClients();
            Sortings = new Dictionary<string, Func<Client, object>>
            {
                { "Без сортировки", x => x.Id },
                { "От А до Я", x => x.LastName },
                { "От Я до А", x => x.LastName }
            };
            DataAccess.RefreshList += DataAccess_RefreshList;

            DataContext = this;
        }

        private void DataAccess_RefreshList()
        {
            Clients = DataAccess.GetClients();
            ApplyFilters();
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            if (filtersChanged)
                _page = 0;

            var search = tbSearch.Text.ToLower().Trim();
            var sorting = cbSort.SelectedItem as string;

            if (string.IsNullOrEmpty(sorting))
                return;

            ClientsForFilters = Clients.Where(x => x.ToString().ToLower().Contains(search)).ToList();


            ClientsForFilters = ClientsForFilters.OrderBy(Sortings[sorting]).ToList();

            if (sorting.Contains("Я до А"))
                ClientsForFilters.Reverse();

            lvClients.ItemsSource = ClientsForFilters.Skip(_page * ITEMSONPAGE).Take(ITEMSONPAGE);
            lvClients.Items.Refresh();

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



        private void miProfile_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as MenuItem).DataContext as Client;

            if (client != null)
            {
                NavigationService.Navigate(new ClientPage(client)
                {
                    Title = client.ToString()
                });
            }
        }

        private void miRequests_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as MenuItem).DataContext as Client;

            if (client != null)
            {
                NavigationService.Navigate(new RequestsListPage(client.Requests.ToList())
                {
                    Title = $"Заявки клиента {client.ToString()}"
                });
            }
        }

        private void btnNewClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage(new Client()));
        }

        private void miNewRequest_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as MenuItem).DataContext as Client;

            if (client != null)
            {
                var request = new Request() { Client = client, User = App.User, StartDate = DateTime.Now };
                NavigationService.Navigate(new RequestPage(request)
                {
                    Title = $"Новая заявка для {client.ToString()}"
                });
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
