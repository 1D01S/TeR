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
    public partial class _class : Page
    {
        private readonly TEntities db;

        public _class(TEntities db)
        {
            InitializeComponent();

            this.db = db;

            // Получение данных из таблицы обновлений и отображение их в DataGrid
            var _class = db.Классы_персонажей.ToList();
            dataGridUpdates.ItemsSource = _class;
        }
    }
}
