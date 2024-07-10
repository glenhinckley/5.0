using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;



using DCSGlobal.BusinessRules.Logging;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class BuildAssemballyFromString : IDisposable
    {

        private logExecption log = new logExecption();
        bool _disposed;


        private string _ConnectionString = string.Empty;
        private string _FileName = Convert.ToString(Guid.NewGuid());
        private string _Path = string.Empty;
        private bool _Test = false;
        private List<string> _ErrorList = new List<string>();
        private List<string> _ReferencedAssemblieDLLs = new List<string>();
        private string _AssemblyPath = @"c:\usr\ass\";
        private string _OutputAssembly = string.Empty;

        public BuildAssemballyFromString(string AssemblyPath)
        {

            _ReferencedAssemblieDLLs.Add("mscorlib.dll");
            _ReferencedAssemblieDLLs.Add("System.dll");
            _ReferencedAssemblieDLLs.Add("System.Text.RegularExpressions.dll");
            _ReferencedAssemblieDLLs.Add("c:\\usr\\ass\\System.Data.dll");
            //  _ReferencedAssemblieDLLs.Add("c:\\usr\\ass\\System.Data.SqlClient.dll");
            _ReferencedAssemblieDLLs.Add("System.IO.dll");
            _ReferencedAssemblieDLLs.Add("System.Collections.dll");
            _ReferencedAssemblieDLLs.Add("System.Text.dll");
            _ReferencedAssemblieDLLs.Add("c:\\usr\\ass\\System.Xml.dll");
            _ReferencedAssemblieDLLs.Add("c:\\usr\\ass\\System.Xml.XPath.dll");


        }




        public BuildAssemballyFromString()
        {

            _ReferencedAssemblieDLLs.Add("mscorlib.dll");
            _ReferencedAssemblieDLLs.Add("System.dll");
            _ReferencedAssemblieDLLs.Add("System.Text.RegularExpressions.dll");
            _ReferencedAssemblieDLLs.Add(_AssemblyPath + "System.Data.dll");
            //  _ReferencedAssemblieDLLs.Add("c:\\usr\\ass\\System.Data.SqlClient.dll");
            _ReferencedAssemblieDLLs.Add(_AssemblyPath + "System.IO.dll");
            _ReferencedAssemblieDLLs.Add(_AssemblyPath + "System.Collections.dll");
            //  _ReferencedAssemblieDLLs.Add("System.Text.dll");
            _ReferencedAssemblieDLLs.Add(_AssemblyPath + "System.Xml.dll");
            _ReferencedAssemblieDLLs.Add(_AssemblyPath + "System.Xml.XPath.dll");


        }


        ~BuildAssemballyFromString()
        {
            Dispose(false);
        }





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
                log.ConnectionString = value;
            }
        }


        public List<string> ErrorList
        {
            get
            {
                return _ErrorList;
            }
        }


        public List<string> ReferencedAssemblieDLLs
        {
            set
            {
                _ReferencedAssemblieDLLs = value;
            }
        }


        public string Path
        {
            set
            {
                _Path = value;
            }
        }


        public string OutputAssembly
        {

            get 
            {
                return _OutputAssembly;
            }
            

        }

        

        public bool Test
        {
            set
            {
                _Test = value;
            }
        }

        public string FileName
        {

            get
            {

                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        public bool Go(string Code)
        {

            bool r = true;


            try
            {
                Microsoft.VisualBasic.VBCodeProvider vbcp = new Microsoft.VisualBasic.VBCodeProvider();
                CompilerParameters cParams = new CompilerParameters();
                cParams.GenerateExecutable = false;
                //  cParams.CompilerOptions = "/c:library";


                _OutputAssembly = _Path + _FileName + ".dll";


                if (!_Test)
                {
                    cParams.OutputAssembly = _OutputAssembly; //_Path + _FileName + ".dll";
                    cParams.GenerateInMemory = false;
                }
                else
                {
                    cParams.GenerateInMemory = true;
                }

                foreach (string DLL in _ReferencedAssemblieDLLs)
                {
                    cParams.ReferencedAssemblies.Add(DLL);

                }

                CompilerResults cResults = vbcp.CompileAssemblyFromSource(cParams, Code);


                // Check for errors
                if (cResults.Errors.Count != 0)
                {
                    foreach (var er in cResults.Errors)
                    {
                        r = false;
                        _ErrorList.Add(er.ToString());
                    }
                }
                else
                {
                    // Attempt to execute method.
                    // object obj = cResults.CompiledAssembly.CreateInstance("WinFormCodeCompile.Transform");
                    // Type t = obj.GetType();
                    //   object[] arg = { this.textBox1 }; // Pass our textbox to the method
                    // t.InvokeMember("UpdateText", BindingFlags.InvokeMethod, null, obj, arg);
                }

            }
            catch (Exception ex)
            {
                log.ExceptionDetails("Compiler", ex);

            }


            return r;


        }
    }
}

