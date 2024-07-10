using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Data.SqlClient;
using System.Data;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class BuildDLLString : IDisposable
    {


        private List<string> _code = new List<string>();
        private List<Rule> _Rules = new List<Rule>();

        private Dictionary<string, string> _vars = new Dictionary<string, string>();



        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";

        private StringStuff ss = new StringStuff();

        private string _VBString = string.Empty;


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BuildDLLString()
        {
            Dispose(false);
        }




        public BuildDLLString()
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


        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }




        public string VBString
        {

            get
            {

                return _VBString;
            }
        }



        public bool BuildAllRules()
        {

            bool r = false;





            return r;

        }




        public bool BuildSingleRule(int RuleID)
        {

            bool r = false;


            BuildVars();

            GetRules(RuleID);

            BuildRulesDLL();
            return r;

        }


        public bool BuildSingleRule(string RuleText)
        {

            bool r = false;

            BuildVars();



            return r;

        }






        private bool BuildRulesDLL()
        {
            bool r = false;

            /// BuildVars();

            //  GetRules();
            _code.Add("Imports Microsoft.VisualBasic");

            _code.Add("Imports System");
            _code.Add("Imports System.Data");
            //_code.Add("Imports System.Data.SqlClient");
            /// _code.Add("Imports System.IO");
            _code.Add("Imports System.Text.RegularExpressions");
            _code.Add("Imports System.Collections");
            _code.Add("Imports System.Collections.Generic");
            _code.Add("Imports System.Text");
            _code.Add("Imports System.Xml");
            _code.Add("Imports System.Xml.XPath");


            _code.Add("\r\n");
            _code.Add("Namespace DCSGlobal.Rules.GeneratedDLL");

            _code.Add("\r\n");
            _code.Add("Public Class modFireRulesAddrVB");

            _code.Add("\r\n");
            _code.Add("Implements IDisposable");

            _code.Add("\t\r\nPrivate disposedValue As Boolean ' To detect redundant calls");
            _code.Add("\t\r\nPrivate  _ConnectionString as String = String.Empty");
            _code.Add("\t\r\nPrivate  _Err as String = String.Empty");
            _code.Add("\t\r\nPrivate  _RuleResults As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)");
            _code.Add("\t\r\nPrivate  _RuleMsg As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)");
            _code.Add("\t\r\nPrivate  _dr As DataRow");
            _code.Add("\t\r\nPrivate  _isBuilt As Boolean = False");

            _code.Add("\t\r\nPrivate  _isMessagesBuilt As Boolean = False");



            _code.Add("' This code added by Visual Basic to correctly implement the disposable pattern.");
            _code.Add("Public Sub Dispose() Implements IDisposable.Dispose");
            _code.Add("'Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.");
            _code.Add("Dispose(True)");
            _code.Add("GC.SuppressFinalize(Me)");
            _code.Add("End Sub");







            //_code.Add("
            //");
            _code.Add("\r\n");
            _code.Add("Protected Overridable Sub Dispose(disposing As Boolean)");
            _code.Add("\tIf Not Me.disposedValue Then");
            _code.Add("\t\tIf disposing Then");
            _code.Add("\r\n");
            _code.Add("\t\tEnd If");

            _code.Add("\tEnd If");
            _code.Add("Me.disposedValue = True");
            _code.Add("End Sub");



            _code.Add("Dim g_sbAllRuleInsert As StringBuilder");

            _code.Add("Dim g_resultAll As String = String.Empty");




            _code.Add("\r\n");
            _code.Add("Public WriteOnly Property ResultAll As String");

            _code.Add("\r\n");

            _code.Add("Set(value As String)");
            _code.Add("g_resultAll = value");
            _code.Add("End Set");
            _code.Add("End Property");


            _code.Add("\r\n");
            _code.Add("Public WriteOnly Property ConnectionString As String");

            _code.Add("\r\n");

            _code.Add("Set(value As String)");
            _code.Add("_ConnectionString = value");
            _code.Add("End Set");
            _code.Add("End Property");



            _code.Add("\r\n");
            _code.Add("Public WriteOnly Property DR As DataRow");
            _code.Add("\r\n");
            _code.Add("\r\nSet(value As DataRow)");

            _code.Add("\r\n _dr = value");
            _code.Add("\r\n End Set");
            _code.Add("\r\nEnd Property");


            _code.Add("\r\nPublic ReadOnly Property RuleResults As Dictionary(Of Integer, Integer)");
            _code.Add("\r\n Get");

            _code.Add("\r\n Return _RuleResults");
            _code.Add("\r\n End Get");
            _code.Add("\r\nEnd Property");

           
            
            
            _code.Add("\r\nPublic ReadOnly Property RuleMessages As Dictionary(Of Integer, String)");
            _code.Add("\r\n Get");
            _code.Add("\r\n  BuildMessages()");
            _code.Add("\r\n Return _RuleMsg");
            _code.Add("\r\n End Get");
            _code.Add("\r\nEnd Property");




            






            _code.Add("\r\n");
            _code.Add("\r\n");
            _code.Add("\r\n");
            _code.Add("\r\n");
            _code.Add("\r\n");

            foreach (KeyValuePair<string, string> v in _vars)
            {
                switch (v.Value)
                {

                    case "System.String":

                        _code.Add("\tPrivate _" + v.Key + "  as " + v.Value + " = String.Empty");

                        break;

                    case "System.Decimal":

                        _code.Add("\tPrivate _" + v.Key + "  as " + v.Value + " = 0");

                        break;


                    case "System.Int16":

                        _code.Add("\tPrivate _" + v.Key + "  as " + v.Value + " = 0");

                        break;


                    case "System.Int32":

                        _code.Add("\tPrivate _" + v.Key + "  as " + v.Value + " = 0");
                        break;


                    case "System.Int64":
                        _code.Add("\tPrivate _" + v.Key + "  as " + v.Value + " = 0");
                        break;

                }
            }













            _code.Add("\r\n");



            _code.Add("\t''' <summary>");
            _code.Add("\t''' Main Call to run rules ");
            _code.Add("\t''' </summary> ");
            _code.Add("\t''' <param name=\"dr\">The DataRow that has tank </param> ");
            _code.Add("\t''' <returns></returns> ");
            _code.Add("\t''' <remarks></remarks> ");
            _code.Add("\tPublic Function ParseRuleDataRow() as Integer");




            _code.Add("\r\n");
            _code.Add("\r\n");

            _code.Add("\t\tDim r as integer = 0");

            _code.Add("\r\n");
            _code.Add("\r\n");
            _code.Add("\r\n");
            _code.Add("\r\n");
            _code.Add("\r\n");




            _code.Add("\t\t'Dim collection As DataTableCollection = ds.Tables");
            _code.Add("\t\t'For i As Integer = 0 To collection.Count - 1");
            _code.Add("\t\t'Dim table As DataTable = collection(i)");

            _code.Add("\r\n");




            foreach (KeyValuePair<string, string> v in _vars)
            {
                switch (v.Value)
                {

                    case "System.String":


                        _code.Add("\tTry");

                        _code.Add("\t\t _" + v.Key + " = Convert.ToString(_dr(\"" + v.Key + "\"))");

                        _code.Add("\tCatch ex As Exception");


                        _code.Add("\t\t_Err = _Err + \"" + v.Key + "\" + \"|\" + ex.Message + vbCrLf ");
                        _code.Add("\t\t r = -1 ");

                        _code.Add("\tEnd Try");
                        _code.Add("\r\n");



                        break;

                    case "System.Decimal":

                        _code.Add("\tTry");

                        _code.Add("\t\t _" + v.Key + " = Convert.ToDecimal(_dr(\"" + v.Key + "\"))");

                        _code.Add("\tCatch ex As Exception");

                        _code.Add("\t\t_Err = _Err + \"" + v.Key + "\" + \"|\" + ex.Message + vbCrLf ");
                        _code.Add("\t\t r = -1");

                        _code.Add("\tEnd Try");

                        _code.Add("\r\n");

                        break;


                    case "System.Int16":

                        _code.Add("\tTry");

                        _code.Add("\t\t _" + v.Key + " = Convert.ToInt16(_dr(\"" + v.Key + "\"))");
                        _code.Add("\tCatch ex As Exception");

                        _code.Add("\t\t_Err = _Err + \"" + v.Key + "\" + \"|\" + ex.Message + vbCrLf ");
                        _code.Add("\t\t r = -1");

                        _code.Add("\tEnd Try");

                        _code.Add("\r\n");

                        break;


                    case "System.Int32":

                        _code.Add("\tTry");

                        _code.Add("\t\t _" + v.Key + " = Convert.ToInt32(_dr(\"" + v.Key + "\"))");
                        _code.Add("\tCatch ex As Exception");

                        _code.Add("\t\t_Err = _Err + \"" + v.Key + "\" + \"|\" + ex.Message + vbCrLf ");
                        _code.Add("\t\t r = -1");
                        _code.Add("\tEnd Try");

                        _code.Add("\r\n");
                        break;


                    case "System.Int64":
                        _code.Add("\tTry");

                        _code.Add("\t\t _" + v.Key + " = Convert.ToInt64(_dr(\"" + v.Key + "\"))");
                        _code.Add("\tCatch ex As Exception");
                        _code.Add("\t\t_Err = _Err + \"" + v.Key + "\" + \"|\" + ex.Message + vbCrLf ");
                        _code.Add("\t\t r = -1");
                        _code.Add("\tEnd Try");

                        _code.Add("\r\n");
                        break;

                }
            }



            // get the list of rules and add them to a list to send to the rules parser



            _code.Add("\r\n");
            _code.Add("\r\n");


            _code.Add("\t\tIf r = 0 Then");
            _code.Add("\r\n");
            _code.Add("\t\t_isBuilt = True");
            _code.Add("\r\n");
            _code.Add("\t\tEnd If");



            _code.Add("\r\n");
            _code.Add("\tReturn r");
            _code.Add("\r\n");
            _code.Add("\tEnd Function");

            _code.Add("\r\n");


            _code.Add("\t\tPublic Sub BuildMessages()");

            try
            {
                using (BuildRuleMessages brm = new BuildRuleMessages())
                {



                    brm.ConnectionString = _ConnectionString;
                    Dictionary<int, string> _RuleMessages = brm.GetRuleMessages();

                    foreach (KeyValuePair<int, string> msg in _RuleMessages)
                    {

                        _code.Add("\r\n_RuleMsg.Add(" + Convert.ToString(msg.Key) + ",\"" + ss.Strip(msg.Value) + "\")");

                    }

                    _code.Add("\r\n  _isMessagesBuilt = true");

                }
            }
            catch (Exception ex)
            {


            }






            _code.Add("\tEnd Sub");


            _code.Add("\t\tPublic Function RunRules() As Integer ");

            _code.Add("\t\tDim R As Integer = 0 ");




            _code.Add("\r\n\tIf _isMessagesBuilt = False Then ");
            _code.Add("\r\n\tBuildMessages() ");
            _code.Add("\r\n\tEnd If");


            _code.Add("\t\tIf _isBuilt = False Then ");
            _code.Add("\t\tParseRuleDataRow() ");
            _code.Add("\t\tEnd If");


            // rule prtoype goes here
            foreach (Rule rule in _Rules)
            {


                _code.Add("\tTry");

             //   _code.Add("\t\t '" + rule.RulePrototypeNote);


                _code.Add("\r\nDim r_" + rule.RuleId + " As Integer = 0");



         //       _code.Add("\t\t r_" + rule.RuleId + " = " + "vb_" + rule.RulePrototype);

                _code.Add("\r\n_RuleResults.Add(" + rule.RuleId + ", r_" + rule.RuleId + ")");

                _code.Add("\tCatch ex As Exception");
                _code.Add("\t\t_Err = _Err + \"" + rule.RuleName + "\" + \"|\" + ex.Message + vbCrLf ");
                _code.Add("\r\nR = -1");
                _code.Add("\r\n_RuleResults.Add(" + rule.RuleId + ", -50000)");




                _code.Add("\tEnd Try");

                _code.Add("\r\n");

            }



            _code.Add("\r\n");
            _code.Add("\r\n");



            _code.Add("\tReturn r");

            _code.Add("\tEnd Function");







            // rule prtoype goes here
            foreach (Rule rule in _Rules)
            {
                _code.Add("\t'Begin RuleId =" + Convert.ToString(rule.RuleId) + "  RuleName=" + rule.RuleName);
                _code.Add("\t'" + rule.RuleDescription);
                _code.Add("\t'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
               // _code.Add("\t\t\t\t " + rule.RuleBody);
                _code.Add("\t'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                _code.Add("\'End RuleId =" + Convert.ToString(rule.RuleId) + "  RuleName=" + rule.RuleName);
                _code.Add("\r\n");
                _code.Add("\r\n");
                _code.Add("\r\n");

            }



















            //_code.Add("''' <summary> </summary> ");
            //_code.Add("''' <param name=\"RULE_ID\"></param>                                                                                                 ");
            //_code.Add("''' <param name=\"RULE_CONTEXT_ID\"></param>                                                                                              ");
            //_code.Add("''' <param name=\"PATINET_NUMBER\"></param>                                                                                          ");
            //_code.Add("''' <param name=\"PAT_HOSE_CODE\"></param>                                                                                           ");
            //_code.Add("''' <param name=\"FACILITY_CODE\"></param>                                                                                           ");
            //_code.Add("''' <param name=\"MESSAGE_ID\"></param>                                                                                              ");
            //_code.Add("''' <param name=\"OPERATOR_ID\"></param>                                                                                             ");
            //_code.Add("''' <param name=\"ADMIT_DATE\"></param>                                                                                              ");
            //_code.Add("''' <param name=\"EVENT_DATE\"></param>                                                                                              ");
            //_code.Add("''' <param name=\"PATIENT_PID\"></param>                                                                                             ");
            //_code.Add("''' <param name=\"INS_TYPE\"></param>                                                                                                ");
            //_code.Add("''' <param name=\"FINACIAL_CLASS\"></param>                                                                                          ");
            //_code.Add("''' <param name=\"PATIENT_TYPE\"></param>                                                                                            ");
            //_code.Add("''' <param name=\"PRI_PLAN_NUMBER\"></param>                                                                                         ");
            //_code.Add("''' <param name=\"SOURCE\"></param>                                                                                                  ");
            //_code.Add("''' <param name=\"EVENT_TYPE\"></param>                                                                                              ");
            //_code.Add("''' <param name=\"BATCH_ID\"></param>                                                                                                ");
            //_code.Add("''' <param name=\"FIRST_NAME\"></param>                                                                                              ");
            //_code.Add("''' <param name=\"LAST_NAME\"></param>                                                                                               ");
            //_code.Add("''' <param name=\"DISCHARGE_DATE\"></param>                                                                                          ");
            //_code.Add("''' <returns></returns>                                                                                                            ");
            //_code.Add("''' <remarks></remarks>                                                                                                            ");
            //_code.Add("Private Function buildXMLRuleInsert(ByVal RULE_ID As Integer, ByVal RULE_CONTEXT_ID As Integer, ByVal PATINET_NUMBER As String, _  ");
            //_code.Add("                            ByVal PAT_HOSE_CODE As String, ByVal FACILITY_CODE As String, ByVal MESSAGE_ID As Integer, _           ");
            //_code.Add("                            ByVal OPERATOR_ID As String, ByVal ADMIT_DATE As String, ByVal EVENT_DATE As String, _                 ");
            //_code.Add("                            ByVal PATIENT_PID As Integer, ByVal INS_TYPE As String, ByVal FINACIAL_CLASS As String, _              ");
            //_code.Add("                            ByVal PATIENT_TYPE As String, ByVal PRI_PLAN_NUMBER As String, ByVal SOURCE As String, _               ");
            //_code.Add("                            ByVal EVENT_TYPE As String, ByVal BATCH_ID As Integer, ByVal FIRST_NAME As String, _                   ");
            //_code.Add("                            ByVal LAST_NAME As String, ByVal DISCHARGE_DATE As String) As String                                   ");
            //_code.Add("    '   Dim ruleConn As SqlConnection = New SqlConnection                                                                          ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("    '   Dim __errMessage As String                                                                                                 ");
            //_code.Add("    Dim __event_datetime As String = String.Empty                                                                                  ");
            //_code.Add("    Dim __strXMLBuilder As New System.Text.StringBuilder                                                                           ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("    Try                                                                                                                            ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("        Dim __admit_date As String = String.Empty                                                                                  ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("        Dim y As String = String.Empty                                                                                             ");
            //_code.Add("        Dim m As String = String.Empty                                                                                             ");
            //_code.Add("        Dim d As String = String.Empty                                                                                             ");
            //_code.Add("        Dim h As String = String.Empty                                                                                             ");
            //_code.Add("        Dim mm As String = String.Empty                                                                                            ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("        If Not IsDBNull(ADMIT_DATE) And ADMIT_DATE.Length > 11 Then                                                                ");
            //_code.Add("            y = ADMIT_DATE.Substring(0, 4)                                                                                         ");
            //_code.Add("            m = ADMIT_DATE.Substring(4, 2)                                                                                         ");
            //_code.Add("            d = ADMIT_DATE.Substring(6, 2)                                                                                         ");
            //_code.Add("            h = ADMIT_DATE.Substring(8, 2)                                                                                         ");
            //_code.Add("            mm = ADMIT_DATE.Substring(10, 2)                                                                                       ");
            //_code.Add("            If (h = \"24\") Then                                                                                                     ");
            //_code.Add("                h = \"23\"                                                                                                           ");
            //_code.Add("                mm = \"59\"                                                                                                          ");
            //_code.Add("            End If                                                                                                                 ");
            //_code.Add("            __admit_date = y + \"-\" + m + \"-\" + d + \" \" + h + \":\" + mm                                                              ");
            //_code.Add("        ElseIf Not IsDBNull(__admit_date) And __admit_date.Length = 8 Then  'subba-020608                                          ");
            //_code.Add("            y = ADMIT_DATE.Substring(0, 4)                                                                                         ");
            //_code.Add("            m = ADMIT_DATE.Substring(4, 2)                                                                                         ");
            //_code.Add("            d = ADMIT_DATE.Substring(6, 2)                                                                                         ");
            //_code.Add("            __admit_date = y + \"-\" + m + \"-\" + d + \" \" + \"00\" + \":\" + \"01\"                                                         ");
            //_code.Add("        Else                                                                                                                       ");
            //_code.Add("            __admit_date = \"1/1/1909\"  ' default value                                                                             ");
            //_code.Add("        End If                                                                                                                     ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("        If Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length > 11 Then                                                                ");
            //_code.Add("            y = EVENT_DATE.Substring(0, 4)                                                                                         ");
            //_code.Add("            m = EVENT_DATE.Substring(4, 2)                                                                                         ");
            //_code.Add("            d = EVENT_DATE.Substring(6, 2)                                                                                         ");
            //_code.Add("            h = EVENT_DATE.Substring(8, 2)                                                                                         ");
            //_code.Add("            mm = EVENT_DATE.Substring(10, 2)                                                                                       ");
            //_code.Add("            If (h = \"24\") Then                                                                                                     ");
            //_code.Add("                h = \"23\"                                                                                                           ");
            //_code.Add("                mm = \"59\"                                                                                                          ");
            //_code.Add("            End If                                                                                                                 ");
            //_code.Add("            __event_datetime = y + \"-\" + m + \"-\" + d + \" \" + h + \":\" + mm                                                          ");
            //_code.Add("        ElseIf Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length = 8 Then                                                             ");
            //_code.Add("            y = EVENT_DATE.Substring(0, 4)                                                                                         ");
            //_code.Add("            m = EVENT_DATE.Substring(4, 2)                                                                                         ");
            //_code.Add("            d = EVENT_DATE.Substring(6, 2)                                                                                         ");
            //_code.Add("            h = \"00\"                                                                                                               ");
            //_code.Add("            mm = \"01\"                                                                                                              ");
            //_code.Add("            __event_datetime = y + \"-\" + m + \"-\" + d + \" \" + h + \":\" + mm                                                          ");
            //_code.Add("        Else                                                                                                                       ");
            //_code.Add("            __event_datetime = \"1/1/1908\" ' default value                                                                          ");
            //_code.Add("        End If                                                                                                                     ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("        __strXMLBuilder.Append(\"<rule_results>\")                                                                                   ");
            //_code.Add("        __strXMLBuilder.Append(\"<rule_id>\" & RULE_ID.ToString() & \"</rule_id>\")                                                    ");
            //_code.Add("        __strXMLBuilder.Append(\"<context_id>\" & RULE_CONTEXT_ID & \"</context_id>\")                                                 ");
            //_code.Add("        __strXMLBuilder.Append(\"<pat_hosp_code>\" & PAT_HOSE_CODE & \"</pat_hosp_code>\")                                             ");
            //_code.Add("        __strXMLBuilder.Append(\"<patient_number>\" & PATINET_NUMBER & \"</patient_number>\")                                          ");
            //_code.Add("        __strXMLBuilder.Append(\"<facility_code>\" & FACILITY_CODE & \"</facility_code>\")                                             ");
            //_code.Add("        __strXMLBuilder.Append(\"<operator_id>\" & OPERATOR_ID & \"</operator_id>\")                                                   ");
            //_code.Add("        __strXMLBuilder.Append(\"<admit_date>\" & __admit_date & \"</admit_date>\")                                                    ");
            //_code.Add("        __strXMLBuilder.Append(\"<PID>\" & PATIENT_PID & \"</PID>\")                                                                   ");
            //_code.Add("        __strXMLBuilder.Append(\"<ins_type>\" & INS_TYPE & \"</ins_type>\")                                                            ");
            //_code.Add("        'strXMLBuilder.Append(\"<financial_class>\" & financial_class & \"</financial_class>\")                                        ");
            //_code.Add("        'strXMLBuilder.Append(\"<patient_type>\" & patient_type & \"</patient_type>\")                                                 ");
            //_code.Add("        __strXMLBuilder.Append(\"<source>\" & SOURCE & \"</source>\")                                                                  ");
            //_code.Add("        __strXMLBuilder.Append(\"<event_type>\" & EVENT_TYPE & \"</event_type>\")                                                      ");
            //_code.Add("        __strXMLBuilder.Append(\"<pri_plan_number>\" & PRI_PLAN_NUMBER & \"</pri_plan_number>\")                                       ");
            //_code.Add("        __strXMLBuilder.Append(\"<batch_id>\" & BATCH_ID & \"</batch_id>\")                                                            ");
            //_code.Add("        __strXMLBuilder.Append(\"<message_id>\" & MESSAGE_ID & \"</message_id>\")                                                      ");
            //_code.Add("        __strXMLBuilder.Append(\"<modified_date>\" & Now().ToString() & \"</modified_date>\")                                          ");
            //_code.Add("        __strXMLBuilder.Append(\"<event_datetime>\" & __event_datetime & \"</event_datetime>\")                                        ");
            //_code.Add("        __strXMLBuilder.Append(\"<patient_first_name>\" & FIRST_NAME & \"</patient_first_name>\")  'subba-121107                       ");
            //_code.Add("        __strXMLBuilder.Append(\"<patient_last_name>\" & LAST_NAME & \"</patient_last_name>\")                                         ");
            //_code.Add("        __strXMLBuilder.Append(\"<discharge_date>\" & DISCHARGE_DATE & \"</discharge_date>\")                                          ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("        __strXMLBuilder.Append(\"</rule_results>\")                                                                                  ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("    Catch ex As Exception                                                                                                          ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("   '     log.ExceptionDetails(\"buildXMLRuleInsert\", ex)                                                                             ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("    End Try                                                                                                                        ");
            //_code.Add("                                                                                                                                   ");
            //_code.Add("    Return __strXMLBuilder.ToString()                                                                                              ");
            //_code.Add("End Function                                                                                                                       ");




            _code.Add("\tEnd Class");

            _code.Add("End Namespace");






            BuildString();

            return r;

        }








        private bool GetRules(int RuleID = 0)
        {

            bool r = false;

            string select = string.Empty;

            _VBString = string.Empty;


            if (RuleID == 0)
            {
                select = "SELECT * FROM RULE_DEF where status = 'A'";
            }
            else
            {
                select = "SELECT * FROM RULE_DEF where rule_id = " + RuleID;
            }


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(select, con))
                {
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {
                        if (idr.HasRows)
                        {
                            // Call Read before accessing data. 
                            while (idr.Read())
                            {
                                if (idr["id"] != System.DBNull.Value)
                                {
                                    //_words.Add(Convert.ToString((string)idr["db_column_name"]));

                                    bool RuleBuilt = false;
                                    Rule _rule = new Rule();

                                    _rule.RuleId = Convert.ToInt64(idr["rule_id"]);
                                    _rule.RuleName = Convert.ToString(idr["rule_name"]);

                                    _rule.RuleName = _rule.RuleName.Replace(" ", "_");


                                    _rule.RuleDescription = Convert.ToString(idr["rule_description"]);
                                    _rule.RuleStatus = Convert.ToString(idr["status"]);
                                    _rule.RuleDef = Convert.ToString(idr["rule_def"]);

                                 //   _RuleMsg.Add(Convert.ToInt32(_rule.RuleId), _rule.RuleDescription);

                                    try
                                    {
                                        using (RuleParser p = new RuleParser())
                                        {
                                           // p.rule_name = _rule.RuleName;
                                           // p.rule_description = _rule.RuleDescription;
                                           // p.rule_id = Convert.ToString(_rule.RuleId);

                                            p.ParseRuleVBS(_rule.RuleDef);


                                       //     _rule.RuleBody = p.RuleVB;
///_rule.RulePrototype = p.RuleVBPrototype;

                                            RuleBuilt = true;

                                        }
                                    }
                                    catch (Exception ex)
                                    {




                                    }



                                    if (RuleBuilt)
                                    {
                                        _Rules.Add(_rule);

                                    }



                                }

                            }
                        }
                    }
                    con.Close();
                }
                return r;
            }
        }


        private bool BuildString()
        {

            bool r = false;

            _VBString = string.Empty;


            foreach (string c in _code)
            {
                _VBString = _VBString + c + "\r\n";
            }


            return r;

        }


        private bool BuildVars()
        {
            bool r = false;

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand("usp_get_all_data_tank_ByID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar, 50).Value = "x";


                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {
                        //  count = idr.VisibleFieldCount;

                        dt = idr.GetSchemaTable();

                    }
                    // 

                }
                con.Close();
            }




            foreach (DataRow rdrColumn in dt.Rows)
            {

                try
                {

                    string columnName = rdrColumn[dt.Columns["ColumnName"]].ToString();
                    string dataType = rdrColumn[dt.Columns["DataType"]].ToString();
                    _vars.Add(columnName, dataType);

                }
                catch (Exception ex)
                {
                    r = false;

                }


            }


            int i = _vars.Count;

            return r;

        }

    }
}
