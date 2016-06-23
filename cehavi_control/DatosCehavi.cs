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
                MessageBox.Show(e.Message, "Exception: Connect");
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
                MessageBox.Show(e.Message, "Exception: LoadData");
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
                MessageBox.Show(e.Message, "Exception: UpdateData");
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
                MessageBox.Show(e.Message, "Exception: InsertData");
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
                MessageBox.Show(e.Message, "Exception: GetCreated");
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
                MessageBox.Show(e.Message, "Exception: GetModified");
                return false;
            }


            return created;
        }


        public bool executeQuery(string query)
        {

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
                MessageBox.Show(e.Message, "Exception: executeQuery");
                return false;
            }

            return true;
        }



        public Int32 BuscaNombreTabla(string nombre, string tabla, string nombreIndex, string nombreCampo)
        {
            Int32 curId = 0;

            // Busca si existe el indice

            if (nombre.Length == 0) return 0;

            try
            {
                string query = "select " + nombreIndex + " from " + tabla +  " where " + nombreCampo + "='" + nombre + "'";
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = query;
                OleDbDataReader resuldata = com.ExecuteReader();
                
                if(resuldata.HasRows)
                {
                    resuldata.Read();
                    curId = resuldata.GetInt32(0);
                }

            


            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: BuscaNombre");
                return 0;
            }


            return curId;
        }



        public Int32 InsertaNombreCampo(string nombre, string tabla, string nombreCampo)
        {

            Int32 curId = 0;
            if (nombre.Length == 0) return 0;

            try
            {
                string query = "insert into " + tabla + "(" + nombreCampo + ")" + " values('" + nombre + "')" ;
                string query2 = "Select @@Identity";

                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = query;
                com.ExecuteNonQuery();

                com.CommandText = query2;
                curId = (Int32)com.ExecuteScalar();

                //com.Dispose();

            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: InsertaNombreCampo");
                return 0;
            }

          return curId;


        }

        public string GetNombreTabla(Int32 numRegistro, string tabla, string indexName, string nombreIndex)
        {

            string curNombre = "";

            try
            {
                string query = "select " + nombreIndex + " from " + tabla + " where " + indexName + "=" + numRegistro.ToString();
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = query;
                OleDbDataReader resuldata = com.ExecuteReader();

                if (resuldata.HasRows)
                {
                    resuldata.Read();
                    curNombre = resuldata.GetString(0);
                }
            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: GetNombreTabla");
                return "";
            }


            return curNombre;

        }


        public AutoCompleteStringCollection GetAutoCompleteCollection(string tabla, string nombreIndex)
        {
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();


            try
            {
                string query = "select " + nombreIndex + " from " + tabla;
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                com.CommandText = query;
                OleDbDataReader resuldata = com.ExecuteReader();

                if (resuldata.HasRows)
                {


                    while (resuldata.Read() )
                    {
                        MyCollection.Add(resuldata.GetString(0));
                    }

                }
                
            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: BuscaNombre");
                return null;
            }

            
            return MyCollection;
            
        }



        public void CargaComboBoxData(System.Windows.Controls.ComboBox comboBox, string tableString)
        {

            /*
             
            DataTable DatosTerapuetas = datos1.LoadData("select Id, Nombre from terapeutas order by Nombre");

            this.comboBoxTerapeutas.ItemsSource = DatosTerapuetas.DefaultView;
            this.comboBoxTerapeutas.DisplayMemberPath = DatosTerapuetas.Columns["Nombre"].ToString();
            this.comboBoxTerapeutas.SelectedValuePath = DatosTerapuetas.Columns["Id"].ToString();
             * * 
             * */

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
                
                comboBox.ItemsSource = ds.DefaultView;
                comboBox.DisplayMemberPath = ds.Columns[1].ToString();
                comboBox.SelectedValuePath = ds.Columns[0].ToString();


            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: LoadData");
                return;
            }




            return;

        }


        /////////////////

    }




}
