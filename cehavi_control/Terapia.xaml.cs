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

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for Terapia.xaml
    /// </summary>
    public partial class Terapia : Window
    {

        private Int32 curTerapia = 0;
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

        private void CargaDatos()
        {
            DataTable DatosDia = new DataTable("Dias");

            DatosDia.Columns.Add("IdDia", Type.GetType("System.Int32"));
            DatosDia.Columns.Add("Dia", Type.GetType("System.String"));

            DatosDia.Rows.Add(1, "Lunes");
            DatosDia.Rows.Add(2, "Martes");
            DatosDia.Rows.Add(3, "Miercoles");
            DatosDia.Rows.Add(4, "Jueves");
            DatosDia.Rows.Add(5, "Viernes");
            DatosDia.Rows.Add(6, "Sabado");
            DatosDia.Rows.Add(7, "Domingo");

            this.comboBoxDias.ItemsSource = DatosDia.DefaultView;
            this.comboBoxDias.DisplayMemberPath = DatosDia.Columns["Dia"].ToString();
            this.comboBoxDias.SelectedValuePath = DatosDia.Columns["IdDia"].ToString();

            this.comboBoxDias.SelectedValue = 1;

            DataTable DatosHora = new DataTable("Horas");

            DatosHora.Columns.Add("IdHora", Type.GetType("System.Int32"));
            DatosHora.Columns.Add("Hora", Type.GetType("System.String"));

            DatosHora.Rows.Add(6, "06");
            DatosHora.Rows.Add(7, "07");
            DatosHora.Rows.Add(8, "08");
            DatosHora.Rows.Add(9, "09");
            DatosHora.Rows.Add(10, "10");
            DatosHora.Rows.Add(11, "11");
            DatosHora.Rows.Add(12, "12");
            DatosHora.Rows.Add(13, "13");
            DatosHora.Rows.Add(14, "14");
            DatosHora.Rows.Add(15, "15");
            DatosHora.Rows.Add(16, "16");
            DatosHora.Rows.Add(17, "17");
            DatosHora.Rows.Add(18, "18");
            DatosHora.Rows.Add(19, "19");
            DatosHora.Rows.Add(20, "20");
            DatosHora.Rows.Add(21, "21");
            DatosHora.Rows.Add(22, "22");



            this.comboBoxHoras.ItemsSource = DatosHora.DefaultView;
            this.comboBoxHoras.DisplayMemberPath = DatosHora.Columns["Hora"].ToString();
            this.comboBoxHoras.SelectedValuePath = DatosHora.Columns["IdHora"].ToString();

            this.comboBoxHoras.SelectedValue = 9;

            DataTable DatosMinutos = new DataTable("Minutos");

            DatosMinutos.Columns.Add("IdMinutos", Type.GetType("System.Int32"));
            DatosMinutos.Columns.Add("Minutos", Type.GetType("System.String"));

            DatosMinutos.Rows.Add(0, "00");
            DatosMinutos.Rows.Add(5, "05");
            DatosMinutos.Rows.Add(10, "10");
            DatosMinutos.Rows.Add(15, "15");
            DatosMinutos.Rows.Add(20, "20");
            DatosMinutos.Rows.Add(25, "25");
            DatosMinutos.Rows.Add(30, "30");
            DatosMinutos.Rows.Add(35, "35");
            DatosMinutos.Rows.Add(40, "40");
            DatosMinutos.Rows.Add(45, "45");
            DatosMinutos.Rows.Add(50, "50");
            DatosMinutos.Rows.Add(55, "55");



            this.comboBoxMinutos.ItemsSource = DatosMinutos.DefaultView;
            this.comboBoxMinutos.DisplayMemberPath = DatosMinutos.Columns["Minutos"].ToString();
            this.comboBoxMinutos.SelectedValuePath = DatosMinutos.Columns["IdMinutos"].ToString();

            this.comboBoxMinutos.SelectedValue = 0;

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            DataTable DatosTerapuetas = datos1.LoadData("select Id, Nombre from terapeutas order by Nombre");

            this.comboBoxTerapeutas.ItemsSource = DatosTerapuetas.DefaultView;
            this.comboBoxTerapeutas.DisplayMemberPath = DatosTerapuetas.Columns["Nombre"].ToString();
            this.comboBoxTerapeutas.SelectedValuePath = DatosTerapuetas.Columns["Id"].ToString();
            this.comboBoxTerapeutas.SelectedValue = 1;




        }


        private void close_Click(object sender, RoutedEventArgs e)
        {

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}