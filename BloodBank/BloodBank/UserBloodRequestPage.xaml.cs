using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for UserBloodRequestPage.xaml
    /// </summary>
    public partial class UserBloodRequestPage : Page
    {
        string id, d_id = null, hos_id, or_Id;
        public UserBloodRequestPage(string id, string hos_id)
        {
            InitializeComponent();
            this.id = id;
            this.hos_id = hos_id;
            loadValues();
        }
        private void loadValues()
        {
            Database d = new Database();
            try
            {
                Req_List.Items.Clear();
                string query = "SELECT NAME FROM USER WHERE TYPE_OF_USER='68';";
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
                    Req_List.Items.Add("No donors");
                }
                Reciv_List.Items.Clear();
                query = "SELECT U.NAME, O.OR_ID FROM USER U,ORDERS O WHERE DEL_DATE IS NULL AND O.DONOR_ID=U.PH_NO AND O.RECIP_ID='" + id + "';";
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
                    Reciv_List.Items.Add("No donors");
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
                string query = "SELECT PH_NO, NAME, B_GRP, EMAIL, LOCATION, CITY FROM USER WHERE NAME='" + Req_List.Text + "';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        d_id = dr["PH_NO"].ToString();
                        D_Name.Text = dr.GetString(dr.GetOrdinal("NAME"));
                        B_grp.Text = dr.GetString(dr.GetOrdinal("B_GRP"));
                        Email.Text = dr.GetString(dr.GetOrdinal("EMAIL"));
                        Loc.Text = dr.GetString(dr.GetOrdinal("LOCATION"));
                        City.Text = dr.GetString(dr.GetOrdinal("CITY"));
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

        private void Donate_ComboBoxClosed(object sender, EventArgs e)
        {
            Req.Visibility = Visibility.Hidden;
            Reciv.Visibility = Visibility.Visible;
            Database d = new Database();
            try
            {
                string[] str = Reciv_List.Text.Split('|');
                or_Id = str[0].Trim();
                string query = "SELECT U.NAME, O.B_GRP, H.NAME, O.DONOR_ID FROM ORDERS O, MED_INST H, USER U WHERE O.OR_ID='" + or_Id + "' AND H.MI_ID=O.MI_ID AND O.DONOR_ID=U.PH_NO;";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        d_id = dr["DONOR_ID"].ToString();
                        Reciv_Or_Id.Text = or_Id;
                        Reciv_D_Name.Text = dr.GetString(0);
                        Reciv_B_grp.Text = dr.GetString(dr.GetOrdinal("B_GRP"));
                        Reciv_Hos.Text = dr.GetString(2);
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

        private void BloodRequest_Click(object sender, RoutedEventArgs e)
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
                cmd.Parameters.AddWithValue("@DONOR_ID", d_id);
                cmd.Parameters.AddWithValue("@MI_ID", hos_id);
                cmd.Parameters.AddWithValue("@REQ_DATE", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@QUANTITY", "1");
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

        private void BloodRecived_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            Database d = new Database();
            try
            {
                string[] a = Reciv_Date.Text.Split('-');
                string s = a[2] + "-" + a[1] + "-" + a[0] + " 00:00:00.0000000";
                string query = "UPDATE ORDERS SET DEL_DATE=@DEL_DATE WHERE OR_ID='" + or_Id + "';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                cmd.Parameters.AddWithValue("@DEL_DATE", s);
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
                MessageBox.Show("Order compeleted successfully");
            }
        }
    }
}
