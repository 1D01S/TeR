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
    public partial class plant : Page
    {
        private List<Растения> originalPlants;
        private List<Растения> plants;
        private readonly TEntities db;
        private Frame MainFrame;
        private bool isGuestUser;

        public plant(TEntities entities, bool isGuestUser, Frame mainFrame)
        {
            InitializeComponent();

            db = entities;
            MainFrame = mainFrame;
            this.isGuestUser = isGuestUser;
            UpdateUIBasedOnUserRole(isGuestUser, this);
            
            originalPlants = db.Растения.ToList();
            plants = new List<Растения>(originalPlants);
            dataGrid.ItemsSource = plants;
        }
        public void UpdateUIBasedOnUserRole(bool isGuestUser, plant plantPage)
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
                Растения selectedPlant = dataGrid.SelectedItem as Растения;
                plants.Remove(selectedPlant);
                db.Растения.Remove(selectedPlant);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Растения newPlant = new Растения();
            plants.Add(newPlant);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newPlant);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Растения p = e.Row.Item as Растения;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                plants.RemoveAt(numRow);
                plants.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Plant
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public DateTime plantingDate { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in plants)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Растения.Add(item);
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