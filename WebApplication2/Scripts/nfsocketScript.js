$(document).ready(function () {
    
    ScPing()

});
var socketTimeout;
function ScPing()
{
    var Cookie = getCookie("NFWSession");

    var jdata = {
        msg: "",
        data: ""
    }
 
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: serverRoot + "nfsock",
        data: JSON.stringify(jdata),
        dataType: "json",
        async: true,
        success: function (e) {
            try {
                var ScCommandArr = new Array();
                $(e).each(function (i, dt) {
                    ScCommandArr.push(SCparseCommand(dt));
                });
   
                if (ScCommandArr.length > 0)
                {
                    
                    $.ajax({
                        type: "POST",
                        contentType: "application/json",
                        url: serverRoot + "nfsockResponce",
                        data: JSON.stringify(ScCommandArr),
                        dataType: "json",
                        async: true
                    });
                }
            }
            catch (ex) {
                console.warn(ex);
            }
            socketTimeout = setTimeout(ScPing, 4000);
        },
        error: function (e) {
            console.warn("nfsock");
            console.warn(e);
            socketTimeout = setTimeout(ScPing, 10000);
        },
    });
}
function SCparseCommand(dt)
{
    console.log("receive socket command:" + dt.msg);
    switch (dt.msg) {
        case 'noAuth': {
            document.location.href = serverRoot + "login";
        } break;
        case 'lrvUpdateChrono': {
            //    mediaId = mediaId, status = status  
            var iframe = document.getElementById("BlockEditIframe");
            if (iframe != null)
                iframe.contentWindow.mediaUpdateChrono(dt.data.mediaId, dt.data.status);

          
        }
            break;
        case 'blockSave': {
           // updateSubBlockImageControl(dt.data.blockId);
        }; break;
        case 'newsNew': {
            //NewsId
        }; break;
        case 'blockAdd': {
            //NewsId
            ReloadNews();
        }; break;
        case 'newsEdit': {
            //NewsId
            ReloadNews();
        }; break;
        case 'newsDelete': {
            //NewsId
            ReloadNews();
        }; break;

        case 'mediaResort': {          
            updateSubBlockImageControl(dt.data.blockId);
            if ($(".mainMediaBox[blockId='" + dt.data.blockId + "']").length > 0) {
                MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
            }
        }; break;
        case 'mediaDelete': {
            //mediaId = MediaId
            try {
                var iframe = document.getElementById("BlockEditIframe");
                if (iframe != null)
                    iframe.contentWindow.mediaRemove(dt.data.mediaId);
                $(".mainVideoMediaItem[mediaId='" + dt.data.mediaId + "']").slowRemove(function () {
                    if ($(".mainVideoMediaItem").length == 0)
                        videoWrClose();
                    else
                        $(".mainVideoMediaItem").first().click();
                });
            }
            catch (e) { console.warn(e)}
            
        }; break;
        case 'mediaRename': {
            //updateSubBlockImageControl(this.data.blockId);
         
            if ($(".mainMediaBox[blockId='" + dt.data.blockId + "']").length > 0) {
                MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
            }
        }; break;
        case 'mediaStatusChange': {
           // blockId = medoas.First().ParentId, mediaId = MediaId, ready = ready, approve = approve
            updateSubBlockImageControl(dt.data.blockId);
            if ($(".mainMediaBox[blockId='" + dt.data.blockId + "']").length > 0) {
                MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
                if ($(".mainMediaBox[mediaId='" + dt.data.mediaId + "']").length > 0) {
                    $("#MediaBlockEditReadyDropDown").prop("checked", dt.data.ready);
                    $("#MediaBlockEditApproveDropDown").prop("checked", dt.data.approve);
                }
            }
        }; break;
        case 'mediaUploadComplite': {

            var iframe = document.getElementById("BlockEditIframe");

            if (iframe != null && iframe.contentWindow.getBlockId() == dt.data.blockId)
                iframe.contentWindow.ReloadMedia();
           
            if ($(".mainMediaBox[blockId='" + dt.data.blockId + "']").length > 0) {
                console.log($(".mainVideoMediaItem").length);
                if ($(".mainVideoMediaItem").length == 0)
                {
                    loadMainVideo(dt.data.blockId);
                }
                else
                 MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
             }
        }; break;
        case 'userLogin': {
     
            // NFSocket.SendToAll.SendData("userLogin", new { userId = sLogin });
        }; break;
        case 'userDisconnect': {
            messagerDeActivateUser(dt.data.userId);
            //data =  new {userId=cl.userId}
           // <div onclick="OpenSendMessage(144)" id="MessagerItemTitle144" class="MessagerActivetrue">Выпускающий (пример)</div>
        }; break;
        case 'userConnect': {
            messagerActivateUser(dt.data.userId);
            //   data = new { userId = userId, userName = userName }
        } break;
        case 'lrvStarted': {
            //    mediaId = mediaId, blockId = blockId  
            try {
                var iframe = document.getElementById("BlockEditIframe");
                if (iframe != null )
                    iframe.contentWindow.BElrvStateChange(dt.data.mediaId, '');
               
               
            }
            catch (ex) {
                console.warn(ex);
            }
            if ($(".mainVideoMediaItem[mediaId='" + dt.data.mediaId + "']").length > 0) {
                MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
                medialrvStateChange(dt.data.mediaId, '');
            }
        } break;
        case 'lrvUpdateStaus': {
            //    mediaId = mediaId, status = status , blockId
            try {
                var iframe = document.getElementById("BlockEditIframe");
                if (iframe != null)
                    iframe.contentWindow.BElrvStateChange(dt.data.mediaId, dt.data.status);
            }
            catch (ex) {
                console.warn(ex);
            }
            if($(".mainVideoMediaItem[mediaId='"+dt.data.mediaId+"']").length>0)
            {
              //  MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
                medialrvStateChange(dt.data.mediaId, dt.data.status);
            }
        } break;
        case 'lrvCreated': {
            //    mediaId = mediaId, blockId = blockId 
            try {
                var iframe = document.getElementById("BlockEditIframe");
                if (iframe != null)
                    iframe.contentWindow.BElrvStateSucc(dt.data.mediaId);
            }
            catch (ex) {
                console.warn(ex);
            }
            if ($(".mainVideoMediaItem[mediaId='" + dt.data.mediaId + "']").length > 0) {
              //  MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
                medialrvStateSucc(dt.data.mediaId, '');
            }
        } break;
        case 'lrvFailed': {
            //    mediaId = mediaId, blockId = blockId  
            try {
                var iframe = document.getElementById("BlockEditIframe");
                if (iframe != null)
                    iframe.contentWindow.BElrvStateErr(dt.data.mediaId);
            }
            catch (ex) {
                console.warn(ex);
            }
            if ($(".mainVideoMediaItem[mediaId='" + dt.data.mediaId + "']").length > 0) {
             //   MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
                medialrvStateErr(dt.data.mediaId);
            }
        } break;
     
        case 'thCreated': {
            //    mediaId = mediaId, blockId = blockId  
            try {
            var iframe = document.getElementById("BlockEditIframe");
            if (iframe != null )
                iframe.contentWindow.mediaThCreate(dt.data.mediaId);
            }
            catch (ex) {
                console.warn(ex);
            }
            if ($("#BlockImageControl" + dt.data.blockId).length>0)
                updateSubBlockImageControl(dt.data.blockId);
            if ($(".mainVideoMediaItem[mediaId='" + dt.data.mediaId + "']").length > 0) {
                MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
            }
        } break;
        case 'thFailed': {
            //    mediaId = mediaId, blockId = blockId  
        } break;
        case 'rssUpdate': {
            //    rssId 
        } break;
        case 'imgCreated': {
            //    mediaId = mediaId, blockId = blockId  
                try {
                    var iframe = document.getElementById("BlockEditIframe");
                    if (iframe != null ) {
                        iframe.contentWindow.mediaImgCreate(dt.data.mediaId);
                    }
                }
                catch (ex) {
                console.warn(ex);
                }
                if ($(".mainVideoMediaItem[mediaId='" + dt.data.mediaId + "']").length > 0) {
                    MainVideoMediaBoxLoad(dt.data.blockId, $(".mainVideoMediaItemActive").attr("mediaid"));
                }
                if ($("#BlockImageControl" + dt.data.blockId).length > 0)
                    updateSubBlockImageControl(dt.data.blockId);
        } break;
        case 'imgFailed': {
            //    mediaId = mediaId, blockId = blockId  
        } break;
        case 'socialStateChange': {
            
            var iframe = document.getElementById("BlockEditIframe");
            if (iframe != null ) {
                iframe.contentWindow.socialFeedTryUpdate(dt.data.feedId);
            }
        } break;
        case 'mediaGraphicsChange': {
            //    blockId = blockId, mediaId = mediaId
        } break;
        
      
        
    }
    return dt.id;
}