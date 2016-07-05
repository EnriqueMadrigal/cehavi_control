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
using System.Collections;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for cita.xaml
    /// </summary>
    public partial class cita : Window
    {

        private Int32 curCita = 0;
        private Int32 curPaciente = 0;
        private string CitaFecha = "";

        public int CurCita
        {
            get
            {
                return curCita;
            }

            set
            {
                curCita = value;
            }
        }

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

        public cita()
        {
            InitializeComponent();
        }



        private void CargaDatos()
        {
            string[] Dias = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            datos1.CargaComboBoxData(this.comboBoxTerapeutas, "select Id,Nombre from terapeutas order by Nombre");


            if (this.curCita == 0)
            {
                this.textBox.Text = "45";
                this.comboBoxTerapeutas.SelectedValue = 1;
                this.comentarios.Text = "";
            }


            else
            {

                DataTable datosCita = datos1.LoadData("select Fecha, Duracion, IdTerapeuta  from Citas where Id=" + this.curCita.ToString());

                Int16 Duracion = (Int16)datosCita.Rows[0]["Duracion"];
                Int16 IdTerapueta = (Int16)datosCita.Rows[0]["IdTerapeuta"];
                DateTime FechaCita = (DateTime)datosCita.Rows[0]["Fecha"];
                string comentarios = (string)datosCita.Rows[0]["comentarios"];

                this.comboBoxTerapeutas.SelectedValue = IdTerapueta;
                this.textBox.Text = Duracion.ToString();
                this.comentarios.Text = comentarios;
                this.Fecha.Text = FechaCita.ToShortDateString();
                this.Hora.Text = FechaCita.ToShortTimeString();
                //this.Fecha.Text = curFecha.ToString("yyyy-MM-dd HH:mm:ss");
            }
                
            }


        private void GetFecha_Click(object sender, RoutedEventArgs e)
        {
            string[] Dias = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };
            Int32 CurTerapeuta = (Int32)this.comboBoxTerapeutas.SelectedValue;

            Calendar1 dlg1 = new Calendar1(CurTerapeuta);


            // dlg1.NombrePaciente = this.NombrePaciente;
            dlg1.Duracion = System.Convert.ToInt32(this.textBox.Text);
            //dlg1.CurTerapeuta = (Int32)this.comboBoxTerapeutas.SelectedValue;
            dlg1.ShowDialog();
            this.CitaFecha = dlg1.CurValue;

            if (this.CitaFecha == null)
            {
                MessageBox.Show("No introdujo una fecha valida:", "Advertencia");
                return;
            }

            if (this.CitaFecha.Length == 0)
            {
                MessageBox.Show("No introdujo una fecha valida:", "Advertencia");
                return;

            }
            DateTime curFecha = System.Convert.ToDateTime(this.CitaFecha);
            this.Fecha.Text = curFecha.ToShortDateString();
            this.Hora.Text = curFecha.ToShortTimeString();

        }



        private void save_Click(object sender, RoutedEventArgs e)
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            ArrayList valores = new ArrayList();

            if (this.CitaFecha == null)
            {
                MessageBox.Show("Selecciona una fecha", "Advertencia");
                return;
            }


            if (this.CitaFecha.Length == 0)
            {
                MessageBox.Show("Selecciona una fecha", "Advertencia");
                return;
            }

            DateTime curFecha = System.Convert.ToDateTime(this.CitaFecha);

             Int32 Duracion = System.Convert.ToInt32(this.textBox.Text);
            Int32 Terapeuta = System.Convert.ToInt32(this.comboBoxTerapeutas.SelectedValue);
            
            
            

            //  Int32 Minuto = System.Convert.ToInt32(this.comboBoxMinutos.SelectedValue);

            


            valores.Add(new Registro("Duracion", Duracion));
            valores.Add(new Registro("IdPaciente", this.curPaciente));
            valores.Add(new Registro("IdTerapeuta", Terapeuta));
            valores.Add(new Registro("comentarios", this.comentarios.Text));
            valores.Add(new Registro("Fecha", curFecha.ToString("yyyy-MM-dd HH:mm:ss")));
            



            if (this.curCita != 0) datos1.UpdateData(valores, this.curCita, "Id", "Citas");
            else this.curCita = datos1.InsertData(valores, "Citas");


            if (this.curCita != 0)
            {
                datos1.executeQuery("delete from eventos where IdTipo=2 and IdEvento=" + this.curCita.ToString());
                datos1.CreateCitaEvent(this.CurCita);
                
            }

            else
            {
                this.curCita = datos1.InsertData(valores, "Citas");

            }



            this.Close();


        }


        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filteredText = cehavi_control.TextFilters.GetNumber(cehavi_control.TextFilters.intPattern, this.textBox.Text);
            cehavi_control.TextFilters.SetControlText((TextBox)sender, filteredText);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatos();
        }
    }



   
}
