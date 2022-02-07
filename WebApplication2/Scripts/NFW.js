var isPinging = false;
var isNewsClicked = false;
window.onbeforeunload = function (evt) {
  //  debugger;
    //  console.log(typeof(isIsaved));
    if ($("#BlockEditIframe").length > 0) {
        console.warn(document.getElementById("BlockEditIframe").contentWindow.isSaved);
        if (document.getElementById("BlockEditIframe").contentWindow.isSaved==false)
        {
            document.getElementById("BlockEditIframe").contentWindow.SaveBlock(false, true);
            unlookBlock(true);
            window.history.pushState(null, 'BlockViewer', serverRoot+'Index');
            return null;
        }


    }
    window.history.pushState(null, 'BlockViewer', serverRoot+'Index');
    return null;
}
var mainMediaPlayerTimer;
$(window).click(function (e) {
   
    $(".blockInplaceAutorSelect").each(function () { blockInplaceAutorClose(this) });
    $(".blockInplaceCameramanSelect").each(function () { blockInplaceCameramanClose(this) });
    $(".blockInplaceCutterSelect").each(function () { blockInplaceCutterClose(this) });
    $(".BlockNameBlockTimeControl.noEdit").each(function () { BlockNameBlockTimeControlExit($(this)) });
    $(".BlockNameTaskTimeControl.noEdit").each(function () { BlockNameBlockTaskTimeControlExit($(this)) });
})


function ClickNews(id, GroupId) {
    if (isNewsClicked)
        return;
    isNewsClicked = true;
    try {
        if ($("#BlockContainer").attr("NewsId") == id && $("#BlockContainer").attr("GroupId") == GroupId) {
            isNewsClicked = false;
            return;
        }
            

        $("#BlockContainer").attr("NewsId", id);
        $("#BlockContainer").attr("GroupId", GroupId);
        $(".NewsCell").removeClass("newsSelected");
        $("#NewsCell" + id).addClass("newsSelected");
        $(".blockItem").remove();
        $("#NFBheadLoader").addClass("active")
        PingServer(() => {
            $("#NFBheadLoader").removeClass("active")
            isNewsClicked = false;
        },
            () => {
                isNewsClicked = false;
            });

        if ($(document).width() < 950)
            scrollToElement("#BlocksPanel");

        window.history.pushState(null, 'news', serverRoot + 'route/' + $("#BlockContainer").attr("prId") + '/' + $("#BlockContainer").attr("GroupId") + "/" + $("#BlockContainer").attr("NewsId"));
    }
    catch (e) {
        console.warn(e)
        isNewsClicked = false;
    }
        return;
    
}

var ExtendedId= new Array();
var BlockTableRowsCount;// количество заполненных строк в таблице
var blockEditorOffset = 0;
function GetBlocksFromNewsSuccess(data)
{
    
    if (data.d.length > 0) {
        var prm = JSON.parse(data.d);
        if (typeof (prm.NewsBlocks) != 'undefined') {
            $("#bpNewsName").html(prm.NewsName);
            $("#bpNewsOwner").html(langTable['CapOwner'] + ": " + (prm.NewsOwner));
            $("#bpNewsDate").html(langTable['CapData'] + ": " + (prm.NewsDate));
            $("#bpNewsDuration").html(langTable['CapDuration'] + ": " + (prm.NewsDuration));
            $("#bpNewsChrono").html(langTable['CapChrono'] + ": " + (prm.NewsChrono));
            $("#bpNewsChronoPlanned").html(langTable['CapPlanned'] + ": " + (prm.NewsChronoPlanned));
            $("#bpNewsChronoCalculated").html(langTable['CapCalc'] + ": " + (prm.NewsChronoCalculated));

            ExtendedId = [];
            //var datar = JSON.parse(data.d);
            ////////!!!!
          //  ReloadBlockContainer(prm);
            // ReloadBlockData(prm);
            //!!!!!!!!!
            reloadBlockList(prm);
            updateBlockData(prm);

            $(".AddBlockButton").fadeIn(500);
            $("#ApproveAllButton").fadeIn(500);
            $("#BlocksPanelHeaad").fadeIn(500);
         
            if (typeof (lastRoute) != 'undefined' && typeof (lastRoute.blockId) != 'undefined' && typeof (lastRoute.groupId) != 'undefined') {
                EditBlock(lastRoute.blockId);
                delete lastRoute.blockId;
                delete lastRoute.groupId;
            }

        }
    }
    
}// оставляем!!!

function FormatBlockLine(RowData)
{
    var ret = '<div >   ' + GetBlockImage() + " <small>  " + RowData.TypeName + "</small> " + RowData.Name + "</div>";
  
    
    //newcell.innerHTML = ""+GetBlockImage(RowData.Ready, RowData.Approve, RowData.Id) + "<small>" + RowData.TypeName + "</small> " + RowData.Name + "";

    return ret;

}

function OpenBlockScript(BlockId, TemplateId)
{
    setBlockActive(BlockId);
    if (checkDef(TemplateId))
    {
        BlockId = BlockId + "&TemplateId=" + TemplateId;
    }
    window.open(serverRoot+'BlockScript.aspx?BlockId=' + BlockId, '_blank', 'width=' + window.screen.availWidth / 2 + ',height=' + window.screen.availHeight / 2 + ',resizable=1');
}
function ClickBlockCell(CellBlockId, BlockId)
{
   
    var cell = document.getElementById(CellBlockId);

    if (document.getElementById(CellBlockId + "Extrow"))
    {
      
        var parent = document.getElementById(CellBlockId + "Extrow").parentNode;
        parent.removeChild(document.getElementById(CellBlockId + "Extrow"));
        return;
    }
    var Parentrow = cell.parentNode;
    AddExtraRow(Parentrow);
  

}
function AddExtraRow(Parentrow, RowН, OldContent)
{
   
    
    var BlockId = Parentrow.getAttribute("id");
    BlockId = BlockId.replace("BaseRowBlockId", "");
    var ParentTable = Parentrow.parentNode;
 
    var row;
    var newcell;
  
    if ((ParentTable.rows.length > Parentrow.rowIndex+1 ))
    {
        
        if (ParentTable.rows[Parentrow.rowIndex + 1].cells.length==1) //проверяем, эта срока вложенная или нет
        {
            row = ParentTable.rows[Parentrow.rowIndex + 1];
        
            //newcell = row.cells[0];
            var rl = row.cells.length;
            while (rl > 1) {
                row.deleteCell(row.cells.length - 1);
                rl--;
            }
        }
        else { // вставляем строки
            row = ParentTable.insertRow(Parentrow.rowIndex + 1);
            BlockTableRowsCount++;
            newcell = row.insertCell(0);
        }
            
        newcell = row.cells[0];
    }
    else { // вставляем строки
        row = ParentTable.insertRow(Parentrow.rowIndex+1);
        BlockTableRowsCount++;
        newcell = row.insertCell(0);
    }

    //////////
   
    row.setAttribute("id", "CellBlockName" + BlockId + "Extrow");
    //var newcell = row.insertCell(0);
    newcell.colSpan = 6;
    if (!(typeof RowН === "undefined"))
    {
        newcell.style.height = RowН;
    }
    if (!(typeof OldContent === "undefined")) {
        newcell.innerHTML = OldContent;
    
    }
    else {
        newcell.innerHTML = "подождите, загружаю";
    }

    newcell.setAttribute("id", "CellBlockNameEx" + BlockId);
    newcell.setAttribute("Extraid", "CellBlockNameEx" + BlockId);
    

    var jd = { version: 1, serv: 2, msg: 3 };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+"testservice.asmx/GetBlockDataEx",
        data: "{sBlockId:'" + BlockId + "'}",
        dataType: "json",
        success: BlockDataExReceived,
        error: AjaxFailed
    }).getAllResponseHeaders();
}
function BlockDataExReceived(data) //ответ с данными развернутого бока
{
    //debugger;
    // var elem = document.getElementById("CellBlockNameEx" + JSON.parse(data.d).Id);
    elem = $("[Extraid=CellBlockNameEx" + JSON.parse(data.d).Id);
    var ret='\
        <div class="media">\
  <div class="media-left media-middle">\
      <img class="media-object" src=' + serverRoot + '"/Images/screenshot.jpg" width="178" height="100">\
  </div>\
  <div class="media-body">\
    <h4 class="media-heading">Middle aligned media</h4>\
    ' + GetBlockDataExText(data) + '\
  </div>\
</div>';
    elem.innerHTML = ret;
    var ret = '\
<table width="100%" cellpadding="0" cellspacing="0" border="0"><tr id="ExtraRowExtBlock">\
<td width="35"></td>\
<td width="180" valign="top"><img src="' + serverRoot + 'Images/screenshot.jpg" width="178" height="100"></td>\
<td width="35"></td>\
<td width="600" valign="top">' + GetBlockDataExText(data) + '\</td>\
   </tr> </table>\
    ';
    elem = $("[Extraid=CellBlockNameEx" + JSON.parse(data.d).Id).html(ret);
 //   elem.innerHTML = ret;
    return;
}/// delete 
function GetBlockDataExText(data)
{
    var dt = JSON.parse(data.d);
    var ret = "";

    
          ret += "<i>" + langTable['CapAutor'] + ":</i> " + (dt.Creator == null ? "" : dt.Creator) + "  ";
     ret+=   "<i>" + langTable['CapCameramen'] + ":</i> " + (dt.Operator == null ? "" : dt.Operator) + "  " +
        "<i>" + langTable['CapTalent'] + ":</i> " + (dt.Jockey == null ? "" : dt.Jockey) + " <br><br><div style=\"cursor:pointer;\" onclick=\"EditBlock('" + JSON.parse(data.d).Id + "')\">" +
        JSON.parse(data.d).Text + "</div><br>" + CreateBlockExButtons(dt.Id);

    return ret;
}
function CreateBlockExButtons(sBlockId)
{
   //
    var ret = "\
    <div class=\"btn-group btn-group-sm\" role=\"group\" aria-label=\"...\">\
  <button type=\"button\" class=\"btn btn-default\" onclick=\"EditBlock(" + sBlockId + ");\">Изменить</button>\
 <button type=\"button\" class=\"btn btn-default\" onclick=\"OpenBlockScript(" + sBlockId + ");\">Скрипт</button>\
<button type=\"button\" class=\"btn btn-default\" onclick=\"DeleteBlock(" + sBlockId + ", 1, event)\">Удалить</button>\
<button type=\"button\" class=\"btn btn-default\" onclick=\"AddBlocks(" + sBlockId + ")\">Новый блок</button>\
</div>";
    return ret;
}
function blinkErrBlock(sBlockId) {
    var elem = $("#BlockNameRowFirstContainer" + sBlockId)
    var isDanger = $(elem).hasClass("NfwBgDanger");
    var isSucc = $(elem).hasClass("NfwBgSuccess");
    $(elem).removeClass("NfwBgSuccess");


    setTimeout(function () {
        $(elem).addClass("NfwBgDanger");
        setTimeout(function () {
            $(elem).removeClass("NfwBgDanger")
            setTimeout(function () {
                $(elem).addClass("NfwBgDanger");
                if (!isDanger)
                    setTimeout(function () {
                        $(elem).removeClass("NfwBgDanger")
                        if (isSucc)
                            $(elem).addClass("NfwBgSuccess");
                    }, 200)
                else
                    if (isSucc)
                        $(elem).addClass("NfwBgSuccess");
            }, 200)
        }, 200);
    }, 200);
}
function checkBlockEditPermition(sBlockId) {
    if (checkURor([26, 29, 31]) == false) {
        blinkErrBlock(sBlockId);
        return false;
    }
    if (checkURor([26]) == false ) {
        var ch=false;
        if (checkURor([29]) == true) {
        
            var creatorId = $("#blockItem" + sBlockId).attr("CreatorId");       
            var currUserId = $(CurrentUserId).val();
            if ($("#blockItem" + sBlockId).length == 0)
                creatorId = currUserId;
            ch = creatorId == currUserId;
            if ($("#BlockNameRowContainer" + sBlockId).hasClass("blockStausGreen"))
                ch = false;
        }

        if (checkURor([28]) == true) {
            var creatorId = $("#blockItem" + sBlockId).attr("CreatorId");
            var currUserId = $(CurrentUserId).val();
            if ($("#blockItem" + sBlockId).length == 0)
                creatorId = currUserId;
            ch = creatorId == currUserId;
            if ($("#BlockNameRowContainer" + sBlockId).hasClass("blockStausGreen"))
                ch = false;
        }
        if (checkURor([31]) == true) {
            ch = true;
        }
        if (ch == false) {
            blinkErrBlock(sBlockId);
            return false;
        }
       
    }
    return true;
}

var blocKlook = false;
function EditBlock(sBlockId) {
    if (blocKlook)
        return;
    blocKlook = true;
   
    if ($("#BlockEditIframe").length>0)
        return;

    setBlockActive(sBlockId);

    if (checkBlockEditPermition(sBlockId) == false) {
        blocKlook = false;
        return;
    }
   
    if( $("#BlockNameRowFirstContainer" + sBlockId).hasClass("NfwBgDanger"))
    {
        var elem = $("#lockUser" + sBlockId).blink();
        blocKlook = false;
        // alert("Block Is Look for Edit. User: " + $("#BlockNameRowFirstContainer" + sBlockId).attr("LookedUserName"));
        return;
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/initialLocking",
        data: JSON.stringify({ id: sBlockId }),
        dataType: "json",
        async: true,
        success: function (e) {
            if ((e.d) > 0) { EditBlockPreccess(e.d) } else { blinkErrBlock(sBlockId); blocKlook = false;} },
        error: function (e){ console.warn(e)}
    });


}

