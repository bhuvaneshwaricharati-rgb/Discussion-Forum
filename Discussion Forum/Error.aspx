<%@ Page Title="Error" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.vb" Inherits="Discussion_Forum.ErrorPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="error-wrapper">
        <div class="error-code"><asp:Label ID="lblErrorCode" runat="server" Text="500"></asp:Label></div>
        <div class="error-title"><asp:Label ID="lblErrorTitle" runat="server" Text="Something went wrong"></asp:Label></div>
        <div class="error-message">
            <asp:Label ID="lblErrorMessage" runat="server" Text="We encountered an unexpected error. Don't worry — our team has been notified and is working on a fix. Please try again later."></asp:Label>
        </div>
        <div style="display:flex; gap:12px;">
            <a href="./" class="btn-forum btn-primary btn-lg">🏠 Go Home</a>
            <a href="javascript:history.back()" class="btn-forum btn-ghost btn-lg">← Go Back</a>
        </div>
    </div>

</asp:Content>
