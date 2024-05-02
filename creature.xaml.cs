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
    public partial class creature : Page
    {
        private List<Существа> originalCreatures;
        private List<Существа> creatures;
        private readonly TEntities db;

        public creature(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            originalCreatures = db.Существа.ToList();
            creatures = new List<Существа>(originalCreatures);
            dataGrid.ItemsSource = creatures;
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Существа selectedCreature = dataGrid.SelectedItem as Существа;
                creatures.Remove(selectedCreature);
                db.Существа.Remove(selectedCreature);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Существа newCreature = new Существа();
            creatures.Add(newCreature);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newCreature);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Существа p = e.Row.Item as Существа;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                creatures.RemoveAt(numRow);
                creatures.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Creature
        {
            public int id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int level { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in creatures)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Существа.Add(item);
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