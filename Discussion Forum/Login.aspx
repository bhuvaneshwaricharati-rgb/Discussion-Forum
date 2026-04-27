<%@ Page Title="Login" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.vb" Inherits="Discussion_Forum.LoginPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="auth-wrapper">
        <div class="auth-card">
            <div class="auth-logo">
                <div class="auth-icon">🔐</div>
                <h1>Welcome Back</h1>
                <p>Sign in to your Discussion Forum account</p>
            </div>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert-forum alert-error">
                <span>⚠️</span>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert-forum alert-success">
                <span>✅</span>
                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
            </asp:Panel>

            <div class="auth-form">
                <div class="form-group">
                    <label for="txtEmail">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Enter your password" TextMode="Password"></asp:TextBox>
                </div>
                <div style="margin-top:24px;">
                    <asp:Button ID="btnLogin" runat="server" Text="🔓 Sign In" CssClass="btn-forum btn-primary btn-block btn-lg" OnClick="btnLogin_Click" />
                </div>
            </div>

            <div class="auth-footer">
                Don't have an account? <a href="Register">Create one now</a>
            </div>
        </div>
    </div>

</asp:Content>
