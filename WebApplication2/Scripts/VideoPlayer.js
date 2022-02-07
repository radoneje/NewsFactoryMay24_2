function ShowMainVideo_old(BlockId) {

    if (!(BlockId === "undefined")) 
        {
        ShowVideo(serverRoot+"handlers/GetBlockImage.ashx?BlockId=" + BlockId, serverRoot+"handlers/GetBlockVideo.ashx?BlockId=" + BlockId )
        }
    }

function ShowArchiveVideo(BlockId)
{
   // debugger;
    if (!(BlockId === "undefined")) 
        {
       // ShowVideo(serverRoot + "handlers/GetArchiveBlockImage.ashx?BlockId=" + BlockId, serverRoot + "handlers/GetArchiveBlockVideo.ashx?BlockId=" + BlockId)
        ShowMainVideo(BlockId, true)
    }
}

function ShowMainVideo(BlockId, archive)
{
   
    

    $('body').append(
        $(div).addClass("videoWr")
        .click(videoWrClose)
        .append($(div)
            .addClass("videoWrClose")
            .html("<span>" + langTable['Close'] + "</span>")
            .click(videoWrClose)
        )
        .append($(div).addClass("videoWrBody").loading50())
        );
    $('body').addClass('overflowHide');
    loadMainVideo(BlockId, archive);
}
function loadMainVideo(BlockId, archive) {
    if (typeof (BlockId) == 'string')
        BlockId = BlockId.trim()
    var url = serverRoot + "elements/mainVideoPlayer.aspx?BlockId=" + BlockId;
    if (archive == true)
        url += "&archive=true"
    $(".videoWrBody").load(url, function () {
        console.log("load after");
        initSubTitleEditor();
    });
}
function videoWrClose(e) {
    //if ($(e.target).hasClass(""));
    if ((typeof(e)!='undefined') && $(e.target).hasClass("stopPropagation"))
        return false;
    $(".videoWr").fadeOut(300, function () {
        $(".videoWr").remove();
        $('body').removeClass('overflowHide');
    });
}

function ShowVideo(poster, src) {
    var myPlayer = videojs('MainVideoPlayer');
    myPlayer.pause();
    myPlayer.poster(poster);
    myPlayer.src(src);
    myPlayer.load();
    myPlayer.on("loadeddata", function (e) {
        e.target.play();
    });
}