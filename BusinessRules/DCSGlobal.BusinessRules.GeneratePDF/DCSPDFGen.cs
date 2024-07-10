using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
//using System.Web.SessionState;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using System.Xml;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using GenCode128;
using QRCode.Codec;
using QRCode.Codec.Data;
using QRCode.Codec.Util;
using DCSGlobal.BusinessRules.Logging;

namespace DCSGlobal.BusinessRules.GeneratePDF
{
    public class DCSPDFGen : IDisposable //System.Web.UI.Page
    {

        string _ConnectionString = string.Empty;

        public string fname = string.Empty;
        private string Bnodex = string.Empty;
        private string Bnodey = string.Empty;
        private string BFontName = string.Empty;
        private string BFontColor = string.Empty;
        private string BFontSize = string.Empty;
        private string Bbackcolor = string.Empty;
        private string BIsBold = string.Empty;
        private string Bbartextsp = string.Empty;

        private string Bbarprefix = string.Empty;
        private string Inodex = string.Empty;
        private string Inodey = string.Empty;
        private string ISizeRatio = string.Empty;

        private string IImageLocation = string.Empty;
        private string Bbarsufix = string.Empty;
        private string BSizeRatio = string.Empty;
        private string BImgWidth = string.Empty;
        private string BImgHeight = string.Empty;

        private string BRotateFlip = string.Empty;
        private string BQRScale = string.Empty;
        private string BQRVersion = string.Empty;
        private string BHR = string.Empty;

        private string BBarWeight = string.Empty;

 

        private string _PdfDBProc = string.Empty;

        logExecption log = new logExecption();

        bool _disposed;

