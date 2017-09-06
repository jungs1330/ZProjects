using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace ZDB
{
    public static class DBExplorer
    {
        enum DataProvider
        { SqlServer, OracleDb, OleDb, Odbc, None }

        public static List<string> GetDBList()
        {
            string dataProvString = ConfigurationManager.AppSettings["DBList"];
            return new List<string>(dataProvString.Split(new char[] { '|' }));
        }

        public static List<string> GetTableList(string dbName)
        {
            DB db = GetDataObject(dbName);
            return db.GetTableList();
        }

        public static List<string> GetViewList(string dbName)
        {
            DB db = GetDataObject(dbName);
            return db.GetViewList();
        }

        public static DataTable GetSchema(string dbName, string objectName, string itemName)
        {
            DB db = GetDataObject(dbName);
            return db.GetSchema(objectName, itemName);
        }

        public static DataTable GetTableData(string dbName, string tableName, long rowLimit)
        {
            DB db = GetDataObject(dbName);
            return db.GetTableData(tableName, rowLimit);
        }

        public static int GetTableColumnIndex(string dbName)
        {
            DB db = GetDataObject(dbName);
            return db.GetTableColumnIndex();
        }

        public static string GetTableColumnName(string dbName)
        {
            DB db = GetDataObject(dbName);
            return db.GetTableColumnName();
        }

        private static DB GetDataObject(string dbName)
        {
            string temp = ConfigurationManager.AppSettings[dbName];
            string[] dbString = temp.Split(new char[] { '|' });
            string dbProvider = dbString[0];
            string connectionString = dbString[1];

            // Transform string to enum.
            DataProvider dp = DataProvider.None;
            if (Enum.IsDefined(typeof(DataProvider), dbProvider))
                dp = (DataProvider)Enum.Parse(typeof(DataProvider), dbProvider);

            IDbConnection conn = null;
            switch (dp)
            {
                case DataProvider.OracleDb:
                    return new DBOracle(connectionString);
                    break;
                case DataProvider.SqlServer:
                    return new DBSQLServer(connectionString);
                    break;
                default:
                    return null;
            }
        }
    }
}
