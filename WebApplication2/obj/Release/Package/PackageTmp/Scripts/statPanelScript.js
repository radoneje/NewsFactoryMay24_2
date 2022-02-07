$(document).ready(function () {
    statFromActivate();
});

function statFromActivate() {
    $('#statPanelSearchDatarange span').html(langTable['DateRangerCap']);
   // $("#dateEnd").val(moment());
   // $("#dateStart").val(moment().subtract(1, 'mounth'));

    $('#statPanelSearchDatarange').daterangepicker({
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
        locale: 
            langTable["calendarLocale"]
        
    }, function(start, end, label) {
        $("#dateStart").val(start.toISOString());
        $("#dateEnd").val(end.toISOString());
        $('#statPanelSearchDatarange span').html(start.format('DD.MM.YYYY') + ' - ' + end.format('DD.MM.YYYY'));
        statGet();
    });

   
}
function statGet() {
    if( $("#dateStart").val().length<4 || 
    $("#dateEnd").val().length < 4)
    {
        $('#statPanelSearchDatarange').addClass( "controlError");
        setTimeout(function () { $('#statPanelSearchDatarange').removeClass("controlError", "") }, 2000);
        return;
    }

   
    var dt = $("#statPanelSearchForm").getFormData();
    var groupBy = 'autor';
    $(".statParamBtn").each(function () {
        if ($(this).hasClass("btn-success"))
            groupBy = $(this).attr("name");
    });
    dt.params.groupBy = groupBy;
    $(".statForm  .gridControls:last").html("").append($("<img></>").attr("src", "/images/loading.gif").addClass("loadingImg"));
    sendFormDataToService(dt.serviceName, dt.params, function (data) {
        $(".statForm  .gridControls:last img").fadeOut(500, function () {
            $(".statPanelRes").html('');
       
        $(".statPanelRes").remove();
            $(".gridControls:last").css("background", "WHITE")
            .append($(div)
            .addClass("statPanelRes")
            .append("<h4>Результаты</h4>"));
        
            var dt = JSON.parse(data.d);
            dt.forEach(function (user) {
                $(".statPanelRes").append($(div).addClass("statPanelUser")
                .append($(div)
                    .addClass("statPanelUserTitle")
                    .html(user.name)
                        )
                    .append($(div)
                    .addClass("statPanelUserCount")
                    .html(user.count)
                        )
                .append($(div).addClass("statPanelUserTotal"))

                    );
                //////////
                user.types.forEach(function (type) {
                    $(".statPanelUser:last").append($(div)
                        .addClass("statPanelUserType")
                        .click(statPanelUserTypeClick)
                        .append($(div)
                            .addClass("statPanelUserTiTleWrapper")
                            .append($(div)
                            .addClass("statPanelUserTypeTitle")
                            .html(type.typeName)
                                )
                                .append($(div)
                                    .addClass("statPanelUserTypeCount")
                                    .html(type.count)
                                )
                        )
                        .append($(div)
                            .addClass("statPanelUserBlocksWrapper")
                            )
                        
                    );
                    ////
                    type.blocks.forEach(function (block) {
                        $(".statPanelUserBlocksWrapper:last")
                        .append($(div)
                        .addClass("statPanelUserTypeBlock")
                        .html(block.title + " / " +
                            block.NewsDate +
                        " / " + block.newsName)
                        );
                    });
                });
            });
            
        });

    });
}

function statPanelUserTypeClick()
{
    if ($(this).children(".statPanelUserBlocksWrapper").is(":visible"))
        $(this).children(".statPanelUserBlocksWrapper").fadeOut(500);
    else
        $(this).children(".statPanelUserBlocksWrapper").fadeIn(500);
}
function statChangeParams(event) {
    console.log(event);
    $(".statParamBtn").removeClass("btn-success").addClass("btn-default");
    $(event.target).addClass("btn-success").removeClass("btn-default");
    statGet();
}