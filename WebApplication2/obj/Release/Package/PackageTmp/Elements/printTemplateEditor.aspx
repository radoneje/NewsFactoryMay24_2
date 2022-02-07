<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printTemplateEditor.aspx.cs" Inherits="WebApplication2.Elements.printTemplateEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server">
            no auth
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server">
            <!------>
            <div class="APprintTemplateSection">
                <div class="APprintTemplateSectionTitle">
                    секция выпуск
                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="WebApplication2.PrintTemplates.PrintTemplatesDataClassesDataContext" EntityTypeName="" OrderBy="Name" Select="new (id, Name, Description)" TableName="TemplateVariables" Where="Type == @Type">
                        <WhereParameters>
                            <asp:Parameter DefaultValue="0" Name="Type" Type="Int32" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                </div>

                <div>
                    <div class="APprintTemplateSectionCode">
                        <textarea id="APtxtNews"></textarea>
                    </div>
                    <div class="APprintTemplateSectionVariable">
                        <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" DataKeyField="id">
                            <ItemTemplate>
                                <div class="APprintTemplateVariabl APprintTemplateVariablNews">
                                    <asp:Label ID="NameLabel" CssClass="APprintTemplateVariablName" runat="server" Text='<%# Eval("Name") %>' />
                                    &nbsp;-&nbsp;
                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NewsFactoryConnectionString %>" SelectCommand="SELECT [Name], [Description], [id] FROM [TemplateVariables] WHERE Id<=100 ORDER BY Name, depend "></asp:SqlDataSource>
                    </div>
                    <div style="clear: both"></div>
                </div>
            </div>
            <!------>
            <div class="APprintTemplateSection">
                <div class="APprintTemplateSectionTitle">
                    секция блок
                </div>

                <div>
                    <div class="APprintTemplateSectionCode">
                        <textarea id="APtxtBlock"></textarea>
               
                    </div>
                    <div class="APprintTemplateSectionVariable">
                        <asp:DataList ID="DataList2" runat="server" DataSourceID="SqlDataSource2" DataKeyField="id">
                            <ItemTemplate>
                                <div class="APprintTemplateVariabl APprintTemplateVariablBlock">
                                    <asp:Label ID="NameLabel" CssClass="APprintTemplateVariablName" runat="server" Text='<%# Eval("Name") %>' />
                                    &nbsp;-&nbsp;
                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:NewsFactoryConnectionString %>" SelectCommand="SELECT [Name], [Description], [id] FROM [TemplateVariables] WHERE Id>100 AND ID<200  ORDER BY Name, depend "></asp:SqlDataSource>
                    </div>
                    <div style="clear: both"></div>
                </div>
            </div>
            <!------>
            <div class="APprintTemplateSection">
                <div class="APprintTemplateSectionTitle">
                    API environment and methods
                </div>

                <div>

                    <div class="APprintTemplateVariabl ">
                        <span>/bootstrap/bootstrap-3.3.2-dist/css/bootstrap.min.css</span>
                        <span>Bootstrap path</span>
                    </div>
                    <div class="APprintTemplateVariabl ">
                        <span>/scripts/jquery-2.0.1.min.js</span>
                        <span>jquery path</span>
                    </div>
                    <div class="APprintTemplateVariabl ">
                        <span>/api/news/video/{newsId}</span>
                        <span>JSON array of video files in item</span>
                    </div>
                    <div class="APprintTemplateVariabl ">
                        <span>/rtf/{newsId}/{filename}.rtf</span>
                        <span>RTF text for prompter</span>
                    </div>
                    <div class="APprintTemplateVariabl ">
                        <span>/rtf/{newsId}/{filename}.txt</span>
                        <span>TXT UTF-8 text for prompter</span>
                    </div>
                    <div class="APprintTemplateVariabl ">
                        <span>/rtf/{newsId}/{filename}.xls</span>
                        <span>xls file of titles news cast</span>
                    </div>
                     <div class="APprintTemplateVariabl ">
                        <span>/title/{newsId}/{filename}.xls</span>
                        <span>TXT UTF-8 file of titles news cast</span>
                    </div>
                      <div class="APprintTemplateVariabl ">
                        <span>/word/{newsId}/{filename}.rtf</span>
                        <span>RTF  file of news cast</span>
                    </div>

                </div>
               
                <script>
            
                    $.ajax({
                        type: "POST",
                        contentType: "application/json;",
                        url: serverRoot + "testservice.asmx/printTemplLoad",
                        data: JSON.stringify({ id: <%= Request.Params["id"]%> }),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        dt = JSON.parse(data.d);
                        if (dt.status < 1)
                            return ajaxErr("/testservice.printTemplLoad error", data);
                        $("#APtxtNews").val(dt.items[0].news);
                      //  codeMirrorNews.setValue(dt.items[0].news)
                        $("#APtxtBlock").val(dt.items[0].block);
                    },
                    error: function (data) {
                        ajaxErr("/testservice.asmx/printTemplLoad error", data);

                    }

                });
                $(".APprintTemplateVariablNews").click(APprintTemplateVariablNewsClick);
                function APprintTemplateVariablNewsClick(e){
     
                    //$("#APtxtNews").val($("#APtxtNews").val()+$(e.currentTarget).find(".APprintTemplateVariablName").html());
                    txtareaInsertAtCaret("APtxtNews",$(e.currentTarget).find(".APprintTemplateVariablName").html());
                    $("#APtxtNews").focus();
                }
                $(".APprintTemplateVariablBlock").click(APprintTemplateVariablBlockClick);
                function APprintTemplateVariablBlockClick(e){
     
                    //$("#APtxtNews").val($("#APtxtNews").val()+$(e.currentTarget).find(".APprintTemplateVariablName").html());
                    txtareaInsertAtCaret("APtxtBlock",$(e.currentTarget).find(".APprintTemplateVariablName").html());
                    $("#APtxtBlock").focus();
                }
                $(".APprintTemplateSectionCode textarea").change(function(){
                    var prm={
                        id:<%= Request.Params["id"]%>,
                        news:$("#APtxtNews").val(),
                        block:$("#APtxtBlock").val()
                    }
                    $.ajax({
                        type: "POST",
                        contentType: "application/json;",
                        url: serverRoot + "testservice.asmx/printTempSave",
                        data: JSON.stringify(prm),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            dt = JSON.parse(data.d);
                            if (dt.status < 1)
                                return ajaxErr("/testservice.printTempSave error", data);
                            $("#APtxtNews").val(dt.items[0].news);
                            $("#APtxtBlock").val(dt.items[0].block);
                        },
                        error: function (data) {
                            ajaxErr("/testservice.asmx/printTempSave error", data);

                        }
                
                    })
                });
                </script>
        </asp:Panel>

    </form>
</body>
</html>
