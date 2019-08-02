Imports System.Globalization
Imports System.Threading
Imports Bogus
Imports OfficeOpenXml

Class MainWindow

    Dim liste As List(Of Employees)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        'AddHandler Me.Loaded, Sub(s, e) MessageBox.Show("hallo")

        ' Add any initialization after the InitializeComponent() call.

        'Thread.CurrentThread.CurrentCulture = New CultureInfo("de-DE")
        'Thread.CurrentThread.CurrentUICulture = New CultureInfo("de-DE")

        liste = New List(Of Employees)()

        Randomizer.Seed = New Random(12)
        Dim ran = New Random(12)

        Dim autos = New Faker(Of Auto)("de").RuleFor(Function(x) x.Modell, Function(x, u) x.Vehicle.Model) _
                                            .RuleFor(Function(x) x.Hersteller, Function(x, u) x.Vehicle.Manufacturer) _
                                            .Generate(10)


        For index = 1 To 1000
            Dim emp = New Faker(Of Employees)("de") _
            .RuleFor(Function(x) x.Name, Function(f, x) f.Name.FirstName) _
            .RuleFor(Function(x) x.GebDatum, Function(f, x) f.Date.Past(40)) _
            .RuleFor(Function(x) x.Gehalt, Function(f, x) f.Random.Number(1000, 8000)) _
            .RuleFor(Function(x) x.AnzahlFinger, Function(f, x) f.Random.Number(1, 12)) _
            .Generate()

            emp.Id = index

            For m = 1 To ran.Next(0, 3)
                emp.Autos.Add(autos(ran.Next(0, 9)))
            Next

            liste.Add(emp)
        Next




        liste = liste.Union(New Faker(Of Employees)("de") _
            .RuleFor(Function(x) x.Name, Function(f, x) f.Name.FirstName) _
            .RuleFor(Function(x) x.GebDatum, Function(f, x) f.Date.Past(40)) _
            .RuleFor(Function(x) x.Gehalt, Function(f, x) f.Random.Number(1000, 8000)) _
            .RuleFor(Function(x) x.AnzahlFinger, Function(f, x) f.Random.Number(1, 12)) _
            .Generate(100)).ToList()
    End Sub

    Private Sub LadeAlles(sender As Object, e As RoutedEventArgs)
        myGrid.ItemsSource = liste
    End Sub

    Private Sub AlleDieMehrAls7FingerHaben(sender As Object, e As RoutedEventArgs)

        Dim query = From emp In liste
                    Where emp.AnzahlFinger > 7 And emp.Gehalt > 3000
                    Order By emp.AnzahlFinger Descending, emp.GebDatum
                    Select emp

        myGrid.ItemsSource = query.ToList()

    End Sub

    Private Sub AlleDieMehrAls7FingerHabenLAMBDA(ewfewfewfwe As Object, e As RoutedEventArgs)

        'liste.Where(x =>
        myGrid.ItemsSource = liste.Where(Function(emp) emp.AnzahlFinger > 7 And emp.Gehalt > 3000) _
                                  .OrderByDescending(Function(x) x.AnzahlFinger) _
                                  .ThenBy(Function(x) x.GebDatum) _
                                  .ToList()

    End Sub

    Private Sub SelectName(sender As Object, e As RoutedEventArgs)
        Dim namen = liste.Select(Function(x) x.Name).Take(10).Reverse().Reverse()
        Dim jahren = liste.Select(Function(x) x.GebDatum.Year).Distinct()

        MessageBox.Show($"Namen: {String.Join(" ⛄⛄ ", namen)}")
        ' 🤪

    End Sub

    Private Sub EmpsByYearIntoBaum(sender As Object, e As RoutedEventArgs)
        mytree.Items.Clear()

        Dim yearsGroups = liste.OrderBy(Function(x) x.GebDatum.Year).GroupBy(Function(x) x.GebDatum.Year)
        For Each yearGroup In yearsGroups
            Dim ti = New TreeViewItem()
            ti.Header = yearGroup.Key.ToString()

            For Each emp In yearGroup.OrderBy(Function(x) x.Name)

                Dim tii = New TreeViewItem()
                tii.Header = emp.Name
                ti.Items.Add(tii)


                emp.Autos.ForEach(Sub(a)
                                      Dim tiii = New TreeViewItem()
                                      tiii.Header = $"{a.Hersteller} {a.Modell}"
                                      tii.Items.Add(tiii)
                                  End Sub)
                'For Each a In emp.Autos
                '    Dim tiii = New TreeViewItem()
                '    tiii.Header = $"{a.Hersteller} {a.Modell}"
                '    tii.Items.Add(tiii)

                'Next
            Next

            mytree.Items.Add(ti)
        Next
    End Sub

    Private Sub DerErsteTjark(sender As Object, e As RoutedEventArgs)

        Dim tjark = liste.LastOrDefault(Function(x) x.Name = "Tjark")
        If tjark Is Nothing Then
            MessageBox.Show("😥")
        Else
            MessageBox.Show("😍")
        End If



    End Sub

    Private Sub NurMitCorvette(sender As Object, e As RoutedEventArgs)

        Dim query = liste.Where(Function(x) x.Autos.Any(Function(a) a.Modell = "Corvette"))

        myGrid.ItemsSource = query.ToList()

        query = query.OrderBy(Function(x) x.Gehalt)

        myGrid.ItemsSource = query.ToList()


    End Sub

    Private Sub Excel(sender As Object, e As RoutedEventArgs)
        Dim pack = New ExcelPackage(New IO.FileInfo("daten.xlsx"))
        Dim ws = pack.Workbook.Worksheets.FirstOrDefault()
        If ws Is Nothing Then
            ws = pack.Workbook.Worksheets.Add("Dings")
        End If
        For index = 1 To 10

            ws.Cells("A1").Value = "HaLLO"
            ws.Cells(index + 1, 1).Value = $"{index} AA"

        Next
        pack.Save()

    End Sub
End Class
