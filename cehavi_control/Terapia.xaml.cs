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
    /// Interaction logic for Terapia.xaml
    /// </summary>
    public partial class Terapia : Window
    {

        private Int32 curTerapia = 0;
        private Int32 curPaciente = 0;
        private string nombrePaciente = "";
        private string TerapiaFecha = "";

        public string NombrePaciente
        {
            get
            {
                return nombrePaciente;
            }

            set
            {
                nombrePaciente = value;
            }
        }

        public Terapia()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatos();



        }


        public void SetCurTerapia(Int32 newTerapia)
        {
            this.curTerapia = newTerapia;
        }


        public Int32 GetCurTerapia()
        {
            return this.curTerapia;
        }

        public void SetCurPaciente(Int32 newPaciente)
        {
            this.curPaciente = newPaciente;
        }

        private void CargaDatos()
        {
            string[] Dias = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            datos1.CargaComboBoxData(this.comboBoxTerapeutas, "select Id,Nombre from terapeutas order by Nombre");
            /*
            DataTable DatosTerapuetas = datos1.LoadData("select Id, Nombre from terapeutas order by Nombre");
            this.comboBoxTerapeutas.ItemsSource = DatosTerapuetas.DefaultView;
            this.comboBoxTerapeutas.DisplayMemberPath = DatosTerapuetas.Columns["Nombre"].ToString();
            this.comboBoxTerapeutas.SelectedValuePath = DatosTerapuetas.Columns["Id"].ToString();
            */

            datos1.CargaComboBoxData(this.Repeticion, "select Id,Nombre from repeticion");


            if (this.curTerapia == 0)
            {
                this.textBox.Text = "45";
                this.Repeticion.SelectedValue = 1;

                this.comboBoxTerapeutas.SelectedValue = 1;
            }


            else
            {

                DataTable datosTerapia = datos1.LoadData("select Fecha, Duracion, IdTerapeuta, Fecha2, Dia, Hora, Periodo from terapias where Id=" + this.curTerapia.ToString());
                Byte Dia = (Byte)datosTerapia.Rows[0]["Dia"];
                Int16 Duracion = (Int16)datosTerapia.Rows[0]["Duracion"];
                Int16 IdTerapueta = (Int16)datosTerapia.Rows[0]["IdTerapeuta"];
                Int16 Periodo = (Int16)datosTerapia.Rows[0]["Periodo"];
                // Byte Hora = (Byte)datosTerapia.Rows[0]["Hora"];
                // Byte Minuto = (Byte)datosTerapia.Rows[0]["Minuto"];
                DateTime curFecha = (DateTime)datosTerapia.Rows[0]["Fecha"];
                DateTime endFecha = (DateTime)datosTerapia.Rows[0]["Fecha2"];
                DateTime Hora = (DateTime)datosTerapia.Rows[0]["Hora"];





                this.Repeticion.SelectedValue = Periodo;
                this.comboBoxTerapeutas.SelectedValue = IdTerapueta;
                this.textBox.Text = Duracion.ToString();
                //this.Repeticion.SelectedValue = 1;
                this.Fecha.Text = Dias[Dia];
                this.Hora.Text = Hora.ToShortTimeString();
                //this.Fecha.Text = curFecha.ToString("yyyy-MM-dd HH:mm:ss");
                this.datePicker0.SelectedDate = curFecha;
                this.datePicker1.SelectedDate = endFecha;
            }







        }


        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            ArrayList valores = new ArrayList();

            if (this.TerapiaFecha == null)
            {
                MessageBox.Show("Selecciona una fecha", "Advertencia");
                return;
            }

            if (this.TerapiaFecha.Length == 0)
            {
                MessageBox.Show("Selecciona una fecha", "Advertencia");
                return;
            }

            DateTime curFecha = System.Convert.ToDateTime(this.TerapiaFecha);

            //  Int32 Dia = System.Convert.ToInt32(this.comboBoxDias.SelectedValue);
            //  Int32 Hora = System.Convert.ToInt32(this.comboBoxHoras.SelectedValue);
            Int32 Duracion = System.Convert.ToInt32(this.textBox.Text);
            Int32 Terapeuta = System.Convert.ToInt32(this.comboBoxTerapeutas.SelectedValue);
            Int32 Periodo = System.Convert.ToInt32(this.Repeticion.SelectedValue);
            DateTime StartFecha = (DateTime)this.datePicker0.SelectedDate;
            DateTime EndFecha = (DateTime)this.datePicker1.SelectedDate;

            //  Int32 Minuto = System.Convert.ToInt32(this.comboBoxMinutos.SelectedValue);

            int CurDia = (int)curFecha.DayOfWeek;


            valores.Add(new Registro("Duracion", Duracion));
            valores.Add(new Registro("IdPaciente", this.curPaciente));
            valores.Add(new Registro("IdTerapeuta", Terapeuta));
            valores.Add(new Registro("Periodo", Periodo));
            valores.Add(new Registro("Fecha", StartFecha.ToString("yyyy-MM-dd HH:mm:ss")));
            valores.Add(new Registro("Fecha2", EndFecha.ToString("yyyy-MM-dd HH:mm:ss")));
            valores.Add(new Registro("Dia", CurDia));
            valores.Add(new Registro("Hora", curFecha.ToString("2000-01-01 HH:mm:ss")));



            if (this.curTerapia != 0) 
            {
                datos1.UpdateData(valores, this.curTerapia, "Id", "terapias");
                datos1.executeQuery("delete from eventos where IdTipo=1 and IdEvento=" + this.curTerapia.ToString());
                datos1.CreateCurrentEvents(this.curTerapia);
            }

            else 
            {
                this.curTerapia = datos1.InsertData(valores, "terapias");
                
            }

            //MessageBox.Show(curFecha.ToShortDateString(),"Fecha");
            //MessageBox.Show(curFecha.ToShortTimeString(), "Hora");

            
            


            this.Close();


        }

        private void GetFecha_Click(object sender, RoutedEventArgs e)
        {
            string[] Dias = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };

            Int32 CurTerapeuta = (Int32)this.comboBoxTerapeutas.SelectedValue;

            Calendar1 dlg1 = new Calendar1(CurTerapeuta);




            dlg1.NombrePaciente = this.NombrePaciente;
            dlg1.Duracion = System.Convert.ToInt32(this.textBox.Text);
            //dlg1.CurTerapeuta = (Int32)this.comboBoxTerapeutas.SelectedValue;
            dlg1.ShowDialog();
            this.TerapiaFecha = dlg1.CurValue;

            if (this.TerapiaFecha == null) 
            {
                MessageBox.Show("No introdujo una fecha valida:", "Advertencia");
                return;
            }

            if (this.TerapiaFecha.Length==0)
            {
                MessageBox.Show("No introdujo una fecha valida:", "Advertencia");
                return;

            }
            DateTime curFecha = System.Convert.ToDateTime(this.TerapiaFecha);
            int curDia = (int) curFecha.DayOfWeek;

            this.Fecha.Text = Dias[curDia];
            this.Hora.Text = curFecha.ToShortTimeString();

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filteredText = cehavi_control.TextFilters.GetNumber(cehavi_control.TextFilters.intPattern, this.textBox.Text);
            cehavi_control.TextFilters.SetControlText((TextBox)sender, filteredText);

        }
    }
}
