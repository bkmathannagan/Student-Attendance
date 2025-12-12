Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Text

Partial Class Report
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtFrom.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")
            txtTo.Text = DateTime.Today.ToString("yyyy-MM-dd")
            GenerateReport()
        End If
    End Sub

    Protected Sub btnGenerate_Click(sender As Object, e As EventArgs)
        GenerateReport()
    End Sub

    Private Sub GenerateReport()
        lblMsg.Text = String.Empty
        Dim fromDate As Date
        Dim toDate As Date
        If Not Date.TryParse(txtFrom.Text, fromDate) OrElse Not Date.TryParse(txtTo.Text, toDate) Then
            lblMsg.Text = "Enter valid From and To dates."
            Return
        End If
        If fromDate > toDate Then
            lblMsg.Text = "From date must be earlier than To date."
            Return
        End If

        Dim sql As String = "SELECT s.StudentID, s.RegNo, s.FirstName, s.LastName, " & _
            "COUNT(a.AttendanceID) AS TotalDays, " & _
            "SUM(IIF(a.Status='Present',1,0)) AS PresentCount, " & _
            "SUM(IIF(a.Status='Absent',1,0)) AS AbsentCount " & _
            "FROM Students s LEFT JOIN Attendance a ON s.StudentID=a.StudentID AND a.AttDate BETWEEN ? AND ? " & _
            "GROUP BY s.StudentID, s.RegNo, s.FirstName, s.LastName ORDER BY s.RegNo"

        Dim dt As DataTable = DataAccess.GetDataTable(sql, New OleDbParameter("@p1", fromDate), New OleDbParameter("@p2", toDate))
        dt.Columns.Add("FullName", GetType(String))
        dt.Columns.Add("Percent", GetType(Decimal))

        For Each r As DataRow In dt.Rows
            r("FullName") = r("FirstName").ToString() & " " & r("LastName").ToString()
            Dim total As Integer = 0
            Integer.TryParse(r("TotalDays").ToString(), total)
            Dim present As Integer = 0
            Integer.TryParse(If(r.IsNull("PresentCount"), "0", r("PresentCount").ToString()), present)
            Dim percent As Decimal = 0
            If total > 0 Then
                percent = (present / total) * 100D
            End If
            r("Percent") = Math.Round(percent, 2)
        Next

        gvReport.DataSource = dt
        gvReport.DataBind()

        ViewState("ReportDT") = dt
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = TryCast(ViewState("ReportDT"), DataTable)
        If dt Is Nothing Then
            lblMsg.Text = "Generate report first."
            Return
        End If

        Dim sb As New StringBuilder()
        sb.AppendLine("RegNo,Student,TotalDays,Present,Absent,Percent")
        For Each r As DataRow In dt.Rows
            sb.AppendLine(String.Format("{0},{1},{2},{3},{4},{5}", EscapeCsv(r("RegNo").ToString()), EscapeCsv(r("FullName").ToString()), r("TotalDays").ToString(), If(r.IsNull("PresentCount"), "0", r("PresentCount").ToString()), If(r.IsNull("AbsentCount"), "0", r("AbsentCount").ToString()), r("Percent").ToString()))
        Next

        Response.Clear()
        Response.ContentType = "text/csv"
        Response.AddHeader("Content-Disposition", "attachment;filename=AttendanceReport.csv")
        Response.Write(sb.ToString())
        Response.End()
    End Sub

    Private Function EscapeCsv(val As String) As String
        If val.Contains(",") OrElse val.Contains("\"" ) OrElse val.Contains("\n") Then
            Return """" & val.Replace("""", """"" ) & """"
        End If
        Return val
    End Function

End Class