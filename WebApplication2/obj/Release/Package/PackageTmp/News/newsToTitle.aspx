<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="newsToTitle.aspx.cs" Inherits="WebApplication2.News.newsToTitle" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .blockItem {
            margin: 10px;
            border-bottom: 1px solid gray;
        }

        .blockTitle {
            padding: 10px;
            font-weight: bold;
        }

        .blockText {
            padding-left: 20px;
            margin-bottom: 5px;
            display: inline-block;
        }

        .pageLink {
            font-size: 1.2em;
            margin: 1em;
            padding: .2em;
            border: 1px solid;
            background: #ffff0024;
        }

        #TitlePageInputCopyBtn {
            cursor: pointer;
        }

        .selectPanel {
            margin: 20px;
            max-width: 300px;
        }
        .blockItemsBox .btn-danger{
            display:none;
        }
        .blockItemsBox.active .btn-danger{
            display:inline-block;
        }
        .blockItemsBox.active .btn-danger{
            display:inline-block;
        }
        .blockItemsBox.active .btn-success{
            display:none;
        }
    </style>
    <div id="scriptDiv" runat="server">
    </div>
    <div class="panel panel-success selectPanel">
        <div class="panel-heading">
            <select id="playListSelect" class="plSelect form-control">
                <option value="0">Choose template</option>
            </select>
        </div>
        <div class="panel-body">

            <div class="input-group">
                <input type="text" id="TitlePageInput" readonly="readonly" class="form-control" placeholder="Link to title page" aria-describedby="basic-addon2">
                <span class="input-group-addon" id="TitlePageInputCopyBtn">copy</span>
            </div>
        </div>
    </div>

    <div class="blocksBox">
    </div>

    <script>
        $(document).ready(function () {
            $("#TitlePageInputCopyBtn").click(function (e) {
                $("#TitlePageInput").select();
                document.execCommand("copy");
            })
            $(".plSelect").change(function (e) {
                $("option[value='0']").remove();
                $("#TitlePageInput").val(serverName + "/news/title.aspx?newsId=" + newsId + "&templateId=" + $(".plSelect").val())
            });
            $.ajax({
                type: "POST",
                contentType: "application/json;",
                url: serverRoot + "testservice.asmx/titleOutGetList",
                data: JSON.stringify({ id: 0 }),
                dataType: "json",
                async: true,
                success: function (data) {

                    dt = JSON.parse(data.d);
                    dt.forEach(function (el) {
                        $(".plSelect").append($("<option value='" + el.id + "'>" + el.title + "</option>"));

                    });
                },
                error: function (data) {
                    ajaxErr("/testservice.asmx/titleOutGetList error", data);
                }

            });

            $.ajax({
                type: "POST",
                contentType: "application/json;",
                url: serverRoot + "testservice.asmx/blockList",
                data: JSON.stringify({ newsId: newsId }),
                dataType: "json",
                async: true,
                success: function (data) {

                    dt = JSON.parse(data.d);
                    j = 0;
                    dt.forEach(function (el) {

                        $(".blocksBox").append($("<div class='blockItem' targetId='" + el.id + "'></div>"));
                        var $ctrl = $(".blockItem[targetId='" + el.id + "']");
                        $ctrl.append($("<div class='blockTitle'></div>").text(el.title));


                        let reg = /\(\(TITLE\s([A-Z]+):(.+)/mg
                        let matches = Array.from(el.text.matchAll(reg));

                        if (matches.length > 0) {
                            var arr = new Array();

                            matches.forEach(function (mt) {
                                $ctrl.append($("<div class='blockItemsBox' id='" + newsId + "_" + j + "'></div>"));
                               j++;
                                arr.push({ class: mt[1], title: mt[2].replace("))", ""), subtitle: "", id: newsId + "_" + j });
                                $ctrl.find(".blockItemsBox").last().append("<input type='button' class='btn  btn-success' value='" + langTable["show"] + "' id='show_" + j + "' number='" + j + "'/>");
                                $("#show_" + j).click(function (e) {
                                    var num =parseInt( $(e.currentTarget).attr("number"))-1;
                                    $.ajax({
                                        type: "POST",  contentType: "application/json; charset=utf-8", dataType:"json", url: serverRoot + "testservice.asmx/titleShow", data: JSON.stringify({ newsId: newsId, cssClass: mt[1], title: mt[2].replace("))", ""), subtitle: "", css: "", id: newsId + "_" + num }), async: true,
                                        success: function (res) {
                                            statusUpdete(res);

                                        }

                                    });
                                });
                                $ctrl.find(".blockItemsBox").last().append("<input type='button' class='btn  btn-danger' value='" + langTable["hide"] + "' id='hide_" + j + "' number='" + j + "'/>");
                                $("#hide_" + j).click(function (e) {
                                     var num =parseInt( $(e.currentTarget).attr("number"))-1;
                                    $.ajax({
                                        type: "POST",  contentType: "application/json; charset=utf-8", dataType:"json", url: serverRoot + "testservice.asmx/titleHide", data: JSON.stringify({ newsId: newsId, cssClass: mt[1], id: newsId + "_" + num }), async: true,
                                        success: function (res) {
                                            statusUpdete(res);

                                        }
                                    });

                                });
                                $ctrl.find(".blockItemsBox").last().append($("<div class='blockText'></div>").html(mt[1] + ": " + mt[2].replace("))", "")));
                            });
                             
                        }

                        reg = /TITLE:(.*)/mg
                        matches = Array.from(el.text.matchAll(reg));
                        reg1 = /NAME:(.*)/mg
                        let matches1 = Array.from(el.text.matchAll(reg1));
                        var i = 0;
                        if (matches.length > 0) {
                            var arr = new Array();
                            matches.forEach(function (mt) {
                                var lt = matches1[i];
                                arr.push({ class: "SOT", title: lt[1], subtitle: mt[1] });
                                $ctrl.append($("<div class='blockItemsBox' id='" + newsId + "_" + j + "'></div>"));
                                i++;
                                j++;
                                $ctrl.find(".blockItemsBox").last().append("<input type='button' class='btn  btn-success' value='" + langTable["show"] + "' id='show_" + j + "' number='" + j + "'/>");
                                $("#show_" + j).click(function (e) {
                                      var num =parseInt( $(e.currentTarget).attr("number"))-1;
                                    $.ajax({
                                        type: "POST", contentType: "application/json; charset=utf-8", dataType: "json", url: serverRoot + "testservice.asmx/titleShow", data: JSON.stringify({ newsId: newsId, cssClass: "SOT", title: lt[1], subtitle: mt[1], css: "", id: newsId + "_" + num }), async: true,
                                        success: function (res) {
                                            statusUpdete(res);
                                        }
                                    });

                                });
                                $ctrl.find(".blockItemsBox").last().append("<input type='button' class='btn  btn-danger' value='" + langTable["hide"] + "' id='hide_" + j + "' number='" + j + "'/>");
                                $("#hide_" + j).click(function (e) {
                                      var num =parseInt( $(e.currentTarget).attr("number"))-1;
                                    $.ajax(
                                        {
                                            type: "POST", contentType: "application/json; charset=utf-8", dataType: "json", url: serverRoot + "testservice.asmx/titleHide", data: JSON.stringify({ newsId: newsId, cssClass: "SOT", id: newsId + "_" + num }), async: true,
                                            success: function (res) {
                                                statusUpdete(res);
                                            }
                                        });

                                });
                                $ctrl.find(".blockItemsBox").last().append($("<div class='blockText'></div>").html("SOT: " + lt[1] + ": " + mt[1]));
                            });
                             
                        }



                        //console.log(arr);

                    });
                },
                error: function (data) {
                    ajaxErr("/testservice.asmx/titleOutGetList error", data);
                }

            });


        });
        function statusUpdete(res) {
           
            var json = JSON.parse(res.d)[newsId];
            console.log(json);
            $(".blockItemsBox").removeClass("active");
            
            $.each(json, function (index, value) {
                 console.log("#" + JSON.parse(value).id);
                 $("#" + JSON.parse(value).id).addClass("active");
          }); 

        }
    </script>


</asp:Content>

