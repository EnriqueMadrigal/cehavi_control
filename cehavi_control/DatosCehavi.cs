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


        public Int32 InsertData(ArrayList datos, string Table)
        {


            Int32 LastInserted = 0;
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

                com.CommandText = "SELECT @@IDENTITY";
                OleDbDataReader resuldata = com.ExecuteReader();

                if (resuldata.HasRows)
                {
                    resuldata.Read();
                    LastInserted = resuldata.GetInt32(0);
                }


            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: InsertData");
                return 0;
            }


            return LastInserted;

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


        public Int32 GetValueTabla(Int32 numRegistro, string tabla, string indexName, string nombreIndex)
        {

            Int32 Curvalue = 0;

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
                    System.Type curType = resuldata.GetType();

                    Curvalue = resuldata.GetInt32(0);
                    if (curType == typeof(Int32)) Curvalue = resuldata.GetInt32(0);
                    if (curType == typeof(Int16)) Curvalue = System.Convert.ToInt32(resuldata.GetInt16(0));
                    if (curType == typeof(Int64)) Curvalue = System.Convert.ToInt32(resuldata.GetInt64(0));
                    if (curType == typeof(Byte)) Curvalue = System.Convert.ToInt32(resuldata.GetByte(0));

                }
            }


            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: GetNombreTabla");
                return 0;
            }


            return Curvalue;

        }



      public DataTable GetEvents()
        {

            try
            {
                DataTable ds = new DataTable("Eventos");
                OleDbCommand com = new OleDbCommand();

                com.Connection = GetConnection();
                //com.CommandText = "select * FROM Terapias WHERE(DatePart('m', Fecha) = " + startDate.Month.ToString() + ") OR (DatePart('m', Fecha) = " + endDate.Month.ToString() + ") order by IdPaciente";
                com.CommandText = "select * from terapias order by IdPaciente";

                    OleDbDataAdapter adapt = new OleDbDataAdapter();
                adapt.SelectCommand = com;
                adapt.Fill(ds);
                //this.curConnection.Close();
                return ds;

            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception: LoadEvents");
                return null;
            }


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

        /*
         *
         *
         * 





         public DataTable GetCurretEvents(DateTime StartDate, DateTime EndDate)
         {
             DataTable DatosEventos = new DataTable("Eventos");

             DatosCehavi datos1 = new DatosCehavi();
             datos1.Connect();

             DataTable TempData = datos1.GetEvents();

             string NombrePaciente = "";
             Int16 StatusPaciente = 0;
             int CurPaciente = 0;

             DatosEventos.Columns.Add("IdEvento", Type.GetType("System.Int32"));
             DatosEventos.Columns.Add("Fecha", Type.GetType("System.DateTime"));
             DatosEventos.Columns.Add("Duracion", Type.GetType("System.Int16"));
             DatosEventos.Columns.Add("Title", Type.GetType("System.String"));


             foreach (DataRow c in TempData.Rows)
             {
                 Int32 IdEvento = (Int32)c["Id"];
                 Int32 IdPaciente = (Int32)c["IdPaciente"];
                 Int16 Duracion = (Int16)c["Duracion"];
                 Int16 IdTerapueta = (Int16)c["IdTerapeuta"];
                 Int16 Periodo = (Int16)c["Periodo"];
                 DateTime AFecha = (DateTime)c["Fecha"];
                 DateTime BFecha = (DateTime)c["Fecha2"];
                 Byte Dia = (Byte)c["Dia"];


                 if (CurPaciente == 0 || IdPaciente != CurPaciente)
                 {
                     CurPaciente = IdPaciente;
                     DataTable DatosPaciente = datos1.LoadData("select * from pacientes where IdPaciente=" + IdPaciente.ToString());
                     NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();
                     StatusPaciente = (Int16)DatosPaciente.Rows[0]["estatus"];
                 }


                 if (DateTime.Compare(BFecha, StartDate) <= 0) continue;
                 if (DateTime.Compare(AFecha, EndDate) >= 0) continue;

                 //if (DateTime.Compare(AFecha, StartDate) > 0) AFecha = StartDate;
                 if (DateTime.Compare(BFecha, EndDate) > 0) BFecha = EndDate;



                 while (DateTime.Compare(AFecha,BFecha)<0)
                 {
                     int curDia = (int)AFecha.DayOfWeek;
                     int diasadd = 0;
                     if (curDia > Dia) diasadd = (6 - curDia) + Dia;
                     if (curDia < Dia) diasadd = Dia - curDia;
                     DateTime EventoFecha = AFecha.AddDays(diasadd);

                     DatosEventos.Rows.Add(IdEvento, AFecha.ToString("yyyy-MM-dd HH:mm:ss"),Duracion,NombrePaciente);

                     if (Periodo == 2) AFecha = AFecha.AddDays(1);
                     if (Periodo == 3) AFecha = AFecha.AddDays(7);
                     if (Periodo == 4) AFecha = AFecha.AddMonths(1);



                 }






             }

             return DatosEventos;

         }

         */


        public void CreateCurretEvents(Int32 curTerapia)
        {
           
            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();

            //DataTable TempData = datos1.GetEvents();
            DataTable TempData = datos1.LoadData("select * from Terapias where Id=" + curTerapia.ToString());
            DataTable TablaColores = datos1.LoadData("select * from Colores order by Id");

            string NombrePaciente = "";
            Int16 StatusPaciente = 0;
            Int32 CurColor = 0;
            int CurPaciente = 0;
            string ColorPaciente = "#000000";
        


            foreach (DataRow c in TempData.Rows)
            {
                Int32 IdEvento = (Int32)c["Id"];
                Int32 IdPaciente = (Int32)c["IdPaciente"];
                Int16 Duracion = (Int16)c["Duracion"];
                Int16 IdTerapueta = (Int16)c["IdTerapeuta"];
                Int16 Periodo = (Int16)c["Periodo"];
                DateTime AFecha = (DateTime)c["Fecha"];
                DateTime BFecha = (DateTime)c["Fecha2"];
                DateTime Hora = (DateTime)c["Hora"];
                Byte Dia = (Byte)c["Dia"];


                if (CurPaciente == 0 || IdPaciente != CurPaciente)
                {
                    CurPaciente = IdPaciente;
                    DataTable DatosPaciente = datos1.LoadData("select * from pacientes where IdPaciente=" + IdPaciente.ToString());
                    NombrePaciente = DatosPaciente.Rows[0]["Nombre"].ToString();
                    StatusPaciente = (Int16)DatosPaciente.Rows[0]["estatus"];
                    CurColor = IdPaciente % 55;
                    ColorPaciente = "#" + TablaColores.Rows[CurColor]["Color"].ToString();
                }


                int curDia = (int)AFecha.DayOfWeek;
                int diasadd = 0;
                if (curDia > Dia) diasadd = (7 - curDia) + Dia;
                if (curDia < Dia) diasadd = Dia - curDia;

                DateTime EventoFecha = AFecha.AddDays(diasadd);
                DateTime EventoFechaStart = new DateTime(AFecha.Year, AFecha.Month, EventoFecha.Day, Hora.Hour, Hora.Minute, 0);
                DateTime EventoFechaEnd = EventoFechaStart.AddMinutes(Duracion);




                while (DateTime.Compare(EventoFechaStart, BFecha) < 0)
                {



                    ArrayList valores = new ArrayList();

                    valores.Add(new Registro("IdEvento", IdEvento));
                    valores.Add(new Registro("Title", NombrePaciente));
                    valores.Add(new Registro("start_event", EventoFechaStart.ToString("yyyy-MM-dd HH:mm:ss")));
                    valores.Add(new Registro("end_event", EventoFechaEnd.ToString("yyyy-MM-dd HH:mm:ss")));
                    valores.Add(new Registro("Color", ColorPaciente));
                    valores.Add(new Registro("status1", 0));
                    valores.Add(new Registro("status2", 0));

                    datos1.InsertData(valores, "Eventos");


                    if (Periodo == 2)
                    {
                        EventoFechaStart = EventoFechaStart.AddDays(1);
                        EventoFechaEnd = EventoFechaEnd.AddDays(1);
                    }

                    if (Periodo == 3)
                    {
                        EventoFechaStart = EventoFechaStart.AddDays(7);
                        EventoFechaEnd = EventoFechaEnd.AddDays(7);
                    }

                    if (Periodo == 4)
                    {
                        EventoFechaStart = EventoFechaStart.AddMonths(1);
                        EventoFechaEnd = EventoFechaEnd.AddMonths(1);
                    }

                                        

                }






            }

          

        }

        /////////////////


public string getJsonEvents(DateTime startDate, DateTime endDate)
        {

            DatosCehavi datos1 = new DatosCehavi();
            datos1.Connect();
            
            DataTable Eventos = datos1.LoadData("select * from Eventos");

            StringBuilder json = new StringBuilder();


            char[] caracter1 = { '"' };
            string quotes = new string(caracter1);

            json.Append("[");

            
            foreach (DataRow c in Eventos.Rows)
            {

                DateTime startFecha = (DateTime)c["start_event"];
                DateTime endFecha = (DateTime)c["end_event"];

                json.Append("{");
                json.Append(quotes + "id" + quotes + ":" + c["IdEvento"].ToString() + ",");
                json.Append(quotes + "title" + quotes + ":" + quotes + c["Title"].ToString() + quotes + ",");
                json.Append(quotes + "start" + quotes + ":" + quotes + startFecha.ToString("o") + quotes + ",");
                json.Append(quotes + "end" + quotes + ":" + quotes + endFecha.ToString("o") + quotes + ",");
                json.Append(quotes + "color" + quotes + ":" + quotes + c["Color"].ToString() + quotes + ",");
                json.Append(quotes + "allday" + quotes + ":" + "false" + ",");
                json.Append(quotes + "editable" + quotes + ":" + "false");
                json.Append("}");
                json.Append(",");

            }

            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();

        }


    }




}
