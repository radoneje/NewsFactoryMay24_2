<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockPanel.ascx.cs" Inherits="WebApplication2.Blocks.BlockPanel" %>
<div class="panel panel-default">

    <div id="BlocksPanel" class="panel-heading" style="">
        <h2 style="margin-top: 0px;">
            <div id="bpNewsName" style="text-align: center; align-content: center" class="blocksHeadControl">
            </div>
        </h2>
        <div class="Blrow">
            <div class="BlrowLeft">
                <div id="bpNewsOwner" class="blocksHeadControl"></div>
            </div>
            <div class="BlrowRight">
                <div id="bpNewsChrono" class="blocksHeadControl"></div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="Blrow">
            <div class="BlrowLeft">
                <div id="bpNewsDate" class="blocksHeadControl"></div>
            </div>
            <div class="BlrowRight">
                <div id="bpNewsChronoPlanned" class="blocksHeadControl"></div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="Blrow">
            <div class="BlrowLeft">
                <div id="bpNewsDuration" class="blocksHeadControl"></div>
            </div>
            <div class="BlrowRight">
                <div id="bpNewsChronoCalculated" class="blocksHeadControl"></div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="Blrow">
            <div class="BlrowLeft">
                <div id="bBlockAdd" style="padding-top: 0.5em;">

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
                     <input type="button" value="unlock Items" class="btn btn-xs btn-default unLockItems" captionid="unLockItems" onclick="unLockItems()">
                </div>

            </div>


            <script>
                $('#AddBlockButton').val(langTable['AddBlockButton']);
            </script>
        </div>

    </div>

    <div id="BlocksPanelHead" class="panel-body" style="padding-top: 0px;">
        <div class="closeBtn NFBfullScreen" id="" onclick="  if($('#BlocksPanelHead').hasClass('blocksFullScreen')){$('#BlocksPanelHead').removeClass('blocksFullScreen');}  else{$('#BlocksPanelHead').addClass('blocksFullScreen');} ">
            <<
            
        </div>
        <div class="closeBtn NFBfullScreen" id="blocksCollapsedBtn">
            ++
        </div>
        <div class="closeBtn NFBfullScreen" id="blocksExpandedBtn" onclick="   ">
            --
        </div>
        <div class="clear"></div>
        <h6>

            <div class="NFBhead">
                <div class="NFBheadTitle">
                    <span id="CapMainBlockName" class="caption caption-html" captionid="CapName"></span>
                </div>
                <div class="NFBheadTime">
                    <span id="CapMainBlockFact" class="caption caption-html" captionid="CapFact"></span>
                </div>
                <div class="NFBheadTime">
                    <span id="CapMainBlockPlan" class="caption caption-html" captionid="CapPlan"></span>
                </div>
                <div class="clear">
                </div>
                <div class="loader" id="NFBheadLoader"></div>
            </div>
            <!--<table width="564" style="min-width:564px; width:100%">
                    <tr >
                        <td width="464" style="width:calc(100% - 100px)"><span id="CapMainBlockName" Class="caption caption-html" captionId="CapName"></span></td>
                        <td width="50"><span id="CapMainBlockFact" class="caption caption-html" captionId="CapFact"></span></td>
                        <td width="50"><span id="CapMainBlockPlan" class="caption caption-html" captionId="CapPlan"></span></td>
                    </tr>
                </table>-->
            <script>
                $('#CapMainBlockName').html(langTable['CapName']);
                $('#CapMainBlockFact').html(langTable['CapFact']);
                $('#CapMainBlockPlan').html(langTable['CapPlan']);
            </script>

        </h6>
        <div id="BlockContainer">
        </div>
    </div>
    <!---<table id="BlocksTable" class="table table-striped table-bordered table-hover table-condensed" dragable="false">
   
            <thead>
          <tr>
            <td width="600"><h6>Название</h6></td>
            
              <td width="70"><h6>Факт</h6></td>
              <td width="70"><h6>План</h6></td>
              <!--<th></th>
              <th></th>-->
    <!--
          </tr>
        </thead>
        <tbody>
            </tbody>
                </table> -->



</div>


