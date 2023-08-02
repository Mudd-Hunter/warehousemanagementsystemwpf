using CsvHelper;
using MySqlConnector;
using System;
using System.Collections.Generic;
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

namespace DS_Warehouse_Management_System_v1._2
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    public static class UserSession
    {
        public static string UserName { get; set; }
        public static bool IsAdmin { get; set; }
    }
    public partial class Login : Window
    {
        private string conn = "Server=208.109.49.241;Database=appusers;Uid=uploader72;Pwd=3825Harley!!;";
        public Login()
        {
            InitializeComponent();

            var sysTime = datetimetextbox.Text;
            DateTime currentTime = DateTime.Now;
            datetimetextbox.Text = sysTime != "" ? sysTime : currentTime.ToString("MMM/ddd hh:mm");

            PreviewKeyDown += Window_PreviewKeyDown;
            

            LoadUsernames();
        }
        private void Curser_Loaded(object sender, RoutedEventArgs e)
        {
            usernameComboBox.Focus();
        }
        public class DailyUserLog
        {
            public string user { get; set; }
            public string pass { get; set; }
            public string sysTime { get; set; }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void LoadUsernames()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(conn))
                {
                    connection.Open();
                    string query = "SELECT user FROM current";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> usernames = new List<string>();
                        while (reader.Read())
                        {
                            usernames.Add(reader["user"].ToString());
                        }

                        usernameComboBox.ItemsSource = usernames;



                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading usernames: " + ex.Message);
            }
        }
        private void usernameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            passwordPasswordBox.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string user = usernameComboBox.SelectedItem as string;
            string pass = passwordPasswordBox.Password;

            var ufilePath = "C:\\Users\\Public\\Documents\\DailyUserLog.csv";

            using (MySqlConnection connection = new MySqlConnection(conn))
            {
                connection.Open();

                string query = "SELECT * FROM current WHERE user = @user AND pass = @pass";
                MySqlCommand command = new MySqlCommand(@query, connection);
                command.Parameters.AddWithValue("@user", user);
                command.Parameters.AddWithValue("@pass", pass);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserSession.UserName = user;
                        UserSession.IsAdmin = Convert.ToBoolean(reader["isAdmin"]);

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();

                        var sysTime = datetimetextbox.Text;

                        using (var uwriter = new StreamWriter(ufilePath, true))
                        using (var csvuWriter = new CsvWriter(uwriter, CultureInfo.InvariantCulture))
                        {
                            var ufileIsEmpty = new FileInfo(ufilePath).Length == 0;

                            if (ufileIsEmpty)
                            {
                                csvuWriter.WriteHeader<DailyUserLog>();
                                csvuWriter.NextRecord();
                            }

                            csvuWriter.WriteField(user);
                            csvuWriter.WriteField(pass);
                            csvuWriter.WriteField(sysTime);
                            csvuWriter.NextRecord();
                        }

                        var adminMenuItem = MainWindow.Instance.FindMenuItemByHeader(MainWindow.Instance.Topmenu, "_Admin");

                        if (UserSession.IsAdmin)
                        {
                            adminMenuItem.Visibility = Visibility.Visible;
                            
                        }
                        else
                        {
                            return;
                        }
                        return;
                      
                    }
                    else
                    {
                        MessageBox.Show("Username and/or Password are Incorret");
                        passwordPasswordBox.Clear();
                        usernameComboBox.Focus();
                    }
                }
                
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
