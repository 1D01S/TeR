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
    public partial class mechanic : Page
    {
        private List<Механики> originalMechanics;
        private List<Механики> mechanics;
        private readonly TEntities db;
        private Frame MainFrame;
        private bool isGuestUser;

        public mechanic(TEntities entities, bool isGuestUser, Frame mainFrame)
        {
            InitializeComponent();

            db = entities;
            MainFrame = mainFrame;
            this.isGuestUser = isGuestUser;
            UpdateUIBasedOnUserRole(isGuestUser, this);

            originalMechanics = db.Механики.ToList();
            mechanics = new List<Механики>(originalMechanics);
            dataGrid.ItemsSource = mechanics;
        }
        public void UpdateUIBasedOnUserRole(bool isGuestUser, mechanic mechanicPage)
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
                Механики selectedMechanic = dataGrid.SelectedItem as Механики;
                mechanics.Remove(selectedMechanic);
                db.Механики.Remove(selectedMechanic);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            Механики newMechanic = new Механики();
            mechanics.Add(newMechanic);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newMechanic);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Механики p = e.Row.Item as Механики;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                mechanics.RemoveAt(numRow);
                mechanics.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Mechanic
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string type { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in mechanics)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.Механики.Add(item);
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