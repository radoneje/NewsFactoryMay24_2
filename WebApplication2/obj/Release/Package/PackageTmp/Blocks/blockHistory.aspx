<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="blockHistory.aspx.cs" Inherits="WebApplication2.Blocks.blockHistory" %>

<!DOCTYPE html>

<!DOCTYPE html>

<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" ID="noAccessPanel" Visible="false">no Access</asp:Panel>
        <asp:Panel runat="server" ID="workPanel" Visible="true">
            <div class="BEhistoryBox" onmousemove="clearLogOutTimeout();">
               <div class="BEhistoryContent">
                 здесь история сохраненных блоков
</div>
                 <div class="controls form-inline " style="width: 200px; float: right;margin-right:15px">
                <div style="display: inline-block; width: 100%;">
                    <div id="BESaveButtonContainer" class="btn-group text-center " style="width: 100%; text-align: center;">
                        <input id="BESaveBtn3" type='button' style="text-align: center; width: 50%;" class='btn btn-success navbar-btn caption caption-value' captionid="CapSave" onclick='SaveBlock(); BEEditor.focus(); clearLogOutTimeout();' "/>
                        <input id="BECloseBtn3" type='button' style="text-align: center; width: 50%; border-top-right-radius: 4px; border-bottom-right-radius: 4px;" class='btn btn-success navbar-btn centered caption caption-value' captionid="CapClose" onclick='ConfirmExit();' />
                        <script>
                            $('#BESaveBtn3').val(langTable['CapSave']);
                            $('#BECloseBtn3').val(langTable['CapClose']);
                        </script>
                    </div>
                </div>
            </div>
            <div style="clear: both"></div>
            </div>
            <div style="clear: both"></div>
        </asp:Panel>
    </form>
    <script src="<%=Application["serverRoot"] %>Scripts/historyScript.js"></script>
</body>