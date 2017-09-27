using PrimaryInterface1._0.Core;
using PrimaryInterface1._0.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PrimaryInterface1._0
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel SourceData = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            ConnectView.DataSource = SourceData;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SourceData.DataCollection.Add(new DeviceModel("first", 4));          
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SourceData.DataCollection.RemoveAt(2);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SourceData.DataCollection.Add(new DeviceModel("second", 4));
            SourceData.DataCollection.Add(new DeviceModel("third", 4));
            SourceData.DataCollection.Add(new DeviceModel("fourth", 4));
        }
    }
}
