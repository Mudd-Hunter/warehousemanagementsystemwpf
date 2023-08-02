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
using System.Windows.Shapes;

namespace DS_Warehouse_Management_System_v1._2.Notes
{
    /// <summary>
    /// Interaction logic for EditNotes.xaml
    /// </summary>
    public partial class EditNotes1 : Window
    {
        public static EditNotes1 Instance;
        public EditNotes1()
        {
            InitializeComponent();
            Instance = this;
            Adminload();

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
        private void Curser_Loaded(object sender, RoutedEventArgs e)
        {
            loadnotesTextBox.Focus();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                btnCancel_Click(sender, e);


            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                btnSave_Click(sender, e);


            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            NotesDoor1.Instance.WindowState = WindowState.Normal;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool isUpdated = false;

            Live.Inbound.Door1 lidoor1 = Application.Current.Windows.OfType<Live.Inbound.Door1>().FirstOrDefault();

            if (lidoor1 != null)
            {
                if (NotesDoor1.Instance == null)
                {
                    NotesDoor1 notesDoor1 = new NotesDoor1();
                    notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    notesDoor1.Show();
                    isUpdated = true;
                }
                else
                {
                    NotesDoor1.Instance.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                    isUpdated = true;
                }

            }
            Live.Outbound.Door1 lodoor1 = Application.Current.Windows.OfType<Live.Outbound.Door1>().FirstOrDefault();
            if (lodoor1 != null)
            {
                if (NotesDoor1.Instance == null)
                {
                    NotesDoor1 notesDoor1 = new NotesDoor1();
                    notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    notesDoor1.Show();
                    isUpdated = true;
                }
                else
                {
                    NotesDoor1.Instance.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                    isUpdated = true;
                }
            }
            DropDoor.Inbound.Door1 didoor1 = Application.Current.Windows.OfType<DropDoor.Inbound.Door1>().FirstOrDefault();
            if (didoor1 != null)
            {
                if (NotesDoor1.Instance == null)
                {
                    NotesDoor1 notesDoor1 = new NotesDoor1();
                    notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    notesDoor1.Show();
                    isUpdated = true;
                }
                else
                {
                    NotesDoor1.Instance.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                    isUpdated = true;
                }
            }
            DropDoor.Preload.Door1 pre1 = Application.Current.Windows.OfType<DropDoor.Preload.Door1>().FirstOrDefault();
            if (pre1 != null)
            {
                if (NotesDoor1.Instance == null)
                {
                    NotesDoor1 notesDoor1 = new NotesDoor1();
                    notesDoor1.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    notesDoor1.Show();
                    isUpdated = true;
                }
                else
                {
                    NotesDoor1.Instance.loadnotesTextBox.Text = loadnotesTextBox.Text;
                    NotesDoor1.Instance.WindowState = WindowState.Normal;
                    isUpdated = true;
                }
            }
            if (isUpdated)
            {
                this.Close();
            }

        }
    }
}
