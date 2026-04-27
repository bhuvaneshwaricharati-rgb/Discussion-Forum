<%@ Page Title="User Management" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.vb" Inherits="Discussion_Forum.Discussion_Forum.AdminUsersPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="admin-page">
        <div class="admin-hero">
            <div class="admin-hero-content">
                <div class="admin-hero-icon">👥</div>
                <div>
                    <h1>User Management</h1>
                    <p>Manage user roles, status, and permissions</p>
                </div>
            </div>
        </div>

        <div class="admin-body">
            <div class="admin-tabs">
                <a href="Dashboard" class="admin-tab">📊 Dashboard</a>
                <a href="Users" class="admin-tab active">👥 Users</a>
                <a href="AuditLogs" class="admin-tab">📋 Audit Logs</a>
            </div>

            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert-forum alert-success">
                <span>✅</span>
                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Search Bar -->
            <div class="admin-toolbar">
                <div class="search-input-group" style="max-width:400px; flex:1;">
                    <span class="search-icon">🔍</span>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search users..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                </div>
                <span class="admin-toolbar-info">
                    <asp:Label ID="lblUserCount" runat="server" Text="0"></asp:Label> users found
                </span>
            </div>

            <!-- Users Table -->
            <div class="admin-panel">
                <div class="admin-panel-body" style="padding:0;">
                    <table class="admin-data-table">
                        <thead>
                            <tr>
                                <th>User</th>
                                <th>Email</th>
                                <th>Role</th>
                                <th>Status</th>
                                <th>Joined</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="d-flex align-center gap-1">
                                                <div class="contributor-avatar" style="width:32px; height:32px; font-size:11px;"><%# AuthHelper.GetInitials(Eval("DisplayName").ToString()) %></div>
                                                <div>
                                                    <div style="font-weight:600;"><%# Server.HtmlEncode(Eval("DisplayName").ToString()) %></div>
                                                    <div style="font-size:0.7rem; color:var(--color-text-secondary);">@<%# Server.HtmlEncode(Eval("Username").ToString()) %></div>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="color:var(--color-text-secondary);"><%# Server.HtmlEncode(Eval("Email").ToString()) %></td>
                                        <td><span class='badge-role <%# GetRoleBadgeClass(Eval("Role").ToString()) %>'><%# Eval("Role") %></span></td>
                                        <td>
                                            <%# If(CBool(Eval("IsActive")), "<span class='badge-status badge-active'>Active</span>", "<span class='badge-status badge-closed'>Inactive</span>") %>
                                        </td>
                                        <td class="time-cell"><%# CDate(Eval("CreatedAt")).ToString("MMM dd, yyyy") %></td>
                                        <td>
                                            <div class="d-flex gap-1" style="flex-wrap:wrap;">
                                                <asp:LinkButton ID="btnMakeAdmin" runat="server" CssClass="btn-forum btn-outline btn-sm" CommandName="SetRole" CommandArgument='<%# Eval("Id") & "|Admin" %>' Visible='<%# Eval("Role").ToString() <> "Admin" %>'>Admin</asp:LinkButton>
                                                <asp:LinkButton ID="btnMakeMod" runat="server" CssClass="btn-forum btn-outline btn-sm" CommandName="SetRole" CommandArgument='<%# Eval("Id") & "|Moderator" %>' Visible='<%# Eval("Role").ToString() <> "Moderator" %>'>Mod</asp:LinkButton>
                                                <asp:LinkButton ID="btnMakeUser" runat="server" CssClass="btn-forum btn-outline btn-sm" CommandName="SetRole" CommandArgument='<%# Eval("Id") & "|User" %>' Visible='<%# Eval("Role").ToString() <> "User" %>'>User</asp:LinkButton>
                                                <asp:LinkButton ID="btnToggleActive" runat="server" CssClass='<%# If(CBool(Eval("IsActive")), "btn-forum btn-danger btn-sm", "btn-forum btn-primary btn-sm") %>' CommandName="ToggleActive" CommandArgument='<%# Eval("Id") %>'>
                                                    <%# If(CBool(Eval("IsActive")), "Deactivate", "Activate") %>
                                                </asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
