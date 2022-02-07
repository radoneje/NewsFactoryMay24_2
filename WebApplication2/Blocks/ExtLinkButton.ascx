<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExtLinkButton.ascx.cs" Inherits="WebApplication2.Blocks.ExtLinkButton" %>


<div id="ExtLinkOn" class="btn-group btn-group-xs" role="group" style="display:inline-block;border: 1px solid transparent; border-radius: 4px; border-color: #ddd; width:595px;">
   
    <input type="button" id="bTempLinkOff" class="btn btn-default  caption caption-value " captionId="CapTempLinkOff" onclick="GetExtLink(false);$('#ExtLinkOn').hide();$('#ExtLinkOff').show();"/>
     <script>$('#bTempLinkOff').val(langTable['CapTempLinkOff'])</script>
                <div style="display:inline-block;width:515px;">
             <input type="text"  id="ExtLink" readonly  onclick="$(this).select();" style="display:inline-block;width:300px; font-size: 12px; margin-left:3px ;"/>
            <input type="checkbox" onchange="GetExtLink(true);"  id="ExtLinkApplyDate" style="font-size:10px;">
            <input type="text" onchange="GetExtLink(true);" id="ExtLinkDate" value="" style="font-size:10px; width:60px"/>
               <label for="ExtIsCommentableCB" style="font-size:9px" class="caption caption-html " captionId="CapComment"><small></small></label>
                     <script>$("label[for='ExtIsCommentableCB']").html(langTable["CapComment"]);</script>
                    <input type="checkbox" onchange="GetExtLink(true);"  id="ExtIsCommentableCB" style="font-size:10px;">
    </div>
    </div>
<div id="ExtLinkOff" class="btn-group btn-group-xs" role="group" style="display:inline-block;border: 1px solid transparent; border-radius: 4px; border-color: #ddd; width:595px;">
    
        <input id="bTempLinkOn" type="button" class="btn btn-default caption caption-value " captionId="TempLinkOn" style="width:65px"  onclick="GetExtLink(true); $('#ExtLinkOn').show();$('#ExtLinkOff').hide();"/>
                                     <script>$('#bTempLinkOn').val(langTable['TempLinkOn'])</script>
        <div style="display:inline-block;width:345px;">
            <div  contenteditable="false" style="display:inline-block;width:185px; font-size: 10px; padding-left:2px ;" placeholder="Временная ссылка"></div>
    </div>
  </div>
  