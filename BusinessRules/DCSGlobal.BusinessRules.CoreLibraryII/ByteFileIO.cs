using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff; 



namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class ByteFileIO : IDisposable
    {
      //  private System.IO.Directory myDir;

  
        private static StringStuff ss = new StringStuff(); 


        private string _ConnectionString = string.Empty;
        private string _Repository = string.Empty;
        private int _isEncrypted = 0;
        private string _FilePath = string.Empty;
        private string _FileName = string.Empty;
        private string _HOSPCODE = string.Empty;
        Byte[] _FileBytes;





        bool _disposed;


        ~ByteFileIO()
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
            }
        }

        public Byte[] FileBytes
        {
            get
            {
                return _FileBytes;

            }


            set
            {

                _FileBytes = value;
            }
        }


        public string Repository
        {

            set
            {

                _Repository = value;
            }
        }



        public string GetNextFolder(string HOSP_CODE)
        {
            int r = 0;
            string _path = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand("usp_FS_GET_NEXT_FOLDER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@GET_NEXT", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@HOSP_CODE", SqlDbType.VarChar, 255).Value = HOSP_CODE;
                    cmd.ExecuteNonQuery();//
                    _path = Convert.ToString(cmd.Parameters["@FOLDER_NAME"]);
                    r = 0;
                }
            }

            if (r > 0)
            {
                return _path;
            }
            else
            {

                return "bokin";
            }

        }


        public string GetFolder(string HOSP_CODE)
        {
            int r = 0;
            string _path = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand("usp_FS_GET_NEXT_FOLDER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@GET_NEXT", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@HOSP_CODE", SqlDbType.VarChar, 255).Value = HOSP_CODE;
                    cmd.ExecuteNonQuery();//
                    _path = Convert.ToString(cmd.Parameters["@FOLDER_NAME"]);
                    r = 0;
                }
            }

            if (r > 0)
            {
                return _path;
            }
            else
            {

                return "bokin";
            }

        }

        public int GetFolderCount(string FSFolder)
        {

            int r = 0;

            try
            {

                var fileCount = (from file in Directory.EnumerateFiles(FSFolder, "*.*", SearchOption.AllDirectories)
                                 select file).Count();
                r = Convert.ToInt32(fileCount);
            }
            catch (Exception ex)
            {

              //  log.ExceptionDetails("folder count failed for folder " + FSFolder, ex);
                r = -1;
            }

            return r;

        }


        public int GetFileID(string FileName, string HOSP_CODE, string FilePath, int isEncrypted)
        {

            int r = 0;

            // Create new SqlConnection object.
            //
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand("usp_FS_GET_FILE_ID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IS_ENCRYPTED", SqlDbType.Int).Value = isEncrypted;
                    cmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar, 1024).Value = StringEXT.Truncate(FilePath, 255);
                    cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 260).Value = StringEXT.Truncate(FileName, 260);
                    cmd.Parameters.Add("@HOSP_CODE", SqlDbType.VarChar, 255).Value = StringEXT.Truncate(HOSP_CODE, 255);
                    cmd.Parameters.Add("@REPOSITORY", SqlDbType.VarChar, 255).Value = StringEXT.Truncate(_Repository, 255);

                    cmd.ExecuteNonQuery();//
                    // Invoke ExecuteReader method.                                                                     
                    // 
                    r = Convert.ToInt32(cmd.Parameters["@FOLDER_ID"]);
                }
                con.Close();
            }


            return r;

        }


        public int GetFileByID(int FileStoreID)
        {


            int r = 0;

            // Create new SqlConnection object.
            //
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand("usp_FS_GET_FILE_ID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@GETFILE", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@FILE_STORE_ID", SqlDbType.Int).Value = FileStoreID;


                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {
                        if (idr.HasRows)
                        {
                            while (idr.Read())
                            {
                                   _FilePath = Convert.ToString(idr["FILE_PATH"]);
                                   _FileName = Convert.ToString(idr["FILE_NAME"]);
                                   _HOSPCODE = Convert.ToString(idr["HOSP_CODE"]);
                                   _Repository = Convert.ToString(idr["REPOSITORY"]);
                                   _isEncrypted = Convert.ToInt32(idr["IS_ENCRYPTED"]);
                                   r = FileStoreID;
                            }
                        }
                    }

                }
                con.Close();
            }


            return r;



        }


        public void Encypt()
        {

        }


        public void Decrypt()
        {

        }


        public int GetFileDetails(int FileID)
        {
            int r = 0;



            return r;

        }




        public void DeleteFile(string FQFN)
        {

            if (System.IO.File.Exists(FQFN))
            {
                // Use a try block to catch IOExceptions, to 
                // handle the case of the file already being 
                // opened by another process. 
                try
                {
                    System.IO.File.Delete(FQFN);
                }
                catch (System.IO.IOException e)
                {
                    //Console.WriteLine(e.Message);
                    return;
                }
            }

        }


        public void Syncdb()
        {


        }




        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                //Error
                //Console.WriteLine("Exception caught in process: {0}",  _Exception.ToString());
            }

            //error occured, return false
            return false;
        }


        public int SaveDocument(Byte[] DocBinaryArray, string HOSP_CODE)
        {


            int r = 0;
            Guid g;

            g = Guid.NewGuid();

            string FileName = Convert.ToString(g) + ".dm";

            string DOCPATH = string.Empty;


            DOCPATH = _Repository + GetFolder(HOSP_CODE);


            if (GetFolderCount(DOCPATH) == 10000)
            {
                DOCPATH = GetNextFolder(HOSP_CODE);
            }

            r = GetFileID(FileName, HOSP_CODE, DOCPATH, 0);


            if (r > 0)
            {
                try
                {


                    FileStream objfilestream = new FileStream(DOCPATH, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(DocBinaryArray, 0, DocBinaryArray.Length);
                    objfilestream.Close();

                }
                catch (Exception ex)
                {
                    r = -1;
                }


            }
            return r;
        }




      






        public int GetDocumentLen(string DocumentName)
        {
            string strdocPath;
            strdocPath = "C:\\DocumentDirectory\\" + DocumentName;

            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            objfilestream.Close();

            return len;
        }


        public int GetDocument(int DocumentID)
        {
            int r = -1;
            int g = 0;
            string DocumentName = string.Empty;

            g = GetFileByID(DocumentID);

            if (g > 0)
            {


           


            string strdocPath;
            strdocPath = "C:\\DocumentDirectory\\" + DocumentName;


            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] documentcontents = new Byte[len];
            objfilestream.Read(documentcontents, 0, len);
            objfilestream.Close();

            _FileBytes = documentcontents;
            r = DocumentID;
            }




            return r;
        
        }
       


        public int BuildPath(string Path)
        {

            int r = -1;







            return r;
          }




    }
}
