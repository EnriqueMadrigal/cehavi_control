using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace cehavi_control
{
    class DatosCehavi
    {
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Datos\\cehavi.accdb;Persist Security Info=True";
        private OleDbConnection curConnection;


       public  bool Connect()
        {
            try
            {
                this.curConnection = new OleDbConnection(this.connectionString);
            }

            catch (Exception a)
             {
                MessageBox.Show("Exception: {0}", a.Message);
                return false;
            }



            return true;
        }

       



        public DataTable LoadData(string tableString)
        {

            try
            {
                DataTable ds = new DataTable("Nombres");
                OleDbCommand com = new OleDbCommand();

                com.Connection = this.curConnection;
                com.CommandText = tableString;
                OleDbDataAdapter adapt = new OleDbDataAdapter();
                adapt.SelectCommand = com;
                adapt.Fill(ds);
                this.curConnection.Close();
                return ds;
                
            }

            catch (Exception e)
            {
                MessageBox.Show("Exception: {0}", e.Message);
                return null;
            }





        }

        /////////////////

    }

  


}
