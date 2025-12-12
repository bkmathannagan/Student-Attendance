Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Partial Class StudentBio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Dim dt As DataTable = DataAccess.GetDataTable("SELECT * FROM Students ORDER BY RegNo")
        gvStudents.DataSource = dt
        gvStudents.DataBind()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        lblMessage.Text = String.Empty
        If String.IsNullOrWhiteSpace(txtRegNo.Text) OrElse String.IsNullOrWhiteSpace(txtFirstName.Text) OrElse String.IsNullOrWhiteSpace(txtClass.Text) Then
            lblMessage.Text = "RegNo, First Name and Class are required."
            Return
        End If

        Dim photoPath As String = String.Empty
        If fuPhoto.HasFile Then
            Try
                Dim ext = Path.GetExtension(fuPhoto.FileName)
                Dim fileName = Guid.NewGuid().ToString() & ext
                Dim savePath = Server.MapPath("~/Images/" & fileName)
                fuPhoto.SaveAs(savePath)
                photoPath = "~/Images/" & fileName
            Catch ex As Exception
                lblMessage.Text = "Error saving photo: " & ex.Message
                Return
            End Try
        End If

        Dim studentID As Integer = 0
        If Not String.IsNullOrEmpty(hfStudentID.Value) Then Integer.TryParse(hfStudentID.Value, studentID)

        If studentID = 0 Then
            ' Insert
            Dim sql = "INSERT INTO Students (RegNo,FirstName,LastName,DOB,Class,Section,Gender,PhotoPath) VALUES (?,?,?,?,?,?,?,?)"
            Dim params = New OleDbParameter() {
                New OleDbParameter("@p1", txtRegNo.Text.Trim()),
                New OleDbParameter("@p2", txtFirstName.Text.Trim()),
                New OleDbParameter("@p3", txtLastName.Text.Trim()),
                New OleDbParameter("@p4", If(String.IsNullOrEmpty(txtDOB.Text), CType(DBNull.Value, Object), txtDOB.Text)),
                New OleDbParameter("@p5", txtClass.Text.Trim()),
                New OleDbParameter("@p6", txtSection.Text.Trim()),
                New OleDbParameter("@p7", ddlGender.SelectedValue),
                New OleDbParameter("@p8", If(String.IsNullOrEmpty(photoPath), String.Empty, photoPath))
            }
            Try
                DataAccess.ExecuteNonQuery(sql, params)
                ClearForm()
                BindGrid()
                lblMessage.CssClass = "text-success"
                lblMessage.Text = "Student added successfully."
            Catch ex As Exception
                lblMessage.Text = "Error adding student: " & ex.Message
            End Try
        Else
            ' Update
            Dim sql = "UPDATE Students SET RegNo=?,FirstName=?,LastName=?,DOB=?,Class=?,Section=?,Gender=?,PhotoPath=? WHERE StudentID=?"
            Dim params = New OleDbParameter() {
                New OleDbParameter("@p1", txtRegNo.Text.Trim()),
                New OleDbParameter("@p2", txtFirstName.Text.Trim()),
                New OleDbParameter("@p3", txtLastName.Text.Trim()),
                New OleDbParameter("@p4", If(String.IsNullOrEmpty(txtDOB.Text), CType(DBNull.Value, Object), txtDOB.Text)),
                New OleDbParameter("@p5", txtClass.Text.Trim()),
                New OleDbParameter("@p6", txtSection.Text.Trim()),
                New OleDbParameter("@p7", ddlGender.SelectedValue),
                New OleDbParameter("@p8", If(String.IsNullOrEmpty(photoPath), txtPhotoHidden.Value, photoPath)),
                New OleDbParameter("@p9", studentID)
            }
            Try
                DataAccess.ExecuteNonQuery(sql, params)
                ClearForm()
                BindGrid()
                lblMessage.CssClass = "text-success"
                lblMessage.Text = "Student updated successfully."
            Catch ex As Exception
                lblMessage.Text = "Error updating student: " & ex.Message
            End Try
        End If
    End Sub

    ' Hidden field to persist existing photo path
    Protected ReadOnly Property txtPhotoHidden As HiddenField
        Get
            Dim hf = TryCast(FindControl("hfPhotoPath"), HiddenField)
            If hf Is Nothing Then
                hf = New HiddenField()
                hf.ID = "hfPhotoPath"
                Page.Form.Controls.Add(hf)
            End If
            Return hf
        End Get
    End Property

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    Private Sub ClearForm()
        hfStudentID.Value = String.Empty
        txtRegNo.Text = String.Empty
        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtDOB.Text = String.Empty
        txtClass.Text = String.Empty
        txtSection.Text = String.Empty
        ddlGender.SelectedIndex = 0
        txtPhotoHidden.Value = String.Empty
        lblMessage.Text = String.Empty
    End Sub

    Protected Sub gvStudents_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditRow" Then
            Dim studentID = Convert.ToInt32(e.CommandArgument)
            Dim dt = DataAccess.GetDataTable("SELECT * FROM Students WHERE StudentID=?", New OleDbParameter("@p1", studentID))
            If dt.Rows.Count > 0 Then
                Dim r = dt.Rows(0)
                hfStudentID.Value = r("StudentID").ToString()
                txtRegNo.Text = r("RegNo").ToString()
                txtFirstName.Text = r("FirstName").ToString()
                txtLastName.Text = If(Convert.IsDBNull(r("LastName")), String.Empty, r("LastName").ToString())
                If Not Convert.IsDBNull(r("DOB")) Then txtDOB.Text = Convert.ToDateTime(r("DOB")).ToString("yyyy-MM-dd")
                txtClass.Text = If(Convert.IsDBNull(r("Class")), String.Empty, r("Class").ToString())
                txtSection.Text = If(Convert.IsDBNull(r("Section")), String.Empty, r("Section").ToString())
                ddlGender.SelectedValue = If(Convert.IsDBNull(r("Gender")), "", r("Gender").ToString())
                txtPhotoHidden.Value = If(Convert.IsDBNull(r("PhotoPath")), String.Empty, r("PhotoPath").ToString())
            End If
        ElseIf e.CommandName = "DeleteRow" Then
            Dim studentID = Convert.ToInt32(e.CommandArgument)
            Try
                DataAccess.ExecuteNonQuery("DELETE FROM Students WHERE StudentID=?", New OleDbParameter("@p1", studentID))
                BindGrid()
                lblMessage.CssClass = "text-success"
                lblMessage.Text = "Student deleted."
            Catch ex As Exception
                lblMessage.Text = "Error deleting student: " & ex.Message
            End Try
        End If
    End Sub

End Class