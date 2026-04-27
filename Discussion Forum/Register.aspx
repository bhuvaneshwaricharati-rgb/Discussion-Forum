<%@ Page Title="Register" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.vb" Inherits="Discussion_Forum.RegisterPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="auth-wrapper">
        <div class="auth-card">
            <div class="auth-logo">
                <div class="auth-icon">✨</div>
                <h1>Create Account</h1>
                <p>Join the professional discussion community</p>
            </div>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert-forum alert-error">
                <span>⚠️</span>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </asp:Panel>

            <div class="auth-form">
                <div class="form-group">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Choose a username" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtDisplayName">Display Name</label>
                    <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" placeholder="Your full name" MaxLength="100"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtEmail">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="you@example.com" TextMode="Email" MaxLength="100"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Minimum 6 characters" TextMode="Password"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" placeholder="Re-enter your password" TextMode="Password"></asp:TextBox>
                </div>
                <div style="margin-top:24px;">
                    <asp:Button ID="btnRegister" runat="server" Text="🚀 Create Account" CssClass="btn-forum btn-primary btn-block btn-lg" OnClick="btnRegister_Click" />
                </div>
            </div>

            <div class="auth-footer">
                Already have an account? <a href="Login">Sign in</a>
            </div>
        </div>
    </div>

</asp:Content>
