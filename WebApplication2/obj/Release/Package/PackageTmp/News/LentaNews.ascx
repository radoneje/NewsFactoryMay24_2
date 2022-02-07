<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LentaNews.ascx.cs" Inherits="WebApplication2.News.LentaNews" %>
<div id="LentaNewsContainer" class="LentaNewsContainer">

    <div class="panel panel-default">
        <div class="panel-heading" style="height: 135px;">
            <h3 class="panel-title">
                <span id="CapLenta" class="caption caption-html" captionid="CapLenta"></span>
                <script>$('#CapLenta').html(langTable['CapLenta'])</script>
                <div id="LentaOverload" captionid="ArchiveOverload" class="alert alert-warning caption caption-html" role="alert">
                    <b>Внимание!</b>Найдено более 100 блоков, показаны первые 100.Уточните критерии.
                </div>
                <script>$('#LentaOverload').html(langTable['ArchiveOverload'])</script>
            </h3>
            <div id="LentaBlocksHeaderContainer">
                &nbsp;
            </div>
        </div>
        <div class="panel-body">
            <div id="LentaFindNewsContainer">
            </div>
        </div>

    </div>
</div>

