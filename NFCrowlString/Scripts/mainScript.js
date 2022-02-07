const maxLen = 50;
var clientId = Math.random() * 100000;

$().ready(function () {
    $(".playlistDropDown").change(function () {
        $(".stringWr").load("/home/playList/" + $(this).val(), function () { });
    });
    $('body').click(function (e) {
        if (!($(e.target).hasClass("activeText") || $(e.target).hasClass("activeTextEditor") || $(e.target).hasClass("findedText"))) {
            $(".activeTextEditor").each(function () {
                var _this = this;
                //saveItemText($(_this).closest(".playListItem").attr("id"));
                closeTextEditor($(_this).closest(".playListItem").attr("id"));
            });
        }
    });
    var findCtrl = $(".findWr").find("input[type='text']");
    findCtrl
        .keyup(function (e) { onFind(findCtrl.val()); if (e.keyCode == 13) { return false; } })
        .change(onFind(findCtrl.val()))
         .on("paste", onFind(findCtrl.val()));
    setTimeout(function () { $('#playlistClientIdHidden').val(clientId) }, 1000);
});


$.fn.autoHeight = function () {
    var _this = this;
    var resizeTextarea = function (el) {
        var offset = el.offsetHeight - el.clientHeight;
        jQuery(el).css('height', el.scrollHeight + offset);
    };
    jQuery(this).on('keyup input', function () { resizeTextarea(this); });
    return this;
}
$.fn.slowRemove = function (callBack) {
    var _this = this;
    $(_this).fadeOut(500, function () { $(_this).remove(); if (typeof (callBack) == "function") callBack(); });
    return _this;
}
function scrollToElement(elem, offset) {
    if (typeof (offset) == 'undefined')
        offset = 0;
    var pos = $(elem).offset().top;
    //   console.log(pos);
    if (pos <= 60)
        pos = pos - 60;

    $('html, body').animate({
        scrollTop: pos + offset
    }, 500);
}

function passiveItemActivate(itemId) {
    $('.strPassiveBox').find('#' + itemId).addClass('forRemove');
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/PassiveItemActivate",
        data: JSON.stringify({ itemId: itemId, clientId: clientId }),
        dataType: "html",
        async: true,
        success: function (data) {
            $('.strPassiveBox').find('#' + itemId).slowRemove(); $('.strActiveBox').prepend(data);
            calculateActiveItems();
            var txtToFind = $(".findWr").find("input").val();
            if (txtToFind != null || txtToFind.length > 0)
                onFind(txtToFind);
        },
        error: function (e) { if (e.status == 200) {  /*scrollToElement($('.activeItem').first(), -50)*/ } else { console.warn("ERROR PassiveItemActivate"); console.warn(e); } }
    })

}
function changeItemText(itemId) {
  

    var ctrl = $("#" + itemId).find(".activeText");
    if ($(ctrl).find(".activeTextEditor").length > 0)
        return saveItemText(itemId);
  
    var txt = RemoveHTMLTag($(ctrl).html());
    $(ctrl).html("").append($("<textarea></textarea")
        .val(txt.trim())
        .addClass("activeTextEditor")
        .keydown(function (e) { if (e.keyCode == 13) { saveItemText($(e.target).closest(".playListItem").attr("id")) } })
        .change(function (e) { { saveItemText($(e.target).closest(".playListItem").attr("id")) } })
        .keydown(inputKeyDown)
        .on("paste", function (e) { console.log(e); setTimeout(inputKeyDown(e),200) })
        );
    
    $(ctrl).find(".activeTextEditor").focus();

    checkInputLen($(ctrl).find(".activeTextEditor"));
}
function closeTextEditor(itemId) {
    var ctrl = $("#" + itemId).find(".activeText");
    var txt = $(ctrl).find(".activeTextEditor").val();
    $(ctrl).html(txt);
    var txtToFind = $(".findWr").find("input").val();
    if (txtToFind!=null || txtToFind.length>0)
        onFind(txtToFind);
    return txt;
}
function saveItemText(itemId) {
    var ctrl = $("#" + itemId).find(".activeText");
   /* if ($(ctrl).find(".activeTextEditor").length == 0)
        return changeItemText(itemId);*/
    var txt = closeTextEditor(itemId);
    txt = txt.trim();
    if (txt.length == 0)
    {
        var id = '#' + 'PassiveItemDelete' + itemId;
        $(id).click();
        return;
    }

    $("#" + itemId).addClass("newItem");
    setTimeout(function () { $("#" + itemId).removeClass("newItem") }, 1000)
    /*ajax to server*/
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/itemChangeText",
        data: JSON.stringify({ itemId: itemId, text: txt, clientId: clientId }),
        dataType: "json",
        async: true,
        success: function (data) { console.log(data); },
        error: function (e) { { console.warn("ERROR saveItemText"); console.warn(e); } }
    })
}
function activeHeaderClick() {
    if ($(".strActiveBoxCollapsed").length > 0) {
        $(".strActiveBoxCollapsed").removeClass("strActiveBoxCollapsed");
        return;
    }
    $(".strActiveWr").addClass("strActiveBoxCollapsed");

}
function inputKeyDown(event)
{
    var ctrl = $(event.target);
    
    checkInputLen(ctrl)
}
function checkInputLen(ctrl) {
    if ($(ctrl).val().length > maxLen)
        $(ctrl).addClass("ctrlOverflow");
    else
        $(ctrl).removeClass("ctrlOverflow");
}
function playlistItemClone(itemId) {
 
   // var itemId = $(ctrl).closest(".playListItem").attr("id");

   $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/itemClone",
        data: JSON.stringify({ itemId: itemId, clientId: clientId }),
        dataType: "html",
        async: true,
        success: function (data) {
            $('.strPassiveBox').prepend(data);
            var first = $('.strPassiveBox').find(".playListItem").first();
            $(first).addClass("newItem");
            setTimeout(function () { $(first).removeClass("newItem") }, 1000);
            var txtToFind = $(".findWr").find("input").val();
            if (txtToFind != null || txtToFind.length > 0)
                onFind(txtToFind);
        },
        error: function (e) { { console.warn("ERROR itemClone"); console.warn(e); } }
    });
 
}
function activeItemUp(ctrl) {
    var wr = $(ctrl).closest(".activeItem");
    

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/activeItemUp",
        data: JSON.stringify({ itemId: $(wr).attr("id"), clientId: clientId }),
        dataType: "JSON",
        async: true,
        success: function (data) {
            $(wr).insertBefore($(wr).prev());
            $(wr).addClass("newItem");
            setTimeout(function () { $(wr).removeClass("newItem") }, 1000);
            var txtToFind = $(".findWr").find("input").val();
            if (txtToFind != null || txtToFind.length > 0)
                onFind(txtToFind);
        },
        error: function (e) { { console.warn("ERROR itemClone"); console.warn(e); } }
    });
   // console.log(wr)
};
function activeItemDown(ctrl) {
    var wr = $(ctrl).closest(".activeItem");

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/activeItemDown",
        data: JSON.stringify({ itemId: $(wr).attr("id"), clientId: clientId }),
        dataType: "JSON",
        async: true,
        success: function (data) {
            $(wr).insertAfter($(wr).next());
            $(wr).addClass("newItem");
            setTimeout(function () { $(wr).removeClass("newItem") }, 1000);
            var txtToFind = $(".findWr").find("input").val();
            if (txtToFind != null || txtToFind.length > 0)
                onFind(txtToFind);
        },
        error: function (e) { { console.warn("ERROR itemClone"); console.warn(e); } }
    });
};
function passiveTimerLoad(itemId) {
  //  console.log($("#" + itemId).find(".timerBox").length);
    if ($("#" + itemId).find(".timerBox").length == 0)
        $("#" + itemId).find(".passiveTimerWr").load("/home/passiveTimer/" + itemId);
    else
        $("#" + itemId).find(".timerBox").slowRemove();
}

