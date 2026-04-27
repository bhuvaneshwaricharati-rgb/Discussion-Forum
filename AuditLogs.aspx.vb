Namespace Discussion_Forum

Public Class AdminAuditLogsPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not AuthHelper.IsModerator(HttpContext.Current) Then
            Response.Redirect("~/Login")
            Return
        End If

        If Not IsPostBack Then
            LoadLogs("", "")
        End If
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs)
        LoadLogs(txtSearch.Text.Trim(), ddlAction.SelectedValue)
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        txtSearch.Text = ""
        ddlAction.SelectedIndex = 0
        LoadLogs("", "")
    End Sub

    Private Sub LoadLogs(searchTerm As String, actionFilter As String)
        Try
            Dim sql As String = "SELECT TOP 200 a.Id, a.Action, a.Details, a.IpAddress, a.CreatedAt, " &
                                "ISNULL(u.Username, 'System') AS Username " &
                                "FROM AuditLogs a LEFT JOIN Users u ON a.UserId = u.Id WHERE 1=1 "

            Dim params As New List(Of Object)

            If Not String.IsNullOrEmpty(searchTerm) Then
                sql &= "AND (u.Username LIKE @p" & params.Count & " OR a.Action LIKE @p" & params.Count & " OR a.Details LIKE @p" & params.Count & ") "
                params.Add("%" & searchTerm & "%")
            End If

            If Not String.IsNullOrEmpty(actionFilter) Then
                sql &= "AND a.Action = @p" & params.Count & " "
                params.Add(actionFilter)
            End If

            sql &= "ORDER BY a.CreatedAt DESC"

            Dim dt As DataTable = DbHelper.ExecuteReader(sql, params.ToArray())

            If dt.Rows.Count > 0 Then
                rptLogs.DataSource = dt
                rptLogs.DataBind()
                pnlNoLogs.Visible = False
                lblLogCount.Text = dt.Rows.Count.ToString()
            Else
                rptLogs.DataSource = Nothing
                rptLogs.DataBind()
                pnlNoLogs.Visible = True
                lblLogCount.Text = "0"
            End If
        Catch
            pnlNoLogs.Visible = True
        End Try
    End Sub

    Protected Function GetActionBadgeClass(action As String) As String
        Select Case action
            Case "Login", "Logout" : Return "badge-active"
            Case "CreateThread", "CreateReply" : Return "badge-pinned"
            Case "DeleteThread", "DeleteReply" : Return "badge-closed"
            Case "ChangeRole", "ToggleUserStatus" : Return "badge-archived"
            Case Else : Return "badge-active"
        End Select
    End Function

End Class

End Namespace
