<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockMediaPanel.ascx.cs" Inherits="WebApplication2.Elements.BlocksExtViewer.BlockMediaPanel" %>

<div class="panel panel-default">
  <div class="panel-heading" style="height:250px">
      

      <asp:PlaceHolder ID="MediaPlayerContainer" runat="server"></asp:PlaceHolder>
  </div>
  <div class="panel-body">
    <asp:PlaceHolder ID="MediaContainer" runat="server"></asp:PlaceHolder>
  </div>
</div>