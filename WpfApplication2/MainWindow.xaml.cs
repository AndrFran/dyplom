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
using System.Collections.Generic;
using WpfApplication2.Properties;
using Nustache.Core;
using Microsoft.Win32;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int index;
        List<TestCase> cases;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(List<TestCase> cases)
        {
            InitializeComponent();
            this.cases = cases;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            

        }
        private void Next(object sender, RoutedEventArgs e)
        {
            if (index < cases.Count)
            {
                TestCase NewCase = cases[index++];
                var html = Render.StringToString(Properties.Resources.testcase, NewCase);
                richTextBox.Document.Blocks.Clear();
                richTextBox.AppendText(html);
                DrawingField.Children.Clear();
                Presenter presenter = Presenter.Instance;
                int y = 0;
                List<UIElement> nodes = presenter.BuildFlowControlGraph(ref y,NewCase.function_nodes,NewCase.path.ToList());
                DrawingField.Height = y;
                foreach (var elem in nodes)
                {
                    DrawingField.Children.Add(elem);
                }
                foreach (MyGrid elem in presenter.grids)
                {
                    DrawingField.Children.Add(elem);
                    Canvas.SetLeft(elem, elem.xpos);
                    Canvas.SetTop(elem, elem.ypos);
                }
            }
}


        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    class Name
    {
        public string Value { get; set; }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool PrintLastName { get; set; }
        public int Age { get; set; }
        public IEnumerable<Name> ChildNames { get; set; }
        public IEnumerable<Name> ParentNames { get; set; }
    }
}
