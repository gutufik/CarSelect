using CarSelect.Data;
using CarSelect.Services;
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
        public List<FuelType> FuelTypes { get; set; }
        public List<Tariff> Tariffs { get; set; }
        public List<DriveType> DriveTypes { get; set; }
        public List<GBType> GBTypes { get; set; }
        public List<BodyType> BodyTypes { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Model> Models { get; set; }

        public RequestPage(Request request)
        {
            InitializeComponent();
            this.Request = request;
            Users = DataAccess.GetUsers();
            States = DataAccess.GetStates();
            Clients = DataAccess.GetClients();
            Cars = DataAccess.GetCars();
            FuelTypes = DataAccess.GetFuelTypes();
            Tariffs = DataAccess.GetTariffs();
            DriveTypes = DataAccess.GetDriveTypes();
            GBTypes = DataAccess.GetGBTypes();
            BodyTypes = DataAccess.GetBodyTypes();
            Brands = DataAccess.GetBrands();
            Models = DataAccess.GetModels();

            dpStartDate.DisplayDateStart = DateTime.Now;
            dpEndDate.DisplayDateStart = request.Id == 0 ? DateTime.Now : request.StartDate;

            if (request.Id == 0)
                dpEndDate.IsEnabled = false;

            if (request.EndDate != null ||
                (request.State == null) ? false :
                ("Завершена_Отклонена".Contains(request.State.Name) ||
                (!request.State.Name.Contains("Новая") && App.User.Role.Name.Contains("Консультант"))))
                this.IsEnabled = false;

            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            if (Request.Client == null)
                sb.AppendLine("Необходимо выбрать клиента");
            if (Request.Tariff == null)
                sb.AppendLine("Необходимо выбрать тариф");
            if (Request.Car == null && Request.State.Name.Contains("Завершена"))
                sb.AppendLine("Невозможно завершить заявку, не выбрав автомобиль");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataAccess.SaveRequest(Request);
            NavigationService.GoBack();
        }

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (EmailService.SendEmail(Request))
            {
                MessageBox.Show("Ok");
            }
            else
                MessageBox.Show("Not ok");
        }
    }
}
