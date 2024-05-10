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
    public partial class _event : Page
    {
        private List<События> originalEvents;
        private List<События> events;
        private readonly TEntities db;
        private Frame MainFrame;
        private bool isGuestUser;

        public _event(TEntities entities, bool isGuestUser, Frame mainFrame)
        {
            InitializeComponent();

            db = entities;
            MainFrame = mainFrame;
            this.isGuestUser = isGuestUser;
            UpdateUIBasedOnUserRole(isGuestUser, this);

            originalEvents = db.События.ToList();
            events = new List<События>(originalEvents);
            dataGrid.ItemsSource = events;
        }
        public void UpdateUIBasedOnUserRole(bool isGuestUser, _event eventPage)
        {
            if (isGuestUser)
            {
                eventPage.DeleteRowButton.IsEnabled = false;
                eventPage.AddNewRowButton.IsEnabled = false;
                eventPage.SaveChangesButton.IsEnabled = false;
            }
            else
            {
                eventPage.DeleteRowButton.IsEnabled = true;
                eventPage.AddNewRowButton.IsEnabled = true;
                eventPage.SaveChangesButton.IsEnabled = true;
            }
        }
        
        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                События selectedEvent = dataGrid.SelectedItem as События;
                events.Remove(selectedEvent);
                db.События.Remove(selectedEvent);

                db.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void AddNewRowButton_Click(object sender, RoutedEventArgs e)
        {
            События newEvent = new События();
            events.Add(newEvent);
            dataGrid.Items.Refresh();
            dataGrid.ScrollIntoView(newEvent);
        }

        private bool flagfix = true;

        public void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            События p = e.Row.Item as События;
            if (flagfix)
            {
                int numRow = e.Row.GetIndex();
                Random rnd = new Random();

                events.RemoveAt(numRow);
                events.Insert(numRow, p);

                flagfix = false;
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
                flagfix = true;
                dataGrid.Items.Refresh();
            }
        }

        public class Event
        {
            public int event_id { get; set; }
            public string название { get; set; }
            public string детали { get; set; }
            public string место { get; set; }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in events)
                {
                    if (db.Entry(item).State == EntityState.Detached)
                    {
                        db.События.Add(item);
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