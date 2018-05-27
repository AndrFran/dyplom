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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var person = new Person
            {
                PrintLastName = false,
                FirstName = "Dima",
                LastName = "Pupkin",
                Age = 40,
                ChildNames = new List<Name>
                                                  {
                                                      new Name
                                                          {
                                                              Value = "Jerry"
                                                          }
                                                  },
                ParentNames = new List<Name>
                                                  {
                                                      new Name
                                                          {
                                                              Value = "Sara"
                                                          },
                                                       new Name
                                                          {
                                                              Value = "Tom"
                                                          },
                                                  }

            };
            var html = Render.StringToString(Properties.Resources.template, person);
            Console.WriteLine(html);
            Console.ReadLine();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            

        }
        private void OpenJson(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                
                string json =System.IO.File.ReadAllText(openFileDialog.FileName);
                Presenter presenter = new Presenter();
                int y = 0 ;
                presenter.ParseFuncNames(json);
                List<UIElement> nodes = presenter.BuildFlowControlGraph(ref y);
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
