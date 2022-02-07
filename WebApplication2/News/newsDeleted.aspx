<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newsDeleted.aspx.cs" Inherits="WebApplication2.News.newsDeleted" %>

<body>
     <style>
         .news-row:nth-child(odd) {
          background-color:#eee
        }
     
         .news-row{
             border-bottom:1px solid #ddd
         }
    </style>

        
    <div style="margin:20px">
    <div style="font-weight:bold">
        <div style="display:inline-block; width:200px" > id</div>
        <div style="display:inline-block; width:calc(100% - 210px);"> programm</div>
        <div style="display:block; width:100%"> title</div>
        <div style="display:inline-block; width:200px;"> date</div>
     
    </div>
<div id="rows" runat="server"></div>
     
   </div>


</body>
