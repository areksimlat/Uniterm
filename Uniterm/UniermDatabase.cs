using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Uniterm
{
    public class UnitermDatabase
    {
        private static string DB_ID_FIELD = "id";
        private static string DB_NAME_FIELD = "name";
        private static string DB_DECS_FIELD = "description";
        private static string DB_SA_FIELD = "sA";
        private static string DB_SB_FIELD = "sB";
        private static string DB_SOP_FIELD = "sOp";
        private static string DB_EA_FIELD = "eA";
        private static string DB_EB_FIELD = "eB";
        private static string DB_EOP_FIELD = "eOp";
        private static string DB_FONT_SIZE_FIELD = "fontSize";
        private static string DB_FONT_NAME_FIELD = "fontName";
        private static string DB_SWITCH_FIELD = "switched";

        private static readonly string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["UnitermConnectionString"].ConnectionString;

        private static UnitermDatabase instance;
        private SqlConnection connection;


        private UnitermDatabase()
        {
            connection = new SqlConnection(connectionString);
        }

        public static UnitermDatabase GetInstance()
        {
            if (instance == null)
                instance = new UnitermDatabase();

            return instance;
        }

        private void connect()
        {
            try
            {
                this.connection.Open();
            }
            catch
            {
                
            }
        }

        public void disconnect()
        {
            try
            {
                this.connection.Close();
            }
            catch
            {

            }
        }

        private SqlDataAdapter createAdapter(string query)
        {
            return new SqlDataAdapter(query, connection);
        }

        private DataTable createDataTable(string query)
        {
            DataTable table = new DataTable();

            connect();

            if (connection.State == ConnectionState.Open)
            {
                SqlDataAdapter adapter = createAdapter(query);
                adapter.Fill(table);
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            disconnect();

            return table;
        }

        private DataRow createDataRow(string query)
        {
            DataTable table = new DataTable();
            DataRow row;

            connect();

            if (connection.State == ConnectionState.Open)
            {
                SqlDataAdapter adapter = createAdapter(query);
                adapter.Fill(table);

                try
                {
                    row = table.Rows[0];
                }
                catch
                {
                    row = null;
                }
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            disconnect();

            return row;
        }

        private void runQuery(string query)
        {
            connect();

            if (connection.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            disconnect();
        }

        public List<string> GetUnitermNames()
        {
            string query = "SELECT " + DB_ID_FIELD + ", " + DB_NAME_FIELD + " FROM Uniterms;";
            DataTable dataTable = createDataTable(query);

            List<string> items = new List<string>();

            foreach (DataRow dataRow in dataTable.Rows)
                items.Add(Convert.ToString(dataRow[DB_NAME_FIELD]));

            return items;
        }

        private int getLastInsertedId()
        {
            string query = "SELECT MAX(" + DB_ID_FIELD + ") AS last_id FROM Uniterms;";
            DataRow dataRow = createDataRow(query);

            return Convert.ToInt32(dataRow["last_id"]);
        }

        public bool AddUniterm(Uniterm uniterm)
        {
            string query;

            if ((uniterm.sA == "" && uniterm.eA == "") || uniterm.name == "")
            {
                return false;
            }
            
            if (uniterm.id == -1 && GetUnitermByName(uniterm.name) != null)
            {
                return false;
            }
      
            try
            {
                if (uniterm.id == -1)
                {
                    query = "INSERT INTO Uniterms " +
                            "(" +
                            DB_ID_FIELD + "," +
                            DB_NAME_FIELD + "," +
                            DB_DECS_FIELD + "," +
                            DB_SA_FIELD + "," +
                            DB_SB_FIELD + "," +
                            DB_SOP_FIELD + "," +
                            DB_EA_FIELD + "," +
                            DB_EB_FIELD + "," +
                            DB_EOP_FIELD + "," +
                            DB_FONT_SIZE_FIELD + "," +
                            DB_FONT_NAME_FIELD + "," +
                            DB_SWITCH_FIELD + ") " +
                            "VALUES (" +
                            "NEXT VALUE FOR UnitermSequence, '" +
                            uniterm.name + "','" +
                            uniterm.description + "','" +
                            uniterm.sA + "','" +
                            uniterm.sB + "','" +
                            uniterm.sOp + "','" +
                            uniterm.eA + "','" +
                            uniterm.eB + "','" +
                            uniterm.eOp + "'," +
                            uniterm.fontSize + ",'" +
                            uniterm.fontFamily + "','" +
                            uniterm.switched + "');";
                }
                else
                {
                    query = "UPDATE Uniterms SET " +
                        DB_NAME_FIELD + " = '" + uniterm.name + "'," +
                        DB_DECS_FIELD + " = '" + uniterm.description + "'," +
                        DB_SA_FIELD + " = '" + uniterm.sA + "'," +
                        DB_SB_FIELD + " = '" + uniterm.sB + "'," +
                        DB_SOP_FIELD + " = '" + uniterm.sOp + "'," +
                        DB_EA_FIELD + " = '" + uniterm.eA + "'," +
                        DB_EB_FIELD + " = '" + uniterm.eB + "'," +
                        DB_EOP_FIELD + " = '" + uniterm.eOp + "'," +
                        DB_FONT_SIZE_FIELD + " = '" + uniterm.fontSize + "'," +
                        DB_FONT_NAME_FIELD + " = '" + uniterm.fontFamily + "'," +
                        DB_SWITCH_FIELD + " = '" + uniterm.switched + "' " +
                            "WHERE " + DB_ID_FIELD + " = " + uniterm.id + ";";
                }

                runQuery(query);

                uniterm.id = getLastInsertedId();
                uniterm.modified = false;
                uniterm.saved = true;
            }
            catch (Exception ex)
            {
               System.Diagnostics.Debug.WriteLine(ex.ToString());
               return false;
            }

            return true;
        }

        public Uniterm GetUnitermByName(string name)
        {
            Uniterm uniterm = null;
            try
            {
                string query = "SELECT * FROM Uniterms WHERE " + DB_NAME_FIELD + "='" + name + "';";
                DataRow dataRow = createDataRow(query);

                if (dataRow != null)
                {
                    uniterm = new Uniterm();
                    uniterm.id = Convert.ToInt32(dataRow[DB_ID_FIELD]);
                    uniterm.name = Convert.ToString(dataRow[DB_NAME_FIELD]);
                    uniterm.description = Convert.ToString(dataRow[DB_DECS_FIELD]);
                    uniterm.sA = Convert.ToString(dataRow[DB_SA_FIELD]);
                    uniterm.sB = Convert.ToString(dataRow[DB_SB_FIELD]);
                    uniterm.sOp = Convert.ToString(dataRow[DB_SOP_FIELD]);
                    uniterm.eA = Convert.ToString(dataRow[DB_EA_FIELD]);
                    uniterm.eB = Convert.ToString(dataRow[DB_EB_FIELD]);
                    uniterm.eOp = Convert.ToString(dataRow[DB_EOP_FIELD]);
                    uniterm.switched = Convert.ToChar(dataRow[DB_SWITCH_FIELD]);
                    uniterm.fontSize = Convert.ToInt32(dataRow[DB_FONT_SIZE_FIELD]);
                    uniterm.fontFamily = Convert.ToString(dataRow[DB_FONT_NAME_FIELD]);
                }
                
            }
            catch (Exception ex)
            {
                
            }

            return uniterm;
        }

        public bool RemoveUnitermById(int id)
        {
            try
            {
                string query = "DELETE FROM Uniterms WHERE " + DB_ID_FIELD + " = " + id + ";";
                runQuery(query);
            }
            catch (Exception ex)
            {
               return false;
            }

            return true;
        }
    }
}