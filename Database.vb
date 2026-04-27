Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO

Public Class DbHelper

    Private Shared _connectionString As String = Nothing

    Public Shared ReadOnly Property ConnectionString As String
        Get
            If _connectionString Is Nothing Then
                _connectionString = ConfigurationManager.ConnectionStrings("ForumDB").ConnectionString
            End If
            Return _connectionString
        End Get
    End Property

    ''' <summary>
    ''' Initialize the database: create tables and seed default data
    ''' </summary>
    Public Shared Sub InitializeDatabase()
        ' Run schema script
        Dim schemaPath As String = HttpContext.Current.Server.MapPath("~/App_Data/forum.sql")
        If File.Exists(schemaPath) Then
            Dim schemaSql As String = File.ReadAllText(schemaPath)
            ExecuteNonQuery(schemaSql)
        End If

        ' Seed default users if none exist
        Dim userCount As Object = ExecuteScalar("SELECT COUNT(*) FROM Users")
        If Convert.ToInt32(userCount) = 0 Then
            SeedDefaultData()
        End If
    End Sub

    Private Shared Sub SeedDefaultData()
        ' Admin user
        Dim adminHash As String = AuthHelper.HashPassword("Admin123!")
        ExecuteNonQuery("INSERT INTO Users (Username, Email, PasswordHash, DisplayName, Bio, Role, IsActive) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, 1)",
                        "admin", "admin@forum.com", adminHash, "Administrator", "Forum administrator with full access.", "Admin")

        ' Moderator user
        Dim modHash As String = AuthHelper.HashPassword("Mod123!")
        ExecuteNonQuery("INSERT INTO Users (Username, Email, PasswordHash, DisplayName, Bio, Role, IsActive) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, 1)",
                        "moderator", "mod@forum.com", modHash, "Moderator", "Forum moderator helping keep discussions on track.", "Moderator")

        ' Demo user
        Dim userHash As String = AuthHelper.HashPassword("User123!")
        ExecuteNonQuery("INSERT INTO Users (Username, Email, PasswordHash, DisplayName, Bio, Role, IsActive) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, 1)",
                        "johndoe", "john@example.com", userHash, "John Doe", "Software developer and tech enthusiast.", "User")

        ' Seed sample threads
        ExecuteNonQuery("INSERT INTO Threads (Title, Body, AuthorId, Status, ViewCount) VALUES (@p0, @p1, 1, 'Pinned', 42)",
                        "Welcome to the Discussion Forum!",
                        "Welcome everyone! This is our new professional discussion forum. Feel free to introduce yourself, ask questions, and participate in meaningful conversations. Please remember to follow our community guidelines and treat everyone with respect.")

        ExecuteNonQuery("INSERT INTO Threads (Title, Body, AuthorId, Status, ViewCount) VALUES (@p0, @p1, 3, 'Active', 18)",
                        "Best practices for secure coding in 2024",
                        "I have been reading up on OWASP Top 10 and wanted to share some thoughts on modern secure coding practices. Input validation, parameterized queries, and proper authentication are just the basics. What security measures does your team prioritize?")

        ExecuteNonQuery("INSERT INTO Threads (Title, Body, AuthorId, Status, ViewCount) VALUES (@p0, @p1, 2, 'Active', 25)",
                        "Tips for effective code reviews",
                        "Code reviews are critical for maintaining quality. Here are some tips that have worked for our team: focus on logic not style, use automated linting, keep PRs small, and always be constructive. What are your best practices?")

        ' Seed sample replies
        ExecuteNonQuery("INSERT INTO Replies (ThreadId, AuthorId, Body, LikeCount) VALUES (1, 3, @p0, 5)",
                        "Thanks for setting this up! Looking forward to great discussions here.")

        ExecuteNonQuery("INSERT INTO Replies (ThreadId, AuthorId, Body, LikeCount) VALUES (1, 2, @p0, 3)",
                        "Welcome to the community everyone! Do not hesitate to reach out to moderators if you need help.")

        ExecuteNonQuery("INSERT INTO Replies (ThreadId, AuthorId, Body, LikeCount) VALUES (2, 1, @p0, 7)",
                        "Great topic! I would add that keeping dependencies updated and using security scanning tools in CI/CD is also essential.")

        ExecuteNonQuery("INSERT INTO Replies (ThreadId, AuthorId, Body, LikeCount) VALUES (2, 2, @p0, 2)",
                        "Agreed. We also use Content Security Policy headers and rate limiting on all our APIs.")

        ExecuteNonQuery("INSERT INTO Replies (ThreadId, AuthorId, Body, LikeCount) VALUES (3, 1, @p0, 4)",
                        "Excellent list! I find that pair programming sessions can sometimes be even more effective than async code reviews.")

        ExecuteNonQuery("INSERT INTO Replies (ThreadId, AuthorId, Body, LikeCount) VALUES (3, 3, @p0, 6)",
                        "Small PRs are a game changer. We set a soft limit of 400 lines and it has dramatically improved our review quality.")

        ' Seed audit logs
        ExecuteNonQuery("INSERT INTO AuditLogs (UserId, Action, Details, IpAddress) VALUES (1, 'Login', 'Administrator logged in', '127.0.0.1')")
        ExecuteNonQuery("INSERT INTO AuditLogs (UserId, Action, Details, IpAddress) VALUES (1, 'CreateThread', 'Created welcome thread', '127.0.0.1')")
    End Sub

    ' --- Core query helpers ---

    Public Shared Function ExecuteScalar(sql As String, ParamArray params() As Object) As Object
        Using conn As New SqlConnection(ConnectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                AddParameters(cmd, params)
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

    Public Shared Function ExecuteNonQuery(sql As String, ParamArray params() As Object) As Integer
        Using conn As New SqlConnection(ConnectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                AddParameters(cmd, params)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Shared Function ExecuteReader(sql As String, ParamArray params() As Object) As DataTable
        Dim dt As New DataTable()
        Using conn As New SqlConnection(ConnectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                AddParameters(cmd, params)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Private Shared Sub AddParameters(cmd As SqlCommand, params() As Object)
        If params IsNot Nothing Then
            For i As Integer = 0 To params.Length - 1
                Dim val As Object = params(i)
                If val Is Nothing Then val = DBNull.Value
                cmd.Parameters.AddWithValue("@p" & i.ToString(), val)
            Next
        End If
    End Sub

End Class
