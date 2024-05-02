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
using TeR;
using static TeR.update;

namespace TeR
{
    public partial class update : Page
    {
        private List<История_Обновлений> originalUpdate;
        private List<История_Обновлений> updates;
        private readonly TEntities db;

        public update(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            originalUpdate = db.История_Обновлений.ToList();
            updates = new List<История_Обновлений>(originalUpdate);
            dataGrid.ItemsSource = updates;
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                История_Обновлений selectedAchievement = dataGrid.SelectedItem as История_Обновлений; // Correct variable name
                updates.Remove(selectedAchievement); // Remove the selected item from updates
                db.История_Обновлений.Remove(selectedAchievement); // Remove the selected item from the database

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            История_Обновлений newUpdate = new История_Обновлений(); // Change the type to История_Обновлений
            updates.Add(newUpdate);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newUpdate);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            История_Обновлений p = e.Row.Item as История_Обновлений;

            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();
                

                updates.RemoveAt(numRow);
                updates.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }
        public class Update
        {
            public int update_id { get; set; }
            public string версия { get; set; }
            public DateTime дата { get; set; }
            public string описание { get; set; }
        }
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in updates)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.История_Обновлений.Add(item); // Add new items to the context
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
