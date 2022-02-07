<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsColumn.ascx.cs" Inherits="WebApplication2.LIte.NewsElements.NewsColumn" %>
<div class="panel panel-default">
  <div class="panel-heading">
   <asp:DropDownList ID="ProgrammDropDown" runat="server" style="width:100%" CssClass="form-control" placeholder="Выберите программу" OnSelectedIndexChanged="ProgrammDropDown_SelectedIndexChanged"  AutoPostBack="true" >
  
    </asp:DropDownList>
  </div>
  <div class="panel-body">
    <div class="panel panel-default">     
    <div class="panel-heading" style="cursor:pointer" onclick="window.location.href='/l/<%=ProgrammDropDown.SelectedValue %>/1/#NewsGroup1'">
        <h5>Планируемые</h5>
        </div>
        <div class="panel-body">
            <a name="NewsGroup1"></a>
           <asp:Panel ID="NewsPlanned" runat="server"></asp:Panel>
        </div>
        <div class="panel-heading" style="cursor:pointer" onclick="window.location.href='/l/<%=ProgrammDropDown.SelectedValue %>/2/#NewsGroup2'">
        <a name="NewsGroup2"></a>
            <h5>Текущие</h5>
        </div>
        <div class="panel-body">
            <asp:Panel ID="NewsToday" runat="server"></asp:Panel>
        </div>
        <div class="panel-heading" style="cursor:pointer" onclick="window.location.href='/l/<%=ProgrammDropDown.SelectedValue %>/3/#NewsGroup3'">
        <a name="NewsGroup3"></a>
            <h5>Прошедшие</h5>
        </div>
        <div class="panel-body">
           <asp:Panel ID="NewsLast" runat="server"></asp:Panel>
        </div>
    </div>
  </div>
</div>