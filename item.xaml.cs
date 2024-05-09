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
    public partial class item : Page
    {
        private List<Предметы> originalItems;
        private List<Предметы> items;
        private readonly TEntities db;
        private Frame MainFrame;
        private bool isGuestUser;

        public item(TEntities entities, bool isGuestUser, Frame mainFrame)
        {
            InitializeComponent();

            db = entities;
            MainFrame = mainFrame;
            this.isGuestUser = isGuestUser;
            UpdateUIBasedOnUserRole(isGuestUser, this);

            originalItems = db.Предметы.ToList();
            items = new List<Предметы>(originalItems);
            dataGrid.ItemsSource = items;
        }
        public void UpdateUIBasedOnUserRole(bool isGuestUser, item itemPage)
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
                Предметы selectedItem = dataGrid.SelectedItem as Предметы;
                items.Remove(selectedItem);
                db.Предметы.Remove(selectedItem);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Предметы newItem = new Предметы();
            items.Add(newItem);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newItem);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Предметы p = e.Row.Item as Предметы;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                items.RemoveAt(numRow);
                items.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Item
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public double price { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in items)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Предметы.Add(item);
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