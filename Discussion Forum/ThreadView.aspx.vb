Public Class ThreadViewPage
    Inherits Page

    Private _threadId As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("id") Is Nothing Then
            Response.Redirect("~/")
            Return
        End If

        _threadId = 0
        Integer.TryParse(Request.QueryString("id"), _threadId)
        If _threadId = 0 Then
            Response.Redirect("~/")
            Return
        End If

        If Not IsPostBack Then
            LoadThread()
            LoadReplies()
            SetupReplyForm()
        End If
    End Sub

    Private Sub LoadThread()
        Try
            ' Increment view count
            DbHelper.ExecuteNonQuery("UPDATE Threads SET ViewCount = ViewCount + 1 WHERE Id = @p0", _threadId)

            Dim dt As DataTable = DbHelper.ExecuteReader(
                "SELECT t.*, u.DisplayName, u.Role FROM Threads t " &
                "INNER JOIN Users u ON t.AuthorId = u.Id WHERE t.Id = @p0", _threadId)

            If dt.Rows.Count = 0 Then
                Response.Redirect("~/")
                Return
            End If

            Dim row As DataRow = dt.Rows(0)
            Dim title As String = row("Title").ToString()
            Dim body As String = row("Body").ToString()
            Dim author As String = row("DisplayName").ToString()
            Dim role As String = row("Role").ToString()
            Dim status As String = row("Status").ToString()
            Dim views As Integer = CInt(row("ViewCount"))
            Dim createdAt As DateTime = CDate(row("CreatedAt"))

            Page.Title = title
            lblTitle.Text = Server.HtmlEncode(title)
            lblBody.Text = Server.HtmlEncode(body)
            lblAuthor.Text = Server.HtmlEncode(author)
            lblTime.Text = AuthHelper.TimeAgo(createdAt)
            lblViews.Text = views.ToString()

            ' Avatar
            divAvatar.InnerText = AuthHelper.GetInitials(author)
            Select Case role
                Case "Admin" : divAvatar.Attributes("class") = "thread-avatar admin-avatar"
                Case "Moderator" : divAvatar.Attributes("class") = "thread-avatar mod-avatar"
                Case Else : divAvatar.Attributes("class") = "thread-avatar"
            End Select

            ' Role badge
            lblRoleBadge.Text = role
            Select Case role
                Case "Admin" : lblRoleBadge.CssClass = "badge-role badge-admin"
                Case "Moderator" : lblRoleBadge.CssClass = "badge-role badge-moderator"
                Case Else : lblRoleBadge.CssClass = "badge-role badge-user"
            End Select

            ' Status badge
            lblStatusBadge.Text = status
            Select Case status
                Case "Active" : lblStatusBadge.CssClass = "badge-status badge-active"
                Case "Pinned" : lblStatusBadge.CssClass = "badge-status badge-pinned"
                Case "Closed" : lblStatusBadge.CssClass = "badge-status badge-closed"
                Case "Archived" : lblStatusBadge.CssClass = "badge-status badge-archived"
            End Select

            ' Mod actions
            If AuthHelper.IsModerator(HttpContext.Current) Then
                pnlModActions.Visible = True
            End If

            ' Reply count
            Dim replyCount As Object = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Replies WHERE ThreadId = @p0", _threadId)
            lblReplyCount.Text = replyCount.ToString()

        Catch ex As Exception
            Response.Redirect("~/")
        End Try
    End Sub

    Private Sub LoadReplies()
        Try
            Dim dt As DataTable = DbHelper.ExecuteReader(
                "SELECT r.Id, r.Body, r.IsEdited, r.LikeCount, r.CreatedAt, u.DisplayName, u.Role " &
                "FROM Replies r INNER JOIN Users u ON r.AuthorId = u.Id " &
                "WHERE r.ThreadId = @p0 ORDER BY r.CreatedAt ASC", _threadId)

            If dt.Rows.Count > 0 Then
                rptReplies.DataSource = dt
                rptReplies.DataBind()
                pnlNoReplies.Visible = False
            Else
                rptReplies.DataSource = Nothing
                rptReplies.DataBind()
                pnlNoReplies.Visible = True
            End If
        Catch
            pnlNoReplies.Visible = True
        End Try
    End Sub

    Private Sub SetupReplyForm()
        If AuthHelper.IsLoggedIn(HttpContext.Current) Then
            pnlReplyForm.Visible = True
            pnlLoginPrompt.Visible = False
            Dim username As String = AuthHelper.GetCurrentUsername(HttpContext.Current)
            ' Get display name for initials
            Try
                Dim displayName As Object = DbHelper.ExecuteScalar("SELECT DisplayName FROM Users WHERE Username = @p0", username)
                If displayName IsNot Nothing Then
                    lblMyInitials.Text = AuthHelper.GetInitials(displayName.ToString())
                End If
            Catch
                lblMyInitials.Text = username.Substring(0, 1).ToUpper()
            End Try
        Else
            pnlReplyForm.Visible = False
            pnlLoginPrompt.Visible = True
        End If
    End Sub

    Protected Sub btnReply_Click(sender As Object, e As EventArgs)
        pnlReplyError.Visible = False
        _threadId = CInt(Request.QueryString("id"))

        Dim replyBody As String = txtReply.Text.Trim()
        If String.IsNullOrEmpty(replyBody) Then
            pnlReplyError.Visible = True
            lblReplyError.Text = "Please enter a reply."
            Return
        End If

        Try
            Dim userId As Integer = AuthHelper.GetCurrentUserId(HttpContext.Current)
            DbHelper.ExecuteNonQuery(
                "INSERT INTO Replies (ThreadId, AuthorId, Body) VALUES (@p0, @p1, @p2)",
                _threadId, userId, replyBody)

            AuthHelper.LogAction(HttpContext.Current, userId, "CreateReply", "Replied to thread #" & _threadId)

            ' Refresh the page
            Response.Redirect("~/ThreadView?id=" & _threadId)
        Catch ex As Exception
            pnlReplyError.Visible = True
            lblReplyError.Text = "An error occurred. Please try again."
        End Try
    End Sub

    Protected Sub rptReplies_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        If e.CommandName = "Like" Then
            _threadId = CInt(Request.QueryString("id"))
            If Not AuthHelper.IsLoggedIn(HttpContext.Current) Then
                Response.Redirect("~/Login")
                Return
            End If

            Dim replyId As Integer = CInt(e.CommandArgument)
            Dim userId As Integer = AuthHelper.GetCurrentUserId(HttpContext.Current)

            Try
                ' Check if already liked
                Dim existing As Object = DbHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM Likes WHERE ReplyId = @p0 AND UserId = @p1", replyId, userId)

                If Convert.ToInt32(existing) = 0 Then
                    ' Add like
                    DbHelper.ExecuteNonQuery("INSERT INTO Likes (ReplyId, UserId) VALUES (@p0, @p1)", replyId, userId)
                    DbHelper.ExecuteNonQuery("UPDATE Replies SET LikeCount = LikeCount + 1 WHERE Id = @p0", replyId)
                Else
                    ' Remove like
                    DbHelper.ExecuteNonQuery("DELETE FROM Likes WHERE ReplyId = @p0 AND UserId = @p1", replyId, userId)
                    DbHelper.ExecuteNonQuery("UPDATE Replies SET LikeCount = CASE WHEN LikeCount > 0 THEN LikeCount - 1 ELSE 0 END WHERE Id = @p0", replyId)
                End If

                Response.Redirect("~/ThreadView?id=" & _threadId)
            Catch
            End Try
        End If
    End Sub

    ' --- Mod Actions ---
    Protected Sub btnClose_Click(sender As Object, e As EventArgs)
        UpdateThreadStatus("Closed")
    End Sub

    Protected Sub btnPin_Click(sender As Object, e As EventArgs)
        UpdateThreadStatus("Pinned")
    End Sub

    Protected Sub btnArchive_Click(sender As Object, e As EventArgs)
        UpdateThreadStatus("Archived")
    End Sub

    Protected Sub btnReopen_Click(sender As Object, e As EventArgs)
        UpdateThreadStatus("Active")
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        _threadId = CInt(Request.QueryString("id"))
        Try
            DbHelper.ExecuteNonQuery("DELETE FROM Threads WHERE Id = @p0", _threadId)
            AuthHelper.LogAction(HttpContext.Current, AuthHelper.GetCurrentUserId(HttpContext.Current), "DeleteThread", "Deleted thread #" & _threadId)
            Response.Redirect("~/")
        Catch
        End Try
    End Sub

    Private Sub UpdateThreadStatus(status As String)
        _threadId = CInt(Request.QueryString("id"))
        Try
            DbHelper.ExecuteNonQuery("UPDATE Threads SET Status = @p0 WHERE Id = @p1", status, _threadId)
            AuthHelper.LogAction(HttpContext.Current, AuthHelper.GetCurrentUserId(HttpContext.Current), "UpdateThreadStatus", "Set thread #" & _threadId & " to " & status)
            Response.Redirect("~/ThreadView?id=" & _threadId)
        Catch
        End Try
    End Sub

    ' --- Template Helpers ---
    Protected Function GetReplyAvatarClass(role As String) As String
        Select Case role
            Case "Admin" : Return "admin-avatar"
            Case "Moderator" : Return "mod-avatar"
            Case Else : Return ""
        End Select
    End Function

    Protected Function GetReplyRoleBadgeClass(role As String) As String
        Select Case role
            Case "Admin" : Return "badge-admin"
            Case "Moderator" : Return "badge-moderator"
            Case Else : Return "badge-user"
        End Select
    End Function

End Class
