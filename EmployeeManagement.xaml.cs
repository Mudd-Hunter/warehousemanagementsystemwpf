using CsvHelper.Configuration.Attributes;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for EmployeeManagement.xaml
    /// </summary>
    /// 
    public class CurrentEmployees
    {
        [Name("Employee Number")]
        public string Empnumber { get; set; }
        [Name("First Name")]
        public string FirstName { get; set; }
        [Name("Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Name("Zip Code")]
        public string ZipCode { get; set; }
        [Name("Phone Number")]
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Shift { get; set; }



    }
    public partial class EmployeeManagement : Window
    {
        public static EmployeeManagement Instance;
        public EmployeeManagement()
        {
            InitializeComponent();
            Instance = this;

            GetEmployees();
            LoadEmployees();
        }
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void GetEmployees()
        {
            string connectionString = "Server=208.109.49.241;Database=appusers;Uid=uploader72;Pwd=3825Harley!!;";
            string query = "SELECT * FROM employees";
            string csvfilePath = "C:\\Users\\Public\\Documents\\CurrentEmployees.csv";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            using (StreamWriter writer = new StreamWriter(csvfilePath))
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    writer.Write($"{reader.GetName(i)}{(i < reader.FieldCount - 1 ? "," : Environment.NewLine)}");


                                }
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        writer.Write($"{reader[i]}{(i < reader.FieldCount - 1 ? "," : Environment.NewLine)}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while downloading the current table: {ex.Message}");
            }
        }
        private void LoadEmployees()
        {
            string efilePath = "C:\\Users\\Public\\Documents\\CurrentEmployees.csv";

            if (File.Exists(efilePath))
            {
                List<CurrentEmployees> dataItems = new List<CurrentEmployees>();


                using (StreamReader reader = new StreamReader(efilePath))
                {
                    string line;
                    bool isFirstLine = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }
                        string[] row = line.Split(',');
                        CurrentEmployees item = new CurrentEmployees
                        {

                            Empnumber = row[0],
                            FirstName = row[1],
                            LastName = row[2],
                            Email = row[3],
                            Address = row[4],
                            City = row[5],
                            State = row[6],
                            ZipCode = row[7],
                            Phone = row[8],
                            Username = row[9],
                            Shift = row[10]
                            
                        };
                        dataItems.Add(item);

                    }
                }

                currentemployeesDataGrid.ItemsSource = dataItems;

            }
            else
            {
                MessageBox.Show("Todays Schedule Not Found");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
