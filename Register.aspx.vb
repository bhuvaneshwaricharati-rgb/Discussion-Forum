Public Class RegisterPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If AuthHelper.IsLoggedIn(HttpContext.Current) Then
                Response.Redirect("~/")
            End If
        End If
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs)
        pnlError.Visible = False

        Dim username As String = txtUsername.Text.Trim()
        Dim displayName As String = txtDisplayName.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()
        Dim password As String = txtPassword.Text
        Dim confirmPassword As String = txtConfirmPassword.Text

        ' Validate required fields
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(displayName) OrElse
           String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) Then
            ShowError("All fields are required.")
            Return
        End If

        ' Validate username length
        If username.Length < 3 Then
            ShowError("Username must be at least 3 characters.")
            Return
        End If

        ' Validate password
        If password.Length < 6 Then
            ShowError("Password must be at least 6 characters.")
            Return
        End If

        ' Confirm passwords match
        If password <> confirmPassword Then
            ShowError("Passwords do not match.")
            Return
        End If

        Try
            ' Check if username exists
            Dim existingUser As Object = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Users WHERE Username = @p0", username)
            If Convert.ToInt32(existingUser) > 0 Then
                ShowError("Username is already taken.")
                Return
            End If

            ' Check if email exists
            Dim existingEmail As Object = DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Users WHERE Email = @p0", email)
            If Convert.ToInt32(existingEmail) > 0 Then
                ShowError("An account with this email already exists.")
                Return
            End If

            ' Hash password
            Dim passwordHash As String = AuthHelper.HashPassword(password)

            ' Create user
            DbHelper.ExecuteNonQuery(
                "INSERT INTO Users (Username, Email, PasswordHash, DisplayName, Role, IsActive) VALUES (@p0, @p1, @p2, @p3, 'User', 1)",
                username, email, passwordHash, displayName)

            ' Redirect to login with success
            Response.Redirect("~/Login?registered=1")

        Catch ex As Exception
            ShowError("An error occurred during registration. Please try again.")
        End Try
    End Sub

    Private Sub ShowError(msg As String)
        pnlError.Visible = True
        lblError.Text = msg
    End Sub

End Class
