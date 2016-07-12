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
using System.IO;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for asistencias.xaml
    /// </summary>
    public partial class asistencias : Window
    {
        public asistencias()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DateTime curdate = DateTime.Now;

            this.datepicker1.SelectedDate = curdate;


        }

        private void datepicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           // MessageBox.Show(this.datepicker1.SelectedDate.ToString());
            loadDatos();
        }

        private void loadDatos()
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();


            DateTime CurTime = (DateTime) this.datepicker1.SelectedDate;
            

            string query = "select * from eventos where datepart('m', start_event) =" + CurTime.Month.ToString() + " and datepart('yyyy', start_event)=" + CurTime.Year.ToString() + " and datepart('d', start_event)=" + CurTime.Day.ToString() + " order by start_event";

            //MessageBox.Show(query);

            DataTable EventosTemp = datos1.LoadData(query);
            

            if (EventosTemp == null) 

            {
                this.listView.ItemsSource = null;
                return;
            }

            DataTable DatosAsistencias = new DataTable("Terapias");
            DatosAsistencias.Columns.Add("IdEvento", Type.GetType("System.Int32"));
            DatosAsistencias.Columns.Add("Nombre", Type.GetType("System.String"));
            DatosAsistencias.Columns.Add("Hora", Type.GetType("System.String"));
            DatosAsistencias.Columns.Add("Estado", Type.GetType("System.String"));
            DatosAsistencias.Columns.Add("Tipo", Type.GetType("System.String"));
            DatosAsistencias.Columns.Add("ImagePath", Type.GetType("System.String"));

            foreach (DataRow c in EventosTemp.Rows)
            {

                string tipoA = c["IdTipo"].GetType().ToString();
                Int32 Id = (Int32)c["Id"];
                Int32 IdEvento = (Int32)c["IdEvento"];
                Byte IdTipo = (Byte)c["IdTipo"];
                Byte Estatus1 = (Byte)c["status1"];
                Byte Estatus2 = (Byte)c["status2"];
                DateTime startFecha = (DateTime)c["start_event"];
                Int32 IdPaciente = 0;
                string NombrePaciente;
                string ImagePath = "";
                string TipoEvento = "Terapia";
                

                if (IdTipo==1)
                {
                    DataTable DatosTerapia = datos1.LoadData("select * from Terapias where Id=" + IdEvento.ToString());
                    IdPaciente = (Int32) DatosTerapia.Rows[0]["IdPaciente"];

                    
                }

                if (IdTipo == 2)
                {
                    DataTable DatosCita = datos1.LoadData("select * from Citas where Id=" + IdEvento.ToString());
                    IdPaciente = (Int32)DatosCita.Rows[0]["IdPaciente"];
                    TipoEvento = "Cita";
                }

                DataTable DatosPaciente = datos1.LoadData("select * from pacientes where IdPaciente=" + IdPaciente.ToString());
                NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();
                Int16 Sexo = (Int16) DatosPaciente.Rows[0]["Sexo"];

                string EstadoEvento = datos1.GetNombreTabla(Estatus1, "EstadoEventos1", "Id", "Nombre");

                String photolocation = "C:\\Datos\\images\\" + IdPaciente.ToString() + ".jpg";  //file name 
                if (File.Exists(photolocation)) ImagePath = photolocation;

                else
                {
                    if (Sexo == 1) ImagePath = "C:\\Datos\\res\\child1.jpg";
                    if (Sexo == 2) ImagePath = "C:\\Datos\\res\\child2.jpg";

                }


                DatosAsistencias.Rows.Add(Id, NombrePaciente, startFecha.ToShortTimeString(),EstadoEvento, TipoEvento,ImagePath);
                

                }

            this.listView.ItemsSource = DatosAsistencias.DefaultView;


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click1(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Clicked");

            object item = this.listView.SelectedItem;


        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Clicked");

            object item = this.listView.SelectedItem;
            Int32 curId = 0;
            DataRowView curRow;

            if (item == null) return;
            if (item is System.Data.DataRowView)
            {
                curRow = (DataRowView)item;
                curId = (Int32)curRow.Row["IdEvento"];
            }

            else  return;

            cambiaestado1 dlg1 = new cambiaestado1();
            dlg1.CurEvento = curId;
            dlg1.ShowDialog();

            if (dlg1.Estado_cambio == false) return;
            MessageBox.Show("Cambio realizado", "Advertencia");
            loadDatos();

        }
    }



}
