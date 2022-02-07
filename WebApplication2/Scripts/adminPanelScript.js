
$(document).ready(function() {
    adminPassSetBorderToDefault();
    adminReadRateSetBorderToDefault();
    syncTemplateInit();
    geoTemplateInit();
    srcTemplateInit();
    socialInit();
    APDeletedInit();
    APDeletedNewsInit();
    rssInit();
    APprogInit();
    AProleInit();
    APuserInit();
    APBlockTypeInit();
    APprintTemplateInit();
    APlayOutInit();
    ATitleOutInit();
});
function syncTemplateInit() {
    //  $("#SyncTemplateBox").modal();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/SyncTemplateGet",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            $(".SyncTemplateItem").remove();
            dt.items.forEach(function (item) {
                addRowToSyncTemplate(item);
            })
        }
    })
}
function geoTemplateInit() {
    //  $("#SyncTemplateBox").modal();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/BEinsertGeo",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            console.log("geoTemplateInit", dt)
            $(".geoTemplateItem").remove();
            dt.forEach(function (item) {
                addRowToGeoTemplate(item);
            })

        }
    })
}

function addRowToGeoTemplate(item) {

    var html = '<div id="geoTemplateItem' + item.id + '" class="geoTemplateItem">\
          <input id="geoTemplateItemName' + item.id + '" class="rsslItemText" type="text"  placeholder="' + langTable["Name"] + '" value="' + item.name + '" onblur="SaveGeoTemplate(\'' + item.id + '\')"/>\
          <input class="btn btn-danger btn-xs" type="button" style="display: inline-block;width:70px" value="' + langTable['CapDelete'] + '" onclick="DelGeoTemplate(\'' + item.id + '\', event)"/>\
      </div>';
    $("#blockGeeoModalBody").append(html);
    $("#geoTemplateItemName" + item.id).focus();
}

function srcTemplateInit() {
    //  $("#SyncTemplateBox").modal();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/BEinsertSrc",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            console.log("srcTemplateInit", dt)
            $(".srcTemplateItem").remove();
            dt.forEach(function (item) {
                addRowToSrcTemplate(item);
            })

        }
    })
}

function addRowToSrcTemplate(item) {

    var html = '<div id="srcTemplateItem' + item.id + '" class="srcTemplateItem">\
          <input id="srcTemplateItemName' + item.id + '" class="rsslItemText" type="text"  placeholder="' + langTable["Name"] + '" value="' + item.name + '" onblur="SaveSrcTemplate(\'' + item.id + '\')"/>\
          <input class="btn btn-danger btn-xs" type="button" style="display: inline-block;width:70px" value="' + langTable['CapDelete'] + '" onclick="DelSrcTemplate(\'' + item.id + '\', event)"/>\
      </div>';
    $("#blockSrcModalBody").append(html);
    $("#srcTemplateItemName" + item.id).focus();
}
function addRowToSyncTemplate(item) {

    var html = '<div id="SyncTemplateItem' + item.id + '" class="SyncTemplateItem">\
          <input id="blockEditModalBodyAddName' + item.id + '" class="rsslItemText" type="text"  placeholder="' + langTable["Name"] + '" value="' + item.name + '" onblur="SaveSyncTemplate(\'' + item.id + '\')"/>\
           <input id="blockEditModalBodyAddCap' + item.id + '"  class="rsslItemText" type="text"  placeholder="' + langTable["Position"] + '"  value="' + item.cap + '" onblur="SaveSyncTemplate(\'' + item.id + '\')"/>\
          <input class="btn btn-danger btn-xs" type="button" style="display: inline-block;width:70px" value="' + langTable['CapDelete'] + '" onclick="DelSyncTemplate(\'' + item.id + '\', event)"/>\
      </div>';
    $("#blockEditModalBody").prepend(html);
}

function SaveSyncTemplate(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/SyncTemplateEdit",
        data: JSON.stringify({ id: id, name: $("#blockEditModalBodyAddName" + id).val(), cap: $("#blockEditModalBodyAddCap" + id).val() }),
        dataType: "json",

    })
}
function DelSyncTemplate(id, event) {
    NFconfirm("Are you sure?", event.pageX-200, event.pageY, id, delSyncTemplateConfirmed);
}
function delSyncTemplateConfirmed(id)
{
    $("#SyncTemplateItem" + id).fadeOut(500, function () { $("#SyncTemplateItem" + id).remove(); });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/SyncTemplateDel",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        

    })
}
function addToSyncTemplate() {

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/SyncTemplateAdd",
        data: JSON.stringify({ id: 0, name: "", cap: "" }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            dt.items.forEach(function (item) {
                addRowToSyncTemplate(item);
            })
            $("#blockEditModalBodyAddName").val("");
            $("#blockEditModalBodyAddCap").val("");
        }
    })
}
function addToGeoTags() {

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/geoTemplateAdd",
        data: JSON.stringify({ id: 0, name: "" }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
          //  dt.items.forEach(function (item) {
            addRowToGeoTemplate(dt.item);
            
          //  })
            
        }
    })
} 
function SaveGeoTemplate(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/geoTemplateEdit",
        data: JSON.stringify({ id: id, name: $("#geoTemplateItemName" + id).val() }),
        dataType: "json",

    })
}
function DelGeoTemplate(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/geoTemplateDel",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        success: function (e) {
            $("#geoTemplateItem" + id).fadeOut(500, function () { $("#geoTemplateItem" + id).remove(); });
        }

    })
}


function addToSrcTags() {

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/srcTemplateAdd",
        data: JSON.stringify({ id: 0, name: "" }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            //  dt.items.forEach(function (item) {
            addRowToSrcTemplate(dt.item);

            //  })

        }
    })
}
function SaveSrcTemplate(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/srcTemplateEdit",
        data: JSON.stringify({ id: id, name: $("#srcTemplateItemName" + id).val() }),
        dataType: "json",

    })
}
function DelSrcTemplate(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/srcTemplateDel",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        success: function (e) {
            $("#srcTemplateItem" + id).fadeOut(500, function () { $("#srcTemplateItem" + id).remove(); });
        }

    })
}

