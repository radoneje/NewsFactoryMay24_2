<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="blockSocial.aspx.cs" Inherits="WebApplication2.Blocks.blockSocial" %>

<!DOCTYPE html>

<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" ID="noAccessPanel" Visible="false">no Access</asp:Panel>
        <asp:Panel runat="server" ID="workPanel" Visible="true">
            <div class="BEsocialBox" onmousemove="clearLogOutTimeout();">
                <div class="BSRow">
                    <div class="BSImageBox">
                        <div class="BSImageBoxMenuWr">
                             <div class="BSImageBoxMenu" onkeypress="clearLogOutTimeout();" >
                                удалить
                            </div>
                        </div>
                        <div class="BSImageWr"  id="BSImageWr" runat="server">
                            <img src="<%=Application["serverRoot"] %>Images/noimage.jpg" class="BSImage" />
                        </div>
                    </div>
                   
                    <div class="BSpublishWr">
                        <img class="loadingImg" src="<%=Application["serverRoot"] %>Images/loading.gif" id="BSpublishWrnoImage" />



                    </div>

                </div>
                <div class="BSRow">
                    <div class="BSbody">
                         <div class="BShead">
                        <div class="BStitle">
                            <input id="BStitleText" type="text" onkeypress="clearLogOutTimeout();" placeholder="title" maxlength="255" runat="server" onblur="BSsave()" onkeyup="BSenablePublish()" />
                        </div>
                        <div id="BSsubTitleText" class="BSsubTitle">
                            <textarea id="BSsubTitleTextCtrl"  onkeypress="clearLogOutTimeout();" placeholder="sub title" maxlength="255" runat="server" onblur="BSsave()" onkeyup="BSenablePublish()" />
                        </div>
                         <div id="BSmyImage" class="BSsubTitle">
                             <img id="BSmyImageImg" src="/images/noimage.jpg" />
                             <div class="btn-group text-center ">
                                 <input id="BSchangeImage" type="button" class="btn btn-success" value="Загрузить свою картинку"  />
                                  <input id="BSchangeImageFile" type="file" style="display:none" accept="image/png, image/jpeg" />
                             </div>

                        </div>

                    </div>
                        <div class="BStext">
                            <textarea id="BSText" placeholder="text" onkeypress="clearLogOutTimeout();"  runat="server" onblur="BSsave()" onkeyup="BSenablePublish()"></textarea>
                        </div>

                    </div>
                </div>
            </div>
            <div class="controls form-inline " style="width: 200px; float: right">
                <div style="display: inline-block; width: 100%;">
                    <div id="BESaveButtonContainer" class="btn-group text-center " style="width: 100%; text-align: center;">
                        <input id="BESaveBtn2" type='button' style="text-align: center; width: 50%;" class='btn btn-success navbar-btn caption caption-value' captionid="CapSave" onclick='SaveBlock(); BEEditor.focus(); clearLogOutTimeout();' "/>
                        <input id="BECloseBtn2" type='button' style="text-align: center; width: 50%; border-top-right-radius: 4px; border-bottom-right-radius: 4px;" class='btn btn-success navbar-btn centered caption caption-value' captionid="CapClose" onclick='ConfirmExit();' />
                        <script>
                            $('#BESaveBtn2').val(langTable['CapSave']);
                            $('#BECloseBtn2').val(langTable['CapClose']);
                        </script>
                    </div>
                </div>
            </div>
            <div style="clear: both"></div>
        </asp:Panel>
    </form>
    <script src="<%=Application["serverRoot"] %>Scripts/socialScrpt.js"></script>
</body>

