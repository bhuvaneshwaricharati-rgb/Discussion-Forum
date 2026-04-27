<%@ Page Title="Admin Dashboard" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.vb" Inherits="Discussion_Forum.Discussion_Forum.AdminDashboardPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-page">
        <!-- Admin Header -->
        <div class="admin-hero">
            <div class="admin-hero-content">
                <div class="admin-hero-icon">⚙️</div>
                <div>
                    <h1>Admin Dashboard</h1>
                    <p>Forum health, metrics, and security overview</p>
                </div>
            </div>
        </div>

        <div class="admin-body">
            <!-- Admin Nav Tabs -->
            <div class="admin-tabs">
                <a href="Dashboard" class="admin-tab active">📊 Dashboard</a>
                <a href="Users" class="admin-tab">👥 Users</a>
                <a href="AuditLogs" class="admin-tab">📋 Audit Logs</a>
            </div>

            <!-- Stats Grid -->
            <div class="admin-stats-grid">
                <div class="admin-stat-card stat-blue">
                    <div class="admin-stat-icon">👥</div>
                    <div class="admin-stat-info">
                        <div class="admin-stat-number"><asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label></div>
                        <div class="admin-stat-label">Total Users</div>
                    </div>
                </div>
                <div class="admin-stat-card stat-green">
                    <div class="admin-stat-icon">✅</div>
                    <div class="admin-stat-info">
                        <div class="admin-stat-number"><asp:Label ID="lblActiveUsers" runat="server" Text="0"></asp:Label></div>
                        <div class="admin-stat-label">Active Users</div>
                    </div>
                </div>
                <div class="admin-stat-card stat-purple">
                    <div class="admin-stat-icon">💬</div>
                    <div class="admin-stat-info">
                        <div class="admin-stat-number"><asp:Label ID="lblTotalThreads" runat="server" Text="0"></asp:Label></div>
                        <div class="admin-stat-label">Total Threads</div>
                    </div>
                </div>
                <div class="admin-stat-card stat-amber">
                    <div class="admin-stat-icon">💭</div>
                    <div class="admin-stat-info">
                        <div class="admin-stat-number"><asp:Label ID="lblTotalReplies" runat="server" Text="0"></asp:Label></div>
                        <div class="admin-stat-label">Total Replies</div>
                    </div>
                </div>
                <div class="admin-stat-card stat-red">
                    <div class="admin-stat-icon">🔥</div>
                    <div class="admin-stat-info">
                        <div class="admin-stat-number"><asp:Label ID="lblActiveThreads" runat="server" Text="0"></asp:Label></div>
                        <div class="admin-stat-label">Active Threads</div>
                    </div>
                </div>
                <div class="admin-stat-card stat-teal">
                    <div class="admin-stat-icon">📈</div>
                    <div class="admin-stat-info">
                        <div class="admin-stat-number"><asp:Label ID="lblWeeklyThreads" runat="server" Text="0"></asp:Label></div>
                        <div class="admin-stat-label">This Week</div>
                    </div>
                </div>
            </div>

            <!-- Two Column Grid -->
            <div class="admin-grid-2col">
                <!-- Top Contributors -->
                <div class="admin-panel">
                    <div class="admin-panel-header">
                        <h3>🏆 Top Contributors</h3>
                    </div>
                    <div class="admin-panel-body">
                        <table class="admin-data-table">
                            <thead>
                                <tr><th>#</th><th>User</th><th>Role</th><th>Posts</th></tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptTopContributors" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="rank-cell"><%# Container.ItemIndex + 1 %></td>
                                            <td>
                                                <div class="d-flex align-center gap-1">
                                                    <div class="contributor-avatar" style="width:28px; height:28px; font-size:10px;"><%# AuthHelper.GetInitials(Eval("DisplayName").ToString()) %></div>
                                                    <span style="font-weight:600;"><%# Server.HtmlEncode(Eval("DisplayName").ToString()) %></span>
                                                </div>
                                            </td>
                                            <td><span class='badge-role <%# GetRoleBadgeClass(Eval("Role").ToString()) %>'><%# Eval("Role") %></span></td>
                                            <td class="number-cell"><%# Eval("PostCount") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>

                <!-- Recent Activity -->
                <div class="admin-panel">
                    <div class="admin-panel-header">
                        <h3>📋 Recent Activity</h3>
                        <a href="AuditLogs" class="btn-forum btn-outline btn-sm">View All</a>
                    </div>
                    <div class="admin-panel-body">
                        <table class="admin-data-table">
                            <thead>
                                <tr><th>User</th><th>Action</th><th>Time</th></tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRecentLogs" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="font-weight:600;"><%# Server.HtmlEncode(Eval("Username").ToString()) %></td>
                                            <td><span class='badge-status <%# GetActionBadgeClass(Eval("Action").ToString()) %>'><%# Eval("Action") %></span></td>
                                            <td class="time-cell"><%# AuthHelper.TimeAgo(CDate(Eval("CreatedAt"))) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
