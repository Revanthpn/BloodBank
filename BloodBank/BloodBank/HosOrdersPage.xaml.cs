using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace BloodBank
{
    /// <summary>
    /// Interaction logic for HosOrdersPage.xaml
    /// </summary>
    public partial class HosOrdersPage : Page
    {
        string id;
        public HosOrdersPage(string id)
        {
            InitializeComponent();
            this.id = id;
            Database d = new Database();
            d.openConnection();
            try
            {
                string query = "SELECT * FROM ORDERS WHERE (RECIP_ID ='" + id + "' OR DONOR_ID ='" + id + "') AND DEL_DATE NOT NULL;";
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, d.con);
                DataTable dt = new DataTable("ORDERS");
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    NoData.Visibility = Visibility.Hidden;
                    Order_List.Visibility = Visibility.Visible;
                    Order_List.ItemsSource = dt.DefaultView;
                }
                else
                {
                    Order_List.Visibility = Visibility.Hidden;
                    NoData.Visibility = Visibility.Visible;
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
