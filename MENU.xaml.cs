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
        public MENU(TEntities entities, bool isGuestUser)
        {
            InitializeComponent();
            db = new TEntities();
            this.isGuestUser = isGuestUser;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
        }




        private void Page1Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new update(db, isGuestUser, MainFrame));
        }

        private void Page2Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new removed(db, isGuestUser, MainFrame));
        }

        private void ItemsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new item(db, isGuestUser, MainFrame));
        }

        private void PlantsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new plant(db, isGuestUser, MainFrame));
        }

        private void CreaturesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new creature(db, isGuestUser, MainFrame));
        }

        private void BiomesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new biome(db, isGuestUser, MainFrame));
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            // In the EventsButton_Click method
            MainFrame.Navigate(new _event(db, isGuestUser, MainFrame));
        }

        private void GameModesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new mode(db, isGuestUser, MainFrame));
        }

        private void MechanicsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new mechanic(db, isGuestUser, MainFrame));
        }

        private void CharacterClassesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new _class(db, isGuestUser, MainFrame));
        }

        private void AchievementsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new achievement(db, isGuestUser, MainFrame));
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
