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
    /// Interaction logic for RequestsListPage.xaml
    /// </summary>
    public partial class RequestsListPage : Page
    {
        public List<Request> Requests { get; set; }

        public RequestsListPage()
        {
            InitializeComponent();
            Requests = DataAccess.GetRequests();
            DataAccess.RefreshList += DataAccess_RefreshList;


            DataContext = this;
        }

        private void DataAccess_RefreshList()
        {
            Requests = DataAccess.GetRequests();
            lvRequests.ItemsSource = Requests;
            lvRequests.Items.Refresh();
        }

        private void btnNewRequest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestPage(new Request()));
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as MenuItem).DataContext as Request;

            if (request != null)
            {
                NavigationService.Navigate(new RequestPage(request));
            }
        }
    }
}
