<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlockEditor.aspx.cs" Inherits="WebApplication2.Blocks.BlockEditor" %>

<%@ Register Src="~/Blocks/BlockEditorPlayer.ascx" TagPrefix="uc1" TagName="BlockEditorPlayer" %>
<%@ Register Src="~/Blocks/ExtLinkButton.ascx" TagPrefix="uc1" TagName="ExtLinkButton" %>





<!DOCTYPE html>


<head runat="server">

    <script>
        const serverRoot='<%=(string)Application["serverRoot"] %>';
    </script>
   
    <link href="<%=(string)Application["serverRoot"] %>Content/jquery-ui.min.css" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>bootstrap/bootstrap-3.3.2-dist/css/bootstrap.min.css" rel="stylesheet" />

    <link href="<%=(string)Application["serverRoot"] %>Styles/NFW.css?date=17032018_5" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Styles/1024style.css" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Styles/1260style.css" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Styles/960style.css" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Styles/560style.css" rel="stylesheet" />

    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery-2.1.3.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery-ui.min.js"></script>   
    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery-ui-timepicker-addon.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/Utils.js?date=11111"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/moment.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery.cookie-1.4.1.min.js"></script>
   
    <script src="<%=(string)Application["serverRoot"] %>bootstrap/bootstrap-3.3.2-dist/js/bootstrap.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Resources/lang0.js"></script>
    <link href="<%=(string)Application["serverRoot"] %>Styles/BlockEditor.css?date=25042018_6" rel="stylesheet" />
    <link href="<%=(string)Application["serverRoot"] %>Styles/custom.css?date=25042018_6" rel="stylesheet" />
    <script src="<%=(string)Application["serverRoot"] %>Scripts/DragAndDrop.js"></script>

    <script src="<%=(string)Application["serverRoot"] %>Scripts/bootstrap3-typeahead.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/video.js"></script>
    <link href="<%=(string)Application["serverRoot"] %>Content/video-js.min.css" rel="stylesheet" />
    <script>
        //   videojs.options.flash.swf = "video-js.swf";
        var userId=<%=UserId%>;
        var langId = localStorage.getItem("languageId");
        BEChangeLang(langId);
        console.log("BEChangeLang");
        function BEChangeLang(Id) {

            langId = Id;
            //  localStorage.setItem("languageId", Id);
            $('#LangSelect>option:eq(' + Id + ')').prop('selected', true);

            $.get(serverRoot+"Resources/lang" + langId + ".js", function (a) {
                eval(a);
                $(".caption").each(function (index, elem) {
                    if ($(elem).hasClass("caption-html")) {
                        $(elem).html(langTable[$(elem).attr('captionId')]);
                    }
                    if ($(elem).hasClass("caption-value")) {
                        $(elem).val(langTable[$(elem).attr('captionId')]);
                    }
                    if ($(elem).hasClass("caption-placeholder")) {
                        $(elem).attr('placeholder', langTable[$(elem).attr('captionId')]);
                    }
                });
            });
        }
        </script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/BlockEdit.js?date=05062020"></script>



    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    
