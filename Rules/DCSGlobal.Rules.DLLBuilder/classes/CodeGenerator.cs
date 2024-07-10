using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;


namespace DCSGlobal.Rules.DLLBuilder.classes
{
   public class CodeGenerator : IDisposable
    {
       
        private VBSKeyWords vbxk = new VBSKeyWords();
        private VBSOperators vbop = new VBSOperators();
        private StringStuff ss = new StringStuff();



        private enum TokenType
        {
            NA = -1,
            Identifier = 0,
            Keyword = 1,
            Separator = 2,
            Operator = 3,
            Literal = 4,
            Comment = 5,
            Parameter = 6

        }

        private string EOT = Convert.ToString((char)3);
        private string EOL = Convert.ToString((char)4);

        private string _RegexName = string.Empty;

        bool _disposed;


        private List<string> _Code = new List<string>();
        private List<varField> _varField = new List<varField>();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CodeGenerator()
        {
            Dispose(false);
        }




        public CodeGenerator()
        {

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







    }
}
