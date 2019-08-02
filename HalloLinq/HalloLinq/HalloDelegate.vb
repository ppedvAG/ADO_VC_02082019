
Delegate Sub EinfacherDelegate()
Delegate Sub DelegategMitPara(msg As String)
Delegate Function MeinCalcDelegate(a As Integer, b As Integer) As Long

Public Class HalloDelegate

    Sub New()

        Dim meinDele As EinfacherDelegate = AddressOf EinfacheMethode
        Dim meinDele2 As Action = AddressOf EinfacheMethode
        Dim meinDeleAno As Action = Sub()
                                        Console.WriteLine("Hallo")
                                    End Sub

        Dim meinDeleAno2 As Action = Sub() Console.WriteLine("Hallo")

        Dim deleMitPara As DelegategMitPara = AddressOf MethodeMitPara
        Dim deleMitPara2 As Action(Of String) = AddressOf MethodeMitPara
        Dim deleMitParaAno As DelegategMitPara = Sub(lala2 As String) Console.WriteLine(lala2)
        Dim deleMitParaAno2 As DelegategMitPara = Sub(lala2) Console.WriteLine(lala2)

        Dim meineCalc As MeinCalcDelegate = AddressOf Minus
        Dim meineCalc2 As Func(Of Integer, Integer, Long) = AddressOf Sum
        Dim meineCalcAno As MeinCalcDelegate = Function(a As Integer, b As Integer)
                                                   Return a + b
                                               End Function
        Dim meineCalcAno2 As MeinCalcDelegate = Function(a, b)
                                                    Return a + b
                                                End Function
        Dim meineCalcAno3 As MeinCalcDelegate = Function(a, b) a + b

        Dim liste As List(Of String)

        liste.Where(Function(x) x.StartsWith("b"))

        Dim lala = liste.Where(AddressOf Filter)
    End Sub

    Function Filter(x As String) As Boolean
        If (x.StartsWith("b")) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Minus(a As Integer, b As Integer) As Long
        Return b - a
    End Function

    Private Function Sum(a As Integer, b As Integer) As Long
        Return a + b
    End Function

    Private Sub MethodeMitPara(txt As String)
        Console.WriteLine(txt)
    End Sub

    Sub EinfacheMethode()
        Console.WriteLine("Hallo")
    End Sub


End Class
