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
    public partial class listageneral : Window
    {

        public int curId = 0;
        private string curTable = "";
        private string IndexName = "";
        private string NameIndex = "";
        public string curValue = "";
        private DataTable curDataTable;

        public listageneral()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.curId = 0;
            this.curValue = "";
            this.Close();

        }

        private void Window_Initialized(object sender, EventArgs e)
        {


        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;
            string ID = (dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            //MessageBox.Show(ID);

            this.curValue = ((DataRowView)dataGrid.SelectedItem).Row[1].ToString();
            this.curId = (int)((DataRowView)dataGrid.SelectedItem).Row[0];


            this.Close();

        }


        private void button0_Click(object sender, RoutedEventArgs e)
        {
            this.curId = 0;
            this.curValue = this.textBox.Text;
            this.Close();

        }

        public void setTable(string newTable)
        {
            this.curTable = newTable;
        }

        public void setNameIndex(string newIndex)
        {
            this.NameIndex = newIndex;
        }

        public void setIndexName(string newIndex)
        {
            this.IndexName = newIndex;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatos();
        }

        private void CargaDatos()
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();
            this.curDataTable = datos1.LoadData("Select " + this.IndexName + "," + NameIndex + " from " + curTable + " order by " + NameIndex);

            this.dataGrid.ItemsSource = this.curDataTable.DefaultView;

            //this.dataGrid.ItemsSource = datos1.LoadData("Select " + this.IndexName + "," + NameIndex + " from " + curTable + " order by " + NameIndex).DefaultView;

        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;

            //MessageBox.Show(ID);

            this.curValue = ((DataRowView)dataGrid.SelectedItem).Row[1].ToString();
            this.curId = (int)((DataRowView)dataGrid.SelectedItem).Row[0];


            this.Close();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string curFilter = this.textBox.Text;
            string filter = string.Format(NameIndex + " LIKE '%{0}%'", curFilter);

            DataRow[] FilteredRows = this.curDataTable.Select(filter);
            if (curFilter.Length > 0)
            {

                if (FilteredRows.Length != 0)
                {
                    this.dataGrid.ItemsSource = FilteredRows.CopyToDataTable().DefaultView;
                }
         
                else
                {
                    this.dataGrid.ItemsSource = null;
                }


            }

            else
            {
                this.dataGrid.ItemsSource = this.curDataTable.DefaultView;
            }
            

        }
    }
}
