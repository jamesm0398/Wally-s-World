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

//Class: AddCustomer
//This class adds a customer to the database, and also completes their order.

namespace JMWally
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Page
    {
        public AddCustomer()
        {
            InitializeComponent();
        }
        //Sql connection setup
        MySqlConnection conn = null;

        string cs = @"server=localhost;userid=root;password=Ts0dl0ttl;database=JmWally";

      
        //Connect to database 
        public void AddC()
        {
            try
            {

                conn = new MySqlConnection(cs);
                conn.Open();
               

                MySqlDataAdapter addOrders = new MySqlDataAdapter("SELECT `OrderID` FROM `orderLine`", conn);
                DataTable all = new DataTable();
                addOrders.Fill(all);
                int orderID = 0;
                string orderString = "";

                foreach(DataRow dr in all.Rows)
                {
                    foreach (DataColumn dc in all.Columns)
                    {
                        orderString = dr[dc].ToString();
                        orderID = Int32.Parse(orderString);
                        BuildSalesRecord(orderID, false);
                    }
                }
                

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            




        }

        //Used to store the new customer's details and the new car 
        int customerID = 0;
        String VIN = "";

        //Method: submit_click()
        //Summary: This function adds the new customer's details to the customer table
        private void submit_Click(object sender, RoutedEventArgs e)
        {
            String FirstName = fName.Text;
            String LastName = lName.Text;
            String phone = phoneNum.Text;
            
            
            string ID = "SELECT CustomerID FROM customer ORDER BY CustomerID DESC LIMIT 1";

            MySqlCommand getID = new MySqlCommand(ID,conn);
            MySqlDataReader rdr = getID.ExecuteReader();

            while(rdr.Read())
            {
                customerID = rdr.GetInt32(0);
                customerID = customerID + 1;
            }

            rdr.Close();


            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO customer (CustomerID, FirstName,LastName, PhoneNum) VALUES (@ID, @Fname, @Lname, @pNum)";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@ID", customerID);
            cmd.Parameters.AddWithValue("@Fname", FirstName);
            cmd.Parameters.AddWithValue("@Lname", LastName);
            cmd.Parameters.AddWithValue("@pNum", phone);

            cmd.ExecuteNonQuery();

            buyNew.Opacity = 100;
            trade.Opacity = 100;

            
            

        }
        //Method: buyNew_click()
        //Summary: Clicked when user is doing a new purchase, visibility buttons and textboxes on screen are changed
        private void buyNew_Click(object sender, RoutedEventArgs e)
        {
            enterNew.Opacity = 100;
            VINNew.Opacity = 100;
            buyNew.Opacity = 0;
            trade.Opacity = 0;
            placeOrder.Opacity = 100;
        }

        //Method: trade_click()
        //Summary: Clicked when the user is doing a trade. They are taken to the trade page.
        private void trade_Click(object sender, RoutedEventArgs e)
        {
            TradeCar trade = new TradeCar(conn);
            this.Content = trade;
        }

        //Method: placeOrder_click()
        //Summary: This method completes the customer's order using the VIN number provided
        private void placeOrder_Click(object sender, RoutedEventArgs e)
        {
            VIN = VINNew.Text;
            string currDate = DateTime.Now.ToString("dd/MM/yyyy");

            int subTotal = getSubTotal(VIN);
            double sPrice = subTotal * 1.4;

            string dealership = GetDealership(VIN);

            int orderID = 0;

            string OID = "SELECT `OrderID` FROM `order` ORDER BY OrderID DESC LIMIT 1";


            MySqlCommand getID = new MySqlCommand(OID, conn);
            MySqlDataReader rdr = getID.ExecuteReader();

            while (rdr.Read())
            {
                orderID = rdr.GetInt32(0);
                orderID = orderID + 1;
            }

            rdr.Close();

            try
            {
                MySqlCommand addOrder = new MySqlCommand();
                addOrder.Connection = conn;
                addOrder.CommandText = "INSERT INTO `order` (`OrderID`, `Date`, `CustomerID`, `subTotal`, `sPrice`, `Status`) VALUES (@id, @date, @cid, @stotal, @sprice, @status)";
                addOrder.Prepare();

                addOrder.Parameters.AddWithValue("@id", orderID);
                addOrder.Parameters.AddWithValue("@date", currDate);
                addOrder.Parameters.AddWithValue("@cid", customerID);
                addOrder.Parameters.AddWithValue("@stotal", subTotal);
                addOrder.Parameters.AddWithValue("@sPrice", sPrice);
                addOrder.Parameters.AddWithValue("@Status", "PAID");

                addOrder.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MySqlCommand addOL = new MySqlCommand();
            addOL.Connection = conn;
            addOL.CommandText = "INSERT INTO `orderLine` (`OrderID`, `VIN`) VALUES (@oid, @vin)";
            addOL.Prepare();

            addOL.Parameters.AddWithValue("@oid", orderID);
            addOL.Parameters.AddWithValue("@vin", VIN);

            addOL.ExecuteNonQuery();

            MySqlCommand changeStatus = new MySqlCommand();
            MySqlTransaction tr = null;
            tr = conn.BeginTransaction();
            changeStatus.Connection = conn;
            changeStatus.Transaction = tr;
            changeStatus.CommandText = "UPDATE vehicle SET inStock = 'No' WHERE VIN = " + "'" + VIN + "'";
            changeStatus.ExecuteNonQuery();
            tr.Commit();


            enterNew.Opacity = 0;
            VINNew.Opacity = 0;
            placeOrder.Opacity = 0;
            orderInfo.Opacity = 100;


            BuildSalesRecord(orderID, true);


               
        }

        //Get subtotal cost of the vehicle
        private int getSubTotal(string vin)
        {
            string ST = "SELECT wPrice FROM vehicle WHERE VIN = " + "'" + vin + "'";
            int subTotal = 0;
            MySqlCommand getST = new MySqlCommand(ST, conn);
            MySqlDataReader rdr = getST.ExecuteReader();

            while (rdr.Read())
            {
                subTotal = rdr.GetInt32(0);
              
            }
            
            rdr.Close();

            return subTotal;
        }

        //Get the name of the dealership where a vehicle is using the vin 
        private string GetDealership(string vin)
        {
            string getDeal = "SELECT DealershipID FROM vehicle WHERE VIN = " + "'" + vin + "'";
            string dealership = "";
            int dShip = 0;
            MySqlCommand getDS = new MySqlCommand(getDeal, conn);
            MySqlDataReader rdr = getDS.ExecuteReader();

            while(rdr.Read())
            {
                dShip = rdr.GetInt32(0);
                switch(dShip)
                {
                    case 0:
                        dealership = "Unknown";
                        break;
                    case 1:
                        dealership = "Sports World";
                        break;
                    case 2:
                        dealership = "Guelph Auto Mall";
                        break;
                    case 3:
                        dealership = "Waterloo";
                        break;
                    default:
                        dealership = "Unknown";
                        break;

                }
            }
            rdr.Close();

            return dealership;
        }

        //Create a sales record using the order table
        private void BuildSalesRecord(int orderID, bool isNewOrder)
        {
            MySqlDataAdapter adapter1 = new MySqlDataAdapter("SELECT * FROM `order` WHERE `OrderID` = " + orderID, conn);

            DataTable data = new DataTable();
            adapter1.Fill(data);

            string date = "";
            string CustomerFname = "";
            string CustomerLname = "";
            string dealership = GetDealership(VIN);
            string status = "";
            int subTotal = 0;
            string subTotalString = "";
            double saleTotal = 0;
            string year = "";
            string make = "";
            string model = "";
            string colour = "";
            string kms = "";

            MySqlDataAdapter adapter3 = new MySqlDataAdapter("SELECT * FROM `orderLine` WHERE `OrderID` = " + orderID, conn);

            DataTable carData = new DataTable();
            adapter3.Fill(carData);
            DataRow carRow = carData.Rows[0];


            string thisVehicle = carRow["VIN"].ToString();


            CustomerFname = GetCustomerFName(customerID);
            CustomerLname = GetCustomerLName(customerID);

            DataRow row = data.Rows[0];

            date = row["Date"].ToString();
            subTotalString = row["sPrice"].ToString();
            subTotal = Int32.Parse(subTotalString);
            status = row["Status"].ToString();

            MySqlDataAdapter adapter2 = new MySqlDataAdapter("SELECT * FROM vehicle WHERE VIN = " + "'" + thisVehicle + "'", conn);
            DataTable carInfo = new DataTable();
            adapter2.Fill(carInfo);

            DataRow row2 = carInfo.Rows[0];

            year = row2["Yeaar"].ToString();
            make = row2["Make"].ToString();
            model = row2["Model"].ToString();
            colour = row2["Colour"].ToString();
            kms = row2["Kms"].ToString();

            saleTotal = subTotal + (subTotal * 0.13);

            StringBuilder salesRec = new StringBuilder();
            salesRec.AppendLine("**********");
            salesRec.AppendLine("Thank you for choosing Wally's World at ");
            salesRec.AppendLine(dealership + " for your quality used vehicle!");
            salesRec.AppendLine("\n");
            salesRec.AppendLine("Date: " + date);
            salesRec.AppendLine("Customer: " + CustomerFname + " " + CustomerLname);
            salesRec.AppendLine("Order ID: " + orderID + " - " + status);
            salesRec.AppendLine("\n");
            salesRec.AppendLine(year + " " + make + " " + model + "," + colour);
            salesRec.AppendLine("VIN: " + thisVehicle + " KMS: " + kms);
            salesRec.AppendLine("\n");
            salesRec.AppendLine("Trade in: $0.00");
            salesRec.AppendLine("\n");
            salesRec.AppendLine("Subtotal = $" + subTotal);
            salesRec.AppendLine("HST (13%) = $" + (subTotal * 0.13));
            salesRec.AppendLine("Sale total = $" + saleTotal);

            if (isNewOrder == true )
            {
                rec.Text = salesRec.ToString();
            }
              
            StreamWriter sales = new StreamWriter(@"Sales.txt", true);
            sales.Write(salesRec.ToString());
           
            sales.Close();
        }

        //Get the customer's first name
        private string GetCustomerFName(int custID)
        {
            string getName = "SELECT FirstName FROM customer WHERE CustomerID = " + custID;

            string fname = "";
            MySqlCommand getFN = new MySqlCommand(getName, conn);
            MySqlDataReader rdr = getFN.ExecuteReader();

            while (rdr.Read())
            {
                fname = rdr.GetString(0);
            }
            rdr.Close();
            return fname;

        }

        //Get the customer's last name
        private string GetCustomerLName(int custID)
        {
            string getName = "SELECT LastName FROM customer WHERE CustomerID = " + custID;

            string lname = "";
            MySqlCommand getLN = new MySqlCommand(getName, conn);
            MySqlDataReader rdr = getLN.ExecuteReader();

            while (rdr.Read())
            {
                lname = rdr.GetString(0);
            }
            rdr.Close();
            return lname;

        }

       
        //Go to the search record page (non functional)
        private void search_Click(object sender, RoutedEventArgs e)
        {
            Retreive rt = new Retreive();
            this.Content = rt;
        }
    }
}
