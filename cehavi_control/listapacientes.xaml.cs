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
using System.Windows.Shapes;
using System.Data;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for listapacientes.xaml
    /// </summary>
    public partial class listapacientes : Window
    {

        public int curPaciente = 0;

        public listapacientes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.curPaciente = 0;
            this.Close();

        }

        private void Window_Initialized(object sender, EventArgs e)
        {


            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();
            this.dataGrid.ItemsSource = datos1.LoadData("Select IdPaciente,Nombre from pacientes order by Nombre").DefaultView;



        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;
            string ID = (dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            //MessageBox.Show(ID);

             ID = ((DataRowView)dataGrid.SelectedItem).Row[0].ToString();
            this.curPaciente = (int)((DataRowView)dataGrid.SelectedItem).Row[0];
            //MessageBox.Show(ID);
            this.Close();

        }
    }
}
