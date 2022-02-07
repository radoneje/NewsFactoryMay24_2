

function InitBlocksDrag(ElementId)
{
    $("#" + ElementId).draggable({
        helper: "clone",
    })
  
}
function InitBlocksDrop(ElementId)
{
    if (parseInt(ElementId) < 0)
    {
        return 0;
    }
    $("#" + ElementId).droppable({
        drop: function (event, ui) {
            DragComplite(ui.draggable.attr("id"),ElementId);
        },
        activate: function (event, ui ) {
            ui.helper.css("border-color", "#bbbbbb").css("background", "opacity", "0.7");
            var color = '#cccccc';
            var rgbaCol = 'rgba(' + parseInt(color.slice(-6, -4), 16)
                + ',' + parseInt(color.slice(-4, -2), 16)
                + ',' + parseInt(color.slice(-2), 16)
                + ',0.5)';
            ui.helper.css('background-color', rgbaCol)
            ;;//делаем при начале переноса
            },
        tolerance: "intersect",
        deactivate: function () {
            //делаем в конце переноса
            $("#" + ElementId).css("border-color", $("#" + ElementId).parent().css("border-color"));
        },
        over: function (event, ui) {
            
            if (checkPossibleDrop(ui.draggable.attr("id"), ElementId, $("#" + ElementId).attr("GroupId")))
                 $("#" + ElementId).css("border-color", "#fab5b5");
           
            
    },
        out: function (event, ui) {
        $("#" + ElementId).css("border-color", $("#" + ElementId).parent().css("border-color"));
        
        },
        accept: ".DragingBlocks",
    });

}

function DragComplite(FromId, ToId)
{
    //console.log("InitBlocksDrop to ID(" + ToId);
    if ((FromId.indexOf("CellBlockName") >= 0) && (ToId.indexOf("CellBlockName") >= 0))
    {
        ChangeBlockPosition(FromId.replace("CellBlockName", ""), ToId.replace("CellBlockName", ""));
        return;
    }
  
    if ((FromId.indexOf("CellBlockName") >= 0) && (ToId.indexOf("CellNewsName") >= 0)) {
        log("bl to news");
        var NewsId = ToId.replace("CellNewsName", "");
        if (NewsId > 0) {
            CopyBlockToNews(FromId.replace("CellBlockName", ""), NewsId);
        }
    }

    if ((FromId.indexOf("CellBlockName") >= 0) && (ToId.indexOf("CellNewsName") >= 0))
    {
        var NewsId = ToId.replace("CellNewsName", "");
        if(NewsId>0)
        {
            CopyBlockToNews(FromId.replace("CellBlockName",""), NewsId);
        }
    }
    

}

function ChangeBlockPosition(SourceId, DestId)
{
    if (checkURor([24]) == false)
        return;
    //log("change pos " + SourceId + " " + DestId);
    // if (SourceId != DestId) 
    {
        var jdata = {
            sBlockId: SourceId,
            sAfterBlockId: DestId
        };

        if ($("#BlockNameRowContainer" + SourceId).attr('bgcolor') != 0) {

            // ищем первый блок
            var preElemId = DestId;
            var req = new Array();
            
            $('.BlockNameRowContainer').each(function (i, elem) {
                
                if ($(elem).attr('bgcolor') == $("#BlockNameRowContainer" + SourceId).attr('bgcolor'))
                {
                   
                    req.push({ sBlockId: $(elem).attr('blockid'), sAfterBlockId: preElemId });
                    preElemId = $(elem).attr('blockid');
                    //console.log("copy src " + $(elem).attr('blockid') + " dest "+ preElemId);
                   /* 
                    preElemId = $(elem).attr('blockid');*/
                    
                }

            });
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url:  serverRoot+"testservice.asmx/BlockMoveGroup",
                data: JSON.stringify({arr:req}),
                dataType: "json",
                async: true,
                success: BlockMoveSucceeded,
                error: AjaxFailed
            });
        }
        else
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/MoveBlock",
            data: JSON.stringify(jdata, null, 2),
            dataType: "json",
            async: true,
            success: BlockMoveSucceeded,
            error: AjaxFailed
        }).getAllResponseHeaders();

       

    }
}
function BlockMoveSucceeded(data) {
    PingServer();
    ShowAlert("Блок перенесен успешно!");
    return;
}
function CopyBlockToNews(SourceId, DestId)
{
    log("CopyBlockToNews");
    if (SourceId != DestId) {
        var jdata = {
            sBlockId: SourceId,
            sAfterBlockId: DestId
        };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/CopyBlockToNews",
            data: JSON.stringify(jdata, null, 2),
            dataType: "json",
            async: true,
            success: BlockCopyToNewsSucceeded,
            error: AjaxFailed
        }).getAllResponseHeaders();
    }
}
function CopyBlockGroupToNews(SourceId, DestId) {
    log("CopyBlockGroupToNews");
    if (SourceId != DestId) {
        var jdata = {
            sBlockId: SourceId,
            sAfterBlockId: DestId
        };
        log(jdata);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/CopyBlockGroupToNews",
            data: JSON.stringify(jdata, null, 2),
            dataType: "json",
            async: true, 
            success: function (data) { log(data), BlockCopyToNewsSucceeded },
            error: AjaxFailed
        }).getAllResponseHeaders();
    }
}

