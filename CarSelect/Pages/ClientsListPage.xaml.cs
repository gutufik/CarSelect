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
    /// Interaction logic for ClientsListPage.xaml
    /// </summary>
    public partial class ClientsListPage : Page
    {
        public List<Client> Clients { get; set; }
        public ClientsListPage()
        {
            InitializeComponent();
            Clients = DataAccess.GetClients();
            DataContext = this;
        }

        private void miProfile_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as MenuItem).DataContext as Client;

            if (client != null)
            {
                NavigationService.Navigate(new ClientPage(client));
            }
        }

        private void miRequests_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as MenuItem).DataContext as Client;

            if (client != null)
            {
                NavigationService.Navigate(new RequestsListPage(client.Requests.ToList()));
            }
        }
    }
}
