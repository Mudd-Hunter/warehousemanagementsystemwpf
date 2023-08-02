using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace DS_Warehouse_Management_System_v1._2
{
    /// <summary>
    /// Interaction logic for EmployeePad.xaml
    /// </summary>
    public partial class EmployeePad : Window
    {
        public static EmployeePad Instance;
        public EmployeePad()
        {
            InitializeComponent();
            Instance = this;

            PreviewKeyDown += Window_PreviewKeyDown;
        }
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LogOutSession_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window != login && window.IsVisible)
                {
                    window.Close();
                }
            }
        }
        private void Minimize()
        {
            WindowState = WindowState.Minimized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Minimize();

        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSubmit_Click(sender, e);


            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Escape)
            {
                btnClear_Click(sender, e);


            }
        }
        


        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string number = button.Content.ToString();
                employeenumberTextBox.Text += number;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (doornumberTextBox.Text == "1")
            {
                string employeeNumber = employeenumberTextBox.Text;
                string filePath = "C:\\Users\\Public\\Documents\\employees.csv";

                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] columns = line.Split(',');

                    if (columns.Length >= 3 && columns[0] == employeeNumber)
                    {
                        string fullName = $"{columns[1]} {columns[2]}";

                        if (Live.Inbound.Door1.Instance != null)
                        {
                            Live.Inbound.Door1.Instance.associateTextBox.Text = fullName;
                            Close();
                            EmployeePad2 employeePad2 = new EmployeePad2();
                            employeePad2.Show();
                            return;
                        }
                        if (Live.Outbound.Door1.Instance != null)
                        {
                            Live.Outbound.Door1.Instance.associateTextBox.Text = fullName;
                            Close();
                            EmployeePad2 employeePad2 = new EmployeePad2();
                            employeePad2.Show();
                            return;
                        }
                        if (DropDoor.Inbound.Door1.Instance != null)
                        {
                            DropDoor.Inbound.Door1.Instance.associateTextBox.Text = fullName;
                            Close();
                            EmployeePad2 employeePad2 = new EmployeePad2();
                            employeePad2.Show();
                            return;
                        }
                        if (DropDoor.Preload.Door1.Instance != null)
                        {
                            DropDoor.Preload.Door1.Instance.associateTextBox.Text = fullName;
                            Close();
                            EmployeePad2 employeePad2 = new EmployeePad2();
                            employeePad2.Show();
                            return;
                        }

                        //Live.Inbound.Door1.Instance.associateTextBox.Text = fullName;




                    }
                    
                    
                        
                    
                }

                MessageBox.Show("Employee Not Found!");



            }
            
            
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            employeenumberTextBox.Clear();
        }
    }
}
