<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubBlockImageControl.aspx.cs" Inherits="WebApplication2.Elements.SubBlockImageControl" %>



<form id="form1" runat="server">
    <div style="cursor: pointer" onclick="ShowMainVideo(' <%=Request.Params["blockId"] %>')">
        <img id="BlockImageControl<%=Request.Params["blockId"] %>" blockId="<%=Request.Params["blockId"] %>" class="media-object blockListImage" src="<%=(string)Application["ServerRoot"] %>handlers/GetBlockImage.ashx?BlockId=<%=Request.Params["blockId"] %>&rnd=<%=new Random().Next(10000).ToString() %>" />

    </div>
    <div class="SubBlockImageControlStatus SubBlockImageControlStatus<%=getState() %> "></div>
</form>

