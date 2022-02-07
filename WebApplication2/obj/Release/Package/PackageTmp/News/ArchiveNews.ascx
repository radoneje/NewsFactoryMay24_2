<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchiveNews.ascx.cs" Inherits="WebApplication2.News.ArchiveNews" %>
<form onsubmit="ArchiveSearch(); return false;">
<div id="ArchiveNewsContainer" class="ArchiveNewsContainer">
          
            <div class="panel panel-default">
  <div class="panel-heading" style="height:135px;">
    <h3 class="panel-title">
        
        <span id="CapArchive" class="caption caption-html" captionId="CapArchive"></span>
        <script>$('#CapArchive').html(langTable['CapArchive'])</script>
      <div id="ArchiveOverload" captionId="ArchiveOverload" class="alert alert-warning caption caption-html" role="alert">
                
             </div>
        <script>$('#ArchiveOverload').html(langTable['ArchiveOverload'])</script>
    </h3>
      <div id="ArchiveBlocksHeaderContainer">
            &nbsp;
   </div>
  </div>
  <div   class="panel-body">
        <div id="ArchiveFindNewsContainer">
         </div>
  </div>
    
</div>
</div>
    </form>