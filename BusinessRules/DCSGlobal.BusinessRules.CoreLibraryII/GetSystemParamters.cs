using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;



namespace DCSGlobal.BusinessRules.CoreLibraryII
{
   public class GetSystemParamters  : IDisposable
    {


   
        private static StringStuff ss = new StringStuff();
        private static string _XMLParameters = string.Empty;
        private static string _ConnectionString = string.Empty;
        private static string _appName = "GetParamtersXML";



        ~GetSystemParamters()
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


       public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
               // log.ConnectionString = value;
            }
        }

       public string AppName
       {

           set
           {
               _appName = value + "." + _appName;
           }
       }


       public string Parameter
       {

           get
           {
               return _XMLParameters;
           }
       }

       


       public int XMLParameters()
       {
           int r = -1;
           try
           {

               using (SqlConnection con = new SqlConnection(_ConnectionString))
               {
                   con.Open();
                   //
                   // Create new SqlCommand object.
                   //
                   using (SqlCommand cmd = new SqlCommand("usp_get_system_preferences", con))
                   {

                       cmd.CommandType = CommandType.StoredProcedure;
                       cmd.Parameters.AddWithValue("@moduleName", "EDIGetResponse");
                       cmd.Parameters.AddWithValue("@lovName ", "_XMLParameters");

                       using (SqlDataReader idr = cmd.ExecuteReader())
                       {
                           //  count = idr.VisibleFieldCount;




                           if (idr.HasRows)
                           {
                               while (idr.Read())
                               {

                                   _XMLParameters = Convert.ToString(idr["lov_value"]);
                                   r = 0;
                               }
                           }
                       }
                   }
               }

           }
           catch (Exception ex)
           {

              // log.ExceptionDetails(_appName, ex);
           }


           return r;

       }

    }
}
