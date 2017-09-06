using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZUtility.Properties;
using ZUtility.Command;
using ZDB;

namespace ZUtility.ViewModel
{
    public class DataViewerViewModel : WorkspaceViewModel
    {
        #region fields
        private List<string> databaseList;
        private string database;
        private DataTable databaseObjects;
        private string databaseObject;
        private DataTable itemList;
        private string item;
        private int tableColumnIndex;
        private DataRowView selectedTable;
        private DataTable columnList;
        private DataTable indexList;
        private DataTable dataList;
        private string tableName;
        private string rowCount;
        private string filterString;
        private string filterColumn;
        private DataView itemListView;
        private bool bCanClearFilter = false;
        #endregion

        #region Constructor
        public DataViewerViewModel()
        {
            base.DisplayName = Resources.MainWindowViewModel_Command_DB_Browser;
            
            this.FilterCommand = new RelayCommand(this.OnFilter, this.CanFilter);
            this.ClearFilterCommand = new RelayCommand(this.OnClearFilter, this.CanClearFilter);
            this.RefreshCommand = new RelayCommand(this.OnRefresh, this.CanRefresh);

            DatabaseList = DBExplorer.GetDBList();
            RowCount = "1000";
        }
        #endregion

        public List<string> DatabaseList
        {
            get { return databaseList; }
            set
            {
                databaseList = value;
                base.OnPropertyChanged("DatabaseList");
            }
        }

        public string Database
        {
            get { return database; }
            set
            {
                database = value;

                base.OnPropertyChanged("Database");

                if (string.IsNullOrWhiteSpace(database))
                {
                    DatabaseObjects = null;
                }
                else
                {
                    DatabaseObjects = DBExplorer.GetSchema(Database, null, null);
                    TableColumnIndex = DBExplorer.GetTableColumnIndex(Database);
                }

            }
        }

        public DataTable DatabaseObjects
        {
            get { return databaseObjects; }
            set
            {
                databaseObjects = value;
                base.OnPropertyChanged("DatabaseObjects");
            }
        }

        public string DatabaseObject
        {
            get { return databaseObject; }
            set
            {
                databaseObject = value;

                base.OnPropertyChanged("DatabaseObject");

                if (string.IsNullOrWhiteSpace(DatabaseObject))
                {
                    ItemList = null;
                }
                else
                {
                    ItemList = DBExplorer.GetSchema(Database, DatabaseObject, null);
                }
            }
        }

        public DataTable ItemList
        {
            get { return itemList; }
            set
            {
                itemList = value;
                base.OnPropertyChanged("ItemList");
                base.OnPropertyChanged("ItemColumnList");
                ItemListView = itemList.DefaultView;
            }
        }

        public DataView ItemListView
        {
            get { return itemListView; }
            set
            {
                itemListView = value;
                base.OnPropertyChanged("ItemListView");
            }
        }

        public string Item
        {
            get { return item; }
            set
            {
                item = value;

                base.OnPropertyChanged("Item");
            }
        }

        public DataColumnCollection ItemColumnList
        {
            get { return ItemList == null ? null : ItemList.Columns; }
        }

        public int TableColumnIndex
        {
            get { return tableColumnIndex; }
            set
            {
                tableColumnIndex = value;

                base.OnPropertyChanged("TableColumnIndex");
            }
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;

                base.OnPropertyChanged("FilterString");
                this.FilterCommand.RaiseCanExecuteChanged();
            }
        }

        public string FilterColumn
        {
            get { return filterColumn; }
            set
            {
                filterColumn = value;

                base.OnPropertyChanged("FilterColumn");
                this.FilterCommand.RaiseCanExecuteChanged();
            }
        }

        public string TableName
        {
            get { return tableName; }
            set
            {
                tableName = value;

                base.OnPropertyChanged("TableName");
            }
        }

        public string RowCount
        {
            get { return rowCount; }
            set
            {
                rowCount = value;

                base.OnPropertyChanged("RowCount");
            }
        }

        public DataRowView SelectedTable
        {
            get { return selectedTable; }
            set
            {
                selectedTable = value;

                base.OnPropertyChanged("SelectedTable");

                if (selectedTable != null && DatabaseObject == "Tables")
                {
                    DataRowView drv = selectedTable as DataRowView;
                    TableName = drv[TableColumnIndex].ToString();

                    FetchTableDetails();
                }
            }
        }

        private void FetchTableDetails()
        {
            ColumnList = DBExplorer.GetSchema(Database, "Columns", TableName);
            IndexList = DBExplorer.GetSchema(Database, "Indexes", TableName);
            DataList = DBExplorer.GetTableData(Database, TableName, long.Parse(RowCount));
        }

        public DataTable ColumnList
        {
            get { return columnList; }
            set
            {
                columnList = value;
                base.OnPropertyChanged("ColumnList");
            }
        }

        public DataTable IndexList
        {
            get { return indexList; }
            set
            {
                indexList = value;
                base.OnPropertyChanged("IndexList");
            }
        }
        
        public DataTable DataList
        {
            get { return dataList; }
            set
            {
                dataList = value;
                base.OnPropertyChanged("DataList");
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand FilterCommand
        {
            get;
            private set;
        }

        public RelayCommand ClearFilterCommand
        {
            get;
            private set;
        }

        public RelayCommand RefreshCommand
        {
            get;
            private set;
        }

        private void OnFilter()
        {
            ItemListView.RowFilter = FilterColumn + " LIKE '%" + FilterString + "%'";
            base.OnPropertyChanged("ItemListView");
            bCanClearFilter = true;
            this.ClearFilterCommand.RaiseCanExecuteChanged();
        }

        private void OnClearFilter()
        {
            ItemListView.RowFilter = "";
            base.OnPropertyChanged("ItemListView");
            bCanClearFilter = false;
            this.ClearFilterCommand.RaiseCanExecuteChanged();
        }

        private void OnRefresh()
        {
            FetchTableDetails();
        }

        private bool CanFilter()
        {
            if (!string.IsNullOrWhiteSpace(FilterColumn) && !string.IsNullOrWhiteSpace(FilterString))
                return true;
            else 
                return false;
        }

        private bool CanClearFilter()
        {
            return bCanClearFilter;
        }

        private bool CanRefresh()
        { return DataList != null; }
    }
}
