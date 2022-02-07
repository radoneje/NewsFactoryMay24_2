$(document).ready(function(){
   // console.log("init messager");
    setTimeout(initMessager, 2000);
    initModalMessager();
    ModalMessagerFlicker();
});
function capMessageClick() {
    if ($("#InMsgContent").is(":visible"))
        $("#InMsgContent").hide();
    else
        $("#InMsgContent").show();
}
function capUsersClick() {
    if ($("#MessagerContent").is(":visible"))
        $("#MessagerContent").hide();
    else
        $("#MessagerContent").show();
}
function initMessager()
{
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url:  serverRoot+"testservice.asmx/messagerUsersGet",
        data: JSON.stringify({d:"data"}),
        dataType: "json",
        async: true,
        success: function (data) {
            var dt = JSON.parse(data.d);
            if ($(dt.allUsers).length > 0)
                $("#MessagerContent").html("");
            //
          //  dt.allUsers = 
           dt.allUsers.splice(0,0,{ UserID: -1, UserName: "<i>to ALL users</i>", Active: true });
            $(dt.allUsers).each(AddMessagerDiv);
            $(dt.active).each(function (i, a) { messagerActivateUser(a) });
          //  console.log(data);
        },
        error: function (data) { 
            console.warn("error messager Init");
            console.warn(data);
            setTimeout(initMessager, 5*1000);
        }
    })
}

function AddMessagerDiv()
{
    
    var ContainerId = "MessagerItem" + this.UserID;
    if ($("#" + ContainerId).length <= 0) {
        //   $("#MessagerContent").append(GenerateBlankMessagerDiv(this.UserID, this.UserName));
        $(".messagerBody").append(GenerateBlankMessagerDiv(this.UserID, this.UserName));
       // $("#MessagerMainPanel").show();
    }
}
function GenerateBlankMessagerDiv(ItemId, UserName)
{
    return '<div class="MessagerItem" id="MessagerItem' + ItemId + '">\
       <small><div onclick="OpenSendMessage(' + ItemId + ')" id="MessagerItemTitle' + ItemId + '" class="MessagerActivefalse">' + UserName + '</div></small> \
      \
    </div>';
}
function messagerActivateUser(id)
{
    $('#MessagerItemTitle' + id).removeClass("MessagerActivefalse");
    $('#MessagerItemTitle' + id).addClass("MessagerActivetrue");
}
function messagerDeActivateUser(id) {
    $('#MessagerItemTitle' + id).addClass("MessagerActivefalse");
    $('#MessagerItemTitle' + id).removeClass("MessagerActivetrue");
}
function AddMessagerRowData(data)
{
    $('#MessagerItemTitle' + data.UserID).html(data.UserName);
    $('#MessagerItemTitle' + data.UserID).removeClass("MessagerActivetrue");
    $('#MessagerItemTitle' + data.UserID).removeClass("MessagerActivefalse");
    $('#MessagerItemTitle' + data.UserID).addClass("MessagerActive"+ data.Active);

}

function OpenSendMessage(ItemId)
{
    if ($("#SendMessageConteiner" + ItemId).length > 0) {
        $("#SendMessageConteiner" + ItemId).remove();
        return;
    }
    $("#MessagerItem" + ItemId).append(
        $(div)
        .SendMessageConteiner(ItemId)
        .addClass("SendMessageConteiner")/*GenerateSendMessageConteiner(ItemId)*/
        .attr("id", "SendMessageConteiner" + ItemId)
        );
    $("#MessagerItem" + ItemId).find('textarea').autoHeight().focus();
}
$.fn.SendMessageConteiner = function (id) {
    var _this = this;

    $(_this)
        .append(
         $("<textarea class='SendMessageTextarea' maxlength='30000' placeholder='ваше сообщение '></textarea>")
         .keyup(function (e) {   $(e.target).siblings(".SendMessageButton").html($(e.target).val().length > 0 ? "@" : "X");  })
        )
        .append($(div).html("X").addClass("SendMessageButton").click(function () { SendMessage(id)}));

    return this;

}

function GenerateSendMessageConteiner(ItemId) {
    return '<div id="SendMessageConteiner'+ItemId+'" class="SendMessageConteiner">\
<div class="input-group">\
<input type="text" id="SendMessageText' + ItemId + '" class="form-control" placeholder="ваше сообщение" aria-describedby="basic-addon2">\
<span class="input-group-addon SendMessageButton"  id="SendMessageBtn' + ItemId + '" onclick="SendMessage(' + ItemId + ')" >@</span>\
</div>\
   \
        </div>';
}
function SendMessage(ToUserID)
{
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        NewsId: ToUserID,
        NewsGroupId: RemoveHTMLTag($("#MessagerItem" + ToUserID).find("textarea").val()),
    }
   
    if (jdata.NewsGroupId.length > 0) {

        $("#SendMessageConteiner" + ToUserID).html("<div class='senMesgPrContainer'><div id='senMesgPrContainerText" + ToUserID + "' class='senMesgPrContainerText'></div><img id='senMesgPrContainerImg" + ToUserID + "' class='senMesgPrContainerImg' src='/images/loading.gif'/></div>");
        $("#senMesgPrContainerText" + ToUserID).html((jdata.NewsGroupId));

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/SendMessage",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function () {
                setTimeout(function () {
                    $("#senMesgPrContainerImg" + ToUserID).fadeOut(500);
                }, 500);
                setTimeout(function () {
                    $("#SendMessageConteiner" + ToUserID).fadeOut(500);
                }, 500);
                setTimeout(function () {
                    $("#SendMessageConteiner" + ToUserID).remove();
                }, 500);
            },
            error: function () {
                AjaxFailed();
                $("#SendMessageConteiner" + ToUserID).removeClass("alert-success");
                $("#SendMessageConteiner" + ToUserID).addClass("alert-danger");
            }
        });
    }
    else {
        $("#SendMessageConteiner" + ToUserID).fadeOut(500, function () {
            $("#SendMessageConteiner" + ToUserID).remove();
        })
        
    }
    
}

