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
            var grid = new Grid();
            

            Rectangle rect = new Rectangle { Width = 70, Height = 30, Fill= Brushes.Azure };
            grid.Children.Add(rect);
            grid.Children.Add(new TextBlock { Text = "Lorem Ipsum" });
            DrawingField.Children.Add(grid);
            Canvas.SetLeft(grid, 130);
            Canvas.SetTop(grid, 170);
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = 165;
            myLine.X2 = 165;
            myLine.Y1 = 30+170;
            myLine.Y2 = 50+170;
            myLine.StrokeThickness = 2;
            DrawingField.Children.Add(myLine);
            Line myLine1 = new Line();
            myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine1.X1 = 165;
            myLine1.X2 = 155;
            myLine1.Y1 = 50+170;
            myLine1.Y2 = 45+170;
            myLine1.StrokeThickness = 2;
            DrawingField.Children.Add(myLine1);
            Line myLine2 = new Line();
            myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine2.X1 = 165;
            myLine2.X2 = 175;
            myLine2.Y1 = 50+170;
            myLine2.Y2 = 45+170;
            myLine2.StrokeThickness = 2;
            DrawingField.Children.Add(myLine2);
            Rectangle rect2 = new Rectangle { Width = 70, Height = 30, Fill = Brushes.Azure };
            DrawingField.Children.Add(rect2);
            Canvas.SetLeft(rect2, 130);
            Canvas.SetTop(rect2, 50+170);
            myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = 165;
            myLine.X2 = 165;
            myLine.Y1 = 50 + 170+30;
            myLine.Y2 = 50 + 170+50;
            myLine.StrokeThickness = 2;
            DrawingField.Children.Add(myLine);
            myLine1 = new Line();
            myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine1.X1 = 165;
            myLine1.X2 = 155;
            myLine1.Y1 = 50 + 170+50;
            myLine1.Y2 = 45 + 170+50;
            myLine1.StrokeThickness = 2;
            DrawingField.Children.Add(myLine1);
            myLine2 = new Line();
            myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine2.X1 = 165;
            myLine2.X2 = 175;
            myLine2.Y1 = 50 + 170+50;
            myLine2.Y2 = 45 + 170+50;
            myLine2.StrokeThickness = 2;
            DrawingField.Children.Add(myLine2);
            rect2 = new Rectangle { Width = 70, Height = 30, Fill = Brushes.Azure };
            DrawingField.Children.Add(rect2);
            Canvas.SetLeft(rect2, 130);
            Canvas.SetTop(rect2, 50 + 170 + 50);

        }
        private void OpenJson(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                
                string json =System.IO.File.ReadAllText(openFileDialog.FileName);
                Presenter presenter = new Presenter();
                presenter.BuildFlowControlGraph(json);
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
