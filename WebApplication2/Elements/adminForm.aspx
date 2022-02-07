<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminForm.aspx.cs" Inherits="WebApplication2.Elements.adminForm" EnableViewState="False" %>

<body>
    <form id="form1" runat="server">
        <div id="adminMainBox">
            <div class="adminForm panel panel-default" style="margin: 10px 5px;">
                <asp:Panel runat="server" ID="errorPanel">
                    <div class="alert alert-danger" style="margin: 10px 5px;">
                        <h4>Ошибка!</h4>
                        Вы не зарегистрированы для просмотра этой страницы.
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="workPanel" class="padding: 10px;">

                    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                        <!---->
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingGeo">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseGeo" aria-expanded="false" aria-controls="collapseGeo"><span id="">GEO title tags</span>
                                  
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseGeo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="collapseGeo">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <!-- Modal -->

                                        <div style="text-align: right">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="addToGeoTags()">
                                        </div>

                                        <div id="blockGeeoModalBody">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <!---->
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingSrc">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseSrc" aria-expanded="false" aria-controls="collapseSrc"><span id="">SOURCE title tags</span>
                                  
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseSrc" class="panel-collapse collapse" role="tabpanel" aria-labelledby="collapseSrc">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <!-- Modal -->

                                        <div style="text-align: right">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="addToSrcTags()">
                                        </div>

                                        <div id="blockSrcModalBody">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <!---->
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingOne">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><span id="AASOTtemplates"></span>
                                        <script>$("#AASOTtemplates").html(langTable["SOTtemplates"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <!-- Modal -->

                                        <div style="text-align: right">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="addToSyncTemplate()">
                                        </div>

                                        <div id="blockEditModalBody">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--------->
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading2">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse2" aria-expanded="false" aria-controls="collapse2"><%=(string)Session["UserName"] %>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse2" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading2">
                                <div class="panel-body">
                                    <div id="adminPassChageForm" class="gridControls" action="passwordChange">
                                        <input type="password" name="oldPass" id="adminOldPass" class="adminControls" maxlength="50" placeholder="current password" />
                                        <input type="password" name="newPass" id="adminNewPass1" class="adminControls" maxlength="50" placeholder="new  password" />
                                        <input type="password" name="newPass2" id="adminNewPass2" class="adminControls" maxlength="50" placeholder="new password" />
                                        <input type="button" class="adminControls btn btn-danger btn-xs " value="change" onclick="adminPasswordChange(event)" />
                                    </div>
                                    <div id="adminReadRateChageForm" class="gridControls" action="readRateChange">
                                        <input type="text" name="readRate" id="adminReadSpeed" class="adminControls" maxlength="50" placeholder="17 sybmols per second" />
                                        <input type="button" class="adminControls btn btn-warning btn-xs " value="change read rate" onclick="adminReadRateChange()" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" id="APsocialPanel" runat="server">
                            <div class="panel-heading" role="tab" id="heading3">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse3" aria-expanded="false" aria-controls="collapse3"><span id="AASocialNetwork"></span>
                                        <script>$("#AASocialNetwork").html(langTable["SocialNetwork"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading3">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="addSocial()" />
                                            <div class="socialBox">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APrssPanel">
                            <div class="panel-heading" role="tab" id="heading4">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse4" aria-expanded="false" aria-controls="collapse3"><span id="AARSSfeed"></span>
                                        <script>$("#AARSSfeed").html(langTable["RSSfeed"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading4">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="addRss()" />
                                            <div class="rssBox">
                                                <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APblockdeletedPanel">
                            <div class="panel-heading" role="tab" id="heading5">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse5" aria-expanded="false" aria-controls="collapse3"><span id="AAdeleteditems"></span>
                                        <script>$("#AAdeleteditems").html(langTable["deleteditems"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse5" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading5">
                                <div class="panel-body">
                                    <div class="gridControls">


                                        <div class="APblockDeletedBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APNewsDeletedPanel">
                            <div class="panel-heading" role="tab" id="heading51">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse51" aria-expanded="false" aria-controls="collapse51"><span id="AAdeletedNews"></span>
                                        <script>$("#AAdeletedNews").html(langTable["deletedNewsCasts"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse51" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading51">
                                <div class="panel-body">
                                    <div class="gridControls">


                                        <div class="APblockDeletedNews">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APNewsPanel">
                            <div class="panel-heading" role="tab" id="heading6">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse6" aria-expanded="false" aria-controls="collapse3"><span id="AAPrograms"></span>
                                        <script>$("#AAPrograms").html(langTable["Programs"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse6" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading5">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="APprogAdd()" />
                                        </div>
                                        <div class="APprogBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APRolePanel">
                            <div class="panel-heading" role="tab" id="heading7">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse7" aria-expanded="false" aria-controls="collapse3"><span id="AAUsersroles"></span>
                                        <script>$("#AAUsersroles").html(langTable["Usersroles"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse7" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading5">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="AProleAdd()" />
                                        </div>
                                        <div class="AProleBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APuserPanel">
                            <div class="panel-heading" role="tab" id="heading8">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse8" aria-expanded="false" aria-controls="collapse3"><span id="AAUsers"></span>
                                        <script>$("#AAUsers").html(langTable["Users"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse8" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading5">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="APuserAdd()" />
                                        </div>
                                        <div class="APuserBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APblockTypePanel">
                            <div class="panel-heading" role="tab" id="heading9">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse9" aria-expanded="false" aria-controls="collapse3"><span id="AAItemtypes"></span>
                                        <script>$("#AAItemtypes").html(langTable["Itemtypes"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapse9" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading9">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="APblockTypeAdd()" />
                                        </div>
                                        <div class="APblockTypeBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->
                        <!------>
                        <div class="panel panel-default" runat="server" id="APprintTemplatePanel">
                            <div class="panel-heading" role="tab" id="heading10">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapsePrintTemplate" aria-expanded="false" aria-controls="collapse10"><span id="AAScriptTemplates"></span>
                                        <script>$("#AAScriptTemplates").html(langTable["ScriptTemplates"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapsePrintTemplate" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading10">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="APprintTemplAdd()" />
                                        </div>
                                        <div class="APprintTemplateBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------->

                        <div class="panel panel-default" runat="server" id="APplayoutTemplatePanel">
                            <div class="panel-heading" role="tab" id="heading11">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapsePlayOut" aria-expanded="false" aria-controls="collapse10"><span id="AAcollapsePlayOut"></span>
                                        <script>$("#AAcollapsePlayOut").html(langTable["PlayOuts"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapsePlayOut" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading10">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="APlayOutAdd()" />
                                        </div>
                                        <div class="APlayOutBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!------>
                        <div class="panel panel-default" runat="server" id="APitleOutPanel">
                            <div class="panel-heading" role="tab" id="heading12">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTitleOut" aria-expanded="false" aria-controls="collapse10"><span id="AAcollapseTitleOut"></span>
                                        <script>$("#AAcollapseTitleOut").html(langTable["TitleOuts"])</script>
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseTitleOut" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading10">
                                <div class="panel-body">
                                    <div class="gridControls">
                                        <div class="addSocialBtnWr">
                                            <input type="button" value="add" class="btn btn-success btn-xs" onclick="ATitleOutAdd()" />
                                        </div>
                                        <div class="ATitleOutBox">
                                            <img src="<%=(string)Application["serverRoot"] %>Images/loading.gif" style="width: 50px" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!------>
                    </div>

                </asp:Panel>

            </div>
            <script src="<%=Application["serverRoot"] %>Scripts/adminPanelScript.js?rand=123"></script>
        </div>

    </form>


</body>

