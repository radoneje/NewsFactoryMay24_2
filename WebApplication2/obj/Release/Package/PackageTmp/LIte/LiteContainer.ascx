<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LiteContainer.ascx.cs" Inherits="WebApplication2.LIte.LiteContainer" %>
<%@ Register Src="~/LIte/NewsElements/NewsColumn.ascx" TagPrefix="uc1" TagName="NewsColumn" %>
<%@ Register Src="~/LIte/BlocksElements/BlocksColumn.ascx" TagPrefix="uc1" TagName="BlocksColumn" %>

<div class="row" style="margin-right:0px; margin-left:0px">
<div class="col-md-4 " style="padding-right:5px;padding-left:5px" >
    <uc1:NewsColumn runat="server" id="NewsColumn" />
</div>
<div class="col-md-8 " style="padding-right:5px;padding-left:5px">
    <uc1:BlocksColumn runat="server" ID="BlocksColumn" />
</div>
    </div>