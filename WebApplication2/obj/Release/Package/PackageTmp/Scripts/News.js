$(document).ready(function () {
 
    window.history.pushState(null, 'index', serverRoot + 'index');

    $("#exitBtn").show();
    $("#exitBtn").attr("title", "exit");
    $("#exitBtn").attr('data-placement', 'top');
    $("#exitBtn").tooltip();
    $("#mainPanelRss").click(() => {
        $(".mainPanelRss").load("/news/rssnews.html");
    })

    ReloadNews();
    initBloks();
    initMainHeadMenu();
    initIframeResize();
    $(".NFfileUploadWr").NfFileUpload();
    
    setInterval(function () {
        if ($("#playoutForm").is(":visible")) {
            playOutStatusUpdate();
        }
    }, 2000);
    $("#blocksCollapsedBtn").click(function () {
        if ($('#BlocksPanelHead').hasClass('blocksCollapsed')) {
            $('#BlocksPanelHead').removeClass('blocksCollapsed');
            localStorage.setItem("blocksCollapsed", 0);
        } else {
            $('#BlocksPanelHead').addClass('blocksCollapsed');
            localStorage.setItem("blocksCollapsed", 1);
        }

    });
    $("#blocksExpandedBtn").click(function () {
        if ($('#BlocksPanelHead').hasClass('blocksExpanded')) {
            $('#BlocksPanelHead').removeClass('blocksExpanded');
            collapseAllBlocks();
            localStorage.setItem("blocksExpanded", 0);

        } else {
            $('#BlocksPanelHead').addClass('blocksExpanded');
            expandAllBlocks();
            localStorage.setItem("blocksExpanded", 1);
        }
    });
    if (localStorage.getItem("blocksCollapsed") == 1){
        $('#BlocksPanelHead').addClass('blocksCollapsed');
    }
    if (localStorage.getItem("blocksExpanded") == 1) {
        $('#BlocksPanelHead').addClass('blocksExpanded');
        expandAllBlocks();
    }
    $(".NFfileUploadHeader").click(function () {
        $(".NFfileUploadWrBox").toggleClass("hided");
    })
});

function initIframeResize(){
    $(window).on('resize', function () {
        if($('#FullScreenDiv').find("iframe").length>0)
        {
            setIframeMaxHeigth($('#FullScreenDiv').find("iframe").attr("id"));
        }
    });

}
function emptyBlocksHeader()
{
  
    $(".blocksHeadControl").html("");
    $("#bpNewsName").html('<h4><div id="CapToStart" class="alert alert-success caption caption-html" captionid="CapToStart" role="alert"></div><script>$("#CapToStart").html(langTable["CapToStart"])</script></h4>');
    $("#AddBlockButton").hide();
    $("#BlockContainer").html("");
    $("#BlockContainer").attr("newsid", -0);
    $("#BlockContainer").attr("groupid", -100);

    $("#BlockContainer").attr("prId", $("#ProgramDropDown").val());
}
function programOnChange()
{
    // initBloks();
    
    emptyBlocksHeader();
    ReloadNews();
 
}
function ReloadNews()
{
    if ($("#ProgramDropDown").val() < 0)
        return programmsReload();
    $("#BlockContainer").attr("prId", $("#ProgramDropDown").val());

    localStorage.setItem("lastProgramm", $("#ProgramDropDown").val());
        var jdata = {
            Cookie: 0,
            NewsId: $("#ProgramDropDown").val()//SelectedProgramListValue
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/GetListOfNews",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: GetNewsListSucceeded,
            error: PingFailed
        });
        
}
var NewsTableRowCount;
function GetNewsListSucceeded(data)
{
   
    //console.log(data.d);
    var list = JSON.parse(data.d);
    NewsTableRowCount = 0;
   
    ReloadNewsContainer(list);
    ReloadNewsData(list);

    if (typeof (lastRoute) != 'undefined' && typeof (lastRoute.newsId) != 'undefined' && typeof (lastRoute.groupId) != 'undefined') {
       
        ClickNews(lastRoute.newsId, lastRoute.groupId);
        delete lastRoute.newsId;
  
    }

    return;
}
function AddNewsRow(RowData) {
    var table = document.getElementById("NewsTable");
    var row;
    // console.log("table.rows.length" + table.rows.length + "  " + NewsTableRowCount);
    if (NewsTableRowCount < table.getElementsByTagName("tr").length) {
        row = table.getElementsByTagName("tr")[NewsTableRowCount];
    } else {
        row = table.insertRow(table.rows.length);
    }
    NewsTableRowCount++;


    row.setAttribute("id", RowData.NewsId + "BaseRowNewsId");
    row.setAttribute("GroupId", RowData.GroupID + "BaseRowGroupId");
    //  console.log("AddNewsRow " + RowData);
    if (RowData.NewsId != 0) {
        row.style.cursor = "pointer";

    }
    else {

    }
    var newcell;
    if (0 < row.cells.length) { newcell = row.cells[0]; newcell.colSpan = 1; }
    else { newcell = row.insertCell(0); }
    //    if (1 < row.cells.length) { newcell = row.cells[1]; }
    //    else { newcell = row.insertCell(1); }


    newcell.setAttribute("id", "CellNewsName" + RowData.NewsId);
    newcell.setAttribute("GroupId", "CellNewsGroup" + RowData.GroupID);
    

    if (RowData.NewsId != 0) {

        newcell.innerHTML = RowData.NewsName;
        console.log(2);
        if (RowData.GroupID == 0) {
            if ($("#BlockContainer").attr("NewsId") == RowData.NewsId && $("#divnewsbuttons"+RowData.NewsId).length==0) //если была ранее нажато конопочка, то добавляем их группу
                newcell.innerHTML += CreateNewsButtons(RowData.NewsId);
        }
        else {
            if ($("#BlockContainer").attr("NewsId") == RowData.NewsId && $("#divnewsbuttons" + RowData.NewsId).length == 0) //если была ранее нажато конопочка, то добавляем их группу
                newcell.innerHTML += CreateCopyNewsButtons(RowData.NewsId);
        }
        InitBlocksDrop("CellNewsName" + RowData.NewsId);
        InitNewsDrag("CellNewsName" + RowData.NewsId);
        newcell.setAttribute("class", "drappableNews");
        
        //newcell.setAttribute("class", "DragingNews");
    }
    else {
              //InitBlocksDrop("CellNewsName" + RowData.GroupID);
        newcell.setAttribute("class", "CellNewsGroup" + RowData.GroupID);
        InitNewsDrop("CellNewsGroup" + RowData.GroupID, "CellNewsName" + RowData.NewsId);
        newcell.innerHTML = "<span class='label label-default'>" + RowData.NewsName+"</span>";
    }
    if (RowData.NewsId != 0) {
        newcell.onclick = function () {
            ClickNews(RowData.NewsId, RowData.GroupID);
            $("#BlockContainer").attr("NewsId") = RowData.NewsId;
            $('.divnewsbuttons').remove();// удаляем кнопки на всех элемантах
            if (RowData.GroupID == 0) {// рисуем кнопки на выбранном элементе
                var div = document.getElementById("CellNewsName" + RowData.NewsId);
                div.innerHTML = div.innerHTML + CreateNewsButtons(RowData.NewsId);
    
            }
                
            else {
                var div = document.getElementById("CellNewsName" + RowData.NewsId);
                div.innerHTML = div.innerHTML + CreateCopyNewsButtons(RowData.NewsId);
            }
        }
        
    }
}

