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
            try
            {
                DataAccess.SaveUser(User);
                NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
