
var BEArr = new Array();
var isPlaying = false;
var playingTimeout;

$(document).ready(function () {
    LookingPinger();
    setInterval(() => {
        SaveBlock(false, false, true);
    }, 20 * 1000)

    initMainHeadMenu();
    var fs = window.localStorage.getItem("BEEditorFontSize");
    if (fs != null) {
        $(".BEEditor").css("font-size", fs);
        $("#BEEditorFSSelector").val(fs);
    }
    intAddSotSelect();
    intAddGeoSelect();
    intAddSrcSelect();


    HTMLFileDropInitByClass("BEbox");
    $('[data-toggle="tooltip"]').tooltip();
  /*  $("#BEEditor")[0].addEventListener("oncopy", async (e) => {
        console.log("oncopy", e);
        var div = Document.createElement("div");
        div.innerHTML = e;
        newClipText = div.textContent || div.innerText || "";
        await navigator.clipboard.writeText(newClipText)
        console.log("newClipText", newClipText);
    })*/
    $(document).keydown(function (e) {

        if (e.ctrlKey) {

            switch (e.keyCode) {

                case 83: { SaveBlock(); BEEditor.focus(); e.preventDefault(); e.stopPropagation(); } break;
                case 70: { BEAddMediaFiles(); e.preventDefault(); e.stopPropagation(); } break;
                case 81: { beExit(e); } break;
                case 85: {
                    $("#addSot").focus().trigger("click");
                    e.preventDefault();
                    e.stopPropagation();
                } break;
                case 73: {
                    BEClickVideoMarkIn();
                    e.preventDefault();
                    e.stopPropagation();
                } break;
                case 79: {
                    BEClickVideoMarkOut();
                    e.preventDefault();
                    e.stopPropagation();
                } break;
            }

        }

        if ((e.shiftKey || e.ctrlKey) & e.keyCode == 32) {

            e.preventDefault();
            e.stopPropagation();
            BEEditorPlayPause();
        }
        if (e.keyCode == 27)
            beExit(e)

        function beExit(e) {
            ConfirmExit(); e.preventDefault(); e.stopPropagation();
        }
    });

    $(".BEshowChernovik").click(function (e) {
        BEshowChernovik(e);
    });
    if (localStorage.getItem("chernovik") == 1) {
        $(".BEshowChernovik").click();
    }
    if ($("#BEEditor").getPreText().replace('\u2038', "").length < 112) {
       
        $("#BlockEditNameTextBox").select().focus();
    }
    else
        $("#BEEditor").focus();
   
    if ($("#BlockEditNameTextBox").val() == " -- ")
        setTimeout(function () {  $("#BlockEditNameTextBox").focus(); }, 500)

    if (localStorage.getItem("BEFullScreen"))
        $("#BEpanelBody").addClass("fullscreen")

});
function log(msg) {
    console.log(msg);
}
$.fn.getPreText = function () {
    var browser = BrowserDetect.browser;
    var ce = $("<pre />").html(this.html());
    if (browser == 'Chrome') {
        ce.find("div").replaceWith(function () { return "\r\n" + this.innerHTML; });
        ce.find("br").replaceWith(function () { return "\r\n" + this.innerHTML; });
        ce.find("\u2038").replaceWith(function () { return ""; });
       
      
    }
    if (browser == 'MSIE')
        ce.find("p").replaceWith(function () { return this.innerHTML + "<br/>"; });
    if (browser == 'Firefox' || browser == 'Opera' || browser == 'MSIE') {
        ce.find("br").replaceWith("\r\n");
        ce.find("\u2038").replaceWith("");
    }
    ce.find("img").replaceWith(function () {
        return "NF::VIDEO::" + JSON.stringify({ mediaId: $(this).attr("MediaId"), mediaType: $(this).attr("mediaType"), markIn: $(this).attr("markIn"), markOut: $(this).attr("markOut") });
        //console.log(this)
    });
    var txt = ce.html();
    txt = txt.replace(/<b>/g, "NF::BOLDSTART").replace(/<\/b>/g, "NF::BOLDEND");
    ce.html(txt);

    return ce.text();
};
var isSaved = false;
var Blockcalctime;
var LogOutTimeout;
var BlockId;



clearLogOutTimeout();

function autoExit() {
    log("autoExit");
    if (SaveBlock())
        window.parent.CloseBlockEditor($("#BlockEditIdHidden").val());
    else 
        setTimeout(autoExit, 1000 * 60)
}
function clearLogOutTimeout()
{
    clearTimeout(LogOutTimeout);
    LogOutTimeout = setTimeout(autoExit, 1000 * 60 * 15);
   
}
function BEOnBlur(ctrl)
{

}
function insertAtCursor(myField, myValue) {
    //IE support
    myField.focus();
    pasteHtmlAtCaret(HiLiteComments(myValue));


   
}
function ConfirmExit() {

   
    SaveBlock(true);
    return ;

    if (confirm(langTable['CapConfirmBlockSave']) == true)
    {
        

        if (SaveBlock(true)) {
            
            
        }
    }
    else
     window.parent.CloseBlockEditor($("#BlockEditIdHidden").val());
    
}
function clearSelection() {
   
    let text = $("#BEEditor").html();
    while (text.indexOf("<b>")>=0)
        text = text.replace("<b>", "").replace("</b>", "");
    //$("#BEEditor").html(text);
    $("#BEEditor").html(HiLiteComments(text));
   
}
function SaveBlock(exit, async, disableHistory)
{
    
    if ($(".mainHeadMenuItemActive").attr("panel") != 'panelBlockEditor') {
        $(".mainHeadMenuItem").removeClass("mainHeadMenuItemActive");
        $("#mainHeadMenuItemEditor").addClass("mainHeadMenuItemActive");
        mainHeadMenuItemClick($("#mainHeadMenuItemEditor"));
        $(".panelBlockEditor").show();
    }

            var RealTime = checkTimes($("#BlockEditRealTextBox"));
            
            if (RealTime <0)
            {
                //RealTime = 0;
        
                $('#blockEditModalBody').html(langTable['CapErrorReal']);
                $('#blockEditModal').modal('show');
                $('#blockEditModal').on('hidden.bs.modal', function (e) {
                    $("#BlockEditRealTextBox").focus();
                })
                
                return false;
            }
            var PlannedTime = checkTimes($("#BlockEditPlannedTextBox"));

            if (PlannedTime <0) {

                //PlannedTime = 0;
                
                $('#blockEditModalBody').html(langTable['CapErrorPlan']);
                $('#blockEditModal').modal('show');
                $('#blockEditModal').on('hidden.bs.modal', function (e) {
                    $("BlockEditPlannedTextBox").focus();
                })
              
                return false;
            }
            if(!checkBlockTextSintax( $("#BEEditor").getPreText())){
              
                $('#blockEditModalBody').html(langTable['CapErrorTags']);
                $('#blockEditModal').modal('show');
                $('#blockEditModal').on('hidden.bs.modal', function (e) {
                    $("BlockEditPlannedTextBox").focus();
                })
                
                return false;
            }
            
            if ($("#BlockEditNameTextBox").val().length==0 ) {

                $('#blockEditModalBody').html(langTable['CapErrorName']);
                $('#blockEditModal').modal('show');
               
                $('#blockEditModal').on('hidden.bs.modal', function (e) {
                    $("#BlockEditNameTextBox").focus();
                })
                return false;
            }
            
            CalculateCalcTime($("#BEEditor").getPreText());
            var CalcTime = checkTimes(/*$("BlockEditCalcTextBox")*/Blockcalctime);
            CalcTime=checktimeFromText(Blockcalctime);
    ////////////
            if ($(".mainHeadMenuItemActive").attr("panel") != 'panelBlockEditor') {
                $(".mainHeadMenuItem").removeClass("mainHeadMenuItemActive");
                $("#mainHeadMenuItemEditor").addClass("mainHeadMenuItemActive");
                mainHeadMenuItemClick($("#mainHeadMenuItemEditor"));
                $(".panelBlockEditor").show();
            }
            if (exit == true) {
                $(".BEwrapper").fadeOut(500);
                setTimeout(function () { $('body').append('<img class="loadingImg" src="' + serverRoot + 'Images/loading.gif" />') }, 500);
            }
    ////////////
   
            var jdata = {
                BlockTypeId: JSON.parse($('#BlockEditTypeDropDown').val()).BlockTypeId,
                BlockName: $("#BlockEditNameTextBox").val(),
                BlockAutor: JSON.parse($('#BlockEditAutorDropDown').val()).UserID, 
                BlockOperator: JSON.parse($('#BlockEditOperatorDropDown').val()).UserID, 
                BlockJockey: JSON.parse($('#BlockEditJockeyDropDown').val()).UserID,
                BlockCutter: JSON.parse($('#BlockEditCutterDropDown').val()).UserID,
                BlockReady: $("#BlockEditRedyDropDown").prop('checked'),
                BlockApprove: $("#BlockEditApproveDropDown").prop('checked'),
                BlockText: changeQuotes($("#BEEditor").getPreText().replace('\u2038', "")),
                BlockDescription: $("#BlockEditDescriptionTextBox").val(),
                BlockChronoCalc: CalcTime,
                BlockChronoTask: PlannedTime,
                BlockChronoReal: RealTime,
                BlockId: $("#BlockEditIdHidden").val(),
                disableHistory: disableHistory||false
            };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url:  serverRoot+"testservice.asmx/SaveBlock",
                data: JSON.stringify(jdata, null, 2),
                dataType: "json",
                async : async!=true ,//  true,
                success: function (dt) {
                    try{
                        if (JSON.parse(dt.d).err == 1) {
                            AjaxFailed();
                        //    alert("Блок не сохранен! Ошибка! Попробуйте еще раз.");
                            NFconfirm(langTable['WarningErrorAddBlock'], 100, 100, 1, AjaxFailed);
                        }
                        else {
                            if (!disableHistory)
                                AjaxSucceeded(dt);
                            isSaved = true;
                            if (exit == true)
                                window.parent.CloseBlockEditor($("#BlockEditIdHidden").val());
                        }
                    }
                    catch(e)
                    {
                        alert("Блок не сохранен! Ошибка! Попробуйте еще раз.");
                        NFconfirm(langTable['WarningErrorAddBlock'], 100, 100, 1, AjaxFailed);
                    }
                },
                error: function () {
                    AjaxFailed();
                   
                    alert("Блок не сохранен! Ошибка! Попробуйте еще раз.");
                }
            }).getAllResponseHeaders();
            //showBlockEditorAlert("Сохранение блока: " );
            return true;
        }
