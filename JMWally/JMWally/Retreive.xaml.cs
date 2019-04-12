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
using System.IO;

//Class: Retreive()
//This class retrieves a sales record given an order id. All of the sales are stored in a text file called Sales.txt, which this class parses
// to retreive the data.

namespace JMWally
{
    /// <summary>
    /// Interaction logic for Retreive.xaml
    /// </summary>
    public partial class Retreive : Page
    {
        public Retreive()
        {
            InitializeComponent();
        }

        private void searchOrder_Click(object sender, RoutedEventArgs e)
        {
            string[] separator = { "**********" };
            string orders = File.ReadAllText(@"Sales.txt");
            string[] orderList = orders.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            int orderNum = Int32.Parse(ordNum.Text);

            foreach(string ord in orderList)
            {
                if(ord.Contains("Order ID: " + orderNum))
                {
                    OFound.Opacity = 100;
                    displayOrder.Text = ord;
                }
            }


        }

       
    }
}
