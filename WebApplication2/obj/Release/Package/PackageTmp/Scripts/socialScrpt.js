$(document).ready(function () {
    socialFeedLoad();
    socialMediaMenuActivate();
    mediaToSocialAdd($(".BSImageWr").attr("mediaId"),$(".BSImageWr").attr("mediaType"));
    $("#BSText").focus();
    $(".BSpublishWr").mouseleave(function () {
        $(".BSConfirm").hide();
        $(".BSbadge").show();
    });
    $("#BSchangeImage").click(() => {
        $("#BSchangeImageFile").click();
    })
    $("#BSchangeImageFile").change((e) => {
        console.log("BSchangeImageFile", $("#BSchangeImageFile")[0].files);
        $("#BSmyImageImg").attr("src",  URL.createObjectURL($("#BSchangeImageFile")[0].files[0]))
    })
   
});
function socialFeedLoad() {
    console.log("socialFeedLoad");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/socialFeedLoad",
        data: JSON.stringify({ id: $("#BlockEditIdHidden").val() }),
        dataType: "json",
        success: function (data) {
            socialFeedLoadComplite(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("socialFeedLoad error");
            console.warn(data);
        }
    });
}
function socialFeedLoadComplite(data) {
    //  $(".BSpublishWr").html("");
    $("#BSpublishWrnoImage").remove();
    
    var imgArr = [];
    data.forEach(function (itm) {
        var feedArr = JSON.parse(itm.feedArr)
       
        if (feedArr.length > 0) {
            feedArr.forEach(a => {
                if (a.imgFile.length > 0)
                    imgArr.push(a);
            })
           
        }
        console.log("feedArr", imgArr);
        if ($("#" + itm.id).length == 0) {
            $(".BSpublishWr").append($(div)
                .addClass("BSpublishItem")
                .append($(div)
                    //.html()
                    .addClass('BSfeedTitle')
                    .attr('feedId', itm.id)
                )
                .append($(div)
                   // .html(itm.count)
                    .addClass('BSbadge')
                    .attr('feedId', itm.id)
                )
                .append($(div)
                     .addClass("BSConfirm")
                     .append($('<button type="button" class="btn btn-default btn-xs btn-bsconfirm bsconfirmGreen"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></button>')
                      .click(function () { BSpublishConfirm(itm.id) })
                     )
                     .append($('<button type="button" class="btn btn-default btn-xs btn-bsconfirm bsconfirmRed"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></button>')
                     .click(function () { BSpublishCancel(itm.id) })
                 
                    )
                .hide()
              
                )
                .attr("id", itm.id)
                
            );
            $("#" + itm.id).children(".BSfeedTitle").click(function (e) { BSshowHistory(e, itm.id) });
            $("#" + itm.id).children(".BSbadge").click(function (e) { BSpublish(itm.id) });
        }
        $("#" + itm.id)
        .removeClass("BSpublishItemError")
        .removeClass("BSpublishItemProcessing")
        .removeClass("BSpublishItemComplite")

        $("#" + itm.id).children(".BSfeedTitle").html(itm.title);
        $("#" + itm.id).children(".BSbadge").html(itm.count);
        if (itm.error > 0)
            $("#" + itm.id).addClass("BSpublishItemError");
        if (itm.status == 1)
            $("#" + itm.id).addClass("BSpublishItemProcessing");
        if (itm.status > 1)
            $("#" + itm.id).addClass("BSpublishItemComplite");
        
    });

    if (imgArr.length > 0) {
        imgArr.sort((a, b) => {

            // Turn your strings into dates, and then subtract them
            // to get a value that is either negative, positive, or zero.
            return new Date(b.insertDate) - new Date(a.insertDate);
        })
        $("#BSmyImageImg").attr("src", "/socialImg/" + imgArr[0].id);
    }
    
 //   if ($(".BSpublishWr").attr("publish") == "disable")
    //    $(".BSpublishItem").addClass("BSpublishItemDisabled");
}
function BSsave()
{
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/socialMessageSave",
        data: JSON.stringify({
            id: $("#BlockEditIdHidden").val(),
            title: $("#BStitleText").val(),
            subTitle: $("#BSsubTitleTextCtrl").val(),
            message: $("#BSText").val(),
            mediaId: $(".BSImageWr").attr("mediaId"),
            mediaType: $(".BSImageWr").attr("mediaType")
        }),
        dataType: "json",
        success: function (data) {
          //  socialFeedLoadComplite(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("socialMessageSave error");
            console.warn(data);
        }
    });
}
function BSpublish(feedId)
{
    //var feedId = $(this).attr("id");
    console.log("BSpublish"+ feedId);
    $("#" + feedId).children(".BSConfirm").show();
    $("#" + feedId).children(".BSbadge").hide();

    // $("#" + feedId).children().last().hide();
 
    return;
}
function getBase64(file) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        console.log(reader.result);
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}

