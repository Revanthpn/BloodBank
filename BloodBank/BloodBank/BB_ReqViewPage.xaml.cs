using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for BB_ReqViewPage.xaml
    /// </summary>
    public partial class BB_ReqViewPage : Page
    {
        string id;
        public BB_ReqViewPage(string id)
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
                string query = "SELECT O.OR_ID, M.NAME, O.B_GRP FROM MED_INST M, ORDERS O WHERE DEL_DATE IS NULL AND M.MI_ID=O.RECIP_ID AND DONOR_ID='" + id + "';";
                d.openConnection();
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ReqList.Items.Add(dr["OR_ID"].ToString() + " | " + dr.GetString(dr.GetOrdinal("NAME")) + " | " + dr.GetString(dr.GetOrdinal("B_GRP")));
                    }
                }
                else
                {
                    ReqList.Items.Add("No requests");
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

        private void ReqList_DropDownClosed(object sender, EventArgs e)
        {
            Database d = new Database();
            try
            {
                string[] a = ReqList.Text.Split('|');
                string or_ID = a[0].Trim();
                //string hos_Name = a[1].Trim();
                //string b_Grp = a[2].Trim();
                d.openConnection();
                string query = "SELECT NAME, PHONE, WEBSITE, B_GRP, LOCATION, CITY, QUANTITY FROM MED_INST M, ORDERS O WHERE O.OR_ID='" + or_ID + "' AND O.RECIP_ID=M.MI_ID;";
                SQLiteCommand cmd = new SQLiteCommand(query, d.con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        HosName.Content = dr.GetString(dr.GetOrdinal("NAME"));
                        HosPh.Content = dr["PHONE"].ToString();
                        HosWebsite.Content = dr.GetString(dr.GetOrdinal("WEBSITE"));
                        HosB_Grp.Content = dr.GetString(dr.GetOrdinal("B_GRP"));
                        HosLoc.Content = dr.GetString(dr.GetOrdinal("LOCATION"));
                        HosCity.Content = dr.GetString(dr.GetOrdinal("CITY"));
                        Quantity.Content = dr["QUANTITY"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("No option selected");
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
    }
}
