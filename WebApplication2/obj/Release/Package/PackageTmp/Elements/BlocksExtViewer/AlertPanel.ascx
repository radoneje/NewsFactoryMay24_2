<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertPanel.ascx.cs" Inherits="WebApplication2.Elements.BlocksExtViewer.AlertPanel" %>
<div class="alert alert-danger" role="alert">
    <strong>Ошибка! </strong>
    <asp:Literal ID="ErrMessageControl" runat="server"></asp:Literal>
</div>