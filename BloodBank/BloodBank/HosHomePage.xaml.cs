using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for HosHomePage.xaml
    /// </summary>
    public partial class HosHomePage : Page
    {
        string id, _name, typ;
        public HosHomePage(string id, string _name, string typ)
        {
            InitializeComponent();
            this.id = id;
            this._name = _name;
            this.typ = typ;
            name.Content = _name;
            if (typ.Equals("66"))
                type.Content = "Blood bank";
            else if (typ.Equals("72"))
                type.Content = "Hospital";
            else
                type.Content = "Invalid";
            Database d = new Database();
            d.openConnection();
            try
            {
                if (typ.Equals("66"))
                {
                    BB.Visibility = Visibility.Visible;
                    string query = "SELECT NAME, PHONE, LOCATION, CITY, WEBSITE, EMAIL FROM MED_INST WHERE MI_ID IN (SELECT RECIP_ID FROM ORDERS WHERE DONOR_ID='" + id + "');";
                    SQLiteDataAdapter da = new SQLiteDataAdapter(query, d.con);
                    DataTable dt = new DataTable("MED_INST");
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        BB_NoData.Visibility = Visibility.Hidden;
                        BB_grid.Visibility = Visibility.Visible;
                        BB_Hoslist.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        BB_grid.Visibility = Visibility.Hidden;
                        BB_NoData.Visibility = Visibility.Visible;
                    }
                }
                else if (typ.Equals("72"))
                {
                    H.Visibility = Visibility.Visible;
                    string query = "SELECT NAME, PHONE, LOCATION, CITY, WEBSITE, EMAIL FROM MED_INST WHERE TYPE_OF_MI='66'";
                    SQLiteDataAdapter da = new SQLiteDataAdapter(query, d.con);
                    DataTable dt = new DataTable("USER");
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        H_BB_NoData.Visibility = Visibility.Hidden;
                        H_BB_grid.Visibility = Visibility.Visible;
                        H_BBlist.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        H_BB_grid.Visibility = Visibility.Hidden;
                        H_BB_NoData.Visibility = Visibility.Visible;
                    }
                    query = "SELECT NAME, PH_NO AS PHONE_NUMBER, B_GRP AS BLOOD_GROUP, LOCATION, CITY, EMAIL FROM USER WHERE MI_ID='" + id + "';";
                    da = new SQLiteDataAdapter(query, d.con);
                    dt = new DataTable("USER");
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        H_P_NoData.Visibility = Visibility.Hidden;
                        H_P_grid.Visibility = Visibility.Visible;
                        Plist.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        H_P_grid.Visibility = Visibility.Hidden;
                        H_P_NoData.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    //list.Visibility = Visibility.Hidden;
                    //label.Content = "ERROR";
                    //noData.Visibility = Visibility.Visible;
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Exception in HosHomePage\n"+excep.Message);
            }
            finally
            {
                d.closeConnection();
            }
        }
    }
}
