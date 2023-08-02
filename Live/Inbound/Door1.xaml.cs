using CsvHelper.Configuration.Attributes;
using DS_Warehouse_Management_System_v1._2.Notes;
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
using System.Windows.Threading;

namespace DS_Warehouse_Management_System_v1._2.Live.Inbound
{
    /// <summary>
    /// Interaction logic for Door1.xaml
    /// </summary>
    public partial class Door1 : Window
    {
        public static Door1 Instance;
        public readonly DispatcherTimer Timer;
        public DateTime endTime;
        [Name("Appointment Time")]
        public static string AppointmentTime;
        [Name("Arrival Time")]
        public static string ArrivalTime;
        public static string Customer;
        [Name("Load ID")]
        public static string LoadID;
        public static string Carrier;
        public static string Trailer;
        [Name("Inbound/Outbound")]
        public static string INOUT;
        [Name("Associate")]
        public static string Associate;
        public static string Notes;
        [Name("Drop/Preload")]
        public static string DropPreload;
        [Name("PLT IN/OUT")]
        public static string PLTINOUT;
        [Name("Door Assigned")]
        public static string DoorAssigned;
        [Name("Completed Time")]
        public static string CompletedTime;
        DateTime ctime = DateTime.Now;
        public Door1()
        {
            InitializeComponent();
            Instance = this;
            Adminload();

            customerTextBox.Text = EnterLoadInfo.Customer;
            loadidTextBox.Text = EnterLoadInfo.LoadID;
            carrierTextBox.Text = EnterLoadInfo.Carrier;
            trailernumberTextBox.Text = EnterLoadInfo.Trailer;
            doorTextBox.Text = EnterLoadInfo.DoorAssigned;
            timeinTextBox.Text = EnterLoadInfo.ArrivalTime;

            DateTime currentTime = DateTime.Now;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            StartTimer();
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
        private void Adminload()
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
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                btnComplete_Click(sender, e);


            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.N)
            {
                btnNotes_Click(sender, e);


            }
        }
        private void Curser_Loaded(object sender, RoutedEventArgs e)
        {
            palletsinTextBox.Focus();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = endTime - DateTime.Now;
            if (remainingTime.TotalSeconds <= 0)
            {
                Timer.Stop();
                timerTextBox.Text = "TIME'S UP!";
            }
            else
            {
                timerTextBox.Text = remainingTime.ToString(@"h\:mm\:ss");
                ControlRoom.Instance.timer1TextBox.Text = remainingTime.ToString(@"h\:mm\:ss");
                DockScreen.Instance.timer1TextBox.Text = remainingTime.ToString(@"h\:mm\:ss");


            }
            if (remainingTime.TotalMinutes <= 30)
            {
                ControlRoom.Instance.btnLiveDoor1.Visibility = Visibility.Collapsed;
                ControlRoom.Instance.btnLiveRushDoor1.Visibility = Visibility.Visible;
                DockScreen.Instance.btnLiveDoor1.Visibility = Visibility.Collapsed;
                DockScreen.Instance.btnLiveRushDoor1.Visibility = Visibility.Visible;

            }
        }
        private void StartTimer()
        {
            endTime = DateTime.Now.AddMinutes(90);
            Timer.Start();
        }

