<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notification.aspx.cs" Inherits="ASPJ_F2.Notification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label3" runat="server" Text="jc  page"></asp:Label>
    <asp:Button ID="LikeB" runat="server" Text="Like" OnClick="LikeB_Click" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"></asp:Timer>
            <asp:Label ID="Label1" runat="server" Text="Panel not refreshed yet."></asp:Label>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
</asp:Content>
