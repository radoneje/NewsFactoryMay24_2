<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlockHead.ascx.cs" Inherits="WebApplication2.LIte.BlocksElements.BlockHead" %>

    <a name="blocks"></a>
            <h2><div id="bpNewsName" style="text-align:center; align-content:center"><asp:Literal ID="NewsNameLiteral" runat="server"></asp:Literal></div></h2>   
               <div class="row">
                <div style="width:50%; display:inline-block; margin-left:10px">
                    <div id="bpNewsOwner">редактор: <asp:Literal ID="EditorLiteral" runat="server"></asp:Literal></div>
                    <div id="bpNewsDate">дата: <asp:Literal ID="DateLiteral" runat="server"></asp:Literal></div>
                    <div id="bpNewsDuration">задано: <asp:Literal ID="DurationLiteral" runat="server"></asp:Literal></div>
                </div>
        
                <div style="width:30%;display:inline-block; margin-right:5px;float:right">
                    <div id="bpNewsChrono" style="float:right;display:block">xpон: <asp:Literal ID="ChronoLiteral" runat="server"></asp:Literal></div>
                    <br />
                    <div id="bpNewsChronoPlanned" style="float:right;display:block">план: <asp:Literal ID="PlannedLiteral" runat="server"></asp:Literal></div>
                    <br />
                    <div id="bpNewsChronoCalculated" style="float:right;display:block">факт: <asp:Literal ID="CalcLiteral" runat="server"></asp:Literal></div>
                </div>
        </div>
                   
 
 