        private void btnNotes_Click(object sender, RoutedEventArgs e)
        {
            NotesDoor1 notesDoor1 = Application.Current.Windows.OfType<NotesDoor1>().FirstOrDefault();

            if (notesDoor1 != null)
            {
                if (notesDoor1.WindowState == WindowState.Minimized)
                {
                    notesDoor1.WindowState = WindowState.Normal;
                }
                else if (notesDoor1.IsActive)
                {
                    notesDoor1.Activate();
                }
            }
            if (notesDoor1 == null)
            {
                notesDoor1 = new NotesDoor1();
                notesDoor1.Left = 860;
                notesDoor1.Top = 285;
                notesDoor1.Show();
            }
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();

            ArrivalTime = timeinTextBox.Text;
            DoorAssigned = doorTextBox.Text;
            LoadID = loadidTextBox.Text;
            Carrier = carrierTextBox.Text;
            Customer = customerTextBox.Text;
            Trailer = trailernumberTextBox.Text;
            DropPreload = "LIVE";
            PLTINOUT = palletsinTextBox.Text;
            CompletedTime = ctime.ToString("hh:mm");

            string sfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";
            string dfilePath = "C:\\Users\\Public\\Documents\\DailyDownload.csv";
            string[] slines = File.ReadAllLines(sfilePath);
            string[] dlines = File.ReadAllLines(dfilePath);

            NotesDoor1 notesDoor1 = Application.Current.Windows.OfType<NotesDoor1>().FirstOrDefault();

            if (notesDoor1 == null)
            {
                for (int s = 1; s < slines.Length; s++)
                {
                    string[] scolumns = slines[s].Split(',');

                    if (scolumns.Length >= 13 && scolumns[11] == doorTextBox.Text && scolumns[3] == loadidTextBox.Text)
                    {
                        scolumns[6] = "COMPLETE";
                        scolumns[7] = associateTextBox.Text;
                        scolumns[10] = palletsinTextBox.Text;
                        scolumns[12] = ctime.ToString("hh:mm");

                        slines[s] = string.Join(",", scolumns);

                        File.WriteAllLines(sfilePath, slines);
                    }
                }
                for (int d = 1; d < dlines.Length; d++)
                {
                    string[] dcolumns = dlines[d].Split(',');

                    if (dcolumns.Length >= 13 && dcolumns[11] == doorTextBox.Text && dcolumns[3] == loadidTextBox.Text)
                    {
                        dcolumns[6] = "COMPLETE";
                        dcolumns[7] = associateTextBox.Text;
                        dcolumns[10] = palletsinTextBox.Text;
                        dcolumns[12] = ctime.ToString("hh:mm");

                        dlines[d] = string.Join(",", dcolumns);

                        File.WriteAllLines(dfilePath, dlines);
                    }
                }
                ControlRoom controlRoom = Application.Current.Windows.OfType<ControlRoom>().FirstOrDefault();
                if (controlRoom != null) 
                {
                    
                    controlRoom.timer1TextBox.Visibility = Visibility.Hidden;
                    DockScreen.Instance.timer1TextBox.Visibility = Visibility.Hidden;

                    if (controlRoom.btnLiveDoor1.Visibility == Visibility.Visible)
                    {
                        controlRoom.btnLiveDoor1.Visibility = Visibility.Collapsed;
                        DockScreen.Instance.btnLiveDoor1.Visibility = Visibility.Collapsed;
                    }
                    else if (controlRoom.btnLiveRushDoor1.Visibility == Visibility.Visible)
                    {
                        controlRoom.btnLiveRushDoor1.Visibility = Visibility.Collapsed;
                        DockScreen.Instance.btnLiveRushDoor1.Visibility = Visibility.Collapsed;
                    }
                    controlRoom.btnDoor1.Visibility = Visibility.Visible;
                    DockScreen.Instance.btnDoor1.Visibility = Visibility.Visible;

                    controlRoom.scheduleCsvData();
                    DockScreen.Instance.scheduleCsvData();
                    this.Close();
                }
            }
            else
            {
                string updatenotes = notesDoor1.loadnotesTextBox.Text;

                for (int s = 1; s < slines.Length; s++)
                {
                    string[] scolumns = slines[s].Split(',');

                    if (scolumns.Length >= 13 && scolumns[11] == doorTextBox.Text && scolumns[3] == loadidTextBox.Text)
                    {
                        scolumns[6] = "COMPLETED";
                        scolumns[7] = associateTextBox.Text;
                        scolumns[8] = updatenotes;
                        scolumns[10] = palletsinTextBox.Text;
                        scolumns[12] = ctime.ToString("hh:mm");

                        slines[s] = string.Join(",", scolumns);
                        File.WriteAllLines(sfilePath, slines);
                    }
                }
                for (int d = 1; d < dlines.Length; d++)
                {
                    string[] dcolumns = dlines[d].Split(',');

                    if (dcolumns.Length >= 13 && dcolumns[11] == doorTextBox.Text && dcolumns[3] == loadidTextBox.Text)
                    {
                        dcolumns[6] = "COMPLETE";
                        dcolumns[7] = associateTextBox.Text;
                        dcolumns[8] = updatenotes;
                        dcolumns[10] = palletsinTextBox.Text;
                        dcolumns[12] = ctime.ToString("hh:mm");

                        dlines[d] = string.Join(",", dcolumns);

                        File.WriteAllLines(dfilePath, dlines);
                    }
                }
                ControlRoom controlRoom = Application.Current.Windows.OfType<ControlRoom>().FirstOrDefault();
                if (controlRoom != null)
                {

                    controlRoom.timer1TextBox.Visibility = Visibility.Hidden;
                    DockScreen.Instance.timer1TextBox.Visibility = Visibility.Hidden;

                    if (controlRoom.btnLiveDoor1.Visibility == Visibility.Visible)
                    {
                        controlRoom.btnLiveDoor1.Visibility = Visibility.Collapsed;
                        DockScreen.Instance.btnLiveDoor1.Visibility = Visibility.Collapsed;
                    }
                    else if (controlRoom.btnLiveRushDoor1.Visibility == Visibility.Visible)
                    {
                        controlRoom.btnLiveRushDoor1.Visibility = Visibility.Collapsed;
                        DockScreen.Instance.btnLiveRushDoor1.Visibility = Visibility.Collapsed;
                    }
                    controlRoom.btnDoor1.Visibility = Visibility.Visible;
                    DockScreen.Instance.btnDoor1.Visibility = Visibility.Visible;

                    controlRoom.scheduleCsvData();
                    DockScreen.Instance.scheduleCsvData();
                    notesDoor1.Close();
                    this.Close();
                }
            }
        }
    }
}
