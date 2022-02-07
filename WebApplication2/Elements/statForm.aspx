<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="statForm.aspx.cs" Inherits="WebApplication2.Elements.statForm" %>

<!DOCTYPE html>


<form id="form1" runat="server">
    <div id="statFormMainBox">
    <div class="statForm panel panel-default" style="margin: 10px 5px;">
        <asp:panel runat="server" id="errorPanel">
        <div class="alert alert-danger" style="    margin: 10px 5px;">
            <h4>Ошибка!</h4>Вы не зарегистрированы для просмотра этой страницы.
        </div>
           </asp:panel>
        <asp:panel runat="server" id="workPanel" class="padding: 10px;">
             <script src="/Scripts/statPanelScript.js"></script>
            <div class="gridControls">
                           <span>
                               start from:
                           </span>
                <div id="statPanelSearchDatarange" class="adminControls control-xs" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
                                <i class="glyphicon glyphicon-calendar fa"></i>
                                <span></span><b class="caret"></b>
                            </div>
                          <span>
                             <input type="button" class="btn btn-success btn-xs statParamBtn" name="author" value="Author" onclick="statChangeParams(event)" /> 
                               <input type="button" class="btn btn-default btn-xs statParamBtn" name="cameraman" value="Cameraman"  onclick="statChangeParams(event)" />
                              <input type="button" class="btn btn-default btn-xs statParamBtn" name="jockey" value="Anchor"  onclick="statChangeParams(event)"/>
                               <input type="button" class="btn btn-default btn-xs statParamBtn" name="cutter" value="Editor" onclick="statChangeParams(event)" />
                               
                           </span>
          
                <div id="statPanelSearchForm" action="statGet">
                    <input type="hidden" id="dateStart" name="dateStart" />
                     <input type="hidden"id="dateEnd"  name="dateEnd" />
            <!--     <input type="button" class="adminControls btn btn-default btn-xs " value="построить статистику" onclick="statGet()"/>-->
         </div>
               </div>
             <div class="gridControls">
                 </div>
         </asp:panel>
    </div>
        </div>
</form>