function EditBlockPreccess(sBlockId){
   blockEditorOffset = 0;
    try {
        blockEditorOffset = document.body.scrollTop > 0 ? document.body.scrollTop : $("#blockItem" + sBlockId).offset().top;
    }
    catch (e) { console.warn(e) }
    var iDiv = CreateFullScreenDiv("FullScreenDiv");
  //  $(iDiv).width($(window).width());
    $(iDiv).html( CreateBlockEditorElements());

    $(".CenralTower").hide();
    $("#RFloatContainer").hide();
    $(".wailt").show();

   
    var isEditor="true"
    if (checkURor([16, 19]) == false)
        isEditor = "false"
  
    $('#FullScreenDiv').append(
        $("<iframe/>")
        .attr("blockId", sBlockId)
        .attr("src", serverRoot+'blocks/BlockEditor.aspx?BlockId=' + sBlockId + "&iseditor=" + isEditor + "&userid=" + $(CurrentUserId).val() + "&rnd=" + parseInt(Math.random() * 10000))
        .attr("id", "BlockEditIframe")
        .attr("height","900")
        .attr("test", "test")
            .load(function (e) { setIframeMaxHeigth(e.currentTarget.id); blocKlook = false; })
        );

    window.scrollTo(0, 0);
  
     window.history.pushState(null, 'news', serverRoot + 'route/' + $("#BlockContainer").attr("prId") + '/' + $("#BlockContainer").attr("GroupId") + "/" + $("#BlockContainer").attr("NewsId") + "/" + sBlockId);
    
}
   
function CreateBlockEditorElements()
{
    var ret;
    return "";
    ret = ' <div  class="panel-heading"> <div class="row">\
        <div id="BlockEditIframe" style="width:300px; margin:0 auto;"></div>\
';

    ret =ret+ "\
<div class=\"btn-group btn-group-sm\" role=\"group\" aria-label=\"...\">\
<button type='button' class='btn btn-success navbar-btn' onclick='EditorSaveAll();'>Сохранить </button>\
<button type='button' class='btn btn-warning navbar-btn' onclick='CloseEditor();'>Закрыть редактор блока </button>\
</div></div>\
";
    return ret;
}
function AddBlockEditorTypes()
{
   return ' <div class="dropdown">\
  <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-expanded="true">\
    ТИП БЛОКА\
    <span class="caret"></span>\
  </button>\
  <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu1">\
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Action</a></li>\
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Another action</a></li>\
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Something else here</a></li>\
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Separated link</a></li>\
  </ul>\
</div>';
}
function EditorSaveAll()
{
    alert("здесь будет сохранение");
}



setTimeout(Ticker, 20000);

function Ticker()
{
    PingServer();
    setTimeout(Ticker, 5000);
}
function PingServer(successClbk, errorClbk) {
    if (isPinging)
        return;

    if (!$("#BlockContainer").attr("NewsId")) {
        setTimeout(PingServer, 1000);
        // console.warn("page not ready, walting 1 second");
        return;
    }
    isPinging = true;
    var Cookie = getCookie("NFWSession");

    var jdata = {
        Cookie: Cookie,
        NewsId: $("#BlockContainer").attr("NewsId"),
        NewsGroupId: $("#BlockContainer").attr("GroupId"),
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/Ping",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: PingSucceeded,
        error: (data) => { if (errorClbk) errorClbk(); PingFailed();}
    });



    function PingSucceeded(data) {
        console.log("PingSucceeded NFWSession=", JSON.parse(data.d).Cookie);
        $(".NFBhead .loader").removeClass("active")
        isPinging = false;
        try {
            var dt = JSON.parse(data.d);
            document.cookie = "NFWSession=" + dt.Cookie;

            $("#exitBtn").attr("title", langTable['ActiveUser'] + ": " + dt.userName);
            $("#hiddenDiv").html(dt.Message);
            GetBlocksFromNewsSuccess({ d: dt.NewsData });

            ReloadInMsg({ d: dt.InMsg });
           
        }
        catch (ex) {
            console.warn(ex.message);
        }
        if (successClbk)
            successClbk();
        /* //console.log("PingSucceeded NFWSession=", JSON.parse(data.d).ActiveUsers);*/
    }
    
}
function PingFailed() {
    console.log("ping failed");
    isPinging = false;
    showWarning(langTable['ErrorNoConnectToServer']);
    $('#DefaultPageFooter').html('<div class="panel panel-default">\
  <div id="ErrorNoConnectToServer" class="panel-danger caption caption-html" captionId="ErrorNoConnectToServer"">' + '</div><script>$("ErrorNoConnectToServer").html(langTable["ErrorNoConnectToServer"])</script></div>');

}
function exitUser(event)
{
    NFconfirm(langTable['AlertExitConfirm'], event.pageX-300, event.pageY, 1, exitUserConfirmad);
}
function exitUserConfirmad(ptm) {
    //if (confirm(langTable['AlertExitConfirm'])) {
    $("#panelWork").html('<img src="' + serverRoot + 'Images/loading.gif"  style="width:50px"/>');
    $(".masterHeadContent").remove();
    $("#exitBtn").remove();

        document.cookie = "NFWSession=0";
        $.ajax({
            type: "POST",
            contentType: "application/json;",
            url: serverRoot+"logout",
            data: JSON.stringify({id:0}),
            dataType: "json",
            async: true,
            success: function () {
              
                window.location.href = serverRoot + "/";

            }
        })
    
  //  }
}
function hideControl(control)
{
    setTimeout(function () {
        $("#" + control.id).hide('blind', {}, 500)
    }, 5000);
}

function showWarning( text) {
    $("#GlobalAlert").toggleClass("alert alert-warning").css("display", "show").text(text).show();
    setTimeout(function () {
        $("#GlobalAlert").hide('blind', {}, 500)
    }, 5000);
}
function ShowAlert(text) {
    $("#GlobalAlert").css("display", "show").text(text).show();
    setTimeout(function () {
        $("#GlobalAlert").hide('blind', {}, 500)
    }, 5000);
}

function deleteBlockConfirmed(id)
{
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        NewsId: id,
        NewsGroupId: 0
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+ "testservice.asmx/DeleteBlock",
        data: JSON.stringify(jdata),
        dataType: "json",
        async: true,
        success: DeleteBlockSucceeded,
        error: AjaxFailed
    });
}
function DeleteBlock(sBlockId, z, event)
{
    UR[$("#ProgramDropDown").val()]
    if (checkBlockEditPermition(sBlockId) == false)
    {
        blinkErrBlock(sBlockId);
        return;
    }
    
    if (checkURor([27, 30]) == false) {
        blinkErrBlock(sBlockId);
        return;
    }
    NFconfirm(langTable['AlertBlockDeleteConfirm'], event.pageX, event.pageY, sBlockId, deleteBlockConfirmed);
}
function DeleteBlockSucceeded(data)
{
    var id = JSON.parse(data.d).id;

    $("#blockItem" + id).addClass("BlockFordelete");
    $("#blockItem" + id).removeClass("blockItem");
    
    $("#blockItem" + id).animate({ opacity: 0 }, 500, function () { $("#blockItem" + id).remove(); });

   // PingServer();
   // ShowAlert(langTable['AlertBlockDelete']);
}


function AddBlocks(AfterBlockId)
{
    if (!checkURor([25, 28])) {
        
        if (!checkDef(AfterBlockId)) {
            $(".AddBlockButton").css("color", "#d9534f");
            setTimeout(function () {
                $(".AddBlockButton").css("color", "");
            }, 500);
        }
        else {
            blinkErrBlock(AfterBlockId);
        }
        return;
    }
    if (!checkDef(AfterBlockId))
        AfterBlockId = 0;
      
   var NewsId = $("#BlockContainer").attr("NewsId");
        
    if(!checkDef(AfterBlockId))
    {
        showWarning(langTable['WarningSelectNews']);
       return 0;
    }

        var Cookie = getCookie("NFWSession");
        var jdata = {
            Cookie: Cookie,
            NewsId: NewsId,
            NewsGroupId: AfterBlockId,
            
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/AddBlock",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: AddBlocksSucceeded,
            error: AjaxFailed
        });
    }
function AddBlocksSucceeded(data)
    {
        if (!(typeof JSON.parse(data.d).NewsId === "undefined"))
            EditBlock(JSON.parse(data.d).NewsId, 0);
        else
        {
            ShowWarning(langTable['WarningErrorAddBlock']);
        }
    }
function AddFileToBlock(BlockId) {

    if (checkURor([26, 29, 31, 57]) == false) {
        blinkErrBlock(BlockId);
        return;
    }

    if (checkBlockEditPermition(BlockId) == false) {
        blinkErrBlock(BlockId);
        return;
    }
    

    var html = '<input type="file" multiple id="FileUploadControl' + BlockId + '"  style="display:none" onchange="FileUploadCahnge(' + BlockId + ', this)"/>'
    $(document.body).append(html);

    var elem = document.getElementById('FileUploadControl' + BlockId); 
    if (elem && document.createEvent) {
        
        var evt = document.createEvent("MouseEvents");
        evt.initEvent("click", true, false);
        elem.dispatchEvent(evt);

    }

  
}
function FileUploadCahnge(BlockId, ctrl) {
           

    for (var i = 0; i < $(ctrl)[0].files.length; i++)
        {
       // AddFileToUpload($(ctrl)[0].files[i], BlockId);

    }
    // $(".NFfileUploadWr").NfFileUpload.addFileList($(ctrl)[0].files, BlockId);
    newFileUpload($(ctrl)[0].files, BlockId);
        $(ctrl).remove();
}
function newFileUpload(files, blockId)
{
    $(".NFfileUploadWr").NfFileUpload.addFileList(files, blockId)
}
///// AJAX запросы
function BlockDown(BlockId) {
    if (checkURor([24]) == false)
        return;
    
    var currArr = new Array();
    var currBg = $("#BlockNameRowContainer" + BlockId).attr("bgcolor");
    if (currBg == 0)
        currArr.push(BlockId);
    else
        $(".BlockNameRowContainer[bgcolor='" + currBg + "']").each(function () { currArr.push($(this).attr("blockId")) });

    var preItem = $("#blockItem" + BlockId).next();
    if (!preItem.hasClass("blockItem"))
        return;
    var preBgcolor = preItem.find(".BlockNameRowContainer").attr("bgcolor");
    if (preBgcolor == 0) {

        $("#blockItem" + currArr[0]).insertAfter(preItem);
        preItem = $("#blockItem" + currArr[0]);
        for (var i = 1; i < currArr.length; i++) {
            $("#blockItem" + currArr[i]).insertAfter(preItem);
            preItem = $("#blockItem" + currArr[i]);
        }

    }
    else {
      //  debugger;
        if (preBgcolor == currBg) {
            while (preBgcolor == currBg) {
                preItem = preItem.next();
                if (preItem == "undefined")
                    preBgcolor == 0;
                else
                    preBgcolor = preItem.find(".BlockNameRowContainer").attr("bgcolor");
            }
            $("#blockItem" + currArr[0]).insertAfter(preItem);
            preItem = $("#blockItem" + currArr[0]);
            for (var i = 1; i < currArr.length; i++) {
                $("#blockItem" + currArr[i]).insertAfter(preItem);
                preItem = $("#blockItem" + currArr[i]);
            }
        }
        else {
            var tmpbg = preBgcolor;
            while (preBgcolor == tmpbg) {
                preItem = preItem.next();
                if (preItem == "undefined")
                    preBgcolor == 0;
                else
                    preBgcolor = preItem.find(".BlockNameRowContainer").attr("bgcolor");
            }
            $("#blockItem" + currArr[0]).insertBefore(preItem);
            preItem = $("#blockItem" + currArr[0]);
            for (var i = 1; i < currArr.length; i++) {
                $("#blockItem" + currArr[i]).insertAfter(preItem);
                preItem = $("#blockItem" + currArr[i]);
            }

        }

    }

    blocksResortSave();
}
function BlockUp(BlockId) {
  
    if (checkURor([24]) == false)
        return;
  
    var currArr = new Array();
        var currBg = $("#BlockNameRowContainer" + BlockId).attr("bgcolor");
        if (currBg == 0)
            currArr.push(BlockId);
        else
            $(".BlockNameRowContainer[bgcolor='" + currBg + "']").each(function () { currArr.push($(this).attr("blockId")) });

        var preItem = $("#blockItem" + BlockId).prev();
        if (!preItem.hasClass("blockItem")) {
 
            
            return;
            
        }
        var preBgcolor = preItem.find(".BlockNameRowContainer").attr("bgcolor");
        if(preBgcolor==0)
        {
            
            $("#blockItem" + currArr[0]).insertBefore(preItem);
            preItem = $("#blockItem" + currArr[0]);
            for (var i = 1; i < currArr.length; i++)
            {
                $("#blockItem" + currArr[i]).insertAfter(preItem);
                preItem = $("#blockItem" + currArr[i]);
            }
            
        }
        else {
            if(preBgcolor==currBg)
            {
                while(preBgcolor==currBg)
                {
                    preItem = preItem.prev();
                    if (preItem == "undefined")
                        preBgcolor == 0;
                    else
                        preBgcolor = preItem.find(".BlockNameRowContainer").attr("bgcolor");
                }
                $("#blockItem" + currArr[0]).insertBefore(preItem);
                preItem = $("#blockItem" + currArr[0]);
                for (var i = 1; i < currArr.length; i++) {
                    $("#blockItem" + currArr[i]).insertAfter(preItem);
                    preItem = $("#blockItem" + currArr[i]);
                }
            }
            else
            {
                var tmpbg = preBgcolor;
                while (preBgcolor == tmpbg) {
                    preItem = preItem.prev();
                    if (preItem == "undefined")
                        preBgcolor == 0;
                    else
                        preBgcolor = preItem.find(".BlockNameRowContainer").attr("bgcolor");
                }
                $("#blockItem" + currArr[0]).insertBefore(preItem);
                preItem = $("#blockItem" + currArr[0]);
                for (var i = 1; i < currArr.length; i++) {
                    $("#blockItem" + currArr[i]).insertAfter(preItem);
                    preItem = $("#blockItem" + currArr[i]);
                }

            }

        }
        blocksResortSave();
    
}
function BlockChangePositionReceived() {
    PingServer();
}
    function AjaxFailed(data) {
        showWarning(langTable['WarningAjaxNotComplite']);
        console.error("AjaxFailed");
    }// общая функция
    function RequestSubBlockData(BlockId) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot+"testservice.asmx/SubBlockData",
            data: "{sBlockId:'" + BlockId + "'}",
            dataType: "json",
            success: SubBlockDataReceived,
            error: AjaxFailed
        });
        
    }
    function  unlookBlock( async)
    {
        $(".NfwBgDanger").removeClass("NfwBgDanger");
        $(".lockUser").html("");
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot+"testservice.asmx/UnlookBlock",
            data: JSON.stringify({ cookie: getCookie("NFWSession") }),
            dataType: "json",
            async: async != true,
            failed: AjaxFailed,
        });
    }
    function CloseBlockEditor(BlockId) {
        window.history.pushState(null, 'news', serverRoot+'route/' + $("#BlockContainer").attr("prId") + '/' + $("#BlockContainer").attr("GroupId") + "/" + $("#BlockContainer").attr("NewsId"));


        if (document.getElementById('FullScreenDiv')) {

            var parent = document.getElementById("FullScreenDiv").parentNode;
            parent.removeChild(document.getElementById("FullScreenDiv"));
            $(".mainPanelNews").show();
            $("#RFloatContainer").show();
            $(".wailt").hide();
            unlookBlock();
           // console.log(blockEditorOffset);
           // document.body.scroll = blockEditorOffset;
            $('html, body').animate({
                scrollTop: blockEditorOffset
            }, 10);
            

            if (CheckDef(BlockId))// отправляем запрос для обновления субблока
            {
                $("#BlockContentUpDown" + BlockId ).find("button").css("background", "red");
                setTimeout(function () { $("BlockContentUpDown" + BlockId).find("button").css("background"); }, 1000);
                RequestSubBlockData(BlockId)
            }
            
            return;
        }

    } /// закрываем окно редактора, разблокируем блок, запрос на обновление сабблока
