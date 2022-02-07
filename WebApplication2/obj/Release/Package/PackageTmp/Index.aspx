<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebApplication2.Index" %>

<%@ Register Src="~/News/NewsPanel.ascx" TagName="NewsPanel" TagPrefix="uc1" %>
<%@ Register Src="~/Blocks/BlockPanel.ascx" TagPrefix="uc1" TagName="BlockPanel" %>
<%@ Register Src="~/RightPanel/RightContainer.ascx" TagPrefix="uc1" TagName="RightContainer" %>
<%@ Register Src="~/News/ArchiveNews.ascx" TagPrefix="uc1" TagName="ArchiveNews" %>
<%@ Register Src="~/News/rssNews.ascx" TagPrefix="uc1" TagName="mainPanelRss" %>

<%@ Register Src="~/Blocks/ArchiveBlocksConteiner.ascx" TagPrefix="uc1" TagName="ArchiveBlocksConteiner" %>
<%@ Register Src="~/News/LentaNews.ascx" TagPrefix="uc1" TagName="LentaNews" %>
<%@ Register Src="~/Blocks/LentaBlocksConteiner.ascx" TagPrefix="uc1" TagName="LentaBlocksConteiner" %>
<%@ Register Src="~/News/PartnersNews.ascx" TagPrefix="uc1" TagName="PartnersNews" %>
<%@ Register Src="~/Blocks/PartnersBlocksConteiner.ascx" TagPrefix="uc1" TagName="PartnersBlocksConteiner" %>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        var doOnClockBlockType = "<%=(string)Application["doOnClickBlockType"] %>";

    </script>
  

    <link href="<%=(string)Application["serverRoot"] %>Content/daterangepicker-bs3.css" rel="stylesheet" />
    <script src="<%=(string)Application["serverRoot"] %>Scripts/moment.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/daterangepicker.js"></script>
    <link href="<%=(string)Application["serverRoot"] %>Content/daterangepicker-bs2.css" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Content/daterangepicker-bs3.css" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Content/video-js.min.css" rel="stylesheet" />

     <script src="<%=(string)Application["serverRoot"] %>Scripts/NFW.js?date=28032021"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/News.js?date=23041028_1"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/DragAndDrop.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/RssScript.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/video.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/VideoPlayer.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/ArchiveScript.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/Lenta.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/FileUpload.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/nfsocketScript.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/Messager.js?date=28032021"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/NFfileUpload.js?date=12022021"></script>
      <script src="<%=(string)Application["serverRoot"] %>Scripts/printThis.js"></script>
          <script src="<%=(string)Application["serverRoot"] %>Scripts/partnersScript.js"></script>
  
    <script>
        videojs.options.flash.swf = "video-js.swf";
    </script>
    <script>
        function PageOnSubmit() {

            if ($("#ArchiveText").is(":focus"))
                ArchiveSearch();
            if ($("#ArchiveAutorDropDown").is(":focus"))
                ArchiveSearch();
            if ($("#ArchiveSearchDatarange1").is(":focus"))
                ArchiveSearch();

        };
       
    
    </script>
    <%=getRouteScript() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headMenu" runat="server">
    <div class="mainHeadMenu">
        <div class="mainHeadMenuItem mainHeadMenuItemActive" panel="mainPanelNews">
            текущие
        </div>
        <div class="mainHeadMenuItem" panel="mainPanelArchive">
            архив
        </div>
        <div class="mainHeadMenuItem" panel="mainPanelPartners">
            Partners
        </div>
        <div class="mainHeadMenuItem" panel="mainPanelLenta">
            лента
        </div>
         <div id="mainPanelRss" class="mainHeadMenuItem" panel="mainPanelRss">
            rss
        </div>
        <div class="mainHeadMenuItem" panel="mainPanelStat">
            статистика
        </div>
        <div class="mainHeadMenuItem" panel="mainPanelAdmin">
            администрирование
        </div>
       <script>$(".mainHeadMenuItem").each(function () { $(this).captionHTML($(this).attr("panel")); });</script>
            
    
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        // unlookBlock(true);
    </script>
    <div class="CenralTower mainPanelNews">
        <div class="CenralTowerLCol">
            <uc1:NewsPanel ID="NewsPanel1" runat="server" Visible="true" />
        </div>
        <div class="CenralTowerCCol">
            <uc1:BlockPanel runat="server" ID="BlockPanel" Visible="true" />
            <div  style="text-align:left; margin-top:-23px; margin-left:15px;">
                <input id="ApproveAllButton" type="button" class="btn btn-default btn-xs" value="approve all" onclick="approveAll()"/>
            </div>
        </div>

    </div>
    <!--- Partners -->
    <div class="CenralTower mainPanelPartners">
        <div class="CenralTowerLCol">
        
            <uc1:PartnersNews runat="server" ID="PartnersNews" Visible="true" />
        </div>
        <div class="CenralTowerCCol">
         
            <uc1:PartnersBlocksConteiner runat="server" ID="PartnersBlocksConteiner" />
        </div>

    </div>
    <!--- Архив -->
    <div class="CenralTower mainPanelArchive">
        <div class="CenralTowerLCol">
            <uc1:ArchiveNews runat="server" ID="ArchiveNews1" Visible="true" />
        </div>
        <div class="CenralTowerCCol">
            <uc1:ArchiveBlocksConteiner runat="server" ID="ArchiveBlocksConteiner1" Visible="true" />

        </div>

    </div>
    <!--rss-->
    <div class="CenralTower mainPanelRss">
        <div class="CenralTowerLCol">
            
            </div>
    </div>
    <!--- ЛЕНТА-->
    <div class="CenralTower mainPanelLenta">
        <div class="CenralTowerLCol">
            <uc1:LentaNews runat="server" ID="LentaNews" Visible="true" />

        </div>
        <div class="CenralTowerCCol">
            <uc1:LentaBlocksConteiner runat="server" ID="LentaBlocksConteiner" Visible="true" />

        </div>
    </div>
    <div class="CenralTower mainPanelStat">
        <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif"  style="width:50px"/>
    </div>
    <div class="CenralTower mainPanelAdmin">
        <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif"  style="width:50px"/>
    </div>

    <div class="CenralTowerRCol" style="">

        <div id="RFloatContainer" style="" class="FixedContainer">
            <uc1:RightContainer runat="server" ID="RightContainer" />
        </div>


    </div>


    <div id="DefaultPageFooter"></div>
    <div id='FileUploadContainer' class='FileUploadContainer'></div>
    <div id='FileUploader' class='FileUploadContainer'></div>
    <iframe id="DownloadIFrame" style='display: none;'></iframe>
    
     <div class="wailt" style="display:none"> <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif"  style="width:50px"/></div>
    <!-- Modal -->
    <div class="modal fade" id="AddForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document" style="width: 80% !important;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <div class="alert alert-success" role="alert">
                        <h4 class="modal-title"><span id="CapChooseBlocks" class="caption caption-html" captionid="CapChooseBlocks">Выбор блоков</span></h4>
                        <script>$("#CapChooseBlocks").html(langTable["CapChooseBlocks"]);</script>
                    </div>

                </div>
                <div class="modal-body">
                    <div>
                        <div style="width: 30%; float: left">
                            <select id="AddFormPrograms" style="width: 100%;width: 100%;margin-bottom: 10px;border-radius: 4px; border: 1px solid #ccc;" onchange="AddFormProgrammSelchange()">
                                <option id="AddFormCapChoosePr" selected value="-1" class="caption caption-html" captionid="CapChoosePr">выбери программу</option>
                            </select>
                            <script>$("#AddFormCapChoosePr").html(langTable["AddFormCapChoosePr"]);</script>
                            <div id="AddFormPlan" class="AddFormGroup NewsGroup" >
                                <span id="CapPlannedNews" class="caption caption-html" style="font-weight:bold" captionid="CapPlannedNews">сегодня</span>
                                <script>$("#CapPlannedNews").html(langTable["CapPlannedNews"]);</script>
                                <div id="AddFormPlanBox" class="AddFormGroupBox" style="font-weight: normal"></div>
                            </div>

                            <div id="AddFormCur" class="AddFormGroup NewsGroup" >
                                <span id="CapTodayNews" class="caption caption-html" style="font-weight:bold" captionid="CapTodayNews">сегодня</span>
                                <script>$("#CapTodayNews").html(langTable["CapTodayNews"]);</script>
                                <div id="AddFormCurrBox" class="AddFormGroupBox" style="font-weight: normal"></div>
                            </div>
                            <div id="AddFormPast"  class="AddFormGroup NewsGroup">
                                <span id="CapLastNews" class="caption caption-html" style="font-weight:bold" captionid="CapLastNews">прошедшие</span>
                                <script>$("#CapLastNews").html(langTable["CapLastNews"]);</script>
                                <div id="AddFormPastBox" class="AddFormGroupBox" style="font-weight: normal"></div>
                            </div>
                        </div>

                        <div id="AddFormBlocksBox" style="width: 65%; float: right"></div>
                        <div style="clear: both"></div>
                    </div>
                    <div>
                        <div class="modal-footer">
                            <input type="button" id="AddFormMoveBlockBtn1" class="btn btn-success btn-sm caption-value"  data-dismiss="modal" onclick="AddFormMoveBlock()" value="Move selected itemd" />
                          
                            <input type="button" id="AddFormCopyBlockBtn2"  class="btn btn-success btn-sm caption-value" captionid="AddFormCopyBlockBtn" data-dismiss="modal" onclick="AddFormCopyBlock()" value="Copy selected itemd" />
                            <script>$('#AddFormCopyBlockBtn').val(langTable['AddFormCopyBlockBtn'])</script>
                            <input type="button" id="AddFormCloseBlockBtn1" class="btn btn-success btn-sm  caption-value" captionid="CapClose" data-dismiss="modal" value="Закрыть" />
                            <script>$('#AddFormCloseBlockBtn').val(langTable['CapClose'])</script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="hiddenDiv" style="display: none"></div>
       </div>
    <!-- -->
    <div class="modal fade" id="playoutForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document" style="width: 80% !important;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <div class="alert alert-success" role="alert">
                        <h4 class="modal-title"><span id="CapPlayout" class="caption caption-html" captionid="PlayOuts"></span></h4>
                        <script>$("#CapPlayout").html(langTable["PlayOuts"]);</script>
                    </div>

                </div>
                <div class="modal-body">
                    <div>
                        <div style="width: 30%; float: left">
                            <select id="CapPlayoutServers" style="width: 100%;width: 100%;margin-bottom: 10px;border-radius: 4px; border: 1px solid #ccc;" onchange="AddFormProgrammSelchange()">
                                <option id="CapPlayoutServersChoosePr" selected value="-1" class="caption caption-html CapPlayoutServersChoose" >select Server</option>
                            </select>
                            <input type="button" id="CapPlayoutServersSendButton" class="btn btn-default" />
                            <script>
                                $("#CapPlayoutServersSendButton").val(langTable["toPlayOuts"]);
              
                            </script>  
                            <input type="button" id="CapTitleServersSendButton" class="btn btn-default" style="display:none" />
                            <script>
                                $("#CapTitleServersSendButton").val(langTable["toTitleOuts"]);
              
                            </script> 
                        </div>

                        <div id="PlayOutWr" style="width: 65%; float: right"></div>
                        <div style="clear: both"></div>
                    </div>
                    <div>
                        <div class="modal-footer">
                            <input type="button" id="AddFormMoveBlockBtn" style="display: none" class="btn btn-success btn-sm caption-value"  data-dismiss="modal" onclick="AddFormMoveBlock()" value="Move selected itemd" />
                          
                            <input type="button" id="AddFormCopyBlockBtn" style="display: none" class="btn btn-success btn-sm caption-value" captionid="AddFormCopyBlockBtn" data-dismiss="modal" onclick="AddFormCopyBlock()" value="Copy selected itemd" />
                            <script>$('#AddFormCopyBlockBtn').val(langTable['AddFormCopyBlockBtn'])</script>
                            <input type="button" id="AddFormCloseBlockBtn" class="btn btn-success btn-sm  caption-value" captionid="CapClose" data-dismiss="modal" value="Закрыть" />
                            <script>$('#AddFormCloseBlockBtn').val(langTable['CapClose'])</script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
 
       </div>
    <!-- -->
     <div class="NFfileUploadWrBox hidden" >
     <div class="NFfileUploadHeader">
         <img src="/images/show.svg"  class="showBtn"/>
         <img src="/images/hide.svg" class="hideBtn" />
     </div>
      <div class="NFfileUploadWr" >
      </div>
          </div>
    <div class="messager">
        <div class="messagerHead">
        <span id="rCapUsers" class="caption caption-html" captionId="CapUsers">сообщения</span> <span class="newMessagesCount" count="0"></span>
             <script>$('#rCapUsers').html(langTable['CapUsers'])</script>
            </div>
         
        <div class="messagerBody">
             <div class="MessagerContent" id="InMsgContent">

          </div>
            </div>
    </div>
   
    <script src="<%=(string)Application["serverRoot"] %>Scripts/subtitleEditor.js"></script>
</asp:Content>
