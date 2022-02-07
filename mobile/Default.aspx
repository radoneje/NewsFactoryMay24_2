<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="mobile.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mobile News Factory May 24</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <script src="/scripts/jquery-2.1.3.min.js"></script>
    <script src="/scripts/bootstrap.min.js"></script>
    <script src="/scripts/mobileScript.js"></script>
    <link href="/styles/bootstrap.min.css" rel="stylesheet" />
    <link href="/styles/mobyleStyle.css" rel="stylesheet" />
    <link href="/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <img src="/images/NewsFactoryLogoW300_notext.png" style="height: 30px; margin: 5px;" />
        
        <div class="loginForm">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="alert alert-success">
                        <h4>Пожалуйста, зарегистрируйтесь</h4>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group-sm">
                        <div id="cWrong" class="alert alert-danger">Имя или пароль неверные.</div>
                        <input type="text" id="tLogin" placeholder="Ваше имя" maxlength="255" class="form-control" />
                        <input type="password" id="tPass" placeholder="Ваш пароль" maxlength="255" class="form-control" />
                        <input type="submit" value="войти" maxlength="255" class="btn btn-sm btn-success form-control" onclick="onLogin(); return false" />
                        <script>
                            $("#cWrong").hide();
                            $("#tLogin").focus();
                            var r = window.localStorage.getItem("mNF24User");
                            if (r != null) {
                                $("#tLogin").val(r);
                                $("#tPass").focus();
                            }
                            r = window.localStorage.getItem("mNF24Pass");
                            if (r != null) {
                                $("#tPass").val(r);
                                $("#tPass").focus();
                            }

                        </script>
                    </div>
                </div>
            </div>

        </div>
        <div id="MainForm" style="display: none">
            <div id="LBox" class="MainBox" style="width: 300px">
                <select id="prSel" class="form-control">
                    <option value="-1" selected="">Выбор программы</option>
                </select>
                <div id="NewsContainer">
                    <div id="planNews">
                        <div class="newsGroupCaption">планируемые</div>
                        <div id="planNewsBox" class="newsGroupContent"></div>
                    </div>
                    <div id="currNews">
                        <div class="newsGroupCaption">сегодня</div>
                        <div id="currNewsBox" class="newsGroupContent"></div>
                    </div>
                    <div id="pastNews">
                        <div class="newsGroupCaption">прошедшие</div>
                        <div id="pastNewsBox" class="newsGroupContent"></div>
                    </div>
                </div>
            </div>
            <div id="RBox" class="MainBox" style="width: calc(100% - 300px - 10px); float: right">
                <div id="RBoxHeader">

                    <input id="bRefresh" type="button" value="обновить" class="btn btn-success btn-xs" onclick="refreshBlocks()" />
                    <input type="button" id="bCollapsed" value="показать все тексты" class="btn btn-default btn-xs" onclick="collapsed = !collapsed; expandBlocks();" />

                </div>
                <div id="BlocksBox">
                    <div id="BlockID" class="blockItem" onclick="collapseBlock(this.id)">
                        <div class="blockItemType" style="background-color:lightgreen">
                            видео
                        </div>
                        <div class="blockItemName" style="">
                            подводка к сюжету 1
                        </div>
                         <div class="blockItemChrono" style="">
                            00:15:20
                        </div>
                        <div class="blockItemText collapsed"  id="blockItemTextBlockID" >
                            text
                        </div>
                    </div>
                </div>
            </div>
        
        <div style="width:100%; border:1px solid #ddd; border-radius:4px; padding:10px;margin:20px auto; display:none">
            <span style="cursor:pointer; font-size:x-small" onclick=" showLogin()">Выйти</span>
        </div>
            </div>
    </form>
</body>
</html>
