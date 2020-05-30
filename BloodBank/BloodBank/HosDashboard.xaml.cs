using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for HosDashboard.xaml
    /// </summary>
    public partial class HosDashboard : Window
    {
        private string id, name, phone, location, city, website, email, type;

        public HosDashboard(
            string id,
            string name,
            string phone,
            string location,
            string city,
            string website,
            string email,
            string type)
        {
            InitializeComponent();
            this.id = id;
            this.name = name;
            this.phone = phone;
            this.location = location;
            this.city = city;
            this.website = website;
            this.email = email;
            this.type = type;
            if (type.Equals("66"))
            {
                ReqBlood.Visibility = Visibility.Collapsed;
            }
            else if (type.Equals("72"))
            {
                ReqView.Visibility = Visibility.Collapsed;
            }
            hosDashboard.Content = new HosHomePage(id, name, type);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Minimise_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            img.Visibility = Visibility.Collapsed;
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            img.Visibility = Visibility.Visible;
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "home":
                    hosDashboard.Content = new HosHomePage(id, name, type);
                    break;
                case "ReqBlood":
                    hosDashboard.Content = new HosBloodRquestPage(id);
                    break;
                case "ReqView":
                    hosDashboard.Content = new BB_ReqViewPage(id);
                    break;
                case "orders":
                    hosDashboard.Content = new HosOrdersPage(id);
                    break;
                case "settings":
                    hosDashboard.Content = new HosSettingsPage(id);
                    break;
                case "logout":
                    LoginPage login = new LoginPage();
                    this.Hide();
                    login.Show();
                    break;
                default:
                    hosDashboard.Content = new HosHomePage(id, name, type);
                    break;
            }
        }
    }
}
