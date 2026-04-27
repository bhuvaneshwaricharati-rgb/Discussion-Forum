<%@ Page Title="New Discussion" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewThread.aspx.vb" Inherits="Discussion_Forum.NewThreadPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div style="max-width:700px; margin:0 auto; padding:32px 16px;">
        <div class="feed-header" style="position:relative; background:transparent; backdrop-filter:none; border:none; padding:0 0 24px 0;">
            <h1>✏️ New Discussion</h1>
            <p>Start a new conversation with the community</p>
        </div>

        <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert-forum alert-error">
            <span>⚠️</span>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </asp:Panel>

        <div class="forum-card no-hover">
            <div class="auth-form">
                <div class="form-group">
                    <label>Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="What do you want to discuss?" MaxLength="200"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Body</label>
                    <asp:TextBox ID="txtBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="8" placeholder="Share your thoughts, ideas, or questions..." style="min-height:180px; resize:vertical;"></asp:TextBox>
                </div>
                <div style="display:flex; gap:12px; margin-top:24px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="🚀 Post Discussion" CssClass="btn-forum btn-primary btn-lg" OnClick="btnSubmit_Click" />
                    <a href="./" class="btn-forum btn-ghost btn-lg">Cancel</a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
