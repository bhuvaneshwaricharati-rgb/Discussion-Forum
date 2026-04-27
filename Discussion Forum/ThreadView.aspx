<%@ Page Title="Thread" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ThreadView.aspx.vb" Inherits="Discussion_Forum.ThreadViewPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div style="max-width:700px; margin:0 auto; border-left:1px solid var(--color-border); border-right:1px solid var(--color-border); min-height:calc(100vh - 64px);">

        <!-- Back Button -->
        <div class="feed-header" style="display:flex; align-items:center; gap:12px;">
            <a href="./" class="btn-forum btn-ghost btn-sm">← Back</a>
            <div>
                <h1 style="font-size:1.2rem;"><asp:Label ID="lblTitle" runat="server"></asp:Label></h1>
            </div>
        </div>

        <!-- Thread Detail -->
        <div class="thread-detail-header">
            <div class="thread-card-header">
                <div class="thread-avatar" id="divAvatar" runat="server"></div>
                <div class="thread-meta">
                    <div class="thread-author-info">
                        <span class="thread-author"><asp:Label ID="lblAuthor" runat="server"></asp:Label></span>
                        <asp:Label ID="lblRoleBadge" runat="server" CssClass="badge-role"></asp:Label>
                        <span class="thread-time">· <asp:Label ID="lblTime" runat="server"></asp:Label></span>
                    </div>
                </div>
                <asp:Label ID="lblStatusBadge" runat="server" CssClass="badge-status"></asp:Label>
            </div>
        </div>

        <div class="thread-detail-body">
            <asp:Label ID="lblBody" runat="server"></asp:Label>
            <div class="thread-stats" style="margin-top:20px;">
                <span class="stat views-stat">👁️ <asp:Label ID="lblViews" runat="server"></asp:Label> views</span>
                <span class="stat replies-stat">💬 <asp:Label ID="lblReplyCount" runat="server"></asp:Label> replies</span>
            </div>
        </div>

        <!-- Thread Actions (Mod/Admin only) -->
        <asp:Panel ID="pnlModActions" runat="server" Visible="false" style="padding:12px 24px; border-bottom:1px solid var(--color-border); display:flex; gap:8px; flex-wrap:wrap;">
            <asp:Button ID="btnClose" runat="server" Text="🔒 Close" CssClass="btn-forum btn-ghost btn-sm" OnClick="btnClose_Click" />
            <asp:Button ID="btnPin" runat="server" Text="📌 Pin" CssClass="btn-forum btn-ghost btn-sm" OnClick="btnPin_Click" />
            <asp:Button ID="btnArchive" runat="server" Text="📦 Archive" CssClass="btn-forum btn-ghost btn-sm" OnClick="btnArchive_Click" />
            <asp:Button ID="btnReopen" runat="server" Text="🔓 Reopen" CssClass="btn-forum btn-ghost btn-sm" OnClick="btnReopen_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="🗑️ Delete" CssClass="btn-forum btn-danger btn-sm" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this thread?');" />
        </asp:Panel>

        <!-- Replies Section -->
        <div style="border-bottom:1px solid var(--color-border); padding:16px 24px;">
            <h3 style="font-size:1rem; font-weight:700;">Replies</h3>
        </div>

        <asp:Repeater ID="rptReplies" runat="server" OnItemCommand="rptReplies_ItemCommand">
            <ItemTemplate>
                <div class="reply-card">
                    <div class="reply-header">
                        <div class='reply-avatar <%# GetReplyAvatarClass(Eval("Role").ToString()) %>'>
                            <%# AuthHelper.GetInitials(Eval("DisplayName").ToString()) %>
                        </div>
                        <div class="thread-meta">
                            <div class="thread-author-info">
                                <span class="thread-author" style="font-size:0.85rem;"><%# Server.HtmlEncode(Eval("DisplayName").ToString()) %></span>
                                <span class='badge-role <%# GetReplyRoleBadgeClass(Eval("Role").ToString()) %>' style="font-size:0.65rem;"><%# Eval("Role") %></span>
                                <span class="thread-time">· <%# AuthHelper.TimeAgo(CDate(Eval("CreatedAt"))) %></span>
                                <%# If(CBool(Eval("IsEdited")), "<span class='text-muted' style='font-size:0.65rem;'>(edited)</span>", "") %>
                            </div>
                        </div>
                    </div>
                    <div class="reply-body"><%# Server.HtmlEncode(Eval("Body").ToString()) %></div>
                    <div class="reply-actions">
                        <asp:LinkButton ID="btnLike" runat="server" CssClass='action-btn' CommandName="Like" CommandArgument='<%# Eval("Id") %>'>
                            ❤️ <%# Eval("LikeCount") %>
                        </asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlNoReplies" runat="server" Visible="false" style="padding:32px; text-align:center;">
            <p class="text-muted">No replies yet. Be the first to respond!</p>
        </asp:Panel>

        <!-- Reply Compose Box -->
        <asp:Panel ID="pnlReplyForm" runat="server" style="padding:24px; border-top:1px solid var(--color-border);">
            <asp:Panel ID="pnlReplyError" runat="server" Visible="false" CssClass="alert-forum alert-error">
                <span>⚠️</span>
                <asp:Label ID="lblReplyError" runat="server"></asp:Label>
            </asp:Panel>
            <div class="compose-box" style="padding:0; border:none;">
                <div class="reply-avatar" style="margin-top:4px;">
                    <asp:Label ID="lblMyInitials" runat="server"></asp:Label>
                </div>
                <div style="flex:1;">
                    <asp:TextBox ID="txtReply" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Write your reply..." style="resize:vertical; min-height:80px;"></asp:TextBox>
                    <div style="margin-top:12px; text-align:right;">
                        <asp:Button ID="btnReply" runat="server" Text="💬 Reply" CssClass="btn-forum btn-primary" OnClick="btnReply_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlLoginPrompt" runat="server" Visible="false" style="padding:24px; text-align:center; border-top:1px solid var(--color-border);">
            <p class="text-muted">
                <a href="Login">Sign in</a> to join the conversation.
            </p>
        </asp:Panel>

    </div>

</asp:Content>