function BSpublishRemove(feedId) {
   // if ($("#" + feedId).attr("publish") == "disable")
   //     return;

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/socialMessageCancel",
        data: JSON.stringify({
            id: $("#BlockEditIdHidden").val(),
            feedId: feedId,
            mediaId: $(".BSImageWr").attr("mediaId"),
            id: $("#BlockEditIdHidden").val(),
        }),
        dataType: "json",
        success: function (data) {
            BSenablePublish();
            socialFeedLoad();
            $(".BSbadge[feedid='+feedId+']").html("0");
            $("#" + feedId).removeClass("BSpublishItemProcessing")
            $("#" + feedId).removeClass("BSpublishItemDisabled")
          
        
            $("#BSmyImageImg").attr("src", "/Images/noimage.jpg")
        },
        error: function (data) {
            console.warn("socialMessageCancel error");
            console.warn(data);
        }
    });

}

function BSpublishConfirmed(feedId)
{
  //  console.log($("#" + feedId).attr("publish"))
    if ($("#" + feedId).attr("publish") == "disable")
        return;

    if ($("#BSchangeImageFile")[0].files.length > 0) {

        console.log("uploadFile", );
        var formData = new FormData();
       
        // Attach file
        var reader = new FileReader();
        reader.readAsDataURL($("#BSchangeImageFile")[0].files[0]);
        reader.onload = function () {
            $.ajax({
                url: serverRoot + "testservice.asmx/socialMessagePublishUploadImage",
                data: JSON.stringify({ fn: $("#BSchangeImageFile")[0].files[0].name, base64: reader.result.replace("data:image/jpeg;base64,", "").replace("data:image/png;base64,", "") }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: (data) => { console.log("upload Image sucess", data); UloadToSoc(data.d) }
                // ... Other options like success and etc
            });
        }

       
    }
    else UloadToSoc("")
    function UloadToSoc(imgUrl) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/socialMessagePublish",
            data: JSON.stringify({
                id: $("#BlockEditIdHidden").val(),
                feedId: feedId,
                title: $("#BStitleText").val(),
                subTitle: $("#BSsubTitleTextCtrl").val(),
                message: $("#BSText").val(),
                mediaId: $(".BSImageWr").attr("mediaId"),
                mediaType: $(".BSImageWr").attr("mediaType"),
                imgFile: imgUrl,
               // blockid: $(".BEWrapper").attr("blockid")
            }),
            dataType: "json",
            success: function (data) {
                BSenablePublish();
                socialFeedLoad();
            },
            error: function (data) {
                console.warn("socialMessagePublish error");
                console.warn(data);
            }
        });
    }
}
function BSpublishConfirm(feedId) {
    BSpublishRemoveConfirmBtn(feedId);
    BSpublishConfirmed(feedId);
}
function BSpublishCancel(feedId) {
    BSpublishRemoveConfirmBtn(feedId);
    BSpublishRemove(feedId);
}
function BSpublishRemoveConfirmBtn(feedId) {
    console.log("BSpublishRemoveConfirmBtn");
    $("#" + feedId).children(".BSConfirm").hide();
    $("#" + feedId).children(".BSbadge").show();
}
function BSdisablePublish(id) {

    if ($("#" + id).attr("publish") == "disable")
        return;
  
    $("#" + id).addClass("BSpublishItemGreen");
    setTimeout(function () {
        $("#" + id)
            .removeClass("BSpublishItemGreen")
            .addClass("BSpublishItemDisabled");
    }, 500);

   
    $("#"+id).attr("publish", "disable");
}
function BSenablePublish() {
    $(".BSpublishItem")
        .attr("publish", "false")
        .removeClass("BSpublishItemDisabled");
   
}
function mediaToSocial(mediaId) {
  
    console.log($("#BETower").is(":visible"));
    if ($("#BETower").is(":visible"))
        return MediaToEditor(mediaId);
    mediaToSocialAdd(mediaId, $("#BEMediaItem" + mediaId).attr("MediaTypeId"));
}
function mediaToSocialAdd(mediaId, mediaType){
    var param = "";
    if (mediaId == 0)
        return;
    if (mediaType == 2) {
        $(".BSImageWr").html($("<video id='BSmediaVideo' class='BSmediaVideo' width='640' height='360' controls ></video>"));
        $("#BSmediaVideo").attr("poster", serverRoot + "handlers/GetBlockImage.ashx?mediaId=" + mediaId + "&rnd=" + Math.random());
        $("#BSmediaVideo").append($("<source></source>")
            .attr("src", serverRoot + "handlers/GetBlockVideo.ashx?MediaId=" + mediaId + "&rnd" + Math.random())
            .attr("type", 'video/mp4')
            );
        $(".BSImageWr").attr("mediaId", mediaId);
        $(".BSImageWr").attr("mediaType", mediaType);

    } else if (mediaType == 1) {
        param = "<img class='BSImage' src='"+serverRoot+"handlers/GetBlockImage.ashx?MediaId=" +
          +mediaId + "' mediaId='" + mediaId + "' mediaType='1' />";
        $(".BSImageWr").html(param);
        $(".BSImageWr").attr("mediaId", mediaId);
        $(".BSImageWr").attr("mediaType", mediaType);
    }
    BSsave();
}
function socialMediaMenuActivate(){
    
   // $(".BSImageBoxMenuWr").on("mouseover", socialMediaMenuMouseEnter);
    $(".BSImageBoxMenuWr")
        .mouseenter(socialMediaMenuMouseEnter)
        //.mouseover(socialMediaMenuMouseEnter)
        .mouseleave(socialMediaMenuHide);

    $(".BSImageBoxMenu").on("click", function () {
        $(".BSImageWr").attr("mediaId", 0);
        $(".BSImageWr").attr("mediaType", 0);
        $(".BSImageWr").html($("<img/>")
            .attr("src", "/Images/noimage.jpg")
            .addClass("BSImage")
            );
        socialMediaMenuHide();
    });
}
function socialMediaMenuHide()
{
    if ($(".BSImageBoxMenu").is(":visible")) {
        $(".BSImageBoxMenu").stop().fadeOut(200);
    }
}
function socialMediaMenuMouseEnter()
{
    if ((!$(".BSImageBoxMenu").is(":visible")) &&  $(".BSImageWr").attr("mediaId")!=0) {
        $(".BSImageBoxMenu").show();
    }
}
function socialFeedTryUpdate(id) {

    if($("#"+id).length>0)
    {
        socialFeedLoad();
    }
}
function BSshowHistory(e, id){
    
    $(".BShistoryWr").remove();
    $(".panelBlockSocial").append($(div)
            .addClass("BShistoryWr")
            .addClass("panel-default")
            .attr("id", "bshistory" + id)
            .css("left", "calc(" + e.pageX + "px - 450px)")
            .css("top", e.pageY + "px")
            .mouseleave(function (e) { $(".BShistoryWr").remove(); })
            .append($(div)
                .addClass("panel-heading BShistoryHeader")
                .css("cursor", "pointer")
                .html($("<div style='float:left'><span>" + langTable['History'] + ": <b><small>" + $("#" + id + " .BSfeedTitle").html() + "</small></b></span></div>"))
                .append($('<div>X</div>')
                    .addClass("closeBtn")
                .click(function () {
                    $(".BShistoryWr").remove();
                })
                
                )
                .append($(div)
                    .addClass("clear")
                 )
            )
            .append($(div)
                .addClass("panel-body")
                .addClass("BShistoryBox")
                .append($("<img/>")
                    .attr("src", serverRoot+"Images/loading.gif")
                    .addClass("loadingImgSmall")
                )
                 .append(" loading...")
            )
        );
    $(".BShistoryWr").draggable({ handle: '.BShistoryHeader' });
    BShistoryLoad(id);
}
function BShistoryLoad(feedId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/socialGetHistory",
        data: JSON.stringify({ id: feedId, blockId: $("#BlockEditIdHidden").val() }),
        dataType: "json",
        success: function (data) {
            BShistoryLoadCompl(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("socialGetHistory error");
            console.warn(data);
        }
    });

};
function BShistoryLoadCompl(dt)
{
    $(".BShistoryBox").html("");
    dt.forEach(function (itm) {
        $(".BShistoryBox").append($(div)
                .attr("id", "BShistory"+itm.id)
                .addClass("BShistoryItem")
                .attr("socialId", itm.socialId)
                .attr("status", itm.status)
                .append($(div)
                    .addClass("BShistoryDate")
                    .html(itm.date)
                )
            );
        if(itm.status==0)
        {
            $("#BShistory" + id)
                .addClass("BShistoryItemEmp")
                .append($(div)
                .addClass("BShistoryItemEmpTxt")
                .addClass("BShistoryItemTxt")
                .html("no")
               )
        }
        else if(itm.status==1)
        {
            $("#BShistory" + itm.id)
               .addClass("BShistoryItemLoading")
               .append($(div)
                .addClass("BShistoryItemLoadingTxt")
                .addClass("BShistoryItemTxt")
                .html("loading...")
               )
        }
        else if (itm.status <1) {
            $("#BShistory" + itm.id)
               .addClass("BShistoryItemError")
                .append($(div)
                .addClass("BShistoryItemErrorTXT")
                .addClass("BShistoryItemTxt")
                .html(itm.eerMessage)
               )
            
        }
        else if (itm.status >1) {
            $("#BShistory" + itm.id)
               .addClass("BShistoryItemSuccess")
               .append($("<input type='text'>")
                .addClass("BShistoryItemSuccessTxt")
                .val(itm.link)
                .click(function () { BSopenLink(itm.link) })
                .prop("readonly", true)
                .attr("id","BShistoryItemSuccessTxt"+itm.id)
               )
               .append($(div)
                .addClass('btn-group')
                .addClass('btn-group-xs')
                .attr('role', 'group')
                
                .append($('<button type="button" class="btn btn-default" onclick="" data-toggle="tooltip" data-placement="bottom" data-original-title="вствить ссылку в текст"><span class="glyphicon glyphicon-leaf" aria-hidden="true"></span></button>')
                    .click(function () { BSLinktoText(itm.link) })
                )
                .append($('<button type="button" class="btn btn-default" onclick="" data-toggle="tooltip" data-placement="bottom" data-original-title="копировать ссылку"><span class="glyphicon glyphicon-duplicate" aria-hidden="true"></span></button>')
                    .click(function () { BSLinkCopy(itm.id) })
                )
               )
                
        }
        $("#BShistory" + itm.id).append($(div)
            .addClass("clear")
            );
    });
   // $('[data-toggle="tooltip"]').tooltip();
}
function BSLinktoText(link)
{
    $("#BSText").val($("#BSText").val() + "\r\n" + link);
    $("#BSText").focus();
    $(".BShistoryWr").remove();

}
function BSopenLink(link)
{
    window.open(link, "target-new");
    $(".BShistoryWr").remove();

}
function BSLinkCopy(id)
{
    $("#BShistoryItemSuccessTxt" + id).focus();
    $("#BShistoryItemSuccessTxt" + id).select();
    document.execCommand('copy');
    //copy;
}
