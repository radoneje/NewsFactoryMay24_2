


$(document).ready(function () {
    $('#NewsEditDate').datetimepicker({
        showSecond: true,
        showHour: false,
        showMinute: false,
        showSecond: false,
        showTimepicker: false

    });
    $('#NewsEditTime').timepicker({
        showSecond: true,
        timeFormat: 'HH:mm:ss',
        timeOnly: true,
    });
    $('#NewsEditDur').timepicker({
        showSecond: true,
        timeFormat: 'HH:mm:ss',
        timeOnly: true,
    });
    $('#Timecode').timepicker({
        showSecond: true,
        timeFormat: 'HH:mm:ss',
        timeOnly: true,
    });
    function ShowDisabledMessage(messageText, messageTitle) {
        var iDiv = CreateFullScreenDiv("EditorAlertDiv")
        iDiv.innerHTML = CreateBlockAlertElements(messageText, messageTitle);
        //document.getElementById("EditorAlertDiv").appendChild(iframe);

        window.scrollTo(0, 0);

    }
    function showBlockEditorAlert(text) {
        $("#BlockEditorAlert").css("display", "show").text(text).show();
        setTimeout(function () {
            $("#BlockEditorAlert").hide('blind', {}, 500)
        }, 5000);
    }
    function CreateBlockAlertElements(messageText, messageTitle) {
        var ret = '<div id="BlockEditorAlertMessage" class="alert alert-warning" role="alert">';
        if (checkDef(messageTitle)) {
            ret = ret + "<h4>" + messageTitle + "</h4>";
        }
        if (checkDef(messageText)) {
            ret = ret + '<p>' + messageText + '</p>'
        }
        ret = ret + '</div>';
        //if (checkDef(window.parent)) {
        ret = ret + "<button id='BlockEditorAlertCloseButton' type='submit' style='width: 100;' class='btn btn-success navbar-btn' onclick=' window.parent.CloseEditor();'>закрыть редактор</button>";
        $("#BlockEditorAlertCloseButton").center();
        //}
        $("#BlockEditorAlertMessage").center();
        return ret;

    }
})

