<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="title.aspx.cs" Inherits="WebApplication2.News.title" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>


    <title>May24 NewsRoom</title>
     <script src="/Scripts/jquery-2.1.3.min.js"></script>
</head>
<body>
    <div id="scriptDiv" runat="server"></div>
    <script>
        getCss(getTitle);
        function getCss(callback) {
            $.ajax({ type: "POST",  dataType: "json", contentType: "application/json;",  url: "/" + "testservice.asmx/cssGet", data: JSON.stringify({ css: css }), async: true, success: function( ret ){
                console.log(JSON.parse(ret.d));
                $("head").append("<style>" + JSON.parse(ret.d).css + "</style>");
                callback();
            }
            });
             
        }
        function getTitle() {
            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json;",
                url: "/" + "testservice.asmx/titleGet",
                data: JSON.stringify({ newsId: newsId }),
                async: true,
                success: function (ret) {
                    var json = JSON.parse(ret.d);
                    if (json.status > 0) {
                        $(".obj").addClass("remove");
                        for (var k in json.items){
                            if (json.items.hasOwnProperty(k)) {
                    
                                if ($("." + k).length == 0) {
                                    $("body").append("<div class='obj " + k + "'></div>");
                                }
                                var parsed = JSON.parse( json.items[k]);
                            
                                $("." + k)
                                    .html($("<div class='title'></div>").html(parsed.title))
                                    .removeClass("remove");
                                if(parsed.subtitle.length>0)
                                    $("." + k).append( $("<div class='subtitle'></div>").text(parsed.subtitle));

                            }
                        }
                        $(".remove").remove();

                    }
                    setTimeout(function () { getTitle(); }, 500)
                },

                error: function (ret) {
                    console.warn(ret);
                    setTimeout(function () { getTitle(); }, 500)
                }
            });
        }

    </script>
</body>
</html>
