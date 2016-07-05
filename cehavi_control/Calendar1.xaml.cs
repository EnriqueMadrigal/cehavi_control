using System;
using System.Collections.Generic;
using System.IO;
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
        private Int32 curTerapeuta = 0;

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

        public int CurTerapeuta
        {
            get
            {
                return curTerapeuta;
            }

            set
            {
                curTerapeuta = value;
            }
        }

        public Calendar1()
        {
            InitializeComponent();
            createFile();
        }

        public Calendar1(Int32 Terapeuta)
        {

            InitializeComponent();
            this.curTerapeuta = Terapeuta;
            createFile();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            //this.webBrowser1.Navigate("E:\\wamp\\www\\nailsalon\\testcalendar.html");
            this.webBrowser1.Navigate("C:\\Datos\\web\\testcalendar.html");


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



        private void createFile()
        {

            StreamWriter file = new System.IO.StreamWriter(@"C:\Datos\Web\getevents.js");

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            StringBuilder jsonData = new StringBuilder();

            

            //file.WriteLine("var curEvents = ");

            DateTime CurTime = DateTime.Now;
            DateTime EndTime = CurTime.AddMonths(1);

            string json = datos1.getJsonEvents(CurTime, EndTime, "IdEvento in (select Id from Terapias where IdTerapeuta=" + this.curTerapeuta.ToString() + ") or IdEvento in (select Id from Citas where IdTerapeuta=" + this.curTerapeuta.ToString() + ")" );

            if (json.Length!=0)
            {
                jsonData.Append("var curEvents = ");
                jsonData.Append(json);
                jsonData.Append(";");

            }

            else
            {
                jsonData.Append("var curEvent = [];");
            }

            file.WriteLine(jsonData.ToString());

            
            file.Close();




        }

    }
}
