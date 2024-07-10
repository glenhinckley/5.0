using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DCSGlobal.BusinessRules.Logging
{
    public class ParseStackTrace : IDisposable
    {


        bool _disposed;
        private string _ReturnString = string.Empty;


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
        ~ParseStackTrace()
        {
            Dispose(false);
        }


        public ParseStackTrace()
        {

        }


        public string ReturnString
        {

            get
            {

                return _ReturnString; 
            }
        }

        public int Go(Exception ex)
        {

            int r = -1;

            try
            {
                StackTrace st = new StackTrace(ex, true);
                //Get the first stack frame
                StackFrame frame = st.GetFrame(0);
                
                //Get the file name
                string fileName = frame.GetFileName();
               
                //Get the method name
                string methodName = frame.GetMethod().Name;

                //Get the line number from the stack frame
                int line = frame.GetFileLineNumber();

                //Get the column number
                int col = frame.GetFileColumnNumber();


                _ReturnString = fileName + "|" + methodName + "|" + Convert.ToString(line) + "|" + Convert.ToString(col) + "|";


                r = 0;
            }
            catch (Exception exs)
            {
                _ReturnString = exs.Message;
            }

            return r;
        }
    }
}
