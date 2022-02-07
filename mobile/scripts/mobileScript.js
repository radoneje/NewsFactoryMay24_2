var guid = "";
var collapsed = true;
var blockCollapsed = new Object();

setInterval(refreshBlocks, 1000 * 60 * 2);
setInterval(onPrgChange, 1000 * 60 * 10);

$().ready(function () {

    $('#LBox').bind('heightChange', function () {
        $('#RBox').css("min-height", $('#LBox').height());
    });
});


function onLogin() {
    if ($("#tLogin").val().length < 1 || $("#tPass").val().length < 1)
        return noLogin();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/handlers/login.ashx",
        data: JSON.stringify({ user: $("#tLogin").val(), pass: $("#tPass").val() }),
        dataType: "json",
        async: true,
        error: function (dt) { console.warn(dt); noLogin(); },
        success: loginAjaxSuccess
    });

}
function noLogin() {
    $("#cWrong").slideDown(500);
    $("#tPass").val('');
    $("#tPass").focus();
    r = window.localStorage.setItem("mNF24Pass", null);
    return false;
}
function loginAjaxSuccess(data) {
    console.log(data);
    if (data.state != "ok") {
        return noLogin();
    }
    guid = data.guid;
    window.localStorage.setItem("mNF24User", $("#tLogin").val());
    window.localStorage.setItem("mNF24Pass", $("#tPass").val());
    window.localStorage.setItem("mNF24Guid", guid);
    $(".loginForm").fadeOut(500, function () {
        $("#MainForm").fadeIn(500);
    });
    init();
}
function showLogin() {
    setTimeout(function () {
        $("#MainForm").fadeOut(500, function () {
            $(".loginForm").fadeIn(500);
        });
    }, 1000);
    clearAll();
}
function clearAll() {
    $(".newsGroupContent").html('');
    $("#prSel").unbind("change");
    $("#prSel").html('');
    $("#BlocksBox").html('');
    $("#BlocksBox").attr('newsid', null);
    $("#RBoxHeader").hide();
    $("#LBox").trigger('heightChange');
}
function init() {
    clearAll();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/handlers/programsGet.ashx",
        data: JSON.stringify({ guid: guid }),
        dataType: "json",
        async: true,
        error: function (e) { console.warn(e); showLogin(); },
        success: function (e) {
            if (e.state != "ok")
                return showLogin();
            $("#prSel").append('<option value="-1" selected>Выбор программы</option>');
            e.items.forEach(function (itm) {
                $("#prSel").append('<option value="' + itm.id + '" >' + itm.name + '</option>');
            });
            $("#prSel").change(onPrgChange);
        }
    });
}
function onPrgChange() {
    //$("#prSel").child().each(function (a, b) { console.log(b); });
    $("#prSel option[value='-1']").remove();
    $("#BlocksBox").html('');
    $("#BlocksBox").attr('newsid',null);
    $("#RBoxHeader").hide();
    $(".newsGroupContent").html("");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/handlers/newsPastGetGet.ashx",
        data: JSON.stringify({ guid: guid, id: $("#prSel").val() }),
        dataType: "json",
        async: true,
        error: function (e) { console.warn(e);  },
        success: function (e) {
            if (e.state != "ok")
                return showLogin();
            e.items.forEach(function (itm) {
                $("#pastNewsBox").append('<div class="newsItem" id="' + itm.id + '" onclick="onClickNews(this.id)">' + itm.name + '  <small>' + itm.date + '</small></div>');
            });
            $("#LBox").trigger('heightChange');
        }
    });
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/handlers/newsPlanGetGet.ashx",
        data: JSON.stringify({ guid: guid, id: $("#prSel").val() }),
        dataType: "json",
        async: true,
        error: function (e) { console.warn(e);  },
        success: function (e) {
            if (e.state != "ok")
                return showLogin();
            e.items.forEach(function (itm) {
                $("#planNewsBox").append('<div class="newsItem" id="' + itm.id + '" onclick="onClickNews(this.id)">' + itm.name + '  <small>' + itm.date + '</small></div>');

            });
            $("#LBox").trigger('heightChange');
        }
    });
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/handlers/newsCurrGetGet.ashx",
        data: JSON.stringify({ guid: guid, id: $("#prSel").val() }),
        dataType: "json",
        async: true,
        error: function (e) { console.warn(e);  },
        success: function (e) {
            if (e.state != "ok")
                return showLogin();
        
            $('.delet').addClass('delet');
            e.items.forEach(function (itm) {
                if ($('#' + itm.id).length == 0) {
                    $("#currNewsBox").append('<div class="newsItem" id="' + itm.id + '" onclick="onClickNews(this.id)">' + itm.name + '  <small>' + itm.date + '</small></div>');
                }
                else {
                    $('#' + itm.id).html(itm.name + '  <small>' + itm.date + '</small>');
                }
                $('#' + itm.id).removeClass('delet');
            });
            $('.delet').remove();
            $("#LBox").trigger('heightChange');
        }
    });


}
function onClickNews(id) {
    updateNews(id);
    /* var target = $(this).attr('href'),
        wWidth = $(window).width(),
        destination = wWidth < 767 ? $(target).offset().top - 125 : $(target).offset().top - 180;
    $('html, body').animate({scrollTop: destination }, 900);*/
    if($(window).width()<768)
    {
        $('html, body').animate({ scrollTop: $("#RBoxHeader").offset().top }, 500);
    }
}
function updateNews(id)
{
    $("#RBoxHeader").fadeIn(500);
    $("#bRefresh").focus();
    $(".newsItem").css("background", "");
    $("#" + id).css("background", "lightgreen");
    $("#BlocksBox").attr('newsid', id);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/handlers/blocksCurrGetGet.ashx",
        data: JSON.stringify({ guid: guid, id: id }),
        dataType: "json",
        async: true,
        error: function (e) { console.warn(e); /*showLogin();*/ },
        success: function (e) {
            if (e.state != "ok")
                return showLogin();
            $("#BlocksBox").html('');
            $('.blockItem').addClass('del');
            e.items.forEach(function (itm) {
               // $("#currNewsBox").append('<div class="newsItem" id="' + itm.id + '" onclick="onClickNews(this.id)">' + itm.name + '  <small>' + itm.date + '</small></div>');
                $('#Block' + itm.id).removeClass('del');
                if ($('#Block' + itm.id).length == 0) {
                
                    var bl = '';
                    bl += ' <div id="Block' + itm.id + '" class="blockItem" style="clear:both" onclick="collapseBlock(this.id)">';
                    bl += ' <div class="titleContainer">';
                    bl += '    <div class="blockItemType" id="blockItemType' + itm.id + '"></div>';
                    bl += '     <div class="blockItemChrono" id="blockItemChrono' + itm.id + '" style=""></div>';
                    bl += '     <div class="blockItemAutor" id="blockItemAutor' + itm.id + '" style=""></div>';
                    bl += ' </div>';
                    bl += '    <div class="blockItemName" id="blockItemName' + itm.id + '" style=""></div>';
                    bl += '    <div class="blockItemText collapsed"  id="blockItemTextBlock' + itm.id + '" >\
                <div class="text" id="blockItemText' + itm.id + '"></div>\
<a href="#" >комментарий</a>\
                </div>';


                    $("#BlocksBox").append(bl);
                    $('#blockItemTextBlock' + itm.id).click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        if ($('#blockItemTextBlock' + itm.id).find("textarea").length == 0) {
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "/mobileService.asmx/getComment",
                                data: JSON.stringify({ id: itm.id }),
                                dataType: "json",
                                success: function (data) {
                                    // data.d
                                    var $ctrl = $('#blockItemTextBlock' + itm.id);
                                    $ctrl.find("a").remove();
                                    $ctrl.append($("<textarea maxlength='1024'></textarea").html(data.d));
                                    $ctrl.append($("<a href='#' class='saveComment'></a>").html("Сохранить комментарий"));
                                    $ctrl.find(".saveComment").click(function (e) {
                                        e.preventDefault();
                                        e.stopPropagation();
                                        saveCommentClick(itm.id);
                                    });
                                },
                                error: function (data) {
                                    console.warn("getComment error");
                                    console.warn(data);
                                }
                            });

                           
                        }
                    });
                    
                }
               

                    var clr = 'lightcoral';


                //  console.log('#blockItemType' + itm.id);
                    $('#Block' + itm.id).removeClass("blockReady0").removeClass("blockReady1").removeClass("blockReady2").addClass("blockReady" + itm.ready);
                 //   $('#blockItemType' + itm.id).css('background-color', clr);

                    $('#blockItemType' + itm.id).html(itm.type);
                    $('#blockItemName' + itm.id).html(itm.name);
                    $('#blockItemChrono' + itm.id).html(itm.chrono);
                    $('#blockItemText' + itm.id).html(itm.text);
                    $('#blockItemAutor' + itm.id).html(itm.autor);
                   
                
                try {
                    if (blockCollapsed["Block" + itm.id])
                        $("#blockItemTextBlock" + +itm.id).show();
                    }
                catch(e){}
            });

            $('del').remove();
          
        }
    });
}
function expandBlocks() {
    if (collapsed) {
        $("#bCollapsed").val("показать все тексты");
        $(".blockItemText").hide();
    }
    else {
        $("#bCollapsed").val("светнуть все тексты");
        $(".blockItemText").show();
    }

    $("#bRefresh").focus();
}
function collapseBlock(id) {
    
    if ($("#blockItemText"+id).is(':Visible')) {
        $("#blockItemText" + id).slideUp(500);
    }
    else
        $("#blockItemText" + id).slideDown(200);
}
function refreshBlocks()
{
    if ($("#BlocksBox").attr('newsid') == null)
        return;
    console.log('update block list');
    blockCollapsed = new Object();
    $(".blockItem").each(function (a, b) { blockCollapsed[$(b).attr('id')] = $("#blockItemText" + $(b).attr('id')).is(':Visible'); });
    updateNews($("#BlocksBox").attr('newsid'));

}
function saveCommentClick(id) {
    var $ctrl = $('#blockItemTextBlock' + id).find("textarea");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/mobileService.asmx/saveComment",
        data: JSON.stringify({ id: id, text: $ctrl.val() }),
        dataType: "json",
        success: function (data) {
            $ctrl.focus();
        },
        error: function () {
            console.warn("saveComment error");
            console.warn(data);
        }
    });
}