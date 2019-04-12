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
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;

//Class: DisplayInventory()
//This class displays the current inventory of vehicles

namespace JMWally
{
    /// <summary>
    /// Interaction logic for DisplayInventory.xaml
    /// </summary>
    public partial class DisplayInventory : Page
    {
        public DisplayInventory()
        {
            InitializeComponent();
        }

        MySqlConnection conn = null;
        MySqlDataReader rdr = null;

        string cs = @"server=localhost;userid=root;password=Ts0dl0ttl;database=JmWally";

        public void displayInt()
        {


            try
            {
                
                conn = new MySqlConnection(cs);
                conn.Open();
                MessageBox.Show("Connected");
                string seeCars = @"SELECT * FROM vehicle";
               


                string year = "";
                string make = "";
                string model = "";
                string colour = "";
                string kms = "";
                string vin = "";
                string inStock = "";

                MySqlCommand cmd = new MySqlCommand(seeCars, conn);
                rdr = cmd.ExecuteReader();

                

                while (rdr.Read())
                {
                    year = rdr.GetString(1);
                    inventory.Text += "\n" + year;
                    make = rdr.GetString(2);
                    inventory.Text += " " + make;
                    model = rdr.GetString(3);
                    inventory.Text += " " + model;
                    colour = rdr.GetString(4);
                    inventory.Text += ", " + colour;
                    kms = rdr.GetString(5);
                    inventory.Text += " - " + kms + "km";
                    vin = rdr.GetString(0);
                    inventory.Text += " VIN: " + vin;
                    inStock = rdr.GetString(7);
                    inventory.Text += " In stock: " + inStock + "\n";
                }

                rdr.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
    }
}
