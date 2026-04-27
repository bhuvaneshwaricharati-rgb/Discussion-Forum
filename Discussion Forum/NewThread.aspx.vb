Public Class NewThreadPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not AuthHelper.IsLoggedIn(HttpContext.Current) Then
            Response.Redirect("~/Login?ReturnUrl=" & Server.UrlEncode(Request.RawUrl))
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        pnlError.Visible = False

        Dim title As String = txtTitle.Text.Trim()
        Dim body As String = txtBody.Text.Trim()

        If String.IsNullOrEmpty(title) Then
            ShowError("Please enter a title for your discussion.")
            Return
        End If

        If String.IsNullOrEmpty(body) Then
            ShowError("Please enter the body of your discussion.")
            Return
        End If

        If title.Length < 5 Then
            ShowError("Title must be at least 5 characters long.")
            Return
        End If

        Try
            Dim userId As Integer = AuthHelper.GetCurrentUserId(HttpContext.Current)

            DbHelper.ExecuteNonQuery(
                "INSERT INTO Threads (Title, Body, AuthorId, Status) VALUES (@p0, @p1, @p2, 'Active')",
                title, body, userId)

            ' Get the new thread ID
            Dim newId As Object = DbHelper.ExecuteScalar("SELECT MAX(Id) FROM Threads WHERE AuthorId = @p0", userId)

            AuthHelper.LogAction(HttpContext.Current, userId, "CreateThread", "Created thread: " & title)

            Response.Redirect("~/ThreadView?id=" & newId.ToString())

        Catch ex As Exception
            ShowError("An error occurred. Please try again.")
        End Try
    End Sub

    Private Sub ShowError(msg As String)
        pnlError.Visible = True
        lblError.Text = msg
    End Sub

End Class
