
$(function () { $("#ArchiveOverload").hide(); });
$(function() {
 
    $('#ArchiveSearchDatarange1 span').html(langTable['DateRangerCap']);
 
    $('#ArchiveSearchDatarange1').daterangepicker({
        format: langTable["calendarDateFormat"],
        startDate: moment().subtract(29, 'days'),
        endDate: moment(),
        minDate: '01/01/1991',
        dateLimit: { days: 60 },
        showDropdowns: true,
        showWeekNumbers: false,
        timePicker: false,
        opens: 'right',
        buttonClasses: ['btn', 'btn-xs'],
        applyClass: 'btn-primary',
        cancelClass: 'btn-default',
        separator: ' till ',
        locale: langTable['calendarLocale']
    }, function(start, end, label) {
        console.log(start.toISOString(), end.toISOString(), label);
        ArchiveStartDate=start.toISOString();
        ArchiveEndDate=end.toISOString()
        $('#ArchiveSearchDatarange1 span').html(start.format('DD.MM.YYYY') + ' - ' + end.format('DD.MM.YYYY'));
    });
 
});// datetimepicker for Archiv
$(function () { DaDInitDropByElement($("#ArchiveNewsContainer")); })
var ArchiveStartDate;
var ArchiveEndDate;
function ArchiveSearch() {

    
    var Cookie = getCookie("NFWSession");
    var txt = $("#ArchiveText").val() || "  ";
    if (txt.length < 3)
        txt += "   ";
        var jdata = {
            Cookie: Cookie,
            EditorId:$("#ArchiveAutorDropDown").val(),
            StartDate: ArchiveStartDate || (new Date(1999,0,0,0,0,0,0)).toISOString() ,
            EndDate: ArchiveEndDate || (new Date().toISOString()),
            Text: txt,// $("#ArchiveText").val(),
            ProgramId: $("#ProgramDropDown").val(),
    }

   

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot +"testservice.asmx/ArchiveSearch",
            data: JSON.stringify(jdata),
            dataType: "json",
            async: true,
            success: FindArchiveNewsResponce,
            error: AjaxFailed
        }).getAllResponseHeaders();
       
        SetValuesWileFindProccesed()
}
function arhiveSearchMounth(iMounts) {
    var Cookie = getCookie("NFWSession");
    var txt = $("#ArchiveText").val() || "  ";
    if (txt.length < 3)
        txt += "   ";
    var endDate = moment().subtract(iMounts - 1, 'months')
    var startDate = moment().subtract(iMounts, 'months')
    var jdata = {
        Cookie: Cookie,
        EditorId: $("#ArchiveAutorDropDown").val(),
        StartDate: startDate.toISOString(),// ArchiveStartDate || (new Date(1999, 0, 0, 0, 0, 0, 0)).toISOString(),
        EndDate: endDate.toISOString(), //ArchiveEndDate || (new Date().toISOString()),
        Text:  $("#ArchiveText").val(),
        ProgramId: $("#ProgramDropDown").val(),
    }



    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/ArchiveSearch",
        data: JSON.stringify(jdata),
        dataType: "json",
        async: true,
        success: FindArchiveNewsResponce,
        error: AjaxFailed
    }).getAllResponseHeaders();

    SetValuesWileFindProccesed()

}
function   SetValuesWileFindProccesed(){

    $("#ArchiveBlocksHeaderContainer").html("<h4>"+ langTable['ArchiveSearching'] +"</h4>");
   $("#ArchiveOverload").hide();
    $("#ArchiveFindNewsContainer").empty();
    $("#ArchiveFindBlocksContainer").empty();

}