function AjaxSucceeded(data) {
            console.log("блок сохранен");
            showBlockEditorAlert("Сохранение блока: " + JSON.parse(data.d).Message);
       
           // alert ("Сохранение блока: "+JSON.parse(data.d).Message);
        }
function AjaxFailed() {
            showBlockEditorWarning("Сохранение не удалось!");
           
        }        
function showBlockEditorWarning(text) {
            $("#BlockEditorAlert").toggleClass("alert alert-warning").css("display", "show").text(text).show();
            setTimeout(function () {
                $("#BlockEditorAlert").hide('blind', {}, 500)
            }, 5000);
        }
function showBlockEditorAlert(text) {
            $("#BlockEditorAlert").css("display", "show").text(text).show();
            setTimeout(function () {
                $("#BlockEditorAlert").hide('blind', {}, 500)
            }, 5000);
        }
function CheckLocking()
        {
            //ShowDisabledMessage("<img src='../Images/ajax-loader-whell.gif'/> Идет загрузка блока  ", "Редактор блока");

        }
function ShowDisabledMessage(messageText, messageTitle)
        {
            var iDiv=CreateFullScreenDiv("EditorAlertDiv")
            iDiv.innerHTML = CreateBlockAlertElements(messageText, messageTitle);
            //document.getElementById("EditorAlertDiv").appendChild(iframe);
            
            window.scrollTo(0, 0);

        }
function CreateBlockAlertElements(messageText, messageTitle)
        {
            var ret = '<div id="BlockEditorAlertMessage" class="alert alert-warning" role="alert">';
            if(checkDef(messageTitle))
            {
                ret = ret + "<h4>" + messageTitle + "</h4>";
            }
            if (checkDef(messageText))
            {
                ret=ret+'<p>'+messageText+'</p>'
            }
            ret = ret + '</div>';
            //if (checkDef(window.parent)) {
                ret = ret + "<button id='BlockEditorAlertCloseButton' type='submit' style='width: 100;' class='btn btn-success navbar-btn' onclick='window.parent.CloseEditor();'>закрыть редактор</button>";
                $("#BlockEditorAlertCloseButton").center();
            //}
            $("#BlockEditorAlertMessage").center();
            return ret;

}
function blockIdGet()
{
    return $("#BlockEditIdHidden").val();
}
function checkBlockTextSintax(txt) {

    var start = txt.indexOf("((");
    while (start >= 0)
    {
        txt = txt.substring(start + 2);
        var end = txt.indexOf("))");
        if (end < 0)
            return false;
        txt = txt.substring(end + 2);
        start = txt.indexOf("((");
    }
    return true;
}

function initLookingPinger(sBlockId)
        {
            BlockId = sBlockId;
            //LookingPinger(sBlockId)
            setTimeout(function () { LookingPinger() }, 1000);
           
        }
        
function LookingPinger() {
    if (BlockId = "")
        return;

            var jdata = {
                sCookie: $.cookie("NFWSession"),
                sNewsId: $("#BlockEditIdHidden").val(),
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url:  serverRoot+"testservice.asmx/BlockLooker",
                data: JSON.stringify(jdata),
                dataType: "json",
                async: true,
                success: function () { setTimeout(function () { LookingPinger() }, 15000); },
                error: function (e) { console.warn("Error BlockLooker"); console.warn(e); setTimeout(function () { LookingPinger() }, 15000); }
            });
            return 1;
}

$(document).ready(function () {
    BlockId = $("#BlockEditIdHidden").val();

    $(".BEPlayerControl").hide();
    $('#ExtLinkOn').hide();
    $('#ExtLinkOff').show();
    $('#ExtEditOn').show();
    $('#ExtEditOff').hide();
    
    
    $('#ExtLinkDate').datepicker();
    $('#ExtEditDate').datepicker();
    CheckExtLink();
    
   
    
    ReloadMedia();
  
  
   // BlockTypeId: JSON.parse($('#BlockEditTypeDropDown').val()).BlockTypeId,
    //onchange
    $("#BEEditor").html(HiLiteComments($("#BEEditor").html()));
    $("#BEEditor").css("visibility", "inherit");
    $("#BEEditor").on( "mouseup", function () {

       // BEOnMouseUP();

    });

    BEEditor.focus();
    BEOnInputText();
                $('#BlockEditPlannedTextBox').datetimepicker({
                showSecond: true,
                timeFormat: 'HH:mm:ss',
                timeOnly: true,

                });
                $('#BlockEditRealTextBox').datetimepicker({
                    showSecond: true,
                    timeFormat: 'HH:mm:ss',
                    timeOnly: true,

                });
                $("#BlockEditRedyDropDown").click(function (e) {  if (! $(e.target).prop('checked')) { $('#BlockEditApproveDropDown').prop('checked', false); }; });
    $("#BlockEditApproveDropDown").click(function (e) { if ($(e.target).prop('checked')) { clearSelection(); $('#BlockEditRedyDropDown').prop('checked', true); }; });
        });

