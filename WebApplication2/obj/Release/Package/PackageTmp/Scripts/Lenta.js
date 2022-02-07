$(function () { $("#LentaOverload").hide(); });
$(function () {

    $('#LentaSearchDatarange1 span').html(langTable['DateRangerCap']);

    $('#LentaSearchDatarange1').daterangepicker({
        format: langTable["calendarDateFormat"],
        startDate: moment(),
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
      /*  locale: {
            applyLabel: 'Подтвердить',
            cancelLabel: 'Отменить',
            fromLabel: 'С',
            toLabel: 'По',
            customRangeLabel: 'Выбор',
            daysOfWeek: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            firstDay: 1
        }*/
        locale:
            langTable["calendarLocale"]
    }, function (start, end, label) {
        
        LentaStartDate = start.toISOString();
        LentaEndDate = end.toISOString()
        $('#LentaSearchDatarange1 span').html(start.format('DD.MM.YYYY') + ' - ' + end.format('DD.MM.YYYY'));
    });

});// datetimepicker for Archiv
var LentaStartDate;
var LentaEndDate;
function LentaSearch() {
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        EditorId: $("#LentaSourceDropDown").val(),
        StartDate: LentaStartDate,
        EndDate: LentaEndDate,
        Text: $("#LentaText").val(),
        ProgramId: $("#ProgramDropDown").val(),
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/LentaSearch",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: FindLentaNewsResponce,
        error: AjaxFailed
    }).getAllResponseHeaders();

    SetLentaValuesWileFindProccesed()
}
function SetLentaValuesWileFindProccesed() {
    $("#LentaBlocksHeaderContainer").html("<h4>" + langTable['PassIsChecking'] + "</h4>");
    $("#LentaOverload").hide();
    $("#LentaFindNewsContainer").empty();
    $("#LentaFindBlocksContainer").empty();
}
function FindLentaNewsResponce(data) {
    var list = JSON.parse(data.d);

    if (parseInt(list.Count) > 99) {
        $("#LentaOverload").show();
        $("#LentaBlocksHeaderContainer").html("");
    }
    else {
        $("#LentaOverload").hide();
        $("#LentaBlocksHeaderContainer").html("<h4>" + langTable['ArchiveSearchCount'] + ":" + list.Count + "</h4>");
    }
    if (list.Count > 0 && $("#LentaSearchClearButton").length == 0) {
        $("#LentaSearchControlButtonContainer").append('<button class="btn btn-default btn-xs" type="button" id="LentaSearchClearButton" onclick="LentaSearchClearResults()">' + langTable['ArchiveSearchClearButton'] + '</button>');
    }
    ReloadLentaBlockContainer(JSON.parse(data.d));
    ReloadLentaBlockData(JSON.parse(data.d));
}
function LentaSearchClearResults() {
    $("#LentaFindNewsContainer").empty();
    $("#LentaFindBlocksContainer").empty();
    $("#LentaSearchClearButton").remove();
    $("#LentaText").val("");
    $("#LentaBlocksHeaderContainer").html("");
    $("#LentaOverload").hide();
    $("#LentaBlocksHeaderContainer").html("");
    $('#LentaSearchDatarange1 span').html(langTable['DateRangerCap']);
    LentaStartDate = null;
    LentaEndDate = null;
    // $("#ArchiveAutorDropDown")[0].selectIndex("0");

}
function ReloadLentaBlockContainer(data) {
    data.Blocks.forEach(AddLentaBlockDiv);
    
}

function AddLentaBlockDiv(data) {
    

    if ($('#BlockLentaNameRowContainer' + data.Id).length < 1)// если контейнер блоков не существует, добавляем
    {

        $("#LentaFindBlocksContainer").append(GenerateLentaBlockBlankDivContainer(data));
        DaDInitDragByElement($("#LentaBlockTitle" + data.Id));
      //  $("#LentaFindSubBlocksContainer" +data.Id).hide();
        // DaDInitDragByElement($('#BlockContent' + RowData.Id));
        // DaDInitDropByElement($('#BlockNameRowContainer' + RowData.Id));
    }
}
function GenerateLentaBlockBlankDivContainer(data)
{
    return '\
    <div class="media">\
  <div class="media-left media-top">\
    \
      <div id="LentaBlockImage' + data.Id + '"></div>\
    \
  </div>\
  <div class="media-body">\
    <h5 class="media-heading"><div id="LentaBlockTitle' + data.Id + '"></h5>\
    <div id="LentaBlockText' + data.Id + '"></div>\
  </div>\
</div>';
    ;// + GenerateArchiveSubBlockBlankDivContainer(data);
}
function ClickLentaBlockNameRowContainer(data) {

}

function ReloadLentaBlockData(data) {
   // log(data);
    data.Blocks.forEach(function (RowData) {
       // RowData = JSON.parse(RowData)
        RowData.Id = RowData.Id;
        //log(ReloadBlockImage(RowData));
        $("#LentaBlockImage" + RowData.Id).html('<img class="media-objects" src="' + RowData.Img + '" width=70></img>');
        
        $("#LentaBlockTitle" + RowData.Id).html("<small>" + RowData.AgencyName + ' </small><a href="' + RowData.Link + '" target="_blank" style="cursor:pointer">' + RowData.Title + "</a><br><small>" + RowData.Date + '</small>');
        $("#LentaBlockText" + RowData.Id).html('<small> ' + RowData.Text + '</small></br>');
        /*
        public string Id { get; set; }
            public string Title { get; set; }
            public string AgencyName { get; set; }
            public string Link { get; set; }
            public string Img { get; set; }
            public string Text { get; set; }
            public string Date { get; set; }
        */

    });

}