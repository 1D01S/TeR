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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeR;

namespace TeR
{
    public partial class MainWindow : Window
    {
        private readonly TEntities db;
        private bool isGuestUser = false;

        public MainWindow()
        {
            InitializeComponent();
            db = new TEntities();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            UpdateUIBasedOnUserRole();
        }
        private void UpdateUIBasedOnUserRole()
        {
            if (isGuestUser)
            {
                // Disable or hide buttons and commands related to editing, adding, or deleting records
                // For example:
                // ...
            }
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Update the UI to reflect the user's guest status
            UpdateUIBasedOnUserRole();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = "a";
            string password = "p";

            if (usernameTextBox.Text == username && passwordBox.Password == password)
            {
                resultTextBlock.Text = "Вход выполнен успешно! 🎉";

                // Open the MENU window
                MENU menu = new MENU(db, false);
                Dispatcher.Invoke(() => MainFrame.Navigate(menu));

                // Navigate to the _event page after a successful login
                _event eventPage = new _event(db, false, MainFrame);
                menu.Dispatcher.Invoke(() => menu.MainFrame.Navigate(eventPage));

                isGuestUser = false;
            }
            else
            {
                resultTextBlock.Text = "Ошибка входа. Пожалуйста, проверьте имя пользователя и пароль. 😕";
            }
        }

        private void GuestLoginButton_Click(object sender, RoutedEventArgs e)
        {
            resultTextBlock.Text = "Вход как гость выполнен успешно! 🎉";

            // Set the IsGuestUser property in the App class
            (App.Current as App).IsGuestUser = true;

            // Open the MENU window
            MENU menu = new MENU(db, true);
            Dispatcher.Invoke(() => MainFrame.Navigate(menu));

            // Navigate to the _event page after a successful login
            _event eventPage = new _event(db, true, MainFrame);
            menu.Dispatcher.Invoke(() => eventPage.UpdateUIBasedOnUserRole((App.Current as App).IsGuestUser, eventPage));
            menu.Dispatcher.Invoke(() => menu.MainFrame.Navigate(eventPage));
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (isGuestUser)
            {
                MessageBox.Show("As a guest user, you don't have the necessary permissions to save changes.", "Permission denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}