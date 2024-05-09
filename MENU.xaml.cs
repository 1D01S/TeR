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

namespace TeR
{
    public partial class MENU : Page
    {
        private readonly TEntities db;
        private bool isGuestUser;
        public MENU()
        {
            InitializeComponent();
            db = new TEntities();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
        }




        private void Page1Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new update(db));
        }

        private void Page2Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new removed(db));
        }

        private void ItemsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new item(db));
        }

        private void PlantsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new plant(db));
        }

        private void CreaturesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new creature(db));
        }

        private void BiomesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new biome(db));
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            // In the EventsButton_Click method
            MainFrame.Navigate(new _event(db, isGuestUser, MainFrame));
        }

        private void GameModesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new mode(db));
        }

        private void MechanicsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new mechanic(db));
        }

        private void CharacterClassesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new _class(db));
        }

        private void AchievementsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new achievement(db));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        internal void Show()
        {
            throw new NotImplementedException();
        }
    }
}
