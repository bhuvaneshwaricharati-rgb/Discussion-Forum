Imports System.Security.Cryptography
Imports System.Web.Security

Public Class AuthHelper

    ' --- Password Hashing (PBKDF2 with SHA256) ---

    Public Shared Function HashPassword(password As String) As String
        Dim salt(15) As Byte
        Using rng = New RNGCryptoServiceProvider()
            rng.GetBytes(salt)
        End Using

        Using pbkdf2 = New Rfc2898DeriveBytes(password, salt, 10000)
            Dim hash() As Byte = pbkdf2.GetBytes(32)
            Dim hashBytes(47) As Byte
            Array.Copy(salt, 0, hashBytes, 0, 16)
            Array.Copy(hash, 0, hashBytes, 16, 32)
            Return Convert.ToBase64String(hashBytes)
        End Using
    End Function

    Public Shared Function VerifyPassword(password As String, storedHash As String) As Boolean
        Try
            Dim hashBytes() As Byte = Convert.FromBase64String(storedHash)
            Dim salt(15) As Byte
            Array.Copy(hashBytes, 0, salt, 0, 16)

            Using pbkdf2 = New Rfc2898DeriveBytes(password, salt, 10000)
                Dim hash() As Byte = pbkdf2.GetBytes(32)
                For i As Integer = 0 To 31
                    If hashBytes(i + 16) <> hash(i) Then Return False
                Next
            End Using
            Return True
        Catch
            Return False
        End Try
    End Function

    ' --- Session Management ---

    Public Shared Sub LoginUser(context As HttpContext, userId As Integer, username As String, role As String)
        FormsAuthentication.SetAuthCookie(username, True)
        context.Session("UserId") = userId
        context.Session("Username") = username
        context.Session("UserRole") = role

        ' Update last login
        DbHelper.ExecuteNonQuery("UPDATE Users SET LastLoginAt = GETDATE() WHERE Id = @p0", userId)

        ' Audit log
        LogAction(context, userId, "Login", "User logged in")
    End Sub

    Public Shared Sub LogoutUser(context As HttpContext)
        Dim userId As Object = context.Session("UserId")
        If userId IsNot Nothing Then
            LogAction(context, CInt(userId), "Logout", "User logged out")
        End If
        FormsAuthentication.SignOut()
        context.Session.Clear()
        context.Session.Abandon()
    End Sub

    Public Shared Function GetCurrentUserId(context As HttpContext) As Integer
        If context.Session("UserId") IsNot Nothing Then
            Return CInt(context.Session("UserId"))
        End If
        Return 0
    End Function

    Public Shared Function GetCurrentUsername(context As HttpContext) As String
        If context.Session("Username") IsNot Nothing Then
            Return context.Session("Username").ToString()
        End If
        Return ""
    End Function

    Public Shared Function GetCurrentUserRole(context As HttpContext) As String
        If context.Session("UserRole") IsNot Nothing Then
            Return context.Session("UserRole").ToString()
        End If
        Return ""
    End Function

    Public Shared Function IsLoggedIn(context As HttpContext) As Boolean
        Return GetCurrentUserId(context) > 0
    End Function

    Public Shared Function IsAdmin(context As HttpContext) As Boolean
        Return GetCurrentUserRole(context) = "Admin"
    End Function

    Public Shared Function IsModerator(context As HttpContext) As Boolean
        Dim role As String = GetCurrentUserRole(context)
        Return role = "Moderator" OrElse role = "Admin"
    End Function

    ' --- Audit Logging ---

    Public Shared Sub LogAction(context As HttpContext, userId As Integer, action As String, details As String)
        Dim ip As String = GetClientIp(context)
        Try
            DbHelper.ExecuteNonQuery("INSERT INTO AuditLogs (UserId, Action, Details, IpAddress) VALUES (@p0, @p1, @p2, @p3)",
                                     userId, action, details, ip)
        Catch
            ' Silently fail - audit logging should not break the app
        End Try
    End Sub

    Public Shared Function GetClientIp(context As HttpContext) As String
        Dim ip As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(ip) Then
            ip = context.Request.ServerVariables("REMOTE_ADDR")
        End If
        If String.IsNullOrEmpty(ip) Then ip = "127.0.0.1"
        Return ip
    End Function

    ' --- Helpers ---

    Public Shared Function GetInitials(displayName As String) As String
        If String.IsNullOrEmpty(displayName) Then Return "?"
        Dim parts() As String = displayName.Trim().Split(" "c)
        If parts.Length >= 2 Then
            Return (parts(0)(0).ToString() & parts(1)(0).ToString()).ToUpper()
        End If
        Return parts(0)(0).ToString().ToUpper()
    End Function

    Public Shared Function TimeAgo(dt As DateTime) As String
        Dim span As TimeSpan = DateTime.Now.Subtract(dt)
        If span.TotalMinutes < 1 Then Return "just now"
        If span.TotalMinutes < 60 Then Return Math.Floor(span.TotalMinutes).ToString() & "m ago"
        If span.TotalHours < 24 Then Return Math.Floor(span.TotalHours).ToString() & "h ago"
        If span.TotalDays < 7 Then Return Math.Floor(span.TotalDays).ToString() & "d ago"
        If span.TotalDays < 30 Then Return Math.Floor(span.TotalDays / 7).ToString() & "w ago"
        Return dt.ToString("MMM dd, yyyy")
    End Function

End Class
