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
using OfficeOpenXml;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;
using System.Data;
using System.IO;
using CsvHelper.Configuration.Attributes;
using CsvHelper;
using System.Globalization;
using System.ComponentModel;

namespace DS_Warehouse_Management_System_v1._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
   //Set classes here so they are usable by all windows. No need to set these in all windows
    public class mainCsvData
    {
        [Name("Appointment Time")]
        public string AppointmentTime { get; set; }
        public string Customer { get; set; }
        [Name("Load ID")]
        public string LoadID { get; set; }
        public string Carrier { get; set; }
        public string Trailer { get; set; }
        [Name("Inbound/Outbound")]
        public string INOUT { get; set; }
        public string Notes { get; set; }
        [Name("Drop/Preload")]
        public string DropPreload { get; set; }

    }
    public class shiftData
    {
        [Name("Employee Number")]
        public string Empnumber { get; set; }
        [Name("First Name")]
        public string FirstName { get; set; }
        [Name("Last Name")]
        public string LastName { get; set; }

    }
    public class ScheduleEntry
    {
        [Name("Appointment Time")]
        public string AppointmentTime { get; set; }
        [Name("Arrival Time")]
        public string ArrivalTime { get; set; }
        public string Customer { get; set; }
        [Name("Load ID")]
        public string LoadID { get; set; }
        public string Carrier { get; set; }
        public string Trailer { get; set; }
        [Name("Inbound/Outbound")]
        public string INOUT { get; set; }
        [Name("Associate")]
        public string Associate { get; set; }
        public string Notes { get; set; }
        [Name("Drop/Preload")]
        public string DropPreload { get; set; }
        [Name("PLT IN/OUT")]
        public string PLTINOUT { get; set; }
        [Name("Door Assigned")]
        public string DoorAssigned { get; set; }
        [Name("Completed Time")]
        public string CompletedTime { get; set; }
    }
    public class CsvData : INotifyPropertyChanged
    {
        [Name("Appointment Time")]
        public string AppointmentTime { get; set; }
        [Name("Arrival Time")]
        public string ArrivalTime { get; set; }
        public string Customer { get; set; }
        [Name("Load ID")]
        public string LoadID { get; set; }
        public string Carrier { get; set; }
        [Name("Trailer Nubmer")]
        public string Trailer { get; set; }
        [Name("Inbound/Outbound")]
        public string INOUT { get; set; }
        public string Associate { get; set; }
        public string Notes { get; set; }
        [Name("Drop/Live/Preload")]
        public string DropPreload { get; set; }
        [Name("Pallets In/Pallets Out")]
        public string PLTINOUT { get; set; }
        [Name("Door Assigned")]
        public string DoorAssigned { get; set; }
        [Name("Completed Time")]
        public string CompletedTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RolloverSchedule
    {
        [Name("Appointment Time")]
        public string AppointmentTime { get; set; }
        [Name("Arrival Time")]
        public string ArrivalTime { get; set; }
        public string Customer { get; set; }
        [Name("Load ID")]
        public string LoadID { get; set; }
        public string Carrier { get; set; }
        public string Trailer { get; set; }
        [Name("Inbound/Outbound")]
        public string INOUT { get; set; }
        [Name("Associate")]
        public string Associate { get; set; }
        public string Notes { get; set; }
        [Name("Drop/Preload")]
        public string DropPreload { get; set; }
        [Name("PLT IN/OUT")]
        public string PLTINOUT { get; set; }
        [Name("Door Assigned")]
        public string DoorAssigned { get; set; }
        [Name("Completed Time")]
        public string CompletedTime { get; set; }

    }
    public class Daily
    {
        [Name("Appointment Time")]
        public string AppointmentTime { get; set; }
        [Name("Arrival Time")]
        public string ArrivalTime { get; set; }
        public string Customer { get; set; }
        [Name("Load ID")]
        public string LoadID { get; set; }
        public string Carrier { get; set; }
        public string Trailer { get; set; }
        [Name("Inbound/Outbound")]
        public string INOUT { get; set; }
        [Name("Associate")]
        public string Associate { get; set; }
        public string Notes { get; set; }
        [Name("Drop/Preload")]
        public string DropPreload { get; set; }
        [Name("PLT IN/OUT")]
        public string PLTINOUT { get; set; }
        [Name("Door Assigned")]
        public string DoorAssigned { get; set; }
        [Name("Completed Time")]
        public string CompletedTime { get; set; }
    }
    public class YardInfo
    {
        [Name("Appointment Time")]
        public string AppointmentTime { get; set; }
        [Name("Arrival Time")]
        public string ArrivalTime { get; set; }
        public string Customer { get; set; }
        [Name("Load ID")]
        public string LoadID { get; set; }
        public string Carrier { get; set; }
        public string Trailer { get; set; }
        [Name("Full/Empty")]
        public string FULLEMPTY { get; set; }
        [Name("Associate")]
        public string Associate { get; set; }
        public string Notes { get; set; }
        [Name("Drop/Preload")]
        public string DropPreload { get; set; }
        [Name("PLT IN/OUT")]
        public string PLTINOUT { get; set; }
        [Name("Spot Assigned")]
        public string SpotAssigned { get; set; }
        [Name("Completed Time")]
        public string CompletedTime { get; set; }
    }
    
    

    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            AdminView();
            GetSchedule();
            GetShiftData();
            LoadScheduleDataGrid();
            LoadShiftData();
        }
        
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            LogOutSession_Click(sender, e);
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Minimize();
        }

        //Sets the Admin View Conditions
        private void AdminView()
        {
            

            if (UserSession.IsAdmin)
            {
                
                InvManLabel.Margin = new Thickness(590, InvManLabel.Margin.Top, InvManLabel.Margin.Right, InvManLabel.Margin.Bottom);
                btnInvMan.Margin = new Thickness(683, btnInvMan.Margin.Top, btnInvMan.Margin.Right, btnInvMan.Margin.Bottom);
                EmpManLabel.Visibility = Visibility.Visible;
                btnEmpMan.Visibility = Visibility.Visible;

            }
            else
            {
                return;
            }
        }
        public MenuItem FindMenuItemByHeader(ItemsControl itemsControl, string header)
        {
            foreach (var item in itemsControl.Items)
            {
                if (item is MenuItem menuItem && menuItem.Header.ToString() == header)
                {
                    return menuItem;
                }
                if (item is ItemsControl newstedItemsControl)
                {
                    var foundMenuItem = FindMenuItemByHeader(newstedItemsControl, header);

                    if (foundMenuItem != null)
                    {
                        return foundMenuItem;
                    }
                }
            }
            return null;
        }
        private void Email()
        {
            System.Diagnostics.Process.Start("mailto:");
        }
        private void GetShiftData()
        {
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            string connectionString = "Server=208.109.49.241;Database=appusers;Uid=uploader72;Pwd=3825Harley!!;";

            string csvfilePath = "C:\\Users\\Public\\Documents\\employees.csv";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                
                connection.Open();

                string query;
                if (currentHour >= 6 && currentHour < 14)
                {
                    query = "SELECT * FROM employees WHERE Shift = 1";
                }
                else if (currentHour >= 14 && currentHour < 22)
                {
                    query = "SELECT * FROM employees WHERE Shift = 2";
                }
                else 
                {
                    query = "SELECT * FROM employees WHERE Shift = 3";
                }
                

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    DataTable shiftTable = new DataTable();

                    using (MySqlDataAdapter shiftadapter = new MySqlDataAdapter(command))
                    {
                        shiftadapter.Fill(shiftTable);
                    }
                    ExportshiftToCsv(shiftTable, csvfilePath);
                }
            }

        }
        private void LoadShiftData()
        {
            currentshiftDataGrid.Items.Clear();
            string sfilePath = "C:\\Users\\Public\\Documents\\employees.csv";

            if (File.Exists(sfilePath))
            {
                

                List<shiftData> dataItems = new List<shiftData>();
                

                using (StreamReader reader = new StreamReader(sfilePath))
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
                        shiftData item = new shiftData
                        {

                            Empnumber = row[0],
                            FirstName = row[1],
                            LastName = row[2],

                        };
                        dataItems.Add(item);

                    }
                }

                currentshiftDataGrid.ItemsSource = dataItems;

            }
            else
            {
                MessageBox.Show("Todays Schedule Not Found");
            }
        }
        private void GetSchedule()
        {
            string connectionString = "Server=208.109.49.241; Database = appschedule; Uid = uploader72; Pwd = 3825Harley!!; ";
            string inboundQuery = "SELECT * FROM ScheduleInbounds";
            string outboundQuery = "SELECT * FROM ScheduleOutbounds";
            string outputPath = "C:\\Users\\Public\\Documents\\Schedule.csv";
            DataTable mergeDataTable = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(inboundQuery, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(mergeDataTable);
                    }
                }
                using (MySqlCommand command = new MySqlCommand(outboundQuery, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(mergeDataTable);
                    }
                }
            }
            ExportDataTableToCsv(mergeDataTable, outputPath);
        }
        public void ExportshiftToCsv(DataTable dataTable, string filePath)
        {
            var schData = new StringBuilder();
            schData.AppendLine(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName)));

            foreach (DataRow row in dataTable.Rows)
            {
                schData.AppendLine(string.Join(",", row.ItemArray));
            }
            File.WriteAllText(filePath, schData.ToString());
        }
        public void ExportDataTableToCsv(DataTable dataTable, string filePath)
        {
            var csvData = new StringBuilder();
            csvData.AppendLine(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName)));

            foreach (DataRow row in dataTable.Rows)
            {
                csvData.AppendLine(string.Join(",", row.ItemArray));
            }
            File.WriteAllText(filePath, csvData.ToString());
        }
        private void LoadScheduleDataGrid()
        {
            string csvFilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";

            if (File.Exists(csvFilePath))
            {
                List<mainCsvData> dataItems = new List<mainCsvData>();


                using (StreamReader reader = new StreamReader(csvFilePath))
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
                        mainCsvData item = new mainCsvData
                        {

                            AppointmentTime = row[0],
                            Customer = row[2],
                            LoadID = row[3],
                            Carrier = row[4],
                            Trailer = row[5],
                            INOUT = row[6],
                            Notes = row[8],
                            DropPreload = row[9]
                        };
                        dataItems.Add(item);


                    }
                }

                scheduleDataGrid.ItemsSource = dataItems;

            }
            else
            {
                MessageBox.Show("Todays Schedule Not Found");
            }
        }
        
        

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Email();
        }

        private void btnControlRoom_Click(object sender, RoutedEventArgs e)
        {
            ControlRoom controlRoom = new ControlRoom();
            controlRoom.Show();
            EmployeePad2 employeePad2 = new EmployeePad2();
            employeePad2.Show();
            DockScreen dockScreen = new DockScreen();
            dockScreen.Show();
            
            this.Close();
        }

        private void btnEmpMan_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManagement employeeManagement = new EmployeeManagement();
            employeeManagement.Show();

        }
    }
}
