using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZDB
{
    internal abstract class DB
    {
        internal abstract List<string> GetTableList();

        internal abstract List<string> GetViewList();

        internal abstract DataTable GetSchema(string objectName, string itemName);

        internal abstract DataTable GetTableData(string tableName, long rowLimit);

        internal abstract int GetTableColumnIndex();

        internal abstract string GetTableColumnName();
    }
}
