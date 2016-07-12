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

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for cambiaestado1.xaml
    /// </summary>
    public partial class cambiaestado1 : Window
    {

        private Int32 curEvento;
        private bool estado_cambio = false;

        public cambiaestado1()
        {
            InitializeComponent();
        }

        public int CurEvento
        {
            get
            {
                return curEvento;
            }

            set
            {
                curEvento = value;
            }
        }

        public bool Estado_cambio
        {
            get
            {
                return estado_cambio;
            }

            set
            {
                estado_cambio = value;
            }
        }

        private void CargaDatos()
        {
            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            datos1.CargaComboBoxData(this.comboBox_Estado, "select Id,Nombre from EstadoEventos1 order by Id");
            this.comboBox_Estado.SelectedValue = 1;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatos();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.estado_cambio = false;
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            string CurEstado =  this.comboBox_Estado.SelectedValue.ToString();

            datos1.executeQuery("update Eventos set status1="+ CurEstado + " where Id=" + this.curEvento.ToString());

            this.estado_cambio = true;
            this.Close();

        }
    }
    }
