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
using System.Security.Cryptography;

namespace CarSelect.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
        public UserPage(User user)
        {
            InitializeComponent();
            User = user;
            Roles = DataAccess.GetRoles();
            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(User.LastName))
                sb.AppendLine("Необходимо заполнить фамилию");
            if (string.IsNullOrWhiteSpace(User.LastName))
                sb.AppendLine("Необходимо заполнить имя");
            if (string.IsNullOrWhiteSpace(User.Login))
                sb.AppendLine("Необходимо заполнить логин");
            if (User.Role == null)
                sb.AppendLine("Необходимо выбрать роль");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (tbNewPassword.Text == "")
                DataAccess.SaveUser(User);
            else
                DataAccess.SaveUser(User, tbNewPassword.Text);

            NavigationService.GoBack();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var passLength = 10;
            var chars = "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "0123456789" +
                "!@#$%";

            RandomNumberGenerator generator = RandomNumberGenerator.Create();

            char[] password = new char[passLength];
            byte[] randomBytes = new byte[passLength];

            generator.GetBytes(randomBytes);

            for (int i = 0; i < passLength; i++)
            {
                int randomCharIndex = randomBytes[i] % chars.Length;
                password[i] = chars[randomCharIndex];
            }

            tbNewPassword.Text = new string(password);
        }
    }
}
