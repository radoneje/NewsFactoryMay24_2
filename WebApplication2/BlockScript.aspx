<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlockScript.aspx.cs" Inherits="WebApplication2.BlockScript" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>News Factory May24 - script window</title>
    <link href="/Styles/NFW.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.3.min.js"></script>
    <script src="/Resources/lang0.js"></script>

    <style>
        body{
            min-width:100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" ID="errorPanel">
            <div class="alert alert-danger" style="margin: 10px 5px;">
                <h4>Ошибка!</h4>
                Вы не зарегистрированы для просмотра этой страницы.
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="workPanel" class="padding: 10px;">
            <div style="no-print">
                <label id="label1" for="TemplatesDropList"></label>
                <script>$("label[for='TemplatesDropList']").html(langTable["CapScriptTemplate"]);</script>
                <asp:DropDownList ID="TemplatesDropList" OnSelectedIndexChanged="TemplatesDropList_SelectedIndexChanged" runat="server" AutoPostBack="True" ClientIDMode="Static"></asp:DropDownList>
                <input id="Button1" type="button" value="Print" onclick="window.print();" />
                <script>$("#Button1").val(langTable["CapPrint"]);</script>
            </div>
            <hr />

            <div>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