function adminPassSetBorderToDefault()
{
    $("#adminNewPass1").change(function () {
        $("#adminNewPass1").removeClass(controlError);
        $("#adminNewPass2").removeClass(controlError);
    });
    $("#adminNewPass2").change(function () {
        $("#adminNewPass1").removeClass(controlError);
        $("#adminNewPass2").removeClass(controlError);
    });
    $("#adminOldPass").change(function () {
        $("#adminOldPass").removeClass(controlError);
    });
}
function adminReadRateSetBorderToDefault() {
    $("#adminReadSpeed").change(function () {
        $("#adminReadSpeed").css("border-bottom", "");
    }); 
}

function adminPasswordChange(event) {


    NFconfirm("Are you sure?", event.pageX , event.pageY, 1, adminPasswordChangeConfirmed);
}
function adminPasswordChangeConfirmed(){

    if($("#adminNewPass1").val()!=$("#adminNewPass2").val() ||$("#adminNewPass1").val().length==0 )
    {
        $("#adminNewPass1").css("border-bottom", "1px solid red");
        $("#adminNewPass2").css("border-bottom", "1px solid red");
        return;
    }
    if ($("#adminOldPass").val().length == 0) {
        $("#adminOldPass").css("border-bottom", "1px solid red");
        return;
    }
    var dt = $("#adminPassChageForm").getFormData();
    try{
        sendFormDataToService(dt.serviceName, dt.params, function (data) {
          
            $("#adminPassChageForm").flipBackground(JSON.parse(data.d).status>0?"green":"red");
        });
    }
    catch (ex) {
        $("#adminPassChageForm").flipBackground("red");
        console.warn(error);
        console.warn(ex);
    }
}
function adminReadRateChange()
{
    if ($("#adminReadSpeed").val().length == 0) {
        $("#adminReadSpeed").val();
        $("#adminReadSpeed").addClass("controlError");
        return;
    }
    var rr;
    try{
        rr= parseInt($("#adminReadSpeed").val());
    }
    catch (ex) {
        $("#adminReadSpeed").val();
        $("#adminReadSpeed").addClass("controlError");
        return;
    }
    if(rr<3 || rr>30)
    {
        $("#adminReadSpeed").val();
        $("#adminReadSpeed").css("border-bottom", "1px solid red");
        return;
    }

    var dt = $("#adminReadRateChageForm").getFormData();
        sendFormDataToService(dt.serviceName, dt.params, function (data) {
          
            $("#adminReadRateChageForm").flipBackground(JSON.parse(data.d).status>0?"green":"red");
        });

}
function addSocial()
{
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot+ "testservice.asmx/socialAdd",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (data) {
            APreloadSocial(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/socialAdd error");
            console.warn(data);
        }

    })
}
function APreloadSocial(data)
{
    $(".socialBox").html("");
    data.feeds.forEach(function (item) {
        APsocialFeedAdd(item, data.types);
    });
    return;
}
function       APsocialFeedAdd(item, types)
{
    
    $(".socialBox").append(
        $(div)
        .attr("id", item.id)
        .addClass("socialItem")
        .append(socialItemTypeadd(item.typeId, types, item.id))
        .append(
            $("<input type='text' onblur='socialItemChange(\""+item.id+"\")'/>")
            .attr("placeholder", langTable['CapName'])
            .attr("name","title")
            .val(item.title)

            .addClass("socialItemText")
        )
        .append(
            $("<input type='text' onblur='socialItemChange(\"" + item.id + "\")'/>")
            .attr("placeholder", "auth Key")
            .attr("name", "authKey")
            .val(item.authKey)
           
            .addClass("socialItemText")
        )
        .append(
         $("<input type='button' class='btn btn-danger btn-xs socialItemBtn' onclick=' deleteSocial(\""+item.id+"\", event)'/>")
         .val(langTable['CapDelete'])
        )
        );
}
function socialItemTypeadd(typeId, types, id){

    var ret = $("<select class='socialItemSelect' onchange='socialItemChange(\"" + id + "\")'/>")
    types.forEach(function (tp) {
        var itm = $("<option></option>");
        itm.html(tp.title);
        itm.attr("value", tp.id);
        if (typeId == tp.id)
            itm.prop("selected", true);
        ret.append(itm);
    })
    ret.change(socialItemChange(id))
    return ret;
}


function deleteSocial(id, event)
{
    NFconfirm("Are you sure?", event.pageX - 150, event.pageY, id, deleteSocialConfirmed);
    }
