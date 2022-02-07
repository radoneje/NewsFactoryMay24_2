function CheckPassword()
{
  
    
    if ($("#fPassword").val().length < 1 )
    {
        LoginMessage.innerHTML = '<div class="alert alert-warning" role="alert">' + langTable['nullPass'] + '</div>';
        $("#fPassword").focus();
        return;
    }

    if ($("#fPassword").val().length > 15)
    {
        LoginMessage.innerHTML = '<div class="alert alert-warning" role="alert">' + langTable['PassToLong'] + '</div>';
        $("#fPassword").val("");
        $("#fPassword").focus();
        return;
    }
        

    LoginMessage.innerHTML = '<div class="alert alert-success" role="alert">'+langTable['PassIsChecking']+'</div>';
        $("#fPassword").focus();
  
        var jdata = {
            NFLoginID: $("#fLogin").val(),
            NFPassword: $("#fPassword").val()
        };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url:  serverRoot+"testservice.asmx/CheckLogin",
            data: JSON.stringify(jdata, null, 2),
            dataType: "json",
            async: true,
            success: AjaxSucceeded,
            error: AjaxFailed
        }).getAllResponseHeaders();
   
}
function AjaxSucceeded(data)
{
    var ret = JSON.parse(data.d);
    console.log(ret);
    try{
        if(ret.Status<1)
        {
            LoginMessage.innerHTML = '<div class="alert alert-warning" role="alert">'+ret.Message+'</div>';
            $("#fPassword").val("");
            $("#fPassword").focus();
            return;
        }
        LoginMessage.innerHTML = '<div class="alert alert-success" role="alert">' + ret.Message + '</div>';

        $("#fPassword").val("");
        $("#fPassword").focus();
        document.cookie = "NFWSession=" + ret.Cookie;
        console.log(document.cookie);
        console.log(ret);
        window.location.href = ret.Url;
   
        return;
    }
    catch(exepot)
    {
        LoginMessage.innerHTML = '<div class="alert alert-danger" role="alert">' + langTable['ErrorTryAgain'] + '</div>';
        $("#fPassword").val("");
        $("#fPassword").focus();
        return;
    }
}
function AjaxFailed() {
    LoginMessage.innerHTML = '<div class="alert alert-danger" role="alert">' + langTable['ErrorNoConnectionTryAgain'] + '</div>';
    $("#fPassword").val("");
    $("#fPassword").focus();
}

///////////CheckLogin()
function CheckLogin()
{
    if ($("#fPassword").val().length < 1) {
        $("#LoginMessagePanel").html('<div class="alert alert-warning" role="alert">' + langTable['nullPass'] +'</div>');
        $("#fPassword").focus();
        return false;
    }

    if ($("#fPassword").val().length > 15) {
        $("#LoginMessagePanel").html('<div class="alert alert-warning" role="alert">' + langTable['PassToLong'] + '</div>');
        
        $("#fPassword").val("");
        $("#fPassword").focus();
        return false;
    }
    $("#MainForm").submit();
}


///////////version 2
$(document).ready(function () {
    reloadLogins();
    window.history.pushState(null, 'login', serverRoot + 'login');
});
function reloadLogins() {
   
    $.ajax({
        type: "POST",
        contentType: "application/json;",
        url:  serverRoot+"testservice.asmx/loginUsersList",
        data: JSON.stringify({ id: 0 }),
        dataType: "json",
        success: function (data) {
            $(".fLogin option").remove();
            var dt = JSON.parse(data.d);
            var r = window.localStorage.getItem("lastLoginUser");
            var selected = false;
            dt.forEach(function (item) {
                console.warn($(".fLogin").length);
                $(".fLogin").append($("<option></option>").attr("value", item.userId).attr("id", "FloginOption" + item.userId).prop("selected", r == item.userId).text(item.userName));
                if(r==item.userId)
                {
                    $("#tmpOptionLogin").remove();
                    $("#hiddenLogin").val(r);
                    selected = true;
                }
            });
            if(!selected)
            {
                $("#hiddenLogin").val($(".fLogin option:first").attr("value"));
                $(".fLogin option:first").prop("selected", true);

            }
               
        },
        error: function(e){
            console.warn("login ajax error");
            console.warn(e);
            }
        });
}
function fLogin(ctrl) {
    window.localStorage.setItem('lastLoginUser', $(ctrl).val());
    $("#hiddenLogin").val($(ctrl).val());
    $("#tmpOptionLogin").remove();
    $('#fPassword').focus();
}
function selectLoginChange(ctrl) {
    $("#hiddenLogin").val($(ctrl).val());
    var r = window.localStorage.setItem("lastLoginUser", $(ctrl).val());
    $('#fPassword').focus();
    $('#fPassword').val("");
}