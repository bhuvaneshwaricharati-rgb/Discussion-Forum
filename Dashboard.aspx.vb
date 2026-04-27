Namespace Discussion_Forum

Public Class AdminDashboardPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Admin/Moderator only
        If Not AuthHelper.IsModerator(HttpContext.Current) Then
            Response.Redirect("~/Login")
            Return
        End If

        If Not IsPostBack Then
            LoadStats()
            LoadTopContributors()
            LoadRecentLogs()
        End If
    End Sub

    Private Sub LoadStats()
        Try
            lblTotalUsers.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Users").ToString()
            lblActiveUsers.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Users WHERE IsActive = 1").ToString()
            lblTotalThreads.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Threads").ToString()
            lblTotalReplies.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Replies").ToString()
            lblActiveThreads.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Threads WHERE Status = 'Active' OR Status = 'Pinned'").ToString()
            lblWeeklyThreads.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Threads WHERE CreatedAt >= DATEADD(day, -7, GETDATE())").ToString()
        Catch
        End Try
    End Sub

    Private Sub LoadTopContributors()
        Try
            Dim sql As String = "SELECT TOP 10 u.DisplayName, u.Role, " &
                                "(SELECT COUNT(*) FROM Threads t WHERE t.AuthorId = u.Id) + " &
                                "(SELECT COUNT(*) FROM Replies r WHERE r.AuthorId = u.Id) AS PostCount " &
                                "FROM Users u ORDER BY PostCount DESC"
            Dim dt As DataTable = DbHelper.ExecuteReader(sql)
            rptTopContributors.DataSource = dt
            rptTopContributors.DataBind()
        Catch
        End Try
    End Sub

    Private Sub LoadRecentLogs()
        Try
            Dim sql As String = "SELECT TOP 15 a.Action, a.CreatedAt, ISNULL(u.Username, 'System') AS Username " &
                                "FROM AuditLogs a LEFT JOIN Users u ON a.UserId = u.Id " &
                                "ORDER BY a.CreatedAt DESC"
            Dim dt As DataTable = DbHelper.ExecuteReader(sql)
            rptRecentLogs.DataSource = dt
            rptRecentLogs.DataBind()
        Catch
        End Try
    End Sub

    Protected Function GetRoleBadgeClass(role As String) As String
        Select Case role
            Case "Admin" : Return "badge-admin"
            Case "Moderator" : Return "badge-moderator"
            Case Else : Return "badge-user"
        End Select
    End Function

    Protected Function GetActionBadgeClass(action As String) As String
        Select Case action
            Case "Login", "Logout" : Return "badge-active"
            Case "CreateThread", "CreateReply" : Return "badge-pinned"
            Case "DeleteThread", "DeleteReply" : Return "badge-closed"
            Case Else : Return "badge-active"
        End Select
    End Function

End Class

End Namespace
