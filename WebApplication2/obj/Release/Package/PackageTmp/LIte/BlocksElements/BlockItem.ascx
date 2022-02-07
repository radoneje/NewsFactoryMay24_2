<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockItem.ascx.cs" Inherits="WebApplication2.LIte.BlocksElements.BlockItem" %>
<asp:HiddenField ID="BlockIdHidden" runat="server" />
    <div style="width:100%; cursor:pointer; border-bottom: solid 1px rgb(223, 223, 223);" onclick="">
        <div style="display:inline-block;width:20px">
            <asp:Image ID="Image1" style="width:20px" runat="server" />
        </div>
        <div style="display:inline-block">
            <small><asp:Literal ID="TypeLiteral" runat="server"></asp:Literal>  <asp:Literal ID="AutorLiteral" runat="server"></asp:Literal></small>
        </div>
        
        <div style="display:inline-block;width:40%; float:right;text-align:right;">
            <small>план: <asp:Literal ID="ChronoPlanLiteral" runat="server"></asp:Literal>
            факт: <asp:Literal ID="ChronoFactLiteral" runat="server"></asp:Literal></small>
        </div>
        <div style="width:100%; padding-left:25px">
            <h4 style="margin-top:1px"><asp:Literal ID="NameLiteral" runat="server"></asp:Literal></h4>
        </div>
   
</div>