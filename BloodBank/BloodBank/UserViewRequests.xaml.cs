using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for UserViewRequests.xaml
    /// </summary>
    public partial class UserViewRequests : Page
    {
        string id, bb_ID = null, bGrp;
        public UserViewRequests(string id, string bGrp)
        {
            InitializeComponent();
            this.id = id;
            this.bGrp = bGrp;
            loadValues();
        }
        private void loadValues()
        {
            Database d = new Database();
            try
            {
                string query = "SELECT NAME FROM USER WHERE PH_NO IN (SELECT DISTINCT RECIP_ID FROM ORDERS WHERE DONOR_ID='" + id + "');";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        RList.Items.Add(dr.GetString(dr.GetOrdinal("NAME")));
                    }
                }
                else
                {
                    RList.Items.Add("No recipients");
                }
                query = "SELECT NAME FROM MED_INST WHERE TYPE_OF_MI='66';";
                d.openConnection();
                cmd = new SQLiteCommand(query, d.con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        BBList.Items.Add(dr.GetString(dr.GetOrdinal("NAME")));
                    }
                }
                else
                {
                    BBList.Items.Add("No blood banks");
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
        private void onR_ComboBoxClosed(object sender, EventArgs e)
        {
            BB_details.Visibility = Visibility.Hidden;
            R_details.Visibility = Visibility.Visible;
            Database d = new Database();
            string query = "SELECT PH_NO, NAME, B_GRP, EMAIL, LOCATION, CITY FROM USER WHERE NAME='" + RList.Text + "';";
            try
            {
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        Ph_no.Text = dr["PH_NO"].ToString();
                        R_Name.Text = dr.GetString(dr.GetOrdinal("NAME"));
                        R_Bgrp.Text = dr.GetString(dr.GetOrdinal("B_GRP"));
                        R_Email.Text = dr.GetString(dr.GetOrdinal("EMAIL"));
                        R_Loc.Text = dr.GetString(dr.GetOrdinal("LOCATION"));
                        R_City.Text = dr.GetString(dr.GetOrdinal("CITY"));
                    }
                }
                //else
                //{
                //    MessageBox.Show("No option selected");
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

        private void onBB_ComboBoxClosed(object sender, EventArgs e)
        {
            R_details.Visibility = Visibility.Hidden;
            BB_details.Visibility = Visibility.Visible;
            Database d = new Database();
            string query = "SELECT MI_ID, NAME, PHONE, WEBSITE, EMAIL, LOCATION, CITY FROM MED_INST WHERE NAME='" + BBList.Text + "'";
            try
            {
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        bb_ID = dr["MI_ID"].ToString();
                        BB_Name.Text = dr["NAME"].ToString();
                        BB_Ph.Text = dr["PHONE"].ToString();
                        BB_Website.Text = dr.GetString(dr.GetOrdinal("WEBSITE"));
                        BB_Email.Text = dr.GetString(dr.GetOrdinal("EMAIL"));
                        BB_Loc.Text = dr.GetString(dr.GetOrdinal("LOCATION"));
                        BB_City.Text = dr.GetString(dr.GetOrdinal("CITY"));
                    }
                }
                //else
                //{
                //    MessageBox.Show("No option selected");
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

        private void DonateBB(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            Database d = new Database();
            string query = "INSERT INTO ORDERS(B_GRP,RECIP_ID,DONOR_ID,MI_ID,REQ_DATE,DEL_DATE,QUANTITY) VALUES(@B_GRP,@RECIP_ID,@DONOR_ID,@MI_ID,@REQ_DATE,@DEL_DATE,@QUANTITY)";
            try
            {
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                cmd.Parameters.AddWithValue("@B_GRP", bGrp);
                cmd.Parameters.AddWithValue("@RECIP_ID", bb_ID);
                cmd.Parameters.AddWithValue("@DONOR_ID", id);
                cmd.Parameters.AddWithValue("@MI_ID", bb_ID);
                cmd.Parameters.AddWithValue("@REQ_DATE", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@DEL_DATE", System.DateTime.Now);
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
            }
            if(flag)
            {
                MessageBox.Show("Donation recorded successfully");
            }
        }
    }
}
