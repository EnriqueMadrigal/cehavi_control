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
using System.Collections;
using Microsoft.Win32;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for editterapeuta.xaml
    /// </summary>
    public partial class editterapeuta : Window
    {

        private Int32 curPaciente = 0;
        private DataTable DatosPaciente;
        private DataTable ComboEstado;
        private DataTable ComboEstadosRepublica;
        private BitmapImage personPhoto;
        private bool newLoadedImage = false;

        public editterapeuta()
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
                this.DatosPaciente = datos1.LoadData("select Nombre,Comentarios,Sexo, Datepart('yyyy',FechaNac),DatePart('m',FechaNac),Datepart('d',FechaNac),estatus, calle, exterior, interior,cp,telefonocasa,telefonorecados,telefonocel,idciudad,idmunicipio,idcolonia, idestado, email from terapeutas where Id=" + this.curPaciente.ToString());
                this.comboBox3.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["estatus"].ToString());
                this.comboBox4.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["idestado"].ToString());

                String NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();

                Int32 AnoNac = Convert.ToInt32(DatosPaciente.Rows[0][3].ToString());
                Int32 MesNac = Convert.ToInt32(DatosPaciente.Rows[0][4].ToString());
                Int32 DiaNac = Convert.ToInt32(DatosPaciente.Rows[0][5].ToString());
                Int32 Sexo = Convert.ToInt32(DatosPaciente.Rows[0]["Sexo"].ToString());


                string Dato1 = ""; // DatosPaciente.Rows[0]["idestado"].GetType().ToString();

                Dato1 = DatosPaciente.Rows[0]["idmunicipio"].ToString();
                Int32 curMunicipio = 0;
                if (Dato1.Length != 0) curMunicipio = Convert.ToInt32(Dato1);

                Dato1 = DatosPaciente.Rows[0]["idcolonia"].ToString();
                Int32 curColonia = 0;
                if (Dato1.Length != 0) curColonia = Convert.ToInt32(Dato1);

                Dato1 = DatosPaciente.Rows[0]["idciudad"].ToString();
                Int32 curCiudad = 0;
                if (Dato1.Length != 0) curCiudad = Convert.ToInt32(Dato1);


                this.Colonia.Text = datos1.GetNombreTabla(curColonia, "colonia", "Id", "Nombre");
                this.Ciudad.Text = datos1.GetNombreTabla(curCiudad, "ciudad", "Id", "Nombre");
                this.Municipio.Text = datos1.GetNombreTabla(curMunicipio, "municipio", "Id", "Nombre");


                this.FechaNac_DatePicker.SelectedDate = new DateTime(AnoNac, MesNac, DiaNac);

                this.NombrePaciente.Text = NombrePaciente;
                this.Calle.Text = DatosPaciente.Rows[0]["calle"].ToString();
                this.exterior.Text = DatosPaciente.Rows[0]["exterior"].ToString();
                this.interior.Text = DatosPaciente.Rows[0]["interior"].ToString();
                this.CodigoPostal.Text = DatosPaciente.Rows[0]["cp"].ToString();
                this.telefonocasa.Text = DatosPaciente.Rows[0]["telefonocasa"].ToString();
                this.telefonomovil.Text = DatosPaciente.Rows[0]["telefonocel"].ToString();
                this.telefonorecados.Text = DatosPaciente.Rows[0]["telefonorecados"].ToString();
                this.email.Text = DatosPaciente.Rows[0]["email"].ToString();

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


                String photolocation = "C:\\Datos\\personal\\" + this.curPaciente.ToString() + ".jpg";  //file name 
                if (File.Exists(photolocation))
                {

                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(photolocation);
                        bitmap.EndInit();
                        this.image.Source = bitmap;
                        this.personPhoto = bitmap;
                    }

                    catch (System.IO.FileNotFoundException)
                    {
                        MessageBox.Show("There was an error opening the bitmap." +
                            "Please check the path.");
                    }


                }






            }

            else
            {
               
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


    
        private void guardaPaciente(object sender, RoutedEventArgs e)
        {

            ArrayList valores = new ArrayList();
            int Sexo = 1;
            if (this.radioButton2.IsChecked.Value) Sexo = 2;

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            string NombrePaciente = this.NombrePaciente.Text;

            if (NombrePaciente.Length < 5)
            {
                MessageBox.Show("El nombre es invalido", "Advertencia:");
                return;
            }

            if (this.FechaNac_DatePicker.SelectedDate == null)
            {
                MessageBox.Show("La Fecha de naciemiento es invalida!", "Advertencia:");
                return;
            }

            valores.Add(new Registro("Nombre", NombrePaciente));
            valores.Add(new Registro("FechaNac", this.FechaNac_DatePicker.SelectedDate));
            valores.Add(new Registro("Sexo", Sexo));
            
            valores.Add(new Registro("estatus", this.comboBox3.SelectedValue));
            valores.Add(new Registro("idestado", this.comboBox4.SelectedValue));
            valores.Add(new Registro("calle", this.Calle.Text));
            valores.Add(new Registro("cp", this.CodigoPostal.Text));

            valores.Add(new Registro("exterior", this.exterior.Text));
            valores.Add(new Registro("interior", this.interior.Text));

            valores.Add(new Registro("telefonocasa", this.telefonocasa.Text));
            valores.Add(new Registro("telefonocel", this.telefonomovil.Text));

            valores.Add(new Registro("telefonorecados", this.telefonorecados.Text));
            valores.Add(new Registro("email", this.email.Text));


            Int32 CurCiudad = datos1.BuscaNombreTabla(this.Ciudad.Text, "Ciudad", "Id", "Nombre");
            if (CurCiudad == 0) CurCiudad = datos1.InsertaNombreCampo(this.Ciudad.Text, "Ciudad", "Nombre");

            Int32 CurMunicipio = datos1.BuscaNombreTabla(this.Municipio.Text, "Municipio", "Id", "Nombre");
            if (CurMunicipio == 0) CurMunicipio = datos1.InsertaNombreCampo(this.Ciudad.Text, "Municipio", "Nombre");

            Int32 CurColonia = datos1.BuscaNombreTabla(this.Colonia.Text, "Colonia", "Id", "Nombre");
            if (CurColonia == 0) CurColonia = datos1.InsertaNombreCampo(this.Colonia.Text, "Colonia", "Nombre");


            valores.Add(new Registro("idciudad", CurCiudad));
            valores.Add(new Registro("idmunicipio", CurMunicipio));
            valores.Add(new Registro("idcolonia", CurColonia));



            if (this.curPaciente != 0) datos1.UpdateData(valores, this.curPaciente, "IdPaciente", "terapeutas");
            else this.curPaciente = datos1.InsertData(valores, "terapeutas");





            if (this.newLoadedImage)
            {
                try
                {

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    String photolocation = "C:\\Datos\\personal\\" + this.curPaciente.ToString() + ".jpg";  //file name 

                    //encoder.Frames.Add(BitmapFrame.Create(this.personPhoto));
                    encoder.Frames.Add(BitmapFrame.Create((BitmapImage)this.image.Source));
                    using (var filestream = new FileStream(photolocation, FileMode.Create))
                        encoder.Save(filestream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save now.");

                }

            }


            this.Close();


        }



        private void buscaColonia(object sender, RoutedEventArgs e)
        {
            listageneral dlg1 = new listageneral();
            dlg1.setIndexName("Id");
            dlg1.setNameIndex("Nombre");
            dlg1.setTable("Colonia");

            dlg1.ShowDialog();
            int newPaciente = dlg1.curId;

            this.Colonia.Text = dlg1.curValue;


        }


        private void buscaCiudad(object sender, RoutedEventArgs e)
        {
            listageneral dlg1 = new listageneral();
            dlg1.setIndexName("Id");
            dlg1.setNameIndex("Nombre");
            dlg1.setTable("Ciudad");

            dlg1.ShowDialog();
            int newPaciente = dlg1.curId;

            this.Ciudad.Text = dlg1.curValue;


        }

        private void buscaMunicipio(object sender, RoutedEventArgs e)
        {
            listageneral dlg1 = new listageneral();
            dlg1.setIndexName("Id");
            dlg1.setNameIndex("Nombre");
            dlg1.setTable("Municipio");

            dlg1.ShowDialog();
            int newPaciente = dlg1.curId;

            this.Municipio.Text = dlg1.curValue;


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            //// Load Image
            string curFileName = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == true) curFileName = openFileDialog.FileName;

            else return;

            MessageBox.Show("Archivo", openFileDialog.FileName);
            try
            {

                string selectedFileName = openFileDialog.FileName;
                // FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                this.image.Source = bitmap;
                this.personPhoto = bitmap;
                this.newLoadedImage = true;


            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("There was an error opening the bitmap." +
                    "Please check the path.");
            }




        }

    }
}
