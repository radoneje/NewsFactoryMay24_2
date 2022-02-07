<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockNamePanel.ascx.cs" Inherits="WebApplication2.Elements.BlocksExtViewer.BlockNamePanel" %>

<div class="panel panel-default">
  <div class="panel-heading">
    <h3 class="panel-title">
        <asp:Literal ID="TitleLiteral" runat="server"></asp:Literal></h3>
      <asp:Literal ID="SubTitleLiteral" runat="server"></asp:Literal>
      
  </div>
  <div class="panel-body">
      <div id="extText" class="panel panel-default">
    <asp:Literal ID="TextLiteral" runat="server"></asp:Literal>
          </div>
      <asp:Panel ID="CommentPanel" runat="server" >
          <div >
          <asp:TextBox ID="CommentTb" placeholder="комментарий" runat="server" style="width:100%" Rows="3" MaxLength="255" TextMode="MultiLine"></asp:TextBox>
         </div>
          <div class="panel panel-default">
              <div style="width:49%; display:inline-block ">
          <small><input id="fileupload" type="file" name="files[]" data-url="FileUpload" multiple onchange="RequestToUpload($(this),'BlockGiud:<%=BlockGuid %>');"/> </small>
          </div>
          <div style="width:49%; display:inline-block " id='FileUploader' class ="FileUploader">
           <div id="FileUploaderTitle" style="height: 10px; margin-bottom: 10px;"></div>
              <div class="progress" style="height: 10px; margin-bottom: 2px;">
                 <div id="FileUploaderProgress" class="progress-bar" role="progressbar" percentComplete="-1" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; height:5px;">\
  
           </div>
                  </div>
              </div>

            
          </div>
          <script src="/Scripts/Utils.js"></script>
          <script src="/Scripts/FileUpload.js"></script>

          <script>
              $("#CommentTb").focus();
          </script>
           
          <div style="width:100%;  text-align:center;">
              <asp:Panel ID="SaveButtonsPanel" runat="server">
          <asp:Button ID="SaveTb" runat="server" Text="Сохранить" CssClass="btn btn-default btn-success" OnClick="SaveTb_Click" />
               </asp:Panel>
         </div>
       </asp:Panel>
  </div>
</div>