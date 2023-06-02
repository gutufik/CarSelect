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
using System.Net.Mail;

namespace CarSelect.Pages
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public Client Client { get; set; }
        public ClientPage(Client client)
        {
            InitializeComponent();
            Client = client;
            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Client.LastName))
                sb.AppendLine("Необходимо заполнить фамилию");
            if (string.IsNullOrWhiteSpace(Client.LastName))
                sb.AppendLine("Необходимо заполнить имя");
            if (Client.Phone == null)
                sb.AppendLine("Необходимо заполнить телефон");
            else
            {
                if (Client.Phone.Length != 11)
                    sb.AppendLine("Некорректная длина телефона");
                if (!Client.Phone.StartsWith("79"))
                    sb.AppendLine("Некорректный формат телефона");
            }
            if (IsEmailValid(Client.Email))
                sb.AppendLine("Некорректный email адрес");


            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataAccess.SaveClient(Client);
            NavigationService.GoBack();
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private bool IsEmailValid(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
