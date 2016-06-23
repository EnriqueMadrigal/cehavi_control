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

        private DataTable DatosTerapias;

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
            CargaTerapias();

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
                this.DatosPaciente = datos1.LoadData("select Nombre,Comentarios,Sexo,IdEscuela,IdGradoEscuela, Datepart('yyyy',FechaNac),DatePart('m',FechaNac),Datepart('d',FechaNac),estatus, calle, exterior, interior,cp,telefonocasa,telefonorecados,telefonocel,idciudad,idmunicipio,idcolonia, idestado, email from pacientes where IdPaciente=" + this.curPaciente.ToString());
                this.comboBox1.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["IdGradoEscuela"].ToString());
                // ComboBoxZone.SelectedValue.ToString());

                this.comboBox2.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["IdEscuela"].ToString());
                this.comboBox3.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["estatus"].ToString());
                this.comboBox4.SelectedValue = Convert.ToInt32(DatosPaciente.Rows[0]["idestado"].ToString());

                String NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();

                Int32 AnoNac = Convert.ToInt32(DatosPaciente.Rows[0][5].ToString());
                Int32 MesNac = Convert.ToInt32(DatosPaciente.Rows[0][6].ToString());
                Int32 DiaNac = Convert.ToInt32(DatosPaciente.Rows[0][7].ToString());
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
            
            listageneral dlg1 = new listageneral();
            dlg1.setIndexName("IdPaciente");
            dlg1.setNameIndex("Nombre");
            dlg1.setTable("pacientes");

            dlg1.ShowDialog();
            
            int newPaciente = dlg1.curId;



            if (newPaciente != 0)   
            {
                this.curPaciente = newPaciente;
                CargaDatos();
                CargaTerapias();
            }

        }

        private void guardaPaciente(object sender, RoutedEventArgs e)
        {

            ArrayList valores = new ArrayList();
            int Sexo = 1;
            if (this.radioButton2.IsChecked.Value) Sexo = 2;

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            string NombrePaciente = this.NombrePaciente.Text;

            if (NombrePaciente.Length <5)
            {
                MessageBox.Show("El nombre es invalido","Advertencia:");
                return;
            }

            valores.Add(new Registro("Nombre", NombrePaciente));
            valores.Add(new Registro("FechaNac", this.FechaNac_DatePicker.SelectedDate));
            valores.Add(new Registro("Sexo", Sexo));
            valores.Add(new Registro("IdGradoEscuela", this.comboBox1.SelectedValue));
            valores.Add(new Registro("IdEscuela", this.comboBox2.SelectedValue));
            valores.Add(new Registro("terapias", 1));
         
            valores.Add(new Registro("estatus", this.comboBox3.SelectedValue));
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

  

            if (this.curPaciente != 0) datos1.UpdateData(valores, this.curPaciente, "IdPaciente", "pacientes");
            else datos1.InsertData(valores,"pacientes");


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


        private void button10_Click(object sender, RoutedEventArgs e)
        {

            if (this.curPaciente == 0)
            {
                MessageBox.Show("Por favor primero, guarde el paciente y luego proceda a dar de alta las terapias", "Advertencia");
                return;
            }

            Terapia dlg1 = new Terapia();
            dlg1.SetCurPaciente(this.curPaciente);
            dlg1.NombrePaciente = this.NombrePaciente.Text;
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

                object curType = ((DataRowView) dataGrid.SelectedItem).Row[0];
               // string curObject = curType.GetType().ToString();

                Int32 curId = (Int32)((DataRowView)dataGrid.SelectedItem).Row["IdTerapia"];

                DatosCehavi datos1 = new DatosCehavi();
                datos1.Connect();

                datos1.executeQuery("delete from terapias where Id=" + curId.ToString());

                MessageBox.Show("Se elimino el registro", "Información");

                CargaTerapias();
            }

           

        }

        private void CargaTerapias()
        {
            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            string[] Dias = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };


            DataTable TerapiasTemp = datos1.LoadData("select Id, Dia, Hora, Duracion, IdTerapeuta, Minuto from terapias where IdPaciente=" + this.curPaciente);

            this.DatosTerapias = new DataTable("Terapias");
            this.DatosTerapias.Columns.Add("IdTerapia", Type.GetType("System.Int32"));
            this.DatosTerapias.Columns.Add("Dia", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Hora", Type.GetType("System.String"));
            this.DatosTerapias.Columns.Add("Duracion", Type.GetType("System.Int16"));
            this.DatosTerapias.Columns.Add("Terapeuta", Type.GetType("System.String"));


            foreach (DataRow c in TerapiasTemp.Rows)
            {
                string tipoA = c["Hora"].GetType().ToString();
                Int32 IdTerapia = (Int32)c["Id"];
                Int16 Dia =  (Int16) c["Dia"];
                Int16 Duracion = (Int16)c["Duracion"];
                Int16 IdTerapueta = (Int16)c["IdTerapeuta"];
                Byte Hora = (Byte)c["Hora"];
                Byte Minuto = (Byte)c["Minuto"];

                string horario =  string.Format("{0:D2}", Hora) + ":" + string.Format("{0:D2}", Minuto);


                this.DatosTerapias.Rows.Add(IdTerapia, Dias[Dia - 1], horario ,Duracion, "Sin asignar");


            }

            this.dataGrid.ItemsSource = this.DatosTerapias.DefaultView;

        }


    }
}


