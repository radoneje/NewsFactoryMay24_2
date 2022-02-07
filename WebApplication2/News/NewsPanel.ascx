<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsPanel.ascx.cs" Inherits="WebApplication2.News.NewsPanel" %>
<%@ Register Src="~/Elements/ProgramComoBox.ascx" TagPrefix="uc1" TagName="ProgramComoBox" %>
<%@ Register Src="~/News/NewsTree.ascx" TagPrefix="uc1" TagName="NewsTree" %>


<div class="panel panel-default">
    <div class="panel-heading">
    <h3 class="panel-title">
       <!-- <asp:DropDownList ID="ProgramDropDown"  runat="server" ClientIDMode="Static">
        </asp:DropDownList>-->
        <select id="ProgramDropDown" onchange="programOnChange()" class="form-control">
            <option id="defProgramDropDown" value="-1" selected>
                ...
            </option>
        </select>
    </h3>
              
        <!---id="btnAddNews"-->
                         <div class="btn-group" role="group" aria-label="...">
 <input type="button" id="AddProgBtn" visible="false" class="btn btn-default btn-xs caption caption-value AddNewsButton" captionid="AddProgBtn" width="100%" onclick="AddNews();" />
          
  <div class="btn-group" role="group">
    <button type="button" class="btn btn-default btn-xs dropdown-toggle AddNewsButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" onclick="addNewsInline(this)">
      <span class="caret" onclick="addNewsInline($(this).parent())"></span>
    </button>
    <ul class="dropdown-menu">
   
    </ul>
  </div>
</div>
           
     <script>$('#AddProgBtn').val(langTable['AddProgBtn'])</script>
              <!---->
        <!---
                             
                <div class="btn-group" role="group" aria-label="...">
 <input type="button" id="AddBlockButton" visible="false" class="btn btn-default btn-xs caption caption-value AddBlockButton" captionid="AddBlockButton" width="100%" onclick="AddBlocks();" />
        
  <div class="btn-group" role="group">
    <button type="button" class="btn btn-default btn-xs dropdown-toggle AddBlockButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" onclick="addBlockByTypeClick(this)">
      <span class="caret" onclick="addBlockByTypeClick($(this).parent())"></span>
    </button>
    <ul class="dropdown-menu">
   
    </ul>
  </div>
</div>
            -->


         </div>
  <div   class="panel-body">
      <div id="NewsContainer">
            </div>
 
     <table id="NewsTable" class="table table-striped table-bordered table-hover table-condensed">
     </table>
  </div>
   
</div>
