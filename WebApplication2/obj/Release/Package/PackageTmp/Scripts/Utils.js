const div = "<div></div>";
const span = "<span></span>";

function newDiv(id)
{
    return $(div).attr("id",id);
}

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}
function CreateFullScreenDiv(sId) {
    var iDiv = document.createElement('div');
    iDiv.id = sId;
    iDiv.className = 'allScreenLayer';
    iDiv.setAttribute("class", 'allScreenLayer');
    iDiv.setAttribute("id", sId);
    document.getElementsByTagName('body')[0].appendChild(iDiv);

    return iDiv;
}
function checkDef(obj)
{
    return (!(typeof obj === "undefined"));
}
jQuery.fn.center = function () {
    this.css(f, "absolute");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                $(window).scrollLeft()) + "px");
    return this;
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + s4() + s4() + s4() + s4() + s4() + s4();
}
var docCookies = {
    getItem: function (sKey) {
        if (!sKey) { return null; }
        return decodeURIComponent(document.cookie.replace(new RegExp("(?:(?:^|.*;)\\s*" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*([^;]*).*$)|^.*$"), "$1")) || null;
    },
    setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
        if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/i.test(sKey)) { return false; }
        var sExpires = "";
        if (vEnd) {
            switch (vEnd.constructor) {
                case Number:
                    sExpires = vEnd === Infinity ? "; expires=Fri, 31 Dec 9999 23:59:59 GMT" : "; max-age=" + vEnd;
                    break;
                case String:
                    sExpires = "; expires=" + vEnd;
                    break;
                case Date:
                    sExpires = "; expires=" + vEnd.toUTCString();
                    break;
            }
        }
        document.cookie = encodeURIComponent(sKey) + "=" + encodeURIComponent(sValue) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
        return true;
    },
    removeItem: function (sKey, sPath, sDomain) {
        if (!this.hasItem(sKey)) { return false; }
        document.cookie = encodeURIComponent(sKey) + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "");
        return true;
    },
    hasItem: function (sKey) {
        if (!sKey) { return false; }
        return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie);
    },
    keys: function () {
        var aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nLen = aKeys.length, nIdx = 0; nIdx < nLen; nIdx++) { aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]); }
        return aKeys;
    }
};
function replacerVideoJSONTag(str, p1) {
  //  console.log("it is video tag");
  //     console.log(str);
 //   console.log(p1);
   
    var obj = JSON.parse(p1);
   //  console.log(obj);
    var ret;
    if (obj.mediaType == 2) {
        ret = "<img class='editorVideoImage' src='" + serverRoot + "handlers/GetBlockImage.ashx?MediaId=" +
            +obj.mediaId + "' markIn='" + obj.markIn + "'" +
            "markOut='" + obj.markOut + "' MediaId='" + obj.mediaId + "'" +
            " mediaType='2' onclick='clickVideoImgInEditor(this)'/>";
    }
    else if (obj.mediaType == 1) {
        ret = "<img class='editorImageImage' src='" + serverRoot + "handlers/GetBlockImage.ashx?MediaId=" +
         +obj.mediaId + "' markIn='0'" +
         "markOut='0' mediaId='" + obj.mediaId + "'" +
         "mediaType='1' onclick='ShowPicture(\"" + obj.mediaId + "\")'/>";
    } else {
        ret = "<img class='editorDocumentImage' src='" + serverRoot + "handlers/GetBlockImage.ashx?MediaId=" +
        +obj.mediaId + "' markIn='0'" +
        "markOut='0' mediaId='" + obj.mediaId + "'" +
        "mediaType='0' onclick='BEDownloadDocument(\"" + obj.mediaId + "\")'/>";
    }
   // console.log(ret);
    return ret;

}
 function ShowDisabledMessage(messageText, messageTitle)
        {
            var iDiv=CreateFullScreenDiv("EditorAlertDiv")
            iDiv.innerHTML = CreateBlockAlertElements(messageText, messageTitle);
            //document.getElementById("EditorAlertDiv").appendChild(iframe);
            
            window.scrollTo(0, 0);

        }
