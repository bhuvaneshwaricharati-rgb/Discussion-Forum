Public Class ErrorPage
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim errorCode As String = Request.QueryString("code")

            Select Case errorCode
                Case "404"
                    lblErrorCode.Text = "404"
                    lblErrorTitle.Text = "Page Not Found"
                    lblErrorMessage.Text = "The page you are looking for does not exist or has been moved. Please check the URL and try again."
                Case "403"
                    lblErrorCode.Text = "403"
                    lblErrorTitle.Text = "Access Denied"
                    lblErrorMessage.Text = "You do not have permission to access this resource. Please contact an administrator if you believe this is an error."
                Case "401"
                    lblErrorCode.Text = "401"
                    lblErrorTitle.Text = "Unauthorized"
                    lblErrorMessage.Text = "You need to sign in to access this page. Please log in and try again."
                Case Else
                    lblErrorCode.Text = "500"
                    lblErrorTitle.Text = "Something Went Wrong"
                    lblErrorMessage.Text = "We encountered an unexpected error. Don't worry — our team has been notified. Please try again later."
            End Select

            Page.Title = "Error " & lblErrorCode.Text
        End If
    End Sub

End Class