////////// ВХОДЯЩИЕ
function ReloadInMsg(data)
{
    if (!(typeof JSON.parse(data.d) === "undefined")) {
        var list = JSON.parse(data.d);
       
            ReloadInMsgContainer(list);
            ReloadInMsgData(list);

            $(".newMessagesCount").html("( " + $(".InMsgItem ").length + " )").attr("count", $(".InMsgItem ").length);
    }
       
}
function ReloadInMsgContainer(data) {

    if (!(typeof data === "undefined")) {//если в данных есть выпуски
       
        data.forEach(AddInMsgDiv);
    }
}
function AddInMsgDiv(data) {

    var ContainerId = "InMsgItem" + data.Id;
    if ($("#" + ContainerId).length <= 0) {
        $("#InMsgContent").append(GenerateBlankInMsgDiv(data));
        $("#" + ContainerId).attr("From", data.From)
        $("#InMsgItemReplyText" + data.Id).autoHeight();
        $('#InMsgItemReplyText' + data.Id).keyup(function (e) {
            $(e.target).siblings(".SendMessageButton").html($(e.target).val().length > 0 ? "@" : "X");
           // if (e.keyCode == 13) {
            //    $('#InMsgItemReplySendBtn' + data.Id).click();
           // }
        });
    }

}
function GenerateBlankInMsgDiv(data)
{
    return '<div class="InMsgItem alert alert-success" id="InMsgItem' + data.Id + '">\
       <small><div id="InMsgItemTitle' + data.Id + '" class="InMsgItemTitle' + data.Id + '"></div></small>\
       <div id="InMsgItemText' + data.Id + '" class="InMsgItemText' + data.Id + '"></div>\
      <div class="SendMessageConteiner" id="ReplyMessageConteiner ' + data.Id +' ">\
<textarea class="SendMessageTextarea" id="InMsgItemReplyText' + data.Id + '" maxlength="30000" placeholder="ответить или закрыть"></textarea>\
<div class="SendMessageButton" id="InMsgItemReplySendBtn' + data.Id + '" onclick="ReplyMessage(' + data.Id + ')">X</div>\
</div>\
    </div>';
}
function ReplyMessage(InMessageId)
{
    
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        NewsId: InMessageId,
        NewsGroupId: RemoveHTMLTag($("#InMsgItemReplyText" + InMessageId).val()),
    }
   // if (jdata.NewsGroupId.length > 0) {
        // $("#SendMessageConteiner" + ToUserID).html("<div class='senMesgPrContainer'><div id='senMesgPrContainerText" + ToUserID + "' class='senMesgPrContainerText'></div><img id='senMesgPrContainerImg" + ToUserID + "' class='senMesgPrContainerImg' src='/images/loading.gif'/></div>");
      //  $("#senMesgPrContainerText" + ToUserID).html((jdata.NewsGroupId));
        $("#InMsgItem" + InMessageId).html("<div class='senMesgPrContainer'><div id='senMesgPrContainerText" + InMessageId + "' class='senMesgPrContainerText'></div><img id='senMesgPrContainerImg" + InMessageId + "' class='senMesgPrContainerImg' src='/images/loading.gif'/></div>");
        $("#senMesgPrContainerText" + InMessageId).html((jdata.NewsGroupId));

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/ReplyMessage",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: function () {
                setTimeout(function () {
                    $("#senMesgPrContainerImg" + InMessageId).fadeOut(500);
                }, 500);
                setTimeout(function () {
                    $("#InMsgItem" + InMessageId).fadeOut(500);
                }, 500);
                setTimeout(function () {
                    $("#InMsgItem" + InMessageId).remove();
                }, 500);
            },
            error: function () {
                $("#InMsgItem" + InMessageId).removeClass("alert-success");
                $("#InMsgItem" + InMessageId).addClass("alert-danger");
                AjaxFailed();
            }
                
        });
  //  }
   /* else
        $("#InMsgItem" + InMessageId).fadeOut(500, function () {
            $("#InMsgItem" + InMessageId).remove();
            if ($(".InMsgItem").length == 0) {
                $("#MessagerMainPanel").hide()
            }
        });*/

}
function ReloadInMsgData(data) {
    if (!(typeof data === "undefined")) {//если в данных есть выпуски
        data.forEach(AddInMsgRowData);
    }
}
function AddInMsgRowData(data) {
    $('#InMsgItemTitle' + data.Id).html("<small>От:</small> "+ data.FromName+"");
    $('#InMsgItemText' + data.Id).html(data.text);
}

function initModalMessager() {
    $(".messagerHead").click(function () {
        if($(".messagerBody").is(":visible"))
        {
            $(".messagerBody").fadeOut(500);
        }
        else {
            $(".messagerBody").fadeIn(500);
        }
    });
}
function ModalMessagerFlicker() {
    if($(".messagerHead").hasClass("newMessage"))
    {
       
     
        $(".messagerHead").removeClass("newMessage");
    }
    else if ($(".newMessagesCount").attr("count")>0) {
        $(".messagerHead").addClass("newMessage");
        var audio = new Audio('/Resources/_Message_audio.mp3');
        audio.volume = 0.8;
        audio.play();
    }
    setTimeout(ModalMessagerFlicker, 4*1000);
}