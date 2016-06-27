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
using System.Windows.Navigation;

namespace cehavi_control
{
    /// <summary>
    /// Interaction logic for Calendar1.xaml
    /// </summary>
    public partial class Calendar1 : Window
    {
        public Calendar1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            this.webBrowser1.Navigate("c:\\Datos\\web\\testcalendar.html");

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            mshtml.HTMLDocument document = (mshtml.HTMLDocument)webBrowser1.Document;
            mshtml.IHTMLElement textArea = document.getElementById("curDate");
            MessageBox.Show(textArea.innerHTML, "Contenido");


        }
    }
}
