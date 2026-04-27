Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        ' Initialize database
        Try
            DbHelper.InitializeDatabase()
        Catch ex As Exception
            ' Log error but don't prevent app from starting
            System.Diagnostics.Debug.WriteLine("DB Init Error: " & ex.Message)
        End Try
    End Sub

End Class