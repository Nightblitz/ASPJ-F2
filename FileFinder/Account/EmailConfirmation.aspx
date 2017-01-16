<%@ Page Title="Email Confirmation Sent" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailConfirmation.aspx.cs" Inherits="FileFinder.Account.EmailConfirmation" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">
            <p>
                An email has been sent to your account. Please view the email and confirm your account to complete the registration process.             
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
            <p class="text-danger">
                An error has occurred.
            </p>
        </asp:PlaceHolder>
    </div>
</asp:Content>
