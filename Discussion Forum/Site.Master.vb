Public Class SiteMaster
    Inherits MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If AuthHelper.IsLoggedIn(HttpContext.Current) Then
            pnlGuest.Visible = False
            pnlUser.Visible = True
            lblUserDisplay.Text = AuthHelper.GetCurrentUsername(HttpContext.Current)
            ' Show admin link for admins/moderators
            pnlAdmin.Visible = AuthHelper.IsModerator(HttpContext.Current)
        Else
            pnlGuest.Visible = True
            pnlUser.Visible = False
        End If
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        AuthHelper.LogoutUser(HttpContext.Current)
        Response.Redirect("~/Login")
    End Sub

End Class