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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
        }
    private void TestCases(object sender, RoutedEventArgs e)
    {
            MainWindow win2 = new MainWindow();
            win2.Show();
        }

    private void Settings(object sender, RoutedEventArgs e)
    {
            Settings win2 = new Settings();
            win2.Show();
        }
    private void File(object sender, RoutedEventArgs e)
    {

    }
    private void EditTemplate(object sender, RoutedEventArgs e)
    {
            TemplateSetting win2 = new TemplateSetting();
            win2.Show();
        }
    }
   
}
