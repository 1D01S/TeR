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
    public partial class _class : Page
    {
        private List<Классы_персонажей> originalClasses;
        private List<Классы_персонажей> classes;
        private readonly TEntities db;

        public _class(TEntities entities)
        {
            InitializeComponent();

            db = entities;
            originalClasses = db.Классы_персонажей.ToList();
            classes = new List<Классы_персонажей>(originalClasses);
            dataGrid.ItemsSource = classes;
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Классы_персонажей selectedClass = dataGrid.SelectedItem as Классы_персонажей;
                classes.Remove(selectedClass);
                db.Классы_персонажей.Remove(selectedClass);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Классы_персонажей newClass = new Классы_персонажей();
            classes.Add(newClass);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newClass);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Классы_персонажей p = e.Row.Item as Классы_персонажей;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                classes.RemoveAt(numRow);
                classes.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class CharacterClass
        {
            public int class_id { get; set; }
            public string название { get; set; }
            public string описание { get; set; }
            public int уровень_сложности { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in classes)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Классы_персонажей.Add(item);
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