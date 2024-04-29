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
        private List<Отчивки> originalAchievements;
        private List<Отчивки> achievements;
        private readonly TEntities db;

        public achievement(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            originalAchievements = db.Отчивки.ToList();
            achievements = new List<Отчивки>(originalAchievements);
            dataGrid.ItemsSource = achievements;
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Отчивки selectedAchievement = dataGrid.SelectedItem as Отчивки;
                achievements.Remove(selectedAchievement); // Удаляем выбранный объект из коллекции
                db.Отчивки.Remove(selectedAchievement); // Удаляем выбранный объект из базы данных

                db.SaveChanges(); // Сохраняем изменения в базе данных
                dataGrid.Items.Refresh(); // Обновляем DataGrid
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Отчивки newAchievement = new Отчивки(); // Создаем новый объект достижения
            achievements.Add(newAchievement); // Добавляем его в коллекцию
            dataGrid.Items.Refresh(); // Обновляем DataGrid
            dataGrid.ScrollIntoView(newAchievement); // Прокручиваем к новому элементу
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
    }
}