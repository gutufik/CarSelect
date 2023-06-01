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
    /// Interaction logic for RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Page
    {
        public Request Request { get; set; }
        public List<User> Users { get; set; }
        public List<State> States { get; set; }
        public List<Client> Clients { get; set; }
        public List<Car> Cars { get; set; }
        public List<Tariff> Tariffs { get; set; }
        public RequestPage(Request request)
        {
            InitializeComponent();
            this.Request = request;
            Users = DataAccess.GetUsers();
            States = DataAccess.GetStates();
            Clients = DataAccess.GetClients();
            Cars = DataAccess.GetCars();
            Tariffs = DataAccess.GetTariffs();

            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataAccess.SaveRequest(Request);
                NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Заполните все поля");
            }

        }
    }
}
