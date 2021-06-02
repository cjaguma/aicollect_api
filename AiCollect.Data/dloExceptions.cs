using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class NoTransactionException : Exception
    {
        public NoTransactionException(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; set; }
    }

    public class AnotherTransactionException : Exception
    {
        public AnotherTransactionException(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; set; }
    }

    public class UnSupportedProviderException : Exception
    {
        public UnSupportedProviderException(object sender)
            : base("The specified data provider is unsupported")
        {
            Sender = sender;
        }

        public object Sender { get; set; }
    }

    public class UnsupportedDataRowStateException : Exception
    {
        public object Sender { get; set; }
        public UnsupportedDataRowStateException(object sender)
            : base("Unsupported DataRowState in Update()")
        {
            Sender = sender;
        }
    }

    public class UnsupportedRowStateException : Exception
    {
        public object Sender { get; set; }
        public UnsupportedRowStateException(object sender)
            : base("Unsupported DataRowState")
        {
            Sender = sender;
        }
    }

    public class RecordModifiedConcurrencyException : Exception
    {
       
        public DataRow CurrentDataRow { get; private set; }
        public DataRow ServerDataRow { get; private set; }

        // this will contain the server row 
        private DataTable ServerDataRowTable;

        public RecordModifiedConcurrencyException(DataRow currentDataRow, DataTable serverDataRowTable)
            : base("Record has been modified by another user")
        {
            
            CurrentDataRow = currentDataRow;
            ServerDataRowTable = serverDataRowTable;
            if (ServerDataRowTable.Rows.Count != 0)
                ServerDataRow = ServerDataRowTable.Rows[0];
        }

        //public void Resolve()
        //{
        // //   Resolve(ResolveType.TakeCurrentRecord);
        //}

        //public void Resolve(ResolveType ResolveType)
        //{
        //   // Resolve(ResolveType, true);
        //}

        //public void Resolve(ResolveType ResolveType, bool persist)
        //{
        //    //Record record = RecordSet.Current;
        //    if (ResolveType == ResolveType.TakeServerRecord)
        //    {
        //        DataRow r = this.ServerDataRow;
        //        for (int i = 0; i < r.Table.Columns.Count; i++)
        //        {
        //            CurrentDataRow[i] = r[i];
        //        }

        //    }
        //    else if (ResolveType == ResolveType.TakeCurrentRecord)
        //    {
        //        //logic to resolve a record
        //        //this is also more like force update
        //        //hear we will just change the updateon column to much that if the server then we update

        //        DataRow r = this.ServerDataRow;
        //        for (int i = 0; i < r.Table.Columns.Count; i++)
        //        {
        //            if (r.Table.Columns[i].ColumnName == "Updated_On")
        //            {
        //                CurrentDataRow[i] = r[i];
        //                break;
        //            }
        //        }

        //    }
        //    else if (ResolveType == ResolveType.ForceUpdate)
        //    {
        //        //logic to force update of a record
        //    }

        //    if (persist)
        //        Sender.Update();
        //}

    }

    public class RecordDeletedConcurrencyException : Exception
    {
        public object Sender { get; private set; }
        public DataRow Row { get; private set; }

        public RecordDeletedConcurrencyException(object sender, DataRow row)
            : base("Record has been deleted by another user")
        {
            Sender = sender;
            Row = row;
        }
    }

}