function programmsReload()
{
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url:  serverRoot+"testservice.asmx/programsListGet",
        data: JSON.stringify({id:0}),
        dataType: "json",
        async: true,
        success: function(data){
            
            var dt = JSON.parse(data.d);
            if(dt.length==0)
            {
                $("#CapToStart").html("У Вас нет доступных программ.<br>Смотрите настройки пользовательских прав.");
                $("#CapToStart").removeClass("alert-success");
                $("#CapToStart").addClass("alert-danger");
                $("#ProgramDropDown").html("");
            }
            else {
                $("#ProgramDropDown option").remove();

                var r = localStorage.getItem("lastProgramm");
                if (typeof (lastRoute) != 'undefined' && typeof (lastRoute.programId) != 'undefined')
                {
                    r = lastRoute.programId;
                    delete lastRoute.programId;
                }

                var selected = false;
                dt.forEach(function (item) {
                    $("#ProgramDropDown").append($("<option></option").attr("value", item.ProgramID).attr("id", "ProgramDropDownOption" + item.ProgramID).text(item.ProgramName));
                    if(r==item.ProgramID)
                    {
                        selected = true;
                        $("#ProgramDropDownOption" + r).prop("selected", true);
                    }
                });
                if(!selected)
                    $("#ProgramDropDown option:first").prop("selected", true);
                ReloadNews();
            }
           
        },
        error: PingFailed
    });

}

function AddProgramsRow(RowData) {

    var option = document.createElement('option');
    option.text = RowData.NewsName;
    option.value = RowData.NewsId;
    document.getElementById("ProgramDropDown").add(option, 0);
}
function setSelectedValue(selectObj, valueToSet) {
    for (var i = 0; i < selectObj.options.length; i++) {
        if (selectObj.options[i].text == valueToSet) {
            selectObj.options[i].selected = true;
            return;
        }
    }
}

////////////

