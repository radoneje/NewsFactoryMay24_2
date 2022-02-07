<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainVideoPlayerElement.aspx.cs" Inherits="WebApplication2.Elements.mainVideoPlayerElement" %>


<form id="form1" runat="server" onsubmit="return false;">


    <div class="panelPlayerMenu stopPropagation">
        <label for="MediaBlockEditReadyDropDown" class="stopPropagation"><small>Готов</small></label>
        <input type="checkBox" id="MediaBlockEditReadyDropDown" onchange="mainVideoMediaItemStatusChange(<%=_mediaId %>)" class="stopPropagation" <%=_ready? "checked":"" %> />
        <label for="MediaBlockEditApproveDropDown" class="stopPropagation"><small>Принят</small> </label>
         <script>$("label[for='MediaBlockEditReadyDropDown'] small").html(langTable["CapReady"]);</script>
        <input type="checkBox" id="MediaBlockEditApproveDropDown" onchange="mainVideoMediaItemStatusChange(<%=_mediaId %>)" class="stopPropagation" <%=_approved? "checked":"" %> />
      <script>$("label[for='MediaBlockEditApproveDropDown'] small").html(langTable["CapApprove"]);</script>
     
         <div class="btn-group " role="group">
             <button type="button" id="MediaBlockSubTextBtn" class="btn btn-default stopPropagation" onclick="subTextEditorOpen( <%=_mediaId %>, <%=_blockId %>)" data-toggle="tooltip" data-placement="top" data-original-title="Subext">
                <span class="glyphicon glyphicon-text-width stopPropagation" aria-hidden="true"></span>
                 бегущая строка
            </button>
            <button type="button" id="MediaBlockSubTitleBtn" class="btn btn-default stopPropagation" onclick="subTitleEditorOpen( <%=_mediaId %>, <%=_blockId %>)" data-toggle="tooltip" data-placement="top" data-original-title="SubTitle">
                <span class="glyphicon glyphicon-text-background stopPropagation" aria-hidden="true"></span>
                субтитры
            </button>
            <button type="button" class="btn btn-default stopPropagation" onclick="DownloadFile(serverRoot + 'handlers/GetMediaSourceFile.ashx?MediaId=' + <%=_mediaId %>)" data-toggle="tooltip" data-placement="top" data-original-title="Скачать">
                <span class="glyphicon glyphicon-floppy-save stopPropagation" aria-hidden="true"></span>
            </button>
            <button type="button" class="btn btn-default stopPropagation" onclick="mediaDelete(<%=_mediaId %>,event)" data-toggle="tooltip" data-placement="top" data-original-title="Удалить">
                <span class="glyphicon glyphicon-trash stopPropagation" aria-hidden="true"></span>

            </button>
        </div>
        <div style="clear: both"></div>
    </div>
    <div class="panelPlayerArchiveMenu stopPropagation">
           <div class="btn-group " role="group">
            
            <button type="button" class="btn btn-default stopPropagation" onclick="DownloadFile(serverRoot + 'handlers/GetArchiveSourceFile.ashx?MediaId=' + <%=_mediaId %>)" data-toggle="tooltip" data-placement="top" data-original-title="Скачать">
                <span class="glyphicon glyphicon-floppy-save stopPropagation" aria-hidden="true"></span>
            </button>
            
        </div>
        <div style="clear: both"></div>
    </div>
    <script>
        $(".panelPlayerMenu").click(function (e) { e.stopPropagation(); });
    </script>

    <asp:panel runat="server" id="panelPlayer" class="stopPropagation">
                    <video   controls poster="<%=(string)Application["ServerRoot"]+ "handlers/Get"+(Request.Params["archive"]!="true"?"":"Archive")+"BlockImage.ashx?MediaId="+ _mediaId.ToString()+"&rnd="+ (new Random()).Next().ToString() %>" class="stopPropagation">
                    <source src="<%=(string)Application["ServerRoot"]+ "handlers/Get"+(Request.Params["archive"]!="true"?"":"Archive")+"BlockVideo.ashx?MediaId="+ _mediaId.ToString()+"&rnd="+ (new Random()).Next().ToString() %>" type="video/mp4" />
                </video>
        <div class="mainVideoPlSTWr subTitleVideoSize30 stopPropagation" >
            <div class="mainVideoPlS stopPropagation" >

        </div>
        </div>
        <div class="subTextEditorWr subTitleVideoSize stopPropagation">
             <textarea class="subTextEditorCrowl stopPropagation" placeholder="текст субтитров"></textarea>
                <script> $(".subTitleEditorCurrText").attr("placeholder", langTable["CapSubTitlesText"])</script>
            </div>
        <div class="subTitleEditorWr subTitleVideoSize stopPropagation">
           
            <div>
                    <select class="mainTitleLayerSelect stopPropagation"></select>
            </div>
            <div class="subTitleEditorLimitBox">
               <div>symbols: </div>
                <input type="number" value="0" readonly id="subTitleEditorCount"/>
                <div>/limit : </div>
                <input type="number"  id="subTitleEditorLimit"/>
            </div>
            <script>
               

            </script>
            <div class="subTitleEditorHead stopPropagation">
                <div class="subTitleEditorCurrTC stopPropagation">
                    00:00:00
                    </div>
                <textarea class="subTitleEditorCurrText stopPropagation" placeholder="текст субтитров"></textarea>
                <script> $(".subTitleEditorCurrText").attr("placeholder", langTable["CapSubTitlesText"])</script>
            </div>
             <div class="subTitleEditorMenu stopPropagation">
                   <div class="btn-group btn-group-sm" role="group">
                        <input  class="mainTitleEditorAddSec stopPropagation" type="number" value="0" max="1024" min="-1024"  /> 
                 
                 <button type="button" class="btn btn-default stopPropagation" onclick="mainTitleEditorAddSec();" data-toggle="tooltip" data-placement="top" data-original-title="Скачать">
                         <span class="glyphicon glyphicon-plus stopPropagation" aria-hidden="true"></span>
                </button>
                 
                       <button type="button" class="btn btn-default stopPropagation" onclick="DownloadFile(serverRoot + 'API/Block/Media/srt/<%=_mediaId %>/' +$('.mainTitleLayerSelect').val() )" data-toggle="tooltip" data-placement="top" data-original-title="Скачать">
                         <span class="glyphicon glyphicon-floppy-save stopPropagation" aria-hidden="true"></span>
                </button>
                      </div>
                 </div>
             <div class="subTitleEditorBody stopPropagation">
                 </div>

        </div>
         <div class="subTitleEditorBlockText stopPropagation"></div>
        <div class="subTitleEditorHelp stopPropagation">
            <div>
               <b> 1. прослушал (shift+enter) 2. вставил(клик) 3. добавил (enter).</b>

            </div>
            <table>
                <tr>
                    <td>
                         <div>Вставить из текста блока - Выделить фрагмент и правая кнопка мыши</div>
            <div>Добавить субтитр - Enter</div>
            <div>Старт-стоп видео - Shift+Пробел</div>
                    </td>
                    <td style="padding-left:20px">
                         <div>Текущий таймкод в субтитры - Shift+Enter</div>
            <div>Вперед-назад видео - Shift+стрелка влево или Shift+стрелка вправо</div>
             <div>Позиционирование - Клик на таймкоде субтитра</div>
                    </td>
                </tr>
            </table>
           
               
            
        </div>
        <script>
            initPlayerTimer();
           
            </script>
                </asp:panel>
    <asp:panel runat="server" id="panelImage" class="stopPropagation">
                     <img src="<%=(string)Application["ServerRoot"]+ "handlers/Get"+(Request.Params["archive"]!="true"?"":"Archive")+"BlockImage.ashx?MediaId="+ _mediaId.ToString() %>" class="stopPropagation"/>
                </asp:panel>
    <asp:panel runat="server" id="panelNoImage" class="stopPropagation">
                    <img src="<%=(string)Application["ServerRoot"]%>Images/noimage.jpg" />" class="stopPropagation" />
                </asp:panel>
    <asp:panel runat="server" id="panelDocument" class="stopPropagation">
                    <img src="<%=(string)Application["ServerRoot"]%>images/document.svg" class="stopPropagation"/>
                </asp:panel>
    <script>
        initMainTitleLayerSelect();
        initMainTitleLayerblockTextConextMenu();
       initMainTitleLayerStringSave();
        mainVideoSubTitlesLoad(<%=_mediaId %>);
        $(".mainMediaPanel").click(function (e) { e.stopPropagation(); })
        $("#panelPlayer").find('video').click(function (e) {
        });
    </script>
    <style>
        .panelPlayerMenu{
            <%=Request.Params["archive"]!="true"?"":"display:none;"%>
        }
        .panelPlayerArchiveMenu{
            <%=Request.Params["archive"]=="true"?"":"display:none;"%>
        }
    </style>
<script>
$(document).ready(function (){
$(".mainVideoPlSTWr").hide();
$("#panelPlayer").find('video').on(
       "playing",
       function (event) {
         console.log(333);
	$(".mainVideoPlSTWr").show();
       });
});
</script>
</form>

