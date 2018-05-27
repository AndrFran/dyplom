using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string strCmdText;
            strCmdText = "cd C:\\Users\\corov\\Desktop\\Dyplom\\pycparser-master";
            string pythonpath = "python";
            string commadn2 = pythonpath + " " + "examples\\c_json.py  " + input.Text.ToString();

            string Text = "/C "+strCmdText + "&"+ commadn2;
            Process process = System.Diagnostics.Process.Start("CMD.exe", Text);
            process.WaitForExit();
            Presenter preseter = Presenter.Instance;
            List<string> names = preseter.ParseFuncNames(System.IO.File.ReadAllText("C:\\Users\\corov\\Desktop\\Dyplom\\pycparser-master\\ssss.json"));
            listBox.Items.Clear();
            foreach (string str in names)
            {
                listBox.Items.Add(new CheckBox() { IsChecked = false, Content = str});
            }

        }
      

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {

                input.Text = openFileDialog.FileName;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {

                output.Text = openFileDialog.FileName;
            }
        }

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    }
