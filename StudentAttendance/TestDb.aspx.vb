Imports System
Imports System.Data
Imports System.Data.OleDb

Partial Class TestDb
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblStatus.Text = String.Empty
            litOutput.Text = String.Empty
        End If
    End Sub

    Protected Sub btnTest_Click(sender As Object, e As EventArgs)
        lblStatus.Text = "Testing..."
        litOutput.Text = String.Empty
        Try
            ' Simple scalar to get number of students
            Dim count As Integer = DataAccess.ExecuteScalar(Of Integer)("SELECT COUNT(*) FROM Students")
            lblStatus.CssClass = "text-success"
            lblStatus.Text = "Connection OK."
            litOutput.Text = String.Format("<div class='alert alert-success mt-2'>Students in DB: <strong>{0}</strong></div>", count)
        Catch ex As Exception
            lblStatus.CssClass = "text-danger"
            lblStatus.Text = "Connection FAILED."
            litOutput.Text = String.Format("<div class='alert alert-danger mt-2'><pre>{0}</pre></div>", Server.HtmlEncode(ex.ToString()))
        End Try
    End Sub

End Class