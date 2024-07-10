


Namespace MathStuff

    Public Class clsMathStuff
        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function

        Public Function RoundOff(ByVal value As Double, ByVal digits As Integer) As Double
            Dim shift As Double

            shift = 10 ^ digits
            Return (CInt(value * shift) / shift)
        End Function

        Public Function RoundAssymetric(ByVal aNumberToRound As Double, _
          Optional ByVal aDecimalPlaces As Double = 0) As Double

            On Error GoTo ErrHandler

            Dim nFactor As Double
            Dim nTemp As Double

            nFactor = 10 ^ aDecimalPlaces
            nTemp = (aNumberToRound * nFactor) + 0.5
            RoundAssymetric = Int(CDec(nTemp)) / nFactor

            '-----------EXIT POINT------------------
ExitPoint:

            Exit Function

            '-----------ERROR HANDLER---------------
ErrHandler:

            Select Case Err.Number
                Case Else
                    ' Your error handling here
                    RoundAssymetric = 0
                    Resume ExitPoint
            End Select
        End Function

    End Class
End Namespace
