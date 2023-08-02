using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using DS_Warehouse_Management_System_v1._2.Notes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using Microsoft.VisualBasic.FileIO;
using MySqlConnector;
using System.Data;

namespace DS_Warehouse_Management_System_v1._2
{
    /// <summary>
    /// Interaction logic for ControlRoom.xaml
    /// </summary>
    /// 
    
    public partial class ControlRoom : Window
    {
        public static ControlRoom Instance;
        public ControlRoom()
        {
            InitializeComponent();
            Instance = this;

            GetShiftData();
            DailyDownload();
            YardInventory();
            scheduleCsvData();
            CheckRemaining();
            Adminload();
            loadshiftData();
        }
        //Recurring Window Methods
        
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void menuDockScreen_Click(object sender, RoutedEventArgs e)
        {
            DockScreen.Instance.Activate();
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
        public void Adminload()
        {
            var adminMenuItem = FindMenuItemByHeader(Topmenu, "_Admin");

            if (UserSession.IsAdmin)
            {
                adminMenuItem.Visibility = Visibility.Visible;

            }
            else
            {
                return;
            }
        }

        //OnLoad Csv Methods
        private void GetShiftData()
        {
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            string connectionString = "Server=208.109.49.241;Database=appusers;Uid=uploader72;Pwd=3825Harley!!;";

            string csvfilePath = "C:\\Users\\Public\\Documents\\shiftdata.csv";

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
        public void loadshiftData()
        {
            shiftDataGrid.Items.Clear();

            string efilePath = "C:\\Users\\Public\\Documents\\shiftdata.csv";

            if (File.Exists(efilePath))
            {
                List<shiftData> dataItems = new List<shiftData>();

                using (StreamReader ereader = new StreamReader(efilePath))
                {
                    string eline;
                    bool isFirstLine = true;
                    while ((eline = ereader.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }
                        string[] erow = eline.Split(',');
                        shiftData item = new shiftData
                        {
                            Empnumber = erow[0],
                            FirstName = erow[1],
                            LastName = erow[2],
                        };
                        dataItems.Add(item);
                    }
                }

                shiftDataGrid.ItemsSource = dataItems;
            }
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
        public void scheduleCsvData()
        {
            string sfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";

            if (File.Exists(sfilePath))
            {
                List<CsvData> dataItems = new List<CsvData>();
                

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
                        CsvData item = new CsvData
                        {

                            AppointmentTime = row[0],
                            ArrivalTime = row[1],
                            Customer = row[2],
                            LoadID = row[3],
                            Carrier = row[4],
                            Trailer = row[5],
                            INOUT = row[6],
                            Associate = row[7],
                            Notes = row[8],
                            DropPreload = row[9],
                            PLTINOUT = row[10],
                            DoorAssigned = row[11],
                            CompletedTime = row[12]
                        };
                        dataItems.Add(item);

                    }
                }
                
                todaysscheduleDataGrid.ItemsSource = dataItems;

            }
            else
            {
                MessageBox.Show("Todays Schedule Not Found");
            }
        }
        public void DailyDownload()
        {
            var dfilePath = "C:\\Users\\Public\\Documents\\DailyDownload.csv";

            using (var dwriter = new StreamWriter(dfilePath, true))
            using (var dcsv = new CsvWriter(dwriter, CultureInfo.InvariantCulture))
            {
                var dfileIsEmpty = new FileInfo(dfilePath).Length == 0;

                if (dfileIsEmpty)
                {
                    dcsv.WriteHeader<Daily>();
                    dcsv.NextRecord();
                }
            }
        }
        public void YardInventory()
        {
            var yfilePath = "C:\\Users\\Public\\Documents\\YardInventory.csv";

            using (var ywriter = new StreamWriter(yfilePath, true))
            using (var ycsv = new CsvWriter(ywriter, CultureInfo.InvariantCulture))
            {
                var yfileIsEmpty = new FileInfo(yfilePath).Length == 0;

                if (yfileIsEmpty)
                {
                    ycsv.WriteHeader<YardInfo>();
                    ycsv.NextRecord();
                }
            }
        }
        public void CheckRemaining()
        {
            var rfilePath = "C:\\Users\\Public\\Documents\\Rolloverdoors.csv";

            using (var rwriter = new StreamWriter(rfilePath, true))
            using (var rcsvWriter = new CsvWriter(rwriter, CultureInfo.InvariantCulture))
            {
                var rfileIsEmpty = new FileInfo(rfilePath).Length == 0;

                if (rfileIsEmpty)
                {
                    rcsvWriter.WriteHeader<RolloverSchedule>();
                    rcsvWriter.NextRecord();
                }
            }
        }

        private void btnDoor1_Click(object sender, RoutedEventArgs e)
        {
            CsvData selectedItem = todaysscheduleDataGrid.SelectedItem as CsvData;
            if (selectedItem != null)
            {
                EnterLoadInfo enterLoadInfo = new EnterLoadInfo(selectedItem);
                enterLoadInfo.Show();
                string buttonText = btnDoor1.Content.ToString();
                string desiredNumber1 = buttonText.Substring(buttonText.Length - 1);
                EnterLoadInfo.Instance.doorTextBox.Text = desiredNumber1;
                
            }
        }

        private void btnLiveDoor1_Click(object sender, RoutedEventArgs e)
        {
            Live.Inbound.Door1 lidoor1 = Application.Current.Windows.OfType<Live.Inbound.Door1>().FirstOrDefault();

            if (lidoor1 != null && NotesDoor1.Instance != null)
            {
                if (lidoor1.WindowState == WindowState.Minimized)
                {
                    lidoor1.WindowState = WindowState.Normal;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;   
                }
                else if (!lidoor1.IsActive && !NotesDoor1.Instance.IsActive)
                {
                    lidoor1.Activate();
                    NotesDoor1.Instance.Activate();
                }
            }
            if (lidoor1 != null && NotesDoor1.Instance == null)
            {
                if (lidoor1.WindowState == WindowState.Minimized)
                {
                    lidoor1.WindowState = WindowState.Normal;
                }
                else if (!lidoor1.IsActive)
                {
                    lidoor1.Activate();
                }
            }
        }

        private void btnLiveRushDoor1_Click(object sender, RoutedEventArgs e)
        {
            Live.Inbound.Door1 lidoor1 = Application.Current.Windows.OfType<Live.Inbound.Door1>().FirstOrDefault();

            if (lidoor1 != null && NotesDoor1.Instance != null)
            {
                if (lidoor1.WindowState == WindowState.Minimized)
                {
                    lidoor1.WindowState = WindowState.Normal;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                }
                else if (!lidoor1.IsActive && !NotesDoor1.Instance.IsActive)
                {
                    lidoor1.Activate();
                    NotesDoor1.Instance.Activate();
                }
            }
            if (lidoor1 != null && NotesDoor1.Instance == null)
            {
                if (lidoor1.WindowState == WindowState.Minimized)
                {
                    lidoor1.WindowState = WindowState.Normal;
                }
                else if (!lidoor1.IsActive)
                {
                    lidoor1.Activate();
                }
            }
        }

        private void btnDropDoor1_Click(object sender, RoutedEventArgs e)
        {
            DropDoor.Inbound.Door1 dropDoor1 = Application.Current.Windows.OfType<DropDoor.Inbound.Door1>().FirstOrDefault();
            if (dropDoor1 != null && NotesDoor1.Instance != null)
            {
                if (dropDoor1.WindowState == WindowState.Minimized)
                {
                    dropDoor1.WindowState = WindowState.Normal;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                }
                else if (!dropDoor1.IsActive && !NotesDoor1.Instance.IsActive)
                {
                    dropDoor1.Activate();
                    NotesDoor1.Instance.Activate();
                }
            }
            if (dropDoor1 != null && NotesDoor1.Instance == null)
            {
                if (dropDoor1.WindowState == WindowState.Minimized)
                {
                    dropDoor1.WindowState = WindowState.Normal;
                }
                else if (!dropDoor1.IsActive)
                {
                    dropDoor1.Activate();
                }

            }
        }

        private void btnLiveOutDoor1_Click(object sender, RoutedEventArgs e)
        {
            Live.Outbound.Door1 lodoor1 = Application.Current.Windows.OfType<Live.Outbound.Door1>().FirstOrDefault();

            if (lodoor1 != null && NotesDoor1.Instance != null)
            {
                if (lodoor1.WindowState == WindowState.Minimized)
                {
                    lodoor1.WindowState = WindowState.Normal;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                }
                else if (!lodoor1.IsActive && !NotesDoor1.Instance.IsActive)
                {
                    lodoor1.Activate();
                    NotesDoor1.Instance.Activate();
                }
            }
            if (lodoor1 != null && NotesDoor1.Instance == null)
            {
                if (lodoor1.WindowState == WindowState.Minimized)
                {
                    lodoor1.WindowState = WindowState.Normal;
                }
                else if (!lodoor1.IsActive)
                {
                    lodoor1.Activate();
                }
            }
        }

        private void btnPreload1_Click(object sender, RoutedEventArgs e)
        {
            var pickupfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";
            var plines = File.ReadAllLines(pickupfilePath);

            DropDoor.Preload.Door1 pre1 = Application.Current.Windows.OfType<DropDoor.Preload.Door1>().FirstOrDefault();
            if (pre1 != null && NotesDoor1.Instance != null)
            {
                if (pre1.WindowState == WindowState.Minimized)
                {
                    pre1.WindowState = WindowState.Normal;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                }
                else if (!pre1.IsActive && !NotesDoor1.Instance.IsActive)
                {
                    pre1.Activate();
                    NotesDoor1.Instance.Activate();
                }

            }
            if (pre1 != null && NotesDoor1.Instance == null)
            {
                if (pre1.WindowState == WindowState.Minimized)
                {
                    pre1.WindowState = WindowState.Normal;
                }
                else if (!pre1.IsActive)
                {
                    pre1.Activate();
                }
            }
            if (pre1 == null)
            {
                DropDoor.Preload.Door1 pickup1 = new DropDoor.Preload.Door1();
                pickup1.Left = 10;
                pickup1.Top = 129;
                if (plines.Length <= 1)
                {
                    return;
                }
                for (int p = 1; p < plines.Length; p++)
                {
                    var pdataLine = plines[p];
                    var pcolumns = pdataLine.Split(',');

                    if (pcolumns.Length >= 13 && pcolumns[9] == "PRE")
                    {
                        if (pcolumns[11] == "1")
                        {
                            pickup1.customerTextBox.Text = pcolumns[2];
                            pickup1.loadidTextBox.Text = pcolumns[3];
                            pickup1.carrierTextBox.Text = pcolumns[4];
                            pickup1.trailernumberTextBox.Text = pcolumns[5];
                            pickup1.palletsinTextBox.Text = pcolumns[10];

                        }

                    }
                    pickup1.Show();

                }
            }
        }

        
    }
}
