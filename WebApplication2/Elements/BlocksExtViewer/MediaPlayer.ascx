<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaPlayer.ascx.cs" Inherits="WebApplication2.Elements.BlocksExtViewer.MediaPlayer" %>
<asp:Panel ID="Panel1" runat="server" >
    <div id="ImageControl" style="width:100%">
        image
    </div>
    <div id="VideoControl" style="width:320px; height: 240px;">
       <video id="MainVideoPlayer" class="video-js vjs-default-skin vjs-big-play-centered"
               controls preload="auto" style="width:auto; height:180px"
               poster="<%=Application["serverRoot"] %>Images/DefaultPlayer.jpg">
               

          <!--  <source id="MainVideoPlayerSource" src="http://video-js.zencoder.com/oceans-clip.webm" type='video/mp4' />
        -->
            <p class="vjs-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="http://videojs.com/html5-video-support/" target="_blank">supports HTML5 video</a></p>
        </video>
        
    </div>
</asp:Panel>
