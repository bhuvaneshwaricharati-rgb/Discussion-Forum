Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadThreads("")
            LoadSidebarStats()
            LoadContributors()
        End If
    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadThreads(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadThreads(searchTerm As String)
        Try
            Dim sql As String = "SELECT t.Id, t.Title, t.Body, t.Status, t.ViewCount, t.CreatedAt, " &
                                "u.DisplayName, u.Role, " &
                                "(SELECT COUNT(*) FROM Replies r WHERE r.ThreadId = t.Id) AS ReplyCount, " &
                                "(SELECT ISNULL(SUM(r2.LikeCount),0) FROM Replies r2 WHERE r2.ThreadId = t.Id) AS TotalLikes " &
                                "FROM Threads t INNER JOIN Users u ON t.AuthorId = u.Id "

            If Not String.IsNullOrEmpty(searchTerm) Then
                sql &= "WHERE (t.Title LIKE @p0 OR t.Body LIKE @p0) "
                sql &= "ORDER BY CASE WHEN t.Status = 'Pinned' THEN 0 ELSE 1 END, t.CreatedAt DESC"
                Dim dt As DataTable = DbHelper.ExecuteReader(sql, "%" & searchTerm & "%")
                BindThreads(dt)
            Else
                sql &= "ORDER BY CASE WHEN t.Status = 'Pinned' THEN 0 ELSE 1 END, t.CreatedAt DESC"
                Dim dt As DataTable = DbHelper.ExecuteReader(sql)
                BindThreads(dt)
            End If
        Catch ex As Exception
            ' If DB not ready, show empty
            pnlNoThreads.Visible = True
        End Try
    End Sub

    Private Sub BindThreads(dt As DataTable)
        If dt.Rows.Count > 0 Then
            rptThreads.DataSource = dt
            rptThreads.DataBind()
            pnlNoThreads.Visible = False
        Else
            rptThreads.DataSource = Nothing
            rptThreads.DataBind()
            pnlNoThreads.Visible = True
        End If
    End Sub

    Private Sub LoadSidebarStats()
        Try
            lblTotalThreads.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Threads").ToString()
            lblTotalReplies.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Replies").ToString()
            lblTotalMembers.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Users").ToString()
        Catch
        End Try
    End Sub

    Private Sub LoadContributors()
        Try
            Dim sql As String = "SELECT TOP 5 u.DisplayName, " &
                                "(SELECT COUNT(*) FROM Threads t WHERE t.AuthorId = u.Id) + " &
                                "(SELECT COUNT(*) FROM Replies r WHERE r.AuthorId = u.Id) AS PostCount " &
                                "FROM Users u WHERE u.IsActive = 1 ORDER BY PostCount DESC"
            Dim dt As DataTable = DbHelper.ExecuteReader(sql)
            rptContributors.DataSource = dt
            rptContributors.DataBind()
        Catch
        End Try
    End Sub

    ' --- Helper functions for template ---
    Protected Function GetAvatarClass(role As String) As String
        Select Case role
            Case "Admin" : Return "admin-avatar"
            Case "Moderator" : Return "mod-avatar"
            Case Else : Return ""
        End Select
    End Function

    Protected Function GetRoleBadgeClass(role As String) As String
        Select Case role
            Case "Admin" : Return "badge-admin"
            Case "Moderator" : Return "badge-moderator"
            Case Else : Return "badge-user"
        End Select
    End Function

    Protected Function GetStatusBadgeClass(status As String) As String
        Select Case status
            Case "Active" : Return "badge-active"
            Case "Pinned" : Return "badge-pinned"
            Case "Closed" : Return "badge-closed"
            Case "Archived" : Return "badge-archived"
            Case Else : Return "badge-active"
        End Select
    End Function

End Class