function UpdatePeoplesListStatus()
{
   
    if (JSON.parse($('#BlockEditTypeDropDown').val()).BlockIsOperator == "True")
        $('#BlockEditOperatorDropDown').attr("disabled", false);
    else
        $('#BlockEditOperatorDropDown').attr("disabled", true);

    if (JSON.parse($('#BlockEditTypeDropDown').val()).BlockIsJockey == "True")
        $('#BlockEditJockeyDropDown').attr("disabled", false);
    else
        $('#BlockEditJockeyDropDown').attr("disabled", true);

}
function GetReadRate() {
    var ret = 17;
       if (JSON.parse($('#BlockEditTypeDropDown').val()).BlockIsJockey == "True")
        ret= JSON.parse($('#BlockEditJockeyDropDown').val()).UserRate;
    else
           ret = JSON.parse($('#BlockEditAutorDropDown').val()).UserRate;
       if (ret < 3)
           ret = 17;
       return ret;
}
function ReloadMedia(Page)
{
    if (typeof (Page) === 'undefined')
    {
        Page = 0;
        if($("#BEMediaContent").attr("SelectedId")!=null)
        {
            Page = parseInt($("#BEMediaContent").attr("SelectedId"));
            if (Page == null)
                Page = 0;
        }
    }
    var cookie = document.cookie.replace("NFWSession=", "")
    var jdata = {
        Cookie: cookie,
        NewsId: $("#BlockEditIdHidden").val(),
        NewsGroupId:Page
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/GetMediaList",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: MediaListSuccess,
        error: function (e) { console.warn("Error GetMediaList"); console.warn(e); }
    }).getAllResponseHeaders();
    return 1;
}
$("#BEMediaContent").sortable({ update: function (event, ui) { MediaListResorted(event, ui); } });
$("#BEMediaContent").disableSelection();
function MediaListSuccess(data)
{
    data = (JSON.parse(data.d));
    $('.BEMediaItem').each(function (Index, Element){$(Element).remove()});
    data.Media.forEach(MakeMediaItemsList);
    $('[data-toggle="tooltip"]').tooltip();
   // console.log('BEMediaContent SORTABLE' + $("#BEMediaContent").length);
   
    /*
    $('.BEMediaItem').each(function (Index, Element) {
        var find = false;
        log($(Element));
        for (var i = 0; i < data.Media.Count; i++)
        {
            if (data.Media[i].Id == $(Element).attr("MediaId"))
                find = true;
        }
        if (find == false)
            $(Element).remove();
    });*/
   
    CreateMediaPaging(data);
    $("#BEMediaContent").sortable({ update: function (event, ui) { MediaListResorted(event, ui); } });
    $("#BEMediaContent").disableSelection();
}
function MakeMediaItemsList(data)
{

    if ($("#BEMediaItem" + data.Id).length <= 0)
    {
        $("#BEMediaContent").append(CreateMediaItem(data));
        $("#MediaBlockEditReadyDropDown" + data.Id).prop("checked", data.Ready);
        $("#MediaBlockEditApproveDropDown" + data.Id).prop("checked", data.Approve);

        $("#BEMediaItem" + data.Id).attr("MarkIn", "0");
        $("#BEMediaItem" + data.Id).attr("MarkOut", "0");
        $("#BEMediaItem" + data.Id).attr("MediaType", data.BlockTypeName.toUpperCase());

    }
    $("#BEMediaItem" + data.Id).attr("MediaId", data.Id);
    $("#BEMediaItem" + data.Id).attr("MediaTypeId", data.BLockType);
    $("#BEMediaItem" + data.Id).attr("SortOrder", data.Sort);
    SortDivs($("#BEMediaContent"), "SortOrder");
}
function CreateMediaItem(data) {
   
    var ret = '<div  id="BEMediaItem' + data.Id + '" class="BEMediaItem ui-sortable-handle" >\
<div class="media">\
    <div id="BEMediaImageContainer' + data.Id + '" class="BEMediaImageContainer media-left media-top"  onclick="ClickMediaItem(\'' + data.Id + '\')">\
<img id="BEMediaImagePr' + data.Id + '" class="BEMediaImagePr" src="'+serverRoot+'handlers/GetBlockImage.ashx?MediaId=' + data.Id + '&rnd=' + Math.random() + '"/></div>\
<div class="media-body" class="BEMediaMediaBodyContainer">\
        <h5 class="media-heading"><small>' + data.BlockTypeName + ' ';
    if (data.BLockType == 2)
        ret += '<b class="mediaDur">'+ msToTime(data.BlockTime*1000)+'</b>';
    else   
        ret += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    //log(data);
    ret+=' </small>\
    \
                            \
                                <label for="MediaBlockEditReadyDropDown' + data.Id + '" class="caption caption-html" captionid="CapReady" style="font-size:small"  >' + langTable["CapReady"] + '</label>\
                                <input type="checkBox"  ID="MediaBlockEditReadyDropDown' + data.Id + '"   onchange="ChangeMediaStatus(' + data.Id + ')" /> \
                                <label for="MediaBlockEditApproveDropDown' + data.Id + '" class="caption caption-html" captionid="CapApprove" style="font-size:small">' + langTable["CapApprove"] + '</label> \
                                <input type="checkBox"  ID="MediaBlockEditApproveDropDown' + data.Id + '"  onchange="ChangeMediaStatus(' + data.Id + ')" /> \
                             \
                        </h5></small>\
   \
    <input type="text" maxlength="250" id="BEMediaItemTitle' + data.Id + '"class="form-control" style="padding-top: 5px; width:215px;height:1.5em" placeholder="введите название" value="' + data.Name + '" onblur="SaveMediaName(' + data.Id + ');"/>\
    ' + CreateMediaButtons(data.Id, data.BLockType) + '\
 \
    </div>\
    </small></div> \
</div>';
    return ret;

}
function CreateMediaButtons(MediaId, BLockType) {

    var ret = "\
    <div class='divnewsbuttons'><p><div class=\"btn-group btn-group-xs\" role=\"group\" aria-label=\"...\">\
<div class=\"btn-group btn-group-xs\" role=\"group\">";
    if (BLockType == 2) {

        ret += "<button type=\"button\" class=\"btn btn-default\" onclick=\"timeMediaToTimeBlock(" + MediaId + ", 0)\" data-toggle='tooltip' data-placement='top' data-original-title='" + langTable['CaptimeToBlock'] + "'><span class='glyphicon glyphicon-time' aria-hidden='true'></span></button>";
    }
    ret += "   <button type=\"button\" class=\"btn btn-default\" onclick=\"DownloadMedia(" + MediaId + ", 0)\" data-toggle='tooltip' data-placement='top' data-original-title='" + langTable['CapSave'] + "'><span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span></button>\
<button type=\"button\" class=\"btn btn-default\" onclick=\"MediaToEditor(" + MediaId + ", 0)\" data-toggle='tooltip' data-placement='top' data-original-title='" + langTable['CapLinkToText'] + "'><span class='glyphicon glyphicon-leaf' aria-hidden='true'></span></button>\
    <button type=\"button\" class=\"btn btn-default\" onclick=\"DeleteMedia(" + MediaId + ",event)\" data-toggle='tooltip' data-placement='top' data-original-title='Удалить'><span class='glyphicon glyphicon-trash' aria-hidden='true'></span></button>\
\
\
  </div>\
</div></p></p>";

    return ret;
}
function CreateMediaPaging(data)
{
  
    $(".BEMediaPage").each(function (Index, Elem) { $(Elem).remove() });
    if (data.Count < 12)
    { return; }
    
    /*if (data.StartItem == 0)
    {
        html += '<li class="disabled"><a href="#" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>';
    }
    else
    {
        html += '<li><a  aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>';
    }*/
    var html = "<div class='BEMediaPage'>";
    //html += '<nav><ul class="pagination  pagination-sm BEMediaPage">';
    for(var i=0; i< data.Count; i=i+10)
    {    
        if (i == data.StartItem) {
           // html += '<li><a onclick="ReloadMedia(' + i + ');">'+i+'</a></li>';
            html += "<input type='button' class='btn btn-default btn-warning btn-xs' disabled='true' value='" + (parseInt(i) + 10) + "' ></input>";
            $("#BEMediaContent").attr("SelectedId", i);
        }
        else
           // html += '<li><a onclick="ReloadMedia(' + i + ');">'+i+'</a></li>';
       // html += '<li class="active"><a onclick="ReloadMedia(' + i + ');">' + i + '<span class="sr-only"></span></a></li>';
            html += "<input type='button' class='btn btn-default btn-xs' value='" +  (parseInt(i) + 10) + "' onclick='ReloadMedia(" + i + ");'></input>";

    }
    // html += "</ul></nav></div>";
    html += "</div>";
    $(".BEMediaFixedHeightContainer").append(html);
   // $(".BEMediaFixedHeightContainer").html(html + $(".BEMediaFixedHeightContainer").html());

}
/////работа с плейером


function BEPlayerScrollerClick2( event)
{
    event.stopPropagation()
   // return;
    var elem = $(event.target);
    var perc = parseFloat(event.offsetX / $(elem).width());
    var dur = parseFloat(videojs('BEPlayer').duration());
    var pos = dur * perc;
    videojs('BEPlayer').currentTime(parseInt(pos));
    console.log(pos);
    
}
function BEClickVideoMarkIn() {
    try {
        if ($("#BEPlayer").length > 0) {
            BESetMarkIn(videojs('BEPlayer').currentTime());
        }
    }
    catch (e) { };
}
function BEDblClickVideoMarkIn()
{
    BESetMarkIn(0);
}
function BESetMarkIn(time)
{
    var MO = parseFloat($("#BEPlayerControlContainer").attr("markIn"));
    if (!typeof (MO) != "undefined" && MO > 0)
        time = 0;

    $('#BEPlayerMarkIn').html(TimeToTimecode(time));
    $("#BEPlayerControlContainer").attr("markIn", time);
    $("#BEPlayerScrollerMArkIn").css("width", parseInt((time/ videojs('BEPlayer').duration()) * 100) + "%");
   
}
function BEClickVideoMarkOut() {
    try {
        if ($("#BEPlayer").length > 0) {
            BESetMarkOut(videojs('BEPlayer').currentTime());
        }
    }
    catch (e) { }
}
function BEDblClickVideoMarkIn() {
    BESetMarkOut(9999);
}
function BESetMarkOut(time)
{
    
   /* console.log("MO duration " + videojs('BEPlayer').duration() + " " + time + " " + $("#BEPlayerControlContainer").attr("markIn"))
    console.log(typeof (time));
    console.log(time == 0);
    console.log(parseFloat(time) < parseFloat($("#BEPlayerControlContainer").attr("markIn")));
    */

    var MO = parseFloat($("#BEPlayerControlContainer").attr("markOut"));

    if (!typeof (MO) != "undefined" && MO > 0)
        time = 0;

    if (typeof (time) == 'undefined' || time == 0 || parseFloat(time) < parseFloat($("#BEPlayerControlContainer").attr("markOut")))
        time = 0;
  //  console.log("MO duration " + videojs('BEPlayer').duration() + " " + time + " " + $("#BEPlayerControlContainer").attr("markIn"))

    $('#BEPlayerMarkOut').html(TimeToTimecode(time));
    $("#BEPlayerControlContainer").attr("markOut", time);
    if(time>0)
        $("#BEPlayerScrollerMArkOut").css("right", "-" + parseInt((time / videojs('BEPlayer').duration()) * 100) + "%");
    else
        $("#BEPlayerScrollerMArkOut").css("right", "-" + 100 + "%");

}

function ShowPicture(MediaId) {
    $("#BEPlayerContainer").hide();
    
    $("#BEDocumentContainer").hide();

    $("#BEImageContainer").html('<img class="BEMediaImage" src="' + serverRoot + 'handlers/GetBlockImage.ashx?MediaId=' + MediaId + '&rnd=' + Math.random() + '" width=100% />');
    $("#BEImageContainer").show();
    
}
function ShowVideo(MediaId, markIn, markOut) {
    
    $("#BEImageContainer").hide();
    $("#BEDocumentContainer").hide();
    $("#BEPlayerContainer").show();
    ShowBEVideo(MediaId, markIn, markOut);
}
function ShowDocument(MediaId) {
   
    $("#BEPlayerContainer").hide();
    $("#BEImageContainer").hide();
    $("#BEDocumentContainer").show();

}

