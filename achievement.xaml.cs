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
    public partial class achievement : Page
    {
        private List<Отчивки> achievements;
        private readonly TEntities db;
        private bool isEditMode = false;

        public achievement(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            achievements = db.Отчивки.ToList();
            dataGrid.ItemsSource = achievements;
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Отчивки p = e.Row.Item as Отчивки;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                achievements.RemoveAt(numRow);
                achievements.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Achivement
        {
            public int achievement_id { get; set; }
            public string название { get; set; }
            public string условия_выполнения { get; set; }
            public int update_id { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in achievements)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Отчивки.Add(item);
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
       private void ToggleEditMode_Click(object sender, RoutedEventArgs e)
        {
            isEditMode = !isEditMode; // Переключаем режим

            if (isEditMode)
            {
                dataGrid.IsReadOnly = false; // Включаем режим редактирования
            }
            else
            {
                dataGrid.IsReadOnly = true; // Выключаем режим редактирования
            }
        }
    }
}