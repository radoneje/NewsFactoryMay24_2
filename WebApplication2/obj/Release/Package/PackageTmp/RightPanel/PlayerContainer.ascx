<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlayerContainer.ascx.cs" Inherits="WebApplication2.RightPanel.PlayerContainer" %>
<div id="VideoPlayer">
    <video id="MainVideoPlayer" class="video-js vjs-default-skin vjs-big-play-centered"
               controls preload="auto" width="216" height="122"
               poster="<%=Application["serverRoot"] %>Images/DefaultPlayer.jpg"
               data-setup='{"example_option":true}'>

          <!--  <source id="MainVideoPlayerSource" src="http://video-js.zencoder.com/oceans-clip.webm" type='video/mp4' />
        -->
            <p class="vjs-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="http://videojs.com/html5-video-support/" target="_blank">supports HTML5 video</a></p>
        </video>
</div>

