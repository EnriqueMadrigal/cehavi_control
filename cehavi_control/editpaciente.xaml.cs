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


            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();
            this.DatosPaciente = datos1.LoadData("select Nombre,Comentarios,Sexo,IdEscuela,IdGradoEscuela, Datepart('yyyy',FechaNac),DatePart('m',FechaNac),Datepart('d',FechaNac) from pacientes where IdPaciente=" + this.curPaciente.ToString());
            ComboGrados = datos1.LoadData("select * from Grados");
            this.comboBox1.ItemsSource = ComboGrados.DefaultView;
            this.comboBox1.DisplayMemberPath = ComboGrados.Columns[1].ToString();
            this.comboBox1.SelectedValuePath = ComboGrados.Columns[0].ToString();
            this.comboBox1.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["IdGradoEscuela"].ToString()); 
            // ComboBoxZone.SelectedValue.ToString());

            ComboEscuelas = datos1.LoadData("select * from Escuelas");
            this.comboBox2.ItemsSource = ComboEscuelas.DefaultView;
            this.comboBox2.DisplayMemberPath = ComboEscuelas.Columns[1].ToString();
            this.comboBox2.SelectedValuePath = ComboEscuelas.Columns[0].ToString();
            this.comboBox2.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["IdEscuela"].ToString());




            String NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();
            
            Int32 AnoNac = Convert.ToInt32(DatosPaciente.Rows[0][5].ToString());
            Int32 MesNac = Convert.ToInt32(DatosPaciente.Rows[0][6].ToString());
            Int32 DiaNac = Convert.ToInt32(DatosPaciente.Rows[0][7].ToString());
            Int32 Sexo =  Convert.ToInt32(DatosPaciente.Rows[0]["Sexo"].ToString());

            this.FechaNac_DatePicker.SelectedDate = new DateTime(AnoNac, MesNac, DiaNac);

            this.NombrePaciente.Text = NombrePaciente;

            if (Sexo==1)
            {
                this.radioButton1.IsChecked = true;

                this.radioButton2.IsChecked = false;
            }
            
            else
            {
                this.radioButton2.IsChecked = true;
                this.radioButton2.IsChecked = false;
            } 

        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            
        }
    }
}
