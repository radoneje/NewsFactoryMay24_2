<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchiveBlocksConteiner.ascx.cs" Inherits="WebApplication2.Blocks.ArchiveBlocksConteiner" %>
<div id="ArchiveBlocksContainer">

    <div class="panel panel-default">
        <div class="panel-heading ">
            <div style="height: 115px;">
                <h3 class="panel-title">
                    <div class="input-group input-group-sm input-group-vertical" style="width: 100%">
                        <!-- /input-group -->
                        <div class="span2" id="ArchiveSearchControlContainer">
                            <input type="text" id="ArchiveText" class="form-control caption caption-placeholder " captionId="capText" />
                            <script>$('#ArchiveText').attr('placeholder', langTable['capText'])</script>
                            <div style="display: flex;width: 100%;align-items: center;">
                            <div style="width:calc(50% - 10PX);">
                            <div id="ArchiveSearchDatarange1" class="pull-right form-control input-sm" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
                                <i class="glyphicon glyphicon-calendar fa"></i>
                                <span></span><b class="caret"></b>
                            </div>
                                </div>
                            <div style="width:calc(50% - 10PX);">
                                <div id="ArchiveSearchBtnMountAgo" class="btn btn-default btn-xs  caption caption-html" onclick="arhiveSearchMounth(1)">One mounth ago</div>
                                <div id="ArchiveSearchBtn2MountAgo" class="btn btn-default btn-xs  caption caption-html" onclick="arhiveSearchMounth(2)">Two months ago</div>
                                <div id="ArchiveSearchBtn3MountAgo" class="btn btn-default btn-xs  caption caption-html" onclick="arhiveSearchMounth(3)">Three months ago</div>
                                </div>
                                 <script>$('#ArchiveSearchBtnMountAgo').html(langTable['ArchiveSearchBtnMountAgo'])</script>
                                <script>$('#ArchiveSearchBtn2MountAgo').html(langTable['ArchiveSearchBtn2MountAgo'])</script>
                                <script>$('#ArchiveSearchBtn3MountAgo').html(langTable['ArchiveSearchBtn3MountAgo'])</script>
                               
                            </div>
                            <asp:DropDownList ID="ArchiveAutorDropDown" onchange="ReloadNews()" CssClass="form-control input-xs" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <script>
                                $("#ArchiveAutorDropDown").children(":first").html(langTable['SelectAutor']);
                                $("#ArchiveAutorDropDown").children(":first").addClass('caption');
                                $("#ArchiveAutorDropDown").children(":first").addClass('caption-html');
                                $("#ArchiveAutorDropDown").children(":first").attr('captionId', 'SelectAutor');
                            </script>
                            <!-- <span class="input-group-btn" >-->
                            <div class="btn-group" role="group" aria-label="..." id="ArchiveSearchControlButtonContainer">
                                <input id="ArchiveSearchBtn" class="btn btn-default btn-xs  caption caption-value" captionId="ArchiveSearchBtn" type="button" onclick="ArchiveSearch()"></input>
                                 <script>$('#ArchiveSearchBtn').val(langTable['ArchiveSearchBtn'])</script>
                                <div class="btn-group" role="group" aria-label="...">
                                    <!-- </span> -->
                                </div>
                            </div>
                            <!-- /input-group -->
                </h3>
            </div>
        </div>
        <div class="panel-body">
            <div id="ArchiveFindBlocksContainer">
            </div>
        </div>

    </div>
</div>
