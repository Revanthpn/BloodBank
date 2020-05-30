using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for user.xaml
    /// </summary>
    public partial class user : Page
    {
        public user()
        {
            InitializeComponent();
        }

        char t = 'R';

        private void loadValues()
        {
            Database d = new Database();
            try
            {
                bbName.Items.Clear();
                string query = "SELECT NAME FROM MED_INST WHERE TYPE_OF_MI='66';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        bbName.Items.Add(dr.GetString(dr.GetOrdinal("NAME")));
                    }
                }
                else
                {
                    bbName.Items.Add("No blood banks");
                }
                HosName.Items.Clear();
                query = "SELECT NAME FROM MED_INST WHERE TYPE_OF_MI='72';";
                cmd = new SQLiteCommand(query, d.con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        HosName.Items.Add(dr.GetString(dr.GetOrdinal("NAME")));
                    }
                }
                else
                {
                    HosName.Items.Add("No hospitals");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                d.closeConnection();
            }
        }

            private void next_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text.Equals("") || email.Text.Equals("") || ph_no.Text.Equals("") || b_group.Text.Equals("") || location.Text.Equals("") || city.Text.Equals("") || password.Password.Equals("") || confirm_password.Password.Equals(""))
            {
                phNoError.Visibility = Visibility.Hidden;
                passError.Visibility = Visibility.Hidden;
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
                phNoError.Visibility = Visibility.Hidden;
                empty.Visibility = Visibility.Hidden;
                passError.Visibility = Visibility.Visible;
            }
            else
            {
                if ("Donor".Equals(type.Text))
                {
                    t = 'D';
                    Usersignup.Visibility = Visibility.Hidden;
                    DonorSignUp.Visibility = Visibility.Visible;
                    DSname.Content = name.Text;
                    DStype.Content = "Donor";
                    loadValues();
                }
                else if ("Recipient".Equals(type.Text))
                {
                    t = 'R';
                    Usersignup.Visibility = Visibility.Hidden;
                    RecipSignUp.Visibility = Visibility.Visible;
                    RSname.Content = name.Text;
                    RStype.Content = "Recipient";
                    loadValues();
                }
            }
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

        private void Dsubmit_Click(object sender, RoutedEventArgs e)
        {
            if (dob.Text.Equals("") || age.Content.Equals("") || weight.Text.Equals("") || lastDonationDate.Text.Equals("") || bbName.Text.Equals(""))
            {
                DSinvalidLastDonationDate.Visibility = Visibility.Hidden;
                DSinvalidDOB.Visibility = Visibility.Hidden;
                DSempty.Visibility = Visibility.Visible;
            }
            else
            {
                string[] a = lastDonationDate.Text.Split('-');
                string[] b = dob.Text.Split('-');
                int yr = int.Parse(a[2]);
                int m = int.Parse(a[1]);
                int day = int.Parse(a[0]);
                DateTime LastDonDate = new DateTime(yr, m, day);
                if (LastDonDate > DateTime.Now)
                {
                    DSinvalidDOB.Visibility = Visibility.Hidden;
                    DSempty.Visibility = Visibility.Hidden;
                    DSinvalidLastDonationDate.Visibility = Visibility.Visible;
                }
                else if (int.Parse(weight.Text) < 50 || int.Parse(weight.Text) >= 200)
                {
                    DSinvalidLastDonationDate.Visibility = Visibility.Hidden;
                    DSempty.Visibility = Visibility.Hidden;
                    DSinvalidDOB.Visibility = Visibility.Visible;
                }
                else
                {
                    DSinvalidDOB.Visibility = Visibility.Hidden;
                    DSempty.Visibility = Visibility.Hidden;
                    DSinvalidLastDonationDate.Visibility = Visibility.Hidden;
                    bool flag1 = true;
                    bool flag2 = submitUserDetails();
                    Database d = new Database();
                    try
                    {
                        d.openConnection();
                        string query = "INSERT INTO DONOR(PH_NO, DOB, WEIGHT, LAST_DONATION_DATE) VALUES(@PH_NO, @DOB, @WEIGHT, @LAST_DONATION_DATE);";
                        SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                        cmd.Parameters.AddWithValue("@PH_NO", ph_no.Text);
                        cmd.Parameters.AddWithValue("@DOB", b[2] + "-" + b[1] + "-" + b[0] + " 00:00:00.0000000");
                        cmd.Parameters.AddWithValue("@WEIGHT", weight.Text);
                        cmd.Parameters.AddWithValue("@LAST_DONATION_DATE", a[2] + "-" + a[1] + "-" + a[0] + " 00:00:00.0000000");
                        cmd.ExecuteNonQuery();
                        query = "SELECT MI_ID FROM MED_INST WHERE NAME='" + bbName.Text + "';";
                        cmd = new SQLiteCommand(query, d.con);
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if(dr.HasRows && dr.Read())
                        {
                            string miID = dr["MI_ID"].ToString();
                            query = "UPDATE USER SET MI_ID='" + miID + "' WHERE PH_NO='" + ph_no.Text + "';";
                            cmd = new SQLiteCommand(query, d.con);
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
                    if(flag1 && flag2)
                    {
                        MessageBox.Show("Donor registered");
                    }
                }
            }
        }

        private void Rsubmit_Click(object sender, RoutedEventArgs e)
        {
            if (HosName.Text.Equals(""))
            {
                RSempty.Visibility = Visibility.Visible;
            }
            else
            {
                RSempty.Visibility = Visibility.Hidden;
                bool flag2 = submitUserDetails();
                bool flag1 = true;
                Database d = new Database();
                try
                {
                    d.openConnection();
                    string query = "SELECT MI_ID FROM MED_INST WHERE NAME='" + HosName.Text + "';";
                    SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows && dr.Read())
                    {
                        string miID = dr["MI_ID"].ToString();
                        query = "UPDATE USER SET MI_ID='" + miID + "' WHERE PH_NO='" + ph_no.Text + "';";
                        cmd = new SQLiteCommand(query, d.con);
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
                    MessageBox.Show("Recipient registered");
                }
            }
        }

        private bool submitUserDetails()
        {
            Database d = new Database();
            bool flag = true;
            d.openConnection();
            string query = "INSERT INTO USER(PH_NO,NAME,B_GRP,EMAIL,LOCATION,CITY,PASSWORD,TYPE_OF_USER) VALUES(@PhNo,@NAME,@B_GRP,@EMAIL,@LOCATION,@CITY,@PASSWORD,@TYPE_OF_USER)";
            SQLiteCommand cmd = new SQLiteCommand(query, d.con);
            cmd.Parameters.AddWithValue("@PhNo", ph_no.Text);
            cmd.Parameters.AddWithValue("@NAME", name.Text);
            cmd.Parameters.AddWithValue("@B_GRP", b_group.Text);
            cmd.Parameters.AddWithValue("@EMAIL", email.Text);
            cmd.Parameters.AddWithValue("@LOCATION", location.Text);
            cmd.Parameters.AddWithValue("@CITY", city.Text);
            cmd.Parameters.AddWithValue("@PASSWORD", password.Password);
            cmd.Parameters.AddWithValue("@TYPE_OF_USER", t);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception excep)
            {
                flag = false;
                MessageBox.Show(excep.Message);
            }
            finally
            {
                d.closeConnection();
            }
            return flag;
        }

        private void dob_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            string[] a = dob.Text.Split('-');
            int dob_yr = int.Parse(a[2]);
            int dob_m = int.Parse(a[1]);
            int dob_d = int.Parse(a[0]);
            DateTime _dob = new DateTime(dob_yr, dob_m, dob_d);
            if(DateTime.Now.Year - _dob.Year < 18 || DateTime.Now.Year - _dob.Year > 65)
            {
                DSinvalidLastDonationDate.Visibility = Visibility.Hidden;
                DSempty.Visibility = Visibility.Hidden;
                DSinvalidDOB.Visibility = Visibility.Visible;
                age.Content = "";
            }
            else
            {
                DSinvalidDOB.Visibility = Visibility.Hidden;
                age.Content = DateTime.Now.Year - _dob.Year;
            }
        }

        private void number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
