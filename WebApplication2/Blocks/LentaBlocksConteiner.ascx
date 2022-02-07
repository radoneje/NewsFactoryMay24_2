<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LentaBlocksConteiner.ascx.cs" Inherits="WebApplication2.Blocks.LentaBlocksConteiner" %>
<div id="LentaBlocksContainer">

    <div class="panel panel-default">
        <div class="panel-heading ">
            <div style="height: 115px;">
                <h3 class="panel-title">
                    <div class="input-group input-group-sm input-group-vertical" style="width: 100%">
                        <!-- /input-group -->
                        <div class="span2" id="LentaSearchControlContainer">

                            <input type="text" id="LentaText" class="form-control caption caption-placeholder " captionid="capText" />
                            <script>$('#LentaText').attr('placeholder', langTable['capText'])</script>
                            <div id="LentaSearchDatarange1" class="pull-right form-control input-sm" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
                                <i class="glyphicon glyphicon-calendar fa"></i>
                                <span></span><b class="caret"></b>
                            </div>
                            <asp:DropDownList ID="LentaSourceDropDown" CssClass="form-control input-xs" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <script>
                                $("#LentaSourceDropDown").children(":first").html(langTable['SelectSource']);
                                $("#LentaSourceDropDown").children(":first").addClass('caption');
                                $("#LentaSourceDropDown").children(":first").addClass('caption-html');
                                $("#LentaSourceDropDown").children(":first").attr('captionId', 'SelectSource');
                            </script>
                            <!-- <span class="input-group-btn" >-->
                            <div class="btn-group" role="group" aria-label="..." id="LentaSearchControlButtonContainer">
                                <input class="btn btn-default btn-xs caption caption-value" id="LentaSearchBtn" captionid="LentaSearchBtn" type="button" onclick="LentaSearch()" />
                                <script>$('#LentaSearchBtn').val(langTable['LentaSearchBtn'])</script>
                                <div class="btn-group" role="group" aria-label="...">
                                    <!-- </span> -->
                                </div>
                            </div>
                            <!-- /input-group -->
                        </div>
                    </div>
                </h3>
            </div>
        </div>

        <div class="panel-body">
            <div id="LentaFindBlocksContainer">
            </div>
        </div>
    </div>
</div>