///// AJAX ответы
    function   SubBlockDataReceived(data)
    {
            ReloadSubBlockData(JSON.parse(data.d));
     }
//////// ЗАГРУЗКА КОНТЕЙНЕРОВ
function reloadBlockList(prm) {
    if ((typeof prm.NewsBlocks == "undefined"))
        return;
    prm.NewsBlocks.forEach(function (itm) {
        if ($('#BlockNameRowContainer' + itm.Id).length ==0)// если контейнер блоков не существует, добавляем
        {
            blockContainerAdd(itm);
            if ($(".blocksExpanded").length > 0) {
                toggleBlockItem(itm.Id);
            }
 
         
        }
    });
}
    function ReloadBlockContainer(data)
    {
        
        if (!(typeof data.NewsBlocks == "undefined")){ // если в данных есть блоки
            data.NewsBlocks.forEach(AddBlockDiv);//добавляем контейнеры блоков
            //return;
            var sortorder = 0;
            $('.BlockNameRowContainer').attr("SortOrder", "");
           
            data.NewsBlocks.forEach(function(NewsBlocks){
            
                $('.BlockNameRowContainer').each(function () {
                   // var id=
                   // log(this.id.indexOf(NewsBlocks.Id));
                    if(this.id.indexOf(NewsBlocks.Id)>=0)
                        $(this).attr("SortOrder", sortorder);
                });
                sortorder ++;
            });

            $('.BlockNameRowContainer').each(function () {

                if ($(this).attr("SortOrder") == "") {
                    $(this).remove();
                }

            });

            SortDivs($(".BlockNameRowContainer").first().parent(), "SortOrder");
          
            DaDInitDragByClass("BlockNameRowContainer");

        }
   
    }
    function AddBlockDiv(RowData)
    {
       
        if ($('#BlockNameRowContainer' + RowData.Id).length < 1)// если контейнер блоков не существует, добавляем
        {
           
            GenerateBlockBlankDivContainer(RowData);
            DaDInitDragByElement($('#BlockContent' + RowData.Id));
          DaDInitDropByElement($('#BlockNameRowFirstContainer' + RowData.Id));
        }

    }
    var ColorTable = [
        "transparent",
        "blue",
        "yellow",
        "black",
        "lightgreen",
        "lightpink",
        "lightseagreen",
        "lightcoral",
        "blueviolet",
        "brown",
        "darkgoldenrod",
        "darkorange",
        "darkturquoise",
        "crimson",
        "orange",
        "green",
        "gray",
        "AliceBlue",
"AntiqueWhite",
"Aqua",
"Aquamarine",
"Azure",
"Beige",
"Bisque",
"Black",
"BlanchedAlmond",
"Blue",
"BlueViolet",
"Brown",
"BurlyWood",
"CadetBlue",
"Chartreuse",
"Chocolate",
"Coral",
"CornflowerBlue",
"Cornsilk",
"Crimson",
"Cyan",
"DarkBlue",
"DarkCyan",
"DarkGoldenRod",
"DarkGray",
"DarkGrey",
"DarkGreen",
"DarkKhaki",
"DarkMagenta",
"DarkOliveGreen",
"DarkOrange",
"DarkOrchid",
"DarkRed",
"DarkSalmon",
"DarkSeaGreen",
"DarkSlateBlue",
"DarkSlateGray",
"DarkSlateGrey",
"DarkTurquoise",
"DarkViolet",
"DeepPink",
"DeepSkyBlue",
"DimGray",
"DimGrey",
"DodgerBlue",
"FireBrick",
"FloralWhite",
"ForestGreen",
"Fuchsia",
"Gainsboro",
"GhostWhite",
"Gold",
"GoldenRod",
"Gray",
"Grey",
"Green",
"GreenYellow",
"HoneyDew",
"HotPink",
"IndianRed ",
"Indigo ",
"Ivory",
"Khaki",
"Lavender",
"LavenderBlush",
"LawnGreen",
"LemonChiffon",
"LightBlue",
"LightCoral",
"LightCyan",
"LightGoldenRodYellow",
"LightGray",
"LightGrey",
"LightGreen",
"LightPink",
"LightSalmon",
"LightSeaGreen",
"LightSkyBlue",
"LightSlateGray",
"LightSlateGrey",
"LightSteelBlue",
"LightYellow",
"Lime",
"LimeGreen",
"Linen",
"Magenta",
"Maroon",
"MediumAquaMarine",
"MediumBlue",
"MediumOrchid",
"MediumPurple",
"MediumSeaGreen",
"MediumSlateBlue",
"MediumSpringGreen",
"MediumTurquoise",
"MediumVioletRed",
"MidnightBlue",
"MintCream",
"MistyRose",
"Moccasin",
"NavajoWhite",
"Navy",
"OldLace",
"Olive",
"OliveDrab",
"Orange",
"OrangeRed",
"Orchid",
"PaleGoldenRod",
"PaleGreen",
"PaleTurquoise",
"PaleVioletRed",
"PapayaWhip",
"PeachPuff",
"Peru",
"Pink",
"Plum",
"PowderBlue",
"Purple",
"RebeccaPurple",
"Red",
"RosyBrown",
"RoyalBlue",
"SaddleBrown",
"Salmon",
"SandyBrown",
"SeaGreen",
"SeaShell",
"Sienna",
"Silver",
"SkyBlue",
"SlateBlue",
"SlateGray",
"SlateGrey",
"Snow",
"SpringGreen",
"SteelBlue",
"Tan",
"Teal",
"Thistle",
"Tomato",
"Turquoise",
"Violet",
"Wheat",
"WhiteSmoke",
"Yellow",
"YellowGreen"
        
       
    ];
   
    function SetBlockRowBgColor(BlockId, e)
    {
       // return;
        //  return;
       // debugger;
        if (checkURor([26, 29, 31]) == false)
            return;
        var CurrClrIndex = $("#BlockNameRowContainer" + BlockId).attr('bgColor');;
        if (doOnClockBlockType == 'normal') {
    

            var ctrl = $("#BlockNameRowContainer" + BlockId); //  $(e.currentarget).closest(".blockItem");
            var next = ctrl.closest(".blockItem").next().find(".BlockNameRowContainer");

            if (CurrClrIndex > 0) {
                ctrl.attr('bgColor', 0);
                ctrl.find(".BlockTypeNameControl")
                       .css('border-bottom', '4px solid TRANSPARENT');
            }
            else {
                if (next != "undefined") {
                    if (next.attr('bgColor') > 0) {
                        ctrl.attr('bgColor', next.attr('bgColor')).find(".BlockTypeNameControl")
                            .css('border-bottom', '4px solid ' + ColorTable[next.attr('bgColor')]);
                    }
                    else {
                        $(".BlockNameRowContainer").each
                        var bgcolor = 0;
                        $(".BlockNameRowContainer").each(function () {

                            var tmp = $(this).attr('bgColor');
                            if (tmp != "undefined") {
                                if (tmp > bgcolor)
                                    bgcolor = tmp;
                            }
                        });
                        bgcolor++;
                        ctrl.attr('bgColor', bgcolor);
                        next.attr('bgColor', bgcolor);

                        ctrl.find(".BlockTypeNameControl")
                        .css('border-bottom', '4px solid ' + ColorTable[bgcolor]);
                        next.find(".BlockTypeNameControl")
                       .css('border-bottom', '4px solid ' + ColorTable[bgcolor]);
                    }
                }
            }
         if (CurrClrIndex > 0) {
                        CurrClrIndex = 0;
                        $("#BlockNameRowContainer" + BlockId).closest(".blockItem").next().find(".BlockNameRowContainer").attr('bgColor', 0)
                    }

            //return;
           // if (CurrClrIndex > 0) {
            //
             //   if (afternext.find(".BlockNameRowContainer").attr('bgColor') == 0) { }
            //}
        }
        else {
      
 
            CurrClrIndex++;
            if (CurrClrIndex !== parseInt(CurrClrIndex, 10))
                CurrClrIndex = 0;
                        if (CurrClrIndex > 8)// if (CurrClrIndex > 16)
                            CurrClrIndex = 0;
                        $("#BlockNameRowContainer" + BlockId).attr('bgColor', CurrClrIndex);
                        $("#BlockTypeNameControl" + BlockId).css('border-bottom', '4px solid ' + ColorTable[CurrClrIndex]);
                      //  $("#BlockNameRowContainer" + BlockId).next().attr('bgColor', CurrClrIndex);
                       // $("#BlockTypeNameControl" + BlockId).next().css('border-bottom', '4px solid ' + ColorTable[CurrClrIndex]);
                      
            
                    
        }
        var arr = new Array();
        $(".BlockNameRowContainer").each(function () {
            arr.push({ id: $(this).attr("blockid"), color: $(this).attr("bgcolor") });
        });
        NFpost(serverRoot + "testservice.asmx/BlockColor", { blocks: arr });
        /*
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot+"testservice.asmx/BlockColor",
            data: JSON.stringify({sBlockId: BlockId , sColorId: CurrClrIndex}),
            dataType: "json",
            
        });*/


    }
    function blockSwipe(elem, direction) {
        if (Math.abs(direction) > window.innerWidth / 4)
        {
            var blockId = $(elem).find(".BlockNameRowContainer").attr("blockid");
            if (direction > 0)
                EditBlock(blockId);
            else
                OpenBlockScript(blockId);
        }
       

        console.warn(direction);
}