function ReloadNewsContainer(data) {
    
    if (!(typeof data === "undefined")) {//если в данных есть выпуски
        data.forEach(AddNewsDiv);
        
    }
    return;
    $('.NewsContainer').each(function () {//удаление ненужных контенеров новостей
        var find = false;// цикл по контейнерам, вложенный по структуре. Если нет контейнера в структуре, удаляем его
        var container = this;

        data.forEach(function (InData) {
            FindedId = InData.NewsId;
            if (FindedId < 0) FindedId = 0-FindedId;
            if (container.id == "NewsContainer" + datar.NewsId) {
                find = true;  
            }
        });
        if (find == false) {
            $(container).remove();
          
        }
        //////
    });
    return;

}
function AddNewsDiv(data) {
   
    var ContainerId = "NewsRow" + data.NewsId;
    ContainerId = ContainerId.replace("-", "");

    if ($("#" + ContainerId).length <= 0) {
         $("#NewsContainer").append("<div id='" + ContainerId + "' class='NewsRow'> ");

    }

    if (data.NewsId < 0)
    {
        if ($("#NewsGroup" + (0 - data.NewsId)).length==0)
            $("#" + ContainerId).append("<div id='NewsGroup" + (0 - data.NewsId) + "' class='NewsGroup'> ");
        $("#NewsGroup" + (0 - data.NewsId)).html(GenerateNewsGroupBlankDivContainer(data));
        $("#NewsGroup" + (0 - data.NewsId)).attr("GroupId", data.GroupID);
        DaDInitDropByElement($("#NewsGroup" + (0 - data.NewsId)));// цель для копирования выпусков
    }
    else {
        if ($("#NewsCell" + data.NewsId).length==0)
            $("#" + ContainerId)
                .append("<div id='NewsCell" + data.NewsId + "' class='NewsCell' newsId='" + data.NewsId + "'> ")
                .swipeX(newsSwipeX)
        ;
           $("#NewsCell" + data.NewsId).append(GenerateNewsBlankDivContainer(data));
       // $("#NewsCell" + data.NewsId).GenerateNewsBlankDivContainer();
        $("#NewsCell" + data.NewsId).attr("GroupId", data.GroupID);
        $("#NewsContentName" + data.NewsId).attr("GroupId", data.GroupID);
        DaDInitDragByElement($("#NewsContentName" + data.NewsId));
        DaDInitDropByElement($("#NewsContent" + data.NewsId));// цель для копирования блоков
        
    }
}

