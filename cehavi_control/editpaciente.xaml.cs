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
            this.DatosPaciente = datos1.LoadData("Select * from pacientes where IdPaciente=" + this.curPaciente.ToString() );
            String NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();
            this.NombrePaciente.Text = NombrePaciente;
            
        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
