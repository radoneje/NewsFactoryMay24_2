$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip({
        placement: 'top'
    });

 //   console.log(".RssFixedHeightContainer");
   // console.log(localStorage.getItem("RssVisible"));
    if(localStorage.getItem("RssVisible")=="true")
        $(".RssFixedHeightContainer").show();

    setTimeout(InitRss, 4000);
});

function capLentaClick()
{
   // console.log($(".RssFixedHeightContainer").is(":visible"));
    if ($(".RssFixedHeightContainer").is(":visible"))
        $(".RssFixedHeightContainer").hide();
    else
        $(".RssFixedHeightContainer").show();

    localStorage.setItem("RssVisible", $(".RssFixedHeightContainer").is(":visible"));

}
function InitRss() {
    RssDataRequest();
}
function RssDataRequest()
{
    var Cookie = getCookie("NFWSession");
    var jdata = {
        Cookie: Cookie,
        NewsId: 0
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url:  serverRoot+"testservice.asmx/GetListOfRss",
        data: JSON.stringify(jdata, null, 2),
        dataType: "json",
        async: true,
        success: RssDataResponce,
        error: function () { setTimeout(RssDataRequest, 1000 * 60 * 3);console.warn("rss not update");}
    });
    
    
}
function RssDataResponce(data)
{
    var list = JSON.parse(data.d);
    ReloadRssContainer(list);
  
    ReloadRssData(list);
    setTimeout(RssDataRequest, 1000 * 60 * 3);//каждые три минуты
}
function ReloadRssContainer(data) {
    if (!(typeof data === "undefined")) {//если в данных есть выпуски
        data.forEach(AddRssDiv);
    }

    return;
}
function AddRssDiv(data) {
    var ContainerId = "RssItem" + data.Id;
    if ($("#" + ContainerId).length <= 0) {
        $("#RssContent").append(GenerateBlankDiv(data.Id));
        
    }
    DaDInitDragByElement($("#RssItemTitle" + data.Id));

}
function GenerateBlankDiv(ItemId)
{
    return '<div class="RssItem" id="RssItem' + ItemId + '">\
    <table class="RssItemTable">\
      <tr>\
        <td  class="RssItemTableCell"><div id="RssItemImage' + ItemId + '"></div></td>\
        <td class="RssItemTableCell"><h6><small><div id="RssItemTitle' + ItemId + '"></small></h6></div></td> \
      </tr>\
      \
    </table>\
    </div>';
}

function ReloadRssData(data) {
    
    if (!(typeof data === "undefined")) {//если в данных есть выпуски
        data.forEach(AddRssRowData);
    }
    if($(".RssItem").length>40)
    {
        while ($(".RssItem").length > 40)
            $(".RssItem").last().remove();
    }
    var i=10000;
    data.forEach(function (RssItem) {
        $("#RssItem"+RssItem.Id).attr("SortOrder",i );
        i--;
    });
    SortDivs($("RssContent"), "SortOrder");

}
function    AddRssRowData(data) {
    $('#RssItemTitle' + data.Id).html('<small>' + data.pubDate + '</small><br><a href="' + data.Link + '"target="_blank"  >' + data.Title + '</a>');
    //$('#RssItemDate' + data.Id).html(data.pubDate.replace(" ","<br>"));
    $('#RssItemImage' + data.Id).html('<img class="RssImage"  width=50 src="' + data.Image + '"></img>');
}

