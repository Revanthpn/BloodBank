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
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for med_inst.xaml
    /// </summary>
    public partial class med_inst : Page
    {
        public med_inst()
        {
            InitializeComponent();
        }

        char t = 'H';

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if(name.Text.Equals("") || email.Text.Equals("") || ph_no.Text.Equals("") || website.Text.Equals("") || location.Text.Equals("") || city.Text.Equals("") || password.Password.Equals("") || confirm_password.Password.Equals(""))
            {
                passError.Visibility = Visibility.Hidden;
                phNoError.Visibility = Visibility.Hidden;
                empty.Visibility = Visibility.Visible;
            }
            else if (!phNoCheck())
            {
                passError.Visibility = Visibility.Hidden;
                empty.Visibility = Visibility.Hidden;
                phNoError.Visibility = Visibility.Visible;
            }
            else if (!password.Password.Equals(confirm_password.Password))
            {
                empty.Visibility = Visibility.Hidden;
                phNoError.Visibility = Visibility.Hidden;
                passError.Visibility = Visibility.Visible;
            }
            else
            {
                passError.Visibility = Visibility.Hidden;
                phNoError.Visibility = Visibility.Hidden;
                empty.Visibility = Visibility.Hidden;
                if ("Hospital".Equals(type.Text))
                {
                    t = 'H';
                    bool flag2 = submitMedInstDetails();
                    if(flag2)
                    {
                        MessageBox.Show("Hospital registered");
                    }
                }
                else if("Blood Bank".Equals(type.Text))
                {
                    t = 'B';
                    MIsignup.Visibility = Visibility.Hidden;
                    Stock.Visibility = Visibility.Visible;
                }
            }
        }

        private bool submitMedInstDetails()
        {
            Database d = new Database();
            bool flag = true;
            d.openConnection();
            string query = "INSERT INTO MED_INST(NAME,PHONE,LOCATION,CITY,WEBSITE,EMAIL,PASSWORD,TYPE_OF_MI) VALUES(@NAME,@PHONE,@LOCATION,@CITY,@WEBSITE,@EMAIL,@PASSWORD,@TYPE_OF_MI)";
            SQLiteCommand cmd = new SQLiteCommand(query, d.con);
            cmd.Parameters.AddWithValue("@NAME", name.Text);
            cmd.Parameters.AddWithValue("@PHONE", ph_no.Text);
            cmd.Parameters.AddWithValue("@LOCATION", location.Text);
            cmd.Parameters.AddWithValue("@CITY", city.Text);
            cmd.Parameters.AddWithValue("@WEBSITE", website.Text);
            cmd.Parameters.AddWithValue("@EMAIL", email.Text);
            cmd.Parameters.AddWithValue("@PASSWORD", password.Password);
            cmd.Parameters.AddWithValue("@TYPE_OF_MI", t);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message);
                flag = false;
            }
            finally
            {
                d.closeConnection();
            }
            return flag;
        }

        private bool phNoCheck()
        {
            bool flag = true;
            foreach (char i in ph_no.Text)
            {
                if (i > '9' || i < '0')
                    flag = false;
            }
            if (ph_no.Text.Length == 10 && flag)
                return true;
            else
                return false;
        }
        private void number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BBsubmit_Click(object sender, RoutedEventArgs e)
        {
            if(quantityAP.Text.Equals("") ||
               quantityBP.Text.Equals("") ||
               quantityOP.Text.Equals("") ||
               quantityABP.Text.Equals("") ||
               quantityAN.Text.Equals("") ||
               quantityBN.Text.Equals("") ||
               quantityON.Text.Equals("") ||
               quantityABN.Text.Equals("") ||
               rateAP.Text.Equals("") ||
               rateBP.Text.Equals("") ||
               rateOP.Text.Equals("") ||
               rateABP.Text.Equals("") ||
               rateAN.Text.Equals("") ||
               rateBN.Text.Equals("") ||
               rateON.Text.Equals("") ||
               rateABN.Text.Equals(""))
            {
                MessageBox.Show("All fields must be filled to submit");
            }
            else
            {
                bool flag2 = submitMedInstDetails();
                bool flag1 = true;
                Database d = new Database();
                try
                {
                    d.openConnection();
                    string query = "SELECT MI_ID FROM MED_INST WHERE NAME='" + name.Text + "';";
                    SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows && dr.Read())
                    {
                        string miID = dr["MI_ID"].ToString();
                        query = "INSERT INTO STOCK(MI_ID, B_GRP, QUANTITY, RATE) VALUES(@MI_ID, @B_GRP, @QUANTITY, @RATE);";
                        cmd = new SQLiteCommand(query, d.con);
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "A+");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityAP.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateAP.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "B+");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityBP.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateBP.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "O+");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityOP.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateOP.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "AB+");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityABP.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateABP.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "A-");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityAN.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateAN.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "B-");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityBN.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateBN.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "O-");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityON.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateON.Text);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.AddWithValue("@MI_ID", miID);
                        cmd.Parameters.AddWithValue("@B_GRP", "AB-");
                        cmd.Parameters.AddWithValue("@QUANTITY", quantityABN.Text);
                        cmd.Parameters.AddWithValue("@RATE", rateABN.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception excep)
                {
                    flag1 = false;
                    MessageBox.Show(excep.Message);
                }
                finally
                {
                    d.closeConnection();
                }
                if (flag1 && flag2)
                {
                    MessageBox.Show("Blood bank registered");
                }
            }
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
