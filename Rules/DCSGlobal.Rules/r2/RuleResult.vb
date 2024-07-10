Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath


Namespace DCSGlobal.Rules


    Public Class RuleResult

        Private _RuleID As Integer = 0
        Private _RuleReturn As Integer = 0
        Private _RuleMessage As String = String.Empty


        Public Property RuleID As Integer
            Set(value As Integer)
                _RuleID = value
            End Set
            Get
                Return _RuleID
            End Get
        End Property


        Public Property RuleReturn As Integer
            Set(value As Integer)
                _RuleReturn = value
            End Set
            Get
                Return _RuleReturn
            End Get
        End Property

        Public Property RuleMessage As String
            Set(value As String)
                _RuleMessage = value
            End Set
            Get
                Return _RuleMessage
            End Get
        End Property



    End Class

End Namespace
