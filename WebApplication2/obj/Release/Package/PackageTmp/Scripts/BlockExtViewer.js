$(document).ready(function () {
    $("#ImageControl").hide();
    $("#VideoControl").hide();
    videojs.options.flash.swf = "/Content/video-js/video-js.swf";
    extFormatText();
});

function extFormatText() {
 
    var txt = $("#extText").html();
  //  txt = txt.replace(/^NF\:\:VIDEO\:\:(\{.+\})/g, function (p1, p2) { console.log("replacer");}/*replacerVideoJSONTag*/);
    //  txt = txt.replace(/NF\:\:VIDEO\:\:(\{[^\)].+\})/g, replacerVideoJSONTag);
   
    $("#extText").html(formatTextNoVideoTag(txt));
}
function DownloadMedia(MediaId)
{
    document.getElementById('DownloadIFrame').src = serverRoot+"handlers/GetMediaSourceFile.ashx?MediaId=" + MediaId;
}
function OpenImageView(MediaId)
{
    videojs('MainVideoPlayer').pause();
    $("#ImageControl").html('<img src="' + serverRoot + 'handlers/GetBlockImage.ashx?MediaId=' + MediaId + '" width="320"/>');
    $("#ImageControl").show();
    $("#VideoControl").hide();
}
function clickVideoImgInEditor(elem)
{
    OpenVideoView($(elem).attr("mediaId"), $(elem).attr("markIn")*100, $(elem).attr("markOut")*100);
}
var inOut = { markIn: 0, markOut: 0 };
function OpenVideoView(MediaId, markIn, markOut) {

    $("#ImageControl").hide();
    $("#VideoControl").show();
   
    var myPlayer = videojs('MainVideoPlayer');
    myPlayer.pause();
    myPlayer.currentTime(0);
    myPlayer.src(serverRoot + "handlers/GetBlockVideo.ashx?MediaId=" + MediaId);
    myPlayer.load();
    
    inOut.markIn = markIn;
    inOut.markOut = markOut;
    myPlayer.on("loadeddata", function (e) {
        
       // console.log(MarkIn);
        if (typeof inOut.markIn != 'undefined')
            myPlayer.currentTime(inOut.markIn / 100);
        else
            myPlayer.currentTime(0);
        
        e.target.play();
        
    });
    myPlayer.on("play", function (e) {
        
    });
    myPlayer.on("timeupdate", function (e) {
        var player = videojs('MainVideoPlayer');
     //   console.log(player.currentTime() + "" + inOut.markOut);
        if (typeof (inOut.markIn) != 'undefined' && typeof (inOut.markOut) != 'undefined' && myPlayer.currentTime() > (inOut.markOut / 100)-0.3 && inOut.markOut>0)
        {
            myPlayer.currentTime(inOut.markIn / 100);
        }
    });
    
    return;
    
}
function OpenDocumentView(MediaId) {
    videojs('MainVideoPlayer').pause();
    $("#ImageControl").hide();
    $("#VideoControl").hide();
}
function ShowPicture(mediaId) {
    OpenImageView(mediaId)
}
function BEDownloadDocument(mediaId)
{
    DownloadMedia(mediaId);
}