function ShowBEVideo(MediaId,markIn, markOut) {

   
    if (!(MediaId === "undefined")) {
        {
            var myPlayer = videojs('BEPlayer');
            myPlayer.pause();
            myPlayer.poster(serverRoot+"handlers/GetBlockImage.ashx?BlockId=" + MediaId +"&rnd"+Math.random());
            myPlayer.src(serverRoot + "handlers/GetBlockVideo.ashx?MediaId=" + MediaId + "&rnd" + Math.random());
            myPlayer.load();
        
            myPlayer.on("loadeddata", function (e) {
                e.target.play();
                if (typeof (markIn) != "undefined" && markIn != 0)
                    BESetMarkIn(markIn);
                else
                    BESetMarkIn(0);

                if (!typeof (markOut) != "undefined" && markOut != 0)
                    BESetMarkOut(markOut);
                else
                    BESetMarkOut(0);
            });
            $("#BEPlayerControlContainer").attr("MediaId", MediaId);

           // $("#BEPlayerControlContainer").attr("MarkIn", "0");

           ;
        }
    }
}
function BEPlaerTimeUpdate(e)
{
    var time=videojs('BEPlayer').currentTime();

    var markIn = $("#BEPlayerControlContainer").attr("markIn");
    if (time < markIn-0.3)
        videojs('BEPlayer').currentTime(markIn);

    var markOut = $("#BEPlayerControlContainer").attr("markOut");
    if (markOut > 0 && markOut > markIn)
    {
        if (time > markOut)
            videojs('BEPlayer').currentTime(markIn);
    }
   // console.log(time + " " + markOut);

    $("#BEPlayerTimecode").html(TimeToTimecode(time));
    $("#BEPlayerScrollerCurPos").css("left", parseInt((time / videojs('BEPlayer').duration()) * 100) + "%");
    if (isPlaying == false)
    {
       
        BEonStartPlaying();
    }
   
    clearTimeout(playingTimeout);
    playingTimeout = setTimeout(BEonStopPlaying, 500);
}
function BEAddVideoToEditor()
{
    if (!$(".panelBlockEditor").is(":visible"))
        return mediaToSocial($("#BEPlayerControlContainer").attr("MediaId"));

    var id = guid();
    var sr = serverRoot;
    if (sr == "undefined")
        sr = "/";
    var param = "<span id='" + id + "'>((" + "<img  class='editorVideoImage' src='" + sr + "handlers/GetBlockImage.ashx?MediaId=" +
        +$("#BEPlayerControlContainer").attr("MediaId") + "' markIn='" + $("#BEPlayerControlContainer").attr("markIn") + "'" +
        "markOut='" + $("#BEPlayerControlContainer").attr("markOut") + "' mediaId='" + $("#BEPlayerControlContainer").attr("MediaId") + "'" +
        "mediaType='2' onclick='clickVideoImgInEditor(this)'></img>))</span>";

    //$("#BEEditor").focus();
    // insertAtCursor($("#BEEditor"), "(param)");
    if ($("#cursorStart").length > 0) {
        $("#BEhiddenDiv").append(param);
        $("#" + id).insertAfter("#cursorStart");
        $("#BEEditor").focus();
    }
    else
        insertAtCursor($("#BEEditor"), param);

   // return "<img src='/handlers/GetBlockImage.ashx?MediaId=" + p1 + "' width='30' onclick='ShowVideo(" + p1 + "," + p2 + "," + p3 + ");' style='cursor:pointer; border:solid 1px green;'><small>" + str + "</small>";

   // var ret = "(( + ":" + $(elemFrom).attr("MediaId") + ":" + MarkIn + ":" + MarkOut + "))";

}
function BEonStartPlaying()
{
    isPlaying = true;
    $(".glyphicon-play").addClass("glyphicon-pause");
    $(".glyphicon-pause").removeClass("glyphicon-play");
}
function BEonStopPlaying()
{
    isPlaying = false;
    $(".glyphicon-pause").addClass("glyphicon-play");
    $(".glyphicon-play").removeClass("glyphicon-pause");
}
function BEEditorPlayPause() {
    try {
        if ($("#BEPlayer").length > 0) {
            var obj = videojs('BEPlayer');
            if (isPlaying)
                obj.pause();
            else
                obj.play();
        }
    } catch (e) {
        console.warn(e);};

    
}
function clickVideoImgInEditor(obj)
{
    
    ShowVideo($(obj).attr("MediaId"), $(obj).attr("markIn"), $(obj).attr("markOut"))
}
////// работа с листом медиа

function BEAddMediaFiles() {

    if ($('#BEFile').length == 0) {
        $("#BEhiddenDiv").append("<input id='BEFile'  type='file' style='visibility:hidden' multiple/>");
      
            $("#BEFile").change(function () {
                window.parent.newFileUpload(this.files, $("#BlockEditIdHidden").val());
                $("#BEFile").remove();
            });
       
    }
    $('#BEFile').click();
}

function MediaToEditor(mediaId){

    if (!$(".panelBlockEditor").is(":visible"))
        return mediaToSocial(mediaId);
   // var ret = "((NF:" + $(elemFrom).attr("MediaType") + ":" + $(elemFrom).attr("MediaId") + ":" + MarkIn + ":" + MarkOut + "))";
   // pasteHtmlAtCaret(HiLiteComments(ret));
 //   if ($("#BEMediaItem" + mediaId).attr("MediaTypeId") == 1)

    var param = "";
    var sr = serverRoot;

    if (sr == 'undefined')
        sr = "/";
    if ($("#BEMediaItem" + mediaId).attr("MediaTypeId") == 2) {

        param = "((" + "<img class='editorVideoImage' src='"+sr+"handlers/GetBlockImage.ashx?MediaId=" +
         +mediaId + "' markIn='" + 0 + "'" +
         "markOut='" + 0 + "' mediaId='" + mediaId + "'" +
         "mediaType='2' onclick='clickVideoImgInEditor(this)'/>))";

    } else if ($("#BEMediaItem" + mediaId).attr("MediaTypeId") == 1)
    {
        param = "((" + "<img class='editorImageImage' src='" + sr + "handlers/GetBlockImage.ashx?MediaId=" +
         +mediaId + "' markIn='0'" +
         "markOut='0' mediaId='" + mediaId + "'" +
         "mediaType='1' onclick='ShowPicture(\"" + mediaId + "\")'/>))";
    } else {
        param = "((" + "<img class='editorDocumentImage' src='" + sr + "handlers/GetBlockImage.ashx?MediaId=" +
         +mediaId + "' markIn='0'" +
         "markOut='0' mediaId='" + mediaId + "'" +
         "mediaType='0' onclick='BEDownloadDocument(\"" + mediaId + "\")'/>))";
    }

    insertAtCursor($("#BEEditor"), param);

}
function BEDownloadDocument(mediaId)
{
    DownloadMedia(mediaId,0);
}

function ClickMediaItem(MediaId)
{
    videojs('BEPlayer').pause();

    
    if ($("#BEMediaItem" + MediaId).attr("MediaTypeId") == 1) {
        
        ShowPicture(MediaId);
        return;
    }
    if ($("#BEMediaItem" + MediaId).attr("MediaTypeId") == 2) {
        $('#BEPlayerMarkIn').html('<small>00:00:00.00</small>');
        $('#BEPlayerMarkOut').html('<small>99:99:99.99</small>');
        ShowVideo(MediaId);
        return;
    }
    
    
    ShowDocument(MediaId);
        
}
function SaveMediaName(MediaId)
{
    var cookie = document.cookie.replace("NFWSession=", "")
    var jdata = {
        Cookie: cookie,
       
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: window.parent.serverRoot + "testservice.asmx/MediaAction?Action=Rename&Name=" + encodeURI($("#BEMediaItemTitle" + MediaId).val()) + "&MediaId=" + MediaId,
        data: JSON.stringify(jdata),
        dataType: "json",
        async: true,
        success: MediaActionSuccess,
        error: function (e) { console.warn("Error MediaAction"); console.warn(e); }
    });
    return 1;
}
function MediaActionSuccess(data) {
    
    ReloadMedia();
  //  showBlockEditorAlert("Сообщение сервера: " + data.d);
}
function MediaListResorted(event,ui)
{
    var str = "";
    $("#BEMediaContent").children().each(function (item) {
        if (str.length > 0)
            str += ",";
        str += $($(".BEMediaItem")[item]).attr("MediaId");
    });

   
    var jdata = {
       
        NewsId: str,
        
    }
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + 'testservice.asmx/MediaAction?Action=Resort',
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: MediaActionSuccess,
        error: function (e) { console.warn("Error MediaAction"); console.warn(e); }
    });
 
}
function ChangeMediaStatus(MediaId) {
    
    //MediaBlockEditApproveDropDown
    var ready = $("#MediaBlockEditReadyDropDown" + MediaId).prop("checked");
    var approve = $("#MediaBlockEditApproveDropDown" + MediaId).prop("checked");

    if (!ready && approve) {
        $("#MediaBlockEditApproveDropDown" + MediaId).prop("checked", false);
        $("#MediaBlockEditReadyDropDown" + MediaId).prop("checked", false);
    }
    else
    if (approve && (!ready))
    { 
        $("#MediaBlockEditReadyDropDown" + MediaId).prop("checked", true);
        $("#MediaBlockEditApproveDropDown" + MediaId).prop("checked", true);
    }
    /// еще раз после изменения статуса
     ready = $("#MediaBlockEditReadyDropDown" + MediaId).prop("checked");
     approve = $("#MediaBlockEditApproveDropDown" + MediaId).prop("checked");

     if (approve && ready) {
         timeMediaToTimeBlock(MediaId)
     }
    var cookie = document.cookie.replace("NFWSession=", "")
    var jdata = {
        Cookie: cookie,

    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/MediaAction?Action=ChangeStatus&Ready=" + ready + "&Approve=" + approve + "&MediaId=" + MediaId,
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: MediaActionSuccess,
        error: function (e) { console.warn("Error MediaAction"); console.warn(e); }
    });
    return 1;

}
function DeleteMedia(MediaId, event) {
    window.parent.mediaDelete(MediaId, event)
  
}
function mediaRemove(mediaId) {
    $(".BEMediaItem[mediaid='" + mediaId + "']").slowRemove();
}

function DownloadMedia(MediaId) {
    window.parent.DownloadFile(serverRoot + "handlers/GetMediaSourceFile.ashx?MediaId=" + MediaId);
    //document.getElementById('DownloadIFrame').src = "../handlers/DownloadSourceMediaFile?MediaId="+MediaId;
}

/////////////// работа с редактором блока

function HiLiteComments(text) {

    str = text.replace(/\(\(([^)]+)\)\)/g, replacer);
    str = str.replace(/\[\[([^\]]+)\]\]/g,replacerPrompt);
    str = str.replace(/^\s+/g, "");
    str = str.replace(/\s+$/g, "");
    str = str.replace(/\n/g, "<br/>")

  
    return str;
}
function replacerVideoTag(str, p1, p2, p3) {

    return "<img src='" + serverRoot + "handlers/GetBlockImage.ashx?MediaId=" + p1 + "' width='30' onclick='ShowVideo(" + p1 + "," + p2 + "," + p3 + ");' style='cursor:pointer; border:solid 1px green;'><small>" + str + "</small>";
}
function replacerImageTag(str, p1, p2, p3) {

    return "<img src='" + serverRoot + "handlers/GetBlockImage.ashx?MediaId=" + p1 + "' width='30'  onclick='ShowPicture(" + p1 + ");' style='cursor:pointer;border:solid 1px green;'><small>" + str + "</small>";
}
function replacerPrompt(str, p1) {

    return "[[<span style='color:green; font-weight:bold'>" + p1 + "</span>]]";
}


