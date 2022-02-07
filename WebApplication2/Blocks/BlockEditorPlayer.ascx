<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockEditorPlayer.ascx.cs" Inherits="WebApplication2.Blocks.BlockEditorPlayer" %>

<video id="BEPlayer" width="366" height="206" src="<%=Application["serverRoot"] %>handlers/GetBlockVideo.ashx?BlockId=81610843" ontimeupdate="BEPlaerTimeUpdate()"  preload="auto" class="video-js vjs-16-9 vjs-default-skin"></video>
       <!--<div id="BEPlayerControlContainer" class="input-group input-group-sm" MediaType="VIDEO">
    <span class="input-group-addon " style="cursor:pointer " id="BEPlayerMarkIn" onclick="BEClickVideoMarkIn();"><small>00:00:00.00</small></span>
    <input class="form-control" style="text-align:center; cursor:pointer; font:bold;" type="text" id="BEPlayerTimecode"  readonly="true"/>
  <span class="input-group-addon "   id="BEPlayerMarkOut" style="cursor:pointer "  onclick="BEClickVideoMarkOut(); "><small>23:59:59.99</small></span>
   
</div>-->
<div id="BEPlayerScrollerWr" onclick="BEPlayerScrollerClick2(event)">
      <div id="BEPlayerScrollerMArkIn"></div>
    
    <div id="BEPlayerScrollerMArkOut"></div>
    <div id="BEPlayerScrollerCurPos"></div>
</div>
<div id="BEPlayerControlContainer"  MediaType="VIDEO" markIn="0" markOut="0">
    <div id="BEPlayerMarkIn" MediaType="VIDEO" onclick="BEClickVideoMarkIn();"  >00:00:00.00</div>
     <div id="BEPlayeCenter">
         <div style="display:inline-block; width:30px; cursor:pointer" onclick="BEAddVideoToEditor()">
             <span class="glyphicon glyphicon-leaf" aria-hidden="true"></span>
         </div>
         <div style="text-align:center; display:inline-block;  font-weight:bold; width:calc(100% - 35px); cursor:pointer" onclick="BEEditorPlayPause()">
             <div   id="BEPlayerIcon" style="text-align:center; display:inline-block;"  >
                   <span class="glyphicon glyphicon-play" aria-hidden="true"></span>
            </div>
            <div   id="BEPlayerTimecode" style="text-align:center; display:inline-block;"  >
                00:00:00.00
            </div>
             </div>
     </div>
    <div id="BEPlayerMarkOut" MediaType="VIDEO" onclick="BEClickVideoMarkOut();" >00:00:00.00</div>
    <div style="clear:both"></div>
</div>
<style>
    #BEPlayerScrollerMArkIn{
           width: 0%;
           height: 100%;
           border-radius: 4px;
           background-color: red;
           left: 0;
           position: relative;
    }
       #BEPlayerScrollerMArkOut{
           width: 100%;
            height: 100%;
            border-radius: 4px;
            background-color: yellow;
            right: -100%;
            top: -100%;
            position: relative;
    }
    #BEPlayerScrollerWr{
            width: 100%;
           height: 1em;
           background-color:#4CAF50;
           cursor:pointer;
           overflow:hidden;
    }
    #BEPlayerScrollerCurPos{
       width: 0.5em;
           height: 100%;
           border-radius: 4px;
           background-color: gray;
           left: 0%;
           position: relative;
           top:-200%;
    }
    #BEPlayerControlContainer{
        width:100%;
        background:#ddd;
        border:1px solid #eee;
        border-bottom-left-radius:4px;
        border-bottom-right-radius:4px;
    }
    #BEPlayerMarkIn{
        font-size:x-small;
        float:left;
        min-width:70px;
        max-width:70px;
        padding:3px;
           cursor:pointer;
    }
     #BEPlayerMarkOut{
        font-size:x-small;
        float:left;
        min-width:70px;
        max-width:70px;
         padding:3px;
         cursor:pointer;
    }
     #BEPlayeCenter{
         width:calc(100% - 140px);
         float:left;
     }
</style>
<script>
    $(".vjs-progress-control").hide();
    $('#BEPlayerScroller').click(function (e) {
        var posY = e.pageX-$(this).position().top;
        var pos = ((e.pageX - $(this).position().left) / $(this).width());
        var currTime = videojs('BEPlayer').duration() * pos;
      
        if( $("#BEPlayerControlContainer").attr("markIn")>currTime)
        {
            BESetMarkIn(currTime);
        }
        if ($("#BEPlayerControlContainer").attr("markOut")>0 && $("#BEPlayerControlContainer").attr("markOut") < currTime) {
            BESetMarkOut(currTime);
        }
        videojs('BEPlayer').currentTime(currTime);

    });
</script>