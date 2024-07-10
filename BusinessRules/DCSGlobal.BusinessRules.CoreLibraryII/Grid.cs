using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;




namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class Grid
    {

        private int _dbCommandTimeOut = 90;
        private string _ConnectionString = string.Empty;
        private string _CommandString = string.Empty;
        private string _TableCssFile = string.Empty;
        private string _TableName = string.Empty;
        private bool _FirstRowisHeaders = false;
        private string _Caption = string.Empty;


        private StringStuff ss = new StringStuff();
        private string _version = ".9";
        private int _build = 1;


        public string Version
        {
           get
            {
                return _version; 

            }


        }



        public int Build
        {
            get
            {
                return _build;

            }


        }


        public int dbCommandTimeOut
        {


            set
            {
                _dbCommandTimeOut = value;
            }
        }
        public string ConnectionString
        {

            set
            {

          
                _ConnectionString = value;
            }
        }

        public string CommandString
        {


            set
            {


                _CommandString = value;
            }
        }


        public string TableCssFile
        {


            set
            {


                _TableCssFile = value;
            }
        }



        public string Caption
        {


            set
            {


                _Caption = value;
            }
        }


        public string TableClass
        {


            set
            {


                _TableName = value;
            }
        }

        public bool FirstRowisHeaders
        {


            set
            {

                _FirstRowisHeaders = value;
            }
        }



        public string grid()
        {



            string thetable = string.Empty;

            int count = 0;
            //  string connString = cmbConectionString.SelectedValue;

            bool headers = true;
            //  bool FirstRowisHeaders = chkFirstRowisHeader.Checked;
            //   string TableCssClass = txtCssFile.Text;
            //  string TableName = txtTableName.Text;

            //   SqlConnection conn = new SqlConnection(connString);



            try
            {





                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {




                    using (SqlCommand cmd = new SqlCommand(_CommandString, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = _dbCommandTimeOut;

                        con.Open();
                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;


                            if (idr.HasRows)
                            {


                                thetable = "<link href=\"/gui/css/" + _TableCssFile + "\" rel=\"stylesheet\" type=\"text/css\" />\r\n";


                                thetable = thetable + "<br/><table class=\"" + _TableName + "\">\r\n";

                                thetable = thetable + " <caption>\r\n" + _Caption + "</caption>\r\n";

                                while (idr.Read())
                                {


                                    // begin headers

                                    if (headers)
                                    {
                                        //  thetable = thetable + "<tr>";
                                        if (!_FirstRowisHeaders)
                                        { count = 0; }
                                        else
                                        {
                                            count = 1;
                                        }
                                        while (count < idr.VisibleFieldCount)
                                        {
                                            if (!_FirstRowisHeaders)
                                            {

                                                //    thetable = thetable + ("<th>");
                                                //    thetable = thetable + (idr.GetName(count));
                                                //   thetable = thetable + ("</th>");

                                                count++;
                                            }

                                            else
                                            {


                                                string thData = string.Empty;

                                                thData = idr.GetName(count);

                                                // COL_NAME|CSS_CLASS|STRING_FORMAT|DATA_TYPE

                                                //      thetable = thetable + ("<th class\"" + ss.ParseDemlimtedString(thData, "|", 2) + "\">");
                                                //      thetable = thetable + (ss.ParseDemlimtedString(thData, "|", 1));
                                                //     thetable = thetable + ("</th>");

                                                count++;

                                            }

                                            headers = false;

                                        }

                                        //    thetable = thetable + ("</tr>");
                                    }
                                    // end headers

                                    // data row




                                  //  int x = 1;



                               /*     if (!_FirstRowisHeaders)
                                    { count = 0; }
                                    else
                                    {
                                        count = 1;
                                    }
                                 
                                 */
                                    count++;
                                     if (count % 2 == 0)
                                    {
                                        thetable = thetable + "<!-- heelllll --><tr  id=\"Odd\">\r\n";
                                    }
                                    else
                                    {
                                        thetable = thetable + "<tr  id=\"Even\">\r\n";
                                    }

                                    int x = 1;
                                    while (x < idr.VisibleFieldCount)
                                    {



                                        if (!_FirstRowisHeaders)
                                        {


                                            thetable = thetable + ("<!-- heelldscsdcsdcsdclll" + Convert.ToString(idr[0]) + " --><td>\r\n");
                                            thetable = thetable + (Convert.ToString(idr[x]));
                                            thetable = thetable + ("</td>\r\n");
                                            x++;
                                        }
                                        else
                                        {


                                            switch (Convert.ToString(idr[0]))
                                            {

                                                case "Caption":

                                                    if (x == 1)
                                                    {
                                                        thetable = thetable + ("<td  class=\"sp\" colspan=\"" + Convert.ToString(idr.VisibleFieldCount - 1) + "\">\r\n");
                                                        thetable = thetable + (Convert.ToString(idr[x]));
                                                        thetable = thetable + ("</td>");
                                                    }
                                                    break;
                                                case "Detail":
                                                   
                                                    if (x == 1)
                                                    {thetable = thetable + ("<td class=\"DetailLeft\">\r\n");}
                                                    else if (x == idr.VisibleFieldCount -1)
                                                    { thetable = thetable + ("<td class=\"DetailRight\">\r\n"); }
                                                    else
                                                    { thetable = thetable + ("<td class=\"Detail\">\r\n"); }

                                                    
                                                    thetable = thetable + (Convert.ToString(idr[x]));
                                                    thetable = thetable + ("</td>");
                                                    break;
                                                case "Header":
                                                     if (x == 1)
                                                    { thetable = thetable + ("<td class=\"HeaderLeft\">\r\n");

                                                    thetable = thetable + "&nbsp;";
                                                    thetable = thetable + ("</td>");
                                                    
                                                    }
                                                    
                                                    
                                                    
                                                    if (x == 2)
                                                    {
                                                        thetable = thetable + ("<td  class=\"HeaderRight\" colspan=\"" + Convert.ToString(idr.VisibleFieldCount - 2) + "\">\r\n");
                                                        thetable = thetable + (Convert.ToString(idr[x]));
                                                        thetable = thetable + ("</td>");

                                                    }

                                              
                                                
                                                    break;
                                               
                                                
                                                case "MSG":

                                                    if (x == 1)
                                                    { thetable = thetable + ("<td class=\"DetailLeft\">\r\n");

                                                    thetable = thetable + "&nbsp;";
                                                    thetable = thetable + ("</td>");
                                                    
                                                    }
                                                    
                                                    
                                                    
                                                    if (x == 2)
                                                    {
                                                        thetable = thetable + ("<td  class\"MSG\" colspan=\"" + Convert.ToString(idr.VisibleFieldCount - 2) + "\">\r\n");
                                                        thetable = thetable + (Convert.ToString(idr[x]));
                                                        thetable = thetable + ("</td>");

                                                    }

                                                    break;
                                                
                                                
                                                default:
                                                    thetable = thetable + ("<td  class=\"xxx\">\r\n");
                                                    thetable = thetable + (Convert.ToString(idr[x]));
                                                    thetable = thetable + ("</td>\r\n");
                                                    break;

                                            }

                                            x++;

                                          

                                        }
                                    }
                                     // count++;
                                    thetable = thetable + "</tr>\r\n";

                                    //end data row




                                }

                                thetable = thetable + "</table>\r\n";

                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {

                thetable = thetable + ("<p>Error in SQL </p>");

                thetable = thetable + ("<pre>" + e.Message + "</pre>");
            }
            finally
            {

            }



            return thetable;


        }

        public string ELgrid()
        {



            string thetable = string.Empty;

            int count = 0;
            //  string connString = cmbConectionString.SelectedValue;

           // bool headers = true;
            //  bool FirstRowisHeaders = chkFirstRowisHeader.Checked;
            //   string TableCssClass = txtCssFile.Text;
            //  string TableName = txtTableName.Text;

            //   SqlConnection conn = new SqlConnection(connString);



            try
            {





                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {




                    using (SqlCommand cmd = new SqlCommand(_CommandString, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = _dbCommandTimeOut;

                        con.Open();
                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;


                            if (idr.HasRows)
                            {


                                thetable = "<link href=\"/gui/css/" + _TableCssFile + "\" rel=\"stylesheet\" type=\"text/css\" />\r\n";


                                thetable = thetable + "<br/><table class=\"" + _TableName + "\">\r\n";

                                thetable = thetable + " <caption>\r\n" + _Caption + "</caption>\r\n";

                                while (idr.Read())
                                {





                                    count++;


                                    if (count % 2 == 0)
                                    {
                                        thetable = thetable + "<tr  id=\"Odd\">\r\n";
                                    }
                                    else
                                    {
                                        thetable = thetable + "<tr  id=\"Even\">\r\n";
                                    }

                                    int x = 1;
                                    while (x < idr.VisibleFieldCount)
                                    {




                                            thetable = thetable + ("<td>\r\n");
                                            thetable = thetable + (Convert.ToString(idr[x]));
                                            thetable = thetable + ("</td>\r\n");
                                            x++;
                                        


                                           

                                       
                                    }

                                    thetable = thetable + "</tr>\r\n";

                                    //end data row




                                }

                                thetable = thetable + "</table>\r\n";

                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {

                thetable = thetable + ("<p>Error in SQL </p>");

                thetable = thetable + ("<pre>" + e.Message + "</pre>");
            }
            finally
            {

            }



            return thetable;


        }





    }
}
