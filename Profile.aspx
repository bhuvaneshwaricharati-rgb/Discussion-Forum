<%@ Page Title="Profile" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.vb" Inherits="Discussion_Forum.ProfilePage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page">
        <!-- Cover Banner -->
        <div class="profile-cover">
            <div class="profile-cover-gradient"></div>
        </div>

        <!-- Profile Card -->
        <div class="profile-card-wrapper">
            <div class="profile-main-card">
                <div class="profile-avatar-section">
                    <div class="profile-avatar-ring">
                        <div class="profile-avatar-lg" id="divProfileAvatar" runat="server"></div>
                    </div>
                    <asp:Panel ID="pnlEditBtn" runat="server" Visible="false">
                        <asp:Button ID="btnToggleEdit" runat="server" Text="Edit Profile" CssClass="btn-forum btn-outline btn-sm" OnClick="btnToggleEdit_Click" />
                    </asp:Panel>
                </div>

                <div class="profile-info-section">
                    <h1 class="profile-display-name"><asp:Label ID="lblDisplayName" runat="server"></asp:Label></h1>
                    <div class="profile-handle">@<asp:Label ID="lblUsername" runat="server"></asp:Label></div>
                    <asp:Label ID="lblRoleBadge" runat="server" CssClass="badge-role" style="margin-top:8px; display:inline-block;"></asp:Label>
                    <p class="profile-bio-text"><asp:Label ID="lblBio" runat="server"></asp:Label></p>
                    <div class="profile-meta-row">
                        <span class="profile-meta-item">📅 Joined <asp:Label ID="lblJoined" runat="server"></asp:Label></span>
                    </div>
                </div>

                <!-- Stats Cards -->
                <div class="profile-stats-grid">
                    <div class="profile-stat-card">
                        <div class="profile-stat-icon">💬</div>
                        <div class="profile-stat-number"><asp:Label ID="lblThreadCount" runat="server" Text="0"></asp:Label></div>
                        <div class="profile-stat-title">Threads</div>
                    </div>
                    <div class="profile-stat-card">
                        <div class="profile-stat-icon">💭</div>
                        <div class="profile-stat-number"><asp:Label ID="lblReplyCount" runat="server" Text="0"></asp:Label></div>
                        <div class="profile-stat-title">Replies</div>
                    </div>
                    <div class="profile-stat-card">
                        <div class="profile-stat-icon">❤️</div>
                        <div class="profile-stat-number"><asp:Label ID="lblLikesReceived" runat="server" Text="0"></asp:Label></div>
                        <div class="profile-stat-title">Likes Earned</div>
                    </div>
                </div>
            </div>

            <!-- Edit Profile Form -->
            <asp:Panel ID="pnlEditForm" runat="server" Visible="false" CssClass="profile-edit-panel">
                <h3 class="section-title">Edit Profile</h3>
                <asp:Panel ID="pnlEditError" runat="server" Visible="false" CssClass="alert-forum alert-error">
                    <span>⚠️</span>
                    <asp:Label ID="lblEditError" runat="server"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlEditSuccess" runat="server" Visible="false" CssClass="alert-forum alert-success">
                    <span>✅</span>
                    <asp:Label ID="lblEditSuccess" runat="server"></asp:Label>
                </asp:Panel>
                <div class="form-group">
                    <label>Display Name</label>
                    <asp:TextBox ID="txtEditDisplayName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Bio</label>
                    <asp:TextBox ID="txtEditBio" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500" placeholder="Tell us about yourself..."></asp:TextBox>
                </div>
                <div class="profile-edit-actions">
                    <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" CssClass="btn-forum btn-primary" OnClick="btnSaveProfile_Click" />
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="btn-forum btn-outline" OnClick="btnCancelEdit_Click" />
                </div>
            </asp:Panel>

            <!-- Recent Threads Section -->
            <div class="profile-threads-section">
                <h3 class="section-title">Recent Threads</h3>
                <asp:Repeater ID="rptUserThreads" runat="server">
                    <ItemTemplate>
                        <div class="thread-card" onclick="window.location='ThreadView?id=<%# Eval("Id") %>'">
                            <div class="thread-title" style="font-size:1rem;"><%# Server.HtmlEncode(Eval("Title").ToString()) %></div>
                            <div class="thread-body-preview" style="font-size:0.85rem;"><%# Server.HtmlEncode(Eval("Body").ToString()) %></div>
                            <div class="thread-stats" style="font-size:0.75rem;">
                                <span class="stat">💬 <%# Eval("ReplyCount") %> replies</span>
                                <span class="stat">👁️ <%# Eval("ViewCount") %> views</span>
                                <span class="stat"><%# AuthHelper.TimeAgo(CDate(Eval("CreatedAt"))) %></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlNoThreads" runat="server" Visible="false" CssClass="empty-state">
                    <div class="empty-icon">📝</div>
                    <p>No threads posted yet.</p>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>