function deleteSocialConfirmed(id){
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot+"testservice.asmx/socialDelete",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        async: true,
        success: function (data) {
            APreloadSocial(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/socialDelete error");
            console.warn(data);
        }

    })
}
function socialItemChange(id)
{
    
    if ($("#" + id).length==0)
        return;
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot+ "testservice.asmx/socialItemChange",
        data: JSON.stringify({ id: id, title: $("#" + id + " input[name='title']").val(), typeId: $("#" + id + " select").val(), authKey: $("#" + id + " input[name='authKey']").val() }),
        dataType: "json",
        async: true,
        success: function (data) {
           // relaodSocial(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/socialItemChange error");
            console.warn(data);
        }

    })
}
function socialInit()
{
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot+"testservice.asmx/socialInit",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (data) {
             APreloadSocial(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/socialInit error");
            console.warn(data);
        }

    })
}
function rssInit() {
    $('#collapse4').on('hidden.bs.collapse', function () {
        $(".rssBox").html("");
    })
    $('#collapse4').on('show.bs.collapse', function () {
        $.ajax({
            type: "POST",
            contentType: "application/json;",
            url: serverRoot + "testservice.asmx/rssInit",
            data: JSON.stringify({ id: 0 }),
            dataType: "json",
            async: true,
            success: function (data) {
               
                APreloadRss(JSON.parse(data.d));
            },
            error: function (data) {
                console.warn("/testservice.asmx/rssInit error");
                console.warn(data);
            }

        })
    })
}
function APreloadRss(data)
{
    $(".rssBox").html("");
   
    
    data.forEach(function (item) {
        APrssFeedAdd(item);
          //  APrssFeedAdd(item);
        });
        return;
}
function test(item) {
    console.log(item);
}
function APrssFeedAdd(item) {
    
    try {
       
        $(".rssBox").append(
            $(div)
            .attr("id", "APrss"+item.id)
            .addClass("socialItem")
            .append(
                $("<input type='text' onblur='rssItemChange(" + item.id + ")'/>")
                .attr("placeholder", langTable['CapName'])
                .attr("name", "name")
                .val(item.Name)
                .addClass("rsslItemText")
            )
            .append(
                $("<input type='text' onblur='rssItemChange(" + item.id + ")'/>")
                .attr("placeholder", "RSS URL")
                .attr("name", "url")
                .val(item.URL)
                .addClass("rsslItemText")
            )
            .append(
             $("<input type='button' class='btn btn-danger btn-xs socialItemBtn' onclick=' deleteRss(" + item.id +" , event)'/>")
             .val(langTable['CapDelete'])
            )
            );
    }
    catch(e)
    {
        console.warn("APrssFeedAdd");
        console.warn(e);
    }
}

function rssItemChange(id)
{
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot + "testservice.asmx/rssItemChange",
        data: JSON.stringify({ id: id, name: $("#APrss" + id + " input[name='name']").val(), URL: $("#APrss" + id + " input[name='url']").val() }),
        dataType: "json",
        async: true,
        success: function (data) {
            // relaodSocial(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/rssItemChange error");
            console.warn(data);
        }

    })
}
function deleteRss(id, event)
{
    NFconfirm("Are you sure?", event.pageX - 150, event.pageY, id, deleteRssConfirmed);
}
function deleteRssConfirmed(id)
{
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot + "testservice.asmx/rssDelete",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        async: true,
        success: function (data) {
            APreloadRss(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/rssDelete error");
            console.warn(data);
        }

    })
}
function addRss()
{
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot + "testservice.asmx/rssAdd",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (data) {
            APreloadRss(JSON.parse(data.d));
        },
        error: function (data) {
            console.warn("/testservice.asmx/rssAdd error");
            console.warn(data);
        }

    })
}
function APDeletedInit() {
    $('#collapse5').on('hidden.bs.collapse', function () {
        $(".APblockDeletedBox").html('<img src="'+serverRoot+'Images/loading.gif"  style="width:50px"/>');
    })
    $('#collapse5').on('show.bs.collapse', function () {
        $(".APblockDeletedBox").load(serverRoot+'Blocks/blockDeleted.aspx');
    })
}

