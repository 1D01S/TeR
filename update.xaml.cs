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

namespace TeR
{
    public partial class update : Page
    {
        private List<История_Обновлений> updates;
        private readonly TEntities db;

        public update(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            updates = db.История_Обновлений.ToList();
            dataGrid.ItemsSource = updates;
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
