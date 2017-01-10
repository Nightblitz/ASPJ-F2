<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SharingCreate.aspx.cs" Inherits="ASPJ_F2.SharingCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Share/Sell Your Designs</legend>
        <div class="form-group">
            <label for="inputTitle">Design Title</label>
            <asp:TextBox ID="TitleTextBox" MaxLength="20" class="form-control" placeholder="Elephants" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TitleTextBox"
                ForeColor="Red" ErrorMessage="**Please Enter Design Title!!">
            </asp:RequiredFieldValidator>
        </div>
        
        <div class="form-group">
            <label for="inputDescription">Design Description</label>
            <asp:TextBox ID="DescriptionTextBox" MaxLength="150" TextMode="multiline" Columns="20" Rows="3" class="form-control" placeholder="This is an awesome design!!" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DescriptionTextBox"
                ForeColor="Red" ErrorMessage="**Please Enter some description!!"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label for="inputCategory">Category</label>
            <div class="input-group-btn">
                <asp:DropDownList ID="CategoryDropDownList" class="form-control" runat="server">
                    <asp:ListItem Text="Select a category" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Arts & Design" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Technology" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Fun" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Fantasy" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Others" Value="5"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CategoryDropDownList"
                    ForeColor="Red" InitialValue="0" ErrorMessage="**Please Select A Category"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <label for="inputTags">Tags</label>
            <asp:TextBox ID="TagsTextBox" MaxLength="15" class="form-control" placeholder="Sweet" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TagsTextBox"
                ForeColor="Red" ErrorMessage="**Please Enter at least one Tag"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <div class="col-sm-3">
                <div class="input-group">
                    <div class="input-group-addon">$</div>
                     <asp:TextBox ID="CostTextBox" MaxLength="15" class="form-control" placeholder="Amount" runat="server"></asp:TextBox>
                    <div class="input-group-addon">.00</div>
                </div>
            </div>
        </div>
        <asp:Button ID="NextBtn" class="btn btn-primary" runat="server" OnClick="NextBtn_Click" Text="Next" />
    </fieldset>
</asp:Content>