function blockContainerAdd(dt) {
    $("#BlockContainer").append(
        $(div)
        .setId('blockItem' + dt.Id)
            .addClass("blockItem")
            .attr("targetId", dt.Id)
            .swipeX(blockSwipe)
        //.append($(div).addClass("blockItemEnable").click(blockItemEnable))
            .append($(div).addClass("blockItemNumber").click(clickBlockItemNumber))
            //.append($("<div class='blockItemEnable'></div>").click(blockItemEnable))
            //.append($(div).addClass("blockItemEnable")
        .append(
            $(div)
            .setId('BlockNameRowContainer' + dt.Id)
            .attr('oncontextmenu', "EditBlock("+ dt.Id + ",0); return false;")
            .addClass('BlockNameRowContainer')
            .attr('blockId', dt.Id)
            .append(
                $(div)
                .setId('BlockNameRowFirstContainer' + dt.Id)
                .addClass("BlockNameRowFirstContainer")
                .append(
                        $(div)
                        .setId('BlockContentUpDown' + dt.Id)
                        .addClass('BlockUpDown')
                        .append($('<div class="btn-group-vertical btn-group-sm" role="group" aria-label="..."></div>')
                          .append($('<button  type="button"  class="btn btn-default btn-xs btnBlockList" ></button >')
                                .append('<span class="glyphicon glyphicon-arrow-up" aria-hidden="true"></span>')
                                .click(function () { BlockUp(dt.Id); setBlockActive(dt.Id); }
                           )
                        )
                           .append($('<button  type="button"  class="btn btn-default btn-xs btnBlockList" ></button >')
                            .append('<span class="glyphicon glyphicon-arrow-down" aria-hidden="true"></span>')
                            .click(function () { BlockDown(dt.Id); setBlockActive(dt.Id) }
                           )
                          )
                        )

                    )
                .append(
                    $(div)
                    .setId('BlockContent' + dt.Id)
                    .addClass('BlockContent')
                    .addClass('FileDroppable')
                    .append(
                        $(div)
                        .setId('BlockTypeNameControl' + dt.Id)
                        .addClass('BlockTypeNameControl')
                        .click(function () { SetBlockRowBgColor(dt.Id); setBlockActive(dt.id) })
                    )
                    .append(
                        $(div)
                        .setId('BlockNameControl' + dt.Id)
                        .addClass('BlockNameControl')
        
                        .attr("onclick", "clickBlockItem("+dt.Id+")" /*ClickBlockNameRowContainer('BlockNameRowFirstContainer' + dt.Id)*/)
                    )
                    .append(
                        $(div)
                        .setId('BlockNameEditControl' + dt.Id)
                        .addClass('BlockNameEditControl')
        .html("<span class='glyphicon glyphicon-asterisk'></span>")
                        .attr("onclick", "clickBlockInplaceEditName(" + dt.Id + ")" /*ClickBlockNameRowContainer('BlockNameRowFirstContainer' + dt.Id)*/)
                    )
                    .append(
                        $(div)
                        .setId('lockUser' + dt.Id)
                        .addClass('lockUser')
                    )
                .append(
                    $(div)
                    .setId('BlockNameMediaControl' + dt.Id)
                    .addClass('BlockNameMediaControl')
                )
                .append(
                        $(div)
                            .setId('BlockNameSrtControl' + dt.Id)
                            .addClass('BlockNameSrtControl')
                    )
                .append(
                    $(div)
                    .setId('BlockNameBlockTimeControl' + dt.Id)
                    .addClass('BlockNameBlockTimeControl')
                    .click(BlockNameBlockTimeControlClick)
                )
        
                .append(
                    $(div)
                    .setId('BlockNameTaskTimeControl' + dt.Id)
                    .addClass('BlockNameTaskTimeControl')
                    .click(BlockNameBlockTaskTimeControlClick)
                )
                .append(
                    $(div)
                    .setId('BlockNameSocialControl' + dt.Id)
                    .addClass('BlockNameSocialControl')
                )
                
                .append(
                    $(div)
                    .addClass('clear')
                )
             )
             .append(
                    $(div)
                    .addClass('clear')
                )
          )
        )
        );
        DaDInitDragByElement($('#BlockContent' + dt.Id));
        DaDInitDropByElement($('#BlockNameRowFirstContainer' + dt.Id));
    }
    function GenerateBlockBlankDivContainer(RowData)/// генерируем основной контейнер блока
    {   
        return '<div   id="BlockNameRowContainer' + RowData.Id + '" oncontextmenu="setBlockActive(' + RowData.Id + '); EditBlock(\'' + RowData.Id + ' \',0); return false;" class="BlockNameRowContainer " BlockId="' + RowData.Id + '"  bgColor="0">\
                        <div  onclick="ClickBlockNameRowContainer(this.id)" class="BlockNameRowFirstContainer NfwBgDanger" id="BlockNameRowFirstContainer' + RowData.Id + '">\
                            <table ip border=0 style="width:100%">\
                                <tr >\
                                    <td width="564" style="width: calc(100% - 100px) ">\
                                        <div id="BlockContent' + RowData.Id + '" class="BlockContent FileDroppable" BlockId="' + RowData.Id + '"><h5>\
                                              <div class="BlockImageControl" id="BlockImageControl' + RowData.Id + '" blockId="' + RowData.Id + '" onclick="SetBlockRowBgColor(\'' + RowData.Id + '\', arguments[0] || window.event);"><img src="" width=16 height=16/></div>\
                                                <small><div class="BlockTypeNameControl" id="BlockTypeNameControl' + RowData.Id + '" ></div></small>\
                                                <div class="BlockNameControl" id="BlockNameControl' + RowData.Id + '"></div>\
                                                <div id="lockUser' + RowData.Id + '" class="lockUser">' + $("#BlockNameRowFirstContainer" + RowData.Id).attr("LookedUserName") + '</div>\
                                          </div></h5>\
                                    <td>\
                                    <td  width="50"><div class="BlockNameBlockTimeControl" id="BlockNameBlockTimeControl' + RowData.Id + '"></div></td>\
                                    <td  width="50"><div class="BlockNameTaskTimeControl" id="BlockNameTaskTimeControl' + RowData.Id + '"></div></td>\
                                </tr>\
                                    \
                            </table>\
                        </div >\
                </div>';
        
    } // здесь форматирование!!!!
    function   clickBlockItem(id)
    {
        setBlockActive(id);
        toggleBlockItem(id);
     
        
}
function toggleBlockItem(id) {
    var a = $("#BlockNameSubRowContainer" + id).length;
    if ($("#BlockNameSubRowContainer" + id).length > 0) {
        if ($("#BlockNameSubRowContainer" + id).find(".noEdit").length > 0 || $(".blocksExpanded").length>0)
            return;
        $("#BlockNameSubRowContainer" + id).remove();
        $("#blockItem" + id).removeClass("expanded");
    }
    else {

        $("#BlockNameRowContainer" + id).append(addSubBlockConteiner(id));
        $('[data-toggle="tooltip"]').tooltip();
        RequestSubBlockData(id);
        HTMLFileDropInitByClass("SubBlockImageControl");
        $("#blockItem" + id).addClass("expanded");
    }
}
    function addSubBlockConteiner(id) {
     
        var ret = $(div)
                   .setId("BlockNameSubRowContainer" + id)
                   .addClass("SubBlockNameRowContainer")
                   .attr("BlockId", id)
                   .append(
                        $(div)
                        .addClass("media")
                        .append(
                            $(div)
                            .addClass("media-left")
                            .append(
                                $(div)
                               .setId("SubBlockImageControl" + id)
                               .addClass("SubBlockImageControl")
                               .attr("blockId", id)
                            )
                            .append(
                            $(div)
                            .addClass("media-body")
                                .html(' <h5 class="media-heading SubBlockAutorRow"><div id="SubBlockAutorControl' + id + '" class="SubBlockAutorControl">' + langTable['CapAutor'] + '</div>\
                                <div id="SubBlockOperatorControl' + id + '" class="SubBlockOperatorControl">' + langTable['CapCameramen'] + '</div>' +
                                '<div id="SubBlockCutterControl' + id + '" class="SubBlockCutterControl">' + langTable['CapCutter'] + '</div>'+
                                    '</h5 >\
                                 <div id="SubBlockTextControl' + id + '" class="SubBlockTextControl" ondblclick="EditBlock(' + id + ')"></div>\
                                  '+ CreateBlockExButtons(id)+'')
                            )
                        )
                    );
        return ret;

                  
    }
   
    function CreateBlockExButtons(sBlockId) {
        //
        var ret = "\
   <div> <div class=\"btn-group btn-group-sm SubBlockButtons\" role=\"group\" aria-label=\"...\">\
  \
<button id='menuBlockBtnInplace" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"OpenBlockInplaceEdit(" + sBlockId + ")\" data-toggle='tooltip' data-placement='top' title='Inplace Editor'><span class='glyphicon glyphicon-asterisk' aria-hidden='true'></span></button>\
    \
    <button id='menuBlockBtnScript" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"OpenBlockScript(" + sBlockId + ")\" data-toggle='tooltip' data-placement='top' title='Скрипт'><span class='glyphicon glyphicon-list-alt' aria-hidden='true'></span></button>\
    <script>$('#menuBlockBtnScript" + sBlockId + "').attr('title', langTable['CapScript'])</script>\
  <button  id='menuBlockBtnEdit" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"EditBlock(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top' title='Изменить'><span class='glyphicon glyphicon-pencil' aria-hidden='true'></span></button>\
<script>$('#menuBlockBtnEdit" + sBlockId + "').attr('title', langTable['CapEdit'])</script>\
<button  id='menuBlockBtnPrint" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"PrintBlock(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top' title='Print'><span class='glyphicon glyphicon-print' aria-hidden='true'></span></button>\
<button  id='menuBlockBtnSaveAs" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"saveBlockText(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top' title='Save Text'><span class='glyphicon glyphicon-superscript' aria-hidden='true'></span></button>\
<button  id='menuBlockBtnAddFile" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"AddFileToBlock(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top' title='Привязать файл'><span class='glyphicon glyphicon-floppy-open' aria-hidden='true'></span></button>\
             <button  id='menuBlockBtnAddSinegy" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"AddSynegyFileToBlock(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top' title='Привязать файл Sinegy'><span class='glyphicon glyphicon-random' aria-hidden='true'></span></button>\
    <script>$('#menuBlockBtnAddFile" + sBlockId + "').attr('title', langTable['CapAddFile'])</script>\
    <button  id='menuBlockBtnAddBlock" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"AddBlocks(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top'><span class='glyphicon glyphicon-sort-by-alphabet' aria-hidden='true'></span></button>\
  <script>$('#menuBlockBtnAddBlock" + sBlockId + "').attr('title', langTable['CapAddBlock']);</script>\
<button  id='menuBlockBtnEDL" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"blockEDL(" + sBlockId + ")\" data-toggle='tooltip' data-placement='top' title='EDL'><span class='glyphicon glyphicon-indent-left' aria-hidden='true'></span></button>\
 \<script>$('#menuBlockBtnEDL" + sBlockId + "').attr('title', 'EDL')</script>\
            <button  id='blockToRss" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"BlockToRss(" + sBlockId + ");event.stopPropagation()\" title=\"to RSS\"><span class='glyphicon glyphicon-phone' aria-hidden='true'></span> </button>\
        <button  id='menuBlockBtnDeleteBlock" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"DeleteBlock(" + sBlockId + ", 0, event)\" data-toggle='tooltip' data-placement='top' title='Удалить'><span class='glyphicon glyphicon-trash' aria-hidden='true'></span></button>\
 \<script>$('#menuBlockBtnDeleteBlock" + sBlockId + "').attr('title', langTable['CapDelete'])</script>\
       </div ></div> ";
        return ret;
    }
    function HilightBlockTime(BlockTime, TaskTime) {
        if (checktimeFromText(BlockTime) > checktimeFromText(TaskTime) && checktimeFromText(TaskTime) > 0)
                return "<p class='text-danger'>" + BlockTime + "</p>";
        
        return "<p class='text-muted'>" + BlockTime + "</p>";
    }// форматирование времени, подсвечиваем при превышении
    function HilightTaskTime(TaskTime) {
        return "<p class='text-muted'>" + TaskTime + "</p>";
    }
    function HilightLockedRow(data) { //delete
        $("#BlockNameRowFirstContainer" + data.Id).removeClass("NfwBgDanger");
        $("#BlockNameRowFirstContainer" + data.Id).removeClass("NfwBgSuccess");
        $("#BlockNameRowFirstContainer" + data.Id).attr("LookedUserName", null);
        // <div id="lockUser' + RowData.Id + '" class="lockUser"></div>\
    
        if (data.LookedUserName.length > 1) {
            $("#BlockNameRowFirstContainer" + data.Id).addClass("NfwBgDanger");
            $("#BlockNameRowFirstContainer" + data.Id).attr("LookedUserName", data.LookedUserName);
            $("#lockUser" + data.Id).html(langTable['LockedBy'] + data.LookedUserName);
        //    console.log("#lockUser" + data.Id + data.LookedUserName);
            return;
        }
        $("#lockUser" + data.Id).html("");
        if (data.CreatorId == CurrentUserId.value) 
            $("#BlockNameRowFirstContainer" + data.Id).addClass("NfwBgSuccess");
 
    }// // delete подсвечиваем фон   заблокированность пользователем
