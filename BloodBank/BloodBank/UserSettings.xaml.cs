using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Page
    {
        string id, _ph_No, _name, _b_grp, _email, _loc, _city, _typ, _password, query;
        public UserSettings(string id)
        {
            InitializeComponent();
            this.id = id;
            Database d = new Database();
            try
            {
                query = "SELECT * FROM USER WHERE PH_NO='" + id + "';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader r = cmd.ExecuteReader();
                if (r.Read() && r.HasRows)
                {
                    ph_No.Content = _ph_No = r["PH_NO"].ToString();
                    name.Content = _name = r["NAME"].ToString();
                    b_Grp.Content = _b_grp = r["B_GRP"].ToString();
                    email.Content = _email = r["EMAIL"].ToString();
                    loc.Content = _loc = r["LOCATION"].ToString();
                    city.Content = _city = r["CITY"].ToString();
                    _typ = r["TYPE_OF_USER"].ToString();
                    _password= r["PASSWORD"].ToString();
                    if(_typ.Equals("68"))
                    {
                        typ.Content = _typ = "Donor";
                    }
                    else
                    {
                        typ.Content = _typ = "Recipient";
                    }
                }
                else
                {
                    MessageBox.Show("No details found for your login");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                d.closeConnection();
            }
        }

        private void changeLocation(object sender, RoutedEventArgs e)
        {
            chng_City_Grid.Visibility = Visibility.Collapsed;
            chng_Password_Grid.Visibility = Visibility.Collapsed;
            Thickness margin = chng_City_Button.Margin;
            margin.Top = 392;
            chng_City_Button.Margin = margin;
            margin = chng_Password_Button.Margin;
            margin.Top = 442;
            chng_Password_Button.Margin = margin;
            chng_Loc_Grid.Visibility = Visibility.Visible;
        }

        private void new_Loc_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (new_Loc.Text.Equals("New location"))
            {
                new_Loc.Clear();
                new_Loc.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void new_Loc_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (new_Loc.Text.Equals(""))
            {
                new_Loc.Foreground = new SolidColorBrush(Colors.Gray);
                new_Loc.Text = "New location";
            }
        }

        private void new_LocButton_Click(object sender, RoutedEventArgs e)
        {
            if (new_Loc.Text.Equals("New location"))
            {
                emptyLoc.Visibility = Visibility.Visible;
            }
            else
            {
                emptyLoc.Visibility = Visibility.Hidden;
                bool flag = true;
                Database d = new Database();
                try
                {
                    query = "UPDATE USER SET LOCATION='" + new_Loc.Text + "' WHERE PH_NO='" + id + "';";
                    d.openConnection();
                    SQLiteCommand cmd = new SQLiteCommand(query,d.con);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    flag = false;
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    d.closeConnection();
                }
                if(flag)
                {
                    MessageBox.Show("Location updated successfully");
                }
            }
        }

        private void changeCity(object sender, RoutedEventArgs e)
        {
            chng_Loc_Grid.Visibility = Visibility.Collapsed;
            chng_Password_Grid.Visibility = Visibility.Hidden;
            Thickness margin = chng_City_Button.Margin;
            margin.Top = 72;
            chng_City_Button.Margin = margin;
            margin = chng_Password_Button.Margin;
            margin.Top = 444;
            chng_Password_Button.Margin = margin;
            chng_City_Grid.Visibility = Visibility.Visible;
        }

        private void new_City_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (new_City.Text.Equals("New city"))
            {
                new_City.Clear();
                new_City.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void new_City_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (new_City.Text.Equals(""))
            {
                new_City.Foreground = new SolidColorBrush(Colors.Gray);
                new_City.Text = "New city";
            }
        }

        private void new_CityButton_Click(object sender, RoutedEventArgs e)
        {
            if (new_City.Text.Equals("New city"))
            {
                emptyCity.Visibility = Visibility.Visible;
            }
            else
            {
                emptyCity.Visibility = Visibility.Hidden;
                bool flag = true;
                Database d = new Database();
                try
                {
                    query = "UPDATE USER SET CITY='" + new_City.Text + "' WHERE PH_NO='" + id + "';";
                    d.openConnection();
                    SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    flag = false;
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    d.closeConnection();
                }
                if (flag)
                {
                    MessageBox.Show("City updated successfully");
                }
            }
        }

        private void changePassword(object sender, RoutedEventArgs e)
        {
            chng_Loc_Grid.Visibility = Visibility.Collapsed;
            chng_City_Grid.Visibility = Visibility.Collapsed;           
            Thickness margin = chng_City_Button.Margin;
            margin.Top = 72;
            chng_City_Button.Margin = margin;
            margin = chng_Password_Button.Margin;
            margin.Top = 124;
            chng_Password_Button.Margin=margin;
            chng_Password_Grid.Visibility = Visibility.Visible;
        }
        
        private void new_PassButton_Click(object sender, RoutedEventArgs e)
        {
            if(old_Pass.Password.Equals("") || new_Pass.Password.Equals("") || confirm_Pass.Password.Equals(""))
            {
                PasswordNotMatching.Visibility = Visibility.Hidden;
                invalidOldPassword.Visibility = Visibility.Hidden;
                emptyPassword.Visibility = Visibility.Visible;
            }
            else if(!new_Pass.Password.Equals(confirm_Pass.Password))
            {
                invalidOldPassword.Visibility = Visibility.Hidden;
                emptyPassword.Visibility = Visibility.Hidden;
                PasswordNotMatching.Visibility = Visibility.Visible;
            }
            else if(!old_Pass.Password.Equals(_password))
            {
                emptyPassword.Visibility = Visibility.Hidden;
                PasswordNotMatching.Visibility = Visibility.Hidden;
                invalidOldPassword.Visibility = Visibility.Visible;
            }
            else
            {
                emptyPassword.Visibility = Visibility.Hidden;
                PasswordNotMatching.Visibility = Visibility.Hidden;
                invalidOldPassword.Visibility = Visibility.Hidden;
                bool flag = true;
                Database d = new Database();
                try
                {
                    query = "UPDATE USER SET PASSWORD='" + new_Pass.Password + "' WHERE PH_NO='" + id + "';";
                    d.openConnection();
                    SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    flag = false;
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    d.closeConnection();
                }
                if (flag)
                {
                    MessageBox.Show("Password updated successfully");
                }
            }
        }
    }
}
