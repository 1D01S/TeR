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

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = "a";
            string password = "p";

            if (usernameTextBox.Text == username && passwordBox.Password == password)
            {
                resultTextBlock.Text = "Вход выполнен успешно! 🎉";

                // Закрываем текущее окно
                

                // Открываем окно MENU
                MENU menu = new MENU();
                Dispatcher.Invoke(() => MainFrame.Navigate(menu));

                // Навигируем к первой странице в MENU
                menu.Dispatcher.Invoke(() => menu.MainFrame.Navigate(new update(new TEntities())));
            }
            else
            {
                resultTextBlock.Text = "Ошибка входа. Пожалуйста, проверьте имя пользователя и пароль. 😕";
            }

        }
    }
}