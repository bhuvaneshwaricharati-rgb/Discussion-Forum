Public Class ProfilePage
    Inherits Page

    Private _profileUserId As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            DetermineUser()
            LoadProfile()
            LoadUserThreads()
        End If
    End Sub

    Private Sub DetermineUser()
        ' If ?id= is passed, show that user; otherwise show current user
        If Request.QueryString("id") IsNot Nothing Then
            Integer.TryParse(Request.QueryString("id"), _profileUserId)
        Else
            If Not AuthHelper.IsLoggedIn(HttpContext.Current) Then
                Response.Redirect("~/Login?ReturnUrl=" & Server.UrlEncode(Request.RawUrl))
                Return
            End If
            _profileUserId = AuthHelper.GetCurrentUserId(HttpContext.Current)
        End If
        ViewState("ProfileUserId") = _profileUserId
    End Sub

    Private Function GetProfileUserId() As Integer
        If _profileUserId > 0 Then Return _profileUserId
        If ViewState("ProfileUserId") IsNot Nothing Then Return CInt(ViewState("ProfileUserId"))
        Return 0
    End Function

    Private Sub LoadProfile()
        Dim uid As Integer = GetProfileUserId()
        Try
            Dim dt As DataTable = DbHelper.ExecuteReader("SELECT * FROM Users WHERE Id = @p0", uid)
            If dt.Rows.Count = 0 Then
                Response.Redirect("~/")
                Return
            End If

            Dim row As DataRow = dt.Rows(0)
            Dim displayName As String = row("DisplayName").ToString()
            Dim username As String = row("Username").ToString()
            Dim role As String = row("Role").ToString()
            Dim bio As String = If(row("Bio") Is DBNull.Value, "", row("Bio").ToString())
            Dim createdAt As DateTime = CDate(row("CreatedAt"))

            lblDisplayName.Text = Server.HtmlEncode(displayName)
            lblUsername.Text = Server.HtmlEncode(username)
            divProfileAvatar.InnerText = AuthHelper.GetInitials(displayName)
            lblBio.Text = If(String.IsNullOrEmpty(bio), "No bio yet.", Server.HtmlEncode(bio))
            lblJoined.Text = createdAt.ToString("MMMM yyyy")

            ' Role badge
            lblRoleBadge.Text = role
            Select Case role
                Case "Admin" : lblRoleBadge.CssClass = "badge-role badge-admin"
                Case "Moderator" : lblRoleBadge.CssClass = "badge-role badge-moderator"
                Case Else : lblRoleBadge.CssClass = "badge-role badge-user"
            End Select

            ' Stats
            lblThreadCount.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Threads WHERE AuthorId = @p0", uid).ToString()
            lblReplyCount.Text = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Replies WHERE AuthorId = @p0", uid).ToString()
            Dim likesReceived As Object = DbHelper.ExecuteScalar(
                "SELECT ISNULL(SUM(r.LikeCount),0) FROM Replies r WHERE r.AuthorId = @p0", uid)
            lblLikesReceived.Text = likesReceived.ToString()

            ' Show edit button only for own profile
            If AuthHelper.IsLoggedIn(HttpContext.Current) AndAlso AuthHelper.GetCurrentUserId(HttpContext.Current) = uid Then
                pnlEditBtn.Visible = True
                txtEditDisplayName.Text = displayName
                txtEditBio.Text = bio
            End If

            Page.Title = displayName & "'s Profile"

        Catch ex As Exception
            Response.Redirect("~/")
        End Try
    End Sub

    Private Sub LoadUserThreads()
        Dim uid As Integer = GetProfileUserId()
        Try
            Dim dt As DataTable = DbHelper.ExecuteReader(
                "SELECT t.Id, t.Title, t.Body, t.ViewCount, t.CreatedAt, " &
                "(SELECT COUNT(*) FROM Replies r WHERE r.ThreadId = t.Id) AS ReplyCount " &
                "FROM Threads t WHERE t.AuthorId = @p0 ORDER BY t.CreatedAt DESC", uid)

            If dt.Rows.Count > 0 Then
                rptUserThreads.DataSource = dt
                rptUserThreads.DataBind()
                pnlNoThreads.Visible = False
            Else
                pnlNoThreads.Visible = True
            End If
        Catch
            pnlNoThreads.Visible = True
        End Try
    End Sub

    Protected Sub btnToggleEdit_Click(sender As Object, e As EventArgs)
        pnlEditForm.Visible = Not pnlEditForm.Visible
    End Sub

    Protected Sub btnCancelEdit_Click(sender As Object, e As EventArgs)
        pnlEditForm.Visible = False
    End Sub

    Protected Sub btnSaveProfile_Click(sender As Object, e As EventArgs)
        pnlEditError.Visible = False
        pnlEditSuccess.Visible = False

        Dim displayName As String = txtEditDisplayName.Text.Trim()
        Dim bio As String = txtEditBio.Text.Trim()

        If String.IsNullOrEmpty(displayName) Then
            pnlEditError.Visible = True
            lblEditError.Text = "Display name is required."
            Return
        End If

        Try
            Dim uid As Integer = AuthHelper.GetCurrentUserId(HttpContext.Current)
            DbHelper.ExecuteNonQuery("UPDATE Users SET DisplayName = @p0, Bio = @p1 WHERE Id = @p2", displayName, bio, uid)

            pnlEditSuccess.Visible = True
            lblEditSuccess.Text = "Profile updated successfully!"

            ' Refresh profile display
            lblDisplayName.Text = Server.HtmlEncode(displayName)
            divProfileAvatar.InnerText = AuthHelper.GetInitials(displayName)
            lblBio.Text = If(String.IsNullOrEmpty(bio), "No bio yet.", Server.HtmlEncode(bio))

        Catch ex As Exception
            pnlEditError.Visible = True
            lblEditError.Text = "An error occurred. Please try again."
        End Try
    End Sub

End Class
