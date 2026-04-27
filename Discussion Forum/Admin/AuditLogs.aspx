<%@ Page Title="Audit Logs" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuditLogs.aspx.vb" Inherits="Discussion_Forum.Discussion_Forum.AdminAuditLogsPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-page">
        <div class="admin-hero">
            <div class="admin-hero-content">
                <div class="admin-hero-icon">📋</div>
                <div>
                    <h1>Audit Logs</h1>
                    <p>Complete action history with IP tracking for compliance</p>
                </div>
            </div>
        </div>

        <div class="admin-body">
            <div class="admin-tabs">
                <a href="Dashboard" class="admin-tab">📊 Dashboard</a>
                <a href="Users" class="admin-tab">👥 Users</a>
                <a href="AuditLogs" class="admin-tab active">📋 Audit Logs</a>
            </div>

            <!-- Filters Toolbar -->
            <div class="admin-toolbar">
                <div class="search-input-group" style="max-width:300px; flex:1;">
                    <span class="search-icon">🔍</span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by user or action..."></asp:TextBox>
                </div>
                <asp:DropDownList ID="ddlAction" runat="server" CssClass="form-control" style="max-width:200px !important;">
                    <asp:ListItem Value="" Text="All Actions"></asp:ListItem>
                    <asp:ListItem Value="Login">Login</asp:ListItem>
                    <asp:ListItem Value="Logout">Logout</asp:ListItem>
                    <asp:ListItem Value="CreateThread">Create Thread</asp:ListItem>
                    <asp:ListItem Value="CreateReply">Create Reply</asp:ListItem>
                    <asp:ListItem Value="DeleteThread">Delete Thread</asp:ListItem>
                    <asp:ListItem Value="ChangeRole">Change Role</asp:ListItem>
                    <asp:ListItem Value="ToggleUserStatus">Toggle Status</asp:ListItem>
                    <asp:ListItem Value="UpdateThreadStatus">Update Thread</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn-forum btn-primary btn-sm" OnClick="btnFilter_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-forum btn-outline btn-sm" OnClick="btnClear_Click" />
                <span class="admin-toolbar-info">
                    <asp:Label ID="lblLogCount" runat="server" Text="0"></asp:Label> entries
                </span>
            </div>

            <!-- Logs Table -->
            <div class="admin-panel">
                <div class="admin-panel-body" style="padding:0;">
                    <table class="admin-data-table">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>User</th>
                                <th>Action</th>
                                <th>Details</th>
                                <th>IP Address</th>
                                <th>Time</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptLogs" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="rank-cell">#<%# Eval("Id") %></td>
                                        <td style="font-weight:600;"><%# Server.HtmlEncode(Eval("Username").ToString()) %></td>
                                        <td>
                                            <span class='badge-status <%# GetActionBadgeClass(Eval("Action").ToString()) %>'>
                                                <%# Eval("Action") %>
                                            </span>
                                        </td>
                                        <td class="details-cell">
                                            <%# Server.HtmlEncode(If(Eval("Details") Is DBNull.Value, "", Eval("Details").ToString())) %>
                                        </td>
                                        <td class="ip-cell">
                                            <%# If(Eval("IpAddress") Is DBNull.Value, "-", Eval("IpAddress")) %>
                                        </td>
                                        <td class="time-cell" style="white-space:nowrap;">
                                            <%# CDate(Eval("CreatedAt")).ToString("MMM dd, yyyy HH:mm") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

            <asp:Panel ID="pnlNoLogs" runat="server" Visible="false" CssClass="empty-state">
                <div class="empty-icon">📭</div>
                <p>No audit logs found matching your criteria.</p>
            </asp:Panel>
        </div>
    </div>

</asp:Content>
