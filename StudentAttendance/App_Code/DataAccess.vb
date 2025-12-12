Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration

Public NotInheritable Class DataAccess

    Private Sub New()
    End Sub

    Private Shared ReadOnly Property ConnectionString As String
        Get
            Dim cs As String = ConfigurationManager.ConnectionStrings("AttendanceDB").ConnectionString
            Return cs
        End Get
    End Property

    Public Shared Function GetDataTable(sql As String, ParamArray parameters() As OleDbParameter) As DataTable
        Dim dt As New DataTable()
        Using cn As New OleDbConnection(ConnectionString)
            Using cmd As New OleDbCommand(sql, cn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Using da As New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Shared Function ExecuteNonQuery(sql As String, ParamArray parameters() As OleDbParameter) As Integer
        Using cn As New OleDbConnection(ConnectionString)
            Using cmd As New OleDbCommand(sql, cn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                cn.Open()
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Shared Function ExecuteScalar(Of T)(sql As String, ParamArray parameters() As OleDbParameter) As T
        Using cn As New OleDbConnection(ConnectionString)
            Using cmd As New OleDbCommand(sql, cn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                cn.Open()
                Dim obj = cmd.ExecuteScalar()
                If obj Is Nothing OrElse Convert.IsDBNull(obj) Then
                    Return Nothing
                End If
                Return CType(Convert.ChangeType(obj, GetType(T)), T)
            End Using
        End Using
    End Function

End Class