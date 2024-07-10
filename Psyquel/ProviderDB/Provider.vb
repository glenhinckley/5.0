Public Class Provider

    Private _ProviderGroupID As Integer = 0
    Private _ProviderID As Integer = 0
    Private _ProviderLegacyID As Integer = 0
    Private _ProviderDomainID As Integer = 0
    Private _LastName As String = String.Empty
    Private _FirstName = String.Empty
    Private _NPI = String.Empty


    Public Property ProviderID As Integer
        Get
            Return _ProviderID
        End Get
        Set(value As Integer)


            _ProviderID = value

        End Set
    End Property

    Public Property ProviderGroupID As Integer
        Get
            Return _ProviderGroupID
        End Get
        Set(value As Integer)

            _ProviderGroupID = value
        End Set
    End Property

    Public Property ProviderLegacyID As Integer
        Get
            Return _ProviderLegacyID
        End Get
        Set(value As Integer)

            _ProviderLegacyID = value


        End Set
    End Property

    Public Property ProviderDomainID As Integer
        Get
            Return _ProviderDomainID
        End Get
        Set(value As Integer)

            _ProviderDomainID = value


        End Set
    End Property

    Public Property LastName As String
        Get
            Return _LastName
        End Get
        Set(value As String)


            _LastName = value
        End Set
    End Property

    Public Property FirstName As String
        Get
            Return _FirstName
        End Get
        Set(value As String)

            _FirstName = value

        End Set
    End Property

    Public Property NPI As String
        Get
            Return _NPI

        End Get
        Set(value As String)

            _NPI = value

        End Set
    End Property

    Public Function Clear() As Integer
        Dim r As Integer = 0

        _ProviderGroupID = 0
        _ProviderID = 0
        _ProviderLegacyID = 0
        _ProviderDomainID = 0
        _LastName = String.Empty
        _FirstName = String.Empty
        _NPI = String.Empty

        Return r

    End Function
End Class
