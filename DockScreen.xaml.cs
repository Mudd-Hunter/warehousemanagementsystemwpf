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
    /// Interaction logic for DockScreen.xaml
    /// </summary>
    public partial class DockScreen : Window
    {
        public static DockScreen Instance;
        public DockScreen()
        {
            InitializeComponent();
            Instance = this;

            Adminload();
            scheduleCsvData();
        }
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void menuControlRoom_Click(object sender, RoutedEventArgs e)
        {
            ControlRoom.Instance.Activate();
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
    }
}
