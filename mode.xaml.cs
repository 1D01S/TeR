using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class mode : Page
    {
        private List<Режимы_игры> originalModes;
        private List<Режимы_игры> gameModes;
        private readonly TEntities db;
        private Frame MainFrame;
        private bool isGuestUser;

        public mode(TEntities entities, bool isGuestUser, Frame mainFrame)
        {
            InitializeComponent();

            db = entities;
            MainFrame = mainFrame;
            this.isGuestUser = isGuestUser;
            UpdateUIBasedOnUserRole(isGuestUser, this);

            originalModes = db.Режимы_игры.ToList();
            gameModes = new List<Режимы_игры>(originalModes);
            dataGrid.ItemsSource = gameModes;
        }
        public void UpdateUIBasedOnUserRole(bool isGuestUser, mode modePage)
        {
            if (isGuestUser)
            {
                DeleteRowButton.IsEnabled = false;
                AddNewRowButton.IsEnabled = false;
                SaveChangesButton.IsEnabled = false;
            }
            else
            {
                DeleteRowButton.IsEnabled = true;
                AddNewRowButton.IsEnabled = true;
                SaveChangesButton.IsEnabled = true;
            }
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Режимы_игры selectedMode = dataGrid.SelectedItem as Режимы_игры;
                gameModes.Remove(selectedMode);
                db.Режимы_игры.Remove(selectedMode);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Режимы_игры newGameMode = new Режимы_игры();
            gameModes.Add(newGameMode);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newGameMode);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Режимы_игры p = e.Row.Item as Режимы_игры;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                gameModes.RemoveAt(numRow);
                gameModes.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class GameMode
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool isFeatured { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in gameModes)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Режимы_игры.Add(item);
                    }
                    else
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Изменения сохранены успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
            }
        }
    }
}