function replacer(str, p1) {
   
    p1 = p1.replace(/^NF\:VIDEO\:([0-9]+)\:([0-9]+)\:([0-9]+)/g, replacerVideoTag);
    p1 = p1.replace(/^NF\:IMAGE\:([0-9]+)\:([0-9]+)\:([0-9]+)/g, replacerImageTag);
    p1 = p1.replace(/NF\:\:VIDEO\:\:(\{.+\})/g, replacerVideoJSONTag);
    var cls = "";
    str.replace(/^\(\(([A-Z]{2,5})\:*\s/, function (b, mark) {
        cls = (mark);
        return b;
    });

    return "<span style='color:red'>((" + p1 + "))</span>";
}
function BEOnPasteText(elem, e) {
    
    elem.focus();
    if (e.clipboardData.getData('text/html').indexOf('class="editorVideoImage"') < 0) {
        pasteHtmlAtCaret(HiLiteComments(e.clipboardData.getData('text/plain')));

        e.stopPropagation();
        e.preventDefault();
    }

    BEOnInputText();
}
function getCaretPosition(editableDiv) {
    var caretPos = 0,
      sel, range;
    if (window.getSelection) {
        sel = window.getSelection();
        if (sel.rangeCount) {
            range = sel.getRangeAt(0);
            if (range.commonAncestorContainer.parentNode == editableDiv) {
                caretPos = range.endOffset;
            }
        }
    } else if (document.selection && document.selection.createRange) {
        range = document.selection.createRange();
        if (range.parentElement() == editableDiv) {
            var tempEl = document.createElement("span");
            editableDiv.insertBefore(tempEl, editableDiv.firstChild);
            var tempRange = range.duplicate();
            tempRange.moveToElementText(tempEl);
            tempRange.setEndPoint("EndToEnd", range);
            caretPos = tempRange.text.length;
        }
    }
    return caretPos;
}

function BEOnInputText()
{
    
    CalculateCalcTime($("#BEEditor").getPreText())
    BEArr.push({ txt: $("#BEEditor").html(), range: getCaretCharacterOffsetWithin("BEEditor") });
    while (BEArr.length > 20)
    {
        BEArr.shift();
    }
    return;       
}
var extrachrono = 0;

function CalculateCalcTime(text) {

    
    $("#BlockEditCalcTextBox").val(getTextToCalc(text));
    return;
}
function getTextToCalc(text) {
    extrachrono = 0;

    text = text.replace(/\(\(([^)]+)\)\)/g, FindChronoInText);
    $("#BESymbolsCount").html(langTable['CapSymbolsCount'] + ": " + text.length);
    text = text.replace(/[0-9]/g, "");

    //$("#BlockEditCalcTextBox").val(msToTime(((text.length / GetReadRate()) + parseInt(extrachrono)) * 1000));
    Blockcalctime = msToTime(((text.length / GetReadRate()) + parseInt(extrachrono)) * 1000);
    return Blockcalctime;
}

function FindChronoInText(text, p1) {
    //ХР 00:00:00
    var regexp = /ХР\s([0-9]+)\:([0-9]+)\:([0-9]+)/g;
    var result = regexp.exec(p1);
    if (result != null) {
        extrachrono += parseInt(parseInt(result[3]) + (result[2] * 60) + (result[1] * 60 * 60));
    }
    else {
        regexp = /ХР\s([0-9]+)\:([0-9]+)/g;
        var result = regexp.exec(p1);
        if (result != null) {
            extrachrono += parseInt(parseInt(result[2]) + (result[1] * 60));
        }
    }

}




var selRange;
function BEOnMouseUP(elem)
{
    var userSelection = window.getSelection().toString();
    if (userSelection.length > 0) {
        // console.log();
        CalculateCalcTime(userSelection);


    }
    else {
        CalculateCalcTime($("#BEEditor").getPreText());
        
    }
}
function pasteHtmlAtCaret(html) {
    var sel, range;
    if (window.getSelection) {
       
        // IE9 and non-IE
        sel = window.getSelection();
        if (sel.getRangeAt && sel.rangeCount) {
            range = sel.getRangeAt(0);
            range.deleteContents();

            // Range.createContextualFragment() would be useful here but is
            // non-standard and not supported in all browsers (IE9, for one)
            var el = document.createElement("div");
            el.innerHTML = html;
            var frag = document.createDocumentFragment(), node, lastNode;
            while ((node = el.firstChild)) {
                lastNode = frag.appendChild(node);
            }
            range.insertNode(frag);

            // Preserve the selection
            if (lastNode) {
                range = range.cloneRange();
                range.setStartAfter(lastNode);
                range.collapse(true);
                sel.removeAllRanges();
                sel.addRange(range);
             
            }
        }
    } else if (document.selection && document.selection.type != "Control") {
        // IE < 9
        document.selection.createRange().pasteHTML(html);
    }
}
/////////////////////////
///////////////// Drag To Text Functions;
function getSelectionHtml() {
    var html = "";
    if (typeof window.getSelection != "undefined") {
        var sel = window.getSelection();
        if (sel.rangeCount) {
            var container = document.createElement("div");
            for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                container.appendChild(sel.getRangeAt(i).cloneContents());
            }
            html = container.innerHTML;
        }
    } else if (typeof document.selection != "undefined") {
        if (document.selection.type == "Text") {
            html = document.selection.createRange().htmlText;
        }
    }
    return (html);
}
$(document).ready(function () {

    document.addEventListener('copy', function (e) {
        
        if ($("#BEEditor").is(":focus")) {
           
            var div = Document.createElement("div");
            div.innerHTML = getSelectionHtml();
            var newClipText = div.textContent || div.innerText || "";
            e.clipboardData.setData('text/plain', newClipText);
            e.preventDefault(); // default behaviour is to copy any selected text


            //getPreText
        }
       
    });

  //  setInterval(ReloadMedia, 20 * 1000);

  //  $("#BEPlayerControlContainer").draggable({ helper: "clone" });
 /*   $("#BEEditor").droppable({
        activate: function (event, ui) {
            ui.helper.css("width", "380px").css("border-color", "#bbbbbb").css("background", "opacity", "0.7").css('z-index', 2000);
            var color = '#cccccc';
            var rgbaCol = 'rgba(' + parseInt(color.slice(-6, -4), 16)
                + ',' + parseInt(color.slice(-4, -2), 16)
                + ',' + parseInt(color.slice(-2), 16)
                + ',0.5)';
            ui.helper.css('background-color', rgbaCol)
                $("#BEEditor").addClass("BEDaDHiLight");
        },
        deactivate: function () {
            $("#BEEditor").removeClass("BEDaDHiLight");
        },
        over: function (event, ui) {
            $("#BEEditor").addClass("DaDOver");
        },
        out: function (event, ui) {
            $("#BEEditor").removeClass("DaDOver");
        },
        drop: function (event, ui) {
            $("#BEEditor").removeClass("DaDOver");          
            BEDropOnBEText(ui.draggable);
        },
        tolerance: 'touch',
        ////////
    });*/

});
function BEDropOnBEText(elemFrom)
{
    $("#BEEditor").focus();
    var MarkIn = $(elemFrom).attr("MarkIn");
    //MarkIn = (parseInt(MarkIn * 100));


    var MarkOut = $(elemFrom).attr("MarkOut");
  //  MarkOut = (parseInt(MarkOut * 100)) ;


    var ret = "((NF:" + $(elemFrom).attr("MediaType") + ":" + $(elemFrom).attr("MediaId") + ":" + MarkIn + ":" + MarkOut + "))";
    pasteHtmlAtCaret(HiLiteComments(ret));
}


///////////////////
    function CheckExtLink() {
        var Cookie = getCookie("NFWSession");

        var jdata = {
            Cookie: Cookie,
            BlockId: BlockId,

        };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/CheckExtLink",
            data: JSON.stringify(jdata, null, 2),
            dataType: "json",
            async: true,
            success: ExtLinkSuccess,
            error: function (e) { console.warn("Error CheckExtLink"); console.warn(e); }
        });
   
}

    function GetExtLink(enabled) {
        
        var DateExp = $("#ExtLinkDate").val();
        var IsExp = $("#ExtLinkApplyDate").prop("checked");
        var IsCommentable = $("#ExtIsCommentableCB").prop("checked");

       // log(IsCommentable);

        if (IsExp == false) {
            $("#ExtLinkDate").prop('readonly', true);;
            document.getElementById("ExtLinkDate").readOnly = true;
            //log("GetExtLink" + $("#ExtLinkApplyDate").prop("checked"));
        }
        else {
            $("#ExtLinkDateExtLinkDate").prop('readonly', false);
            document.getElementById("ExtLinkDate").readOnly = false;
        }
        var Cookie = getCookie("NFWSession");
        
        var jdata = {
            Cookie: Cookie,
            Enabled: enabled,
            IsExp: IsExp,
            ExpDate: $("#ExtLinkDate").val(),
            BlockId: $("#BlockEditIdHidden").val(),
            IsCommentable: IsCommentable,
            
        };

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/GrantExtLink",
            data: JSON.stringify(jdata, null, 2),
            dataType: "json",
            async: true,
            success: ExtLinkSuccess,
            error: function (e) { console.warn("Error GrantExtLink"); console.warn(e); }
        });


    }
    function getBlockId()
    {
        return $("#BlockEditIdHidden").val();
    }
    function ExtLinkSuccess(data)
    {
        //log(data);
        var InData = JSON.parse(data.d);
        if (data.d.length > 10)
            UpdateExtLinkStstus(InData);
        else
        {
            $('#ExtLinkOn').hide();
            $('#ExtLinkOff').show();
        }
        
    }
    function UpdateExtLinkStstus(data)
    {
        
       
        $("#ExtLinkDate").val(data.ExpDate),
        $("#ExtLinkApplyDate").prop("checked", data.IsExp);
        $("#ExtLink").val(data.Link);
        $("#ExtIsCommentableCB").prop("checked", data.IsCommentable);



        if (data.ExperienceWarnining && $("#ExtLinkApplyDate").prop("checked"))
            $("#ExtLinkDate").css("background-color", "#f0ad4e");
        else
            $("#ExtLinkDate").css("background-color", "white");


        if (data.Enabled == true) {
            $('#ExtLinkOn').show();
            $("#ExtLink").focus().select(); 

            console.log("copy");
            $('#ExtLinkOff').hide();

            
        }
            else{
                $('#ExtLinkOn').hide();
                $('#ExtLinkOff').show();
            }

    }
    var clickId = 0;
    var StartCalcTime;
    function ChronoCalculatedStart(elem) {
        
        if ($(elem).attr("clicked") == "false") {
            $(elem).attr("clicked", "true");
            var dt = new Date();
            StartCalcTime = dt.getTime();
            clickId = setInterval(function () {
                $("#labelBlockEditRealTextBox").css("color", "red");
                setTimeout(function () { $("#labelBlockEditRealTextBox").css("color", 'white') }, 250);
            }, 500);
        }
        else {
            $(elem).attr("clicked", "false");
            clearInterval(clickId);
            $("#labelBlockEditRealTextBox").css("color", 'white');

            var dt = new Date();
            var duration = parseInt((dt.getTime() - StartCalcTime) );


            extrachrono = 0;

            text = $("#BEEditor").getPreText().replace(/\(\(([^)]+)\)\)/g, FindChronoInText);

            duration=duration+parseInt(extrachrono)*1000;


            var milliseconds = parseInt((duration % 1000) / 100)
            , seconds = parseInt((duration / 1000) % 60)
            , minutes = parseInt((duration / (1000 * 60)) % 60)
            , hours = parseInt((duration / (1000 * 60 * 60)) % 24);

            hours = (hours < 10) ? "0" + hours : hours;
            minutes = (minutes < 10) ? "0" + minutes : minutes;
            seconds = (seconds < 10) ? "0" + seconds : seconds;

            $("#BlockEditRealTextBox").val(hours + ":" + minutes + ":" + seconds);
            //alert(hours + ":" + minutes + ":" + seconds + "." + milliseconds);
        }
    }
