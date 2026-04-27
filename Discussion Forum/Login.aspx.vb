Public Class LoginPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' If already logged in, redirect home
            If AuthHelper.IsLoggedIn(HttpContext.Current) Then
                Response.Redirect("~/")
            End If

            ' Check for success message (from registration)
            If Request.QueryString("registered") = "1" Then
                pnlSuccess.Visible = True
                lblSuccess.Text = "Account created successfully! Please sign in."
            End If
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        pnlError.Visible = False
        pnlSuccess.Visible = False

        Dim email As String = txtEmail.Text.Trim()
        Dim password As String = txtPassword.Text

        ' Validate inputs
        If String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) Then
            ShowError("Please enter both email and password.")
            Return
        End If

        Try
            ' Look up user by email
            Dim dt As DataTable = DbHelper.ExecuteReader(
                "SELECT Id, Username, PasswordHash, Role, IsActive, DisplayName FROM Users WHERE Email = @p0", email)

            If dt.Rows.Count = 0 Then
                ShowError("Invalid email or password.")
                Return
            End If

            Dim row As DataRow = dt.Rows(0)

            ' Check if account is active
            If Not CBool(row("IsActive")) Then
                ShowError("Your account has been deactivated. Contact an administrator.")
                Return
            End If

            ' Verify password
            If Not AuthHelper.VerifyPassword(password, row("PasswordHash").ToString()) Then
                ShowError("Invalid email or password.")
                Return
            End If

            ' Login success
            Dim userId As Integer = CInt(row("Id"))
            Dim username As String = row("Username").ToString()
            Dim role As String = row("Role").ToString()

            AuthHelper.LoginUser(HttpContext.Current, userId, username, role)

            ' Redirect to returnUrl or home
            Dim returnUrl As String = Request.QueryString("ReturnUrl")
            If Not String.IsNullOrEmpty(returnUrl) Then
                Response.Redirect(returnUrl)
            Else
                Response.Redirect("~/")
            End If

        Catch ex As Exception
            ShowError("An error occurred. Please try again.")
        End Try
    End Sub

    Private Sub ShowError(msg As String)
        pnlError.Visible = True
        lblError.Text = msg
    End Sub

End Class