function FindArchiveNewsResponce(data)
{
   
    if (data.Error == "true")
    {
        GrlobalWarning("Error: "+ data.Message);
    }
    var list = JSON.parse(data.d);
    if (parseInt(list.Count) > 99) {
        $("#ArchiveOverload").show();
        $("#ArchiveBlocksHeaderContainer").html("");
        

    }
    else {
        $("#ArchiveOverload").hide();
        $("#ArchiveBlocksHeaderContainer").html("<h4>" + langTable['ArchiveSearchCount'] +": "+ list.Count + "</h4>");
    }

    if (list.Count > 0 && $("#ArchiveSearchClearButton").length==0) {
        $("#ArchiveSearchControlButtonContainer").append('<input class="btn btn-default btn-xs caption caption-value" captionId="ArchiveSearchClearButton" type="button" id="ArchiveSearchClearButton" onclick="ArchiveSearchClearResults()"/><script>$("#ArchiveSearchClearButton").val(langTable["ArchiveSearchClearButton"])</script>');
    }
 
    list.Blocks.forEach(AddArchiveNewsContainer)

}
function AddArchiveNewsContainer(data)
{
    
    if($("#ArchiveNewsItem"+data.NewsId).length==0)
    {
        $("#ArchiveFindNewsContainer").append(GenerateArchiveNewsItem(data));
        $("#ArchiveNewsItem" + data.NewsId).attr("Blocks", "0, " + data.BlockId);
        $("#ArchiveNewsItem" + data.NewsId).attr("NewsId", data.NewsId);
        //////////////// 
      

    }
    else {
        var attr = $("#ArchiveNewsItem" + data.NewsId).attr("Blocks");
        $("#ArchiveNewsItem" + data.NewsId).attr("Blocks", attr + ", " + data.BlockId);
       
    }
    $("#ArchiveNewsItem" + data.NewsId).attr("FindedText", data.RequestString);
    DaDInitDragByElement($("#ArchiveNewsItem" + data.NewsId));

}
function GenerateArchiveNewsItem(data) {
    var ret = '<div id="ArchiveNewsItem' + data.NewsId + '" class="ArchiveNewsItem" onclick="ClickArchiveNewsItem(this);">\
<h5>' + data.NewsName + '<br><small>' + data.NewsDate + '</small></h5>\
    </div>';
    return ret;
}
function ClickArchiveNewsItem(objArchiveNewsItem)
{
    $(".ArchiveNewsItem").removeClass("newsSelected");
    $(objArchiveNewsItem).addClass("newsSelected");
    
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        EditorId: $(objArchiveNewsItem).attr("FindedText"),
        StartDate: "",
        EndDate: "",
        Text: $(objArchiveNewsItem).attr("Blocks"),
        ProgramId: $(objArchiveNewsItem).attr("id"),
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/ArchiveSearchBlocks",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: FindArchiveBlockResponce,
        error: AjaxFailed
    }).getAllResponseHeaders();
    $("#ArchiveFindBlocksContainer").empty();

            $('.divarchnewsbuttons').remove();// удаляем кнопки на всех элемантах
            
                // рисуем кнопки на выбранном элемен
            
             $(objArchiveNewsItem).append(CreateArchNewsButtons($(objArchiveNewsItem).attr("NewsId")));
           $('[data-toggle="tooltip"]').tooltip();
            
       
    //////////////

}
function FindArchiveBlockResponce(data)
{
    ReloadArchiveBlockContainer(JSON.parse(data.d));
    ReloadArchiveBlockData(JSON.parse(data.d));
    if($(document).width()<960)
    scrollToElement($("#ArchiveBlocksContainer"));
}
function ReloadArchiveBlockContainer(data)
{
    
    data.forEach(AddArchiveBlockDiv);
}
function AddArchiveBlockDiv(dataRow)
{
    var data = JSON.parse(dataRow)
    

    if ($('#BlockArchiveNameRowContainer' + data.BlockId).length < 1)// если контейнер блоков не существует, добавляем
    {
        
        $("#ArchiveFindBlocksContainer").append(GenerateArchiveBlockBlankDivContainer(data));
        $("#ArchiveFindSubBlocksContainer" + +data.BlockId).hide();
       // DaDInitDragByElement($('#BlockContent' + RowData.Id));
       // DaDInitDropByElement($('#BlockNameRowContainer' + RowData.Id));
    }
}
function GenerateArchiveBlockBlankDivContainer(data)
{
    
    return '<div   id="ArchiveFindBlocksContainer' + data.BlockId + '" class="BlockArchiveNameRowContainer">\
                        <div onclick="ClickArchiveBlockNameRowContainer(' + data.BlockId + ')" class="BlockArchiveNameRowFirstContainer" id="BlockNameRowFirstContainer' + data.BlockId + '">\
                            <table width="564" border=0>\
                                <tr >\
                                    <td width="464">\
                                        <div id="ArchiveBlockContent' + data.BlockId + '" class="ArchiveBlockContent" ><h5>\
                                              \
                                                <small><div class="BlockTypeNameControl" id="ArchiveBlockTypeNameControl' + data.BlockId + '"></div></small>\
                                                <div class="BlockArchiveNameControl" id="ArchiveBlockNameControl' + data.BlockId + '"></div>\
                                          </div></h5>\
                                    <td>\
                                    <td  width="50"><div class="BlockNameTaskTimeControl" id="ArchiveBlockNameTaskTimeControl' + data.BlockId + '"></div></td>\
                                    <td  width="50"><div class="BlockNameBlockTimeControl" id="ArchiveBlockNameBlockTimeControl' + data.BlockId + '"></div></td>\
                                </tr>\
                                    \
                            </table>\
                        </div >\
                </div>' + GenerateArchiveSubBlockBlankDivContainer(data);
}
function GenerateArchiveSubBlockBlankDivContainer(data)
{
    var SubBlockConteinerId =  "ArchiveFindSubBlocksContainer"+ data.BlockId;
    return '<div  id="' + SubBlockConteinerId + '" class="ArchiveFindSubBlocksContainer">\
                        <table><tr><td valign="top">\
                        </td><td padding=3>\
                            <div class="media">\
                               <div class="media-left">\
                                \
                                   <div id="ArchiveSubBlockImageControl' + data.BlockId + '" class="ArchiveSubBlockImageControl">\
                                        <img class="media-object" src="..." alt="..." width="64" height="64">\
                                    </div>\
                                </div>\
                            <div class="media-body">\
                                 <h5 class="media-heading"><div id="ArchiveSubBlockAutorControl' + data.BlockId + '" class="SubBlockAutorControl">Автор</div>\
                                <div id="ArchiveSubBlockOperatorControl' + data.BlockId + '" class="SubBlockOperatorControl">Оператор</div></h5>\
                                 <div id="ArchiveSubBlockTextControl' + data.BlockId + '" class="SubBlockTextControl"></div>\
                            </div>\
                        </div></td></tr></table> \
             </div> ';
}
///// get Data
function  ReloadArchiveBlockData(data)
{
    
   
    data.forEach(function (RowData) {
        RowData = JSON.parse(RowData)
        RowData.Id = RowData.BlockId;
       // log(ReloadBlockImage(RowData));
        $("#ArchiveBlockTypeNameControl" + RowData.BlockId).html(RowData.BlockTypeName);
        $("#ArchiveBlockNameControl" + RowData.BlockId).html(RowData.BlockName);
        $("#ArchiveBlockNameBlockTimeControl" + RowData.BlockId).html(RowData.BlockTime);
        $("#ArchiveBlockNameTaskTimeControl" + RowData.BlockId).html(RowData.TaskTime);
        $("#ArchiveBlockImageControl" + RowData.BlockId).html(/*ReloadBlockImage(RowData)*/);

        //// субблок

        var text = RowData.BlockText.replace(/\(\(NF[^\)]+\)\)/g, ReloadSubBlockDataReplacer);
        text = text.replace(/\n/g, "<span class='break'></span>\n");
        text = text.replace(/\((ПОДВОДКА:[^\)]+)\)/gmi, (a, b) => { return "<span style='color:green'>" + b + "</span>" });
        text = text.replace(/\(\([^\)]+\)\)/g, function (a) {
            var cls = "";
            a.replace(/^\(\(([A-Z]{2,5})\:*\s/, function (b, mark) {
                cls = (mark);

                // b=b.replace("TITLE", "RRRRR");
                return b;
            });

            a = a.replace("TITLE:", "<span class='subBlockMark'>TITLE:</span>")
            a = a.replace("NAME:", "<span class='subBlockMark'>NAME:</span>")
            a = a.replace("TEXT:", "<span class='subBlockMark'>TEXT:</span>")
            a = a.replace("SOT:", "<span class='subBlockMark'>SOT:</span>")

            return "<span class='colorText " + cls + "'>" + a + "</span>";
        });

        $("#ArchiveSubBlockOperatorControl" + RowData.Id).html(RowData.BlockOperator == '' ? '' : (langTable['CapCameramen'] + ": " + RowData.BlockOperator));
        $("#ArchiveSubBlockAutorControl" + RowData.Id).html(RowData.BlockAutor == '' ? '' : (langTable['CapAutor'] + ": " + RowData.BlockAutor));
        $("#ArchiveSubBlockTextControl" + RowData.Id).html('<small>' + text+'</small>');
        $("#ArchiveSubBlockImageControl" + RowData.Id).html($('<img class="media-object blockListImage" src="' + serverRoot + 'handlers/GetArchiveBlockImage.ashx?BlockId=' + RowData.BlockId + '&Rnd=' + parseInt(Math.random() * 10000) + '" width="100" height="56">')
            .click(function () { ShowArchiveVideo(RowData.Id) })
            );


    });
   // var datar = JSON.parse(data);

   // datar.forEach(function (RowData) {
        /*
       // log("__>>"+RowData.TypeName);
        $("#ArchiveBlockTypeNameControl" + RowData.BlockId).html(RowData.TypeName);
        $("#ArchiveBlockNameControl" + RowData.BlockId).html(TypeName);
        $("#ArchiveBlockNameBlockTimeControl" + RowData.BlockId).html(HilightBlockTime(RowData.BlockTime, RowData.TaskTime));
        $("#ArchiveBlockNameTaskTimeControl" + RowData.BlockId).html(HilightTaskTime(RowData.TaskTime));
        $("#ArchiveBlockImageControl" + RowData.Blockd).html(ReloadBlockImage(RowData));
        
        Approve: trueBlockAitor: ""BlockId: 46003650265149BlockName: "Новый блок"BlockText: ""BlockTime: "00:00:00"BlockTypeName: "Студия"Ready: trueTaskTime: "00:00:00"
        BlockId = bl.Id,
                        BlockName = HiLiteFind(bl.Name, InData.Text),
                        BlockText = HiLiteFind(bl.BlockText, InData.EditorId),
                        BlockTypeName = bl.TypeName,
                        BlockAutor = bl.Autor,
                        Ready = bl.Ready,
                        Approve = bl.Approve,
                        BlockTime = TimeSpan.FromSeconds((double)bl.BlockTime).ToString(),
                        TaskTime = TimeSpan.FromSeconds((double)bl.TaskTime ).ToString(),
        */

   // });
}
function ClickArchiveBlockNameRowContainer(BlockId) {

    if ($("#ArchiveFindSubBlocksContainer" + BlockId).is(":visible"))
    {
        $("#ArchiveFindSubBlocksContainer" + BlockId).hide();
    }
    else {
        $("#ArchiveFindSubBlocksContainer" + BlockId).show();
    }
}
function ArchiveSearchClearResults(){
    $("#ArchiveFindNewsContainer").empty();
    $("#ArchiveFindBlocksContainer").empty();
    $("#ArchiveSearchClearButton").remove();
    $("#ArchiveText").val("");
    $("#ArchiveBlocksHeaderContainer").html("");
    $("#ArchiveOverload").hide();
    $("#ArchiveBlocksHeaderContainer").html("");
    $('#ArchiveSearchDatarange1 span').html(langTable['DateRangerCap']);
    ArchiveStartDate = null;
    ArchiveEndDate = null;
   // $("#ArchiveAutorDropDown")[0].selectIndex("0");

}

function CreateArchNewsButtons(sBlockId) {
    
    return "\
    <div class='divarchnewsbuttons'><p><div class=\"btn-group btn-group-xs\" role=\"group\" aria-label=\"...\">\
<div class=\"btn-group btn-group-xs\" role=\"group\">\
\
    <button type=\"button\" class=\"btn btn-default\" onclick=\"DaDToArchiveToNews(" + sBlockId + ",0)\" data-toggle='tooltip' data-placement='top' data-original-title='Создать текущий выпуск'><span class='glyphicon glyphicon-open' aria-hidden='true'></span></button>\
\
          \
  </div>\
</div>";
   
}
function DaDToArchiveToNews(FromId) {
    log(FromId);
    var jdata = {
        sSourceId: FromId,
        sDestGroupId: 0,
        sDestProgramId: ProgramDropDown.options[ProgramDropDown.selectedIndex].value,
        Coockie: getCookie("NFWSession")

    };
    log(jdata);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/ArchiveToNews",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: NewsMoveSucceeded,
        error: AjaxFailed
    }).getAllResponseHeaders();
}


///log(fixdiv);
