
Public Class Employees
    Property Id As Integer
    Property Name As String
    Property GebDatum As Date
    Property Gehalt As Decimal
    Property AnzahlFinger As Integer

    Property Autos As List(Of Auto) = New List(Of Auto)
End Class

Public Class Auto
    Property Modell As String
    Property Hersteller As String
End Class