function BlockCopyToNewsSucceeded(data) {
    PingServer();
    ShowAlert("Блок скоприрован в выпуск успешно!");
        return;
}
function checkPossibleDrop(idFrom, idTo, GrouoId)
{
    if(idFrom.indexOf("CellBlockName")>=0)
    {
        if (idTo.indexOf("CellBlockName") >= 0)
            return true;
        if (idTo.indexOf("CellNewsName") >= 0)
            return true;
    }

    return false;
}
/////////////
function InitNewsDrag(ElementId)
{
    $("#" + ElementId).draggable({
        helper: "clone",
    })
}
function InitNewsDrop(ElementClass, ElementId)
{
    $("." + ElementClass).droppable({
        drop: function (event, ui) {

            if ((ui.draggable.attr("GroupId").replace("CellNewsGroup", "") == 0) || (ElementClass.replace("CellNewsGroup", "") == 0))// проверка, что бы не копировалось между копилкой и шаблонами
                dragNewsComplite(ui.draggable.attr("id").replace("CellNewsName", ""), ElementClass.replace("CellNewsGroup", ""));
                
        },
        activate: function (event, ui) {
            ui.helper.css("border-color", "#bbbbbb").css("background", "opacity", "0.7");
            var color = '#cccccc';
            var rgbaCol = 'rgba(' + parseInt(color.slice(-6, -4), 16)
                + ',' + parseInt(color.slice(-4, -2), 16)
                + ',' + parseInt(color.slice(-2), 16)
                + ',0.5)';
            ui.helper.css('background-color', rgbaCol)
            $(ui.helper).children(".divnewsbuttons").remove;
            ;;//делаем при начале переноса
        },
        deactivate: function () {
            //делаем в конце переноса
            $("." + ElementClass).css("border-color", $("." + ElementClass).parent().css("border-color"));
        },
        over: function (event, ui) {
            
            if (ui.draggable.attr("GroupId") != ElementClass)// проверка копирования в разные группы
            {
                if ((ui.draggable.attr("GroupId").replace("CellNewsGroup", "") == 0) || (ElementClass.replace("CellNewsGroup", "") == 0))// проверка, что бы не копировалось между копилкой и шаблонами
                    $("." + ElementClass).css("border-color", "#fab5b5");
            }


        },
        out: function (event, ui) {
            
            $("." + ElementClass).css("border-color", $("." + ElementClass).parent().css("border-color"));

        },
        accept: ".drappableNews",

    });
    
}
function checkPossibleNewsDrop(GroupFrom, GroupTo)
{
    if (!checkDef(GroupFrom))
        return true;
    var FromId = GroupFrom.replace("CellNewsGroup", "");
    var toId = GroupTo.replace("CellNewsGroup", "");
    var ret = false;
    if (FromId != toId)
    {
        if (FromId > 0 && toId == 0) {   
            return true;
        } 
        if (FromId == 0) {
            return true;
        }
    }
    
    return false;
}
function dragNewsComplite(sFromId, sToGroupId)
{
     
        var jdata = {
            sSourceId: sFromId,
            sDestGroupId: sToGroupId,
            sDestProgramId:ProgramDropDown.options[ProgramDropDown.selectedIndex].value,
            Coockie: getCookie("NFWSession")

        };
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

///////////
function DaDInitDragByClass(elementClassName)
{
    $("." + elementClassName).draggable({
        helper: "clone",
    })
}
function DaDInitDragByElement(element) {
   
    //
    $(element).draggable({
        helper: "clone",
    })
}
function DaDInitDropByElement(element) {
  //  
    //
    var ElementId = $(element).attr("id");
    
    $(element).droppable({
        tolerance: "pointer",
        activate: function (event, ui) {
            ui.helper.css("width", "260px").css("border-color", "#bbbbbb").css("background", "opacity", "0.7").css('z-index', 2000);
            if ($(ui.draggable).attr("id").indexOf("Block") >= 0)
            {
                ui.helper.css("width", "540px");
            }
            var color = '#cccccc';
            var rgbaCol = 'rgba(' + parseInt(color.slice(-6, -4), 16)
                + ',' + parseInt(color.slice(-4, -2), 16)
                + ',' + parseInt(color.slice(-2), 16)
                + ',0.5)';
            ui.helper.css('background-color', rgbaCol)
            if ($(ui.draggable).attr("id").indexOf("Block") >= 0) {
                //  $(".BlockNameRowFirstContainer").addClass("DaDHiLight");
                $('.blockItem').addClass("DaDHiLight");
                $(".NewsCell").addClass("DaDNewsHiLight");
            }
           // log("activate" + $(ui.draggable).attr("id").indexOf("News") + " " + $(ui.draggable).attr("GroupId"));

            if ($(ui.draggable).attr("id").indexOf("News") == 0) {
                
                $(".BlockNameRowFirstContainer").addClass("DaDHiLight");
                $(".NewsGroup").addClass("DaDHiLight");
                if($(ui.draggable).attr("GroupId")==0)
                     $(".ArchiveNewsContainer").addClass("DaDHiLight");
                
            }
            if ($(ui.draggable).attr("id").indexOf("ArchiveNews") == 0) {

                // $(".BlockNameRowFirstContainer").addClass("DaDHiLight");
                $(".NewsGroup").addClass("DaDHiLight");

            }
            if ($(ui.draggable).attr("id").indexOf("RssItemTitle") == 0) {// из ленты
                $(".NewsCell").addClass("DaDHiLight");
            }
            if ($(ui.draggable).attr("id").indexOf("LentaBlockTitle") == 0) {// из ленты
                $(".NewsCell").addClass("DaDHiLight");
            }
            
            ;;//делаем при начале переноса
        },
        deactivate: function () {
            $("*").removeClass("DaDHiLight");
            $(".NewsGroup").removeClass("DaDHiLight");
        },
        over: function (event, ui) {
            
        log("over");

            if ($(ui.draggable).attr("id").indexOf("Block") >= 0 && $(element).attr("id").indexOf("Block") >= 0 )
                $(element).addClass("DaDOver");// из блоков в блоки

            if (($(ui.draggable).attr("id").indexOf("RssItemTitle") == 0 || $(ui.draggable).attr("id").indexOf("LentaBlockTitle")==0) && $(element).attr("id").indexOf("NewsContent") == 0) {
                $(element).addClass("DaDOver");
            }//из ленты в ньюса
            
            if ($(ui.draggable).attr("id").indexOf("Block") >= 0 && $(element).attr("id").indexOf("NewsContentName") >= 0) {

                $(element).addClass("DaDOver");// 
            }// из блоков в ньюса

            if ($(ui.draggable).attr("id").indexOf("NewsContentName") >= 0 && $(element).attr("id").indexOf("NewsGroup") >= 0) {
                var FromId = $(ui.draggable).attr("id").replace("NewsContentName", "");
                
                if ($("#NewsCell" + FromId).attr("GroupId") == 0 && $(element).attr("GroupId") > 0)
                    $(element).addClass("DaDOver");//из ньюсов в копилки
                if ($("#NewsCell" + FromId).attr("GroupId") > 0 && $(element).attr("GroupId") == 0)
                    $(element).addClass("DaDOver");//из  копилки в ньюса
            }
            if ($(ui.draggable).attr("id").indexOf("Block") >= 0 && $(element).attr("id").indexOf("NewsContent") >= 0)
                $(element).addClass("DaDOver");//из ньюсов в ньюса
            
            if ($(ui.draggable).attr("id").indexOf("NewsContentName") == 0 && $(element).attr("id").indexOf("ArchiveNewsContainer") >= 0) {
                var FromId = $(ui.draggable).attr("id").replace("NewsContentName", "");
                if ($("#NewsCell" + FromId).attr("GroupId")==0)
                    $(element).addClass("DaDOver");//из ньюсов  в архив
            }

            if ($(ui.draggable).attr("id").indexOf("ArchiveNewsItem") == 0 && $(element).attr("id").indexOf("NewsGroup") == 0) {
                var FromId = $(ui.draggable).attr("id").replace("NewsContent", "");
                //if ($(element).attr("GroupId") == 0)
                    $(element).addClass("DaDOver");//из архива в ньюса
            }

        },
        out: function (event, ui) {
            $(element).removeClass("DaDOver");
        },
        drop: function (event, ui) {
            $("*").removeClass("DaDOver");
            $(".BlockNameRowFirstContainer").removeClass("DaDHiLight");
            DaDDragDropping(ui.draggable, element);
        },
    });
}
function DaDDragDropping(elementFrom, elementTo) {


    if (elementFrom.attr("id").indexOf("Block") >= 0 && elementTo.attr("id").indexOf("Block") >= 0) {// ресорт блоки
       // log("----->" + elementFrom.attr("id") + " " + ToId);
        var FromId = elementFrom.attr("id").replace("BlockContent", "");
        var ToId = elementTo.attr("id").replace("BlockNameRowFirstContainer", "");
        if (FromId != ToId) {
         //   log("-->" + FromId + " " + ToId);
            ChangeBlockPosition(FromId, ToId);
            return;
        }
    }
    if ((elementFrom.attr("id").indexOf("Block") >= 0) && (elementTo.attr("id").indexOf("NewsContent") >= 0)) {// копируем блоки
      
        var FromId = elementFrom.attr("id").replace("BlockContent", "");
        var ToId = elementTo.attr("id").replace("NewsContent", "");
        if (ToId > 0) {
           // Debug;
            if ($("#BlockNameRowContainer" + FromId).attr("bgcolor") == null || $("#BlockNameRowContainer" + FromId).attr("bgcolor") == 'undifined') {
                CopyBlockToNews(FromId, ToId);
             //   console.log("no group");
            }
            else {
                CopyBlockGroupToNews(FromId, ToId);
               // console.log(" group");
                /*$(".BlockNameRowContainer[bgcolor='" + $("#BlockNameRowContainer" + FromId).attr("bgcolor") + "']").each(function (a, b) {

                    CopyBlockToNews($(b).attr("id").replace("BlockNameRowContainer", ""), ToId);
                    console.log(b);
                });*/

            }
        }
    }
    if (elementFrom.attr("id").indexOf("NewsContent") >= 0 && elementTo.attr("id").indexOf("NewsGroup") >= 0) {// копируем выпуски
        var FromId = elementFrom.attr("id").replace("NewsContentName", "");
        var ToId = elementTo.attr("id").indexOf("NewsGroup");
        if (FromId != ToId) {
            var GroupId = $(elementTo).attr("GroupId");
            //  log("eto!!!" + "#NewsGroup" + elementTo.attr("id"));
            if (($("#NewsCell" + FromId).attr("GroupId") == 0 && $(elementTo).attr("GroupId") > 0) ||
                ($("#NewsCell" + FromId).attr("GroupId") > 0 && $(elementTo).attr("GroupId") == 0)) {
                console.log("copy to copybox");
                DaDCopyNews(FromId, GroupId);
                return;
            }

        }
    }

    if (elementFrom.attr("id").indexOf("NewsContentName") >= 0 && elementTo.attr("id").indexOf("ArchiveNewsContainer") >= 0) {

        var FromId = elementFrom.attr("id").replace("NewsContentName", "");
        var ToId = elementTo.attr("id");
        //log("NTA " + FromId + " " + ToId + " FromGr " + $("#NewsCell" + FromId).attr("GroupId"));
        if ($("#NewsCell" + FromId).attr("GroupId") == 0) {
            DaDToArchiveToNews(FromId)//из ньюсов  в архив
        }

    }

    if (elementFrom.attr("id").indexOf("RssItemTitle") == 0 && (elementTo.attr("id").indexOf("NewsContent")==0)) {// из ленты в ньса
       
        var FromId = elementFrom.attr("id").replace("RssItemTitle", "");
        
        var ToId = elementTo.attr("id").replace("NewsContent", "");
        if (ToId > 0) {
            DaDCopyRssToNews(FromId, ToId);
        }
    }

    if (elementFrom.attr("id").indexOf("LentaBlockTitle") == 0 && (elementTo.attr("id").indexOf("NewsContent") == 0)) {// из ленты в ньса

        var FromId = elementFrom.attr("id").replace("RssItemTitle", "");

        var ToId = elementTo.attr("id").replace("NewsContent", "");
        if (ToId > 0) {
            DaDCopyRssToNews(FromId, ToId);
        }
    }
   
}

function DaDCopyRssToNews(FromId, ToId)
{
    log("CopyRssToNews");
    var jdata = {
        sSourceId: FromId,
        sDestGroupId: ToId,
        sDestProgramId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value,
        Coockie: getCookie("NFWSession")

    };
    log(jdata);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/RssToNews",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: NewsMoveSucceeded,
        error: AjaxFailed
    }).getAllResponseHeaders();
}
//////////////function
var DragSource;
function HTMLFileDropInitByClass(ClassName)
{
   // console.log($("." + ClassName).length);
    $("." + ClassName).each(function () {/* log("-->" + $(this).attr("id"));*/ HTMLFileDropInitFF($(this).attr("id")); });
}
function HTMLFileDropInitFF(elementId) {
   
  //  console.log("HTMLFileDropInit");
    var dropZone = document.getElementById(elementId);
    dropZone.droppable = true;
    dropZone.addEventListener('dragstart', handleDragStart, false);
    dropZone.addEventListener('dragenter', handleDragEnter, false);
    dropZone.addEventListener('dragover', handleDragOver, false);
    dropZone.addEventListener('dragleave', handleDragLeave, false);
    dropZone.addEventListener('dragend', handleDragEnd, false);
    dropZone.addEventListener('drop', handleFileSelect, false);
}

