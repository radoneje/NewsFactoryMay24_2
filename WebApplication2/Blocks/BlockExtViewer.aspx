<%@ Page Title="NewsFactory NewsBlockView" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="BlockExtViewer.aspx.cs" Inherits="WebApplication2.Blocks.BlockExtViewer" Async="True" %>

<%@ Register Src="~/Elements/BlocksExtViewer/AlertPanel.ascx" TagPrefix="uc1" TagName="AlertPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Styles/old.css" rel="stylesheet" />
    <link href="/Styles/BlockEditor.css" rel="stylesheet" />
       <script src="/Scripts/video.js"></script>
<link href="/Content/video-js.min.css" rel="stylesheet" />
    <script src="/Scripts/Utils.js"></script>
    <script src="/Scripts/BlockExtViewer.js"></script>

    <meta name="viewport" content="width=device-width, initial-scale=1"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6">
                <asp:PlaceHolder ID="AlertContainer" runat="server">
                  </asp:PlaceHolder>
            </div>
            </div>
  <div class="row">
    <div class="col-md-8"><asp:PlaceHolder ID="BlockNameContainer" runat="server">
                  </asp:PlaceHolder></div>
  <div class="col-md-4">
      <asp:PlaceHolder ID="BlockMediaContainer" runat="server"></asp:PlaceHolder>
  </div>
  </div>
</div>
     <iframe id="DownloadIFrame" style='display:none;'>
    </iframe>
</asp:Content>
