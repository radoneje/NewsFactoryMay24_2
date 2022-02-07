


$(document).ready(function () {
  
})

function initSubTitleEditor() {

    initSubTitleEditorDo(function () {
        $(".subTitleEditorCurrText")
            .focus()
            .autoHeight()
            .on("change", subTitleAdd)
            .keydown(function (e) {
                mainVideoPlayerButtonControls(e);

            });
    })

    function initSubTitleEditorDo(clbk) {

        if ($(".subTitleEditorCurrText").length > 0) {
            clbk();
            return;
        }
        else {
            setTimeout(function (){initSubTitleEditorDo(clbk)}, 200)
            return;
        }
    }
}



var currPlaingTime = 0;
function onTrackedVideoFrame(currentTime, duration){
    currPlaingTime=(currentTime);
 
}

function subTextEditorOpen(mediaId, blockId) {
    if ($("#panelPlayer").hasClass("subTitleEditor")) {
        subTitleEditorClose();
    }
    if ($("#panelPlayer").hasClass("subTextEditor")) {
        subTextEditorClose();
        return;
    }
    $(".mainVideoPlayerPlayer").addClass("fullWidtPlayer");
    $("#panelPlayer").addClass("subTextEditor");

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/SubBlockData",
        data: "{sBlockId:'" + blockId + "'}",
        dataType: "json",
        success: function (data) {
            var text = JSON.parse(data.d).Text.replace(/\(\(NF[^\)]+\)\)/g, ReloadSubBlockDataReplacer);
            var txt = "";
            for (var i = 0; i < text.length; i++) {
                txt += "<span>" + text[i] + "</span>";
            }
            $(".subTitleEditorBlockText").html(txt);
            $(".subTitleEditorBlockText span").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
          
            })
  
        },
        error: function (e) { console.warn(e); }
    });

    
    console.log("SubTitleTextGet");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/SubTitleTextGet",
        data: JSON.stringify({  blockId: mediaId }),
        dataType: "json",
        success: function (data) {
            console.log("SubTitleTextGet SUCCSSS", data );
            $(".subTextEditorCrowl").val(data.d);
         
        },
        error: function (e) { console.warn(e); }
    });

    $(".subTextEditorCrowl").change(function () {
        
        var text = $(".subTextEditorCrowl");
        var dt = JSON.stringify({ text: $(".subTextEditorCrowl").val(), blockId: mediaId });
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/SubTitleTextSet",
            data: dt,
            dataType: "json",
            success: function (data) {
                console.log("title text change success")
            },
            error: function (e) { console.warn(e); }
        });

    });

    setTimeout(function () {
        //console.log($(".panelPlayer").find('video').height());
        $(".subTitleVideoSize ").height($("#panelPlayer").find('video').height())
        $(".subTitleVideoSize30 ").height($("#panelPlayer").find('video').height() - 30)
        $(".subTextEditorCrowl").focus();
    }, 500)

}