function APDeletedNewsInit() {
    $('#collapse51').on('hidden.bs.collapse', function () {
        $(".APblockDeletedBox").html('<img src="' + serverRoot + 'Images/loading.gif"  style="width:50px"/>');
    })
    $('#collapse51').on('show.bs.collapse', function () {
        $(".APblockDeletedNews").load(serverRoot + 'News/newsDeleted.aspx');
    })
}
function blockUndelete(id) {


    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "api/BlocksUndelete/" + (id),
        data: JSON.stringify({ id: (id) }),
        dataType: "json",
        async: true,
        success: function (dt) {
            $("#APBlockDeleted" + id).fadeOut(500, function () {
                $("#APBlockDeleted" + id).remove();
            });
        }

    });
}
function newsUndelete(id) {


    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "api/newsUndelete/" + (id),
        data: JSON.stringify({ id: (id) }),
        dataType: "json",
        async: true,
        success: function (dt) {
            $("#APnewsDeleted" + id).fadeOut(500, function () {
                $("#APnewsDeleted" + id).remove();
            });
        }

    });
}
function APprogInit() {

        $('#collapse6').on('hidden.bs.collapse', function () {
            $(".APprogBox").html("").loading50();
        })
        $('#collapse6').on('show.bs.collapse', function () {
            $.ajax({
                type: "POST",
                contentType: "application/json;",
                url: serverRoot + "testservice.asmx/progInitAP",
                data: JSON.stringify({ id: 0 }),
                dataType: "json",
                async: true,
                success: function (data) {
                    dt = JSON.parse(data.d);
                    if (data.status < 1)
                        return ajaxErr("/testservice.asmx/APprogInit error", dt.message);
                    APreloadProgr(dt.items);
                },
                error: function (data) {
                    ajaxErr("/testservice.asmx/APprogInit error", data);
                }

            });
           
        })

  
};
 function APreloadProgr(data){
     $(".APprogBox").html("");


     data.forEach(function (item) {
         APprogListAdd(item);
         //  APrssFeedAdd(item);
     });
 }
 function APprogListAdd(item) {
     try {

         $(".APprogBox").append(
             $(div)
             .attr("id", "APprog" + item.id)
             .addClass("socialItem")
             .append(
                 $("<input type='text' onblur='APprogItemChange(" + item.id + ")'/>")
                 .attr("placeholder", langTable['CapName'])
                 .attr("name", "name")
                 .val(item.Name)
                 .addClass("rsslItemText")
             )
           
             .append(
              $("<input type='button' class='btn btn-danger btn-xs socialItemBtn' onclick=' APprogDel(" + item.id + " , event)'/>")
              .val(langTable['CapDelete'])
             )
             );
     }
     catch (e) {
         console.warn("APprogListAdd");
         console.warn(e);
     }
 }
 function APprogItemChange(id) {

     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/progItemChange",
         data: JSON.stringify({ id: id, name: $("#APprog" + id + " input[name='name']").val() }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 throw dt.message;
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/APprogItemChange error", data);
         }

     })
 }
 function APprogDel(id, event) {
     NFconfirm("Are you sure?", event.pageX - 150, event.pageY, id, APprogDelConfirmed);


 }
 function APprogDelConfirmed(id)
 {
     $("#APprog" + id).fadeOut(500, function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/progDel",
             data: JSON.stringify({ id: id }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     throw dt.message;
                 APreloadProgr(dt.items)
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/APprogDel error", data);
             }

         });
     });
 }
 function APprogAdd() {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/progAdd",
         data: JSON.stringify({ id: 0 }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                   throw  dt.message;
             APreloadProgr(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/APprogInit error", data);
         }

     })
 }

 function AProleInit() {
     $('#collapse7').on('hidden.bs.collapse', function () {
         $(".AProleBox").html("").loading50();
     })
     $('#collapse7').on('show.bs.collapse', function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/roleInitAP",
             data: JSON.stringify({ id: 0 }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (data.status < 1)
                     return ajaxErr("/testservice.asmx/roleInitAP error", dt.message);
                 APreloadRole(dt.items);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/roleInitAP error", data);
             }

         });

     })
 }
 function APreloadRole(data)
 {
     $(".AProleBox").html("");

     data.forEach(function (item) {
         AProleListAdd(item);
         //  APrssFeedAdd(item);
     });
 }
 function AProleListAdd(item) {
    // console.log(item);
     try {

         $(".AProleBox").append(
             $(div)
             .attr("id", "AProle" + item.URoleID)
             .addClass("socialItem RoleItem")
             .append(
                 $("<input type='text' onblur='AProleItemChange(" + item.URoleID + ")'/>")
                 .attr("placeholder", langTable['CapName'])
                 .attr("name", "name")
                 .val(item.URoleName)
                 .addClass("rsslItemText")
             )
              .append(
              $("<input type='button' name='edit' class='btn btn-warning btn-xs socialItemBtn' onclick=' AProleEdit(" + item.URoleID + " , event)'/>")
              .val(langTable['CapEdit'])
             )
            .append(
              $("<input type='button' class='btn btn-success btn-xs socialItemBtn' onclick=' AProleCopy(" + item.URoleID + " , event)'/>")
              .val("copy")
             )

             .append(
              $("<input type='button' name='del' class='btn btn-danger btn-xs socialItemBtn' onclick=' AProleDel(" + item.URoleID + " , event)'/>")
              .val(langTable['CapDelete'])
             )
             .append($(div)
                .attr("id", "socialItemExt" + item.URoleID)
                .addClass("socialItemExt")
             )
             );

         if(item.URoleUndelete==true)
         {
             $("#AProle" + item.URoleID + " input[type='text']").attr("readonly","true");
             $("#AProle" + item.URoleID + " input[name='edit']").remove();
             $("#AProle" + item.URoleID + " input[name='del']").remove();

         }
     }
     catch (e) {
         console.warn("AProleListAdd");
         console.warn(e);
     }
 }
 function AProleAdd() {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/roleAdd",
         data: JSON.stringify({ id: 0 }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/roleAdd error", data);
             APreloadRole(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/roleAdd error", data);
         }

     })
 }
 function AProleItemChange(id)
 {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/roleItemChange",
         data: JSON.stringify({ id: id, name: $("#AProle" + id + " input[name='name']").val() }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 throw dt.message;
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/roleItemChange error", data);
         }

     })
 }
 function AProleDel(id, event) {
     NFconfirm("Are you sure?", event.pageX - 150, event.pageY, id, AProleDelConfirmed);

 }
 function AProleDelConfirmed(id) {
     console.log(id);
     $("#AProle" + id).fadeOut(500, function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/roleDel",
             data: JSON.stringify({ id: id }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     ajaxErr("/testservice.asmx/AProleDelConfirmed error", data);
                 APreloadRole(dt.items)
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/AProleDelConfirmed error", data);
             }

         })
     });
 }
 function AProleEdit(id, event) {
     if ($("#socialItemExt" + id).html().length > 10)
     {
         $(".socialItemExt").html("");
         return;
     }
     $(".socialItemExt").html("");
     $("#socialItemExt"+id).loading50();
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/roleGetForm",
         data: JSON.stringify({ id: id }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/AProleEdit error", data);
           
             APreloadRoleForm(dt.items, id);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/AProleEdit error", data);
         }

     })
 }
 function AProleCopy(id, event) {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/roleCopy",
         data: JSON.stringify({ id: id }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/roleCopy error", data);
             APreloadRole(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/roleCopy error", data);
         }

     })
 }
 function APreloadRoleForm(items, id)
 {
     $("#socialItemExt" + id).html("");
     items.forEach(function (item) {
         $("#socialItemExt" + id).append($(div)
             .attr("id", "#socialItemExtRight" + item.URightID)
             .addClass("socialItemExtRight")
             .attr("URightID", item.URightID)
             .attr("URoleID", id)
             .append($("<input type='checkbox'/>").prop('checked', item.value).attr("id", "socialItemExtRightCB" + item.URightID))
             .append($("<label/>").attr('for', "socialItemExtRightCB" + item.URightID).html(item.title))
             );

     });
     $(".socialItemExtRight input[type='checkbox']").change(function (e) {
         //console.log(e);
         var elem =$(e.target).parent();

         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/roleChange",
             data: JSON.stringify({ URoleID: $(elem).attr("URoleID"), URightID: $(elem).attr("URightID"), val: $(e.target).is(':checked') }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     ajaxErr("/testservice.asmx/roleChange error", data);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/roleChange error", data);
             }

         })

     });
  
 }

 function APuserInit() {
     $('#collapse8').on('hidden.bs.collapse', function () {
         $(".APuserBox").html("").loading50();
     })
     $('#collapse8').on('show.bs.collapse', function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/userInitAP",
             data: JSON.stringify({ id: 0 }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (data.status < 1)
                     return ajaxErr("/testservice.asmx/userInitAP error", dt.message);
                 APreloadUser(dt.items);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/userInitAP error", data);
             }

         });

     })
 }
 function APreloadUser(data) {
     $(".APuserBox").html("").append($(div).append("<input type='button' value='unlock Items' class='btn btn-warning btn-xs unLockItems'/>"));
     $(".unLockItems").click(function () {
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
     });

     data.forEach(function (item) {
         APuserListAdd(item);
         //  APrssFeedAdd(item);
     });
 }
 function APuserListAdd(item) {
     try {
   
         $(".APuserBox").append(
             $(div)
             .attr("id", "APuser" + item.UserID)
             .addClass("socialItem UserItem")
             .append(
                 $("<input type='text' onblur='APuserItemChange(" + item.UserID + ")'/>")
                 .attr("placeholder", langTable['CapName'])
                 .attr("name", "name")
                 .val(item.name)
                 .addClass("rsslItemText")
             )
             .append(
                 $("<input type='password' onblur='APuserItemChange(" + item.UserID + ")'/>")
                 .attr("placeholder", 'password')
                 .attr("name", "pass")
                 .val(item.pass)
                 .addClass("rsslItemText")
             )
              .append(
              $("<input type='button' name='edit' class='btn btn-warning btn-xs socialItemBtn' onclick=' APuserEdit(" + item.UserID + " , event)'/>")
              .val(langTable['CapEdit'])
             )
            .append(
              $("<input type='button' class='btn btn-success btn-xs socialItemBtn' onclick=' APuserCopy(" + item.UserID + " , event)'/>")
              .val(langTable['CapCopy'])
             )

             .append(
              $("<input type='button' name='del' class='btn btn-danger btn-xs socialItemBtn' onclick=' APuserDel(" + item.UserID + " , event)'/>")
              .val(langTable['CapDelete'])
             )
             .append($(div)
                .attr("id", "socialItemExt" + item.UserID)
                .addClass("socialItemExt")
             )
             );

         if (item.UserID <=1) {
             $("#APuser" + item.UserID + " input[type='text']").attr("readonly", "true");
             $("#APuser" + item.UserID + " input[name='edit']").remove();
             $("#APuser" + item.UserID + " input[name='del']").remove();

         }
     }
     catch (e) {
         console.warn("APuserListAdd");
         console.warn(e);
     }
 }    
 function APuserAdd() {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/userAdd",
         data: JSON.stringify({ id: 0 }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/userAdd error", data);
             APreloadUser(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/userAdd error", data);
         }

     })
 }
 function APuserDel(id, event) {
     NFconfirm("Are you sure?", event.pageX - 150, event.pageY, id, APuserDelConfirmed);
 }
 function APuserDelConfirmed(id) {
     $("#APuser" + id).fadeOut(500, function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/userDel",
             data: JSON.stringify({ id: id }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     ajaxErr("/testservice.asmx/APuser error", data);
                 APreloadUser(dt.items)
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/APuser error", data);
             }

         })
     });
 }
 function APuserItemChange(id)
 {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/userItemChange",
         data: JSON.stringify({ id: id, name: $("#APuser" + id + " input[type='text']").val(), pass: $("#APuser" + id + " input[type='password']").val() }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/APuserItemChange error", data.message);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/APuserItemChange error", data);
         }

     })
 }
 function APuserCopy(id)
 {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/userCopy",
         data: JSON.stringify({ id: id }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/userCopy error", data);
             APreloadUser(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/userCopy error", data);
         }

     })
 }
 function APuserEdit(id) {
     if ($("#socialItemExt" + id).html().length > 10) {
         $(".socialItemExt").html("");
         return;
     }
     $(".socialItemExt").html("");
     $("#socialItemExt" + id).loading50();
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/userGetForm",
         data: JSON.stringify({ id: id }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/APuserEdit error", data);

             APreloadUserForm( dt.admRoles, id);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/APuserEdit error", data);
         }

     })
 }
 function APreloadUserForm(admRoles, id) {
     $("#socialItemExt" + id).html("");
     admRoles.forEach(function (item) {
         $("#socialItemExt" + id).append($(div)
             .attr("id", "#socialItemExtRight" + item.URightID)
             .addClass("socialItemExtRight userAdminR")
             .attr("roleId", item.roleId)
             .attr("userId", id)
             .append($("<input type='checkbox'/>").prop('checked', item.value).attr("id", "userExtRightCB" + item.roleId))
             .append($("<label/>").attr('for', "userExtRightCB" + item.roleId).html(item.roleName))
             );

     });
     $(".userAdminR input[type='checkbox']").change(function (e) {
         //console.log(e);
         var elem = $(e.target).parent();

         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/userAdminChange",
             data: JSON.stringify({ URoleID: $(elem).attr("roleId"), UserID: $(elem).attr("userId"), val: $(e.target).is(':checked') }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     ajaxErr("/testservice.asmx/userAdminChange error", data);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/userAdminChange error", data);
             }

         })
     });
     $("#socialItemExt" + id).append($(div)
         .addClass("APuserExProgBox")
         .append($(div)
            .addClass("APuserExProg")
            .loading50()
         )
         .append($(div)
            .addClass("APuserExVal")
         )
         .append($(div)
            .addClass("BSRow")
         )
         );
     $.getJSON(serverRoot + /*"API/ProgrammsListGet.ashx"*/"API/rogrammsListWidthRight/"+id, function (progList) {
         $(".APuserExProg").html("");
         progList.items.forEach(function (item) {
             $(".APuserExProg").append($(div)
                 .addClass("APuserExProgItem")
                 .attr("progId", item.id)
                 .attr("userId", id)
                 .html(item.name)
                 .click(APuserExProgItemSelect)
                 );
         });
     });
    
    

 }
 function APuserExProgItemSelect(event)
 {
     $(".APuserExProgItemSelect").removeClass("APuserExProgItemSelect");
     var elem = ($(event.target));
     $(elem).addClass("APuserExProgItemSelect");

     $(".APuserExVal").html("").loading50().attr("progId", $(elem).attr("progId"));
      
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/userGetFormProg",
         data: JSON.stringify({ progId: $(elem).attr("progId"), userId: $(elem).attr("userId") }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/userGetFormProg error", data);

             APreloadUserFormProg(dt.o, dt.roles);
             scrollToElement($(".APuserExVal"), -100);     
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/userGetFormProg error", data);
         }

     })
 }
 function APreloadUserFormProg(items, roles)
 {
     $(".APuserExVal").html("");
     $(".APuserExVal").append($(div).addClass("APuserExValO"));
     items.forEach(function (item) {
         $(".APuserExValO").append($(div)
             .addClass("socialItemExtRight userAdminR")
             .append($("<input type='checkbox'/>")
                .prop('checked', item.value)
                .attr("id", "userExtRightCBO" + item.roleId)
                .attr("roleId", item.roleId)
               )
             .append($("<label/>").attr('for', "userExtRightCBO" + item.roleId).html(item.name))
             );

     });
     $(".APuserExValO input[type='checkbox']").change(function (e) {
         //console.log(e);
         var elem = $(e.target).parent();

         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/userRightOChange",
             data: JSON.stringify({ roleId: $(e.target).attr("roleId"), userId: $(".APuserExProgItemSelect").attr("userId"), progId: $(".APuserExProgItemSelect").attr("progId"), val: $(e.target).is(':checked') }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     ajaxErr("/testservice.asmx/roleChange error", data);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/roleChange error", data);
             }

         })

     });

     $(".APuserExValO").prepend($("<select/>")
           .change(function (e) {
               var elem = $(e.target).parent();
               $.ajax({
                   type: "POST",
                   contentType: "application/json;",
                   url: serverRoot + "testservice.asmx/userRoleChange",
                   data: JSON.stringify({ roleId: $(e.target).val(), userId: $(".APuserExProgItemSelect").attr("userId"), progId: $(".APuserExProgItemSelect").attr("progId") }),
                   dataType: "json",
                   async: true,
                   success: function (data) {
                       dt = JSON.parse(data.d);
                       if (dt.status < 1)
                           ajaxErr("/testservice.asmx/userRoleChange error", data);
                   },
                   error: function (data) {
                       ajaxErr("/testservice.asmx/userRoleChange error", data);
                   }

               })
           })
         );
     roles.forEach(function (role) {
         $(".APuserExValO select").append($("<option/>")
             .val(role.roleId)
             .html(role.name)
             .prop("selected", role.val)
             );
     });

 }
 function APBlockTypeInit() {
     $('#collapse9').on('hidden.bs.collapse', function () {
         $(".APblockTypeBox").html("").loading50();
     })
     $('#collapse9').on('show.bs.collapse', function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/blockTypeInitAP",
             data: JSON.stringify({ id: 0 }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (data.status < 1)
                     return ajaxErr("/testservice.asmx/blockTypeInitAP error", dt.message);
                 APreloadBlockType(dt.items);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/blockTypeInitAP error", data);
             }

         });

     })
 }
 function APreloadBlockType(data) {
     $(".APblockTypeBox").html("");
     data.forEach(function (item) {
         $(".APblockTypeBox").append(
             $(div)
             .attr("id", "APblockType" + item.id)
             .addClass("socialItem blockTypeItem")
               .attr("typeId", item.id)
             .append(
                 $("<input type='text'/>")
                 .attr("placeholder", langTable['CapName'])
                 .attr("name", "name")
                 .attr("id", "APblockTypeName"+item.id)
                 .val(item.TypeName)
                 .addClass("rsslItemText")
             )
             .append($(div)
             .addClass("APblockTypeVal")
             .attr("typeId", item.id)
             .append($("   <input type='checkbox'  name='APcbIsAutor' ></input>").prop("checked", item.Autor))
             .append("<label for='APcbIsAutor' >" + langTable['CapAutor'] + "</label>")
              .append($("   <input type='checkbox' name='APcbIsCameramen' ></input>").prop("checked", item.Operator))
             .append("<label for='APcbIsCameramen' >" + langTable['CapCameramen'] + "</label>")
              .append($("   <input type='checkbox' name='APcbIsJockey' ></input>").prop("checked", item.Jockey))
             .append("<label for='APcbIsJockey' >" + langTable['CapTalent'] + "</label>")
             )

             
             );
        
     });
     $(".blockTypeItem input").change(APblockTypeItemChange);

     $(".APblockTypeBox").append($(div)
         .append($(div).html(langTable["ClikToItem"]))
         .append($(div)
         .append($("<select></select>")
            .change(doOnClockBlockTypeChange)
            .append($("<option " + (doOnClockBlockType == "normal" ? "selected" : "") + " value='normal'>" + langTable["ClikToItemAuto"] + "</option>"))
            .append($("<option " + (doOnClockBlockType != "normal" ? "selected" : "") + " value='manual'>" + langTable["ClikToItemManual"] + "</option>"))
         )
         )
         );
 }

 function APblockTypeAdd() {

     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/blockTypeAdd",
         data: JSON.stringify({id:0}),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1) 
                 return ajaxErr("/testservice.blockTypeAdd error", data);
             APreloadBlockType(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/blockTypeAdd error", data);
         }

     })
 }
 function APblockTypeItemChange(e)
 {
    
     var id=$(e.target).parent().first().attr("typeId");
     var prm = {
         id: id,
         title: $("#APblockTypeName" + id).val(),
         isAutor: $("#APblockType" + id + " input[name='APcbIsAutor']").is(":checked"),
         isCameramen: $("#APblockType" + id + " input[name='APcbIsCameramen']").is(":checked"),
         isJockey: $("#APblockType" + id + " input[name='APcbIsJockey']").is(":checked"),
     };
    
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/blockTypeEdit",
         data: JSON.stringify(prm),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 return ajaxErr("/testservice.APblockTypeEdit error", data);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/APblockTypeEdit error", data);
         }

     })
   
 }
 function APblockTypeDel(id, event) {

 }

 function APprintTemplateInit() {
     $('#collapsePrintTemplate').on('hidden.bs.collapse', function () {
         $(".APprintTemplateBox").html("").loading50();
     })
     $('#collapsePrintTemplate').on('show.bs.collapse', function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/printTemplateInitAP",
             data: JSON.stringify({ id: 0 }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (data.status < 1)
                     return ajaxErr("/testservice.asmx/APprintTemplateBox error", dt.message);
                 APprintTemplateReload(dt.items);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/APprintTemplateBox error", data);
             }

         });

     })
 }
 function APprintTemplateReload(data)
 {
     $(".APprintTemplateBox").html("");
     data.forEach(function (item) {
         $(".APprintTemplateBox").append(
             $(div)
             .attr("id", "APprintTemplate" + item.id)
             .addClass("socialItem printTemplateItem")
               .attr("typeId", item.id)
             .append(
                 $("<input type='text'/>")
                 .attr("placeholder", langTable['CapName'])
                 .attr("name", "name")
                 .attr("printTemplId", item.id)
                 .val(item.name)
                 .addClass("rsslItemText")
             )

              .append(
              $("<input type='button' name='edit' class='btn btn-warning btn-xs socialItemBtn' onclick=' APprintTemplateEdit(" + item.id + " , event)'/>")
              .val(langTable['CapEdit'])
             )
            .append(
              $("<input type='button' class='btn btn-success btn-xs socialItemBtn' onclick=' APprintTemplateCopy(" + item.id + " , event)'/>")
              .val("copy")
             )
             .append(
              $("<input type='button' name='del' class='btn btn-danger btn-xs socialItemBtn' onclick=' APprintTemplateDelete(" + item.id + " , event)'/>")
              .val(langTable['CapDelete'])
             )
             )
             .append($(div)
                .attr("id", "printTemplateItemExt" + item.id)
                .addClass("socialItemExt")

             );
   

     });
     $(".printTemplateItem input[type='text']").change(APprintTemplChange);
 }

 function APprintTemplateDelete(id, event){
     NFconfirm("Are you sure?", event.pageX - 150, event.pageY, id, APprintTemplateDeleteConfirmed);

 }
 function APprintTemplateDeleteConfirmed(id) {
     console.log("#APprintTemplate" + id);
     $("#APprintTemplate" + id).fadeOut(500, function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/printTemplateDel",
             data: JSON.stringify({ id: id }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (dt.status < 1)
                     ajaxErr("/testservice.asmx/printTemplateDel error", data);
                 APreloadRole(dt.items)
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/printTemplateDel error", data);
             }

         })
     });
 }
 function APprintTemplChange(event)
 {
    
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/printTemplateChangeText",
         data: JSON.stringify({ id: $(event.target).attr("printTemplId"), val: $(event.target).val() }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 ajaxErr("/testservice.asmx/printTemplateChangeText error", data);
             //APreloadRole(dt.items)
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/printTemplateChangeText error", data);
         }

     })

 }

 function APprintTemplAdd() {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/printTemplAdd",
         data: JSON.stringify({ id: 0 }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 return ajaxErr("/testservice.printTemplAdd error", data);
             APprintTemplateReload(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/blockTypeAdd error", data);
    
         }

         });
 };
 function APprintTemplateCopy(id, event) {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/printTemplCopy",
         data: JSON.stringify({ id: id }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 return ajaxErr("/testservice.printTemplCopy error", data);
             APprintTemplateReload(dt.items);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/printTemplCopy error", data);

         }

     });
 }
 function APprintTemplateEdit(id) {
     if ($("#printTemplateItemExt" + id).html().length > 10) {
         $(".socialItemExt").html("");
         return;
     }
     $(".socialItemExt").html("");
     $("#printTemplateItemExt" + id).loading50().load(serverRoot + "elements/printTemplateEditor.aspx?id=" + id);
     $('html, body').animate({
         scrollTop: $("#printTemplateItemExt" + id).offset().top-80
     }, 100);
 }

 function doOnClockBlockTypeChange(e) {
    
     doOnClockBlockType=($(e.currentTarget).val());
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/doOnClockBlockType",
         data:JSON.stringify( { value: doOnClockBlockType }),
         dataType: "json",
         async: true,
         error: function (data) {
             ajaxErr("/testservice.asmx/doOnClockBlockTypeChange error", data);
         }

     })
 }
 function APlayOutInit(){
     $('#collapsePlayOut').on('hidden.bs.collapse', function () {
         $(".APlayOutBox").html("").loading50();
     })
     $('#collapsePlayOut').on('show.bs.collapse', function () {
         $.ajax({
             type: "POST",
             contentType: "application/json;",
             url: serverRoot + "testservice.asmx/playOutInitAP",
             data: JSON.stringify({ id: 0 }),
             dataType: "json",
             async: true,
             success: function (data) {
                 dt = JSON.parse(data.d);
                 if (data.status < 1)
                     return ajaxErr("/testservice.asmx/playOutInitAP error", dt.message);
                 APlayOutReload(dt.items, dt.types);
             },
             error: function (data) {
                 ajaxErr("/testservice.asmx/playOutInitAP error", data);
             }

         });

     })
 }
 function APlayOutReload(items, types) {
     console.log(items);
     $(".APlayOutBox").html("");
     items.forEach(function (item) {
         $(".APlayOutBox").append($("<div id='" + item.id + "' class='socialItem playOutItem'></div>"));
         var $c = $("#" + item.id);
         $c.append($("<div></div>").append($('<input type="text" onblur="" placeholder="Title" name="title" class="socialItemText">').val(item.title)))
         $c.append($("<select class='socialItemSelect'></div>"));
         var $sel = $c.find("select");
         types.forEach(function (type) {
             $sel.append("<option value='"+type.id+"' "+ (type.id==item.typeId?" selected='selected'":"")+" >"+type.Title+"</option>");
         });
         $sel.change(function () { APlayOutChange(item.id) });
         
         $c.append($('<input type="text" onblur="" placeholder="Path to server" name="path" class="socialItemText">').val(item.path));
         $c.append($('<input type="text" onblur="" placeholder="Replace local path" name="replace" class="socialItemText">').val(item.replace));

         $c.append($('<input type="text" onblur="" placeholder="URL to Server" name="url" class="socialItemText">').val(item.url));
         $c.append($('<input type="text" onblur="" placeholder="Srt Prefix" name="SrtPrefix" class="socialItemText">').val(item.SrtPrefix));
         
         $c.append($("<input type='button' class='btn btn-danger btn-xs socialItemBtn' onclick='APlayOutDelete(\"" + item.id + "\")'/>")
         .val(langTable['CapDelete']));
         $c.find("input[type='text']").blur(function () { APlayOutChange(item.id) });
          
         });
 }
 function APlayOutAdd()
 {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/playOutAdd",
         data: JSON.stringify({ id: 0 }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 return ajaxErr("/testservice.playOutAdd error", data);
             APlayOutReload(dt.items, dt.types);
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/playOutAdd error", data);

         }

     });
 }
 function APlayOutDelete(id) {
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/playOutdelete",
         data: JSON.stringify({ id: id }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 return ajaxErr("/testservice.playOutdelete error", data);
             //APlayOutReload(dt.items, dt.types);
             $("#" + id).fadeOut(500, function () { $("#" + id).remove(); });
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/playOutdelete error", data);
         }

     });
 }
 
 function APlayOutChange(id) {
     var $c = $("#" + id);
     $.ajax({
         type: "POST",
         contentType: "application/json;",
         url: serverRoot + "testservice.asmx/playOutChange",
         data: JSON.stringify({
             id: id,
             title: $c.find("input[name='title']").val(),
             path: $c.find("input[name='path']").val(),
             url: $c.find("input[name='url']").val(),
             replace: $c.find("input[name='replace']").val(),
             type: $c.find("select").val(),
             SrtPrefix: $c.find("input[name='SrtPrefix']").val(),
         }),
         dataType: "json",
         async: true,
         success: function (data) {
             dt = JSON.parse(data.d);
             if (dt.status < 1)
                 return ajaxErr("/testservice.playOutdelete error", data);
             //APlayOutReload(dt.items, dt.types);
            // $("#" + id).fadeOut(500, function () { $("#" + id).remove(); });
         },
         error: function (data) {
             ajaxErr("/testservice.asmx/playOutdelete error", data);

         }

     });
}
///////