function handleDragStart(evt) {
   
    DragSource = evt.target;

    


}
function handleDragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
   
}
function handleDragLeave(evt) {
   
    event.stopPropagation();
    event.preventDefault();
    $(".DaDOver").removeClass("DaDOver");

    return false;
}
function handleDragEnd(evt) {
   
    $(".DaDHiLight").removeClass("DaDHiLight");
    DragSource = null;
}
var dragging = 0;
function handleDragEnter(evt) {
    evt.stopPropagation();
    evt.preventDefault();
   
    $(evt.target).addClass("DaDOver");
 
        /////////
        ///////
    ////////// 
}
function handleFileSelect(evt) {
    $(".DaDOver").removeClass("DaDOver");
    evt.stopPropagation();
    evt.preventDefault();

   
    var files = evt.dataTransfer.files;
    var output = [];
    var blockId = $(evt.target).attr("blockId");
    if (typeof (blockId)=="undefined" || blockId.lenght==0 || blockId==0)
        blockId = $(evt.target).closest("[blockId]").attr("blockId");

    if (typeof (newFileUpload) != "undefined")
        newFileUpload(files, blockId);
    else
        window.parent.newFileUpload(files, blockId);

    for (var i = 0, f; f = files[i]; i++) {
       
        if (f.size > 0) {
            //new Worker(
             //  AddFileToUpload(f, evt.target.id.replace("BlockImageControl", ""))
            
           //     );
        }
        
    }
    
    //document.getElementById('list').innerHTML = '<ul>' + output.join('') + '</ul>';
}

window.addEventListener("dragover", function (e) {
    e = e || event;
    e.preventDefault();
}, false);
window.addEventListener("drop", function (e) {
    e = e || event;
    e.preventDefault();
}, false);