function subTitleEditorOpen(mediaId, blockId) {

   /* $("#panelPlayer").find('video').on(
       "timeupdate",
       function (event) {
           onTrackedVideoFrame(this.currentTime, this.duration);
       });*/
    if ($("#panelPlayer").hasClass("subTextEditor")) {
        subTextEditorClose();
    }
    if ($("#panelPlayer").hasClass("subTitleEditor")) {
        subTitleEditorClose();
    
            return;
    }
    $(".mainTitleEditorAddSec").keydown(function (e) {
        if (e.keyCode == 13 ) {
            e.preventDefault();
            mainTitleEditorAddSec();
          //  console.log("mainTitleEditorAddSec");
        }
    });

    $(".subTitleEditorCurrText")
        .focus()
        .autoHeight()
       // .on("change", subTitleAdd)
       /* .keydown(function (e) {
                mainVideoPlayerButtonControls(e);   
        })*/
        $(".mainVideoPlayerPlayer").addClass("fullWidtPlayer");
        $("#panelPlayer").addClass("subTitleEditor");
        setTimeout(function () {
            //console.log($(".panelPlayer").find('video').height());
            $(".subTitleVideoSize ").height($("#panelPlayer").find('video').height())
            $(".subTitleVideoSize30 ").height($("#panelPlayer").find('video').height() - 30)
            $(".subTitleEditorCurrText").focus();
        }, 500)
    
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/SubBlockData",
            data: "{sBlockId:'" + blockId + "'}",
            dataType: "json",
            success: function (data) {
                var text = JSON.parse(data.d).Text.replace(/\(\(NF[^\)]+\)\)/g, ReloadSubBlockDataReplacer);
                var txt = "";
                for (var i = 0; i < text.length; i++) {
                    txt += "<span>" + text[i] + "</span>";
                }
                $(".subTitleEditorBlockText").html(txt);
                $(".subTitleEditorBlockText span").click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    //initMainTitleLayerblockTextConextMenu();
                })
            },
            error: function (e) { console.warn(e);}
        });

}
function subTitleEditorClose() {
    $("#panelPlayer").removeClass("subTitleEditor");
    $(".mainVideoPlayerPlayer").removeClass("fullWidtPlayer");
   
}
function subTextEditorClose() {
    $("#panelPlayer").removeClass("subTextEditor");
    $(".mainVideoPlayerPlayer").removeClass("fullWidtPlayer");
}
function subTitleAdd(e) {
   

        var txt = $(e.target).val();
        var sec = timeToSeconds($(".subTitleEditorCurrTC").html());
    $(e.target).val("").css("height", "").removeClass("overload");
    $("#subTitleEditorCount").val("");
        var list = subTitleEditorItemToList();
       
        var inserted= ({ time: sec, text:RemoveHTMLTag( txt )});

       

        var find = false;
        list.forEach(function (elem) {
            if(elem.time==inserted.time)
            {
                elem.text+= " " +  inserted.text;
                find = true;
            }
        });
        if (!find)
            list.push(inserted);

      //  list = list.sort(function (a, b) { return b.time - a.time });

        mainTitleLayerToServer(list);

}
function mainTitleLayerToServer(list) {
    $(".subTitleEditorItem").remove();
    var layerId=$(".mainTitleLayerSelect").val();
    serv("mediaGraphisNewList", function (data) {

        data.forEach(subTitleEditorBodyItemAdd);
        //  console.log(window['mainVideoTitleArray']);
        if (typeof (window['mainVideoTitleArray']) != "undefined" || window['mainVideoTitleArray'].length == 0) {
            mainVideoSubTitlesLoad($(".mainMediaBox").attr("mediaId"))

        }
        else {
            for (var i = 0; i < window['mainVideoTitleArray'].length; i++) {
                if (window['mainVideoTitleArray'][i].id == layerId) {
                    window['mainVideoTitleArray'][i].items = data;
                }
            }
        }
     //   console.log(window['mainVideoTitleArray']);
    }, {
        id: layerId,
        mediaId: $(".mainMediaBox").attr("mediaId"),
        items: list
    });
}
function subTitleEditorItemToList() {
    var list = new Array();
    $(".subTitleEditorItem").each(function () {
        var arrTxt = $(this).find(".subTitleEditorText").html();
        var arrSec = timeToSeconds($(this).find(".subTitleEditorTC ").html());
        list.push({ time: arrSec, text: arrTxt });
    });
    return list;
}
function subTitleEditorBodyItemAdd(elem) {
        $(".subTitleEditorBody").prepend($(div)
                    .addClass("subTitleEditorItem stopPropagation")
                    .attr("id", elem.id)
                    .append(
                    $(div)
                        .addClass("subTitleEditorTC stopPropagation")
                        .html(msToTime(elem.timeInSec * 1000)))
                        .click(function (e) {
                            $("#panelPlayer").find('video').get(0).currentTime = timeToSeconds($(e.target).html())
                        })
                     .append(
                        $(div)
                            .addClass("subTitleEditorText stopPropagation")
                            .html(RemoveHTMLTag(elem.text)
                          )
                        )
                        .append($(div)
                        .addClass("subTitleEditorDel stopPropagation")
                        .html(' <span class="glyphicon glyphicon-trash stopPropagation" aria-hidden="true"></span>')
                        .click(subTitleEditorItemDelete)
                        )
    )
  
}
function subTitleEditorItemDelete(e)
{
    e.preventDefault();
    e.stopPropagation();
    NFconfirm(langTable['AlertMediaDeleteConfirm'], event.pageX - 300, event.pageY, $(e.target).closest(".subTitleEditorItem").attr("id"), subTitleEditorItemDeleteConfirmed);
}
function subTitleEditorItemDeleteConfirmed(itemId) {
    console.log(itemId);
    $("#" + itemId).fadeOut(500, function () {
        $(this).remove();
        mainTitleLayerToServer(subTitleEditorItemToList());
    });
   
}
function initMainTitleLayerStringSave() {
   
   
}
function initMainTitleLayerblockTextConextMenu() {

    $(".subTitleEditorBlockText").contextmenu(function (e) {

        var txt = "";
        try {
            var selection = window.getSelection();
           
            $(".subTitleEditorBlockText span").each(function (i, elem) {

                if (selection.containsNode(elem)) {
                    txt += $(elem).html();
                    $(elem).addClass("selected")
                }
                else
                    $(elem).removeClass("selected")
                

            })
            if ($(".subTitleEditorBlockText span.selected").length > 0) {
                $(".subTitleEditorBlockText span.selected").first().prev().addClass("selected")
                $(".subTitleEditorBlockText span.selected").last().next().addClass("selected")
                txt = selection.anchorNode.wholeText + txt + selection.focusNode.wholeText;
            }
            else {
                txt = selection.anchorNode.wholeText;
                $(".subTitleEditorBlockText span.selected").removeClass("selected");
            }

           
            //selection.anchorNode.classList.add("selected");
           // $(selection.anchorNode).addClass("selected")
           // $(selection.focusNode).addClass("selected")
          //  txt = selection.anchorNode.wholeText + txt + selection.focusNode.wholeText;
        }
        catch(e) { console.warn(e)}

      
        txt = txt.replace(/\n/g, ' ');
        var $ctrl;
        if ($("#panelPlayer").hasClass("subTitleEditor")) {
            $ctrl = $(".subTitleEditorCurrText");
        }
        if ($("#panelPlayer").hasClass("subTextEditor")) {
            $ctrl = $(".subTextEditorCrowl");
        }

        $ctrl.val($ctrl.val() + " " + RemoveHTMLTag(txt).replace(/\s$/,""))
        .focus()
        .trigger("input");

        if ($("#panelPlayer").hasClass("subTextEditor")) {
            $ctrl.change();
        }

        e.preventDefault();
    })
}

