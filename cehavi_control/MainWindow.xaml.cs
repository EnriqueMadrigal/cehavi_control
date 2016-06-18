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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New");
        }

        private void mnuNew_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listaPacientes(object sender, RoutedEventArgs e)
        {
            listapacientes dlg1 = new listapacientes();
            dlg1.ShowDialog(); 

        }
        private void muestraPaciente(object sender, RoutedEventArgs e)
        {
            int curPaciente = 0;
            listapacientes dlg1 = new listapacientes();
            dlg1.ShowDialog();
            curPaciente = dlg1.curPaciente;

            if (curPaciente == 0) return;

           // MessageBox.Show(curPaciente.ToString(), "Paciente Seleccionado");
            editpaciente dlg2 = new editpaciente();
            dlg2.SetPaciente(curPaciente);
            dlg2.ShowDialog();
            

          
        }

        private void newPaciente(object sender, RoutedEventArgs e)
        {
            editpaciente dlg2 = new editpaciente();
            dlg2.ShowDialog();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