////////ЗАГРУЗКА ДАННЫХ
    function updateBlockData(dt) {
      //  console.log(dt);
        $(".blockItem").addClass("deleted");
        if (!(typeof dt.NewsBlocks == "undefined")) {
         
            var sortOrder = 0;
            dt.NewsBlocks.forEach(function (itm) {
               

                $("#blockItem" + itm.Id).removeClass("deleted");
                $("#blockItem" + itm.Id).attr("sortOrder", sortOrder);
                $("#blockItem" + itm.Id).attr("CreatorId", itm.CreatorId);

                sortOrder++;
                var lcl = $("#blockItem" + itm.Id).find(".blockItemNumber");
                if(! lcl.hasClass("BlockItemNumberActive"))
                    lcl.html(sortOrder);

                var BgColorIndex = 0;
                if (itm.BgColorIndex != "")
                    BgColorIndex = itm.BgColorIndex;
                $("#BlockNameRowContainer" + itm.Id).attr("bgcolor", BgColorIndex);
                $("#BlockTypeNameControl" + itm.Id).css('border-bottom', '4px solid ' + ColorTable[BgColorIndex] );
                $("#BlockTypeNameControl" + itm.Id).html(itm.TypeName);
                if (!$("#BlockNameControl" + itm.Id).hasClass("noEdit"))
                    $("#BlockNameControl" + itm.Id).html(itm.Name + "<span class='rowAuthor'>" + (itm.UserName ? itm.UserName:"")+"</span>");
                if (!$("#BlockNameBlockTimeControl" + itm.Id).hasClass("noEdit"))
                    $("#BlockNameBlockTimeControl" + itm.Id).html(HilightBlockTime(itm.BlockTime, itm.TaskTime));
                if (!$("#BlockNameTaskTimeControl" + itm.Id).hasClass("noEdit"))
                     $("#BlockNameTaskTimeControl" + itm.Id).html(HilightTaskTime(itm.TaskTime));
                $("#BlockNameMediaControl" + itm.Id).addClass("BlockNameMediaControlState" + itm.mediaSatus);

                if (itm.srtStatus>0)
                    $("#BlockNameSrtControl" + itm.Id).addClass("BlockNameSrtControlState");
                else
                    $("#BlockNameSrtControl" + itm.Id).removeClass("BlockNameSrtControlState");

           
                $("#BlockNameRowContainer" + itm.Id).removeClass("blockStausRed");
                $("#BlockNameRowContainer" + itm.Id).removeClass("blockStausYellow");
                $("#BlockNameRowContainer" + itm.Id).removeClass("blockStausGreen");
                $("#BlockNameRowContainer" + itm.Id).addClass(getBlockStatusClass(itm));

                $("#BlockNameRowFirstContainer" + itm.Id)
                    .removeClass("NfwBgDanger")
                    .removeClass("NfwBgSuccess")
                    .attr("LookedUserName", null);
               
                if (itm.LookedUserName.length > 1) {
                    $("#BlockNameRowFirstContainer" + itm.Id)
                        .addClass("NfwBgDanger")
                        .attr("LookedUserName", itm.LookedUserName);
                    $("#lockUser" + itm.Id).html(langTable['LockedBy'] + itm.LookedUserName);

                }
                else {
                    $("#lockUser" + itm.Id).html("");
                    if (itm.CreatorId == CurrentUserId.value)
                        $("#BlockNameRowFirstContainer" + itm.Id).addClass("NfwBgSuccess");
                }
                //////////
                if ($("#SubBlockTextControl" + itm.Id).length > 0) {
                    RequestSubBlockData(itm.Id);
                }

            });
        }
        $(".deleted").remove();
        SortDivs($("#BlockContainer"), "sortOrder");
    }
    function ReloadBlockData(data) {
        
        if (!(typeof data.NewsBlocks == "undefined")) { // если в данных есть блоки

       
            data.NewsBlocks.forEach(function (RowData) {
          
                var BgColorIndex = 0;
                if (RowData.BgColorIndex != "")
                    BgColorIndex = RowData.BgColorIndex;
                $("#BlockNameRowContainer" + RowData.Id).attr("bgcolor", BgColorIndex);
   
                // $("#BlockImageControl" + RowData.Id).css('borderColor', ColorTable[BgColorIndex]);
                $("#BlockImageControl" + RowData.Id).css('boxShadow', ColorTable[BgColorIndex] + " 0 0 2em");
                //console.log(RowData);
                $("#BlockTypeNameControl" + RowData.Id).html(RowData.TypeName);
                $("#BlockNameControl" + RowData.Id).html(RowData.Name);
              /*  console.log(RowData.BlockTime);
                if (RowData.BlockTime == "00:00:00")
                    RowData.BlockTime = RowData.CalcTime;
                console.log(RowData.BlockTime);*/
                $("#BlockNameBlockTimeControl" + RowData.Id).html(HilightBlockTime(RowData.BlockTime, RowData.TaskTime));
                $("#BlockNameTaskTimeControl" + RowData.Id).html(HilightTaskTime(RowData.TaskTime));
                $("#BlockImageControl" + RowData.Id).html(ReloadBlockImage(RowData));
                if ($("#SubBlockTextControl" + RowData.Id).length > 0)
                {
                    
                    RequestSubBlockData(RowData.Id);
                }
                    //
                HilightLockedRow(RowData);


            });
        }
    }
    function getBlockStatusClass(data){
        var image = "blockStausRed";
        if (data.Ready == true)
            image = "blockStausYellow";
        if (data.Approve == true)
            image = "blockStausYellow";
        if (data.Approve == true && data.Ready == true)
            image = "blockStausGreen";
        return image;
    }
    
    function ReloadSubBlockDataReplacer(str, strp1) {
       
      //  console.log("ReloadSubBlockDataReplacer" + strp1);
       // var ret = strp1.replace(/\)\)(.+)/, function (s, p1) {/* console.log("ReloadSubBlockDataReplacer1" + p1);*/  return p1; });
       
        return "";
}

    function updateSubBlockImageControl(id)
    {
       // $("#SubBlockImageControl" + id).html('<div style="cursor:pointer" onclick="ShowMainVideo(' + id + ')"><img id="BlockImageControl' + id + '" class="media-object blockListImage"  src="' + serverRoot + 'handlers/GetBlockImage.ashx?BlockId=' + id + '&Rnd=' + parseInt(Math.random() * 10000) + '"/></div><div class="SubBlockImageControlStatus"></div>');

        $("#SubBlockImageControl" + id).load(serverRoot + "elements/SubBlockImageControl.aspx?blockId="+id);
    }
    function ReloadSubBlockData(data)
    {
        //console.log(data.Cutter);
        $("#SubBlockOperatorControl" + data.Id).html("<small>" + langTable['CapCameramen'] + ": </small><span class='CapCameramen' onclick='CapCameramenClick(this)' >" + (data.Operator == null ? ' -- ' : data.Operator) + "</span>");
        $("#SubBlockCutterControl" + data.Id).html("<small>" + langTable['CapCutter'] + ": </small><span class='CapCutter' onclick='CapCutterClick(this)' >" + (data.Cutter == null ? ' -- ' : data.Cutter) + "</span>");
        if ($("#blockItem" + data.Id).find(".blockInplaceAutorSelect").length == 0)
        $("#SubBlockAutorControl" + data.Id).html("<small>" + langTable['CapAutor'] + ": </small><span class='CapAutor' onclick='CapAutorClick(this, event)'>" + (data.Creator == null ? ' -- ' : data.Creator) + "</span>");

        var text = data.Text.replace(/\(\(NF[^\)]+\)\)/g, ReloadSubBlockDataReplacer);
        text = text.replace(/\n/g, "<span class='break'></span>\n");
        text = text.replace(/NF::BOLDSTART/g, "<b style='background: yellow;'>");
        text = text.replace(/NF::BOLDEND/g, "</b>");
        text = text.replace(/\((ПОДВОДКА:[^\)]+)\)/gmi, (a, b) => {return "<span style='color:green'>"+b+"</span>" });
        text = text.replace(/\(\([^\)]+\)\)/g, function (a) {
            var cls = "";
            a.replace(/^\(\(([A-Z]{2,5})\:*\s/, function (b, mark) {
                cls = (mark);
               
               // b=b.replace("TITLE", "RRRRR");
                return b;
            });
         
            a=a.replace("TITLE:", "<span class='subBlockMark'>TITLE:</span>")
            a = a.replace("NAME:", "<span class='subBlockMark'>NAME:</span>")
            a = a.replace("TEXT:", "<span class='subBlockMark'>TEXT:</span>")
            a = a.replace("SOT:", "<span class='subBlockMark'>SOT:</span>")

            return "<span class='colorText " + cls + "'>" + a + "</span>";
        });

        //console.log(text);
        if (!$("#SubBlockTextControl" + data.Id).hasClass("noEdit")) {
            $("#SubBlockTextControl" + data.Id).html('<small>' + text + '</small>');
            
        }
      //  console.log("ReloadSubBlockData");
        //$("#SubBlockImageControl" + data.Id).html('<img title="dd" id="' + data.Id + '" class="media-object" BlockId="' + data.Id + '" src="/Images/screenshot.jpg" width="178" height="100">');
        if ($("#SubBlockImageControl" + data.Id +" div").length==0 )
            updateSubBlockImageControl(data.Id)
            // log('../handlers/GetBlockImage.ashx?BlockId="' + data.Id  );
    } ///заглушка картинки !!!!
    function initBloks() {
        try {
            $("#bpNewsName").html('<h4><div id="CapToStart" class="alert alert-success caption caption-html" captionId="CapToStart" role="alert"></div><script>$("#CapToStart").html(langTable["CapToStart"])</script><h4>');
            document.getElementById("bpNewsOwner").innerHTML = "&nbsp;";
            document.getElementById("bpNewsDate").innerHTML = "&nbsp;";
            document.getElementById("bpNewsDuration").innerHTML = "&nbsp; ";
            document.getElementById("bpNewsChrono").innerHTML = "&nbsp; ";
            document.getElementById("bpNewsChronoPlanned").innerHTML = "&nbsp;";
            document.getElementById("bpNewsChronoCalculated").innerHTML = "&nbsp;";

            $("#BlockContainer").attr("NewsId", 0);
            $("#BlockContainer").attr("GroupId", 0);

            $(".AddBlockButton").hide();
            $("#BlocksPanelHeaad").hide();
            PingServer();

        }
        catch (ex) {

        }
    } // загружаем первоначальные данные в хедеh
    function checkUR(id) {

       /* var tmp = UR[$("#ProgramDropDown").val()];
        console.log(tmp);
        console.log(id);
        console.log(tmp[id]);*/

        if (typeof (UR[$("#ProgramDropDown").val()]) == 'undefined')
            return false;
        if (typeof (UR[$("#ProgramDropDown").val()][id]) == 'undefined')
            return false;

        
        return true;
    }
    function checkURand(e) {
        var ret = true;
        e.forEach(function (elem) {
            ret = ret && checkUR(elem);
        });
        return ret;
    }
    function checkURor(e) {
        var ret = false;
        e.forEach(function (elem) {
            
            ret = ret || checkUR(elem);

         
        });
  
        return ret;
    }
    function initMainVideoMediaBox(blockId, mediaId, archive)
    {
        MainVideoMediaBoxLoad(blockId, mediaId, archive);
        $(window).bind("keydown", mainVideoKeyDown);
        $(".mainMediaBox").on("remove", function () {
            $(window).unbind("keydown", mainVideoKeyDown);
        })

    }
    function mainVideoKeyDown(e) {
        mainVideoPlayerButtonControls(e);
    }
    function MainVideoMediaBoxLoad(blockId, mediaId, archive)
    {
        $("#panelPlayer").click(function (e) { e.stopPropagation() })
        var cookie = document.cookie.replace("NFWSession=", "")
        var jdata = {
            Cookie: cookie,
            NewsId: blockId,
            NewsGroupId: 0,
            Archive:archive
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/GetMediaListFull",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function (data) { mediaListReceived(data, mediaId, archive) },
            error: function (e) { console.warn("ERROR GetMediaListFull"); console.warn(e); }
        });
        return 1;
    }
    function mediaListReceived(data, mediaId, archive)
    {
       
        var dt = (JSON.parse(data.d));
        var activeItem = "";
        if ($(".mainVideoMediaItemActive").length >0) {
            activeItem= $(".mainVideoMediaItemActive").attr("mediaId");
        }

        $(".mainVideoMediaWr").html("").append($(div).addClass("mainVideoMediaBox"));
   
    //    $(".mainVideoMediaBox").html("");
        dt.Media.forEach(function (item) {
            $(".mainVideoMediaBox").append($(div)
                .addClass("mainVideoMediaItem stopPropagation")
                .attr("mediaId", item.Id)
                .attr("approve", item.Approve)
                .attr("archive", archive)
                .attr("ready", item.Ready)
                .attr("typeId", item.BLockType)
                .append($("<input type='text'/>").addClass("mainVideoMediaItemTitle").val(item.Name).change(mainVideoMediaItemTitleChange))
                .append($("<img></img>").attr("src", item.BLockType > 0 ? (serverRoot + "handlers/Get" + (archive?"Archive":"") + "BlockImage.ashx?mediaId=" + item.Id + "&rnd=" + Math.random(1000)) : (serverRoot + "images/document.svg")))
                .append($("<img class='videoImageIcon'></img>").attr("src", serverRoot + "images/video-play-button.svg"))
                .append($("<img class='pictureImageIcon'></img>").attr("src", serverRoot + "images/picture.svg"))
                );
        });

        $(".mainVideoMediaBox img").resize(function () { $(this).css("max-height", ($(this).width() / 16) * 9) });
        $(".mainVideoMediaBox img").load(function () { $(this).css("max-height", ($(this).width() / 16) * 9) });
       // setTimeout(function(){ $(".mainVideoMediaBox img").each(function () { $(this).css("max-height", ($(this).width() / 16) * 9) })}, 1000);

        slick($(".mainVideoMediaBox"));
     

        $(".mainVideoMediaItem").click(function (e) {
            var ctrl = $(e.target).closest(".mainVideoMediaItem");
           

            $(".mainVideoMediaItemActive").removeClass("mainVideoMediaItemActive");
            $(".mainVideoPlayerPlayer").removeClass("fullWidtPlayer");
            $(ctrl).addClass("mainVideoMediaItemActive");
            $(".mainMediaBox").attr("mediaId", $(ctrl).attr("mediaId")).attr("typeId", $(ctrl).attr("typeId"));
            $("#panelPlayerWr").loading50().load(serverRoot + "elements/mainVideoPlayerElement.aspx?mediaId=" + $(ctrl).attr("mediaId") + "&blockType=" + $(ctrl).attr("typeId") + "&archive=" + $(ctrl).attr("archive"),
                function (e) {
                    $("#panelPlayerWr").click(function (e) { e.stopPropagation(); })
                    $("#subTitleEditorLimit").val(localStorage.getItem("subTitleEditorLimit") || 90);
                    $("#subTitleEditorLimit").change(function () {
                        localStorage.setItem("subTitleEditorLimit", $("#subTitleEditorLimit").val())
                    })
                    $("#subTitleEditorLimit").click(function (e) { e.stopPropagation(); })
                    $("#subTitleEditorCount").click(function (e) { e.stopPropagation(); e.preventDefault(); });
                    $(".subTitleEditorCurrText").on("input", function () {
                        console.log("input")
                        var $ctrl = $(".subTitleEditorCurrText");
                        $("#subTitleEditorCount").val($ctrl.val().length);

                        if ($ctrl.val().length > parseInt($("#subTitleEditorLimit").val()))
                            $ctrl.addClass("overload")
                        else
                            $ctrl.removeClass("overload")

                    })
                }
            )
            subTitleEditorClose();
        });

        // $(".mainVideoMediaItem[mediaid='" + mediaId + "']").first().addClass("mainVideoMediaItemActive");
        // $(".mainMediaBox").attr("mediaId", mediaId);
      //  if ($(".mainVideoMediaItemActive").length == 0) {
       //     $(".mainVideoMediaItem").first().click();
       // }
        if (activeItem != "") {
            var elem = $(".mainVideoMediaItem[mediaId='" + activeItem + "']");
            if($(elem).length>0)
            {
                $(elem).addClass("mainVideoMediaItemActive");
            }
            else $(".mainVideoMediaItem").first().click();
                
        }
        else
            $(".mainVideoMediaItem").first().click();

        $(".mainVideoMediaBox ")
            .click(function (e) { e.stopPropagation() })
            
        $(".slick-arrow").click(function (e) { e.stopPropagation() })
    
    }
   
    function mainVideoMediaItemTitleChange(e) {
       
        var jdata = {
            Cookie: 0,

        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/MediaAction?Action=Rename&Name=" + encodeURI($(e.target).val()) + "&MediaId=" + $(e.target).closest(".mainVideoMediaItem").attr("mediaId"),
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function () { },
            error: function (e) { console.warn("ERROR MediaAction?Action=Rename"); console.warn(e); }
        });
    }
    function mainVideoMediaItemStatusChange(MediaId) {

        //MediaBlockEditApproveDropDown
        var ready = $("#MediaBlockEditReadyDropDown").prop("checked");
        var approve = $("#MediaBlockEditApproveDropDown").prop("checked");

        if (!ready && approve) {
            $("#MediaBlockEditApproveDropDown" ).prop("checked", false);
            $("#MediaBlockEditReadyDropDown" ).prop("checked", false);
        }
        else
            if (approve && (!ready)) {
                $("#MediaBlockEditReadyDropDown").prop("checked", true);
                $("#MediaBlockEditApproveDropDown").prop("checked", true);
            }
        /// еще раз после изменения статуса
        ready = $("#MediaBlockEditReadyDropDown").prop("checked");
        approve = $("#MediaBlockEditApproveDropDown").prop("checked");

        
        var cookie = document.cookie.replace("NFWSession=", "")
        var jdata = {
            Cookie: cookie,

        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/MediaAction?Action=ChangeStatus&Ready=" + ready + "&Approve=" + approve + "&MediaId=" + MediaId,
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function () { },
            error: function (e) { console.warn(e);}
        });
        return 1;

    }
    function mediaDelete(MediaId, event) {
        NFconfirm(langTable['AlertMediaDeleteConfirm'], event.pageX-250, event.pageY, MediaId, mediaDeleteComplited);
    }
    function mediaDeleteComplited(MediaId) {
        var cookie = document.cookie.replace("NFWSession=", "")
        var jdata = {
            Cookie: cookie,
        };

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/MediaAction?Action=DeleteMedia&MediaId=" + MediaId,
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function (data) {
                $("#BEMediaItem" + MediaId).fadeOut(500, function () {
                    $("#BEMediaItem" + MediaId).remove();
                });
                //  MediaActionSuccess(data);
            },
            error: function (e) { console.warn(e) }
        });



        return 1;
    }
    function slick(elem)
    {
        $().ready(function () {
            $(elem).slick({
                dots: true,
                infinite: false,
                speed: 300,
                slidesToShow: 4,
                slidesToScroll: 4,
                autoplay: false,
                autoplaySpeed: 300,
                responsive: [
                  {
                      breakpoint: 1024,
                      settings: {
                          slidesToShow: 3,
                          slidesToScroll: 3,
                          infinite: true,
                          dots: true
                      }
                  },
                  {
                      breakpoint: 600,
                      settings: {
                          slidesToShow: 2,
                          slidesToScroll: 2
                      }
                  },
                  {
                      breakpoint: 480,
                      settings: {
                          slidesToShow: 1,
                          slidesToScroll: 1
                      }
                  }
                  // You can unslick at a given breakpoint now by adding:
                  // settings: "unslick"
                  // instead of a settings object
                ]
            });
        });
    }
   
    function initPlayerTimer() {
        onMainMediaPlayerTimer();
        $("#panelPlayer").on("remove", function () { if (typeof (mainMediaPlayerTimer)!= 'undefined') clearTimeout(mainMediaPlayerTimer); })
    }
    function onMainMediaPlayerTimer() {
        /*обработчик событий плейера
        window['mainVideoTitleArray']
        
        
        */
        try{
            var player = $("#panelPlayer").find('video').get(0);
            currentTime = parseInt(player.currentTime);
            $(".subTitleEditorItemSelected").removeClass("subTitleEditorItemSelected");
            if (typeof (window['mainVideoTitleArray']) != "undefined") {           
                window['mainVideoTitleArray'].forEach(function (layer) {
                    var find = false;
                   
                   
                   
                   
                    layer.items = layer.items.filter(function (e) { return e.timeInSec >= currentTime });
                   
                   // layer.items = layer.items.sort(function (a, b) { return parseInt(a) - parseInt(b) });
                   // console.log("find item", layer.items, layer.items.length)
                    if (layer.items.length > 0) {
                        $(".mainVideoPlSTitem" + layer.id).html(layer.items[layer.items.length-1].text);
                        $("#" + layer.items[layer.items.length-1].id).addClass("subTitleEditorItemSelected");
                    }

                    /*layer.items.forEach(function (item) {
                        if (parseInt(item.timeInSec) <= currentTime && currentTime < item.endtimeInSec) {

                            console.log("find item", item.timeInSec, item.endtimeInSec, currentTime  )
                            $(".mainVideoPlSTitem" + layer.id).html(item.text);
                            $("#" + item.id).addClass("subTitleEditorItemSelected");
                            find=true
                        }
                        if(!find)
                            $(".mainVideoPlSTitem" + layer.id).html("");
                    });*/
                })
            }
        }
        catch (e) {
            console.warn(e)
        }
        mainMediaPlayerTimer = setTimeout(onMainMediaPlayerTimer, 200);
    }


    function mainVideoPlayerButtonControls(e) {
        
        if ($("#panelPlayer").find('video').length > 0) {
            var player = $("#panelPlayer").find('video').get(0);
            if (e.keyCode == 13 && e.shiftKey == true) {
               
                $(".subTitleEditorCurrTC ").html(msToTime(player.currentTime * 1000));
                e.preventDefault();
                e.stopPropagation();
            }
            if (e.keyCode == 13 && e.shiftKey == false) {
                e.preventDefault();
                $(e.target).change();
                $(".subTitleEditorCurrTC ").html(msToTime(player.currentTime * 1000));
                player.pause();
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            if (e.keyCode == 32 && e.shiftKey == true)// space
            {
                e.preventDefault();
                if (player.paused) {
                    player.play();
                }
                else
                    player.pause();

                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            if (e.keyCode == 37 && e.shiftKey == true) {
                player.currentTime -= 1;
                e.preventDefault();
                e.stopPropagation();
            }//left
            if (e.keyCode == 39 && e.shiftKey == true) {
                player.currentTime += 1;
                e.preventDefault();
                e.stopPropagation();
            }//ri
            if (e.keyCode == 38 && e.shiftKey == true) {
                player.currentTime = 0;
                e.preventDefault();
                e.stopPropagation();
              
            }//UP
            if (e.keyCode == 40 && e.shiftKey == true) {
                e.preventDefault();
                e.stopPropagation();
            }//DOWN
        }
    }
    function mainVideoSubTitlesLoad(mediaId) {
        serv("mainVideoSubTitlesLoad", function (data) {
            window['mainVideoTitleArray'] = data;
           
            $(".mainVideoPlSTitem").remove();

            window['mainVideoTitleArray'].forEach(function (itm) {
                $(".mainVideoPlS").append(
                    $(div)
                    .attr("name", itm.title)
                    .addClass("mainVideoPlSTitem stopPropagation")
                    .addClass("mainVideoPlSTitem" + itm.id)
                    .attr("layerId", itm.id)
                    );

            });
            console.log("mainVideoSubTitlesLoad");
           // $(".subTitleEditorBody").animate({ scrollTop: $(".subTitleEditorBody")[0].scrollHeight }, "fast");
            $(".subTitleEditorBody")[0].scrollTop = $(".subTitleEditorBody")[0].scrollHeight;
        }, { mediaId: mediaId })
    }

    function medialrvStateCreate(mediaId) {
        return '<div class="mediaLRVSC' + mediaId + ' medialrvStateContainer">\
            LRV: <span class="mediaLRVSCtext' + mediaId + '">55%</span>\
               \
                <div class="mediaLRVSCprogress' + mediaId + ' BElrvProgressContainer BElrvProgressPosition"></div>\
                <div class="mediaLRVSCerror' + mediaId + ' BElrvErrorContainer BElrvProgressPosition"></div>\
                <div class="mediaLRVSCsucess' + mediaId + ' BElrvSuccessContainer BElrvProgressPosition"></div>\
            </div>';
    }
    function medialrvStatecheck(mediaId) {
        if ($(".mediaLRVSC" + mediaId).length < 1) {
            $(".mainVideoMediaItem[mediaId='" + mediaId + "']").append(medialrvStateCreate(mediaId));
        }
    }
    function medialrvStateChange(mediaId, val) {

        medialrvStatecheck(mediaId);
        $(".mediaLRVSCprogress" + mediaId).show();
        $(".mediaLRVSCsucess" + mediaId).hide();
        $(".mediaLRVSCerror" + mediaId).hide();

        if (typeof (val) == 'undefined' || val == '')
            val = 'started';
        $(".mediaLRVSCtext" + mediaId).html(val);
        if (val.indexOf("%") > 0) {
            $(".mediaLRVSCprogress" + mediaId).css("width", "calc(" + val + " - 10px)");
            $(".mediaLRVSCprogress" + mediaId).show();
        }
        else {
            $(".mediaLRVSCprogress" + mediaId).hide();
        }
    }
    function medialrvStateErr(mediaId) {
        medialrvStatecheck(mediaId);
        $(".mediaLRVSCprogress" + mediaId).hide();
        $(".mediaLRVSCsucess" + mediaId).hide();
        $(".mediaLRVSCerror" + mediaId).show();

        $(".mediaLRVSCtext" + mediaId).html("error");
    }
    function medialrvStateSucc(mediaId) {
        medialrvStatecheck(mediaId);
        $(".mediaLRVSCprogress" + mediaId).hide();
        $(".mediaLRVSCerror" + mediaId).hide();
        $(".mediaLRVSCsucess" + mediaId).show();

        $(".mediaLRVSCtext" + mediaId).html("OK");

        setTimeout(function () {
            $(".mediaLRVSC" + mediaId).fadeOut(500, function () {
                $(".mediaLRVSC" + mediaId).remove();
            });
        }, 5000)
    }
   
    
    
    function clickBlockInplaceEditName(blockId) {

        if (checkBlockEditPermition(blockId) == false)
            return;
        setBlockActive(blockId);
        var ctrl = $("#BlockNameControl" + blockId);
        if (ctrl.hasClass("noEdit"))
            return;
        ctrl.addClass("noEdit");
        var text = ctrl.html();
        text = text.replace(/<span.+/, '');
        $(ctrl).html($("<input type='text' class='inplaceBlockTitleEditor' blockId='" + blockId + "' maxlength='200'></input>").val(text).blur(inplaceBlockTitleEditorClose).keydown(inplaceBlockTitleEditorKeyDown));
        $(ctrl).children().focus().select();
    }
    function inplaceBlockTitleEditorClose(e) {
  
        var ctrl = $(e.target);
        var blockId = ctrl.attr("blockId");
        var text = RemoveHTMLTag(ctrl.val());

        if (text.length == 0) {
            text = "__";
            $(ctrl).parent().removeClass("noEdit").html(text);
            return;
        }

        $(ctrl).parent().removeClass("noEdit").html(text);
       
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: serverRoot+"testservice.asmx/updateBlockTitle",
                data: JSON.stringify( { id:blockId, title:text}),
                dataType: "json",
                success: function(e){},
                error: function(e){console.warn("ERROR inplaceBlockTitleEditorClose");console.warn(e)}
            })
        
        return true;
    }
    function inplaceBlockTitleEditorKeyDown(e){
        if(e.keyCode==13)
            inplaceBlockTitleEditorClose(e)
    }
    var inplaceIntervalLooker;
    function OpenBlockInplaceEdit(blockId) {

        if (checkBlockEditPermition(blockId) == false)
            return;
        
       
       
        if ($("#BlockNameRowFirstContainer" + blockId).hasClass("NfwBgDanger")) {
            var elem = $("#lockUser" + blockId).blink();
            // alert("Block Is Look for Edit. User: " + $("#BlockNameRowFirstContainer" + sBlockId).attr("LookedUserName"));
            return;
        }
        setBlockActive(blockId);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/initialLocking",
            data: JSON.stringify({ id: blockId }),
            dataType: "json",
            async: true,
            success: function (e) { if ((e.d) > 0) { OpenBlockInplaceEditProcess(e.d) } else blinkErrBlock(blockId); },
            error: function (e){ console.warn(e)}
        });
    }
    function OpenBlockInplaceEditProcess(blockId)
    {
        var ctrl = $("#SubBlockTextControl" + blockId);
        if (ctrl.hasClass("noEdit"))
            return;
        $(".SubBlockTextControl.noEdit").each(function () {
            var id = $(this).attr("id").replace("SubBlockTextControl", "");
            inplaceBlockEditorClose(id);
        });


        ctrl.addClass("noEdit");
        var text = ctrl.html();
        $(ctrl).html($("<textarea class='inplaceBlockEditor' blockId='" + blockId + "'></textarea>")).append($(div).addClass("loading").loading50());//.blur(inplaceBlockEditorClose));

        var subCtrl = $(ctrl).find("textarea").hide();

        $(ctrl).append($(div).addClass("inplaceBlockEditorMenu").load(serverRoot + "blocks/blockInplaceEditorMenu.aspx?blockId=" + blockId));

        
        
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/getBlockText",
            data: JSON.stringify({ id: blockId }),
            dataType: "json",
            success: function (e) {
            
                var dt=JSON.parse(e.d);
                $(subCtrl).val(dt.text).attr("readrate", dt.readrate).show().focus();
                $(subCtrl).siblings(".loading").remove();
                LookingPinger1(JSON.parse(e.d).id);
               

               // inplaceIntervalLooker = setTimeout(function () {  }, 2000);
            },
            error: function (e) { console.warn("ERROR inplaceBlockTitleEditorClose"); console.warn(e) }
        });
        }

      
    
    function inplaceBlockEditorClose(blockId) {
        var ctrl = $(".inplaceBlockEditor[blockid='" + blockId + "']");

        var text = RemoveHTMLTag(ctrl.val());
        var calcTime=CalculateCalcTime(ctrl);
         text = text.replace(/\(\(NF[^\)]+\)\)/g, ReloadSubBlockDataReplacer);
        $(ctrl).parent().removeClass("noEdit").html("<small>"+text+"</small>");

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/updateBlockText",
            data: JSON.stringify({ id: blockId, text: text, calcTime: calcTime }),
            dataType: "json",
            success: function (e) { },
            error: function (e) { console.warn("ERROR inplaceBlockEditorClose"); console.warn(e) }
        });
      

        clearTimeout(inplaceIntervalLooker);
        unlookBlock(false);

        return true;

    }

   
    function LookingPinger1(id) {
        if (id == "")
            return;
  
        //sEditedBlockId = sBlockId;
        // var cookie = document.cookie.replace("NFWSession=", "")
        var jdata = {
            sCookie: "",
            sNewsId: ""+id,
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/BlockLooker",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function () {
                inplaceIntervalLooker = setTimeout(function () { LookingPinger1(id); }, 2000);
            },
            error: function () {
                inplaceIntervalLooker = setTimeout(function () { LookingPinger1(id); }, 2000);
            }
        });
        return 1;
    }

    function approveAll()
    {
        if (checkURor([26, 29, 31]) == false) {
            blinkErrBlock(sBlockId);
            return;
        }
        NFconfirm("Approve All blocks.<br>You shure?", event.pageX - 300, event.pageY, 1, approveAllConfirmed);
    }
    function approveAllConfirmed(e)
    {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/approveAll",
            data: JSON.stringify({ id: $("#BlockContainer").attr("NewsId") }),
            dataType: "json",
            success: function (e) {
                $(".BlockNameRowContainer").addClass("blockStausGreen");
            },
            error: function (e) { console.warn("ERROR approveAllConfirmed"); console.warn(e) }
        });
    }

