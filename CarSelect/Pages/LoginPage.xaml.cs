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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();

            tbLogin.Text = Properties.Settings.Default.Login;
            cbRemember.IsChecked = !string.IsNullOrWhiteSpace(tbLogin.Text);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void pbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

        private void Login()
        {
            string login = tbLogin.Text;
            string password = pbPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((App.User = DataAccess.Login(login, password)) != null)
            {
                if (cbRemember.IsChecked.GetValueOrDefault())
                    Properties.Settings.Default.Login = login;
                else
                    Properties.Settings.Default.Login = null;
                Properties.Settings.Default.Save();

                NavigationService.Navigate(new CarsListPage());
                (App.Current.MainWindow as MainWindow).ChangeNavigationVisibility(App.User.Role.Name);
            }
            else
                MessageBox.Show("Неправильный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
