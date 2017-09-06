using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace ZDB
{
    internal class DBOracle : DB 
    {
        const int tableColumnIndex = 1;
        const string tableColumnName = "TABLENAME";
        private string cnString = "";
        
        internal DBOracle(string connectionString)
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
            OracleConnection con = new OracleConnection(cnString);
            con.Open();
            DataTable dt = null;
            string[] schemaRestriction = null;

            switch (objectName)
            {
                case "Columns":
                    schemaRestriction = new string[] { null, itemName };
                    break;
                case "Index":
                    schemaRestriction = new string[] { null, null, null, itemName };
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
            string commandText = "SELECT * FROM " + tableName + (rowLimit == 0 ? "" : " WHERE ROWNUM <= " + rowLimit);
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
            OracleConnection con = new OracleConnection(cnString);
            DataTable dt = new DataTable();
            con.Open();

            try
            {
                string commandText = sql;
                OracleCommand cmd = new OracleCommand(commandText, con);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
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
            OracleConnection con = new OracleConnection(cnString);
            DataTable dt = new DataTable();
            con.Open();

            try
            {
                string commandText = sql;
                OracleCommand cmd = new OracleCommand(commandText, con);
                OracleDataReader dr = cmd.ExecuteReader();
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
            OracleConnection connection = new OracleConnection(cnString);
            connection.Open();
            OracleCommand cmd = new OracleCommand();
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
            OracleConnection connection = new OracleConnection(cnString);
            connection.Open();
            OracleCommand cmd = new OracleCommand();
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
