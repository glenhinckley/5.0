using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class DataTableMaintance : IDisposable
    {

        ~DataTableMaintance()
        {
            Dispose(false);
        }



        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }



        public DataTable MergeTable(DataTable dtSource, DataTable dtTarget)
        {
       
            foreach (DataRow dr in dtSource.Rows)
            {
                dtTarget.Rows.Add(dr.ItemArray);
            }
           return dtTarget;

        }





    }
}
