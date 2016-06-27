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

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for Calendar1.xaml
    /// </summary>
    public partial class Calendar1 : Window
    {

        private string nombrePaciente = "";
        private string curValue = "";
        private Int32 duracion;

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

        public string CurValue
        {
            get
            {
                return curValue;
            }

            set
            {
                curValue = value;
            }
        }

        public int Duracion
        {
            get
            {
                return duracion;
            }

            set
            {
                duracion = value;
            }
        }

        public Calendar1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            this.webBrowser1.Navigate("E:\\wamp\\www\\nailsalon\\testcalendar.html");
            
         

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            mshtml.HTMLDocument document = (mshtml.HTMLDocument)webBrowser1.Document;
            mshtml.IHTMLElement textArea = document.getElementById("curDate");
            //MessageBox.Show(textArea.innerHTML, "Contenido");
            this.curValue = textArea.innerHTML;
            this.Close();
          

        }

        private void SetHtmlValues()
        {

            mshtml.HTMLDocument document = (mshtml.HTMLDocument)webBrowser1.Document;
            mshtml.IHTMLElement textArea1 = document.getElementById("PacientName");
            mshtml.IHTMLElement textArea2 = document.getElementById("EventDuration");

            textArea1.innerHTML = nombrePaciente;
            textArea2.innerHTML = duracion.ToString();
            
        }

        private void webBrowser1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
             SetHtmlValues();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser1.InvokeScript("Testfunction");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser1.InvokeScript("Testfunction1");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser1.InvokeScript("Testfunction2");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser1.InvokeScript("Testfunction3");
        }


    }
}