function GenerateNewsBlankDivContainer(data) {
    ret='\
                        <h5><div class="NewsContent" id="NewsContent' + data.NewsId + '"  oncontextmenu="OpenNewsScript(' + data.NewsId + '); return false;">\
<div id="NewsContentName'+ data.NewsId + '"></div>\
        </div ></h5>';

   
    /*if (data.GroupID == 0 && data.NewsId>0)
        ret = ret + CreateNewsButtons(data.NewsId);
    if (data.GroupID > 0 && data.NewsId > 0)
        ret = ret + CreateCopyNewsButtons(data.NewsId);*/
    return ret;
}// здесь форматирование ячейки
function GenerateNewsGroupBlankDivContainer(data) {
    return '\
                       <h4> <div class="NewsContent" id="NewsContent' + (0 - data.NewsId) + '">\
<div id="NewsContentName' + (0 - data.NewsId) + '"></div>\
        </div></h4>';
}
function CreateCopyNewsButtons(sBlockId) {
    
    var ret = "\
    <div class='divnewsbuttons' id='divnewsbuttons" + sBlockId + "'><p><div class=\"btn-group btn-group-sm\" role=\"group\" aria-label=\"...\">\
       \
            <button id='menuNewsBtnEdit" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"EditNews(" + sBlockId + ",1);event.stopPropagation()\" data-toggle='tooltip' data-placement='top' title='Изменить'><span class='glyphicon glyphicon-pencil' aria-hidden='true'><span></button>\
            <script>$('#menuNewsBtnEdit" + sBlockId + "').attr('title', langTable['CapEdit'])</script>\
            <button id='menuNewsBtnToCurrent" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"DaDCopyNews(" + sBlockId + ",0); event.stopPropagation()\" data-toggle='tooltip' data-placement='top' title='Создать текущий выпуск'><span class='glyphicon glyphicon-open' aria-hidden='true'><span></button>\
            <script>$('#menuNewsBtnToCurrent" + sBlockId + "').attr('title', langTable['CapToCurrent'])</script>\
            <button id='menuNewsBtnDelete" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"DeleteNews(" + sBlockId + ", 1, event)\" data-toggle='tooltip' data-placement='top' title='Удалить' ><span class='glyphicon glyphicon-trash' aria-hidden='true'><span></button>\
            <script>$('#menuNewsBtnDelete" + sBlockId + "').attr('title', langTable['CapDelete'])</script>\
        \
    </div>";

    return ret;
}
function CreateNewsButtons(sBlockId) {

  var  ret= "\
    <div class='divnewsbuttons' id='divnewsbuttons" + sBlockId + "'><p><div class=\"btn-group btn-group-sm\" role=\"group\" aria-label=\"...\">\
  \
        <button id='menuNewsBtnScript" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"OpenNewsScript(" + sBlockId + ");event.stopPropagation()\" data-toggle='tooltip' data-placement='top' ><span class='glyphicon glyphicon-list-alt' aria-hidden='true'></span></button>\
        <script>$('#menuNewsBtnScript" + sBlockId + "').attr('title', langTable['CapScript'])</script>\
        <button  id='menuNewsBtnEdit" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"EditNews(" + sBlockId + ",0);event.stopPropagation()\" data-toggle='tooltip' data-placement='top' title='Изменить'><span class='glyphicon glyphicon-pencil' aria-hidden='true'></span></button>\
        <script>$('#menuNewsBtnEdit" + sBlockId + "').attr('title', langTable['CapEdit'])</script>\
        <button  id='menuNewsBtnUpload" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"NewsUpload(" + sBlockId + ");event.stopPropagation()\" data-toggle='tooltip' data-placement='top' title='Копировать из других выпусков'><span class='glyphicon glyphicon-upload' aria-hidden='true'></span></button>\
        <script>$('#menuNewsBtnUpload" + sBlockId + "').attr('title', langTable['CapNewsBtnUpload'])</script>\
\
       \
        <button  id='menuNewsBtnToArchive" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"DaDNewsToArchive(" + sBlockId + ",0, event);event.stopPropagation()\" data-toggle='tooltip' data-placement='top' title='В архив'><span class='glyphicon glyphicon-lock' aria-hidden='true'></span></button>\
        <script>$('#menuNewsBtnToArchive" + sBlockId + "').attr('title', langTable['CapToArchive'])</script>\
       \
<div class='dropdown btn-group btn-group-sm' style='display:inline'>\
  <button class='btn btn-default dropdown-toggle' type='button'  id='dropdownNewsMenu" + sBlockId + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='true' >\
   <span class='glyphicon glyphicon-menu-hamburger'></span>\
    <span class='caret'></span>\
  </button>\
  <ul class='dropdown-menu dropdown-menu-right' aria-labelledby='dropdownNewsMenu" + sBlockId + "'>\
    <li>\
        <button  id='menuNewsBtnToWord" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"NewsToWord(" + sBlockId + ");event.stopPropagation()\" ><span class='glyphicon glyphicon-save' aria-hidden='true'></span> </button>\
        <script>$('#menuNewsBtnToWord" + sBlockId + "').append( langTable['CapmenuNewsBtnToWord'])</script>\
    </li>\
    <li>\
 <button  id='menuNewsBtnToTemplates" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"DaDCopyNews(" + sBlockId + ",102);event.stopPropagation()\" '><span class='glyphicon glyphicon-duplicate' aria-hidden='true'></span> </button>\
        <script>$('#menuNewsBtnToTemplates" + sBlockId + "').append( langTable['CapToTemplate'])</script>\
    </li>\
    <li>\
 <button  id='menuNewsBtnToCopybox" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"DaDCopyNews(" + sBlockId + ",2);event.stopPropagation()\"><span class='glyphicon glyphicon-piggy-bank' aria-hidden='true'></span> </button>\
        <script>$('#menuNewsBtnToCopybox" + sBlockId + "').append(langTable['CapToCopybox'])</script>\
</li>\
 <li>\
 <button  id='menuNewsBtnToPlayout" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"newsToPlayOut(" + sBlockId + ");event.stopPropagation()\" ><span class='glyphicon glyphicon-copy' aria-hidden='true'></span>  </button>\
        <script>$('#menuNewsBtnToPlayout" + sBlockId + "').append( langTable['toPlayOuts'])</script>\
        \
</li>\
      <li>\
 <button  id='menuNBtnToTitleout" + sBlockId + "' type=\"button\" class=\"btn btn-default\" onclick=\"newsToTitleOut(" + sBlockId + ");event.stopPropagation()\" ><span class='glyphicon glyphicon-copy' aria-hidden='true'></span>  </button>\
        <script>$('#menuNBtnToTitleout" + sBlockId + "').append( langTable['toTitleOuts'])</script>\
        \
</li>\
       <li role='separator' class='divider'></li>\
       <li>\
 <button  id='' type=\"button\" class=\"btn btn-default\" onclick=\"window.open('/news/prompter.html?newsid="+ sBlockId+"');event.stopPropagation()\" ><span class='glyphicon glyphicon-hdd' aria-hidden='true'></span> Открыть суфлер  </button>\
        \
</li>\
    <li role='separator' class='divider'></li>\
    <li>\
 <button  id='menuNewsBtnDelete" + sBlockId + "' type=\"button\" class=\"btn btn-danger\" onclick=\"DeleteNews(" + sBlockId + ", 0, event);event.stopPropagation()\" ><span class='glyphicon glyphicon-trash' aria-hidden='true'></span>  </button>\
        <script>$('#menuNewsBtnDelete" + sBlockId + "').append( langTable['CapDelete'])</script>\
        \
</li>\
  </ul>\
</div>\
\
    </div>";
  return ret;
   
}
function CreateNewsData(data) {
   
}
////////ЗАГРУЗКА ДАННЫХ
function ReloadNewsData(data) {
    if (!(typeof data === "undefined")) {//если в данных есть выпуски
        data.forEach(AddNewsRowData);


        var sort = 0;
        $('.NewsRow').attr("SortOrder", "");

        data.forEach(function (NewsBlocks) {// цикл - проставляем атрибуты для сортировки
            $('#NewsRow' + NewsBlocks.NewsId).attr("SortOrder", sort);
            $('#NewsRow' + (0 - NewsBlocks.NewsId)).attr("SortOrder", sort);
            sort++;
        });



        $('.NewsRow').each(function () {

            if ($(this).attr("SortOrder") == "") {
                $(this).remove();
            }

        });
        SortDivs($('#NewsContainer'), "SortOrder");
    }
}
function AddNewsRowData(data) {
   
    if (data.NewsId > 0) {///это выпуски
        if (data.GroupID == 0) {
            $('#NewsContentName' + data.NewsId).html("<h5>" + data.NewsName + "<br><small>" + data.NewsDate + "</small></h5>");
        }
        else {
            $('#NewsContentName' + data.NewsId).html("<h5>" + data.NewsName + "<br></h5>");
        }
        //$('#NewsContentData' + data.NewsId).html("<h5><small>" + data.NewsDate + "</h5></small>");
        $('#NewsContent' + data.NewsId).click(function () {
           ClickNews(data.NewsId, data.GroupID);
            
            $('.divnewsbuttons').remove();// удаляем кнопки на всех элемантах
            if (data.GroupID==0) {   
                // рисуем кнопки на выбранном элемен
                
                var $c = $('#NewsContent' + data.NewsId)
                $c.append(CreateNewsButtons(data.NewsId));
                $c.find(".dropdown").find("button").click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    console.log($c.find(".dropdown-menu"));
                    $c.find(".dropdown-menu").css("display","block");
                });
                $c.find(".dropdown-menu").mouseleave(function () { $(this).fadeOut(500) });
                $c.find(".dropdown-menu").find("button").click(function () { $c.find(".dropdown-menu").fadeOut(500) });
                $('[data-toggle="tooltip"]').tooltip();
            }
            else
            {
                $('#NewsContent' + data.NewsId).append(CreateCopyNewsButtons(data.NewsId));
            }
        });

     }
    else {/// это названия групп
        $('#NewsContentName' + (0 - data.NewsId)).html(data.NewsName);
    }
    if ($('#addPersonalFolderBtn').length == 0)
        $("#NewsContentName1001").html($("#NewsContentName1001").html() + "<input type='button' style='margin-left:10px' class='btn btn-default btn-xs  caption caption-value' captionid='CapAddPersonalCopybox' value='" + langTable['CapAddPersonalCopybox'] + "' onclick='addPersonalFolder()' id='addPersonalFolderBtn' />");
}
//////// действия
function NewsMoveSucceeded(data) {
    ReloadNews();
    ShowAlert(langTable['AlertNewsMove']);
}
function DaDNewsToArchive(FromId, ToId, event) {

    event.stopPropagation();

    if (checkURor([17, 20]) == false) {
        $("#NewsCell" + FromId).css("border-left", "4px solid 	#d9534f");
        setTimeout(function () { $("#NewsCell" + FromId).css("border-left", ""); }, 500);
        return;
    }
    if (typeof (event) != 'undefined') {
        NFconfirm(langTable['AlertNewsArchiveConfirm'], document.width>950? event.pageX:50, event.pageY, { FromId: FromId, ToId: ToId }, DaDNewsToArchiveConfirmed);

    }
    else
        DaDNewsToArchiveConfirmed({ FromId: FromId, ToId: ToId });
}
function DaDNewsToArchiveConfirmed(prm)
    {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/NewsToArchive",
        data: JSON.stringify({ id: prm.FromId }),
        dataType: "json",
        async: true,
        success: DeleteNewsSucceeded,
        error: AjaxFailed
    });
};

