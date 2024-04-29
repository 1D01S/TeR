using System;
using System.Collections.Generic;
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
        private readonly TEntities db;

        public achievement(TEntities db)
        {
            InitializeComponent();

            this.db = db;

            // Получение данных из таблицы обновлений и отображение их в DataGrid
            var achievement = db.Отчивки.ToList();
            dataGridUpdates.ItemsSource = achievement;
        }
    }
}
