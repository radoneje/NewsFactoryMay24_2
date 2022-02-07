<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsItem.ascx.cs" Inherits="WebApplication2.LIte.NewsElements.NewsItem" %>
<asp:Panel ID="ItemPanel" runat="server">
<div class="LileNewsItem" style=" border-bottom: solid 1px rgb(223, 223, 223);" onclick="window.location.href='/LNews/<%=NewsIdHidden.Value %>/#blocks'">
    <asp:HiddenField ID="NewsIdHidden" runat="server" />
    <asp:Literal ID="NewsLinkLiteral" runat="server">
    </asp:Literal>
<h4>
    <asp:Literal ID="NewsNameLiteral" runat="server">
    </asp:Literal>

<br /><small>
<asp:Literal ID="NewsDateLiteral" runat="server">
    </asp:Literal>
    </small></h4>
</div>
</asp:Panel>
