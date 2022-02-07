<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="blockInplaceEditorMenu.aspx.cs" Inherits="WebApplication2.Blocks.blockInplaceEditorMenu" %>

<!DOCTYPE html>

    
 <div class="inplaceBlockEditorMenuWr">
     <div class="inplaceBlockEditorMenuApproveReadyWr">
   <label for="inplaceBlockEditorMenuReady">готов</label>
     <input type="checkbox"  id="inplaceBlockEditorMenuReady" <%=isReady()?"checked='checked'":"" %>/>
     <label for="inplaceBlockEditorMenuApprove">принят</label>
     <input type="checkbox"  id="inplaceBlockEditorMenuApprove" <%=isApprove()?"checked='checked'":"" %>/>
         </div>
  <input type="button" class="inplaceBlockEditorMenuClose" onclick="inplaceBlockEditorClose('<%=Request.Params["blockId"]%>')" />

 </div>
<script>
    $(".inplaceBlockEditorMenuWr [type='button']").val(langTable['CapClose']);
    $("label[for='inplaceBlockEditorMenuReady']").html(langTable["CapReady"]);
    $("label[for='inplaceBlockEditorMenuApprove']").html(langTable["CapApprove"]);


    $("#inplaceBlockEditorMenuReady").click(function (e) {  if (! $(e.target).prop('checked')) { $('#inplaceBlockEditorMenuApprove').prop('checked', false); }; });
    $("#inplaceBlockEditorMenuApprove").click(function (e) { if ($(e.target).prop('checked')) { $('#inplaceBlockEditorMenuReady').prop('checked', true); }; });
    $("#inplaceBlockEditorMenuReady").change(function(e){
        inplaceBlockEditorMenuStateChange(e);
    
    });
    $("#inplaceBlockEditorMenuApprove").change(function(e){
        inplaceBlockEditorMenuStateChange(e);
    
    });

</script>
   

