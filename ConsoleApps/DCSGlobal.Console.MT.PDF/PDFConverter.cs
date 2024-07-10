using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpertPdf.HtmlToPdf;
using DCSGlobal.BusinessRules.Logging;



namespace DCSGlobal.BusinessRules.GeneratePDF
{
    public class PDFConverter
    {
        string _ConnectionString = string.Empty;

        logExecption log = new logExecption();


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
                pdfConverter.LicenseKey = "bkVcTlZOX11eTldAXk5dX0BfXEBXV1dX";
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
                
                
                //outFile = Path.Combine(GetAppPath(), "RenderedPage.pdf");
                pdfConverter.SavePdfFromHtmlStringToFile(html, filename);
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
                pdfConverter.LicenseKey = "bkVcTlZOX11eTldAXk5dX0BfXEBXV1dX";
                pdfConverter.PdfDocumentOptions.EmbedFonts = false;
                pdfConverter.PdfDocumentOptions.ShowFooter = false;
                pdfConverter.PdfDocumentOptions.ShowHeader = false;
                pdfConverter.PdfDocumentOptions.InternalLinksEnabled = true;
                pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = false;
                pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
                //outFile = Path.Combine(GetAppPath(), "RenderedPage.pdf");


                pdfConverter.SavePdfFromHtmlStringToFile(html, filename, BaseURL);



            }

            catch (Exception ex)
            {
                log.ExceptionDetails("toFileFromString_baseURL", ex);
            }

            ;

            return r;
        }
    }
}



