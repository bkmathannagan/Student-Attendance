Imports System
Imports System.Data
Imports System.Data.OleDb

Partial Class Attendance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd")
            BindStudents()
        End If
    End Sub

    Private Sub BindStudents()
        lblMsg.Text = String.Empty
        Dim dtStudents As DataTable = DataAccess.GetDataTable("SELECT StudentID,RegNo,FirstName,LastName FROM Students ORDER BY RegNo")
        gvAttendance.DataSource = dtStudents
        gvAttendance.DataBind()

        ' Load existing attendance for selected date
        Dim attDate As Date = DateTime.Today
        If Not String.IsNullOrEmpty(txtDate.Text) Then
            Date.TryParse(txtDate.Text, attDate)
        End If
        Dim dtAtt As DataTable = DataAccess.GetDataTable("SELECT * FROM Attendance WHERE AttDate=?", New OleDbParameter("@p1", attDate))
        Dim attDict As New Dictionary(Of Integer, DataRow)()
        For Each r As DataRow In dtAtt.Rows
            Dim sid = Convert.ToInt32(r("StudentID"))
            attDict(sid) = r
        Next

        For Each row As GridViewRow In gvAttendance.Rows
            Dim hfID As Integer = Convert.ToInt32(gvAttendance.DataKeys(row.RowIndex).Value)
            Dim ddl = TryCast(row.FindControl("ddlStatus"), DropDownList)
            Dim txt = TryCast(row.FindControl("txtRemarks"), TextBox)
            If attDict.ContainsKey(hfID) Then
                Dim r = attDict(hfID)
                If Not Convert.IsDBNull(r("Status")) Then
                    ddl.SelectedValue = r("Status").ToString()
                End If
                If Not Convert.IsDBNull(r("Remarks")) Then
                    txt.Text = r("Remarks").ToString()
                End If
            End If
        Next
    End Sub

    Protected Sub btnLoad_Click(sender As Object, e As EventArgs)
        BindStudents()
    End Sub

    Protected Sub btnSaveAttendance_Click(sender As Object, e As EventArgs)
        lblMsg.Text = String.Empty
        Dim attDate As Date
        If Not Date.TryParse(txtDate.Text, attDate) Then
            lblMsg.Text = "Please enter a valid date."
            Return
        End If

        For Each row As GridViewRow In gvAttendance.Rows
            Dim studentID As Integer = Convert.ToInt32(gvAttendance.DataKeys(row.RowIndex).Value)
            Dim ddl = TryCast(row.FindControl("ddlStatus"), DropDownList)
            Dim txt = TryCast(row.FindControl("txtRemarks"), TextBox)
            Dim status = ddl.SelectedValue
            Dim remarks = txt.Text.Trim()

            ' Check if attendance exists
            Dim cnt As Integer = DataAccess.ExecuteScalar(Of Integer)("SELECT COUNT(*) FROM Attendance WHERE StudentID=? AND AttDate=?", New OleDbParameter("@p1", studentID), New OleDbParameter("@p2", attDate))
            If cnt > 0 Then
                ' Update
                Dim sql = "UPDATE Attendance SET Status=?,Remarks=? WHERE StudentID=? AND AttDate=?"
                DataAccess.ExecuteNonQuery(sql, New OleDbParameter("@p1", status), New OleDbParameter("@p2", remarks), New OleDbParameter("@p3", studentID), New OleDbParameter("@p4", attDate))
            Else
                ' Insert
                Dim sql = "INSERT INTO Attendance (StudentID,AttDate,Status,Remarks) VALUES (?,?,?,?)"
                DataAccess.ExecuteNonQuery(sql, New OleDbParameter("@p1", studentID), New OleDbParameter("@p2", attDate), New OleDbParameter("@p3", status), New OleDbParameter("@p4", remarks))
            End If
        Next

        lblMsg.CssClass = "text-success"
        lblMsg.Text = "Attendance saved."
    End Sub

End Class