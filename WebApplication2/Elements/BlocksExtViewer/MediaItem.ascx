<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaItem.ascx.cs" Inherits="WebApplication2.Elements.BlocksExtViewer.MediaItem" %>
<div class="media">
  <div class="media-left media-middle">
    <a href="#">
        <asp:HyperLink ID="HyperLink1" runat="server">
        <asp:Image ID="MediaImage" runat="server" Width="64px" style="cursor:pointer;"/>
            </asp:HyperLink>
    </a>
  </div>
  <div class="media-body">
    <h6 class="media-heading">
        <asp:Literal ID="MediaTypeLiteral" runat="server"></asp:Literal>
        <asp:Literal ID="MediaNameLiteral" runat="server"></asp:Literal></h6>
      <asp:Literal ID="DownloadButtonPlaceholder" runat="server"></asp:Literal>
        
  </div>
</div>