<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chernovik.aspx.cs" Inherits="WebApplication2.Elements.chernovik" %>

    <form id="form1" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
                 
            <div class="closeBtn" onclick="$('.chernovik').fadeOut(500)">Х</div>
                     
                     <div style="clear:both"></div>
            <h4>Черновик</h4>
            </div>
         <div class="panel-body">
             <div class="newsSelect">

             </div>
           <textarea onpaste="chernovikPaste"></textarea>
             </div>
    </div>
    </form>