function CreateBlockAlertElements(messageText, messageTitle)
        {
            var ret = '<div id="BlockEditorAlertMessage" class="alert alert-warning" role="alert">';
            if(checkDef(messageTitle))
            {
                ret = ret + "<h4>" + messageTitle + "</h4>";
            }
            if (checkDef(messageText))
            {
                ret=ret+'<p>'+messageText+'</p>'
            }
            ret = ret + '</div>';
            //if (checkDef(window.parent)) {
                ret = ret + "<button id='BlockEditorAlertCloseButton' type='submit' style='width: 100;' class='btn btn-success navbar-btn' onclick='window.parent.CloseEditor();'>закрыть редактор</button>";
                $("#BlockEditorAlertCloseButton").center();
            //}
            $("#BlockEditorAlertMessage").center();
            return ret;

        }

        $.datepicker.regional['ru'] = {
            closeText: 'Закрыть',
            prevText: '<Пред',
            nextText: 'След>',
            currentText: 'Сегодня',
            monthNames: ['01', '02', '03', '04', '05', '06',
            '07', '08', '09', '10', '11', '12'],
            monthNamesShort: ['01', '02', '03', '04', '05', '06',
            '07', '08', '09', '10', '11', '12'],
            dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
            dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
            dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            weekHeader: 'Не',
            dateFormat: 'dd.mm.yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['ru']);
  
        if (!(typeof ($.timepicker) == 'undefined'))
            {
        $.timepicker.regional['ru'] = {
            timeOnlyTitle: 'Выберите время',
            timeText: 'Время',
            hourText: 'Часы',
            minuteText: 'Минуты',
            secondText: 'Секунды',
            millisecText: 'Миллисекунды',
            timezoneText: 'Часовой пояс',
            currentText: 'Сейчас',
            closeText: 'Закрыть',
            timeFormat: 'HH:mm',
            amNames: ['AM', 'A'],
            pmNames: ['PM', 'P'],
            isRTL: false
        };
        }
        if (!(typeof ($.timepicker) == 'undefined')) {
            $.timepicker.setDefaults($.timepicker.regional['ru']);
        }
        if (!(typeof ($.datepicker) == 'undefined')) {
            $.datepicker.setDefaults($.datepicker.regional['ru']);
        }

        function checkTimes(ctrl)
        {
            return checktimeFromText($(ctrl).val())
        }
        function checktimeFromText(value){
            //alert(value);
           // debugger;
            var regex = new RegExp(/^(\d{2}):?(\d{2}):?(\d{2})$/);
            //  console.log(ctrl.value);
            var match = regex.exec(value);
            try {
                if (match!= null && (match[1] && match[2] && match[3])) {

                    var ret = (parseInt(match[1]) * 60 * 60) + (parseInt(match[2]) * 60) + parseInt(match[3]);

                    return ret;
                }
             
            }
            catch (exep) {
                return -1;
               
            }

            regex = new RegExp(/^(\d{1,2}):?(\d\d)$/);
            match = regex.exec(value);
            try {
                if (match != null && (match[1] && match[2])) {

                    var ret =  (parseInt(match[1]) * 60) + parseInt(match[2]);

                    return ret;
                }
              
            }
            catch (exep) {
                return -1;
               
            }

            regex = new RegExp(/^(\d{1,2})$/);
            match = regex.exec(value);
            try {
                if (match != null && (match[1])) {

                    var ret = (parseInt(match[1]));

                    return ret;
                }

            }
            catch (exep) {
                return -1;

            }

            

            



            return -1;

            return "";
        }
        /*короткая функция логирования*/      function log(data) // короткая функция логирования
        { console.log(data); }

        function timeToSeconds(hhmmss) {
            var a = hhmmss.split(':'); // split it at the colons

            // minutes are worth 60 seconds. Hours are worth 60 minutes.
            return (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);

        }
function msToTime(s) {
            var ms = s % 1000;
            s = (s - ms) / 1000;
            var secs = s % 60;
            
            s = (s - secs) / 60;
            var mins = s % 60;
            var hrs = (s - mins) / 60;

            if (secs < 10) secs = "0" + secs;
            if (mins < 10) mins = "0" + mins;
            if (hrs < 10) hrs = "0" + hrs;

            return hrs + ':' + mins + ':' + secs;
}
function msToTimeWidthMs(s) {
    var ms = s % 1000;
    s = (s - ms) / 1000;
    var secs = s % 60;

    s = (s - secs) / 60;
    var mins = s % 60;
    var hrs = (s - mins) / 60;

    //if (ms < 10) secs = "0" + secs;
    if (secs < 10) secs = "0" + secs;
    if (mins < 10) mins = "0" + mins;
    if (hrs < 10) hrs = "0" + hrs;

    return hrs + ':' + mins + ':' + secs+'.'+ms;
}
function jsonDateToDate(jsonDate)
{
  
    var matc = jsonDate.match(/\d+/)[0];
    var dt = new Date(matc);
    return dt;
}
function dateToDateString(date)
{
    return date.getDate() + "." + (date.getMonth() + 1) + '.' + '.' + date.getFullYear();
}
function stringToDate(dateString) {
    return new Date(dateString);
}

function SortDivs(ParentElement, IntegerAttributeName) // сортировка элементов в родительском контейнере
        {
            /// передаем родительский элемент и обязательное название числового параметра для сортировки
            for (var i = $($(ParentElement).children()).length-1; i >= 0; i--) {
                var CurrElement = $($(ParentElement).children()).get(i);
                var CurrValue = $(CurrElement).attr(IntegerAttributeName);
                var prevKey = i-1;

                while (prevKey >=0) {
                    var PrevElement = $($(ParentElement).children()).get(prevKey);
                    var PrevValue = $(PrevElement).attr(IntegerAttributeName);
                    if (parseInt(PrevValue) > parseInt(CurrValue)) {
                        $(PrevElement).insertAfter($(CurrElement));
                    }
                    prevKey--;
                }
            }
        }
        function TimeToTimecode(t) {
           
            var ms = parseInt((t - parseInt(t)) * 100)
            if (ms < 10)
                ms = "0" + ms;
           
             return (msToTime(t * 1000) + "." + ms);
        }
        function TimecodeToMs(str)
        {
            //12:34:56.78
            var h = str.substring(0, 2);
            var m = str.substring(3, 5);
            var s = str.substring(6, 8);
            var ms = str.substring(9, 11);
            return parseFloat(h*60*60 + m*60 + s +"."+ ms);

        }
        function RemoveHTMLTag(text) {
            var div = document.createElement("div");
            div.innerHTML = text;
            return div.textContent || div.innerText || "";

        }
////////////////////
        function insertTextAtCursor(text) {
            var sel, range, html;
            if (window.getSelection) {
                sel = window.getSelection();
                if (sel.getRangeAt && sel.rangeCount) {
                    range = sel.getRangeAt(0);
                    range.deleteContents();
                    range.insertNode(document.createTextNode(text));
                }
            } else if (document.selection && document.selection.createRange) {
                document.selection.createRange().text = text;
            }
        }
        function saveSelection() {
            if (window.getSelection) {
                sel = window.getSelection();
                if (sel.getRangeAt && sel.rangeCount) {
                    return sel.getRangeAt(0);
                }
            } else if (document.selection && document.selection.createRange) {
                return document.selection.createRange();
            }
            return null;
        }
        function restoreSelection(range) {
            if (range) {
                if (window.getSelection) {
                    sel = window.getSelection();
                    sel.removeAllRanges();
                    sel.addRange(range);
                } else if (document.selection && range.select) {
                    range.select();
                }
            }
        }
        function pasteHtmlAtCaret(html) {
            var sel, range;
            if (window.getSelection) {
                // IE9 and non-IE
                sel = window.getSelection();
                if (sel.getRangeAt && sel.rangeCount) {
                    range = sel.getRangeAt(0);
                    range.deleteContents();

                    // Range.createContextualFragment() would be useful here but is
                    // only relatively recently standardized and is not supported in
                    // some browsers (IE9, for one)
                    var el = document.createElement("div");
                    el.innerHTML = html;
                    var frag = document.createDocumentFragment(), node, lastNode;
                    while ((node = el.firstChild)) {
                        lastNode = frag.appendChild(node);
                    }
                    range.insertNode(frag);

                    // Preserve the selection
                    if (lastNode) {
                        range = range.cloneRange();
                        range.setStartAfter(lastNode);
                        range.collapse(true);
                        sel.removeAllRanges();
                        sel.addRange(range);
                    }
                }
            } else if (document.selection && document.selection.type != "Control") {
                // IE < 9
                document.selection.createRange().pasteHTML(html);
            }
        }  // это вставка в редактируемые дивы
        var saveSelection, restoreSelection;

        if (window.getSelection && document.createRange) {
            saveSelection = function (containerEl) {
                var doc = containerEl.ownerDocument, win = doc.defaultView;
                var range = win.getSelection().getRangeAt(0);
                var preSelectionRange = range.cloneRange();
                preSelectionRange.selectNodeContents(containerEl);
                preSelectionRange.setEnd(range.startContainer, range.startOffset);
                var start = preSelectionRange.toString().length;

                return {
                    start: start,
                    end: start + range.toString().length
                }
            };

            restoreSelection = function (containerEl, savedSel) {
                var doc = containerEl.ownerDocument, win = doc.defaultView;
                var charIndex = 0, range = doc.createRange();
                range.setStart(containerEl, 0);
                range.collapse(true);
                var nodeStack = [containerEl], node, foundStart = false, stop = false;

                while (!stop && (node = nodeStack.pop())) {
                    if (node.nodeType == 3) {
                        var nextCharIndex = charIndex + node.length;
                        if (!foundStart && savedSel.start >= charIndex && savedSel.start <= nextCharIndex) {
                            range.setStart(node, savedSel.start - charIndex);
                            foundStart = true;
                        }
                        if (foundStart && savedSel.end >= charIndex && savedSel.end <= nextCharIndex) {
                            range.setEnd(node, savedSel.end - charIndex);
                            stop = true;
                        }
                        charIndex = nextCharIndex;
                    } else {
                        var i = node.childNodes.length;
                        while (i--) {
                            nodeStack.push(node.childNodes[i]);
                        }
                    }
                }

                var sel = win.getSelection();
                sel.removeAllRanges();
                sel.addRange(range);
            }
        } else if (document.selection) {
            saveSelection = function (containerEl) {
                var doc = containerEl.ownerDocument, win = doc.defaultView || doc.parentWindow;
                var selectedTextRange = doc.selection.createRange();
                var preSelectionTextRange = doc.body.createTextRange();
                preSelectionTextRange.moveToElementText(containerEl);
                preSelectionTextRange.setEndPoint("EndToStart", selectedTextRange);
                var start = preSelectionTextRange.text.length;

                return {
                    start: start,
                    end: start + selectedTextRange.text.length
                }
            };

            restoreSelection = function (containerEl, savedSel) {
                var doc = containerEl.ownerDocument, win = doc.defaultView || doc.parentWindow;
                var textRange = doc.body.createTextRange();
                textRange.moveToElementText(containerEl);
                textRange.collapse(true);
                textRange.moveEnd("character", savedSel.end);
                textRange.moveStart("character", savedSel.start);
                textRange.select();
            };
        }
var BrowserDetect = {
            init: function () {
                this.browser = this.searchString(this.dataBrowser) || "Other";
                this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion) || "Unknown";
            },
            searchString: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var dataString = data[i].string;
                    this.versionSearchString = data[i].subString;

                    if (dataString.indexOf(data[i].subString) !== -1) {
                        return data[i].identity;
                    }
                }
            },
            searchVersion: function (dataString) {
                var index = dataString.indexOf(this.versionSearchString);
                if (index === -1) {
                    return;
                }

                var rv = dataString.indexOf("rv:");
                if (this.versionSearchString === "Trident" && rv !== -1) {
                    return parseFloat(dataString.substring(rv + 3));
                } else {
                    return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
                }
            },

 dataBrowser: [
                { string: navigator.userAgent, subString: "Chrome", identity: "Chrome" },
                { string: navigator.userAgent, subString: "MSIE", identity: "Explorer" },
                { string: navigator.userAgent, subString: "Trident", identity: "Explorer" },
                { string: navigator.userAgent, subString: "Firefox", identity: "Firefox" },
                { string: navigator.userAgent, subString: "Safari", identity: "Safari" },
                { string: navigator.userAgent, subString: "Opera", identity: "Opera" }
            ]

        };

  BrowserDetect.init();
  function DownloadFile(url) {
      document.getElementById('DownloadIFrame').src = url;
  }

  function ChangeLang(Id)
  {
      console.log("change lang "+ Id);
      langId = Id;
      localStorage.setItem("languageId", Id);
      $('#LangSelect>option:eq(' + Id + ')').prop('selected', true);
      
      console.log(serverRoot + "Resources/lang" + langId + ".js?a="+Math.random(1));
     // $.getScript(serverRoot + "Resources/lang" + langId + ".js?a=" + Math.random(1), function (a) {
      $.get(serverRoot + "Resources/lang" + langId + ".js?a=" + Math.random(1), function (a) {
          eval(a);
          
         
          $(".caption").each(function (index, elem) {
              if ($(elem).hasClass("caption-html")) {
                  $(elem).html(langTable[$(elem).attr('captionId')]);
              }
              if ($(elem).hasClass("caption-value")) {
                  $(elem).val(langTable[$(elem).attr('captionId')]);
              }
              if ($(elem).hasClass("caption-placeholder")) {
                  $(elem).attr('placeholder', langTable[$(elem).attr('captionId')]);
              }
          });
      });
  }
  $.fn.getFormData = function () {
      var $_this = this;
      var ret = {};
      $($_this).children("input").each(function (i,e) {
          var name = $(e).attr("name");
          if (typeof(name) != 'undefined')
                ret[name] = $(e).val();
      });
      
      return { serviceName: $($_this).attr("action"), params: ret };

  }
  function sendFormDataToService(serviceName, params, callback)
  {
      $.ajax(
      {
          type: "POST",
          contentType: "application/json;",
          url: serverRoot+ "testservice.asmx/"+ serviceName,
          data: JSON.stringify(params),
          dataType: "json",
          async: true,
          success:callback,
          error: function (dt) {
              console.warn("error");
              console.warn(dt);
          }

      });
  }
  $.fn.flipBackground = function (color) {
      var $_this = this;
      $($_this).css("background", color);
      setTimeout(function () {
          $($_this).css("background", "");
      }, 1000);
      return $_this;
  }
  $.fn.setId = function (id) {
      var $_this = this;
      $_this.attr("id", id);
      return $_this;
  }
  $.fn.blink = function () {
      var $_this = this;
      var elem = $(this);
      $(elem).fadeOut(200, function () {
          $(elem).fadeIn(200, function () {
              $(elem).fadeOut(200, function () {
                  $(elem).fadeIn(200, function () {
                      $(elem).fadeOut(200, function () {
                          $(elem).fadeIn(200, function () {
                          })
                      })
                  })
              })
          });
      });
      return $_this;
  }
  function eventFire(el, etype) {
      if (el.fireEvent) {
          el.fireEvent('on' + etype);
      } else {
          var evObj = document.createEvent('Events');
          evObj.initEvent(etype, true, false);
          el.dispatchEvent(evObj);
      }
  }
  function NFconfirm(msg, X, Y, param, successFuncCallBack) {
      $('body').append(
          $(div)
          .addClass("NfwConfirm")
          .css("left", "calc(" + X + "px - 50px)")
           .css("top", Y-20 + "px")
           .mouseleave(function (e) { $(".NfwConfirm").remove(); })
           .append($(div)
               .addClass("panel-heading")
               .html($("<div class='NfwConfirmMessage'>" + msg + "</div>")
               .append($('<div>X</div>')
                    .addClass("closeBtn")
                .click(function () {
                    $(".NfwConfirm").remove();
                    })
                )
                 .append($(div)
                 .addClass("panel-body")
                   .addClass("NfwConfirmButtons")
                  .append($("<input type='button' Value='Yes'/>")
                    .click(function () { successFuncCallBack(param); $(".NfwConfirm").remove(); })
                    .addClass("NfwConfirmBtn")
                    .addClass("btn btn-success btn-sm")
                 )
                 .append($("<input type='button' Value='No'/>")
                    .click(function () { $(".NfwConfirm").remove(); })
                    .addClass("NfwConfirmBtn")
                    .addClass("btn btn-danger btn-sm")
                    .focus()
                 )
                )
              )
           )
          );
      var elem = $(".NfwConfirm");
      var vpH = $(window).height() + $(window).scrollTop();
      var elemH = $(elem).height();

      var i = 0;
         
          while (i<50 && parseInt($(elem).offset().top + elemH) > parseInt(vpH)-10)
          {
              i++;
              $(elem).css("top", Y - 20 - 10*i);
          }


     
  }
  function formatTextNoVideoTag(txt) {
      while (txt.indexOf("NF::VIDEO::") > 0) {
          var st = txt.indexOf("NF::VIDEO::");
          var en = txt.indexOf("}", st);
          if (en < st) {
              txt = txt.replace("NF::VIDEO::", "");
          }
          else {
              var substr = txt.substr(st + 11, en - st + 1 - 11);
              txt = txt.substr(0, st) + replacerVideoJSONTag("", substr) + txt.substr(en + 1, txt.length - (en + 1));
              // console.log(substr);
          }
      }
      while (txt.indexOf("\n") >= 0) {
          txt = txt.replace("\n", "<br>");
      }
      txt=txt.replace(/NF::BOLDSTART/g, "").replace(/NF::BOLDEND/g, "")

      return txt;
  }

  $.fn.slowRemove = function (callBack) {
      var _this = this;
      $(_this).fadeOut(500, function () { $(_this).remove(); if (typeof (callBack) == "function") callBack(); });
      return _this;
  }