//////////

var editable,
        selection, range;
var afterFocus = [];
$(document).ready(function () {
    console.log("READY");
    editable = document.getElementById('BEEditor');
    editable.onkeyup = captureSelection;

    // Recalculate selection after clicking/drag-selecting
    editable.onmousedown = function (e) {
        editable.className = editable.className + ' selecting';
    };
    editable.onblur = function (e) {
        try{
            var cursorStart = document.createElement('span'),
                collapsed = !!range.collapsed;

            cursorStart.id = 'cursorStart';
            cursorStart.appendChild(document.createTextNode('\u2038'));

            // Insert beginning cursor marker
            range.insertNode(cursorStart);

            // Insert end cursor marker if any text is selected
            if (!collapsed) {
                var cursorEnd = document.createElement('span');
                cursorEnd.id = 'cursorEnd';
                range.collapse();
                range.insertNode(cursorEnd);
            }
        }
        catch (ex) {
            console.warn("error");
            console.warn(ex);
        }
    };

    // Add callbacks to afterFocus to be called after cursor is replaced
    // if you like, this would be useful for styling buttons and so on
 
    editable.onfocus = function (e) {
        // Slight delay will avoid the initial selection
        // (at start or of contents depending on browser) being mistaken
        setTimeout(function () {
            var cursorStart = document.getElementById('cursorStart'),
                cursorEnd = document.getElementById('cursorEnd');

            // Don't do anything if user is creating a new selection
            if (editable.className.match(/\sselecting(\s|$)/)) {
                if (cursorStart) {
                    cursorStart.parentNode.removeChild(cursorStart);
                }
                if (cursorEnd) {
                    cursorEnd.parentNode.removeChild(cursorEnd);
                }
            } else if (cursorStart) {
                captureSelection();
                var range = document.createRange();

                if (cursorEnd) {
                    range.setStartAfter(cursorStart);
                    range.setEndBefore(cursorEnd);

                    // Delete cursor markers
                    cursorStart.parentNode.removeChild(cursorStart);
                    cursorEnd.parentNode.removeChild(cursorEnd);

                    // Select range
                    selection.removeAllRanges();
                    selection.addRange(range);
                } else {
                    range.selectNode(cursorStart);

                    // Select range
                    selection.removeAllRanges();
                    selection.addRange(range);

                    // Delete cursor marker
                    document.execCommand('delete', false, null);
                }
            }

            // Call callbacks here
            for (var i = 0; i < afterFocus.length; i++) {
                afterFocus[i]();
            }
            afterFocus = [];
            
            // Register selection again
            captureSelection();
        }, 10);
    };

});
    // Populates selection and range variables
var captureSelection = function (e) {
    
        // Don't capture selection outside editable region
        var isOrContainsAnchor = false,
            isOrContainsFocus = false,
            sel = window.getSelection(),
            parentAnchor = sel.anchorNode,
            parentFocus = sel.focusNode;

        while (parentAnchor && parentAnchor != document.documentElement) {
            if (parentAnchor == editable) {
                isOrContainsAnchor = true;
            }
            parentAnchor = parentAnchor.parentNode;
        }

        while (parentFocus && parentFocus != document.documentElement) {
            if (parentFocus == editable) {
                isOrContainsFocus = true;
            }
            parentFocus = parentFocus.parentNode;
        }

        if (!isOrContainsAnchor || !isOrContainsFocus) {
            return;
        }

        selection = window.getSelection();

        // Get range (standards)
        if (selection.getRangeAt !== undefined) {
            range = selection.getRangeAt(0);

            // Get range (Safari 2)
        } else if (
            document.createRange &&
            selection.anchorNode &&
            selection.anchorOffset &&
            selection.focusNode &&
            selection.focusOffset
        ) {
            range = document.createRange();
            range.setStart(selection.anchorNode, selection.anchorOffset);
            range.setEnd(selection.focusNode, selection.focusOffset);
        } else {
            // Failure here, not handled by the rest of the script.
            // Probably IE or some older browser
        }
    };

    // Recalculate selection while typing
    
document.onmouseup = function (e) {
    try {
        if (editable.className.match(/\sselecting(\s|$)/)) {
            editable.className = editable.className.replace(/ selecting(\s|$)/, '');
            captureSelection();
        }
    }
    catch (e) { }
    };
    function getCaretCharacterOffsetWithin(element) {
        return;
        var caretOffset = 0;
        if (typeof window.getSelection != "undefined") {
            var range = window.getSelection().getRangeAt(0);
            var preCaretRange = range.cloneRange();
            preCaretRange.selectNodeContents(element);
            preCaretRange.setEnd(range.endContainer, range.endOffset);
            caretOffset = preCaretRange.toString().length;
        } else if (typeof document.selection != "undefined" && document.selection.type != "Control") {
            var textRange = document.selection.createRange();
            var preCaretTextRange = document.body.createTextRange();
            preCaretTextRange.moveToElementText(element);
            preCaretTextRange.setEndPoint("EndToEnd", textRange);
            caretOffset = preCaretTextRange.text.length;
        }
        return caretOffset;
    }
    $(document).keydown(function (event) {
       // console.log(event);
        if (!event.ctrlKey || event.keyCode != 90) {
            return;
        }
        event.preventDefault();
       
        if (BEArr.length > 0 && $("#BEEditor").is(":focus"))
        {
            var elem=BEArr.pop()
           

            
            $("#BEEditor").html(elem.txt);

          
           
        }
           
        
    });

    function insertTextAtCursor(text) {
        var sel, range, html;
        if (window.getSelection) {
            sel = window.getSelection();
            if (sel.getRangeAt && sel.rangeCount) {
                range = sel.getRangeAt(0);
                range.deleteContents();
                range.insertNode(document.createTextNode(text));
            }
        } else if (document.selection && document.selection.createRange) {
            document.selection.createRange().text = text;
        }
    }

