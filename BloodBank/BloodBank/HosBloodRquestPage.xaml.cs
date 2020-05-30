using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for HosBloodRquestPage.xaml
    /// </summary>
    public partial class HosBloodRquestPage : Page
    {
        string id, bb_ID = null, or_ID = null;
        public HosBloodRquestPage(string id)
        {
            InitializeComponent();
            this.id = id;
            loadValues();
        }

        private void loadValues()
        {
            Database d = new Database();
            try
            {
                Req_List.Items.Clear();
                string query = "SELECT NAME FROM MED_INST WHERE TYPE_OF_MI='66';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Req_List.Items.Add(dr.GetString(dr.GetOrdinal("NAME")));
                    }
                }
                else
                {
                    Req_List.Items.Add("No blood banks");
                }
                Reciv_List.Items.Clear();
                query = "SELECT M.NAME, O.OR_ID FROM MED_INST M,ORDERS O WHERE DEL_DATE IS NULL AND O.DONOR_ID=M.MI_ID AND O.RECIP_ID='" + id + "';";
                cmd = new SQLiteCommand(query, d.con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Reciv_List.Items.Add(dr["OR_ID"].ToString() + " | " + dr.GetString(dr.GetOrdinal("NAME")));
                    }
                }
                else
                {
                    Reciv_List.Items.Add("No orders placed");
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

        private void Req_ComboBoxClosed(object sender, EventArgs e)
        {
            Reciv.Visibility = Visibility.Hidden;
            Req.Visibility = Visibility.Visible;
            Database d = new Database();
            try
            {
                string query = "SELECT MI_ID, NAME, EMAIL, LOCATION, CITY FROM MED_INST WHERE NAME='" + Req_List.Text + "';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        bb_ID = dr["MI_ID"].ToString();
                        BB_Name.Content = dr.GetString(dr.GetOrdinal("NAME"));
                        Email.Content = dr.GetString(dr.GetOrdinal("EMAIL"));
                        Loc.Content = dr.GetString(dr.GetOrdinal("LOCATION"));
                        City.Content = dr.GetString(dr.GetOrdinal("CITY"));
                    }
                }
                //else
                //{
                //    MessageBox.Show("No rows selected");
                //}
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

        private void BloodRequest_Click(object sender, RoutedEventArgs e)
        {
            if(Quantity.Text.Equals("") && !numberCheck(Quantity))
            {
                MessageBox.Show("Enter a valid number as quantity");
            }
            else
            {
                bool flag = true;
                Database d = new Database();
                string query = "INSERT INTO ORDERS(B_GRP,RECIP_ID,DONOR_ID,MI_ID,REQ_DATE,QUANTITY) VALUES(@B_GRP,@RECIP_ID,@DONOR_ID,@MI_ID,@REQ_DATE,@QUANTITY)";
                try
                {
                    d.openConnection();
                    SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                    cmd.Parameters.AddWithValue("@B_GRP", B_grp.Text);
                    cmd.Parameters.AddWithValue("@RECIP_ID", id);
                    cmd.Parameters.AddWithValue("@DONOR_ID", bb_ID);
                    cmd.Parameters.AddWithValue("@MI_ID", id);
                    cmd.Parameters.AddWithValue("@REQ_DATE", System.DateTime.Now);
                    cmd.Parameters.AddWithValue("@QUANTITY", Quantity.Text);
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
                    loadValues();
                }
                if (flag)
                {
                    MessageBox.Show("Order placed successfully");
                }
            }
        }

        private void Reciv_ComboBoxClosed(object sender, EventArgs e)
        {
            Req.Visibility = Visibility.Hidden;
            Reciv.Visibility = Visibility.Visible;
            Database d = new Database();
            try
            {
                string[] str = Reciv_List.Text.Split('|');
                or_ID = str[0].Trim();
                string query = "SELECT H.NAME, O.B_GRP, O.QUANTITY FROM ORDERS O, MED_INST H WHERE O.OR_ID='" + or_ID + "' AND H.MI_ID=O.DONOR_ID;";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        Reciv_Or_Id.Content = or_ID;
                        Reciv_BB_Name.Content = dr.GetString(dr.GetOrdinal("NAME"));
                        Reciv_B_grp.Content = dr.GetString(dr.GetOrdinal("B_GRP"));
                        Reciv_quantity.Text = dr["QUANTITY"].ToString();
                    }
                }
                //else
                //{
                //   MessageBox.Show("No rows selected");
                //}
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

        private void BloodRecived_Click(object sender, RoutedEventArgs e)
        {
            if (Reciv_quantity.Text.Equals("") || Reciv_Date.Text.Equals("") || !numberCheck(Reciv_quantity))
            {
                MessageBox.Show("Enter a valid value for quantity and received date");
            }
            else
            {
                bool flag = true;
                Database d = new Database();
                try
                {
                    string[] a = Reciv_Date.Text.Split('-');
                    string s = a[2] + "-" + a[1] + "-" + a[0] + " 00:00:00.0000000";
                    string query = "UPDATE ORDERS SET DEL_DATE=@DEL_DATE WHERE OR_ID='" + or_ID + "';";
                    d.openConnection();
                    SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                    cmd.Parameters.AddWithValue("@DEL_DATE", s);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    flag = false;
                    MessageBox.Show("<<<<<"+ex.Message+">>>>>");
                }
                finally
                {
                    d.closeConnection();
                    loadValues();
                }
                if (flag)
                {
                    MessageBox.Show("Order compeleted successfully");
                }
            }
        }

        private void number_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool numberCheck(TextBox textBox)
        {
            foreach (char i in textBox.Text)
            {
                if (i > '9' || i < '0')
                    return false;
            }
            return true;
        }
    }
}
