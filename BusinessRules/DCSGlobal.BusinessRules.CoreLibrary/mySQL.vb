
Namespace MySQLStuff


    Public Class VBmySQL

        Private Structure myTimeInfo
            Public day As String
            Public month As String
            Public year As String
            Public hour As String
            Public min As String
            Public sec As String
        End Structure

        Private objmyTime As New myTimeInfo
        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function
        Public Function myNow() As String

            myNow = (UCase(Format(Now(), "yyyy-MM-dd HH:mm:ss")))


        End Function

        Public Function VBDateToMYSQL(ByVal dateInput As String) As String

            Dim strYear, strMonth, strDay, strHour, strMinute, strSecond As String


            strYear = CStr(Year(dateInput))
            strMonth = CStr(Month(dateInput))
            strDay = CStr(Day(dateInput))
            strHour = CStr(Hour(dateInput))
            strMinute = CStr(Minute(dateInput))
            strSecond = CStr(Second(dateInput))

            If Len(strHour) = 1 Then
                strHour = "0" & strHour
            ElseIf Len(strHour) = 0 Then
                strHour = "00"
            End If

            If Len(strMinute) = 1 Then
                strMinute = "0" & strMinute
            ElseIf Len(strMinute) = 0 Then
                strMinute = "00"
            End If

            If Len(strSecond) = 1 Then
                strSecond = "0" & strSecond
            ElseIf Len(strSecond) = 0 Then
                strSecond = "00"
            End If

            If Len(strDay) = 1 Then
                strDay = "0" + strDay
            ElseIf Len(strDay) = 0 Then
                strDay = "00"
            End If

            If Len(strMonth) = 1 Then
                strMonth = "0" & strMonth
            ElseIf Len(strMonth) = 0 Then
                strMonth = "00"
            End If

            VBDateToMYSQL = strYear + "-" + strMonth + "-" + strDay + " " + strHour + ":" + strMinute + ":" + strSecond

        End Function

        Public Function ParsemyDateTime(ByVal mySqlDate As String) As Integer

            objmyTime.year = Left(mySqlDate, 4)
            objmyTime.month = Mid(mySqlDate, 5, 2)
            objmyTime.day = Mid(mySqlDate, 9, 2)
            objmyTime.hour = Mid(mySqlDate, 12, 2)
            objmyTime.min = Mid(mySqlDate, 16, 2)
            objmyTime.sec = Right(mySqlDate, 2)

            Return 0


        End Function

        Public Property MySqlDateTimeString() As String

            Get
                Return objmyTime.day
            End Get
            Set(ByVal Value As String)
                'objmyTime.day = Value
            End Set



        End Property




        Public Property myDay() As String
            Get
                Return objmyTime.day
            End Get
            Set(ByVal Value As String)
                objmyTime.day = Value
            End Set

        End Property


        Public Property myMonth() As String
            Get
                Return objmyTime.month
            End Get
            Set(ByVal Value As String)
                objmyTime.month = Value
            End Set

        End Property


        Public Property myYear() As String
            Get
                Return objmyTime.year
            End Get
            Set(ByVal Value As String)
                objmyTime.year = Value
            End Set

        End Property

        Public Property myHour() As String
            Get
                Return objmyTime.hour
            End Get
            Set(ByVal Value As String)
                objmyTime.hour = Value
            End Set

        End Property

        Public Property myMin() As String
            Get
                Return objmyTime.min
            End Get
            Set(ByVal Value As String)
                objmyTime.min = Value
            End Set

        End Property

        Public Property mySec() As String
            Get
                Return objmyTime.sec
            End Get
            Set(ByVal Value As String)
                objmyTime.sec = Value
            End Set

        End Property

    End Class



End Namespace
