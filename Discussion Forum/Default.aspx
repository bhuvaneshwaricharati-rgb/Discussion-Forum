<%@ Page Title="Home" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Discussion_Forum._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="forum-layout">
        <!-- Left Sidebar -->
        <div class="forum-sidebar">
            <ul class="sidebar-nav">
                <li><a href="./" class="active"><span class="nav-icon">🏠</span> Home</a></li>
                <li><a href="Profile"><span class="nav-icon">👤</span> Profile</a></li>
                <% If AuthHelper.IsModerator(HttpContext.Current) Then %>
                <li><a href="Admin/Dashboard"><span class="nav-icon">⚙️</span> Admin</a></li>
                <% End If %>
            </ul>

            <% If AuthHelper.IsLoggedIn(HttpContext.Current) Then %>
            <div style="padding: 16px; margin-top: 16px;">
                <a href="NewThread" class="btn-forum btn-primary btn-block" style="text-align:center;">
                    ✏️ New Discussion
                </a>
            </div>
            <% End If %>
        </div>

        <!-- Main Feed -->
        <div class="forum-main">
            <div class="feed-header">
                <h1>Home</h1>
                <p>Latest discussions from the community</p>
            </div>

            <!-- Search -->
            <div class="search-wrapper">
                <div class="search-input-group">
                    <span class="search-icon">🔍</span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search discussions..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                </div>
            </div>

            <!-- Thread List -->
            <asp:Repeater ID="rptThreads" runat="server">
                <ItemTemplate>
                    <div class="thread-card" onclick="window.location='ThreadView?id=<%# Eval("Id") %>'">
                        <div class="thread-card-header">
                            <div class='thread-avatar <%# GetAvatarClass(Eval("Role").ToString()) %>'>
                                <%# AuthHelper.GetInitials(Eval("DisplayName").ToString()) %>
                            </div>
                            <div class="thread-meta">
                                <div class="thread-author-info">
                                    <span class="thread-author"><%# Server.HtmlEncode(Eval("DisplayName").ToString()) %></span>
                                    <span class='badge-role <%# GetRoleBadgeClass(Eval("Role").ToString()) %>'><%# Eval("Role") %></span>
                                    <span class="thread-time">· <%# AuthHelper.TimeAgo(CDate(Eval("CreatedAt"))) %></span>
                                </div>
                            </div>
                            <span class='badge-status <%# GetStatusBadgeClass(Eval("Status").ToString()) %>'>
                                <%# Eval("Status") %>
                            </span>
                        </div>
                        <div class="thread-title"><%# Server.HtmlEncode(Eval("Title").ToString()) %></div>
                        <div class="thread-body-preview"><%# Server.HtmlEncode(Eval("Body").ToString()) %></div>
                        <div class="thread-stats">
                            <span class="stat replies-stat">💬 <%# Eval("ReplyCount") %> replies</span>
                            <span class="stat views-stat">👁️ <%# Eval("ViewCount") %> views</span>
                            <span class="stat likes-stat">❤️ <%# Eval("TotalLikes") %> likes</span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlNoThreads" runat="server" Visible="false" style="padding:48px; text-align:center;">
                <div style="font-size:48px; margin-bottom:16px;">📭</div>
                <h3 style="color:var(--color-text-secondary); margin-bottom:8px;">No discussions yet</h3>
                <p class="text-muted">Be the first to start a conversation!</p>
                <% If AuthHelper.IsLoggedIn(HttpContext.Current) Then %>
                <a href="NewThread" class="btn-forum btn-primary" style="margin-top:16px;">✏️ Start a Discussion</a>
                <% End If %>
            </asp:Panel>

            <!-- Pagination -->
            <asp:Panel ID="pnlPagination" runat="server" CssClass="pagination-bar">
            </asp:Panel>
        </div>

        <!-- Right Sidebar -->
        <div class="forum-aside">
            <!-- Forum Stats Widget -->
            <div class="aside-widget">
                <div class="aside-widget-header">📊 Forum Stats</div>
                <div class="aside-widget-body" style="padding:16px;">
                    <div class="d-flex justify-between mb-1">
                        <span class="text-secondary" style="font-size:0.8rem;">Total Threads</span>
                        <span style="font-weight:700; font-size:0.8rem;"><asp:Label ID="lblTotalThreads" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <div class="d-flex justify-between mb-1">
                        <span class="text-secondary" style="font-size:0.8rem;">Total Replies</span>
                        <span style="font-weight:700; font-size:0.8rem;"><asp:Label ID="lblTotalReplies" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <div class="d-flex justify-between">
                        <span class="text-secondary" style="font-size:0.8rem;">Members</span>
                        <span style="font-weight:700; font-size:0.8rem;"><asp:Label ID="lblTotalMembers" runat="server" Text="0"></asp:Label></span>
                    </div>
                </div>
            </div>

            <!-- Top Contributors Widget -->
            <div class="aside-widget">
                <div class="aside-widget-header">🏆 Top Contributors</div>
                <div class="aside-widget-body">
                    <asp:Repeater ID="rptContributors" runat="server">
                        <ItemTemplate>
                            <div class="contributor-item">
                                <span class="contributor-rank"><%# Container.ItemIndex + 1 %></span>
                                <div class="contributor-avatar"><%# AuthHelper.GetInitials(Eval("DisplayName").ToString()) %></div>
                                <div class="contributor-info">
                                    <div class="contributor-name"><%# Server.HtmlEncode(Eval("DisplayName").ToString()) %></div>
                                    <div class="contributor-posts"><%# Eval("PostCount") %> posts</div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>

    <!-- Floating New Thread Button (mobile) -->
    <% If AuthHelper.IsLoggedIn(HttpContext.Current) Then %>
    <a href="NewThread" class="btn-new-thread" title="New Discussion">✏️</a>
    <% End If %>

</asp:Content>
