Public Class Form1
    Private Sub EmployeesBindingNavigatorSaveItem_Click(sender As Object, e As EventArgs) Handles EmployeesBindingNavigatorSaveItem.Click
        Me.Validate()
        Me.EmployeesBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.DataSet1)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataSet1.Employees' table. You can move, or remove it, as needed.
        Me.EmployeesTableAdapter.Fill(Me.DataSet1.Employees)

    End Sub
End Class