        ~DCSPDFGen()
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
                log.Dispose(); 
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

         
        public string PdfDBProc
        {
            set
            {


                _PdfDBProc = value;
            }
        }

        
        public string ConnectionString
        {
            set
            {

                log.ConnectionString = _ConnectionString;
                _ConnectionString = value;
            }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_doc_id"></param>
        /// <param name="BatchID"></param>
        /// <param name="fileName"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string createPDFFile(string _doc_id, string BatchID , string fileName, string userid)
        {
            fname = fileName;
            string sqlString = string.Empty;
            string str = string.Empty;
            string _pid = _doc_id;
            string _pathospid = BatchID;
            
             
            System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection();
            SqlCommand Command = null;
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                string pdffile = string.Empty;
                string xmlfilecontent = string.Empty;
                string fontname = null;
                string fontisbold = null;
                string fontsize = null;
                string fontcolor = null;
                string x = null;
                string y = null;
                string value = null;

                sqlString = _PdfDBProc;  //"usp_get_pdf_final_data_xml";
                SqlDataReader myDataReader = null;

                myConnection.ConnectionString = _ConnectionString;
                myConnection.Open();

                Command = new SqlCommand(sqlString, myConnection);
                //Get DB Stored procedure name from config
                Command.CommandType = CommandType.StoredProcedure;
                //Command.CommandTimeout = Convert.ToInt32("180");

                Command.Parameters.AddWithValue("@pdfparam", SqlDbType.VarChar).Value  = _pid;
                //Command.Parameters["@pdfparam"].Direction = ParameterDirection.Input;
                //Command.Parameters["@pdfparam"].Value = _pid;

                Command.Parameters.AddWithValue("@pathospid", SqlDbType.VarChar).Value = BatchID;

                //Command.Parameters.Add("@pathospid", SqlDbType.VarChar);
                //Command.Parameters["@pathospid"].Direction = ParameterDirection.Input;
                //Command.Parameters["@pathospid"].Value = BatchID;

                //Command.Parameters.Add("@struser", SqlDbType.VarChar);
                //Command.Parameters["@struser"].Direction = ParameterDirection.Input;
                //Command.Parameters["@struser"].Value = userid;

                //Command.Parameters.Add("@hosp_code", SqlDbType.VarChar);
                //Command.Parameters["@hosp_code"].Direction = ParameterDirection.Input;
                //Command.Parameters["@hosp_code"].Value = _pat_hosp_code;

                myDataReader = Command.ExecuteReader();

                while (myDataReader.Read())
                {
                    if (myDataReader.GetValue(myDataReader.GetOrdinal("pdffilename")) != System.DBNull.Value) 
                    {
                        //pdffile = myDataReader.GetValue(myDataReader.GetOrdinal("pdffilename")).ToString().Trim();
                        pdffile = fileName;
                        xmlfilecontent = myDataReader.GetValue(myDataReader.GetOrdinal("xmlcontent")).ToString().Trim();
                    }
                }

                if ((xmlfilecontent.Trim().Length > 0))
                {
                    xmldoc.LoadXml(xmlfilecontent);
                }

                // PDF file name comes from db  //System.Web.Hosting.HostingEnvironment.MapPath(path);
                //PdfDocument inputdoc = PdfReader.Open(Server.MapPath(pdffile.ToString()), PdfDocumentOpenMode.Import);
                //PdfDocument inputdoc = PdfReader.Open(System.Web.Hosting.HostingEnvironment.MapPath(pdffile.ToString()), PdfDocumentOpenMode.Import);
                PdfDocument inputdoc = PdfReader.Open(pdffile.ToString(), PdfDocumentOpenMode.Import);

                int totalPages = inputdoc.PageCount;
                int _pageCount = 0;
                MemoryStream stream = new MemoryStream();
                PdfDocument document = new PdfDocument();


                try
                {
                    while ((_pageCount < totalPages))
                    {
                        PdfPage inputpage = inputdoc.Pages[_pageCount];
                        PdfPage page = document.AddPage(inputpage);

                        if ((xmlfilecontent.Trim().Length > 0))
                        {
                            try
                            {
                                //Begin Add Barcode     
                                GenerateBarCode(ref page, Convert.ToInt32(_pageCount), _pid, _pathospid, userid);
                                //End Add Barcode
                            }
                            catch (System.Exception ex)
                            {
                                log.ExceptionDetails(ex.Message, ex.StackTrace);
                            }

                            try
                            {
                                //Begin Add Image Files     
                                EmbedImages(ref page, Convert.ToInt32(_pageCount), _pid, _pathospid, userid);
                                //End Add Image Files 

                            }
                            catch (System.Exception ex)
                            {
                                log.ExceptionDetails(ex.Message, ex.StackTrace);
                            }

                            string nodeName = null;
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                            nodeName = "//page" + (_pageCount + 1).ToString() + "/node";



                            foreach (XmlNode node in xmldoc.DocumentElement.SelectNodes(nodeName))
                            {
                                x = node.Attributes["x"].InnerText;
                                y = node.Attributes["y"].InnerText;
                                value = node.SelectSingleNode("value").InnerText;
                                fontname = node.SelectSingleNode("font").InnerText;
                                fontcolor = node.SelectSingleNode("font").Attributes["color"].InnerText;
                                fontsize = node.SelectSingleNode("font").Attributes["size"].InnerText;
                                fontisbold = node.SelectSingleNode("font").Attributes["isbold"].InnerText;

                                if (value.Trim().Length > 0)
                                {
                                    XFont font = new XFont(fontname, Convert.ToDouble(fontsize), XFontStyle.Regular, options);

                                    if (fontisbold.ToLower() == "y")
                                    {
                                        font = new XFont(fontname, Convert.ToDouble(fontsize), XFontStyle.Bold, options);
                                    }

                                    gfx.DrawString(value.ToString(), font, getColor(fontcolor), new XRect(Convert.ToDouble(x), Convert.ToDouble(y), Convert.ToDouble(page.Width), Convert.ToDouble(page.Height)), XStringFormats.TopLeft);

                                }
                            }
                        }
                        _pageCount = _pageCount + 1;
                    }
                    //document.Save((@"c:/usr/temp/abc.pdf"));

                    document.Save(fname);
                }
                catch (System.Exception ex)
                {
                    //Global.logExceptionGeneric(ex.Message, ex.StackTrace);
                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                Command = null;
            }
            return str;
        }

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="pat_hosp_code"></param>
        /// <param name="fileName"></param>
        /// <param name="userid"></param>
        /// <param name="docpagesize"></param>
        /// <returns></returns>
        public string addJPGToPDFFile(string doc_id, string pat_hosp_code, string fileName, string userid, string docpagesize)
        {
            fname = fileName;
            string str = string.Empty;
            string _pid = doc_id;
            string _pathospid = pat_hosp_code;
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                string pdffile = fileName.Replace("_1.jpg", ".pdf");

                int Onodex = 0;
                int Onodey = 0;
                string OSizeRatio = string.Empty;
                string OImgWidth = string.Empty;
                string OImgHeight = string.Empty;

                string OimageFile = string.Empty;


                // PDF file name comes from db
                PdfDocument inputdoc = PdfReader.Open(pdffile.ToString(), PdfDocumentOpenMode.Import);
                int totalPages = inputdoc.PageCount;
                int _pageCount = 0;
                MemoryStream stream = new MemoryStream();
                PdfDocument document = new PdfDocument();


                try
                {
                    while ((_pageCount < totalPages))
                    {
                        PdfPage inputpage = inputdoc.Pages[_pageCount];
                        PdfPage page = document.AddPage(inputpage);

                        // Add ID Image file to PDF

                        Onodex = 100;
                        Onodey = 350;
                        OimageFile = fileName;
                        OSizeRatio = "6";
                        OImgWidth = "400";
                        OImgHeight = "400";

                        try
                        {
                            int SizeRatio = Convert.ToInt32(OSizeRatio);
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XImage image = XImage.FromFile(OimageFile);
                            double width = image.PixelWidth * Convert.ToInt32(OImgWidth) / image.HorizontalResolution;
                            double height = image.PixelHeight * Convert.ToInt32(OImgHeight) / image.HorizontalResolution;
                            gfx.DrawImage(image, Onodex, Onodey, width / Convert.ToInt32(OSizeRatio), height / Convert.ToInt32(OSizeRatio));
                            gfx.Dispose();
                            gfx = null;
                            image.Dispose();
                        }
                        catch (System.Exception ex)
                        {
                            string sLoginID = string.Empty;

                            if (HttpContext.Current.Session["sUserID"] != null && !string.IsNullOrEmpty((string)HttpContext.Current.Session["sUserID"]))
                            //if (Session["sUserID"] != null && !string.IsNullOrEmpty((string)Session["sUserID"]))
                            {
                                sLoginID = (string)HttpContext.Current.Session["sUserID"];
                            }

                            log.ExceptionDetails(ex.Message, ex.StackTrace);
                        }


                        _pageCount = _pageCount + 1;
                    }

                    fname = fname.Replace(".jpg", ".pdf");

                    document.Save(fname);
                }
                catch (System.Exception ex)
                {
                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                }
                finally
                {
                    //Response.Flush();
                    stream.Close();
                    document.Close();
                    document.Dispose();
                    stream.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            return str;
        }
        
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="pat_hosp_code"></param>
        /// <param name="fileName"></param>
        /// <param name="userid"></param>
        /// <param name="docpagesize"></param>
        /// <param name="addbarcodes"></param>
        /// <returns></returns>
        public string addBarcodesJPGToPDFFile(string doc_id, string pat_hosp_code, string fileName, string userid, string docpagesize, string addbarcodes)
        {
            fname = fileName;

            XmlDataDocument xmldoc = new XmlDataDocument();
            string pdffile = string.Empty;
            string xmlfilecontent = string.Empty;
            string fontname = null;
            string fontisbold = null;
            string fontsize = null;
            string fontcolor = null;
            string x = null;
            string y = null;
            string value = null;

            string sqlString = string.Empty;
            string str = string.Empty;
            string _pid = doc_id;
            string _pathospid = pat_hosp_code;
            try
            {
                System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection();
                SqlCommand Command = null;
                SqlDataReader myDataReader = null;

                myConnection.ConnectionString = _ConnectionString;
                myConnection.Open();
                sqlString = "usp_get_pdf_final_data_xml";
                Command = new SqlCommand(sqlString, myConnection);
                //Get DB Stored procedure name from config
                Command.CommandType = CommandType.StoredProcedure;
                //Command.CommandTimeout = Convert.ToInt32("180");

                Command.Parameters.Add("@pdfparam", SqlDbType.VarChar);
                Command.Parameters["@pdfparam"].Direction = ParameterDirection.Input;
                Command.Parameters["@pdfparam"].Value = _pid;

                Command.Parameters.Add("@pathospid", SqlDbType.VarChar);
                Command.Parameters["@pathospid"].Direction = ParameterDirection.Input;
                Command.Parameters["@pathospid"].Value = _pathospid;

                myDataReader = Command.ExecuteReader();

                while (myDataReader.Read())
                {
                    
                    if (myDataReader.GetValue(myDataReader.GetOrdinal("pdffilename")) != System.DBNull.Value) 
                    {
                        pdffile = myDataReader.GetValue(myDataReader.GetOrdinal("pdffilename")).ToString().Trim();

                        xmlfilecontent = myDataReader.GetValue(myDataReader.GetOrdinal("xmlcontent")).ToString().Trim();
                    }
                }


                int Onodex = 0;
                int Onodey = 0;
                string OSizeRatio = string.Empty;
                string OImgWidth = string.Empty;
                string OImgHeight = string.Empty;

                string OimageFile = string.Empty;

                if ((xmlfilecontent.Trim().Length > 0))
                {
                    xmldoc.LoadXml(xmlfilecontent);
                }
                // PDF file name comes from db
                PdfDocument inputdoc = PdfReader.Open(pdffile.ToString(), PdfDocumentOpenMode.Import);
                int totalPages = inputdoc.PageCount;
                int _pageCount = 0;
                MemoryStream stream = new MemoryStream();
                PdfDocument document = new PdfDocument();
                
                // 

               // mohan set the page size
              /// PageSize.Letter  


                try
                {
                    while ((_pageCount < totalPages))
                    {
                        PdfPage inputpage = inputdoc.Pages[_pageCount];
                        PdfPage page = document.AddPage(inputpage);


                        if ((xmlfilecontent.Trim().Length > 0))
                        {
                            if (addbarcodes == "Y")
                            {
                                //Begin Add Barcode     
                                //End Add Barcode
                                GenerateBarCode(ref page, Convert.ToInt32(_pageCount), _pid, _pathospid, userid);
                            }

                            string nodeName = null;
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                            nodeName = "//page" + (_pageCount + 1).ToString() + "/node";

                            ////page1/node

                            foreach (XmlNode node in xmldoc.DocumentElement.SelectNodes(nodeName))
                            {
                                x = node.Attributes["x"].InnerText;
                                y = node.Attributes["y"].InnerText;
                                value = node.SelectSingleNode("value").InnerText;
                                fontname = node.SelectSingleNode("font").InnerText;
                                fontcolor = node.SelectSingleNode("font").Attributes["color"].InnerText;
                                fontsize = node.SelectSingleNode("font").Attributes["size"].InnerText;
                                fontisbold = node.SelectSingleNode("font").Attributes["isbold"].InnerText;

                                if (value.Trim().Length > 0)
                                {
                                    XFont font = new XFont(fontname, Convert.ToDouble(fontsize), XFontStyle.Regular, options);

                                    if (fontisbold.ToLower() == "y")
                                    {
                                        font = new XFont(fontname, Convert.ToDouble(fontsize), XFontStyle.Bold, options);
                                    }

                                    gfx.DrawString(value.ToString(), font, getColor(fontcolor), new XRect(Convert.ToDouble(x), Convert.ToDouble(y), Convert.ToDouble(page.Width), Convert.ToDouble(page.Height)), XStringFormats.TopLeft);

                                }
                            }

                            // Add ID Image file to PDF

                            Onodex = 100;
                            Onodey = 110;
                            OimageFile = fileName;
                            OSizeRatio = "6";
                            OImgWidth = "400";
                            OImgHeight = "400";

                            try
                            {
                                int SizeRatio = Convert.ToInt32(OSizeRatio);
                                // XGraphics gfx = XGraphics.FromPdfPage(page);
                                XImage image = XImage.FromFile(OimageFile);
                                double width = image.PixelWidth * Convert.ToInt32(OImgWidth) / image.HorizontalResolution;
                                double height = image.PixelHeight * Convert.ToInt32(OImgHeight) / image.HorizontalResolution;
                                gfx.DrawImage(image, Onodex, Onodey, width / Convert.ToInt32(OSizeRatio), height / Convert.ToInt32(OSizeRatio));
                                gfx.Dispose();
                                gfx = null;
                                image.Dispose();
                            }
                            catch (System.Exception ex)
                            {
                                string sLoginID = string.Empty;
                                if (HttpContext.Current.Session["sUserID"] != null && !string.IsNullOrEmpty((string)HttpContext.Current.Session["sUserID"]))
                                {
                                    sLoginID = (string)HttpContext.Current.Session["sUserID"];
                                }

                                log.ExceptionDetails(ex.Message, ex.StackTrace);
                            }

                        }
                        _pageCount = _pageCount + 1;
                    }

                    fname = fname.Replace(".jpg", ".pdf");


                    document.Save(fname);

                }
                catch (System.Exception ex)
                {
                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                }
                finally
                {
                    stream.Close();
                    document.Dispose();
                    document.Close();
                    stream.Dispose();
                }

            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            return str;
        }

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="pat_hosp_code"></param>
        /// <param name="fileName"></param>
        /// <param name="userid"></param>
        /// <param name="docpagesize"></param>
        /// <param name="addbarcodes"></param>
        /// <returns></returns>
        public string addBarcodesToPDFFile(string doc_id, string pat_hosp_code, string fileName, string userid, string docpagesize, string addbarcodes)
        {
            fname = fileName;
            string str = string.Empty;
            string _pid = doc_id;
            string _pathospid = pat_hosp_code;
            try
            {
                string pdffile = fileName;

                PdfDocument inputdoc = PdfReader.Open(pdffile.ToString(), PdfDocumentOpenMode.Import);
                int totalPages = inputdoc.PageCount;
                int _pageCount = 0;
                MemoryStream stream = new MemoryStream();
                PdfDocument document = new PdfDocument();


                try
                {
                    while ((_pageCount < totalPages))
                    {
                        PdfPage inputpage = inputdoc.Pages[_pageCount];
                        PdfPage page = document.AddPage(inputpage);

                        if (addbarcodes == "Y")
                        {
                            //Begin Add Barcode     
                            //End Add Barcode
                            GenerateBarCode(ref page, Convert.ToInt32(_pageCount), _pid, pat_hosp_code, userid);
                        }

                        _pageCount = _pageCount + 1;
                    }


                    document.Save(fname);

                }
                catch (System.Exception ex)
                {

                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                }
                finally
                {
                    stream.Close();
                    document.Close();
                    document.Dispose();
                    stream.Dispose();
                }

            }
            catch (System.Exception ex)
            {

                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            return str;
        }

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BarCodeText"></param>
        /// <param name="prefix"></param>
        /// <param name="sufix"></param>
        /// <param name="fname"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="RotateFlip"></param>
        /// <returns></returns>
        private string GenerateNewBarcodeImgNew(string BarCodeText, string prefix, string sufix, string fname, string fontName, int fontSize, int width, int height, int RotateFlip)
        {

            try
            {
                // Multiply the lenght of the code by 40 (just to have enough width)
                int w = BarCodeText.Length * width;

                // Create a bitmap object of the width that we calculated and height of 100
                Bitmap oBitmap = new Bitmap(w, height);

                // then create a Graphic object for the bitmap we just created.
                Graphics oGraphics = Graphics.FromImage(oBitmap);

                // Now create a Font object for the Barcode Font
                // (in this case the IDAutomationHC39M) of 18 point size
                Font oFont = new Font(fontName, fontSize, FontStyle.Regular);

                // Let's create the Point and Brushes for the barcode
                PointF oPoint = new PointF(2f, 2f);
                SolidBrush oBrushWrite = new SolidBrush(System.Drawing.Color.Black);
                SolidBrush oBrush = new SolidBrush(System.Drawing.Color.White);

                // Now lets create the actual barcode image
                // with a rectangle filled with white color
                oGraphics.FillRectangle(oBrush, 0, 0, w, height);


                // We have to put prefix and sufix of an asterisk (*),
                // in order to be a valid barcode
                oGraphics.DrawString(prefix + BarCodeText + sufix, oFont, oBrushWrite, oPoint);

                //Dim BarCodeFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "temp\" & Guid.NewGuid().ToString() & ".jpg"
                //oBitmap.Save(BarCodeFile, ImageFormat.Jpeg)

                oBitmap.RotateFlip((RotateFlipType)RotateFlip);
                oBitmap.Save(fname, ImageFormat.Jpeg);
                oGraphics.Dispose();

                oGraphics = null;
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }

            return fname;
        }
       
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="imgLocation"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="SizeRatio"></param>
        /// <param name="userid"></param>
        private void AddImageFile(ref PdfPage page, string imgLocation, int x, int y, int SizeRatio, string userid)
        {
            try
            {
                XGraphics gfx = XGraphics.FromPdfPage(page);
                try
                {
                    XImage image = XImage.FromFile(imgLocation);
                    double width = image.PixelWidth * 72 / image.HorizontalResolution;
                    double height = image.PixelHeight * 72 / image.HorizontalResolution;
                    gfx.DrawImage(image, x, y, width / SizeRatio, height / SizeRatio);
                    gfx.Dispose();
                    gfx = null;
                    image.Dispose();
                }
                catch (System.Exception ex)
                {
                    gfx.Dispose();

                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                }

            }
            catch (System.Exception ex)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="imgLocation"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="SizeRatio"></param>
        /// <param name="userid"></param>
        private void AddBarcode(ref PdfPage page, string imgLocation, int x, int y, int SizeRatio, string userid)
        {
            try
            {
                //PdfSharp.Drawing.BarCodes.TextLocation
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XImage image = XImage.FromFile(imgLocation);
                double width = image.PixelWidth * 72 / image.HorizontalResolution;
                double height = image.PixelHeight * 72 / image.HorizontalResolution;
                gfx.DrawImage(image, x, y, width / SizeRatio, height / SizeRatio);
                gfx.Dispose();
                gfx = null;
                image.Dispose();
                try
                {
                    //System.IO.File.Delete(Server.MapPath(fname & ".jpg"))
                    System.IO.File.Delete(imgLocation);
                }
                catch (System.Exception ex)
                {
                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }

        }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagecount"></param>
        /// <param name="pid"></param>
        /// <param name="pathospid"></param>
        /// <param name="userid"></param>
        private void EmbedImages(ref PdfPage page, int pagecount, string pid, string pathospid, string userid)
        {
            try
            {
                pagecount = pagecount + 1;
                if ((getPdfImageDetails(ref page, pid, pagecount, pathospid, userid)))
                {
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }

        }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pdfID"></param>
        /// <param name="pagecount"></param>
        /// <param name="pathospid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool getPdfImageDetails(ref PdfPage page, string pdfID, int pagecount, string pathospid, string userid)
        {
            SqlConnection ruleConn = new SqlConnection();
            SqlDataReader myDataReader = null;
            SqlCommand ruleComm = null;
            string sqlString = null;
            int _i = 0;

            bool f = false;

            f = false;

            try
            {
                ruleConn.ConnectionString = _ConnectionString;
                ruleConn.Open();

                sqlString = "usp_get_pdf_image_details";
                ruleComm = new SqlCommand(sqlString, ruleConn);

                ruleComm.CommandType = CommandType.StoredProcedure;
                ruleComm.CommandTimeout = Convert.ToInt32("180");

                ruleComm.Parameters.Add("@pdfid", SqlDbType.VarChar);
                ruleComm.Parameters["@pdfid"].Direction = ParameterDirection.Input;
                ruleComm.Parameters["@pdfid"].Value = pdfID;

                ruleComm.Parameters.Add("@pagecount", SqlDbType.VarChar);
                ruleComm.Parameters["@pagecount"].Direction = ParameterDirection.Input;
                ruleComm.Parameters["@pagecount"].Value = pagecount;

                ruleComm.Parameters.Add("@pathospid", SqlDbType.VarChar);
                ruleComm.Parameters["@pathospid"].Direction = ParameterDirection.Input;
                ruleComm.Parameters["@pathospid"].Value = pathospid;

                myDataReader = ruleComm.ExecuteReader();

                while (myDataReader.Read())
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(myDataReader.GetValue(myDataReader.GetOrdinal("id")))))
                    {
                        Inodex = myDataReader.GetValue(myDataReader.GetOrdinal("nodex")).ToString().Trim();
                        Inodey = myDataReader.GetValue(myDataReader.GetOrdinal("nodey")).ToString().Trim();
                        IImageLocation = myDataReader.GetValue(myDataReader.GetOrdinal("imglocation")).ToString().Trim();
                        ISizeRatio = myDataReader.GetValue(myDataReader.GetOrdinal("SizeRatio")).ToString().Trim();
                        string imageFile = string.Empty;

                        imageFile = IImageLocation;

                        AddImageFile(ref page, imageFile, Convert.ToInt32(Inodex), Convert.ToInt32(Inodey), Convert.ToInt32(ISizeRatio), userid);

                        _i = _i + 1;


                        f = true;
                    }

                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            finally
            {
                ruleComm = null;
                ruleConn.Close();
                ruleConn = null;
            }
            return f;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagecount"></param>
        /// <param name="pid"></param>
        /// <param name="pathospid"></param>
        /// <param name="userid"></param>
        private void GenerateBarCode(ref PdfPage page, int pagecount, string pid, string pathospid, string userid)
        {
            try
            {
                pagecount = pagecount + 1;
                // Dim barcodeFile As String = o.GenerateNewBarcodeImgNew(Bbartextsp, Bbarprefix, Bbarsufix, fname & ".jpg", BFontName, BFontSize, BImgWidth, BImgHeight, BRotateFlip)
                // AddBarcode(page, barcodeFile, Bnodex, Bnodey, BSizeRatio)
                if ((getPdfBarcodeDetails(ref page, pid, pagecount, pathospid, userid)))
                {
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }

        }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pdfID"></param>
        /// <param name="pagecount"></param>
        /// <param name="pathospid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool getPdfBarcodeDetails(ref PdfPage page, string pdfID, int pagecount, string pathospid, string userid)
        {
            SqlConnection ruleConn = new SqlConnection();
            SqlDataReader myDataReader = null;
            SqlCommand ruleComm = null;
            string sqlString = null;
            int _i = 0;

            bool f = false;

            f = false;

            try
            {
                ruleConn.ConnectionString = _ConnectionString;
                ruleConn.Open();

                sqlString = "usp_get_pdf_barcode_details";
                ruleComm = new SqlCommand(sqlString, ruleConn);

                ruleComm.CommandType = CommandType.StoredProcedure;
                ruleComm.CommandTimeout = Convert.ToInt32("180");

                ruleComm.Parameters.Add("@pdfid", SqlDbType.VarChar);
                ruleComm.Parameters["@pdfid"].Direction = ParameterDirection.Input;
                ruleComm.Parameters["@pdfid"].Value = pdfID;

                ruleComm.Parameters.Add("@pagecount", SqlDbType.VarChar);
                ruleComm.Parameters["@pagecount"].Direction = ParameterDirection.Input;
                ruleComm.Parameters["@pagecount"].Value = pagecount;

                ruleComm.Parameters.Add("@pathospid", SqlDbType.VarChar);
                ruleComm.Parameters["@pathospid"].Direction = ParameterDirection.Input;
                ruleComm.Parameters["@pathospid"].Value = pathospid;

                myDataReader = ruleComm.ExecuteReader();

                while (myDataReader.Read())
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(myDataReader.GetValue(myDataReader.GetOrdinal("id")))))
                    {
                        Bnodex = myDataReader.GetValue(myDataReader.GetOrdinal("nodex")).ToString().Trim();
                        Bnodey = myDataReader.GetValue(myDataReader.GetOrdinal("nodey")).ToString().Trim();
                        BFontName = myDataReader.GetValue(myDataReader.GetOrdinal("fontname")).ToString().Trim();
                        BFontColor = myDataReader.GetValue(myDataReader.GetOrdinal("fontcolor")).ToString().Trim();
                        BFontSize = myDataReader.GetValue(myDataReader.GetOrdinal("FontSize")).ToString().Trim();
                        Bbackcolor = myDataReader.GetValue(myDataReader.GetOrdinal("backcolor")).ToString().Trim();
                        BIsBold = myDataReader.GetValue(myDataReader.GetOrdinal("isbold")).ToString().Trim();
                        Bbartextsp = myDataReader.GetValue(myDataReader.GetOrdinal("bartext")).ToString().Trim();
                        Bbarprefix = myDataReader.GetValue(myDataReader.GetOrdinal("barprefix")).ToString().Trim();
                        Bbarsufix = myDataReader.GetValue(myDataReader.GetOrdinal("barsufix")).ToString().Trim();
                        BSizeRatio = myDataReader.GetValue(myDataReader.GetOrdinal("SizeRatio")).ToString().Trim();
                        BImgWidth = myDataReader.GetValue(myDataReader.GetOrdinal("imgwidth")).ToString().Trim();
                        BImgHeight = myDataReader.GetValue(myDataReader.GetOrdinal("imgheight")).ToString().Trim();
                        BRotateFlip = myDataReader.GetValue(myDataReader.GetOrdinal("RotateFlip")).ToString().Trim();
                        BQRScale = myDataReader.GetValue(myDataReader.GetOrdinal("scale")).ToString().Trim();
                        BQRVersion = myDataReader.GetValue(myDataReader.GetOrdinal("version")).ToString().Trim();
                        BHR = myDataReader.GetValue(myDataReader.GetOrdinal("hr")).ToString().Trim();
                        BBarWeight = myDataReader.GetValue(myDataReader.GetOrdinal("barweight")).ToString().Trim();

                        string barcodeFile = string.Empty;

                        if (BFontName == "CODE128")
                        {
                            if (BHR == "Y")
                            {
                                barcodeFile = GenerateNewCODE128ImgHR(Bbartextsp, Bbarprefix, Bbarsufix, fname + "_" + (_i).ToString() + ".jpg", BFontName, int.Parse(BFontSize), int.Parse(BImgWidth), int.Parse(BImgHeight), int.Parse(BRotateFlip), int.Parse(BBarWeight));
                            }
                            else
                            {
                                barcodeFile = GenerateNewCODE128Img(Bbartextsp, Bbarprefix, Bbarsufix, fname + "_" + (_i).ToString() + ".jpg", BFontName, int.Parse(BFontSize), int.Parse(BImgWidth), int.Parse(BImgHeight), int.Parse(BRotateFlip), int.Parse(BBarWeight));

                            }
                        }
                        else if (BFontName == "QRCODE")
                        {
                            barcodeFile = GenerateNewQRCode(Bbartextsp, Bbarprefix, Bbarsufix, fname + "_" + (_i).ToString() + ".jpg", BFontName, int.Parse(BFontSize), int.Parse(BImgWidth), int.Parse(BImgHeight), int.Parse(BRotateFlip), int.Parse(BQRScale),
                            int.Parse(BQRVersion));
                        }
                        else
                        {
                            //' CODE39
                            barcodeFile = GenerateNewBarcodeImgNew(Bbartextsp, Bbarprefix, Bbarsufix, fname + "_" + (_i).ToString() + ".jpg", BFontName, int.Parse(BFontSize), int.Parse(BImgWidth), int.Parse(BImgHeight), int.Parse(BRotateFlip));
                        }

                        //string barcodeFile = GenerateNewBarcodeImgNew(Bbartextsp, Bbarprefix, Bbarsufix, fname + "_" + Convert.ToString(_i) + ".jpg", BFontName, Convert.ToInt32(BFontSize), Convert.ToInt32(BImgWidth), Convert.ToInt32(BImgHeight), Convert.ToInt32(BRotateFlip));
                        AddBarcode(ref page, barcodeFile, Convert.ToInt32(Bnodex), Convert.ToInt32(Bnodey), Convert.ToInt32(BSizeRatio), userid);

                        _i = _i + 1;


                        f = true;
                    }

                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            finally
            {
                ruleComm = null;
                ruleConn.Close();
                ruleConn = null;
            }
            return f;

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BarCodeText"></param>
        /// <param name="prefix"></param>
        /// <param name="sufix"></param>
        /// <param name="fname"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="RotateFlip"></param>
        /// <param name="BarWeight"></param>
        /// <returns></returns>
        public string GenerateNewCODE128ImgHR(string BarCodeText, string prefix, string sufix, string fname, string fontName, int fontSize, int width, int height, int RotateFlip, int BarWeight)
        {
            System.Drawing.Image code128img = default(System.Drawing.Image);

            try
            {
                code128img = Code128Rendering.MakeBarcodeImageHR(BarCodeText, BarWeight, true);
                //2
                code128img.RotateFlip((RotateFlipType)RotateFlip);
                code128img.Save(fname, ImageFormat.Jpeg);

            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }

            return fname;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BarCodeText"></param>
        /// <param name="prefix"></param>
        /// <param name="sufix"></param>
        /// <param name="fname"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="RotateFlip"></param>
        /// <param name="BarWeight"></param>
        /// <returns></returns>
        public string GenerateNewCODE128Img(string BarCodeText, string prefix, string sufix, string fname, string fontName, int fontSize, int width, int height, int RotateFlip, int BarWeight)
        {
            System.Drawing.Image code128img = default(System.Drawing.Image);

            try
            {
                code128img = Code128Rendering.MakeBarcodeImage(BarCodeText, BarWeight, true);
                code128img.RotateFlip((RotateFlipType)RotateFlip);
                code128img.Save(fname, ImageFormat.Jpeg);

            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }

            return fname;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BarCodeText"></param>
        /// <param name="prefix"></param>
        /// <param name="sufix"></param>
        /// <param name="fname"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="RotateFlip"></param>
        /// <param name="Scale"></param>
        /// <param name="Version"></param>
        /// <returns></returns>
        public string GenerateNewQRCode(string BarCodeText, string prefix, string sufix, string fname, string fontName, int fontSize, int width, int height, int RotateFlip, int Scale,
        int Version)
        {
            QRCode.Codec.QRCodeEncoder qrEnc = default(QRCode.Codec.QRCodeEncoder);
            qrEnc = new QRCodeEncoder();
            string encStr = BarCodeText;
            try
            {
                qrEnc.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrEnc.QRCodeScale = Scale;
                //3
                qrEnc.QRCodeVersion = Version;
                //4
                qrEnc.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                System.Drawing.Image QRImage = default(System.Drawing.Image);
                QRImage = qrEnc.Encode(encStr);
                QRImage.Save(fname, ImageFormat.Jpeg);

            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            return fname;
        }

       
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontcolor"></param>
        /// <returns></returns>
        private XBrush getColor(string fontcolor)
        {
            XSolidBrush x = null;

            switch (fontcolor.ToLower())
            {
                case "blue":
                    x = XBrushes.Blue;
                    break; // TODO: might not be correct. Was : Exit Select

                    //break;
                case "yellow":
                    x = XBrushes.Yellow;
                    break; // TODO: might not be correct. Was : Exit Select

                   // break;
                case "red":
                    x = XBrushes.Red;
                    break; // TODO: might not be correct. Was : Exit Select

                   // break;
                case "green":
                    x = XBrushes.Green;
                    break; // TODO: might not be correct. Was : Exit Select

                  //  break;
                case "brown":
                    x = XBrushes.Brown;
                    break; // TODO: might not be correct. Was : Exit Select

                   // break;
                case "gray":
                    x = XBrushes.Gray;
                    break; // TODO: might not be correct. Was : Exit Select

                  //  break;
                case "gold":
                    x = XBrushes.Gold;
                    break; // TODO: might not be correct. Was : Exit Select

                    // break;
                case "orange":
                    x = XBrushes.Orange;
                    break; // TODO: might not be correct. Was : Exit Select

                  //  break;
                case "purple":
                    x = XBrushes.Purple;
                    break; // TODO: might not be correct. Was : Exit Select

                   // break;
                case "silver":
                    x = XBrushes.Silver;
                    break; // TODO: might not be correct. Was : Exit Select

                   // break;
                case "teal":
                    x = XBrushes.Teal;
                    break; // TODO: might not be correct. Was : Exit Select

                  //  break;
                case "violet":
                    x = XBrushes.Violet;
                    break; // TODO: might not be correct. Was : Exit Select

                  //  break;
                case "white":
                    x = XBrushes.White;
                    break; // TODO: might not be correct. Was : Exit Select

                  //  break;
                default:
                    x = XBrushes.Black;
                    break; // TODO: might not be correct. Was : Exit Select

                    // break;
            }

            return x;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="pat_hosp_code"></param>
        /// <param name="fileName"></param>
        /// <param name="userid"></param>
        /// <param name="docpagesize"></param>
        /// <param name="addbarcodes"></param>
        /// <returns></returns>
        public string addBarcodesToPDFFileWithRepeat(string doc_id, string pat_hosp_code, string fileName, string userid, string docpagesize, string addbarcodes)
        {
            fname = fileName;
            string str = string.Empty;
            string _pid = doc_id;
            string _pathospid = pat_hosp_code;
            try
            {
                string pdffile = fileName;

                PdfDocument inputdoc = PdfReader.Open(pdffile.ToString(), PdfDocumentOpenMode.Import);
                int totalPages = inputdoc.PageCount;
                int _pageCount = 0;
                int _pageInit = 0;
                MemoryStream stream = new MemoryStream();
                PdfDocument document = new PdfDocument();


                try
                {
                    while ((_pageCount < totalPages))
                    {
                        PdfPage inputpage = inputdoc.Pages[_pageCount];
                        PdfPage page = document.AddPage(inputpage);

                        if (addbarcodes == "Y")
                        {
                            //Begin Add Barcode     
                            //End Add Barcode
                            GenerateBarCode(ref page, Convert.ToInt32(_pageInit), _pid, pat_hosp_code, userid);
                        }

                        _pageCount = _pageCount + 1;
                    }


                    document.Save(fname);
                }
                catch (System.Exception ex)
                {
                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                    return "Error";
                }
                finally
                {
                    stream.Close();
                    document.Close();
                    document.Dispose();
                    stream.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
            }
            return "Success";
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="pat_hosp_code"></param>
        /// <param name="fileName"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string StampPatientDataToInBoundFax_PDFFile(string doc_id, string pat_hosp_code, string fileName, string userid)
        {
            fname = fileName;
            string sqlString = string.Empty; 
            string str = string.Empty;
            string _pid = doc_id;
            string _pathospid = pat_hosp_code;
            System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection();
            SqlCommand Command = null;
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                string pdffile = string.Empty;
                string xmlfilecontent = string.Empty;
                string fontname = null;
                string fontisbold = null;
                string fontsize = null;
                string fontcolor = null;
                string x = null;
                string y = null;
                string value = null;


                SqlDataReader myDataReader = null;

                myConnection.ConnectionString = _ConnectionString;
                myConnection.Open();
                sqlString = "usp_get_pdf_final_data_xml";
                Command = new SqlCommand(sqlString, myConnection);
                //Get DB Stored procedure name from config
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandTimeout = Convert.ToInt32("180");

                Command.Parameters.Add("@pdfparam", SqlDbType.VarChar);
                Command.Parameters["@pdfparam"].Direction = ParameterDirection.Input;
                Command.Parameters["@pdfparam"].Value = _pid;

                Command.Parameters.Add("@pathospid", SqlDbType.VarChar);
                Command.Parameters["@pathospid"].Direction = ParameterDirection.Input;
                Command.Parameters["@pathospid"].Value = _pathospid;

                Command.Parameters.Add("@struser", SqlDbType.VarChar);
                Command.Parameters["@struser"].Direction = ParameterDirection.Input;
                Command.Parameters["@struser"].Value = userid;

                myDataReader = Command.ExecuteReader();

                while (myDataReader.Read())
                {
                    if (myDataReader.GetValue(myDataReader.GetOrdinal("pdffilename")) != System.DBNull.Value) 
                    {
                        pdffile = myDataReader.GetValue(myDataReader.GetOrdinal("pdffilename")).ToString().Trim();

                        xmlfilecontent = myDataReader.GetValue(myDataReader.GetOrdinal("xmlcontent")).ToString().Trim();
                    }
                }


                if ((xmlfilecontent.Trim().Length > 0))
                {
                    xmldoc.LoadXml(xmlfilecontent);
                }

                // Stamp patient data to Inbound fax pdf file
                pdffile = fileName;
                PdfDocument inputdoc = PdfReader.Open(pdffile.ToString(), PdfDocumentOpenMode.Import);
                int totalPages = inputdoc.PageCount;
                int _pageCount = 0;
                MemoryStream stream = new MemoryStream();
                PdfDocument document = new PdfDocument();


                try
                {
                    while ((_pageCount < totalPages))
                    {
                        PdfPage inputpage = inputdoc.Pages[_pageCount];
                        PdfPage page = document.AddPage(inputpage);

                        if ((xmlfilecontent.Trim().Length > 0))
                        {
                            //Begin Add Barcode     
                            GenerateBarCode(ref page, Convert.ToInt32(_pageCount), _pid, _pathospid, userid);
                            //End Add Barcode

                            string nodeName = null;
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                            nodeName = "//page" + (_pageCount + 1).ToString() + "/node";



                            foreach (XmlNode node in xmldoc.DocumentElement.SelectNodes(nodeName))
                            {
                                x = node.Attributes["x"].InnerText;
                                y = node.Attributes["y"].InnerText;
                                value = node.SelectSingleNode("value").InnerText;
                                fontname = node.SelectSingleNode("font").InnerText;
                                fontcolor = node.SelectSingleNode("font").Attributes["color"].InnerText;
                                fontsize = node.SelectSingleNode("font").Attributes["size"].InnerText;
                                fontisbold = node.SelectSingleNode("font").Attributes["isbold"].InnerText;

                                if (value.Trim().Length > 0)
                                {
                                    XFont font = new XFont(fontname, Convert.ToDouble(fontsize), XFontStyle.Regular, options);

                                    if (fontisbold.ToLower() == "y")
                                    {
                                        font = new XFont(fontname, Convert.ToDouble(fontsize), XFontStyle.Bold, options);
                                    }

                                    gfx.DrawString(value.ToString(), font, getColor(fontcolor), new XRect(Convert.ToDouble(x), Convert.ToDouble(y), Convert.ToDouble(page.Width), Convert.ToDouble(page.Height)), XStringFormats.TopLeft);

                                }
                            }
                        }
                        _pageCount = _pageCount + 1;
                    }

                    document.Save(fname);
                }
                catch (System.Exception ex)
                {
                    log.ExceptionDetails(ex.Message, ex.StackTrace);
                    return "Error";
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (System.Exception ex)
            {
                log.ExceptionDetails(ex.Message, ex.StackTrace);
                return "Error";
            }
            finally
            {
                Command = null;
                myConnection.Close();
                myConnection = null;
            }
            return "Success";
        }
    }
}
