using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;

namespace cehavi_control
{
    class DatosCehavi
    {
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Datos\\cehavi.accdb;Persist Security Info=True";
       private  OleDbConnection curConnection;


        public DatosCehavi()
        {
            this.curConnection = new OleDbConnection(this.connectionString);
        }

        

       public  bool Connect()
        {
            try
            {
                 this.curConnection.Open();
            }

            catch (Exception e)
             {
                MessageBox.Show(e.Message, "Exception: {0}");
                return false;
            }



            return true;
        }

       
        private OleDbConnection GetConnection()
        {

            if (this.curConnection.State != ConnectionState.Open)
            {
                Connect();
            }

            return this.curConnection;
        }


        public DataTable LoadData(string tableString)
        {

            try
            {
                DataTable ds = new DataTable("Nombres");
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = tableString;
                OleDbDataAdapter adapt = new OleDbDataAdapter();
                adapt.SelectCommand = com;
                adapt.Fill(ds);
                //this.curConnection.Close();
                return ds;
                
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: {0}");
                return null;
            }





        }


        public bool UpdateData(ArrayList datos, int curRegistro, string NameIndex, string Table)
        {



            string query = "";

            query = "UPDATE " + Table + " SET ";

                       
            foreach (Registro c in datos)
            {

                query = query + c.GetName() + "= ";


                string TipoDato = c.getValue().GetType().ToString();
                if (TipoDato == "System.String" || TipoDato == "System.DateTime")
                {
                    query = query + "'";
                    query = query + c.getValue().ToString();
                    query = query + "'";
                }

                
                else
                {
                    query = query + c.getValue().ToString();
                }

                //MessageBox.Show("TipoDato: {0}", c.getValue().GetType().ToString());
                //MessageBox.Show("TipoDato: {0}", c.getValue().ToString());
                query = query + ",";

            }



            if (GetModified(Table))
            {
                query = query + "modified = '" + DateTime.Now.ToString() +"',";

            }


            query = query.Remove(query.Length - 1);
            query = query + " WHERE " + NameIndex + "=" + curRegistro.ToString();

            //MessageBox.Show(query, "Query");



            try
            {

                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = query;
                com.ExecuteNonQuery();
                //com.Dispose();

            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: {0}");
                return false;
            }


            return true;

        }


        public bool InsertData(ArrayList datos, string Table)
        {



            string query = "";

            query = "INSERT INTO " + Table + " (";


            foreach (Registro c in datos)
            {
                query = query + c.GetName() + ",";
            }



            if (GetCreated(Table))
            {
                query = query + "created,";
            }


            if (GetModified(Table))
            {
                query = query + "modified,";
            }


            query = query.Remove(query.Length - 1);
            query = query + ") values(";



            foreach (Registro c in datos)
            {

                string TipoDato = c.getValue().GetType().ToString();
                if (TipoDato == "System.String" || TipoDato == "System.DateTime")
                {
                    query = query + "'";
                    query = query + c.getValue().ToString();
                    query = query + "'";
                }


                else
                {
                    query = query + c.getValue().ToString();
                }

                query = query + ",";


            }



            if (GetCreated(Table))
            {
                query = query + "'";
                query = query + DateTime.Now.ToString();
                query = query + "',";

            }


            if (GetModified(Table))
            {
                query = query + "'";
                query = query + DateTime.Now.ToString();
                query = query + "',";

            }


            query = query.Remove(query.Length - 1);
            query = query + ")";



            //MessageBox.Show(query, "Query");

       

            try
            {

                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = query;
                com.ExecuteNonQuery();
                //com.Dispose();

            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: {0}");
                return false;
            }


            return true;

        }







        private bool GetCreated(string table)
        {

            bool created = false;

            string tableString = "select top 1 * from " + table + "";
            try
            {
                DataTable ds = new DataTable("Nombres");
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = tableString;
                OleDbDataAdapter adapt = new OleDbDataAdapter();
                adapt.SelectCommand = com;
                adapt.Fill(ds);
                //this.curConnection.Close();


                foreach (DataColumn column in ds.Columns)
                {
                    if (column.ColumnName == "created") created = true;
           
                 }

         
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: {0}");
                return false;
            }


            return created;
        }





        private bool GetModified(string table)
        {

            bool created = false;

            string tableString = "select top 1 * from " + table + "";
            try
            {
                DataTable ds = new DataTable("Nombres");
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = tableString;
                OleDbDataAdapter adapt = new OleDbDataAdapter();
                adapt.SelectCommand = com;
                adapt.Fill(ds);
                //this.curConnection.Close();


                foreach (DataColumn column in ds.Columns)
                {
                    if (column.ColumnName == "modified") created = true;

                }


            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: {0}");
                return false;
            }


            return created;
        }


        /////////////////

    }




}
