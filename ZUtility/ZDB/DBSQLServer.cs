using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZDB
{
    internal class DBSQLServer : DB 
    {
        const int tableColumnIndex = 2;
        const string tableColumnName = "TABLENAME";
        private string cnString = "";

        internal DBSQLServer(string connectionString)
        {
            cnString = connectionString;
        }

        internal override List<string> GetTableList()
        {
            string commandText = "SELECT TABLE_NAME FROM USER_TABLES ORDER BY TABLE_NAME";
            return PopulateList(commandText);
        }

        internal override List<string> GetViewList()
        {
            string commandText = "SELECT VIEW_NAME FROM USER_VIEWS ORDER BY VIEW_NAME";
            return PopulateList(commandText);
        }

        internal override DataTable GetSchema(string objectName, string itemName)
        {
            SqlConnection con = new SqlConnection(cnString);
            con.Open();
            DataTable dt = null;
            string[] schemaRestriction = null;

            switch (objectName)
            {
                case "Columns":
                    schemaRestriction = new string[] { null, null, itemName };
                    break;
                case "Index":
                    schemaRestriction = new string[] { null, null, itemName };
                    break;
            }

            try
            {
                if (!string.IsNullOrEmpty(objectName) && !string.IsNullOrEmpty(itemName))
                    dt = con.GetSchema(objectName, schemaRestriction);
                else if (!string.IsNullOrEmpty(objectName))
                    dt = con.GetSchema(objectName);
                else
                    dt = con.GetSchema("MetaDataCollections");
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
                con.Dispose();
            }

            return dt;
        }

        internal override DataTable GetTableData(string tableName, long rowLimit)
        {
            string commandText = "SELECT " + (rowLimit == 0 ? "" : " TOP " + rowLimit) + " * FROM " + tableName;
            return PopulateTable(commandText);
        }

        internal override int GetTableColumnIndex()
        {
            return tableColumnIndex;
        }

        internal override string GetTableColumnName()
        {
            return tableColumnName;
        }
        private DataTable PopulateTable(string sql)
        {
            SqlConnection con = new SqlConnection(cnString);
            DataTable dt = new DataTable();
            con.Open();

            try
            {
                string commandText = sql;
                SqlCommand cmd = new SqlCommand(commandText, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                adapter.Dispose();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
                con.Dispose();
            }

            return dt;
        }

        private List<string> PopulateList(string sql)
        {
            List<string> result = new List<string>();
            SqlConnection con = new SqlConnection(cnString);
            DataTable dt = new DataTable();
            con.Open();

            try
            {
                string commandText = sql;
                SqlCommand cmd = new SqlCommand(commandText, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    result.Add(dr[0].ToString());
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
                con.Dispose();
            }

            return result;
        }

        private bool ExecuteNonQuery(string commandText)
        {
            SqlConnection connection = new SqlConnection(cnString);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.CommandText = commandText;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }

            return true;
        }

        private bool ExecuteScalar(string commandText, out object returnValue)
        {
            returnValue = null;
            SqlConnection connection = new SqlConnection(cnString);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.CommandText = commandText;
                returnValue = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }

            return true;
        }
    }
}
