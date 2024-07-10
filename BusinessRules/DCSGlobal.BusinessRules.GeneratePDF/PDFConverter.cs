using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpertPdf.HtmlToPdf;
using DCSGlobal.BusinessRules.Logging;



namespace DCSGlobal.BusinessRules.GeneratePDF
{
    public class PDFConverter : IDisposable
    {
        
        logExecption log = new logExecption();
        
        
        bool _disposed;

        ~PDFConverter()
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





        string _ConnectionString = string.Empty;




        private int _LeftMargin = 36;
        private int _RightMargin = 36;
        private int _TopMargin = 36;
        private int _BottomMargin = 36;



        public string ConnectionString
        {
            set
            {

                log.ConnectionString = _ConnectionString;
                _ConnectionString = value;
            }
        }


        public int LeftMargin
        {
            set
            {

                _LeftMargin = value;
            }
        }


        public int RightMargin
        {
            set
            {

                _RightMargin = value;
            }
        }


        public int TopMargin
        {
            set
            {

                _TopMargin = value;
            }
        }


        public int BottomMargin
        {
            set
            {

                _BottomMargin = value;
            }
        }

        public int toFileFromString(string html, string filename)
        {

            int r = 0;


            try
            {
                PdfConverter pdfConverter = new PdfConverter();
                //pdfConverter.GetPdfBytesFromHtmlString(html);
                pdfConverter.LicenseKey = "vZaPnYWdiY2JnYyMk42djoyTjI+ThISEhA==";
                pdfConverter.PdfDocumentOptions.EmbedFonts = false;
                pdfConverter.PdfDocumentOptions.ShowFooter = false;
                pdfConverter.PdfDocumentOptions.ShowHeader = false;
                pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = false;
                pdfConverter.PdfDocumentOptions.InternalLinksEnabled = true;
                pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;

                pdfConverter.PdfDocumentOptions.LeftMargin = _LeftMargin;
                pdfConverter.PdfDocumentOptions.RightMargin = _RightMargin;
                pdfConverter.PdfDocumentOptions.TopMargin = _TopMargin;
                pdfConverter.PdfDocumentOptions.BottomMargin = _BottomMargin;
                pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;

                //outFile = Path.Combine(GetAppPath(), "RenderedPage.pdf");
                pdfConverter.SavePdfFromHtmlStringToFile(html, filename);

                pdfConverter = null;

            }
            catch (Exception ex)
            {
                log.ExceptionDetails("toFileFromString", ex);
                //Console.WriteLine(ex.Message);
                //return;
            }

            return r;
        }

        public int toFileFromString(string html, string filename, string BaseURL)
        {

            int r = 0;


            try
            {




                PdfConverter pdfConverter = new PdfConverter();

                //  pdfConverter.GetPdfBytesFromHtmlString(html);
                pdfConverter.LicenseKey = "vZaPnYWdiY2JnYyMk42djoyTjI+ThISEhA==";
                pdfConverter.PdfDocumentOptions.EmbedFonts = false;
                pdfConverter.PdfDocumentOptions.ShowFooter = false;
                pdfConverter.PdfDocumentOptions.ShowHeader = false;
                pdfConverter.PdfDocumentOptions.InternalLinksEnabled = true;
                pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = false;
                pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
                //outFile = Path.Combine(GetAppPath(), "RenderedPage.pdf");




                pdfConverter.PdfDocumentOptions.LeftMargin = _LeftMargin;
                pdfConverter.PdfDocumentOptions.RightMargin = _RightMargin;
                pdfConverter.PdfDocumentOptions.TopMargin = _TopMargin;
                pdfConverter.PdfDocumentOptions.BottomMargin = _BottomMargin;

                pdfConverter.SavePdfFromHtmlStringToFile(html, filename, BaseURL);

                pdfConverter = null;

            }

            catch (Exception ex)
            {
                log.ExceptionDetails("toFileFromString_baseURL", ex);
            }

      

            return r;
        }
    }
}



