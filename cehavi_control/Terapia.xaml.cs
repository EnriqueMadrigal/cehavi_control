﻿using System;
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


        public void SetCurPaciente (Int32 newPaciente)
        {
            this.curPaciente = newPaciente;
        }

        private void CargaDatos()
        {
           
           

      

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            DataTable DatosTerapuetas = datos1.LoadData("select Id, Nombre from terapeutas order by Nombre");

            this.comboBoxTerapeutas.ItemsSource = DatosTerapuetas.DefaultView;
            this.comboBoxTerapeutas.DisplayMemberPath = DatosTerapuetas.Columns["Nombre"].ToString();
            this.comboBoxTerapeutas.SelectedValuePath = DatosTerapuetas.Columns["Id"].ToString();


            datos1.CargaComboBoxData(this.Repeticion, "select Id,Nombre from repeticion");
            

            if (this.curTerapia == 0)
            {
                this.textBox.Text = "40";
                this.Repeticion.SelectedValue = 1;

                this.comboBoxTerapeutas.SelectedValue = 1;
            }


            else
            {

                DataTable datosTerapia = datos1.LoadData("select dia, duracion, IdTerapeuta, Hora, Minuto from terapias where Id=" + this.curTerapia.ToString());

               // Int16 Dia = (Int16)datosTerapia.Rows[0]["Dia"];
                Int16 Duracion = (Int16)datosTerapia.Rows[0]["Duracion"];
                Int16 IdTerapueta = (Int16)datosTerapia.Rows[0]["IdTerapeuta"];
               // Byte Hora = (Byte)datosTerapia.Rows[0]["Hora"];
               // Byte Minuto = (Byte)datosTerapia.Rows[0]["Minuto"];

              
                this.comboBoxTerapeutas.SelectedValue = IdTerapueta;
                this.textBox.Text = Duracion.ToString();
                this.Repeticion.SelectedValue = 1;

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

          //  Int32 Dia = System.Convert.ToInt32(this.comboBoxDias.SelectedValue);
          //  Int32 Hora = System.Convert.ToInt32(this.comboBoxHoras.SelectedValue);
            Int32 Duracion = System.Convert.ToInt32(this.textBox.Text);
            Int32 Terapeuta = System.Convert.ToInt32(this.comboBoxTerapeutas.SelectedValue);
          //  Int32 Minuto = System.Convert.ToInt32(this.comboBoxMinutos.SelectedValue);

            valores.Add(new Registro("Duracion", Duracion));
            valores.Add(new Registro("IdPaciente", this.curPaciente));
            valores.Add(new Registro("IdTerapeuta", Terapeuta));

            if (this.curTerapia != 0) datos1.UpdateData(valores, this.curTerapia, "Id", "terapias");
            else datos1.InsertData(valores, "terapias");

            this.Close();

            
        }

        private void GetFecha_Click(object sender, RoutedEventArgs e)
        {
            Calendar1 dlg1 = new Calendar1();
            dlg1.NombrePaciente = this.NombrePaciente;
            dlg1.Duracion = System.Convert.ToInt32(this.textBox.Text);
            dlg1.ShowDialog();
            this.Fecha.Text = dlg1.CurValue;


        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filteredText = cehavi_control.TextFilters.GetNumber(cehavi_control.TextFilters.intPattern, this.textBox.Text);
            cehavi_control.TextFilters.SetControlText((TextBox)sender, filteredText);

        }
    }
}