//// работа с обновлением медиа

    function BElrvStateCreate(mediaId) {
        return '<div id="BELRVSC' + mediaId + '" class="BElrvStateContainer">\
            LRV: <span id="BELRVSCtext' + mediaId + '">55%</span>\
               \
                <div id="BELRVSCprogress' + mediaId + '"  class="BElrvProgressContainer BElrvProgressPosition"></div>\
                <div id="BELRVSCerror' + mediaId + '"  class="BElrvErrorContainer BElrvProgressPosition"></div>\
                <div id="BELRVSCsucess' + mediaId + '" class="BElrvSuccessContainer BElrvProgressPosition"></div>\
            </div>';
    }
    function BElrvStatecheck(mediaId) {
        if ($("#BELRVSC" + mediaId).length < 1) {
            $("#BEMediaImageContainer" + mediaId).append(BElrvStateCreate(mediaId));
        }
    }
    function BElrvStateChange(mediaId, val) {

        BElrvStatecheck(mediaId);
        $("#BELRVSCprogress" + mediaId).show();
        $("#BELRVSCsucess" + mediaId).hide();
        $("#BELRVSCerror" + mediaId).hide();

        if (typeof (val) == 'undefined' || val == '')
            val = 'started';
        $("#BELRVSCtext" + mediaId).html(val);
        if (val.indexOf("%") > 0) {
            $("#BELRVSCprogress" + mediaId).css("width", "calc(" + val + " - 10px)");
            $("#BELRVSCprogress" + mediaId).show();
        }
        else {
            $("#BELRVSCprogress" + mediaId).hide();
        }
    }
    function BElrvStateErr(mediaId) {
        BElrvStatecheck(mediaId);
        $("#BELRVSCprogress" + mediaId).hide();
        $("#BELRVSCsucess" + mediaId).hide();
        $("#BELRVSCerror" + mediaId).show();

        $("#BELRVSCtext" + mediaId).html("error");
    }
    function BElrvStateSucc(mediaId) {
        BElrvStatecheck(mediaId);
        $("#BELRVSCprogress" + mediaId).hide();
        $("#BELRVSCerror" + mediaId).hide();
        $("#BELRVSCsucess" + mediaId).show();

        $("#BELRVSCtext" + mediaId).html("OK");

        setTimeout(function () {
            $("#BELRVSC" + mediaId).fadeOut(500, function () {
                $("#BELRVSC" + mediaId).remove();
            });
        }, 5000)
    }
    function mediaImgCreate(mediaId) {
        // console.log("mediaImgCreate " + mediaId);
        // console.log($("#BEMediaImagePr" + mediaId).length)
        $("#BEMediaImagePr" + mediaId).attr("src", serverRoot+'handlers/GetBlockImage.ashx?MediaId=' + mediaId + '&rnd=' + Math.random());
     //   $("#BEMediaImageContainer" + mediaId).prepend('<img id="BEMediaImagePr' + mediaId + '" class="BEMediaImagePr" src="/handlers/GetBlockImage.ashx?MediaId=' + mediaId + '&rnd=' + Math.random() + '"/>');

    }
    function mediaThCreate(mediaId) {
        //   console.log("mediaThCreate " + mediaId);

      //  $("#BEMediaImagePr" + mediaId).remove();
        //$("#BEMediaImageContainer" + mediaId).prepend('<img id="BEMediaImagePr' + mediaId + '" class="BEMediaImagePr" src="/handlers/GetBlockImage.ashx?MediaId=' + mediaId + '&rnd=' + Math.random() + '"/>');
        $("#BEMediaImagePr" + mediaId).attr("src", serverRoot+'handlers/GetBlockImage.ashx?MediaId=' + mediaId + '&rnd=' + Math.random());

    }
    function initMainHeadMenu() {
        $(".mainHeadMenuItem").click(mainHeadMenuItemClick);
        $(".mainHeadMenu").fadeIn(500);
        $(".mainHeadMenuItem").last().addClass("mainHeadMenuItemlast");
      //  BEeditrActivate();
        
    }
    function mainHeadMenuItemClick(elem) {
        //console.log(elem);
        if ($(elem.target).hasClass("mainHeadMenuItemActive"))
            return;

        $(".mainHeadMenuItem").removeClass("mainHeadMenuItemActive");
        $(elem.target).addClass("mainHeadMenuItemActive");
        //centralTowerActivate();
        BEeditrActivate();
    }
    function BEeditrActivate()
    {
        $(".BEpanel").hide();
        $("." + $(".mainHeadMenuItemActive").attr("panel")).fadeIn(500, function () {
            // if (!$(".CenralTowerRColWr{").is(":visible"))
            //    setTimeout(function () { $(".CenralTowerRColWr{").fadeIn(500); }, 1000);
        });
        if($(".mainHeadMenuItemActive").attr("panel")=='panelBlockSocial')
        {
            if ($('.BEsocialBox').length == 0) {
                $(".panelBlockSocial").load("/Blocks/blockSocial.aspx?id=" + $("#BlockEditIdHidden").val() + "&rnd" + Math.random(), function () {
                    addTextToSocial();
                });
            }
            else
                addTextToSocial();
        }
        if ($(".mainHeadMenuItemActive").attr("panel") == 'panelBlockHistory') {
            if ($('.BEhistoryBox').length == 0) {
                $(".panelBlockHistory").load("/Blocks/blockHistory.aspx?id=" + $("#BlockEditIdHidden").val() + "&rnd" + Math.random(), function () {
                    addTextToSocial();
                });
            }
        }
        else {
            $(".panelBlockHistory").html( '<img class="loadingImg" src="'+serverRoot+'Images/loading.gif" />');
        }
            //BEsocialBox
    }
        
    function addTextToSocial()
    {
        if ($("#BSText").val().length == 0) {
            var str = $("#BEEditor").getPreText().replace('\u2038', "");
            $("#BSText").val(str.replace(/\(NF[^).]+\)/g, "(MEDIA)"));
            BSsave();
        };
        if ($("#BStitleText").val().length == 0) {
            $("#BStitleText").val($('#BlockEditNameTextBox').val());
            BSsave();
        };
    }
    function BEFontSizeChange(val) {
        $(".BEEditor").css("font-size", val);
        window.localStorage.setItem("BEEditorFontSize", val);
    }

    function initBESynchTemplate() {
        $('#BESynchTemplate').attr('placeholder', langTable['CapFioRequest']);

        $('#BESynchTemplate').typeahead({
            source: function (query, process) {
                console.log(query);
                $.post(serverRoot + "handlers/SynhTemplateGet.ashx", { txt: query },
                    function (data) {
                        var ret = new Array();
                        data.items.forEach(function (el) {
                            ret.push(el.id + "|" + el.name + "|" + el.cap);
                        });
                        // console.warn(data);
                        return process(ret);
                    }, 'json');
            }
            , highlighter: function (item) {
                var parts = item.split('|');
                parts.shift();
                return parts[0];

            }, updater: function (item) {
                var parts = item.split('|');
                var id = parts.shift();

                setTimeout(function () {
                    $("#BESynchTemplate").val('');
                    $("#BEEditor").focus();
                    setTimeout(function () {
                        var position = $("#addSot").offset();
                        addSot({ pageX: position.left, pageY: position.top }, parts[0], parts[1]);
                       // insertAtCursor($("#BEEditor"), HiLiteComments('<br>((SOT ХР 00:00:00<br>NAME: ' + parts[0] + '<br>TITLE: ' + parts[1] + '<br>))<br>'));

                    }, 50);
                    //    $("#BEEditor").append(HiLiteComments('<br>((СИНХРОН ХР 00:00:00<br>СИНХРОНИРУЕМЫЙ: ' + parts[0] + '<br>ТИТР: ' + parts[1] + '<br>))<br>'));
                }, 200);


                return "";
            }
        });
}
function BEshowFullScreen(e) {
    $("#BEpanelBody").toggleClass("fullscreen")

    if ($("#BEpanelBody").hasClass("fullscreen"))
        localStorage.setItem("BEFullScreen", true)
    else
        localStorage.removeItem("BEFullScreen")
}
function BEshowHistory(e)
{
     //debugger;
        var id = $("#BlockEditIdHidden").val();
        $(".BEhistoryWr").remove();
        $('body').append($(div)
             .addClass("BEhistoryWr")
             .css("left", "calc(" + e.pageX + "px - 450px)")
             .css("top", e.pageY - 20 + "px")
             .append($(div)
                 .addClass("panel-default")
                 .append($(div)
                 .addClass("panel-heading")
                 .attr("id", "BEhistoryHeader")
                 .css("cursor", "pointer")
                 .html($("<div style='float:left'><span>" + langTable['History'] + "</span></div>"))
                 .append($('<div>X</div>')
                    .addClass("closeBtn")
                    .click(function () {
                        $(".BEhistoryWr").fadeOut(500, function () {
                            $(".BEhistoryWr").remove();
                        })
                    })
                    )
                    .append($(div)
                    .addClass("clear")
                 )
            )
                 .append($(div)
                 .addClass("panel-body")
                 .addClass("BShistoryBox BEhistoryBox")
                 .append($("<img/>")
                    .attr("src", serverRoot + "Images/loading.gif")
                    .addClass("loadingImgSmall")
                  )
                 .append(" loading...")
                )


             )
            // .mouseleave(function () { $(".BEhistoryWr").fadeOut(500, function(){$(".BEhistoryWr").remove();}) })
             );

        $(".BEhistoryWr").draggable({ handle: '#BEhistoryHeader' });
        BEhistoryGet(id);
}
function BEhistoryGet(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/blockHistoryNoTextGet",
        data: JSON.stringify({id:id}),
        dataType: "json",
        async: true,
        success: BEhistoryGetSucc,
        error: function (data) {
            console.warn("error blockHistoryNoTextGet");
            console.warn(data);
        }
    });
}
function BEhistoryGetSucc(data)
{
    var dt = JSON.parse(data.d);
    $(".BEhistoryBox").html("");

    dt.forEach(function (itm) {
        $(".BEhistoryBox").append($(div)
                .attr("id", "BEhistory" + itm.id)
                .addClass("BShistoryItem BEhistoryItem")
                .append($(div)
                .addClass("BEhistoryRow")
                
                    .append($(div)
                        .addClass("BEhistoryDate")
                        .html(itm.date)
                    )
                    .append($(div)
                        .addClass("BEhistoryDate")
                        .html(itm.user)
                    )
                )
                .append($(div)
                    .addClass("BEhistoryTextWrapper")
                    .attr("id", "BEhistoryTextWrapper" + itm.id)
                )

               .click(function () { BEhistoryTextGet(itm.id);  })
            );
    });
}
function BEhistoryTextGet(id) {
    if ($("#BEhistoryTextContent" + id).is(":Visible")) {
        $("#BEhistoryTextContent" + id).fadeOut(500, function () { $(".BEhistoryTextWrapper").html("") })
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/stringblockHistoryTextGet",
            data: JSON.stringify({ id: id }),
            dataType: "json",
            async: true,
            success: BEhistoryTextGetSucc,
            error: function (data) {
                console.warn("error stringblockHistoryTextGet");
                console.warn(data);
            }
        });
    }
}
function BEhistoryTextGetSucc(data)
{
    var itm = JSON.parse(data.d);
    $(".BEhistoryTextWrapper").html("");
    $("#BEhistoryTextWrapper" + itm.id).append(
        $(div)
            .attr("id", "BEhistoryTextContent" + itm.id)
            .html(formatTextNoVideoTag(itm.text))
            .addClass("BEhistoryTextContent")
            .click(function () {
                $(".BEEditor").html($(".BEEditor").html() + HiLiteComments("<br>===NF::HISTORY===<br>" +formatTextNoVideoTag( itm.text) + ""));
                $(".BEhistoryWr").fadeOut(500, function () { $(".BEhistoryWr").remove() });
            })
        );
   
}
function timeMediaToTimeBlock(mediaId) {


    $("#BlockEditRealTextBox").val($("#BEMediaItem" + mediaId).find(".mediaDur").html());
}
function mediaUpdateChrono(mediaId, chrono) {
   // console.log('BEMediaItem' + mediaId + "    " + chrono);
    var elem = $("#BEMediaItem" + mediaId);
    if (elem.length > 0) {
        console.log("find target");
        $(elem).find(".mediaDur").html(chrono);
    }

}
function addSot(e, name, title) {


    var markIn = "00:00:00";
    var markOut = "00:00:00";
    if( $("#BEPlayerControlContainer").length>0){
        markIn = $("#BEPlayerControlContainer").attr("markIn");
        markOut = $("#BEPlayerControlContainer").attr("markOut");
    }
    if (typeof (markIn) == 'undefined')
        markIn = "00:00:00";
    else
        markIn = msToTime((markIn) * 1000);

    if (typeof (markOut) == 'undefined')
        markOut = "00:00:00";
    else
        markOut = msToTime((markOut) * 1000);
    $(".BEhistoryWr").remove();
    $('body').append($(div)
         .addClass("BEhistoryWr")
         .css("left", "calc(" + e.pageX + "px + 50px)")
         .css("top", e.pageY - 20 + "px")
         .append($(div)
             .addClass("panel-default")
             .append($(div)
             .addClass("panel-heading")
             .attr("id", "BEhistoryHeader")
             .css("cursor", "pointer")
             .html($("<div style='float:left'><span>" + langTable['SOT'] + "</span></div>"))
             .append($('<div>X</div>')
                .addClass("closeBtn")
                .click(function () {
                    $(".BEhistoryWr").fadeOut(500, function () {
                        $(".BEhistoryWr").remove();
                    })
                })
                )
                .append($(div)
                .addClass("clear")
             )
        )
             .append($(div)
             .addClass("panel-body")
             .addClass("BShistoryBox BEhistoryBox")
             .append($("<img/>")
                .attr("src", serverRoot + "Images/loading.gif")
                .addClass("loadingImgSmall")
              )
             .append(" loading...")
             .load("/elements/sotEditor.aspx", function () {
                 $(".sotName").focus().val(name);
                 $(".sotTitle").val(title);
                 if (name != null)
                     $(".sotText").focus();
                 $("#addSotIn").val(markIn);
                 $("#addSotOut").val(markOut);
                 $("#addSotChrono").val(msToTime((checkTimes("#addSotOut") - checkTimes("#addSotIn")) * 1000));
             })
            )


         )
        // .mouseleave(function () { $(".BEhistoryWr").fadeOut(500, function(){$(".BEhistoryWr").remove();}) })
         );

    $(".BEhistoryWr").draggable({ handle: '#BEhistoryHeader' });
}
function addSotEditChrono() {
    var txt = (msToTime((checkTimes("#addSotChrono") * 1000)));
    $("#addSotChrono").val(txt);
}

