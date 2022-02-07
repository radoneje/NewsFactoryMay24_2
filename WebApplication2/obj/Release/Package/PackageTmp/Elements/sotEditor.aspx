<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sotEditor.aspx.cs" Inherits="WebApplication2.Elements.sotEditor" %>



<form id="form_sot" runat="server">
    <div style="margin-bottom:10px;">
        <div id="BEChronoContainer" class="control-group">

                                            <div class="controls form-inline ">
                                                <div>
                                                    <label for="addSotChrono" class="label label-default BElabel " >Chrono</label>
                                                    <input name="addSotChrono" type="text" value="00:00:00"  id="addSotChrono" class="form-control BlockEditDropDown" style="width:90px;" onchange="addSotEditChrono()">
                                                </div>
                                            </div>

                                            <div class="controls form-inline ">
                                                <div>
                                                    <label for="addSotIn" class="label label-default BElabel " >Mark IN</label>
                                                    <input name="addSotIn" type="text" value="00:00:00" id="addSotIn" class="form-control BlockEditDropDown selAll" onchange="isSaved=false;addSotEditOut()" onkeyup="addSotEditOut()" style="width:90px;" >
                                                </div>
                                            </div>
                                            <div class="controls form-inline ">
                                                <div>
                                                    <label for="addSotOut" id="labelBlockEditRealTextBox" clicked="false" class="label label-default BElabel" style="cursor: pointer" data-toggle="tooltip" data-placement="top" onchange="">Mark OUT</label>
                                                    <input name="addSotOut" type="text" value="00:00:00" id="addSotOut" class="form-control BlockEditDropDown selAll" onchange="isSaved=false;addSotEditOut();" onkeyup="addSotEditOut()" style="width:90px;">
                                                </div>
                                            </div>
       
                                            <div style="clear: both"></div>
                                        </div>
    </div>
    <div class="input-group">
        <span class="input-group-addon" id="basicaddonSotName">Name:&nbsp</span>
        <input type="text" class="form-control sotName" aria-describedby="basicaddonSotName">
    </div>
    <div class="input-group">
        <span class="input-group-addon" id="basicaddonSotTitle">Title:&nbsp&nbsp&nbsp&nbsp</span>
        <input type="text" class="form-control sotTitle" aria-describedby="basicaddonSotTitle">
    </div>
   
    <textarea class="sotText"></textarea>
    <input type="button" class="btn btn-default btn-success" value="Add Sot" onclick="insertSot()" />
</form>
<script>
    $(document).ready(function () {
        $(".selAll").focus(function () { $(this).select(); });
    });
</script>
