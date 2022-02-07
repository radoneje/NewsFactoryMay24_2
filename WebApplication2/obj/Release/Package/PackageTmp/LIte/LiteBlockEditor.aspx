<%@ Page Title="Lite Block Editor  NewsFactory May24" Language="C#" MasterPageFile="~/Lite.Master" AutoEventWireup="true" CodeBehind="LiteBlockEditor.aspx.cs" Inherits="WebApplication2.LIte.LiteBlockEditor" %>
<%@ Register Src="~/Elements/BlocksExtViewer/AlertPanel.ascx" TagPrefix="uc1" TagName="AlertPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <script src="/Scripts/BlockExtViewer.js"></script>
     <iframe id="DownloadIFrame" style='display:none;'>
    </iframe>
</asp:Content>
