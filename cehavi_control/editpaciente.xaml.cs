using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for editpaciente.xaml
    /// </summary>
    public partial class editpaciente : Window
    {

        private int curPaciente = 0;
        private DataTable DatosPaciente;
        private DataTable ComboEscuelas;
        private DataTable ComboGrados;
        private DataTable ComboEstado;
        private DataTable ComboEstadosRepublica;


        public editpaciente()
        {
            InitializeComponent();
        }




        public void SetPaciente(int newPaciente)
        {
            this.curPaciente = newPaciente;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatos();

        }

        private void CargaDatos()
        {

            ////////

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();


            ComboGrados = datos1.LoadData("select * from Grados");
            this.comboBox1.ItemsSource = ComboGrados.DefaultView;
            this.comboBox1.DisplayMemberPath = ComboGrados.Columns[1].ToString();
            this.comboBox1.SelectedValuePath = ComboGrados.Columns[0].ToString();

            ComboEscuelas = datos1.LoadData("select * from Escuelas");
            this.comboBox2.ItemsSource = ComboEscuelas.DefaultView;
            this.comboBox2.DisplayMemberPath = ComboEscuelas.Columns[1].ToString();
            this.comboBox2.SelectedValuePath = ComboEscuelas.Columns[0].ToString();

            ComboEstado = datos1.LoadData("select * from estado_paciente");
            this.comboBox3.ItemsSource = ComboEstado.DefaultView;
            this.comboBox3.DisplayMemberPath = ComboEstado.Columns[1].ToString();
            this.comboBox3.SelectedValuePath = ComboEstado.Columns[0].ToString();

            ComboEstadosRepublica = datos1.LoadData("select * from Estados");
            this.comboBox4.ItemsSource = ComboEstadosRepublica.DefaultView;
            this.comboBox4.DisplayMemberPath = ComboEstadosRepublica.Columns[1].ToString();
            this.comboBox4.SelectedValuePath = ComboEstadosRepublica.Columns[0].ToString();




            if (this.curPaciente != 0)
            {
                this.DatosPaciente = datos1.LoadData("select Nombre,Comentarios,Sexo,IdEscuela,IdGradoEscuela, Datepart('yyyy',FechaNac),DatePart('m',FechaNac),Datepart('d',FechaNac),estatus from pacientes where IdPaciente=" + this.curPaciente.ToString());
                this.comboBox1.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["IdGradoEscuela"].ToString());
                // ComboBoxZone.SelectedValue.ToString());

                this.comboBox2.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["IdEscuela"].ToString());
                this.comboBox3.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["estatus"].ToString());

                String NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();

                Int32 AnoNac = Convert.ToInt32(DatosPaciente.Rows[0][5].ToString());
                Int32 MesNac = Convert.ToInt32(DatosPaciente.Rows[0][6].ToString());
                Int32 DiaNac = Convert.ToInt32(DatosPaciente.Rows[0][7].ToString());
                Int32 Sexo = Convert.ToInt32(DatosPaciente.Rows[0]["Sexo"].ToString());

                this.FechaNac_DatePicker.SelectedDate = new DateTime(AnoNac, MesNac, DiaNac);

                this.NombrePaciente.Text = NombrePaciente;

                if (Sexo == 1)
                {
                    this.radioButton1.IsChecked = true;
                    this.radioButton2.IsChecked = false;
                }

                else
                {
                    this.radioButton2.IsChecked = true;
                    this.radioButton1.IsChecked = false;
                }

            }

            else
            {
                this.comboBox1.SelectedValue = 1;
                this.comboBox2.SelectedValue = 1;
                this.comboBox3.SelectedValue = 1;
                this.comboBox4.SelectedValue = 14;
                this.FechaNac_DatePicker.SelectedDate = DateTime.Today;
                this.NombrePaciente.Text = "";
                this.radioButton1.IsChecked = true;
                this.radioButton2.IsChecked = false;



            }


            /////////
        }


        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

        }



        private void cancelaCambios(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       

        private void borraPaciente(object sender, RoutedEventArgs e)
        {
            this.DatosPaciente.Clear();
            this.comboBox2.SelectedValue = 1;
            this.FechaNac_DatePicker.SelectedDate = DateTime.Today;
            this.NombrePaciente.Text = "";
            this.radioButton1.IsChecked = true;
            this.radioButton2.IsChecked = false;
            this.curPaciente = 0;

        }


        private void cargaPaciente(object sender, RoutedEventArgs e)
        {
            listapacientes dlg1 = new listapacientes();
            dlg1.ShowDialog();
            int newPaciente = dlg1.curPaciente;

            if (newPaciente != 0)   CargaDatos();
            
        }

        private void guardaPaciente(object sender, RoutedEventArgs e)
        {

            ArrayList valores = new ArrayList();
            int Sexo = 1;
            if (this.radioButton2.IsChecked.Value) Sexo = 2;

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();


            valores.Add(new Registro("Nombre", this.NombrePaciente.Text));
            valores.Add(new Registro("FechaNac", this.FechaNac_DatePicker.SelectedDate));
            valores.Add(new Registro("Sexo", Sexo));
            valores.Add(new Registro("IdGradoEscuela", this.comboBox1.SelectedValue));
            valores.Add(new Registro("IdEscuela", this.comboBox2.SelectedValue));
            valores.Add(new Registro("terapias", 1));
            valores.Add(new Registro("estatus", this.comboBox3.SelectedValue));
            valores.Add(new Registro("calle", this.Calle.Text));
            valores.Add(new Registro("exterior", this.exterior.Text));
            valores.Add(new Registro("interior", this.interior.Text));
            valores.Add(new Registro("telefonocasa", this.telefonocasa.Text));
            valores.Add(new Registro("telefonomovil", this.telefonomovil.Text));
            valores.Add(new Registro("telefonorecados", this.telefonorecados.Text));

            Int32 CurCiudad = datos1.BuscaNombreTabla(this.Ciudad.Text, "Ciudad", "Id", "Nombre");
            if (CurCiudad == 0) CurCiudad = datos1.InsertaNombreCampo(this.Ciudad.Text, "Ciudad", "Nombre");

            Int32 CurMunicipio = datos1.BuscaNombreTabla(this.Municipio.Text, "Municipio", "Id", "Nombre");
            if (CurMunicipio == 0) CurMunicipio = datos1.InsertaNombreCampo(this.Ciudad.Text, "Municipio", "Nombre");

            Int32 CurColonia = datos1.BuscaNombreTabla(this.Colonia.Text, "Colonia", "Id", "Nombre");
            if (CurColonia == 0) CurColonia = datos1.InsertaNombreCampo(this.Colonia.Text, "Colonia", "Nombre");
            

            valores.Add(new Registro("idciudad", CurCiudad));
            valores.Add(new Registro("idmunicipio", CurMunicipio));
            valores.Add(new Registro("idcoloia", CurColonia));



            if (this.curPaciente != 0) datos1.UpdateData(valores, this.curPaciente, "IdPaciente", "pacientes");
            else datos1.InsertData(valores,"pacientes");


            this.Close();


        }


    }
}


