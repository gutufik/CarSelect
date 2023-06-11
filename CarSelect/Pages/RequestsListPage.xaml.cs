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
    /// Interaction logic for RequestsListPage.xaml
    /// </summary>
    public partial class RequestsListPage : Page
    {
        private int ITEMSONPAGE = 10;
        private int _page = 0;
        private int pagesCount => (RequestsForFilters.Count / ITEMSONPAGE) + (RequestsForFilters.Count % ITEMSONPAGE == 0 ? 0 : 1);

        public List<Request> Requests { get; set; }
        public List<Request> RequestsForFilters { get; set; }
        public List<Client> Clients { get; set; }
        public List<State> States { get; set; }

        public RequestsListPage()
        {
            InitializeComponent();
            Requests = DataAccess.GetRequests();
            Clients = DataAccess.GetClients();
            States = DataAccess.GetStates();
            States.Insert(0, new State { Name = "Все", Requests = Requests.ToList() });

            DataAccess.RefreshList += DataAccess_RefreshList;


            DataContext = this;
        }
        public RequestsListPage(List<Request> requests)
        {
            InitializeComponent();
            Requests = requests;
            DataAccess.RefreshList += DataAccess_RefreshList;


            DataContext = this;
        }

        private void DataAccess_RefreshList()
        {
            Requests = DataAccess.GetRequests();
            States[0].Requests = Requests;
            ApplyFilters();
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            if (filtersChanged)
                _page = 0;

            var date = dpDate.SelectedDate;
            var client = cbClient.SelectedItem as Client;
            var state = cbState.SelectedItem as State;


            if (state == null)
                return;


            RequestsForFilters = state.Requests.Where(x => (date == null? true:  x.StartDate == date)
                                                            && (client == null? true: x.Client == client)).ToList();

            lvRequests.ItemsSource = RequestsForFilters.Skip(_page * ITEMSONPAGE).Take(ITEMSONPAGE);
            lvRequests.Items.Refresh();

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

        private void btnNewRequest_Click(object sender, RoutedEventArgs e)
        {
            var request = new Request() { User = App.User, StartDate = DateTime.Now };
            NavigationService.Navigate(new RequestPage(request)
            {
                Title = "Новая заявка"
            });
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as MenuItem).DataContext as Request;

            if (request != null)
                NavigationService.Navigate(new RequestPage(request)
                {
                    Title = $"Заявка №{request.Id}"
                });
        }

        private void btnRequestsStatistics_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestsByStatePage(Requests));
        }

        private void cbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            cbClient.ItemsSource = Clients.Where(p => p.ToString().ToLower().Contains(cbClient.Text.ToLower())).ToList();
            cbClient.IsDropDownOpen = true;
        }
    }
}
