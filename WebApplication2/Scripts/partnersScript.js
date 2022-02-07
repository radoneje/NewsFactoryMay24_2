$(document).ready(function () {

    $("#partnersProgr").change(updatePartnersNews);
    updatePartnersPrograms();

    $(".partnersRefreshButton").click(updatePartnersPrograms);


});

function updatePartnersPrograms() {
    $("#partnersFindNewsContainer").html();
    $("#partnersFindNewsContainer").loading50();
   
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/partnersProgramsGet",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            console.log(dt);
            $("#partnersProgr option").remove();
            dt.forEach(function (elem) {
                $("#partnersProgr").append($("<option></option>").attr("value", elem.id).html(elem.clientTitle + " / " + elem.title));
            });
            $("#partnersProgr option").first().attr("selected", "selected");
            updatePartnersNews();
        }
    })
}
function updatePartnersNews() {

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/partnersNewsGet",
        data: JSON.stringify({ progId: $("#partnersProgr").val() }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            $("#partnersFindNewsContainer").html("")
            dt.forEach(function (item) {
                $("#partnersFindNewsContainer").append("<div class='partnersNewsItem ' newsId='"+item.id+"'><h5><span class='partnerNTitle'>"+item.title+"</span><br><small>"+moment(item.newsDate).format("DD.MM.YYYY HH:mm")+"</small></h5></div>")
            });
            $(".partnersNewsItem").click(clickPartnerItem)
            $("#partnerNewsTitle").html("");
            $("#partnerNewsSubTitle").html("");
        }
    })
}
function clickPartnerItem(e) {
    $(".partnersNewsItem").removeClass("newsSelected");
    $(e.currentTarget).addClass("newsSelected");
    $("#partnerNewsTitle").html($(e.currentTarget).find(".partnerNTitle").text());
    $("#partnerNewsSubTitle").html($(e.currentTarget).find("small").text());
    var newsId = $(e.currentTarget).attr("newsId");
    $("#PartnersFindBlocksContainer").loading50();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/partnersBlocksGet",
        data: JSON.stringify({ id: newsId }),
        dataType: "json",
        success: function (data) {
            var dt = JSON.parse(data.d);
            console.log(dt)
            $("#PartnersFindBlocksContainer").html("")
            dt.forEach(function (item) {
                var cls = ""
                
                if (item.isReady)
                    cls = "ready";
                if (item.isApprove)
                    cls = "approve";

                $("#PartnersFindBlocksContainer").append(
                    $("<div class='BlockPartnerNameRowContainer " + cls+"' blockId='" + item.id + "'></div>")
                        .append(GeneratePartnersBlockBlankDivContainer(item))
                    
                );

                $("#ArchiveBlockTypeNameControl" + item.id).html(item.type);
                $("#ArchiveBlockNameControl" + item.id).html(item.title);
                $("#ArchiveBlockNameBlockTimeControl" + item.id).html('');
                $("#ArchiveBlockNameTaskTimeControl" + item.id).html('' );
                $("#ArchiveBlockImageControl" + item.id).html(/*ReloadBlockImage(RowData)*/);


               // $("#ArchiveSubBlockOperatorControl" + RowData.Id).html(RowData.BlockOperator == '' ? '' : (langTable['CapCameramen'] + ": " + RowData.BlockOperator));
                $("#ArchiveSubBlockAutorControl" + item.id).html(item.author == '' ? '' : (langTable['CapAutor'] + ": " + item.author));
                $("#ArchiveSubBlockTextControl" + item.id).html('<small>' + item.text.replace(/\(\(NF[^\)]+\)\)/g, ReloadSubBlockDataReplacer) + '</small>');
                $("#ArchiveSubBlockImageControl" + item.id).html($('<img class="media-object blockListImage" src="' + serverRoot + 'handlers/GetArchiveBlockImage.ashx?BlockId=' + item.id + '&Rnd=' + parseInt(Math.random() * 10000) + '" width="100" height="56">')
                    .click(function () { ShowArchiveVideo(item.id) })
                );

            });
            
        }
    })

    
}
function GeneratePartnersBlockBlankDivContainer(data) {

    return '<div   id="PartnersFindBlocksContainer' + data.id + '" class="BlockArchiveNameRowContainer">\
                        <div onclick="ClickArchiveBlockNameRowContainer(' + data.id + ')" class="BlockArchiveNameRowFirstContainer" id="BlockNameRowFirstContainer' + data.BlockId + '">\
                            <table width="564" border=0>\
                                <tr >\
                                    <td width="464">\
                                        <div id="ArchiveBlockContent' + data.id + '" class="ArchiveBlockContent" ><h5>\
                                              \
                                                <small><div class="BlockTypeNameControl" id="ArchiveBlockTypeNameControl' + data.id + '"></div></small>\
                                                <div class="BlockArchiveNameControl" id="ArchiveBlockNameControl' + data.id + '"></div>\
                                          </div></h5>\
                                    <td>\
                                    <td  width="50"><div class="BlockNameTaskTimeControl" id="ArchiveBlockNameTaskTimeControl' + data.id + '"></div></td>\
                                    <td  width="50"><div class="BlockNameBlockTimeControl" id="ArchiveBlockNameBlockTimeControl' + data.id + '"></div></td>\
                                </tr>\
                                    \
                            </table>\
                        </div >\
                </div>' + GeneratePartnersSubBlockBlankDivContainer(data);
}
function GeneratePartnersSubBlockBlankDivContainer(data) {
    var SubBlockConteinerId = "ArchiveFindSubBlocksContainer" + data.id;
    return '<div  id="' + SubBlockConteinerId + '" class="ArchiveFindSubBlocksContainer">\
                        <table><tr><td valign="top">\
                        </td><td padding=3>\
                            <div class="media">\
                               <div class="media-left">\
                                \
                                   <div id="ArchiveSubBlockImageControl' + data.id + '" class="ArchiveSubBlockImageControl">\
                                        <img class="media-object" src="..." alt="..." width="64" height="64">\
                                    </div>\
                                </div>\
                            <div class="media-body">\
                                 <h5 class="media-heading"><div id="ArchiveSubBlockAutorControl' + data.id + '" class="SubBlockAutorControl">Автор</div>\
                               </h5>\
                                 <div id="ArchiveSubBlockTextControl' + data.id + '" class="SubBlockTextControl"></div>\
                            </div>\
                        </div></td></tr></table> \
             </div> ';
}