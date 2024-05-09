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
    public partial class biome : Page
    {
        private List<Биомы> originalBiomes;
        private List<Биомы> biomes;
        private readonly TEntities db;
        private Frame MainFrame;
        private bool isGuestUser;

        public biome(TEntities entities, bool isGuestUser, Frame mainFrame)
        {
            InitializeComponent();

            db = entities;
            MainFrame = mainFrame;
            this.isGuestUser = isGuestUser;
            UpdateUIBasedOnUserRole(isGuestUser, this);

            originalBiomes = db.Биомы.ToList();
            biomes = new List<Биомы>(originalBiomes);
            dataGrid.ItemsSource = biomes;
        }
        public void UpdateUIBasedOnUserRole(bool isGuestUser, biome biomePage)
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
                Биомы selectedBiome = dataGrid.SelectedItem as Биомы;
                biomes.Remove(selectedBiome);
                db.Биомы.Remove(selectedBiome);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Биомы newBiome = new Биомы();
            biomes.Add(newBiome);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newBiome);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Биомы p = e.Row.Item as Биомы;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                biomes.RemoveAt(numRow);
                biomes.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Biome
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool isExplored { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in biomes)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Биомы.Add(item);
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