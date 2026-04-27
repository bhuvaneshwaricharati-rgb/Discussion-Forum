Namespace Discussion_Forum

Public Class AdminUsersPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not AuthHelper.IsModerator(HttpContext.Current) Then
            Response.Redirect("~/Login")
            Return
        End If

        If Not IsPostBack Then
            LoadUsers("")
        End If
    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadUsers(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadUsers(searchTerm As String)
        Try
            Dim sql As String = "SELECT Id, Username, Email, DisplayName, Role, IsActive, CreatedAt FROM Users "
            Dim dt As DataTable

            If Not String.IsNullOrEmpty(searchTerm) Then
                sql &= "WHERE Username LIKE @p0 OR Email LIKE @p0 OR DisplayName LIKE @p0 "
                sql &= "ORDER BY CreatedAt DESC"
                dt = DbHelper.ExecuteReader(sql, "%" & searchTerm & "%")
            Else
                sql &= "ORDER BY CreatedAt DESC"
                dt = DbHelper.ExecuteReader(sql)
            End If

            rptUsers.DataSource = dt
            rptUsers.DataBind()
            lblUserCount.Text = dt.Rows.Count.ToString()
        Catch
        End Try
    End Sub

    Protected Sub rptUsers_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        If e.CommandName = "SetRole" Then
            Dim parts() As String = e.CommandArgument.ToString().Split("|"c)
            Dim userId As Integer = CInt(parts(0))
            Dim newRole As String = parts(1)

            Try
                DbHelper.ExecuteNonQuery("UPDATE Users SET Role = @p0 WHERE Id = @p1", newRole, userId)
                AuthHelper.LogAction(HttpContext.Current, AuthHelper.GetCurrentUserId(HttpContext.Current),
                    "ChangeRole", "Changed user #" & userId & " role to " & newRole)

                pnlSuccess.Visible = True
                lblSuccess.Text = "User role updated to " & newRole & "."
                LoadUsers("")
            Catch
            End Try

        ElseIf e.CommandName = "ToggleActive" Then
            Dim userId As Integer = CInt(e.CommandArgument)

            Try
                ' Toggle IsActive
                DbHelper.ExecuteNonQuery("UPDATE Users SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END WHERE Id = @p0", userId)
                AuthHelper.LogAction(HttpContext.Current, AuthHelper.GetCurrentUserId(HttpContext.Current),
                    "ToggleUserStatus", "Toggled active status for user #" & userId)

                pnlSuccess.Visible = True
                lblSuccess.Text = "User status updated."
                LoadUsers("")
            Catch
            End Try
        End If
    End Sub

    Protected Function GetRoleBadgeClass(role As String) As String
        Select Case role
            Case "Admin" : Return "badge-admin"
            Case "Moderator" : Return "badge-moderator"
            Case Else : Return "badge-user"
        End Select
    End Function

End Class

End Namespace
