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

//Class: MainWindow
//This is the main entry point for the front end database program

namespace JMWally
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        

        //User wants to add a customer and order, display "add customer" page
        private void addCustomer_Click(object sender, RoutedEventArgs e)
        {
            

           
           AddCustomer addC = new AddCustomer();
            this.Content = addC;
            addC.AddC();

            

        }

        //User wants to retreive a past sales record
        private void sales_Click(object sender, RoutedEventArgs e)
        {
            Retreive rt = new Retreive();
            this.Content = rt;
        }

        //User wants to display the inventory
        private void displayInventory_Click(object sender, RoutedEventArgs e)
        {
            DisplayInventory dinv = new DisplayInventory();
            this.Content = dinv;
            dinv.displayInt();
        }
    }
}