function timerItemIsEnd(ctrl) {

    var id = $(ctrl).closest(".timerItem").attr("id");
    var checked = ($(ctrl).attr('checked') || false); checked = !checked; $('#'+id).find('.timerItemEndDate').prop('readonly', checked)
}
function timerItemAdd(ctrl) {
    var wr = $(ctrl).closest(".passiveItem");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/timerItemAdd",
        data: JSON.stringify({ itemId: $(wr).attr("id"), clientId: clientId }),
        dataType: "html",
        async: true,
        success: function (data) {
            $(wr).find(".timerBoxItems").prepend(data);
            $(wr).find(".passiveControlBtnTimer").addClass("passiveControlBtnTimerState_Yes");
        },
        error: function (e) { { console.warn("ERROR itemClone"); console.warn(e); } }
    });
}
function timerItemSectDelete(ctrl) {
    var id = $(ctrl).closest(".timerItem").attr("id");
    var cont = $(ctrl).closest(".passiveItem");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/home/timerItemDelete",
        data: JSON.stringify({ timerId: id, clientId: clientId }),
        dataType: "json",
        async: true,
        success: function (data) {
            $("#" + data.timerId).slowRemove(function () {              
                if ($(cont).find(".timerItem").length == 0) {
                    $(cont).find(".passiveControlBtnTimerState_Yes").removeClass("passiveControlBtnTimerState_Yes");
                }
            });
        },
        error: function (e) { { console.warn("ERROR itemClone"); console.warn(e); } }
    });
}
function calculateActiveItems() {
    $(".activeCount").html($(".activeItem").length);
}
function RemoveHTMLTag(text) {
    var div = document.createElement("div");
    div.innerHTML = text;
    return div.textContent || div.innerText || "";

}
function onFind(pattern) {
    pattern = pattern.trim();
    $(".findedItem").each(function () {
        var _this = this;
        var toFind = $(_this).find(".activeText").html();
        toFind = toFind.trim();
        
        $(_this).find(".activeText").html(RemoveHTMLTag(toFind));
    });
    $(".findedItem").removeClass("findedItem");
   
    
    if (pattern.length>0)
    $(".activeText").each(function () {
        var _this = this;
        var toFind = $(_this).html();
        toFind = toFind.trim();
        var r = new RegExp(pattern, "g");
        if (r.exec(toFind)!=null) {
            $(_this).closest(".playListItem").addClass("findedItem");

            toFind=toFind.replace(r, "<span class='findedText'>" + pattern + "</span>");
            $(_this).html(toFind);
        }
        

    });
    $(".findedText").click(function () {
     
        var ctrl = $(this).closest(".passiveItem");
        if (ctrl.length > 0) {
            
            if ($(ctrl).find('.activeTextEditor').length == 0) {
                console.log("changeItemText");
                changeItemText($(ctrl).attr("id"));
            }
        }
    });
}