</head>
<body onload="">
    <div onmousemove="clearLogOutTimeout();">
        <form id="form1" runat="server" onmousemove="clearLogOutTimeout()">
            <div class="BEWrapper" blockid="<%=Request.Params["blockid"] %>">
                <div class="BEbox" id="BEbox">
            <div id="BlockEditorAlert" class="alert alert-success" role="alert" style="display: none; position: fixed; top: 0;"></div>

            <asp:HiddenField ID="BlockEditIdHidden" runat="server" ClientIDMode="Static" />
            <div class="BEmenuWR">
                <div class="mainHeadMenu" style="display: block">
                    <div id="mainHeadMenuItemEditor" class="mainHeadMenuItem mainHeadMenuItemActive" panel="panelBlockEditor">
                        редактор
                    </div>
                    <div id="mainHeadMenuItemSocial" class="mainHeadMenuItem" panel="panelBlockSocial">
                        социальные сети
                    </div>
                    <script>
                        $("#mainHeadMenuItemEditor").captionHTML("blockEditor");
                        $("#mainHeadMenuItemSocial").captionHTML("blockSocial");
                    </script>

                </div>

            </div>
            <div class="BEwrapper">
                <!-- начало центра-->
                <div class="">
                    <div class="BETower2222  BEpanel panelBlockEditor" id="BETower2222" style="">
                        <div class="BETowerL">
                            <!-- начало левой-->
                            <div id="BESecondContainer" class="form-group-row" style="width: 100%">
                                <div class="panel panel-default">
                                    <div class="panel-heading">


                                        <div class="control-group" style="width: 100%">
                                            <asp:DropDownList ClientIDMode="Static" ID="BlockEditTypeDropDown" runat="server" Style="" class="form-control" onchange="UpdatePeoplesListStatus();isSaved=false;"></asp:DropDownList>
                                            <div class="BlockEditNameTextBoxContainer">
                                            <asp:TextBox ClientIDMode="Static" onkeyup="clearLogOutTimeout();isSaved=false;$('.BlockEditNameTextBoxCounter').html($(this).val().length) " ID="BlockEditNameTextBox" placeholder="название блока " runat="server" Style="" class="form-control caption caption-placeholder " captionId="CapBlockName"></asp:TextBox>
                                            <div class="BlockEditNameTextBoxCounter">0</div>
                                            </div>
                                                <script>$('.BlockEditNameTextBoxCounter').html($("#BlockEditNameTextBox").val().length);
                                                    $('#BlockEditNameTextBox').attr('placeholder', langTable['CapBlockName']);</script>
                                        </div>
                                        <br />
                                        <div id="BEComboBoxContainer" class="control-group">
                                            <div class="controls form-inline " id="BlockEditAutorDropDownBox">
                                                <div >
                                                    <label for="BlockEditAutorDropDown" class="label label-default BElabel caption caption-html" captionid="CapAutor"></label>
                                                    <script>$("label[for='BlockEditAutorDropDown']").html(langTable["CapAutor"]);</script>
                                                    <asp:DropDownList ClientIDMode="Static" ID="BlockEditAutorDropDown" runat="server" class="form-control BlockEditDropDown"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="controls form-inline " id="BlockEditOperatorDropDownBox">
                                                <div >
                                                    <label for="BlockEditOperatorDropDown" class="label label-default BElabel caption caption-html" captionid="CapCameramen"></label>
                                                    <script>$("label[for='BlockEditOperatorDropDown']").html(langTable["CapCameramen"]);</script>
                                                    <asp:DropDownList ClientIDMode="Static" ID="BlockEditOperatorDropDown" runat="server" class="form-control BlockEditDropDown"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="controls form-inline "id="BlockEditJockeyDropDownBox">
                                                
                                                    <label for="BlockEditJockeyDropDown" class="label label-default  BElabel caption caption-html" captionid="CapTalent">Ведущий</label>
                                                    <script>$("label[for='BlockEditJockeyDropDown']").html(langTable["CapTalent"]);</script>
                                                    <asp:DropDownList ClientIDMode="Static" ID="BlockEditJockeyDropDown" runat="server" class="form-control BlockEditDropDown"></asp:DropDownList>
                                              
                                            </div>
                                            <div class="controls form-inline " id="BlockEditCutterDropDownBox">
                                                <div >
                                                    <label for="BlockEditCutterDropDown" class="label label-default  BElabel caption caption-html" captionid="CapCutter">Монтажер</label>
                                                    <script>$("label[for='BlockEditCutterDropDown']").html(langTable["CapCutter"]);</script>
                                                    <asp:DropDownList ClientIDMode="Static" ID="BlockEditCutterDropDown" runat="server" class="form-control BlockEditDropDown"></asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                        <br />
                                        <div class="control-group" id="ExtLinkButtonGroup">
                                            <div class="controls form-inline ">
                                                <uc1:ExtLinkButton runat="server" ID="ExtLinkButton" />
                                                
                                            </div>
                                        </div>


                                    </div>

                                    <div class="panel-body " id="BEpanelBody">
                                        <div class="BEpanelWr">
                                            <div id="BEEditContainer" class="control-group">

                                                <div class="controls form-inline BETextConrols">
                                                    <div class="btn-group btn-group-xs" role="group" style="float:left">
                                                       
                                                        <button type="button" id="addSot" class="btn btn-default" >
                                                            SOT 
                                                        </button>
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                            <span class="caret"></span>
                                                            <span class="sr-only">Toggle Dropdown</span>
                                                        </button>
                                                        <ul class="addSotSelect dropdown-menu">
                                                           
                                                        </ul>
                                                      

                                                    </div>
                                                    <div class="btn-group btn-group-xs" role="group" style="float:left; margin-left:12px;">
                                                        <button type="button" id="addGeo" class="btn btn-default" >
                                                            GEO 
                                                        </button>
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                            <span class="caret"></span>
                                                            <span class="sr-only">Toggle Dropdown</span>
                                                        </button>
                                                        <ul class="addGeoSelect dropdown-menu">
                                                           
                                                        </ul>
                                                    </div>
                                                    <div class="btn-group btn-group-xs" role="group" style="float:left; margin-left:12px;">
                                                        <button type="button" id="addSrc" class="btn btn-default" >
                                                            SRC 
                                                        </button>
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                            <span class="caret"></span>
                                                            <span class="sr-only">Toggle Dropdown</span>
                                                        </button>
                                                        <ul class="addSrcSelect dropdown-menu">
                                                           
                                                        </ul>
                                                    </div>


                                                    <div style="float:left;margin-left: 20px;">
                                                        
                                                        
                                                    <label for="BlockEditRedyDropDown" style="cursor: pointer" class=" caption caption-html" captionid="CapReady"></label>
                                                    <script>$("label[for='BlockEditRedyDropDown']").html(langTable["CapReady"]);</script>
                                                    <asp:CheckBox ClientIDMode="Static" ID="BlockEditRedyDropDown" runat="server" />
                                                    <label for="BlockEditApproveDropDown" style="cursor: pointer; padding-left: 10px;" class="caption caption-html" captionid="CapApprove"></label>
                                                    <script>$("label[for='BlockEditApproveDropDown']").html(langTable["CapApprove"]);</script>
                                                    <asp:CheckBox ClientIDMode="Static" ID="BlockEditApproveDropDown" runat="server" />
                                                    <div class="btn-group btn-group-xs" role="group" style="margin-left:1em" >
                                                        <button type="button" id="clearSelected Btn" class="btn btn-default" onclick="clearSelection()" >
                                                            Clear all selection 
                                                        </button>
                                                    </div>
                                               
                                                    </div>

                                                      <!--input type="text" id="BESynchTemplate" style="height: 1.8em; font-size: 12px; padding-left: 1px; padding-top: 2px; width: 250px;" class="form-control caption caption-placeholder " captionid="CapFioRequest" placeholder="наберите ФИО для вставки синхрона" /-->
                                                     <div class="closeBtn showHistory BEshowFullscreen" style=" float:right" onclick="BEshowFullScreen(event)" aria-hidden="true" data-toggle='tooltip' data-placement='bottom' data-original-title=''><span class="glyphicon glyphicon-fullscreen"></span></div>
                                                     <div class="closeBtn showHistory BEshowChernovik" style=" float:right" onclick="" aria-hidden="true" data-toggle='tooltip' data-placement='bottom' data-original-title=''><span class="glyphicon glyphicon-barcode"></span></div>
 
                                                    <div class="closeBtn showHistory histroy" style=" float:right" onclick="BEshowHistory(event)" aria-hidden="true" data-toggle='tooltip' data-placement='bottom' data-original-title=''><span class="glyphicon glyphicon-menu-hamburger"></span></div>
                                                   
                                                    <div style="float: right; display:block; font-size:small">
                                                        <label id="BESymbolsCount" class="label label-default"></label>
                                                        <label>font-size:</label><select id="BEEditorFSSelector" style="height: 1.8em; font-size: 12px; padding-left: 1px; padding-top: 0px; padding-bottom: 0px;" class="form-control" onchange="BEFontSizeChange(this.value)">
                                                            <option value="small">small</option>
                                                            <option value="medium">medium</option>
                                                            <option value="large">large</option>
                                                            <option value="x-large" selected>x-large</option>
                                                            <option value="xx-large">xx-large</option>
                                                        </select>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                                <script>$(".histroy").attr('data-original-title', langTable["History"]);</script>
                                                <!--
                                                 <label for="BlockEditRealTextBox" id="labelBlockEditRealTextBox" clicked="false" class="label label-default BElabel" style="cursor: pointer" onclick="ChronoCalculatedStart(this)" data-toggle='tooltip' data-placement='top' data-original-title=''></label>
                                                <script>$("label[for='BlockEditRealTextBox']").attr('data-original-title', langTable["CapClickToCalculate"]);</script>
                                                <script>$("label[for='BlockEditRealTextBox']").html(langTable["CapReal"]);</script>
                                                <asp:TextBox class="form-control" ClientIDMode="Static" ID="BlockEditTextTextBox" placeholder="Текст Блока" runat="server" Rows="10"  TextMode="MultiLine"  style="width:100%;   height: 203px;" onchange="CalculateCalcTime(this.value)"></asp:TextBox> -->

                                                <div class="BEEditorContainer panelBlockEditor">
                                                    <div class="BEEditor blocksExpanded" id="BEEditor" style="visibility: hidden;" contenteditable="true"  onpaste='BEOnPasteText($("#BEEditor"), event);clearLogOutTimeout();' oninput="console.log('input');BEOnInputText();clearLogOutTimeout();isSaved=false;" onmouseup="BEOnMouseUP(this);" onmouseout="BEOnMouseUP(this);">
                                                        <asp:Literal ID="BlockEditTextTextBoxLiteral" runat="server"></asp:Literal>
                                                    </div>

                                                </div>
                                                <asp:TextBox class="form-control caption caption-placeholder " captionId="CapComment" ClientIDMode="Static" ID="BlockEditDescriptionTextBox" placeholder="Комментарий" runat="server" Rows="2" TextMode="MultiLine" Style="width: 100%; resize:none"  onkeypress="clearLogOutTimeout();isSaved=false;" MaxLength="254"></asp:TextBox>
                                                <script>$('#BlockEditDescriptionTextBox').attr('placeholder', langTable['CapComment']).attr("MaxLength", "254")</script>
                                            </div>

                                        </div>
                                        <br />
                                        <div id="BEChronoContainer" class="control-group">

                                            <div class="controls form-inline ">
                                                <div >
                                                    <label for="BlockEditCalcTextBox" class="label label-default BElabel"></label>
                                                    <script>$("label[for='BlockEditCalcTextBox']").captionHTML("CapCalc");</script>
                                                    <asp:TextBox class="form-control BlockEditDropDown" ClientIDMode="Static" ID="BlockEditCalcTextBox" runat="server" Width="90" ReadOnly="True">00:00:00</asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="controls form-inline ">
                                                <div >
                                                    <label for="BlockEditPlannedTextBox" class="label label-default BElabel"></label>
                                                    <script>$("label[for='BlockEditPlannedTextBox']").captionHTML("CapPlanned");</script>
                                                    <asp:TextBox class="form-control BlockEditDropDown" ClientIDMode="Static" ID="BlockEditPlannedTextBox" runat="server" Width="90" onchange="isSaved=false;">00:00:00</asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="controls form-inline ">
                                                <div >
                                                    <label for="BlockEditRealTextBox" id="labelBlockEditRealTextBox" clicked="false" class="label label-default BElabel" style="cursor: pointer" onclick="ChronoCalculatedStart(this)" data-toggle='tooltip' data-placement='top' data-original-title=''></label>
                                                    <script>$("label[for='BlockEditRealTextBox']").attr('data-original-title', langTable["CapClickToCalculate"]);</script>
                                                    <script>$("label[for='BlockEditRealTextBox']").captionHTML("CapReal");</script>
                                                    <asp:TextBox class="form-control BlockEditDropDown" ID="BlockEditRealTextBox" runat="server" Width="90" ClientIDMode="Static" onchange="isSaved=false;">00:00:00</asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="BESaveBtnGroup controls form-inline " style="">
                                                <div style=" width: 100%;">
                                                    <div id="BESaveButtonContainer" class="btn-group text-center " style="width: 100%; text-align: center;">
                                                        <input id="BESaveBtn" type='button' style="text-align: center; width: 50%;" class='btn btn-success navbar-btn caption caption-value' captionid="CapSave" onclick='SaveBlock(); BEEditor.focus();' />
                                                        <input id="BECloseBtn" type='button' style="text-align: center; width: 50%; border-top-right-radius: 4px; border-bottom-right-radius: 4px;" class='btn btn-success navbar-btn centered caption caption-value' captionid="CapClose" onclick='ConfirmExit();' />
                                                        <script>
                                                            $('#BESaveBtn').val(langTable['CapSave']);
                                                            $('#BECloseBtn').val(langTable['CapClose']);
                                                        </script>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="clear: both"></div>
                                        </div>

                                        <script>
                                            $('[data-toggle="tooltip"]').tooltip();
                                        </script>

                                    </div>
                                </div>


                            </div>
                            <!-- left end -->
                        </div>
                    </div>
                    <div class="panelBlockSocial BEpanel panel panel-default" style="display: none">
                        <img class="loadingImg" src="<%=Application["serverRoot"] %>Images/loading.gif" />
                    </div>


                    <div class="BETowerLR">
                        <!-- начало right-->
                        <div class="panel panel-default" style="width: 100%">
                            <div class="panel-body ">
                                <div id="BEMultimediaContainer" style="">
                                    <div id="BEPlayerContainer" class="BEPlayerControl">

                                        <uc1:BlockEditorPlayer runat="server" ID="BlockEditorPlayer" />
                                    </div>
                                    <div id="BEImageContainer" class="BEPlayerControl"  style="width: 100%; height: 100%">
                                    </div>
                                    <div id="BEDocumentContainer" class="BEPlayerControl" style=""></div>

                                </div>

                                <div class="BEMediaFixedHeightContainer" style="">
                                    <div class="BEMediaContent" id="BEMediaContent" style=""></div>
                                    <!--<div id="BEAddMedia" class="BEAddMedia caption-html" captionid="CapAddFile" onclick="BEAddMediaFiles()">
                                        Загрузить
                                    </div> -->
                                    <div class="btn-group" role="group" style="margin-top:10px">
                                        <button id="BEAddMedia" type="button" data-toggle="tooltip" data-placement="top" class="btn btn-default btn  AddNewsButton" onclick="BEAddMediaFiles()">
                                            <span class="glyphicon glyphicon-floppy-open" "></span>
                                        </button>
                                        <button id="BESortMedia" type="button" data-toggle="tooltip" data-placement="top" class="btn btn-default AddNewsButton" onclick="BESortMediaFiles(event)">
                                            <span class="glyphicon glyphicon glyphicon-sort-by-alphabet" aria-hidden="true"></span>
                                        </button>
                                    </div>
                                    <!---
                                        -->
                                    <script>
                                     
                                        $('#BEAddMedia').attr('title', langTable['CapAddFile']);
                                        $('#BESortMedia').attr('title', langTable['CapSortFile']);
                                    </script>
                                </div>
                            </div>
                        </div>
                        <!--  right end -->
                    </div>
                    <div style="clear: both"></div>
                </div>


            </div>

            <!-- конец центра-->



            <!-- Modal -->
            <div class="modal fade" id="blockEditModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <div class="alert alert-danger" role="alert">
                                <h4 class="modal-title"><span id="CapErrorBE" class="caption caption-html" captionid="CapError">Ошибка</span></h4>
                                <script>$("#CapErrorBE").html(langTable["CapError"]);</script>
                            </div>

                        </div>
                        <div class="modal-body" id="blockEditModalBody">
                        </div>
                        <div class="modal-footer">
                            <input type="button" id="ErrorCloseBlockBtn" class="btn btn-success btn-sm  caption-value" captionid="CapClose" data-dismiss="modal" value="Закрыть" />
                            <script>$('#ErrorCloseBlockBtn').val(langTable['CapClose'])</script>
                        </div>
                    </div>
                </div>
            </div>
                </div>
                </div>
        </form>
        
        <div id="BEhiddenDiv"></div>
    </div>
</body>

