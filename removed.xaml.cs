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
    public partial class removed : Page
    {
        private List<Удалённое> originalRemoved;
        private List<Удалённое> removedItems;
        private readonly TEntities db;

        public removed(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            originalRemoved = db.Удалённое.ToList();
            removedItems = new List<Удалённое>(originalRemoved);
            dataGrid.ItemsSource = removedItems;
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Удалённое selectedItem = dataGrid.SelectedItem as Удалённое;
                removedItems.Remove(selectedItem);
                db.Удалённое.Remove(selectedItem);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Удалённое newRemovedItem = new Удалённое();
            removedItems.Add(newRemovedItem);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newRemovedItem);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Удалённое p = e.Row.Item as Удалённое;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                removedItems.RemoveAt(numRow);
                removedItems.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class RemovedItem
        {
            public int id { get; set; }
            public string description { get; set; }
            public DateTime date { get; set; }
            public bool isPermanent { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in removedItems)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Удалённое.Add(item);
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