$.fn.captionHTML= function(captionId)
{
      var _this=this;
      $(_this)
      .addClass("caption")
      .addClass("caption-html")
      .attr("captionid", captionId)
      .html(langTable[captionId]);
     
      return _this;
  }
$.fn.loading50 = function () {
    var _this = this;

    $(_this).append($("<img src='"+serverRoot+"images/loading.gif' style='max-width:100%'/>")
        .css("width", 50 + "px")
        .addClass("loading")
        );
    return _this
}
var ajaxErr = function (where, message) {
    console.warn(where);
    console.warn(message);
}
function txtareaInsertAtCaret(areaId, text) {
    var txtarea = document.getElementById(areaId);
    if (!txtarea) { return; }

    var scrollPos = txtarea.scrollTop;
    var strPos = 0;
    var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
        "ff" : (document.selection ? "ie" : false));
    if (br == "ie") {
        txtarea.focus();
        var range = document.selection.createRange();
        range.moveStart('character', -txtarea.value.length);
        strPos = range.text.length;
    } else if (br == "ff") {
        strPos = txtarea.selectionStart;
    }

    var front = (txtarea.value).substring(0, strPos);
    var back = (txtarea.value).substring(strPos, txtarea.value.length);
    txtarea.value = front + text + back;
    strPos = strPos + text.length;
    if (br == "ie") {
        txtarea.focus();
        var ieRange = document.selection.createRange();
        ieRange.moveStart('character', -txtarea.value.length);
        ieRange.moveStart('character', strPos);
        ieRange.moveEnd('character', 0);
        ieRange.select();
    } else if (br == "ff") {
        txtarea.selectionStart = strPos;
        txtarea.selectionEnd = strPos;
        txtarea.focus();
    }

    txtarea.scrollTop = scrollPos;
}
function scrollToElement(elem, offset) {
    if (typeof (offset) == 'undefined')
        offset = 0;
    var pos = $(elem).offset().top;
    //   console.log(pos);
    if (pos <= 60)
        pos = pos - 60;

    $('html, body').animate({
        scrollTop: pos + offset
    }, 500);
}
$.fn.swipeX = function (retFunc) {
    var _this = this;
    var started = false;
    var leftButtonDown = false;
    var st = 0;
    var end = 0;

    $(_this).on("touchmove", function (ev) {
        if(st==null)
            st = ev.originalEvent.touches[0].pageX;
       
        end = ev.originalEvent.touches[0].pageX;

        if (end != null && st != null) {
           // processRotateInPixel(st - end);
            // ev.preventDefault();
        }


    });
    $(_this).on("touchstart ", function (ev) {
        // ev.preventDefault();

      
        st = null;
        end = null;
    });
    $(_this).on("touchend ", function (ev) {
        // ev.preventDefault();
        
        retFunc(_this, end-st );
        st = null;
        end = null;
    });

  /*  $(_this).mousedown(function (ev) {
        //    ev.preventDefault();
        st = null;
        end = null;
        started = true;
        console.log("start swipe");
        if (ev.which === 1) leftButtonDown = true;

    });

    $(_this).mousemove(function (ev) {
        //   ev.preventDefault();
        st += end;
        end = ev.pageX;
        console.log(st);
        if (end != null && st != null && leftButtonDown && (window.innerWidth/4)<(st+end)) {
            retFunc(st + end);
        }
    });

    $(_this).mouseup(function (ev) {
        //      ev.preventDefault();
        st = null;
        end = null;
        started = false;
        if (ev.which === 1) leftButtonDown = false;
    });*/


    return _this;
}
function setIframeMaxHeigth(id)
{
    
    var iFrameID = document.getElementById(id);
    if (iFrameID) {
        // here you can make the height, I delete it first, then I make it again
        iFrameID.height = "";
        iFrameID.height = iFrameID.contentWindow.document.body.scrollHeight + "px";
    }
    //console.log(e);
}
$.fn.autoHeight = function () {
    var _this = this;
    var resizeTextarea = function (el) {
        var offset = el.offsetHeight - el.clientHeight;
        jQuery(el).css('height', el.scrollHeight + offset);
    };
    jQuery(this).on('keyup input', function () { resizeTextarea(this); });
    return this;
}
function serv(name, callback, param) {
    if(typeof(param)=="undefined" || param==0)
        param = { id: 0 };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot + "testservice.asmx/" + name,
        data: JSON.stringify(param),
        dataType: "json",
        success: function (data) { callback(JSON.parse(data.d)) },
        error: function (e) { console.warn(name); console.warn(e); }

    });
}
function checkVisible(elem, threshold, mode) {
    threshold = threshold || 0;
    mode = mode || "visible";
    var rect=elem.getBoundingClientRect();
    var viewH = Math.max(document.documentElement.clientHeight, window.innerHeight);
    var above = rect.bottom - threshold >= 0;
    var below = rect.top - viewH + threshold >= 0;

    return mode ==="above" ? above: (mode=== "bellow"? below : ! above && ! below);


}
function NFpost(URL, data, ret)
{
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: URL,
        data: JSON.stringify(data),
        dataType: "json",
        success: ret,
        error: function (e) { console.warn("ERRR in POST"); console.warn(e) }
    });
}
$.fn.selectText = function () {
    var doc = document
        , element = this[0]
        , range, selection
        ;
    if (doc.body.createTextRange) {
        range = document.body.createTextRange();
        range.moveToElementText(element);
        range.select();
    } else if (window.getSelection) {
        selection = window.getSelection();
        range = document.createRange();
        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);
    }
};



        
        
