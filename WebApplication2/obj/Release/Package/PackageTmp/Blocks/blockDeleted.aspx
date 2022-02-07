<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="blockDeleted.aspx.cs" Inherits="WebApplication2.Blocks.blockDeleted" %>
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
        <div style="display:inline-block; width:200px;"> Person</div>
        <div style="display:inline-block; width:100px"> </div>
    </div>
<div id="rows" runat="server"></div>
     
   </div>


</body>