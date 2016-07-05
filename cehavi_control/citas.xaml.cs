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
    /// Interaction logic for citas.xaml
    /// </summary>
    public partial class citas : Window
    {
        private DataTable DatosCitas;
        private int curPaciente = 0;

        public int CurPaciente
        {
            get
            {
                return curPaciente;
            }

            set
            {
                curPaciente = value;
            }
        }

        public citas()
        {
            InitializeComponent();
        }


        private void CargaCitas()
        {

            //return;
            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            string[] Dias = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };


            DataTable CitasTemp = datos1.LoadData("select Id, Fecha,  Duracion, IdTerapeuta  from Citas where IdPaciente=" + this.CurPaciente);


            if (CitasTemp == null) return;
            if (CitasTemp.Rows.Count == 0) return;

            this.DatosCitas = new DataTable("Citas");
            this.DatosCitas.Columns.Add("IdCita", Type.GetType("System.Int32"));
            this.DatosCitas.Columns.Add("Dia", Type.GetType("System.String"));
            this.DatosCitas.Columns.Add("Hora", Type.GetType("System.String"));
            this.DatosCitas.Columns.Add("Duracion", Type.GetType("System.Int16"));
            this.DatosCitas.Columns.Add("Terapeuta", Type.GetType("System.String"));
            

            foreach (DataRow c in CitasTemp.Rows)
            {

                string tipoA = c["IdTerapeuta"].GetType().ToString();
                Int32 IdTerapia = (Int32)c["Id"];
                Int16 IdTerapueta = (Int16)c["IdTerapeuta"];
                DateTime FechaCita = (DateTime)c["Fecha"];
               Int16 Duracion = (Int16)c["Duracion"];
            



                string NombreTerapeuta = datos1.GetNombreTabla(IdTerapueta, "Terapeutas", "Id", "Nombre");
           


                //  string horario =  string.Format("{0:D2}", Hora) + ":" + string.Format("{0:D2}", Minuto);


                this.DatosCitas.Rows.Add(IdTerapia, FechaCita.ToShortDateString(), FechaCita.ToShortTimeString(), Duracion.ToString(), NombreTerapeuta);


            }

            this.dataGrid.ItemsSource = this.DatosCitas.DefaultView;

        }


        private void button10_Click(object sender, RoutedEventArgs e)
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

         
            cita dlg1 = new cita();
            dlg1.CurPaciente = this.curPaciente;
            //dlg1.NombrePaciente = this.NombrePaciente.Text;
            dlg1.ShowDialog();

            MessageBox.Show("Operación realizada", "Información");
            CargaCitas();

        }


        private void button11_Click(object sender, RoutedEventArgs e)
        {

            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;

            object curType = ((DataRowView)dataGrid.SelectedItem).Row[0];
            // string curObject = curType.GetType().ToString();

            Int32 curId = (Int32)((DataRowView)dataGrid.SelectedItem).Row["IdCita"];
            cita dlg1 = new cita();
            dlg1.CurPaciente = this.curPaciente;
            dlg1.CurCita = curId;
            dlg1.ShowDialog();

            MessageBox.Show("Operación realizada", "Información");
            CargaCitas();



        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {


            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;
            string ID = (dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            //MessageBox.Show(ID);



            MessageBoxResult result = MessageBox.Show("Esta seguro que desea elimiar esta Cita", "Advertencia", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.OK)
            {

                object curType = ((DataRowView)dataGrid.SelectedItem).Row[0];
                // string curObject = curType.GetType().ToString();

                Int32 curId = (Int32)((DataRowView)dataGrid.SelectedItem).Row["IdCita"];

                DatosCehavi datos1 = new DatosCehavi();
                datos1.Connect();

                datos1.executeQuery("delete from Citas where Id=" + curId.ToString());
                datos1.executeQuery("delete from eventos where IdTipo=2 and IdEvento=" + curId.ToString());

                MessageBox.Show("Se elimino el registro", "Información");

                CargaCitas();
            }



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaCitas();
        }




        ///////////
    }
}