function blockItemEnable(ctrl) {

    $(ctrl.currentTarget).parent().attr("isDisable", true);
}

    function clickBlockItemNumber(e)
    {
        if (checkURor([24]) == false)
            return;
        var ctrl = $(e.currentTarget);
    
        if (ctrl.hasClass("BlockItemNumberActive"))
            return;
        $(".BlockItemNumberActive").each(function () { exitEditNumber(this) });
        $(".BlockItemNumberActive").removeClass("BlockItemNumberActive");

        $(ctrl).addClass("BlockItemNumberActive");
        var val = $(ctrl).html();
        $(ctrl).html($("<input type='text' class='BlockItemNumberEditor' maxlength='2'/>").val(val))
        
        var tb = $(ctrl).find("input");
        tb.focus().keydown(function (e) {
            if (e.keyCode == 8)
                return true;
            if (e.keyCode == 13)
                exitEditNumber($(e.target).parent());
            if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)))
                return false;
        });
        
        tb.blur(function () { exitEditNumber(ctrl) }).select();
        

        
    }
    function exitEditNumber(ctrl) {
        $(ctrl).removeClass("BlockItemNumberActive");
        var val = $(ctrl).find("input").val().replace(/^(\d{1,2})/, "$1");
        if (val.length == 0)
            val = 0;
        val = parseInt(val);

        var currArr = new Array();
        var currBg = $(ctrl).parent().find("[bgcolor]").attr("bgcolor");
        if (currBg == 0)
            currArr.push($(ctrl).parent());
        else
            $(".BlockNameRowContainer[bgcolor='" + currBg + "']").each(function () {
                currArr.push($(this).closest(".blockItem"))
            });



        $(ctrl).html(val);
        if(val<1)
        {
            currArr[0].insertBefore($(".blockItem").first());
            for (var i = 1; i < currArr.length; i++) {
                currArr[i].insertAfter(currArr[0]);
            }
        }
        else {
            if($(".blockItem[sortorder='"+parseInt(val-1)+"']").length>0)
            {
                var current = $(ctrl).parent().attr("sortorder");
                if (current >= (val))
                {
                    currArr[0].insertBefore($(".blockItem[sortorder='" + parseInt(val - 1) + "']"));
                    for (var i = 1; i < currArr.length; i++) {
                        currArr[i].insertAfter(currArr[0]);
                    }
                }
                else
                {
                    console.log(2);
                    currArr[0].insertAfter($(".blockItem[sortorder='" + parseInt(val - 1) + "']"));
                    for (var i = 1; i < currArr.length; i++) {
                        currArr[i].insertAfter(currArr[0]);
                    }
                }
                   // $(ctrl).parent().insertAfter($(".blockItem[sortorder='" + parseInt(val - 1) + "']"));
            }
            else {
                // $(ctrl).parent().insertAfter($(".blockItem").last());
                currArr[0].insertAfter($(".blockItem").last());
                for (var i = 1; i < currArr.length; i++) {
                    currArr[i].insertAfter(currArr[0]);
                }
            }
        }

    
        blocksResortSave();
        
    }
    function blocksResortSave() {
        var i = 0;
        var arr = new Array();
        $(".blockItem").each(function () {
            $(this).find(".blockItemNumber").html(++i);
            arr.push($(this).attr("id").replace("blockItem", ""));
        });

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/UpdateBlocksSort",
            data: JSON.stringify({ arr: arr }),
            dataType: "json",
            success: function () { },
            error: function (e) { console.warn("ERRR UpdateBlocksSort"); console.warn(e) }
        })
    }

    function inplaceBlockEditorMenuStateChange(e)
    {
        var ready=$(e.target).parent().find("input[type='checkbox']").first().is(":checked");
        var approve=$(e.target).parent().find("input[type='checkbox']").last().is(":checked");
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/inplaceBlockReadyApprove",
            data: JSON.stringify({ id: $(e.target).closest("[blockid]").attr("blockId"), ready: ready, approve: approve }),
            dataType: "json",
            success: function () { },
            error: function (e) { console.warn("ERRR UpdateBlocksSort"); console.warn(e) }
        });
        var total = new Object;

        total.Ready = ready;
        total.Approve = approve;
        
        var prt = $(e.target).closest(".BlockNameRowContainer");
        prt
            .removeClass("blockStausRed")
            .removeClass("blockStausYellow")
            .removeClass("blockStausGreen")
            .addClass(getBlockStatusClass(total));
        
    }
    function CapCameramenClick(ctrl)
    {
        if (checkURor([24]) == false)
            return;
        event.bubbles = false;
        // console.log(event);
        var blockid = $(ctrl).closest("[blockid]").attr("blockid");

        if ($("#BlockNameRowFirstContainer" + blockid).hasClass("NfwBgDanger")) {
            var elem = $("#lockUser" + blockid).blink();
            // alert("Block Is Look for Edit. User: " + $("#BlockNameRowFirstContainer" + sBlockId).attr("LookedUserName"));
            return;
        }
        if ($("#blockItem" + blockid).find(".blockInplaceCameramanSelect").length > 0)
            return;
        ///////////////
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/inplaceBlockCameramanGet",
            data: JSON.stringify({ id: blockid }),
            dataType: "json",
            success: function (e) {
                var arr = JSON.parse(e.d);
                var sel = $("<select class='blockInplaceCameramanSelect'></select>");
                arr.forEach(function (elem) {
                    var opt = $("<option></option>");
                    opt.val(elem.id).html(elem.name);
                    if (elem.active == true)
                        opt.prop("selected", true);
                    sel.append(opt);

                });

                $(ctrl).html(sel);
                $(ctrl).find("select").focus().click(function (e) { e.stopPropagation(); }).change(function (e) {
                    blockInplaceCameramanClose(e.currentTarget);
                });
            },
            error: function (e) { console.warn("ERRR inplaceBlockCameramanGet"); console.warn(e) }
        });


        ////////////////
       
}
function CapCutterClick(ctrl) {
    if (checkURor([24, 57 ]) == false)
        return;
    event.bubbles = false;
    // console.log(event);
    var blockid = $(ctrl).closest("[blockid]").attr("blockid");

    if ($("#BlockNameRowFirstContainer" + blockid).hasClass("NfwBgDanger")) {
        var elem = $("#lockUser" + blockid).blink();
        // alert("Block Is Look for Edit. User: " + $("#BlockNameRowFirstContainer" + sBlockId).attr("LookedUserName"));
        return;
    }
    if ($("#blockItem" + blockid).find(".blockInplaceCutterSelect").length > 0)
        return;
    ///////////////
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/inplaceBlockCutterGet",
        data: JSON.stringify({ id: blockid }),
        dataType: "json",
        success: function (e) {
            var arr = JSON.parse(e.d);
            var sel = $("<select class='blockInplaceCutterSelect'></select>");
            arr.forEach(function (elem) {
                var opt = $("<option></option>");
                opt.val(elem.id).html(elem.name);
                if (elem.active == true)
                    opt.prop("selected", true);
                sel.append(opt);

            });

            $(ctrl).html(sel);
            $(ctrl).find("select").focus().click(function (e) { e.stopPropagation(); }).change(function (e) {
                blockInplaceCutterClose(e.currentTarget);
            });
        },
        error: function (e) { console.warn("ERRR inplaceBlockCutterGet"); console.warn(e) }
    });


    ////////////////

}
    function CapAutorClick(ctrl, event) {
        if (checkURor([24]) == false)
            return;
        event.bubbles = false;
       // console.log(event);
        var blockid = $(ctrl).closest("[blockid]").attr("blockid");

        if ($("#BlockNameRowFirstContainer" + blockid).hasClass("NfwBgDanger")) {
            var elem = $("#lockUser" + blockid).blink();
            // alert("Block Is Look for Edit. User: " + $("#BlockNameRowFirstContainer" + sBlockId).attr("LookedUserName"));
            return;
        }

        if ($("#blockItem" + blockid).find(".blockInplaceAutorSelect").length > 0)
            return;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: serverRoot + "testservice.asmx/inplaceBlockAutorsGet",
                data: JSON.stringify({id:blockid }),
                dataType: "json",
                success: function (e) {
                    var arr = JSON.parse(e.d);
                    var sel = $("<select class='blockInplaceAutorSelect'></select>");
                    arr.forEach(function (elem) {
                        var opt = $("<option></option>");
                        opt.val(elem.id).html(elem.name);
                        if (elem.active == true)
                            opt.prop("selected", true);
                        sel.append(opt);

                    });
                  
                    $(ctrl).html(sel);
                    $(ctrl).find("select").focus().click(function (e) {e.stopPropagation();  }).change(function (e) {
                        blockInplaceAutorClose(e.currentTarget);
                    });
                },
                error: function (e) { console.warn("ERRR inplaceBlockAutorsGet"); console.warn(e) }
            });
        
    }

    function blockInplaceCameramanClose(ctrl) {
        var blockId = $(ctrl).closest("[blockid]").attr("blockid");
        var newAutorId = $(ctrl).val();
        var newAutorName = $(ctrl).find("[value='" + newAutorId + "']").html();
        $(ctrl).parent().html(newAutorName);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/inplaceBlockCameramenSet",
            data: JSON.stringify({ id: blockId, autorId: newAutorId }),
            dataType: "json",
            success: function (e) { },
            error: function (e) { console.warn("ERRR blockInplaceAutorClose"); console.warn(e) }
        });
}
function blockInplaceCutterClose(ctrl) {
    var blockId = $(ctrl).closest("[blockid]").attr("blockid");
    var newAutorId = $(ctrl).val();
    var newAutorName = $(ctrl).find("[value='" + newAutorId + "']").html();
    $(ctrl).parent().html(newAutorName);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/inplaceBlockCutterSet",
        data: JSON.stringify({ id: blockId, autorId: newAutorId }),
        dataType: "json",
        success: function (e) { },
        error: function (e) { console.warn("ERRR blockInplaceCutterClose"); console.warn(e) }
    });
}
    function blockInplaceAutorClose(ctrl) {
     
        var blockId = $(ctrl).closest("[blockid]").attr("blockid");
        var newAutorId = $(ctrl).val();
        var newAutorName = $(ctrl).find("[value='" + newAutorId + "']").html();
        $(ctrl).parent().html(newAutorName);
        
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/inplaceBlockAutorSet",
            data: JSON.stringify({ id: blockId, autorId: newAutorId }),
            dataType: "json",
            success: function (e) {},
            error: function (e) { console.warn("ERRR blockInplaceAutorClose"); console.warn(e) }
        });

    }


   
    
    function CalculateCalcTime(ctrl) {
     //   debugger;
        var extrachrono = 0;
        var text = $(ctrl).val();
        text = text.replace(/\(\(([^)]+)\)\)/g, FindChronoInText);
        text = text.replace(/[0-9]/g, "");

        //$("#BlockEditCalcTextBox").val(msToTime(((text.length / GetReadRate()) + parseInt(extrachrono)) * 1000));
       // Blockcalctime = msToTime(((text.length / GetReadRate()) + parseInt(extrachrono)) * 1000);
        var ret = parseInt(text.length / parseInt($(ctrl).attr("readrate"))+parseInt(extrachrono));
        return ret;

        function FindChronoInText(text, p1) {
            //ХР 00:00:00
            var regexp = /ХР\s([0-9]+)\:([0-9]+)\:([0-9]+)/g;
            var result = regexp.exec(p1);
            if (result != null)
                extrachrono += parseInt(parseInt(result[3]) + (result[2] * 60) + (result[1] * 60 * 60));

        }

    }
    function BlockNameBlockTimeControlClick(event) {
        if (checkURor([24]) == false)
            return;

        var ctrl = $(event.currentTarget);
        if (ctrl.hasClass("noEdit"))
            return;

        event.stopPropagation();
        $(".BlockNameBlockTimeControl.noEdit").each(function () { BlockNameBlockTimeControlExit($(this)) });
        var val = $(ctrl).find("p").html();
        ctrl.addClass("noEdit");
        ctrl.html($("<input type='text' class='BlockInplaceTimeControl' maxlength='8'/>").val(val));
        ctrl.find("input").focus().select().keydown(function (e) {

            if (e.keyCode == 8 || e.keyCode == 186)
                return true;
            if(e.keyCode==13)
            {
                BlockNameBlockTimeControlExit($(e.currentTarget).parent());
            }
            if  (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)))
                return false;
   
           
        });
    }
    function BlockNameBlockTimeControlExit(ctrl) {
        var inp = $(ctrl).find("input");
        var time = checkTimes(inp);
        ctrl.removeClass("noEdit");
        if(time<0)
            $(ctrl).html("<p class='text-muted'>ERROR</p>");
        else {
            $(ctrl).html("<p class='text-muted'>" + msToTime(time * 1000) + "</p>");
            $.post(serverRoot + "testservice.asmx/inplaceBlockChronoSet", {id:$(ctrl).closest("[blockid]").attr("blockid"), time:time},function(){},"application/json" )
           
        }
        
    }
    function BlockNameBlockTaskTimeControlClick() {
        if (checkURor([24]) == false)
            return;
        var ctrl = $(event.currentTarget);
        if (ctrl.hasClass("noEdit"))
            return;

        event.stopPropagation();
        $(".BlockNameTaskTimeControl.noEdit").each(function () { BlockNameBlockTaskTimeControlExit($(this)) });
        var val = $(ctrl).find("p").html();
        ctrl.addClass("noEdit");
        ctrl.html($("<input type='text' class='BlockInplaceTimeControl' maxlength='8'/>").val(val));
        ctrl.find("input").focus().select().keydown(function (e) {

            if (e.keyCode == 8 || e.keyCode == 186)
                return true;
            if (e.keyCode == 13) {
                BlockNameBlockTaskTimeControlExit($(e.currentTarget).parent());
            }
             if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)))
                return false;


        });
    }
    function BlockNameBlockTaskTimeControlExit(ctrl) {
        var inp = $(ctrl).find("input");
        var time = checkTimes(inp);
        ctrl.removeClass("noEdit");
        if (time < 0)
            $(ctrl).html("<p class='text-muted'>ERROR</p>");
        else {
            $(ctrl).html("<p class='text-muted'>" + msToTime(time * 1000) + "</p>");
            $.post(serverRoot + "testservice.asmx/inplaceBlockTaskTimeSet", { id: $(ctrl).closest("[blockid]").attr("blockid"), time: time }, function () { }, "application/json")

        }
}
function unLockItems() {

    console.log("CLICK")
        var tmp = $(".unLockItems").val();
        $(".unLockItems").val("in work..");
        $.ajax({
            type: "POST",
            contentType: "application/json;",
            url: serverRoot + "testservice.asmx/unLockItems",
            data: JSON.stringify({ id: 0 }),
            dataType: "json",
            async: true,
            success: function (data) {
                $(".unLockItems").val("ready");
                setTimeout(function () { $(".unLockItems").val("unlock Items") }, 2000)
            },
            error: function (data) {
                ajaxErr("/testservice.asmx/unlockItems error", data);
                $(".unLockItems").val(tmp);
            }

        })
   
}
    function addBlockByTypeClick(ctrl) {
        if (checkURor([24]) == false)
            return;
        var field = $(ctrl).siblings("ul").html($(div).loading50());
        /*<li><a href="#">Dropdown link</a></li>
   <li><a href="#">Dropdown link</a></li>*/


        NFpost(serverRoot + "testservice.asmx/getAvBlockTypes", { NewsId: $("#BlockContainer").attr("NewsId") }, function (e) {
            var arr = JSON.parse(e.d);
            field.html("");
            if (arr.status > 0)
                arr.val.forEach(function (elem) {
                    field.append($("<li></li>").append($('<a href="#"></a>')
                        .html(elem.TypeName)
                        .attr("typeId", elem.id)
                        .click(function (e){addBlockByType($(e.currentTarget).attr("typeId"))}
                        ))
                        )
                });
        })
    }
    function addBlockByType(typeId){
        NFpost(serverRoot + "testservice.asmx/BlockByTypeCreate", { NewsId: $("#BlockContainer").attr("NewsId"), typeId: typeId, groupId: $("#BlockContainer").attr("GroupId") }, function (e) {
        
            var dt = JSON.parse(e.d);
            console.log(dt);
            reloadBlockList(JSON.parse(dt.NewsData));
            updateBlockData(JSON.parse(dt.NewsData));
        });
    }
    function blockEDL(id)
    {
        $("#DownloadIFrame").attr("src", serverRoot+"API/EDL/" + id+"/edl.xml");
    }
    function setBlockActive(id) {
        $(".blockItem").removeClass("active");
        $("#blockItem"+id).addClass("active")
    }
    function PrintBlock(id) {
      
      
        var html = $("#SubBlockTextControl" + id).find("small").html();
        var title = $("#BlockNameControl" + id).html();
        var header = $("#BlockNameSubRowContainer" + id).find(".SubBlockAutorRow").html();
        $("body").append("<div id='printBlock' style='font-size:11px;line-height:14px'></div>");
       // html = html.replace(/[\n\r]+/g, "<br/>\n");
        html = html.replace(/[\n\r]+/g, "<div class='printLIneDivider'></div>\n");
        html = html.replace(/\(\([^\)]+\)\)/gm,  (a, b) => { return "<br><div ><i style='font-weight:normal'>"+a+"</i></div>"});

        while (html.indexOf("<br><br><br>") >= 0) {
            html = html.replace("<br><br><br>", "<br><br>");
        }
        $("#printBlock").html(title + "<br>" + header + "<hr><br>"+html+"")
        .printThis({
            importCSS: true,
            importStyle: false,        
        });
        setTimeout(function(){ $("#printBlock").remove();}, 500);
    }
    function printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        var a = document.body.scrollTop;
        document.body.innerHTML = printContents;
        document.body.style.fontSize = "18px";
        window.print();
        document.body.style.fontSize = "inherit";

        document.body.innerHTML = originalContents;
        document.body.scrollTop = a;
}
function expandAllBlocks() {
   
    $(".blockItem:not(.expanded)").each(function (i, e) { 
        toggleBlockItem($(e).attr("targetId"));
    });
}
function collapseAllBlocks() {

    $(".blockItem.expanded").each(function (i, e) {
        toggleBlockItem($(e).attr("targetId"));
    });
}