function addSotEditOut() {
    $("#addSotChrono").val(msToTime((checkTimes("#addSotOut") - checkTimes("#addSotIn"))*1000));
}
function insertSot() {
    

    var txt = ("\n((SOT ХР " + (msToTime((checkTimes("#addSotChrono") * 1000))) + "  [in " + (msToTime((checkTimes("#addSotIn") * 1000))) + " out " + (msToTime((checkTimes("#addSotOut") * 1000))) + "]\nNAME: " + $(".sotName").val().replace("((", "").replace("))", "") + "\nTITLE: " + $(".sotTitle").val().replace("((", "").replace("))", "") + "\nTEXT: " + $(".sotText").val().replace("((", "").replace("))", "") + " ))");
    var pos = $("#BEEditor").scrollTop();
    setTimeout(function () {
        
        $("#BEEditor").focus();
      
        setTimeout(function () {
            insertAtCursor($("#BEEditor"), txt);
            $("#BEEditor").scrollTop(pos);
            BEOnInputText();

        }, 50);
        //    $("#BEEditor").append(HiLiteComments('<br>((СИНХРОН ХР 00:00:00<br>СИНХРОНИРУЕМЫЙ: ' + parts[0] + '<br>ТИТР: ' + parts[1] + '<br>))<br>'));
    }, 200);
    $(".BEhistoryWr").fadeOut(500, function () { $(".BEhistoryWr").remove();  });
}
function BESortMediaFiles(e) {
    NFconfirm(langTable['WarningSortMediaFiles'], e.pageX, e.pageY, 1, BESortMediaFilesConfirmed);
}
function BESortMediaFilesConfirmed(e) {
    $("#BEMediaContent").html("").loading50();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/mediaSortByAZ",
        data: JSON.stringify({ id: parseInt($("#BlockEditIdHidden").val()) }),
        dataType: "json",
        async: true,
        success: function (e) { $("#BEMediaContent").html(""); ReloadMedia(); },
        error: function(e){console.warn("ERROR mediaSortByAZ");}
    });
}
function intAddGeoSelect() {
  
    $("#addGeo").click(function (e) { insertAtCursor(BEEditor, "((GEO: ))") });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/BEinsertGeo",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (e) {
            try {
               
                var data = JSON.parse(e.d);
                var $ctrl = $(".addGeoSelect");
                console.log("intAddGeoSelect", $ctrl)
                data.forEach(function (dt) {
                    
                    if (dt.id == 0)
                        $ctrl.append('<li role="separator" class="divider"></li>');
                    else
                        $ctrl.append($("<li></li>").append($("<a href='#'></a>").html(dt.name).attr("val", dt.val).click(function (ev) {
                            var $c = $(ev.currentTarget);
                            insertAtCursor(BEEditor, "((GEO: " + dt.name+"))");
                        })));
                });
              
            }
            catch (ex) { console.warn(ex); }

        },
        error: function (e) { }
    });

}
function intAddSrcSelect() {
    $("#addSrc").click(function (e) { insertAtCursor(BEEditor, "((SOURCE: ))") });
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/BEinsertSrc",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (e) {
            try {
                var data = JSON.parse(e.d);
                var $ctrl = $(".addSrcSelect");
                data.forEach(function (dt) {
                    if (dt.id == 0)
                        $ctrl.append('<li role="separator" class="divider"></li>');
                    else
                        $ctrl.append($("<li></li>").append($("<a href='#'></a>").html(dt.name).click(function (ev) {
                          
                            insertAtCursor(BEEditor, "((SOURCE: "+ dt.name+"))");
                        })));
                });
               
            }
            catch (ex) { console.warn(ex); }

        },
        error: function (e) { }
    });

}
function intAddSotSelect() {
    $("#addSot").click(function (e) { addSot(e) });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/BEinsertList",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (e) {
            try {
                var data = JSON.parse(e.d);
                var $ctrl = $(".addSotSelect");
                data.forEach(function (dt) {
                    if (dt.id == 0)
                        $ctrl.append('<li role="separator" class="divider"></li>');
                    else
                        $ctrl.append($("<li></li>").append($("<a href='#'></a>").html(dt.title).attr("val", dt.val).click(function (ev) {
                            var $c = $(ev.currentTarget);
                            insertAtCursor(BEEditor, $c.attr("val"));
                        })));
                });
                $ctrl.append('<li role="separator" class="divider"></li>');
                $ctrl.append($('<li ></li>').append($('<input type="text" id="BESynchTemplate" style="height: 1.8em; font-size: 12px; padding-left: 1px; padding-top: 2px;  width: 250px; margin:0 4px" class="form-control caption caption-placeholder " captionid="CapFioRequest" placeholder="наберите ФИО для вставки синхрона" />').click(function (e) {
                    e.stopPropagation();
                })));
                initBESynchTemplate();
                     }
            catch (ex) { console.warn(ex);}

        },
        error: function (e) {  }
    });
   //addSotSelect
}
function BEshowChernovik(e) {
    if ($(".chernovik").length > 0) {
        var $ctrl = $(".chernovik");
        if ($ctrl.is(":visible")) {
            $ctrl.fadeOut(500);
            localStorage.setItem("chernovik", 0);
            $(".BEshowChernovik").removeClass("active");
        }
        else {
            $ctrl.fadeIn(500);
            $(".BEshowChernovik").addClass("active");
            localStorage.setItem("chernovik", 1);
        }
        return;
    }
    $("body").append("<div class='chernovik BETowerLR'></div>");
    $(".chernovik").load("/elements/chernovik.aspx", function () {
        localStorage.setItem("chernovik", 1);
        $(".BEshowChernovik").addClass("active");
        $.get("/API/BlockChernovik/" + blockIdGet(), function (dt) {
            $(".chernovik textarea").val(dt).change(function (e) {
                var $ctrl = $(e.currentTarget);                
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: serverRoot + "testservice.asmx/BlockChernovikSave",
                    data: JSON.stringify({ id: blockIdGet(), txt: RemoveHTMLTag($ctrl.val()) }),
                    dataType: "json",
                    async: true,
                    success: function () {},
                    error: function (e) { console.warn("Error BlockChernovikSave");  }
                });
            }).focus();

        });

        $(".chernovik textarea").bind("paste", function () {
            var $elem = $(this);
            setTimeout(function () {
                var txt = $elem.val();
                $elem.val(RemoveHTMLTag(txt.replace("<br>","\r\n")));
        },100);

        });
    });
}
function chernovikPaste() {
    alert($(".chernovik textarea").val());
}
function changeQuotes(text) {
    var el = document.createElement("DIV");
    el.innerHTML = text;
    for (var i = 0, l = el.childNodes.length; i < l; i++) {
        if (el.childNodes[i].hasChildNodes() && el.childNodes.length > 1) {
            el.childNodes[i].innerHTML = changeQuotes(el.childNodes[i].innerHTML);
        }
        else {
            el.childNodes[i].textContent = el.childNodes[i].textContent.replace(/\x27/g, '\x22').replace(/(\w)\x22(\w)/g, '$1\x27$2').replace(/(^)\x22(\s)/g, '$1»$2').replace(/(^|\s|\()"/g, "$1«").replace(/"(\;|\!|\?|\:|\.|\,|$|\)|\s)/g, "»$1");
        }
    }
    return el.innerHTML;
}