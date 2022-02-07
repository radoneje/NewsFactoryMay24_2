<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplication2.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h4>
                Проверка скорости чтения
            </h4>
        </div>
        <div id="dText" class="panel-body">
            Пока, как рассказывает РБК Дерлятка, у проекта 12 партнеров, предлагающих свои товары и услуги в обмен на Sweatcoin, например производитель обуви Vivobarefoot. «Мы только начинаем, и поиск партнеров не самая сложная проблема. У нас очень таргетированная аудитория», — говорит предприниматель. Основатели проекта рассчитывают, что в дальнейшем и сами пользователи смогут предлагать собственные товары и услуги: «например, диетолог может указать, что его услуги стоят 20 swc за сеанс». В лондонском магазине Vivobarefoot стоимость кроссовок начинается от 85 фунтов. Пользователи Sweatcoin смогут за 250 swc купить любую пару, но это с учетом 80% скидки, говорит Дерлятка.

Подробнее на РБК:

        </div>
        <script>
            var timer;
            var intervalID;
        </script>
        <div style="width:100%; text-align:center">
        <input type="button" id="bStart" class="btn btn-success" value="Start" onclick="timer = new Date(); $(this).hide(); $('#bStop').show(); $('#Res').html('0'); intervalID = setInterval(function () { var i = parseInt($('#Res').html()); i++; $('#Res').html(i) }, 1000)"/>
        <input type="button" style="display:none" id="bStop" class="btn btn-danger" value="Stop" onclick="clearInterval(intervalID);timer2 = new Date(); var diff = timer2.getTime() - timer.getTime(); var seconds = Math.floor(diff / (1000)); $(this).hide(); $('#bStart').show(); $('#Res').html('Скорость чтения: ' + parseInt($('#dText').html().length / seconds) + ' символов в секунду');"/>
    </div>
        
    <div id="Res" class="panel-footer" ></div>
        </div>
    <script>
        //var intervalID = setInterval(...)
        //clearInterval(intervalID)


    </script>
</asp:Content>