function ajax(url, data) {
    return new Promise((res, rej) => {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + url, // "testservice.asmx/GetBlockDataEx",
            data: data,// "{sBlockId:'" + BlockId + "'}",
            dataType: "json",
            success: (data) => { return res(data)},
            error: (data) => { return rej(data)}
        });
    })
}
async function AddSynegyFileToBlock(blockId) {
    $("#AddForm").modal();
    $("#AddForm").attr("blockId", blockId);
    var $box = $("#AddForm").find(".modal-body")
    var tmp = $box.html();
    $('#AddForm').on('hidden.bs.modal', function (e) {
        if (tmp) { 
        $box.html(tmp);
        tmp = null;
        }
    })

 
    $box.html("<div id='cynBox'style='height:300px;    overflow-y: scroll;'></div>");


    $("#cynBox").loading50()
    var res = await ajax("testservice.asmx/GetSynegyRoot", JSON.stringify({ currID: "14CF8BD9-EA64-478C-B147-B004F422BBE4" }));
    $("#cynBox").html('')
    var data = JSON.parse(res.d);
    data.elem.forEach((elem) => {
        insertElemToSynegyList(elem, $("#cynBox"), blockId);
    })
}
function insertElemToSynegyList(elem, $parent, blockId) {
    $parent.append($("<div></div>").attr("id", elem.id));
    var $inserted = $("#" + elem.id);
    $inserted.append(
        $("<div class='cynDescr'></div>")
            .append($("<div class='cyIcon" + elem.type + "'></div>"))
            .append($("<div class='cyTitle'></div>").html(elem.name)))
    $inserted.click(async (e) => {
        e.preventDefault();
        e.stopPropagation();
         if (elem.type == 10 || elem.type == 16 || elem.type == 20) {
             $("#AddForm").modal("hide");
             var res = await ajax("testservice.asmx/SaveSynegyRoot", JSON.stringify({ elemId: elem.id, elemTitle:elem.name, blockId: blockId }));
           
         }
        if (elem.type == 3 || elem.type == 15 || elem.type == 19 ) {
            if (!$inserted.hasClass("expanded")) {
                $inserted.addClass("expanded")
                $inserted.append($("<div style='margin-left: 2em;'></div>").attr("id", "childrens_" + elem.id))
                $("#childrens_" + elem.id).loading50()
                var res = await ajax("testservice.asmx/GetSynegyRoot", JSON.stringify({ currID: elem.id }));
                var data = JSON.parse(res.d);
                setTimeout(() => {
                    $("#childrens_" + elem.id).html('')
                    data.elem.forEach(async (children) => {

                        insertElemToSynegyList(children, $("#childrens_" + elem.id), blockId);
                    })
                }, 200)


            }
            else {
               
                    $inserted.removeClass("expanded")
                    $("#childrens_" + elem.id).remove();
                
            }
        }
    })

}
async function saveBlockText(id) {

    console.log(serverRoot + "blocktext/" + id)
    let response = await fetch(serverRoot + "blocktext/" + id);
    if (response.ok) { 
        let json = await response.json();
        alert(json.status);
    } else {
        alert("Ошибка HTTP: " + response.status);
    }
    /*
    var elem = document.createElement("iframe")
    elem.style.display = "none"
    elem.src ="/blocktext/"+id
    document.body.appendChild(elem);
    setTimeout(function() {elem.parentElement.removeChild(elem) },1000)*/
}
async function BlockToRss(id) {
    $.post(serverRoot + "testservice.asmx/blockToRss", { id }, function (e) {
        console.log(blockToRss, e);
    }, "application/json")

}




