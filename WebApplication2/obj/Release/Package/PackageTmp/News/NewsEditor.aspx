<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsEditor.aspx.cs" Inherits="WebApplication2.News.NewsEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NewsEditor</title>
    <script>
        const serverRoot = '<%=(string)Application["serverRoot"] %>';
    </script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery-2.1.3.min.js"></script>
    <link href="<%=(string)Application["serverRoot"] %>Content/jquery-ui.min.css" rel="stylesheet" />
    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery-ui.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Resources/lang0.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/Utils.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/jquery-ui-timepicker-addon.min.js"></script>
    <script src="<%=(string)Application["serverRoot"] %>Scripts/NewsEdit.js"></script>
    <link href="<%=(string)Application["serverRoot"] %>bootstrap/bootstrap-3.3.2-dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="<%=(string)Application["serverRoot"] %>bootstrap/bootstrap-3.3.2-dist/js/bootstrap.min.js"></script>
    <link href="<%=(string)Application["serverRoot"] %>Styles/newsEditorStyle.css" rel="stylesheet" />

    <script>
        var langId = localStorage.getItem("languageId");
        ChangeLang(langId);
    </script>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="NewsEditorNewsId" runat="server" />
        <asp:HiddenField ID="NewsEditorGroupId" runat="server" />
        <div class="panel-heading">
            <div style="height: 0px">
                <div id="BlockEditorAlert" class="alert alert-success" role="alert" style="display: none; min-height: 300px; min-width: 300px;"></div>
            </div>
            <div class="NEWR  panel panel-default" style="">
                <div class="panel-heading">
                    <h3 class="panel-title"><span id="CapNewsEditor" class="caption caption-html" captionid="CapNewsEditor"></span></h3>
                    <script>$('#CapNewsEditor').html(langTable['CapNewsEditor']);</script>
                </div>
                <div class="panel-body">
                    <style>
                        .NElabel {
                            background-color: #eee;
                            color: #888;
                            font-weight: normal;
                            display: block;
                            text-align: left;
                            margin-bottom: 0px;
                        }
                    </style>
                    <div style="width: 100%">
                        <label for="NewsEditorNewsName" id="NECapName" class="NElabel label label-default  caption caption-html" captionid="CapName">Название:</label>
                        <script>$('#NECapName').html(langTable['CapName']);</script>

                        <asp:TextBox ID="NewsEditorNewsName" runat="server" class="form-control  caption caption-placeholder" placeholder="Название" ClientIDMode="Static" Style="font-size: large"></asp:TextBox>
                        <script>$('#NewsEditorNewsName').attr('placeholder', langTable['CapName']);</script>
                    </div>
                    <div style="width: 100%; margin-top: 10px;">
                        <label for="NewsEditEditorDropDown" id="NECapOwner" class="NElabel label label-default caption caption-html" captionid="CapOwner">Выпускающий:</label>
                        <script>$('#NECapOwner').html(langTable['CapOwner']);</script>

                        <asp:DropDownList ClientIDMode="Static" ID="NewsEditEditorDropDown" runat="server" Style="width: 100%; border-radius: 4PX;"></asp:DropDownList>
                    </div>
                    <div style="width: 100%; margin-top: 10px;">
                        <div style="width: calc(50% - 2px); display: inline-block">
                            <label id="NECapDateTime" class="NElabel label label-default caption caption-html" captionid="CapDateTime">Дата и время:</label>
                            <script>$('#NECapDateTime').html(langTable['CapDateTime']);</script>
                            <asp:TextBox ClientIDMode="Static" ID="NewsEditDate" placeholder="дата" runat="server" Style="width: calc(50% - 2px)"></asp:TextBox>
                            <asp:TextBox ClientIDMode="Static" ID="NewsEditTime" placeholder="время" runat="server" Style="width: calc(50%); margin-left: -2px;"></asp:TextBox>
                        </div>
                        <div style="width: calc(50% - 2px); display: inline-block">
                            <label id="NECapDuration" class="NElabel label label-default caption caption-html" captionid="CapDuration">Продолжительность:</label>
                            <script>$('#NECapDuration').html(langTable['CapDuration']);</script>
                            <asp:TextBox ClientIDMode="Static" ID="NewsEditDur" CssClass="caption caption-html" captionId="CapDuration" placeholder="продолжительность" runat="server" Style="width: 100%"></asp:TextBox>
                            <script>$('#NewsEditDur').attr('placeholder', langTable['CapDuration']);</script>
                        </div>
                    </div>
                    <div style="width: 100%; margin-top: 10px;">
                        <label id="NECapDescription" class="NElabel label label-default caption caption-html" captionid="CapDescription">Описание:</label>
                        <script>$('#NECapDescription').html(langTable['CapDescription']);</script>
                        <asp:TextBox ID="BlockEditDescription" runat="server" class="form-control caption caption-placeholder" captionId="CapDescription" placeholder="Описание" ClientIDMode="Static"></asp:TextBox>
                        <script>$('#BlockEditDescription').attr('placeholder', langTable['CapDescription']);</script>
                    </div>
                    <div style="width: 100%; margin-top: 10px;">
                        <div style="width: calc(50% - 2px); display: inline-block">
                            <div style="width: calc(50% - 2px); display: inline-block">
                                <label id="NECapChrono" class="NElabel label label-default  caption caption-html" captionid="CapChrono">Хронометраж</label>
                                <script>$('#NECapChrono').html(langTable['CapChrono']);</script>
                                <asp:TextBox ClientIDMode="Static" ID="ChronoReal" placeholder="" runat="server" Enabled="False" Style="width: 100%"></asp:TextBox>
                            </div>
                            <div style="width: calc(50% - 2px); display: inline-block">
                                <label id="NECapChronoPlanned" class="NElabel label label-default caption caption-html" captionid="CapPlanned" align="right ">Планируемый</label>
                                <script>$('#NECapChronoPlanned').html(langTable['CapPlanned']);</script>
                                <asp:TextBox ClientIDMode="Static" ID="CronoPlanned" class=" caption caption-placeholder" captionId="CapPlanned" placeholder="продолжительность" runat="server" Enabled="False" Style="width: 100%"></asp:TextBox>
                                <script>$('#CronoPlanned').attr('placeholder', langTable['CapPlanned']);</script>
                            </div>
                        </div>
                        <div style="width: calc(50% - 2px); display: inline-block">
                            <div style="width: calc(50% - 2px); display: inline-block">
                                <label id="NECapChronoCalc" class="NElabel label label-default  caption caption-html" captionid="CapCalc">Рассчетный</label>
                                <script>$('#NECapChronoCalc').html(langTable['CapCalc']);</script>
                                <asp:TextBox ClientIDMode="Static" ID="CronoCalc" runat="server" Enabled="False" Style="width: 100%"></asp:TextBox>

                            </div>
                            <div style="width: calc(50% - 2px); display: inline-block">
                            </div>
                        </div>
                    </div>
                    <div style="width: 100%; margin-top: 10px;">
                        <label id="NECapCarrier" class="NElabel label label-default  caption caption-html" captionid="CapCarrier" align="right ">Кассета</label>
                        <script>$('#NECapCarrier').html(langTable['CapCarrier']);</script>
                        <div style="width: calc(50% - 2px); display: inline-block">
                            <asp:TextBox ClientIDMode="Static" ID="Cassete" CssClass="caption caption-placeholder" captionId="CapCarrier" placeholder="кассета" runat="server" Style="width: 100%"></asp:TextBox>
                            <script>$('#Cassete').attr('placeholder', langTable['CapCarrier']);</script>
                        </div>
                        <div style="width: calc(50% - 2px); display: inline-block">
                            <asp:TextBox ClientIDMode="Static" ID="Timecode" placeholder="таймкод" CssClass="caption caption-placeholder" captionId="CapTimecode" runat="server" Style="width: 100%"></asp:TextBox>
                            <script>$('#Timecode').attr('placeholder', langTable['CapTimecode']);</script>
                        </div>
                    </div>
                   
                    <div class="BESaveBtnGroup controls form-inline " style="text-align: right">
                        <div style="width: 100%; max-width: 300px;">
                            <div id="BESaveButtonContainer" class="btn-group text-center " style="width: 100%; text-align: center;">
                                <asp:Button ID="BESaveBtn" runat="server" OnClick="NewsEditorSaveButton_Click" class="btn btn-success navbar-btn caption caption-value" captionId="CapSave" Text="Сохранить" Style="text-align: center; width: 50%;" ClientIDMode="Static" />

                                <input id="BECloseBtn" type="button" style="text-align: center; width: 50%; border-top-right-radius: 4px; border-bottom-right-radius: 4px;" class="btn btn-success navbar-btn centered caption caption-value" captionid="CapClose" onclick=" window.parent.CloseEditor(this);" value="Close" />
                                <script>
                                    $('#BESaveBtn').val(langTable['CapSave']);
                                    $('#BECloseBtn').val(langTable['CapClose']);
                                </script>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </form>
</body>
</html>
