using CsvHelper;
using CsvHelper.Configuration.Attributes;
using DS_Warehouse_Management_System_v1._2.Notes;
using DS_Warehouse_Management_System_v1._2.DropDoor;
using System;
using System.Collections.Generic;
using System.Globalization;
using DS_Warehouse_Management_System_v1._2.Live;
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
    /// Interaction logic for EnterLoadInfo.xaml
    /// </summary>
    public partial class EnterLoadInfo : Window
    {
        public static EnterLoadInfo Instance;
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
        public EnterLoadInfo(CsvData selectedItem)
        {
            InitializeComponent();
            Instance = this;

            DailyDownload();
            loadtimerTextBox.Text = DateTime.Now.ToString("hh:mm");
            loadidTextBox.Text = selectedItem.LoadID;
            customerTextBox.Text = selectedItem.Customer;
            carrierTextBox.Text = selectedItem.Carrier;
            trailernumberTextBox.Text = selectedItem.Trailer;
            inoutTextBox.Text = selectedItem.INOUT;
            droppreTextBox.Text = selectedItem.DropPreload;
            DropStatus();
            Adminload();
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

        private void Curser_Loaded(object sender, RoutedEventArgs e)
        {
            
                loadidTextBox.Focus();
            
        }
        public void DailyDownload()
        {
            var dfilePath = "C:\\Users\\Public\\Documents\\DailyDownload.csv";

            using (var dwriter = new StreamWriter(dfilePath, true))
            using (var csvdWriter = new CsvWriter(dwriter, CultureInfo.InvariantCulture))
            {
                var dfileIsEmpty = new FileInfo(dfilePath).Length == 0;
                
                if (dfileIsEmpty)
                {
                    csvdWriter.WriteHeader<Daily>();
                    csvdWriter.NextRecord();
                }
            }
        }
        public void DropStatus()
        {
            if (inoutTextBox.Text == "IN" && droppreTextBox.Text == "DROP")
            {
                btnDropLoadDoor.Visibility = Visibility.Visible;
                btnDropLoadYard.Visibility = Visibility.Visible;
            }
        }
        public string LIVE = "LIVE";
        public string DROP = "DROP";

        private void btnDropLoadDoor_Checked(object sender, RoutedEventArgs e)
        {
            droppreTextBox.Text = DROP;
        }

        private void btnDropLoadYard_Checked(object sender, RoutedEventArgs e)
        {
            droppreTextBox.Text = "YARD";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            ArrivalTime = loadtimerTextBox.Text;
            DoorAssigned = doorTextBox.Text;
            LoadID = loadidTextBox.Text;
            Customer = customerTextBox.Text;
            Carrier = carrierTextBox.Text;
            Trailer = trailernumberTextBox.Text;
            Notes = loadnotesTextBox.Text;
            INOUT = inoutTextBox.Text;
            
            if (droppreTextBox.Text == "LIVE" && INOUT == "IN")
            {
                if (DoorAssigned == "1")
                {
                    ControlRoom.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    ControlRoom.Instance.btnLiveDoor1.Visibility = Visibility.Visible;
                    ControlRoom.Instance.timer1TextBox.Visibility = Visibility.Visible;
                    DockScreen.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    DockScreen.Instance.btnLiveDoor1.Visibility = Visibility.Visible;
                    DockScreen.Instance.timer1TextBox.Visibility = Visibility.Visible;

                    Live.Inbound.Door1 lidoor1 = new Live.Inbound.Door1();
                    lidoor1.Left = 10;
                    lidoor1.Top = 285;
                    lidoor1.Show();

                    string sfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";
                    string tempsfilePath = "C:\\Users\\Public\\Documents\\TempSchedule.csv";

                    using (var sreader = new StreamReader(sfilePath))
                    using (var csvsReader = new CsvReader(sreader, CultureInfo.InvariantCulture))
                    {
                        var srecords = csvsReader.GetRecords<ScheduleEntry>().ToList();

                        var sheaderRecord = csvsReader.HeaderRecord;

                        using (var swriter = new StreamWriter(tempsfilePath))
                        using (var csvsWriter = new CsvWriter(swriter, CultureInfo.InvariantCulture))
                        {
                            csvsWriter.WriteField("Appointment Time");
                            csvsWriter.WriteField("Arrival Time");
                            csvsWriter.WriteField("Customer");
                            csvsWriter.WriteField("Load ID");
                            csvsWriter.WriteField("Carrier");
                            csvsWriter.WriteField("Trailer");
                            csvsWriter.WriteField("Inbound/Outbound");
                            csvsWriter.WriteField("Associate");
                            csvsWriter.WriteField("Notes");
                            csvsWriter.WriteField("Drop/Preload");
                            csvsWriter.WriteField("PLT IN/OUT");
                            csvsWriter.WriteField("Door Assigned");
                            csvsWriter.WriteField("Completed Time");
                            csvsWriter.NextRecord();

                            bool matchFound = false;

                            foreach (var srecord in srecords)
                            {
                                if (srecord.LoadID == loadidTextBox.Text)
                                {
                                    srecord.Notes = loadnotesTextBox.Text;
                                    srecord.ArrivalTime = loadtimerTextBox.Text;
                                    srecord.Trailer = trailernumberTextBox.Text;
                                    srecord.DoorAssigned = doorTextBox.Text;
                                    matchFound = true;

                                    
                                }
                                
                                csvsWriter.WriteRecord(srecord);
                                csvsWriter.NextRecord();
                            }
                            
                            if (!matchFound)
                            {
                                var newsRecord = new ScheduleEntry
                                {
                                    AppointmentTime = "WORK IN",
                                    ArrivalTime = loadtimerTextBox.Text,
                                    Customer = customerTextBox.Text,
                                    LoadID = loadidTextBox.Text,
                                    Carrier = carrierTextBox.Text,
                                    Trailer = trailernumberTextBox.Text,
                                    INOUT = inoutTextBox.Text,
                                    Associate = "",
                                    Notes = loadnotesTextBox.Text,
                                    DropPreload = "LIVE",
                                    DoorAssigned = "1",
                                    CompletedTime = "",
                                };
                                csvsWriter.WriteRecord(newsRecord);
                                csvsWriter.NextRecord();
                            }
                        }
                    }
                    File.Replace(tempsfilePath, sfilePath, null);
                    if (string.IsNullOrEmpty(loadnotesTextBox.Text) )
                    {
                        lidoor1.WindowState = WindowState.Minimized;

                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                    else
                    {
                        NotesDoor1 notesDoor1 = new NotesDoor1();
                        notesDoor1.Left = 860;
                        notesDoor1.Top = 285;
                        notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                        notesDoor1.Show();
                        notesDoor1.WindowState = WindowState.Minimized;
                        lidoor1.WindowState= WindowState.Minimized;
                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                }

            }
            else if (droppreTextBox.Text == "LIVE" && INOUT == "OUT")
            {
                if (DoorAssigned == "1")
                {
                    ControlRoom.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    ControlRoom.Instance.btnLiveOutDoor1.Visibility = Visibility.Visible;
                    ControlRoom.Instance.timer1TextBox.Visibility = Visibility.Visible;
                    DockScreen.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    DockScreen.Instance.btnLiveOutDoor1.Visibility = Visibility.Visible;
                    DockScreen.Instance.timer1TextBox.Visibility = Visibility.Visible;

                    Live.Outbound.Door1 lodoor1 = new Live.Outbound.Door1();
                    lodoor1.Left = 10;
                    lodoor1.Top = 285;
                    lodoor1.Show();

                    string sfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";
                    string tempsfilePath = "C:\\Users\\Public\\Documents\\TempSchedule.csv";

                    using (var sreader = new StreamReader(sfilePath))
                    using (var csvsReader = new CsvReader(sreader, CultureInfo.InvariantCulture))
                    {
                        var srecords = csvsReader.GetRecords<ScheduleEntry>().ToList();

                        var sheaderRecord = csvsReader.HeaderRecord;

                        using (var swriter = new StreamWriter(tempsfilePath))
                        using (var csvsWriter = new CsvWriter(swriter, CultureInfo.InvariantCulture))
                        {
                            csvsWriter.WriteField("Appointment Time");
                            csvsWriter.WriteField("Arrival Time");
                            csvsWriter.WriteField("Customer");
                            csvsWriter.WriteField("Load ID");
                            csvsWriter.WriteField("Carrier");
                            csvsWriter.WriteField("Trailer");
                            csvsWriter.WriteField("Inbound/Outbound");
                            csvsWriter.WriteField("Associate");
                            csvsWriter.WriteField("Notes");
                            csvsWriter.WriteField("Drop/Preload");
                            csvsWriter.WriteField("PLT IN/OUT");
                            csvsWriter.WriteField("Door Assigned");
                            csvsWriter.WriteField("Completed Time");
                            csvsWriter.NextRecord();

                            bool matchFound = false;

                            foreach (var srecord in srecords)
                            {
                                if (srecord.LoadID == loadidTextBox.Text)
                                {
                                    srecord.Notes = loadnotesTextBox.Text;
                                    srecord.ArrivalTime = loadtimerTextBox.Text;
                                    srecord.Trailer = trailernumberTextBox.Text;
                                    srecord.DoorAssigned = doorTextBox.Text;
                                    matchFound = true;


                                }

                                csvsWriter.WriteRecord(srecord);
                                csvsWriter.NextRecord();
                            }

                            if (!matchFound)
                            {
                                var newsRecord = new ScheduleEntry
                                {
                                    AppointmentTime = "WORK IN",
                                    ArrivalTime = loadtimerTextBox.Text,
                                    Customer = customerTextBox.Text,
                                    LoadID = loadidTextBox.Text,
                                    Carrier = carrierTextBox.Text,
                                    Trailer = trailernumberTextBox.Text,
                                    INOUT = inoutTextBox.Text,
                                    Associate = "",
                                    Notes = loadnotesTextBox.Text,
                                    DropPreload = "LIVE",
                                    DoorAssigned = "1",
                                    CompletedTime = "",
                                };
                                csvsWriter.WriteRecord(newsRecord);
                                csvsWriter.NextRecord();
                            }
                        }
                    }
                    File.Replace(tempsfilePath, sfilePath, null);
                    if (string.IsNullOrEmpty(loadnotesTextBox.Text))
                    {
                        lodoor1.WindowState = WindowState.Minimized;

                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                    else
                    {
                        NotesDoor1 notesDoor1 = new NotesDoor1();
                        notesDoor1.Left = 860;
                        notesDoor1.Top = 285;
                        notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                        notesDoor1.Show();
                        notesDoor1.WindowState = WindowState.Minimized;
                        lodoor1.WindowState = WindowState.Minimized;
                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                }
            }
            else if (droppreTextBox.Text == "DROP" && INOUT == "IN")
            {
                if (DoorAssigned == "1")
                {
                    ControlRoom.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    ControlRoom.Instance.btnDropDoor1.Visibility = Visibility.Visible;
                    DockScreen.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    DockScreen.Instance.btnDropDoor1.Visibility = Visibility.Visible;

                    DropDoor.Inbound.Door1 dropDoor1 = new DropDoor.Inbound.Door1();
                    dropDoor1.Left = 10;
                    dropDoor1.Top = 285;
                    dropDoor1.Show();

                    string sfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";
                    string tempsfilePath = "C:\\Users\\Public\\Documents\\TempSchedule.csv";

                    using (var sreader = new StreamReader(sfilePath))
                    using (var csvsReader = new CsvReader(sreader, CultureInfo.InvariantCulture))
                    {
                        var srecords = csvsReader.GetRecords<ScheduleEntry>().ToList();

                        var sheaderRecord = csvsReader.HeaderRecord;

                        using (var swriter = new StreamWriter(tempsfilePath))
                        using (var csvsWriter = new CsvWriter(swriter, CultureInfo.InvariantCulture))
                        {
                            csvsWriter.WriteField("Appointment Time");
                            csvsWriter.WriteField("Arrival Time");
                            csvsWriter.WriteField("Customer");
                            csvsWriter.WriteField("Load ID");
                            csvsWriter.WriteField("Carrier");
                            csvsWriter.WriteField("Trailer");
                            csvsWriter.WriteField("Inbound/Outbound");
                            csvsWriter.WriteField("Associate");
                            csvsWriter.WriteField("Notes");
                            csvsWriter.WriteField("Drop/Preload");
                            csvsWriter.WriteField("PLT IN/OUT");
                            csvsWriter.WriteField("Door Assigned");
                            csvsWriter.WriteField("Completed Time");
                            csvsWriter.NextRecord();

                            bool matchFound = false;

                            foreach (var srecord in srecords)
                            {
                                if (srecord.LoadID == loadidTextBox.Text)
                                {
                                    srecord.Notes = loadnotesTextBox.Text;
                                    srecord.ArrivalTime = loadtimerTextBox.Text;
                                    srecord.Trailer = trailernumberTextBox.Text;
                                    srecord.DoorAssigned = doorTextBox.Text;
                                    matchFound = true;


                                }

                                csvsWriter.WriteRecord(srecord);
                                csvsWriter.NextRecord();
                            }

                            if (!matchFound)
                            {
                                var newsRecord = new ScheduleEntry
                                {
                                    AppointmentTime = "WORK IN",
                                    ArrivalTime = loadtimerTextBox.Text,
                                    Customer = customerTextBox.Text,
                                    LoadID = loadidTextBox.Text,
                                    Carrier = carrierTextBox.Text,
                                    Trailer = trailernumberTextBox.Text,
                                    INOUT = inoutTextBox.Text,
                                    Associate = "",
                                    Notes = loadnotesTextBox.Text,
                                    DropPreload = "DROP",
                                    DoorAssigned = "1",
                                    CompletedTime = "",
                                };
                                csvsWriter.WriteRecord(newsRecord);
                                csvsWriter.NextRecord();
                            }
                        }
                    }
                    File.Replace(tempsfilePath, sfilePath, null);
                    if (string.IsNullOrEmpty(loadnotesTextBox.Text))
                    {
                        dropDoor1.WindowState = WindowState.Minimized;
                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                    else
                    {
                        NotesDoor1 notesDoor1 = new NotesDoor1();
                        notesDoor1.Left = 860;
                        notesDoor1.Top = 285;
                        notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                        notesDoor1.Show();
                        notesDoor1.WindowState = WindowState.Minimized;
                        dropDoor1.WindowState = WindowState.Minimized;
                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                }
            }
            else if (droppreTextBox.Text == "PRE" && INOUT == "OUT")
            {
                if (DoorAssigned == "1")
                {
                    ControlRoom.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    ControlRoom.Instance.btnPreload1.Visibility = Visibility.Visible;
                    DockScreen.Instance.btnDoor1.Visibility = Visibility.Collapsed;
                    DockScreen.Instance.btnPreload1.Visibility = Visibility.Visible;

                    DropDoor.Preload.Door1 preDoor1 = new DropDoor.Preload.Door1();
                    preDoor1.Left = 10;
                    preDoor1.Top = 285;
                    preDoor1.Show();

                    string sfilePath = "C:\\Users\\Public\\Documents\\Schedule.csv";
                    string tempsfilePath = "C:\\Users\\Public\\Documents\\TempSchedule.csv";

                    using (var sreader = new StreamReader(sfilePath))
                    using (var csvsReader = new CsvReader(sreader, CultureInfo.InvariantCulture))
                    {
                        var srecords = csvsReader.GetRecords<ScheduleEntry>().ToList();

                        var sheaderRecord = csvsReader.HeaderRecord;

                        using (var swriter = new StreamWriter(tempsfilePath))
                        using (var csvsWriter = new CsvWriter(swriter, CultureInfo.InvariantCulture))
                        {
                            csvsWriter.WriteField("Appointment Time");
                            csvsWriter.WriteField("Arrival Time");
                            csvsWriter.WriteField("Customer");
                            csvsWriter.WriteField("Load ID");
                            csvsWriter.WriteField("Carrier");
                            csvsWriter.WriteField("Trailer");
                            csvsWriter.WriteField("Inbound/Outbound");
                            csvsWriter.WriteField("Associate");
                            csvsWriter.WriteField("Notes");
                            csvsWriter.WriteField("Drop/Preload");
                            csvsWriter.WriteField("PLT IN/OUT");
                            csvsWriter.WriteField("Door Assigned");
                            csvsWriter.WriteField("Completed Time");
                            csvsWriter.NextRecord();

                            bool matchFound = false;

                            foreach (var srecord in srecords)
                            {
                                if (srecord.LoadID == loadidTextBox.Text)
                                {
                                    srecord.Notes = loadnotesTextBox.Text;
                                    srecord.ArrivalTime = loadtimerTextBox.Text;
                                    srecord.Trailer = trailernumberTextBox.Text;
                                    srecord.DoorAssigned = doorTextBox.Text;
                                    matchFound = true;


                                }

                                csvsWriter.WriteRecord(srecord);
                                csvsWriter.NextRecord();
                            }

                            if (!matchFound)
                            {
                                var newsRecord = new ScheduleEntry
                                {
                                    AppointmentTime = "WORK IN",
                                    ArrivalTime = loadtimerTextBox.Text,
                                    Customer = customerTextBox.Text,
                                    LoadID = loadidTextBox.Text,
                                    Carrier = carrierTextBox.Text,
                                    Trailer = trailernumberTextBox.Text,
                                    INOUT = inoutTextBox.Text,
                                    Associate = "",
                                    Notes = loadnotesTextBox.Text,
                                    DropPreload = "DROP",
                                    DoorAssigned = "1",
                                    CompletedTime = "",
                                };
                                csvsWriter.WriteRecord(newsRecord);
                                csvsWriter.NextRecord();
                            }
                        }
                    }
                    File.Replace(tempsfilePath, sfilePath, null);
                    if (string.IsNullOrEmpty(loadnotesTextBox.Text))
                    {
                        preDoor1.WindowState = WindowState.Minimized;
                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                    else
                    {
                        NotesDoor1 notesDoor1 = new NotesDoor1();
                        notesDoor1.Left = 860;
                        notesDoor1.Top = 285;
                        notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                        notesDoor1.Show();
                        notesDoor1.WindowState = WindowState.Minimized;
                        preDoor1.WindowState = WindowState.Minimized;
                        ControlRoom.Instance.Activate();
                        ControlRoom.Instance.scheduleCsvData();
                        DockScreen.Instance.scheduleCsvData();
                        this.Close();
                    }
                }
            }

            string Lives = droppreTextBox.Text;

            var dfilePath = "C:\\Users\\Public\\Documents\\DailyDownload.csv";

            if (Lives == "LIVE")
            {
                if (doorTextBox.Text == "1")
                {
                    using (var dwriter = new StreamWriter(dfilePath, true))
                    using (var dcsv = new CsvWriter(dwriter, CultureInfo.InvariantCulture))
                    {
                        var fileIsEmpty = new FileInfo(dfilePath).Length == 0;

                        if (fileIsEmpty)
                        {
                            dcsv.WriteHeader<Daily>();
                            dcsv.NextRecord();
                        }

                        dcsv.WriteField(AppointmentTime);
                        dcsv.WriteField(ArrivalTime);
                        dcsv.WriteField(Customer);
                        dcsv.WriteField(LoadID);
                        dcsv.WriteField(Carrier);
                        dcsv.WriteField(Trailer);
                        dcsv.WriteField(INOUT);
                        dcsv.WriteField(Associate);
                        dcsv.WriteField(Notes);
                        dcsv.WriteField(DropPreload);
                        dcsv.WriteField(PLTINOUT);
                        dcsv.WriteField(DoorAssigned);
                        dcsv.WriteField(CompletedTime);
                        dcsv.NextRecord();
                    }
                }
            }
            else if (Lives == "DROP")
            {
                if (doorTextBox.Text == "1")
                {
                    using (var dwriter = new StreamWriter(dfilePath, true))
                    using (var dcsv = new CsvWriter(dwriter, CultureInfo.InvariantCulture))
                    {
                        var fileIsEmpty = new FileInfo(dfilePath).Length == 0;

                        if (fileIsEmpty)
                        {
                            dcsv.WriteHeader<Daily>();
                            dcsv.NextRecord();
                        }

                        dcsv.WriteField(AppointmentTime);
                        dcsv.WriteField(ArrivalTime);
                        dcsv.WriteField(Customer);
                        dcsv.WriteField(LoadID);
                        dcsv.WriteField(Carrier);
                        dcsv.WriteField(Trailer);
                        dcsv.WriteField(INOUT);
                        dcsv.WriteField(Associate);
                        dcsv.WriteField(Notes);
                        dcsv.WriteField(DropPreload);
                        dcsv.WriteField(PLTINOUT);
                        dcsv.WriteField(DoorAssigned);
                        dcsv.WriteField(CompletedTime);
                        dcsv.NextRecord();
                    }
                }
            }
            else if (Lives == "PRE")
            {
                if (doorTextBox.Text == "1")
                {
                    using (var dwriter = new StreamWriter(dfilePath, true))
                    using (var dcsv = new CsvWriter(dwriter, CultureInfo.InvariantCulture))
                    {
                        var fileIsEmpty = new FileInfo(dfilePath).Length == 0;

                        if (fileIsEmpty)
                        {
                            dcsv.WriteHeader<Daily>();
                            dcsv.NextRecord();
                        }

                        dcsv.WriteField(AppointmentTime);
                        dcsv.WriteField(ArrivalTime);
                        dcsv.WriteField(Customer);
                        dcsv.WriteField(LoadID);
                        dcsv.WriteField(Carrier);
                        dcsv.WriteField(Trailer);
                        dcsv.WriteField(INOUT);
                        dcsv.WriteField(Associate);
                        dcsv.WriteField(Notes);
                        dcsv.WriteField(DropPreload);
                        dcsv.WriteField(PLTINOUT);
                        dcsv.WriteField(DoorAssigned);
                        dcsv.WriteField(CompletedTime);
                        dcsv.NextRecord();
                    }
                }
            }
        }

        private void btnIN_Checked(object sender, RoutedEventArgs e)
        {
            INOUT = "IN";
        }

        private void btnEmpty_Checked(object sender, RoutedEventArgs e)
        {
            INOUT = "EMPTY";
        }
    }
}
