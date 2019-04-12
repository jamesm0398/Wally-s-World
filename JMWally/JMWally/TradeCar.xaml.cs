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

namespace JMWally
{
    /// <summary>
    /// Interaction logic for TradeCar.xaml
    /// </summary>
    public partial class TradeCar : Page
    {
        public TradeCar(MySqlConnection conn)
        {
            InitializeComponent();
        }


    }


}