function DaDCopyNews(sFromId, sToGroupId) {

    var jdata = {
        sSourceId: sFromId,
        sDestGroupId: sToGroupId,
        sDestProgramId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value,
        Coockie: getCookie("NFWSession")

    };
    log(jdata);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/MoveNews",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: NewsMoveSucceeded,
        error: AjaxFailed
    }).getAllResponseHeaders();

}
function OpenNewsScript(NewsId, TemplateId) {
    if ($("#NewsCell" + NewsId).attr("groupid") != 0)
        return;
    if (checkDef(TemplateId)) {
        NewsId = NewsId + "&TemplateId=" + TemplateId;
    }
    window.open(serverRoot+'BlockScript.aspx?NewsId=' + NewsId, '_blank', 'width=' + window.screen.availWidth / 2 + ',height=' + window.screen.availHeight / 2 + ',resizable=1');
}
function EditNews(NewsId, GroupId) {
    if (checkURor([17, 20]) == false)
    {
        $("#NewsCell" + NewsId).css("border-left", "4px solid 	#d9534f");
        setTimeout(function () { $("#NewsCell" + NewsId).css("border-left","" ); },500);
        return;
    }

    // console.log('news/NewsEditor.aspx?NewsId=' + NewsId + '&GroupId=' + GroupId);
    var iDiv = CreateFullScreenDiv("FullScreenDiv");

    iDiv.innerHTML = CreateBlockEditorElements();

    $('#FullScreenDiv').append(
       $("<iframe/>")
       .attr("src", serverRoot+'news/NewsEditor.aspx?NewsId=' + NewsId + '&GroupId=' + GroupId + "&rnd=" + parseInt(Math.random() * 10000))
       .attr("id", "NewsEditIframe")
       .attr("test", "test")
       .load(function (e) { setIframeMaxHeigth(e.currentTarget.id) })
       );


 /*   var iframe = document.createElement('iframe');
    iframe.src = ;
    iframe.style.width = '100%';
    iframe.style.height = '100%';
    iframe.id = "NewsEditIframe";
    document.getElementById("FullScreenDiv").appendChild(iframe);*/


    window.scrollTo(0, 0);
    $(".CenralTower").hide();
}
function CloseEditor(ctrl) {
    log("Close Editor");
    if (document.getElementById('FullScreenDiv')) {
       
        $("#FullScreenDiv").fadeOut(500, function () { $("#NewsEditIframe").contents().find("#BESaveBtn").click(); setTimeout(function () { $("#FullScreenDiv").remove() }, 1500) });
       
     /*   var parent = document.getElementById("FullScreenDiv").parentNode;
        parent.removeChild(document.getElementById("FullScreenDiv"));*/
       // 
       
    }
    $(".mainPanelNews").show();
  //  $(".mainHeadMenuItem").first().click();
    ReloadNews();

}
function DeleteNews(NewsId, GroupId, event) {
    event.stopPropagation();
    if (!checkURor([18, 21])) {
        $("#NewsCell" + NewsId).css("border-left", "4px solid 	#d9534f");
        setTimeout(function () { $("#NewsCell" + NewsId).css("border-left", ""); }, 500);
        return;
    }
    NFconfirm(langTable['AlertNewsDeleteConfirm'], document.width>650?event.pageX:54, event.pageY, { NewsId: NewsId, GroupId: GroupId }, DeleteNewsConfirmed);
}
function     DeleteNewsConfirmed(prm){

    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        NewsId: prm.NewsId,
        NewsGroupId: prm.GroupId
    }
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url:  serverRoot+"testservice.asmx/DeleteNews",
        data: JSON.stringify(jdata),
        dataType: "json",
        async: true,
        success: DeleteNewsSucceeded,
        error: AjaxFailed
    });

    //window.open('NewsScript.aspx?BlockId=' + BlockId, '_blank', 'width=335,height=330,resizable=1');
}
function DeleteNewsSucceeded(data) {
    emptyBlocksHeader();
    
    var id = JSON.parse(data.d).id;
    $("#NewsRow" + id).addClass("NewsRowdelete");
    $("#NewsRow" + id).removeClass("NewsRow");
    $("#NewsRow" + id).animate({ opacity: 0 }, 500, function () { $("#NewsRow" + id).remove(); ReloadNews(); });

  //  ShowAlert(langTable['AlertNewsDelete']);
}
function AddNews() {

    console.log("NewsID=" + ProgramDropDown.options[ProgramDropDown.selectedIndex].value);
    console.log(checkURor([16,19]));
    if (checkURor([16, 19]) == false) {
        $("#btnAddNews").css("color", "#d9534f");     
        setTimeout(function () {
            $("#btnAddNews").css("color", "");
            
        }, 500);
        return;
    }
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        NewsId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value,
        NewsGroupId: 0
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/AddNews",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: AddNewsSucceeded,
        error: AjaxFailed
    }).getAllResponseHeaders();

}
function AddNewsSucceeded(data) {
    if (!(typeof JSON.parse(data.d).NewsId === "undefined"))
        EditNews(JSON.parse(data.d).NewsId, 0);
}
function NewsToArchive(sBlockId) {
    alert("здесь будет помещение в архив")
}
function addPersonalFolder()
{
    var jdata = {
        Cookie: getCookie("NFWSession"),
        ProgrammId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value,
        NewsGroupId: 0
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/addPersonalFolder",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: function (data) {
            ReloadNews();
        },
        error: PingFailed
    })
}
function NewsToWord(id)
{
    $("#DownloadIFrame").attr("src", serverRoot+"word/" + id+"/script.rtf");
}
function NewsUpload(toId) {
 
    if (checkURor([17, 20]) == false) {
        $("#NewsCell" + toId).css("border-left", "4px solid 	#d9534f");
        setTimeout(function () { $("#NewsCell" + toId).css("border-left", ""); }, 500);
        return;
    }
    $("#AddForm").modal();
    $("#AddForm").attr("newsId", toId);
    $("#AddFormBlocksBox").html("");
    $(".AddFormGroupBox").html("");
    $(".AddFormProgramsOption").remove();
    $("#AddFormCopyBlockBtn").hide();
    $("#AddFormMoveBlockBtn").hide();
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+"api/ProgrammsListGet.ashx",
        data: JSON.stringify({id:"0"}),
        dataType: "json",
        async: true,
        success: function (dt) {
            
           
            dt.items.forEach(function (data) {
                $("#AddFormPrograms").append('<option class="AddFormProgramsOption" value="' + data.id + '">' + data.name + '</option>');
            });
        }
       
    })

}
function newsToPlayOut(id) {
    if (checkURor([17, 20]) == false) {
        $("#NewsCell" + toId).css("border-left", "4px solid 	#d9534f");
        setTimeout(function () { $("#NewsCell" + toId).css("border-left", ""); }, 500);
        return;
    }
    $("#playoutForm").modal().attr("newsId", id);
    playOutStatusUpdate();
    if ($(".CapPlayoutServersChoose").length < 2)
    {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/playOutGetList",
            data: JSON.stringify({ id: 0 }),
            dataType: "json",
            success: function (data) {
                var items = JSON.parse(data.d);
                $("#CapPlayoutServers").html();
                items.forEach(function (item) {
                    
                    $("#CapPlayoutServers").append("<option value='" + item.id + "' class='CapPlayoutServersChoose'>" + item.title + "</option>");
                });
            },
            error: AjaxFailed
        });
        $("#CapPlayoutServers").change(function () {
            $(this).find("option[value='-1']").remove();
        });
        $("#CapTitleServersSendButton").click(function (e) {
            window.open('/news/newsToTitle.aspx?newsId=' + $(e.currentTarget).closest(".NewsCell").attr("newsId"));
        });
        $("#CapPlayoutServersSendButton").click(function () {

            var $btn = $("#CapPlayoutServersSendButton");
            if (!$btn.hasClass("btn-default"))
                return;
            var serverId = $(".CapPlayoutServersChoose:selected").val();
           
            if (serverId == "-1") {
                $("#CapPlayoutServers").blink();
                return;
            }
            var txt = $btn.val();
            $btn.val("sending...").removeClass("btn-default").addClass("btn-warning").blink();    
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: serverRoot + "testservice.asmx/playOutSend",
                data: (JSON.stringify({ newsid: $("#playoutForm").attr("newsId"), serverid: serverId })),
                dataType: "json",
                success: function (data) {
                    //var items = JSON.parse(data.d);
                    $btn.val("complite").removeClass("btn-warning").addClass("btn-success");
                    setTimeout(function(){$btn.val(txt).removeClass("btn-success").addClass("btn-default")}, 10000);
                    playOutStatusUpdate();
                },
                error: function (d) {
                    AjaxFailed(d);
                    $btn.val("error").removeClass("btn-warning").addClass("btn-danger");
                    setTimeout(function () { $btn.val(txt).removeClass("btn-danger").addClass("btn-default") }, 4000);
                }
            });
        });
        
    }
   

}
function playOutStatusUpdate() {
    console.log("update");
    $("#PlayOutWr").load(serverRoot + "elements/PlayOutTasks.aspx");
}
function AddFormProgrammSelchange()
{
    
    $("#AddFormBlocksBox").html("");
    $(".AddFormGroupBox").html("");
    $("#AddFormCopyBlockBtn").hide();
    $("#AddFormMoveBlockBtn").hide();
    if (parseInt($("#AddFormPrograms").val()) < 0)
        return;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+"api/NewsPlanGetGet.ashx?id=" + ($("#AddFormPrograms").val()),
        data: JSON.stringify({ id: parseInt($("#AddFormPrograms").val()) }),
        dataType: "json",
        async: true,
        success: function (dt) {
            dt.items.forEach(function (data) {
                $("#AddFormPlanBox").append('<div class="AddFormPlanItem" style=""  onclick="AddFormNewsClock(' + data.id + ')">' + data.name + ' <small>' + data.date + '</small></div>');
            });
        }
    });
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+ "api/NewsCurGetGet.ashx?id=" + ($("#AddFormPrograms").val()),
        data: JSON.stringify({ id: parseInt($("#AddFormPrograms").val()) }),
        dataType: "json",
        async: true,
        success: function (dt) {
            dt.items.forEach(function (data) {
                $("#AddFormCurrBox").append('<div class="AddFormPlanItem"  onclick="AddFormNewsClock(' + data.id + ')">' + data.name + ' <small>' + data.date + '</small></div>');
            });
        }
    });
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+"api/NewsPastGetGet.ashx?id=" + ($("#AddFormPrograms").val()),
        data: JSON.stringify({ id: parseInt($("#AddFormPrograms").val()) }),
        dataType: "json",
        async: true,
        success: function (dt) {
            dt.items.forEach(function (data) {
                $("#AddFormPastBox").append('<div class="AddFormPlanItem" onclick="AddFormNewsClock(' + data.id + ')">' + data.name + ' <small>' + data.date + '</small></div>');
            });
        }
    });
}
function AddFormNewsClock(newsId) {
    if (checkURor([16, 19]) == false)
        return;

    $("#AddFormBlocksBox").html("");
    $("#AddFormCopyBlockBtn").hide();
    $("#AddFormMoveBlockBtn").hide();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+"api/BlocksGet.ashx?id=" + (newsId),
        data: JSON.stringify({ id: parseInt(newsId) }),
        dataType: "json",
        async: true,
        success: function (dt) {
           // console.log(dt);
            dt.items.forEach(function (data) {
                $("#AddFormBlocksBox").append('<div class="AddFormBlocksRow"><input type="checkbox" class="AddFormBlocksCheckbox" blockid="' + data.id + '" style="margin-right:10px"/> <div style="display:inline-block;width:130px"><b>' + data.type + '</b></div>' + data.name + '</div>');
               // $("#AddFormPastBox").append('<div style="padding-left: 10px; cursor: pointer;background:white" onclick="AddFormNewsClock(' + data.id + ')">' + data.name + ' <small>' + data.date + '</small></div>');
            });
            $(".AddFormBlocksRow input").click(function (e) {
                e.stopPropagation();
            });
            $(".AddFormBlocksRow").click(function (e) {
                var $row = $(e.currentTarget);
                var $cb = $row.find(".AddFormBlocksCheckbox");
                if ($cb.prop("checked")) {
                    $cb.prop("checked", false)
                }
                else
                    $cb.prop("checked", true)

            });
            if (dt.items.length > 0)
                $("#AddFormBlocksBox").prepend($('<input type="button" class="btn btn-default btn-xs" style="margin-bottom:10px;" value="select all"/>')
                    .click(function () { $(".AddFormBlocksCheckbox").prop("checked", true); })
                    );
            $("#AddFormCopyBlockBtn").show();
            $("#AddFormMoveBlockBtn").show();
        }
    });
}
function AddFormCopyBlock()
{
    var arr = new Array();

    $(".AddFormBlocksCheckbox").each(function (i, e) {
        if ($(e).prop("checked") == true)
        arr.push({ sBlockId: $(e).attr("blockId"), sAfterBlockId: $("#AddForm").attr("newsId") });
    });
    AddFormCopyBlockEach(arr);

}
function AddFormMoveBlock(){
    AddFormCopyBlock();
    var arr = new Array();
    $(".AddFormBlocksCheckbox").each(function (i, e) {
        if ($(e).prop("checked") == true) {
           // sBlockId: $(e).attr("blockId");
            setTimeout(function(){ deleteBlockConfirmed($(e).attr("blockId")); }, 1000);
        }
    });

}
function AddFormCopyBlockEach(arr){

   
    if (arr.length>0)
    {
        var itm = arr.shift();
           
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url:  serverRoot+"testservice.asmx/CopyBlockToNews",
                data: JSON.stringify(itm),
                dataType: "json",
                async: true,
                success: function (data) { AddFormCopyBlockEach(arr); },
                error: function (data) { console.warn(data) },
            });
        }
 
}
function initMainHeadMenu()
{
    $(".mainHeadMenuItem").click(mainHeadMenuItemClick);
    $(".mainHeadMenu").fadeIn(500);
    $(".mainHeadMenuItem").last().addClass("mainHeadMenuItemlast");
    centralTowerActivate();
    rightTowerActivate();
    $(".mainHeadMenuItem").first().click();
}
function mainHeadMenuItemClick(elem)
{
    //console.log(elem);
    if ($(elem.target).hasClass("mainHeadMenuItemActive"))
        return;

    $(".mainHeadMenuItem").removeClass("mainHeadMenuItemActive");
    $(elem.target).addClass("mainHeadMenuItemActive");
    centralTowerActivate();
}
function centralTowerActivate()
{
    $(".CenralTower").hide();
    $("." + $(".mainHeadMenuItemActive").attr("panel")).fadeIn(500, function () {
       // if (!$(".CenralTowerRColWr{").is(":visible"))
        //    setTimeout(function () { $(".CenralTowerRColWr{").fadeIn(500); }, 1000);
    });
    
    if ($(".mainHeadMenuItemActive").attr("panel") == 'mainPanelAdmin' && $(".adminForm").length == 0) {
        $.get(serverRoot + "elements/adminForm.aspx", function (res) { $(".mainPanelAdmin").html($(res).find("#adminMainBox").html()) });
    }else if ($(".mainHeadMenuItemActive").attr("panel") == 'mainPanelStat' && $(".statForm").length == 0) {
        $.get(serverRoot + "elements/statForm.aspx", function (res) { $(".mainPanelStat").html($(res).find("#statFormMainBox").html()) });
    }
 
}
function rightTowerActivate() {
   
  //  fixdiv();
    $('.FixedContainer').css({
        'position': 'fixed',
        'top': 53 + 'px',
        'left': 'calc((100% - 310px) - 10px);'
    });
    $('.FixedContainer').fadeIn(500);
    $(window).scroll(fixdiv);
}
function fixdiv() {

    var $cash = $('.FixedContainer');
    if ($(window).scrollTop() > 0 /*&& ($("RFloatCantainer").is(":visible")==true)*/) {
        //  console.log();
        var pos = 53 - $(window).scrollTop();
        if (pos < 5)
            pos = 5;
      
        $cash.css({
            'position': 'fixed',
            'top': pos + 'px',
            'left': 'calc((100% - 310px) - 10px);'
        });
    }
    if ($(window).scrollTop() > 34) {
        $(".mainHeadMenu").addClass("mainHeadMenuScroll");
        $(".mainHeadMenuItem").addClass("mainHeadMenuItemScroll");

    }
    else {
        $(".mainHeadMenu").removeClass("mainHeadMenuScroll");
        $(".mainHeadMenuItem").removeClass("mainHeadMenuItemScroll");
    }
}
function newsSwipeX(elem, direction) {
    console.log(elem);
   
    if (Math.abs(direction) > window.innerWidth / 4)
    {
       // alert(direction)
        var ch = $(elem).find(".NewsCell");
        if (direction > 0)
            EditNews($(ch).attr("newsid"), $(ch).attr("groupid"))
        else
            OpenNewsScript($(ch).attr("newsid"));
   }

}

function addNewsInline(ctrl) {
    if (checkURor([24]) == false)
        return;
    var field = $(ctrl).siblings("ul").html($(div).loading50());
    NFpost(serverRoot + "testservice.asmx/getCopyBoxNews", { progId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value }, function (e) {
        var arr = JSON.parse(e.d);
        field.html("");
        if (arr.status > 0)
            arr.val.forEach(function (elem) {
                field.append($("<li></li>").append($('<a href="#"></a>')
                    .html(elem.name)
                    .attr("newsId", elem.id)
                    .click(function (e) { addNewsInlineCompl($(e.currentTarget).attr("newsId")) }
                    ))
                    )
            });
    })
}
function addNewsInlineCompl(newsId) {
    NFpost(serverRoot + "testservice.asmx/CopyBoxNewsToNews", { newsId: newsId, progId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value }, function (e) { console.log(e); ReloadNews(); });
}

function newsToTitleOut(newsId) {
   // window.open("/news/newsToTitle.aspx?id="+newsId);
    window.open("/news/newsTitles.html?id=" + newsId);
}
