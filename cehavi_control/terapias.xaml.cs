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
    /// Interaction logic for terapias.xaml
    /// </summary>
    public partial class terapias : Window
    {

        private DataTable DatosTerapias;
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

        public terapias()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaTerapias();
        }


        private void CargaTerapias()
        {

            //return;
            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            string[] Dias = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };


            DataTable TerapiasTemp = datos1.LoadData("select Id, Fecha, Fecha2,Dia, Hora, Duracion, IdTerapeuta, Periodo from terapias where IdPaciente=" + this.CurPaciente);
            if (TerapiasTemp == null) return;

            this.DatosTerapias = new DataTable("Terapias");
            this.DatosTerapias.Columns.Add("IdTerapia", Type.GetType("System.Int32"));
            this.DatosTerapias.Columns.Add("Inicio", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Dia", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Hora", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Duracion", Type.GetType("System.Int16"));
            this.DatosTerapias.Columns.Add("Periodo", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Terapeuta", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Fin", Type.GetType("System.String"));

            foreach (DataRow c in TerapiasTemp.Rows)
            {

                string tipoA = c["Fecha"].GetType().ToString();
                Int32 IdTerapia = (Int32)c["Id"];
                Int16 IdTerapueta = (Int16)c["IdTerapeuta"];
                DateTime startFecha = (DateTime)c["Fecha"];
                DateTime endFecha = (DateTime)c["Fecha2"];
                DateTime Hora = (DateTime)c["Hora"];

                Int16 Periodo = (Int16)c["Periodo"];
                Int16 Duracion = (Int16)c["Duracion"];
                Byte curDia = (Byte)c["Dia"];



                string NombreTerapeuta = datos1.GetNombreTabla(IdTerapueta, "Terapeutas", "Id", "Nombre");
                string NombrePeriodo = datos1.GetNombreTabla(Periodo, "repeticion", "Id", "Nombre");


                //  string horario =  string.Format("{0:D2}", Hora) + ":" + string.Format("{0:D2}", Minuto);


                this.DatosTerapias.Rows.Add(IdTerapia, startFecha.ToShortDateString(), Dias[curDia], Hora.ToShortTimeString(), Duracion.ToString(), NombrePeriodo, NombreTerapeuta, endFecha.ToShortDateString());


            }

            this.dataGrid.ItemsSource = this.DatosTerapias.DefaultView;

        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            if (this.curPaciente == 0)
            {
                MessageBox.Show("Por favor primero, guarde el paciente y luego proceda a dar de alta las terapias", "Advertencia");
                return;
            }

            Terapia dlg1 = new Terapia();
            dlg1.SetCurPaciente(this.curPaciente);
            //dlg1.NombrePaciente = this.NombrePaciente.Text;
            dlg1.ShowDialog();
          
            MessageBox.Show("Operación realizada", "Información");
            CargaTerapias();

        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {

            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;

            object curType = ((DataRowView)dataGrid.SelectedItem).Row[0];
            // string curObject = curType.GetType().ToString();

            Int32 curId = (Int32)((DataRowView)dataGrid.SelectedItem).Row["IdTerapia"];
            Terapia dlg1 = new Terapia();
            dlg1.SetCurPaciente(this.curPaciente);
            dlg1.SetCurTerapia(curId);
            dlg1.ShowDialog();

            MessageBox.Show("Operación realizada", "Información");
            CargaTerapias();



        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {


            DataGridCellInfo curcell = dataGrid.CurrentCell;

            object item = dataGrid.SelectedItem;

            if (item == null) return;
            string ID = (dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            //MessageBox.Show(ID);



            MessageBoxResult result = MessageBox.Show("Esta seguro que desea elimiar esta terapia", "Advertencia", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.OK)
            {

                object curType = ((DataRowView)dataGrid.SelectedItem).Row[0];
                // string curObject = curType.GetType().ToString();

                Int32 curId = (Int32)((DataRowView)dataGrid.SelectedItem).Row["IdTerapia"];

                DatosCehavi datos1 = new DatosCehavi();
                datos1.Connect();

                datos1.executeQuery("delete from terapias where Id=" + curId.ToString());
                datos1.executeQuery("delete from eventos where IdTipo=1 and IdEvento=" + curId.ToString());

                MessageBox.Show("Se elimino el registro", "Información");

                CargaTerapias();
            }



        }




    }


}

