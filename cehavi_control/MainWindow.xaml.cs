using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using System.Collections;


namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New");
        }

        private void mnuNew_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listaPacientes(object sender, RoutedEventArgs e)
        {
            listapacientes dlg1 = new listapacientes();
            dlg1.ShowDialog(); 

        }
        private void muestraPaciente(object sender, RoutedEventArgs e)
        {
            int curPaciente = 0;

            /*
            listapacientes dlg1 = new listapacientes();
            dlg1.ShowDialog();
            curPaciente = dlg1.curPaciente;
            */

            listageneral dlg1 = new listageneral();
            dlg1.setIndexName("IdPaciente");
            dlg1.setNameIndex("Nombre");
            dlg1.setTable("pacientes");

            dlg1.ShowDialog();
            curPaciente = dlg1.curId;

            




            if (curPaciente == 0) return;

           // MessageBox.Show(curPaciente.ToString(), "Paciente Seleccionado");
            editpaciente dlg2 = new editpaciente();
            dlg2.SetPaciente(curPaciente);
            dlg2.ShowDialog();
            

          
        }

        private void newPaciente(object sender, RoutedEventArgs e)
        {
            editpaciente dlg2 = new editpaciente();
            dlg2.ShowDialog();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void otraOpcion(object sender, RoutedEventArgs e)
        {
            // return;

            /*

                
                     ArrayList valores = new ArrayList();
          
                   string[] estados =
                   {"Aguascalientes",
       "Baja California",
       "Baja California Sur",
       "Campeche",
       "Coahuila",
       "Colima",
       "Chiapas",
       "Chihuahua",
       "Distrito Federal",
       "Durango",
       "Guanajuato",
       "Guerrero",
       "Hidalgo",
       "Jalisco",
       "México",
       "Michoacán",
       "Morelos",
       "Nayarit",
       "Nuevo León",
       "Oaxaca",
       "Puebla",
       "Querétaro",
       "Quintana Roo",
       "San Luis Potosí",
       "Sinaloa",
       "Sonora",
       "Tabasco",
       "Tamaulipas",
       "Tlaxcala",
       "Veracruz",
       "Yucatán",
       "Zacatecas"};

                   DatosCehavi datos1 = new DatosCehavi();
                   datos1.Connect();

                   for(int i=0;i<estados.Length;i++)
                   {
                       valores.Add(new Registro("Id", i+1));
                       valores.Add(new Registro("Nombre", estados[i]));
                       datos1.InsertData(valores, "Estados");
                       valores.Clear();

                   }

  */
            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();
            //datos1.executeQuery("delete from repeticion");
            //datos1.executeQuery("insert into repeticion(Id,Nombre) values(1,'Una sola vez')");
            //datos1.executeQuery("insert into repeticion(Id,Nombre) values(2,'Diario')");

            DateTime CurTime = DateTime.Now;
            

            DateTime FechaHoy = new DateTime(CurTime.Year, CurTime.Month, 1);

            DateTime EndTime =  FechaHoy.AddMonths(1);

            DataTable TempData = datos1.GetEvents();


            string NombrePaciente="";
            Int16 StatusPaciente=0;
            int CurPaciente = 0;


            DataTable DatosEventos = new DataTable("Eventos");
            DatosEventos.Columns.Add("IdEvento", Type.GetType("System.Int32"));
            DatosEventos.Columns.Add("Fecha", Type.GetType("System.DateTime"));
            DatosEventos.Columns.Add("Duracion", Type.GetType("System.Int16"));
            DatosEventos.Columns.Add("Title", Type.GetType("System.String"));


            foreach (DataRow c in TempData.Rows)
            {
                Int32 IdEvento = (Int32)c["Id"];
                Int32 IdPaciente = (Int32)c["IdPaciente"];
                Int16 Duracion = (Int16)c["Duracion"];
                Int16 IdTerapueta = (Int16)c["IdTerapeuta"];
                Int16 Periodo = (Int16)c["Periodo"];
                DateTime curFecha = (DateTime)c["Fecha"];
                DateTime endFecha = (DateTime)c["Fecha2"];


                if (CurPaciente==0 || IdPaciente != CurPaciente)
                {
                    CurPaciente = IdPaciente;
                    DataTable DatosPaciente = datos1.LoadData("select * from pacientes where IdPaciente=" + IdPaciente.ToString());
                    NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();
                    StatusPaciente = (Int16)DatosPaciente.Rows[0]["estatus"];
                }


                if (StatusPaciente == 1 && Periodo !=1)
                {

                    ////// Crear eventos
                    DateTime newDate = curFecha;
                    DateTime FechaEvento = curFecha;
                    while (DateTime.Compare(FechaEvento, endFecha) <=0)
                    {

                        DatosEventos.Rows.Add(IdEvento,FechaEvento,Duracion,NombrePaciente);

                        if (Periodo == 2) FechaEvento = FechaEvento.AddDays(1);
                        if (Periodo == 3) FechaEvento = FechaEvento.AddDays(7);
                        if (Periodo == 4) FechaEvento = FechaEvento.AddMonths(1);

                    }


                    

                }








            }





                //datos1.executeQuery("delete from Colonia where Nombre=''");
                //datos1.executeQuery("delete from Ciudad where Nombre=''");
                //datos1.executeQuery("update pacientes set idestado=14");

            }


    }
}



