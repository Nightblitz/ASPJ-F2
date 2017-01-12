<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImageShare.aspx.cs" Inherits="ASPJ_F2.ImageShare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h1>Files - How to prepare Your Design!!
                        </h1>
                    </div>
                    <div class="panel-body">
                        <div class="col-xs-8 col-sm-6">
                            <div class="form-group">
                                <h2>Things to remember:</h2>
                                <ul>
                                    <li>We accept only .PNG files only</li>
                                    <li>Only 1 file is allowed to be uploaded</li>
                                    <li>100MB is the maximum total file size</li>
                                </ul>
                            </div>
                        </div>
                        <div class="col-xs-8 col-sm-6">
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Details of Design</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="Label1" runat="server" Text="Title:"></asp:Label>
                                            </div>
                                            <asp:Label ID="DesignTitleLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="Label2" runat="server" Text="Category:"></asp:Label>
                                            </div>
                                            <asp:Label ID="CategoryLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="label3" runat="server" Text="Cost:"></asp:Label>
                                            </div>
                                            <asp:Label ID="CostLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="Label4" runat="server" Text="Seller:"></asp:Label>
                                            </div>
                                            <asp:Label ID="SellerLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="container">
                            <div class="col-lg-6">
                                <label for="inputMainFile">Main File</label>
                                <asp:FileUpload class="custom-file-input" ID="FileUploadMain" runat="server" />
                                <asp:Button ID="PreviewBtnMain" Style="margin: 5px;" class="btn btn-success" runat="server" OnClick="btnPreviewMain_Click" Text="Preview" />
                                <asp:Button ID="DeleteBtnMain" Style="margin: 5px;" class="btn btn-danger" Text="Delete" runat="server" OnClick="btnDeleteMain_Click" />
                                <div class="form-group">
                                    <asp:Image ID="MainUploadedImage" BorderColor="Black" BorderStyle="Solid" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="StatusLabelMain" Style="font-size: medium" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="panel panel-danger">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">Display Images
                                        </h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <label for="XtraStuff">**Images here will be displayed for customers</label>
                                        </div>
                                        <div class="form-group">
                                            <asp:Image ID="SecondaryUploadedImage" BorderColor="Black" BorderStyle="Solid" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="ShareBtn" class="btn btn-warning" runat="server" Text="Share" OnClick="ShareBtn_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<fieldset>
        <legend>Share/Sell Your Designs</legend>
        <div class="form-group">
            <label for="inputTitle">Design Title</label>
            <asp:TextBox ID="TitleTextBox" MaxLength="20" class="form-control" placeholder="Elephants" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TitleTextBox"
                ForeColor="Red" ErrorMessage="**Please Enter Design Title!!">
            </asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <div class="row">
                <h4>&nbsp;&nbsp;&nbsp; Files - How to prepare Your Design</h4>
                <h5>&nbsp;&nbsp;&nbsp;&nbsp; Things to remember:</h5>
                <ul>
                    <li>We accept only .PNG files only</li>
                    <li>10 is the maximum number of files</li>
                    <li>100MB is the maximum total file size</li>
                </ul>
                <div class="col-lg-6">
                    <label for="inputMainFile">Main File</label>
                    <asp:FileUpload class="custom-file-input" ID="FileUpload1" onclick="Browse_click" runat="server" />
                    <asp:Button ID="PreviewButton" Style="margin: 5px;" class="btn btn-success" runat="server" OnClick="btnPreview_Click" Text="Preview" />
                    <asp:Button ID="btnDelete" Style="margin: 5px;" class="btn btn-danger" Text="Delete" runat="server" OnClick="btnDelete_Click" />
                </div>
                <div class="col-lg-6">
                    <label for="inputSecondaryFiles">Display Files</label>
                    <asp:FileUpload ID="FileUpload2" runat="server" />
                    <asp:Button ID="PreviewButton2" Style="margin: 5px;" class="btn btn-success" runat="server" OnClick="btnPreview2_Click" Text="Preview" />
                    <asp:Button ID="btnDelete2" Style="margin: 5px;" class="btn btn-danger" Text="Delete" runat="server" OnClick="btnDelete2_Click" />
                    <asp:Repeater ID="rptrUserPhotos" runat="server">
                        <ItemTemplate>
                            <span class="saucer">
                                <asp:ImageButton OnCommand="imgUserPhoto_Command" CommandArgument="<%# Container.DataItem %>" ImageUrl="<%# Container.DataItem %>" ID="imgUserPhoto" Style="width: 100px; height: 100px;" runat="server" alt="" />
                                <asp:CheckBox ID="cbDelete" special="<%# Container.DataItem %>" ImageUrl="<%# Container.DataItem %>" Text="Delete" runat="server" />
                            </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="row">
                <asp:Label ID="StatusLabel" Style="font-size: medium" runat="server"></asp:Label>
            </div>
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
                    <input type="text" class="form-control" id="AmountTextBox" placeholder="Amount">
                    <div class="input-group-addon">.00</div>
                </div>
            </div>
        </div>
        <asp:Button ID="UploadBtn" class="btn btn-primary" runat="server" OnClick="UploadBtn_Click" Text="Upload" />
    </fieldset>--%>
</asp:Content>