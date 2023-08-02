﻿using System;
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
    /// Interaction logic for NotesDoor1.xaml
    /// </summary>
    public partial class NotesDoor1 : Window
    {
        public static NotesDoor1 Instance;
        public NotesDoor1()
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
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                btnUpdate_Click(sender, e);


            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditNotes1 editNotes1 = new EditNotes1();
            editNotes1.loadnotesTextBox.Text = loadnotesTextBox.Text;
            editNotes1.Show();
            this.WindowState = WindowState.Minimized;
        }
    }
}