function ATitleOutInit() {
    $('#collapseTitleOut').on('hidden.bs.collapse', function () {
        $(".ATitleOutBox").html("").loading50();
    })
    $('#collapseTitleOut').on('show.bs.collapse', function () {
        $.ajax({
            type: "POST",
            contentType: "application/json;",
            url: serverRoot + "testservice.asmx/titleOutInitAP",
            data: JSON.stringify({ id: 0 }),
            dataType: "json",
            async: true,
            success: function (data) {
                dt = JSON.parse(data.d);
                if (data.status < 1)
                    return ajaxErr("/testservice.asmx/titleOutInitAP error", dt.message);
                ATitleOutReload(dt.items, dt.types);
            },
            error: function (data) {
                ajaxErr("/testservice.asmx/titleOutInitAP error", data);
            }

        });

    })
}
function ATitleOutReload(items, types) {
    
    $(".ATitleOutBox").html("");
    items.forEach(function (item) {
        $(".ATitleOutBox").append($("<div id='" + item.id + "' class='socialItem titleOutItem'></div>"));
        var $c = $("#" + item.id);
        $c.append($("<div></div>").append($('<input type="text" onblur="" placeholder="Title" name="title" class="socialItemText">').val(item.title)))
       
        $c.append($('<textarea placeholder="css" name="url" class="socialItemText"></textarea>').val(item.css));

        $c.append($("<input type='button' class='btn btn-danger btn-xs socialItemBtn' onclick='ATitleOutDelete(\"" + item.id + "\")'/>")
            .val(langTable['CapDelete']));
        $c.find("input[type='text']").blur(function () { ATitleOutChange(item.id) });
        $c.find("textarea").blur(function () { ATitleOutChange(item.id) });

    });
}
function ATitleOutAdd() {
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot + "testservice.asmx/titleOutAdd",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        async: true,
        success: function (data) {
            dt = JSON.parse(data.d);
            if (dt.status < 1)
                return ajaxErr("/testservice.titleOutAdd error", data);
            ATitleOutReload(dt.items, dt.types);
        },
        error: function (data) {
            ajaxErr("/testservice.asmx/titleOutAdd error", data);

        }

    });
}
function ATitleOutDelete(id) {
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot + "testservice.asmx/titleOutdelete",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        async: true,
        success: function (data) {
            dt = JSON.parse(data.d);
            if (dt.status < 1)
                return ajaxErr("/testservice.titleOutdelete error", data);
            //APlayOutReload(dt.items, dt.types);
            $("#" + id).fadeOut(500, function () { $("#" + id).remove(); });
        },
        error: function (data) {
            ajaxErr("/testservice.asmx/titleOutdelete error", data);
        }

    });
}

function ATitleOutChange(id) {
    var $c = $("#" + id);
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url: serverRoot + "testservice.asmx/totleOutChange",
        data: JSON.stringify({
            id: id,
            title: $c.find("input[name='title']").val(),
            css: $c.find("textarea").val(),
            
        }),
        dataType: "json",
        async: true,
        success: function (data) {
            dt = JSON.parse(data.d);
            if (dt.status < 1)
                return ajaxErr("/testservice.titleOutChange error", data);
            //APlayOutReload(dt.items, dt.types);
            // $("#" + id).fadeOut(500, function () { $("#" + id).remove(); });
        },
        error: function (data) {
            ajaxErr("/testservice.asmx/titleOutChange error", data);

        }

    });
}