function initMainTitleLayerSelect() {

        $(".mainTitleLayerSelect").change(function(e){mainTitleListLoad($(e.target).val())});
            serv("graphicsLayerGet", function (data) {
                $(".mainTitleLayerSelect").find('option').remove();
                data.forEach(function (itm) {
                    $(".mainTitleLayerSelect")
                        .append($("<option></option>")
                        .attr("value", itm.id)
                        .html(itm.title)
                        );
                });
                $(".mainTitleLayerSelect").find('option').first().prop("selected", true);
                setTimeout(function(){ mainTitleListLoad($(".mainTitleLayerSelect").val(), $(".mainMediaBox").attr("mediaId"))}, 500);
            });
        }
        
function mainTitleListLoad(layerId, mediaId) {
    if (typeof (layerId) != null && typeof (mediaId) != null && layerId != "undefined" && layerId !=null) {
        serv("mediaGraphisGet", function (data) {
            data.forEach(subTitleEditorBodyItemAdd);
        }, { id: layerId, mediaId: mediaId });
    }

}
function mainTitleEditorAddSec() {
    var val = parseInt($(".mainTitleEditorAddSec").val());
    $(".mainTitleEditorAddSec").val(0);
    if (val == 0)
        return;
    var list = subTitleEditorItemToList();
    for (var i = 0; i < list.length; i++) {
        list[i].time += val;
        if (list[i].time < 0)
            list[i].time = 0;
    }  
    var find = "";
    var newList = new Array();
    list.forEach(function(itm){
        if (itm.time == 0)
            find += itm.text + " ";
        else
            newList.push(itm); 
    });

    if (find.length > 0) {
        newList.push({time:0, text:find});
        list = newList;
    }
    mainTitleLayerToServer(list);
    $(".subTitleEditorCurrText ").focus();

}
