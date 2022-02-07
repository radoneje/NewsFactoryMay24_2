<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainVideoPlayer.aspx.cs" Inherits="WebApplication2.Elements.mainVideoPlayer" %>

<form id="form1" runat="server" onsubmit="return false;">
    <div class="mainMediaBox" id="mainMediaBox<%=Request.Params["BlockId"] %>" blockId="<%=Request.Params["BlockId"] %>">
   
        <div class="mainVideoPlayerRow">
        <div class="mainVideoPlayerCell">
            <div class="mainVideoPlayerPlayer">
              <asp:panel runat="server" id="panelNoImage"  >
                    <img src="<%=(string)Application["ServerRoot"]%>images/noimage.jpg" />
                </asp:panel>
                <asp:panel runat="server" id="panelPlayerWr"  >
                    <!--script>
                        $("#panelPlayerWr").load(serverRoot+"elements/mainVideoPlayerElement.aspx?mediaId="+<%=_mediaId.ToString()%>+"&blockType=<%=_blockType.ToString()%>")
                    </!--script-->
                </asp:panel>
                
            </div>
            <div class="mainVideoMediaWr">
                <div class="mainVideoMediaBox">
                    </div>
            </div>
        </div>
    </div>
    <script>
        $().ready(function () { 
            initMainVideoMediaBox(<%=Request.Params["blockId"] %>, <%=_mediaId.ToString()%>, <%=Request.Params["archive"]==null?"false":"true"%> ); 
            
            <%=Request.Params["archive"]==null? "HTMLFileDropInitByClass('mainMediaBox');":""%>
           
        });
   
</script>

        </div>
</form>



