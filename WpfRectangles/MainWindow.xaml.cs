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
using WpfRectangles.Processing;
using WpfRectangles.Logging;

namespace WpfRectangles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tb_Path.Text = "C:\\Temp\\RectLog.txt";
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            bool isFile = (rb_File.IsChecked == true) ? true : false;

            var log = new Log(isFile, tb_Console, tb_Path.Text);
            log.Info("Run proccess");

            DoWork work = new DoWork(log);

            log = null;
        }
    }
}
