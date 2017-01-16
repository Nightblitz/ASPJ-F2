<%@ Page Title="Register Profile Picture" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterProfilePicture.aspx.cs" Inherits="FileFinder.Account.RegisterProfilePicture" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
        <asp:Label runat="server" ID="ProfilePicture" CssClass="col-md-2 control-label" Font-Bold="true">Profile Picture</asp:Label>
        <div class="col-md-10">
            <asp:FileUpload ID="ProfilePictureUpload" runat="server" />
            <br />
            <asp:Button ID="ButtonUpload" Text="Upload" runat="server" OnClick="ButtonUpload_Click" />
            <br />
            <asp:Panel ID="ProfilePicturePanel" runat="server"></asp:Panel>
            <asp:TextBox ID="ProfilePictureFileName" runat="server" Visible="false"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <br />
            <asp:Button runat="server" OnClick="ProfilePictureNext_Click" Text="Next" CssClass="btn btn-default" />
        </div>
    </div>
</asp:Content>
