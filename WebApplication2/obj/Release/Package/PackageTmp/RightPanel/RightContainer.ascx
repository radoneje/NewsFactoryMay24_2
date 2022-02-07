<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightContainer.ascx.cs" Inherits="WebApplication2.RightPanel.RightContainer" %>
<%@ Register Src="~/RightPanel/PlayerContainer.ascx" TagPrefix="uc1" TagName="PlayerContainer" %>


<div class="panel panel-default">
  <div class="panel-heading" style="">
    <div class="panel-title ">
        <h4 class="rssTitleHeader" onclick="capLentaClick();" style=""><span id="rCapLenta" class="caption caption-html" captionId="CapLenta" "></span><script>$('#rCapLenta').html(langTable['CapLenta'])</script></h4>
    </div>
  </div>
 
      <div class="panel-body" style="padding-right:0">
     
      <div class="RssFixedHeightContainer">
          <div class="RssContent" id="RssContent"></div>
      </div>
          </div>
  
  
    <!--div class="panel-body" Id="MessagerMainPanel">
        <h4 class="rssTitleHeader" onclick="capMessageClick();"><span id="rCapMessages" class="caption caption-html" captionId="CapMessages"></span><script>$('#rCapMessages').html(langTable['rCapMessages'])</script></h4>
        <div class="MessagerFixedHeightContainer">
          <div class="MessagerContent" id="InMsgContent">

          </div>
      </div>
    </!--div>

   
    <!--div class="panel-body">
        <h4 class="rssTitleHeader" onclick="capUsersClick();"><span id="rCapUsers" class="caption caption-html" captionId="CapUsers"></span><script>$('#rCapUsers').html(langTable['CapUsers'])</script></h4>
        <div class="MessagerFixedHeightContainer">
          <div class="MessagerContent" id="MessagerContent">
              <img src="<%=Application["serverRoot"] %>"Images/loading.gif" style="max-width:50px" />
          </div>
      </div>
   
 </div>
    </div-->
</div>

