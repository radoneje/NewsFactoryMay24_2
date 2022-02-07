$(document).ready(function () { $("#FileUploadContainer").hide(); });
/*(function addXhrProgressEvent($) {
    var originalXhr = $.ajaxSettings.xhr;
    $.ajaxSetup({
        progress: function () { console.log("standard progress callback"); },
        xhr: function () {
            var req = originalXhr(), that = this;
            if (req) {
                if (typeof req.addEventListener == "function") {
                    req.addEventListener("progress", function (evt) {
                        that.progress(evt);
                    }, false);
                }
            }
            return req;
        }
    });
})(jQuery);
*/


function AddFileToUpload(file, BlockId)
{
    log(file);
    log(BlockId);
    ///затычка перьми - удаления
  /*  $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serverRoot+ "/testservice.asmx/BlockAddFileNameOnly",
        data: JSON.stringify({ blockId: BlockId, filename: file.name }),
        dataType: "json",
        async: true,
    });

    return;*/
    /// конец удаления затычки перьми
    $("#FileUploadContainer").show();
    var bId = BlockId;
    BlockId +="N"+ $('.FileUploader').length;

    $("#FileUploadContainer").append(GreateUploadElement(file, BlockId));

    var reader = new FileReader();

    BlockId = BlockId;
    FileName = file.name;
    StartDate = new Date();
    //var self = this;
  
   // Getting the properties of file from file field
    var formData = new window.FormData();                  // Creating object of FormData class
    formData.append("file", file); // Appending parameter named file with properties of file_field to form_data
    formData.append("BlockId", bId);
    formData.append("ContainerId", BlockId);
    var StartTime=new Date();
   
    $.ajax({

        url: serverRoot+'handlers/FileUpload.ashx?BlockId=' + bId + '&ContainerId=' + BlockId + '&FileName=' + encodeURIComponent(file.name),
        data: file,//formData,
        processData: false,
        contentType: false,
        context: { BlockId: BlockId, FileName: file.name, StartDate: StartTime, FileName:file.name },
        type: 'POST',
        success: function (data) {
            UploadFileSuccess(data);
            
        },
        error: function (xhr, ajaxOptions, thrownError) {
          //  debugger;
            UploadFileError(bId, BlockId, FileName);

        },
        xhr: function(){
            // get the native XmlHttpRequest object
            var xhr = $.ajaxSettings.xhr();
           
        
            xhr.upload.onprogress = function (evt) {        
                var percentComplete = parseInt(evt.loaded / evt.total * 100);
                if ((evt.loaded / evt.total * 100) - $('#FileUploaderProgress' + BlockId).attr("percentComplete") > 1) {
                    var d = new Date();              
                    var msFromStart = d.getTime() - StartDate.getTime();              
                    var TimeFromStart = msToTime(parseInt((evt.total - evt.loaded) / (evt.loaded / msFromStart)));
                    if (FileName.length > 35)
                        FileName = FileName.substring(0, 32) + "...";           
                    $('#FileUploaderProgress' + BlockId).css("width", percentComplete + "%");           
                    $("#FileUploaderTitle" + BlockId).html("<h6><small>" + FileName + ", : " + percentComplete + "% -" + TimeFromStart + "</h6></small>");
                }
              
            };
           
            return xhr ;
        } ,
        
    });

}
function UploadFileSuccess(data) {
    data = JSON.parse(data);
    $("#FileUploaderProgress" + data.ContainerId).addClass("progress-bar-success")
    setTimeout(function () {
        $("#FileUploader" + data.ContainerId).remove();
    }, 4000);
    ShowAlert(langTable['AlertUploadFileComplited']);
    if ($("#BlockEditIframe").length > 0) {
        document.getElementById("BlockEditIframe").contentWindow.ReloadMedia();
    }
}
function UploadFileError(BlockId, ContainerId, FileName)
{

    if (FileName.length > 35)
        FileName = that.context.FileName.substring(0, 32) + "...";

    $("#FileUploaderTitle" + ContainerId).html("<h6><small>" + FileName + ": Ошибка  <i class='glyphicon glyphicon-remove' style='cursor:pointer' onclick='$(\"#FileUploader" + ContainerId + "\").remove();'</i></small></h6>");
    $("#FileUploaderProgress" + ContainerId).addClass("progress-bar-danger");
    $('#FileUploaderProgress' + ContainerId).css("width", 50 + "%");

    showWarning(langTable['WarningErrorFileUpload']);
}


function GreateUploadElement(file, BlockId) {
    return '<div id="FileUploader' + BlockId + '" class ="FileUploader">\
<div id="FileUploaderTitle' + BlockId + '" style="height: 5px; margin-bottom: 10px;"></div>\
<div class="progress" style="height: 5px; margin-bottom: 2px;">\
\
  <div id="FileUploaderProgress' + BlockId + '" class="progress-bar" role="progressbar" percentComplete="-1" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; height=5px;">\
   \
  </div>\
</div></div>';
}

///////////////////

function FileUpload(ContainerId, UrlToUpload) {
    if($("#"+ContainerId).length>0)
    {
        $("#"+ContainerId).children().remove();
    }
    
    this.ContainerId = ContainerId;
    this.UrlToUpload = UrlToUpload;
    this.stack =  [];
    this.addToUpload = function (files, BlockId) {
        
        for (var i = 0; i < files.length; i++) {
            var item = { file: files[i], Id: parseInt(Math.random() * 10000000) };
            this.stack[this.stack.length]=(item);
        }      
       

        $("#" + this.ContainerId).html("");   
        $("#" + this.ContainerId).show();
        
        for (var i = 0; i < this.stack.length; i++ )
        {
            var html = '\<div id="FileIploaderItem' + this.stack[i].Id + '">\
           <div id="FileUploaderTitle' + this.stack[i].Id + '" style="height: 10px; margin-bottom: 10px;">' + this.stack[i].file.name + '</div>\
              <div class="progress" style="height: 10px; margin-bottom: 2px;">\
                 <div id="FileUploaderProgress' + this.stack[i].Id + '" class="progress-bar" role="progressbar" percentComplete="-1" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; height:10px;">\
           </div>\
                  </div></div>\
            ';
            $("#" + this.ContainerId).append(html);
            if (this.stack.length <= files.length)
            {
                this.UploadFile(this.stack[0]);
            }
        }
        
    };////////addToUpload
    this.UploadFile = function (st) {
        this.StartTime = new Date();
        var that = this;
        $.ajax({
            url: this.UrlToUpload + '/' + encodeURIComponent(st.file.name),
            data: st.file,//formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                that.UploadFileSuccess(data);

            },
            error: function (data) {
            //    debugger;
                that.UploadFileFailed(data);

            },
            xhr: function () {
                
                var xhr = jQuery.ajaxSettings.xhr();
                if (xhr.upload) {
                    xhr.upload.addEventListener('progress', function (event) {
                        var percent = 0;
                        var position = event.loaded || event.position;
                        var total = event.total;
                        if (event.lengthComputable) {
                            percent = Math.ceil(position / total * 100);
                        }
                        that.UploadProgress(event, position, total, percent);
                        //options.uploadProgress(event, position, total, percent);
                    }, false);
                }
                return xhr;
            },

        });///////ajax
        this.UploadFileSuccess = function (data) {

            //$("#FileUploaderTitle"+st.Id).html("<h6>" + "Готово: " + st.file.name + "</h6>");
            $("#FileUploaderProgress" + st.Id).addClass("progress-bar-success");
            this.stack.shift();
            if (this.stack.length > 0)
                this.UploadFile(this.stack[0])
            setTimeout($("#FileIploaderItem" + st.Id).remove(), 4000)
        };// UploadFileSuccess
        this.UploadProgress = function (event, position, total, percent) {

            $('#FileUploaderProgress' + st.Id).css("width", percent + "%");
            var d = new Date();
            var msFromStart = d.getTime() - this.StartTime.getTime();
            var TimeFromStart = msToTime(parseInt((total - position) / (position / msFromStart)));
            $("#FileUploaderTitle" + st.Id).html("<h6>" + "" + st.file.name + " " + percent + "%   -" + TimeFromStart + "</h6>");

            // setTimeout(function () { $("#FileIploaderItem" + st.Id).remove(); }, 10000);
            

        };// UploadFileProgress
        this.UploadFileFailed = function (data) {

            console.log("before"+this.stack);
            if (this.stack.length > 1) {
                var a = this.stack[0];
                this.stack.shift();
                this.stack.push(a);
            }
            console.log("after" + +this.stack);

            if (this.stack.length > 0)
                this.UploadFile(this.stack[0])
            setTimeout(this.UploadTimeout, 1000);

            $("#FileUploaderTitle" + st.Id).html("<h6><small>" + ": Ошибка  </small></h6>");
            $("#FileUploaderProgress" + st.Id).addClass("progress-bar-danger");
            $('#FileUploaderProgress' + st.Id).css("width", 100 + "%");

        };// UploadFileFailed
    }// UploadFile

        this.UploadTimeout=function()
        {
            
        }
        
        

        
   

    
}
var FU;
$(document).ready(function () {
    //FU = new FileUpload("FileUploader", "FileUpload");
    //FU = new FileChankedUpload("FileUploader", "FileUpload");
    FU = new UploadQuery("FileUploader", "FileUpload");
});


function RequestToUpload(FileCtrl, BlockId) {
    if ($(FileCtrl)[0].files.lenght < 1)
        return;
    
   
    FU.AddFiles($(FileCtrl)[0].files, guid(), BlockId);
    return;
}
function BlockStertUploadFiles(files, BlockId) {
    $("#FileUploadContainer").show();
   
    FU.AddFiles(files);
    return;
}
/////////////////////////////////////////////////
window.BlobBuilder = window.MozBlobBuilder || window.WebKitBlobBuilder ||
                     window.BlobBuilder;


////////////*/
function UploadQuery(ContainerId, UrlToUpload) {
    if ($("#" + ContainerId).length > 0) {
        $("#" + ContainerId).children().remove();
    }

    this.ContainerId = ContainerId;
    this.UrlToUpload = UrlToUpload;
    
    this.query = [];
    var that = this;
    this.ActiveQueryes = 0;
    this.MAX_QUERYES = 8; 
    this.BYTES_PER_CHUNK = 1024 * 1024; // 1MB chunk sizes.
    this.FileProgress={};
    

    this.AddFiles=function(files, FolderGuid, BlockId)
    {
        //log("BlockId->>", BlockId);
        if (typeof(FolderGuid)=='undefended')
        {FolderGuid = guid();}
        
      //  console.log(FolderGuid);
        for (var i = 0; i < files.length; i++) {
            var FileItem = { file: files[i], FileId: guid(), FolderGuid: FolderGuid, FileInFolderNumber: i, FilesInFolderCount: files.length, BlockId:BlockId };

            this.AddChunksToQuery(FileItem);
            this.FileProgress[FileItem.FileId] = 0;
        }
        this.ReloadControls();
       
    }//////AddFiles
    this.AddChunksToQuery = function (FileItem) {
        
        const SIZE = FileItem.file.size;
        var start = 0;
        var end = this.BYTES_PER_CHUNK;
        var i = 0;
        while (start < SIZE) {

            var chunk;
            if (typeof FileItem.file.slice === 'function') {
                chunk = FileItem.file.slice(start, end);
            } else if (typeof FileItem.file.webkitSlice === 'function') {
                chunk = FileItem.file.webkitSlice(start, end);
            }
            //запоняем структуру
            var queryItem = { FolderGuid: FileItem.FolderGuid, FileInFolderNumber: FileItem.FileInFolderNumber, FilesInFolderCount: FileItem.FilesInFolderCount, file: FileItem.file, FileId: FileItem.FileId, Chunk: chunk, ChunkBegin: start, ChunkEnd: start + chunk.size, ChunkNumber: i, BytesSend: 0, IsActive: false, IsComplite: false, StartTime: new Date(), BytesUpload: 0 , BlockId:FileItem.BlockId};
            //console.log(queryItem);
            this.query[this.query.length] = queryItem;
            console.log(queryItem);
            i++;
            start = end;
            end = start + this.BYTES_PER_CHUNK;
        }
        this.Query();
    }// AddChunksToQuery
    this.ReloadControls = function () {
        
        for (var i = 0; i < this.query.length; i++) {
            if ($("#FileUploaderItem" + this.query[i].FileId).length == 0) {
                $("#" + this.ContainerId).append(this.CreateFileUploaderItem(this.query[i]));
                
            }
        }
    }/// reload controls
    this.CreateFileUploaderItem = function (Elem) {
        var html = '\<div id="FileUploaderItem' + Elem.FileId + '">\
           <div id="FileUploaderTitle' + Elem.FileId  + '" style="height: 10px; margin-bottom: 10px;"><small>'+ Elem.file.name  + '<small></div>\
              <div class="progress" style="height: 10px; margin-bottom: 2px;">\
                 <div id="FileUploaderProgress' + Elem.FileId + '" class="progress-bar" role="progressbar" percentComplete="-1" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; height:10px;">\
           </div>\
                  </div></div>\
            ';
        return html;
    }
    this.Query= function () {

        if (this.query.length > 0  ) {
            
                for (var i = 0; i < this.query.length && this.ActiveQueryes < this.MAX_QUERYES; i++)
                {
                    if(!this.query[i].IsActive)
                    {
                       
                        this.UploadChunk(this.query[i]);

                    }
                }

        } ///if (this.query.length > 0  )
    }//// this.Query= function ()
    this.UploadChunk = function (ChankItem, asError) {
  
        if (!asError)
            that.ActiveQueryes++;
        ChankItem.StartDate = new Date();
          ///{FolderGuid}/{FolderCount}/{FileNumber}/{FileGuid}/
        var xhr = new XMLHttpRequest();
        xhr.open('POST', this.UrlToUpload + "/" + ChankItem.FolderGuid + "/" + ChankItem.FilesInFolderCount + "/" + ChankItem.FileInFolderNumber + "/" + ChankItem.FileId + "/" + ChankItem.ChunkNumber, true);
        xhr.setRequestHeader('Content-Range', 'bytes ' + ChankItem.ChunkBegin + '-' + ChankItem.ChunkEnd + '/' + ChankItem.file.size);
        xhr.setRequestHeader('Origin-Filename', encodeURIComponent(ChankItem.file.name));
        xhr.setRequestHeader('Origin-BlockId', encodeURIComponent(ChankItem.BlockId));
        xhr.send(ChankItem.Chunk);
        xhr.onloadend = function (e) { /*console.log("loadend"); */};
        xhr.onerror = function (e) {
            console.warn(ChankItem.file.name + " error send chunk " + e.message + that.ActiveQueryes); $("#FileUploaderProgress" + ChankItem.FileId).addClass("progress-bar-danger");
            setTimeout(that.UploadChunk(ChankItem,true), 500)
        };
        xhr.onloadstart = function (e) {  }
        xhr.onload = function (e) {  
            e.preventDefault();
            if (JSON.parse(e.srcElement.responseText).status==false) {
                   console.warn(ChankItem.file.name + " error send chunk " + ChankItem.ChunkNumber + "- server Error: " + JSON.parse(e.srcElement.responseText).Message); $("#FileUploaderProgress" + ChankItem.FileId).addClass("progress-bar-danger");
                   setTimeout(that.UploadChunk(ChankItem, true), 10);
                return false;
            }
            if (JSON.parse(e.srcElement.responseText).bytes != ChankItem.Chunk.size) {
                console.warn(ChankItem.file.name + " error send chunk no correct server responce" + that.ActiveQueryes); $("#FileUploaderProgress" + ChankItem.FileId).addClass("progress-bar-danger");
                setTimeout(that.UploadChunk(ChankItem, true), 1000);
                return false;
            }

            //console.log("ChunkCompl message send file Id:" + ChankItem.FileId + " chunkId:" + ChankItem.ChunkNumber + " ActiveQueries:" + that.ActiveQueryes);
            document.addEventListener("ChunkCompl", that.ChunkComplHandler, false);
            var event = new CustomEvent("ChunkCompl", { detail: { Item: ChankItem }, bubbles: true, cancelable: true });
            document.dispatchEvent(event);
        };
        //xhr.onprogress = function (e) {
           //if (e.lengthComputable) {
                
           // }
       // }
    }
    this.ChunkComplHandler = function (e) {
        that.ActiveQueryes--;
           for(var i=0; i<that.query.length; i++)//////// убираем лишнее из очереди и запускаем еще раз просмотр очередей
        {
            if(that.query[i]===e.detail.Item)
            {
                that.query.splice(i, 1);
                    break;
            }
        }
           that.Query();//////// убираем лишнее из очереди и запускаем еще раз просмотр очередей
           //console.log("CHUNK complite message receivedfile Id:" + e.detail.Item.FileId + " chunkId:" + e.detail.Item.ChunkNumber + " ActiveQueries:" + that.ActiveQueryes);
           that.FileProgress[e.detail.Item.FileId] += that.BYTES_PER_CHUNK;
           that.UpdateProgressBar(e.detail.Item);
           var find = false;
        for (var i = 0; i < that.query.length ; i++)//////// проверяем окончание файла
                if (that.query[i].FileId == e.detail.Item.FileId) 
                  find = true;
        if (find == false || that.query.length==0) {//это полностью загруженный файл
                //console.log("file complite message send file Id:" + e.detail.Item.FileId + " chunkId:" + e.detail.Item.ChunkNumber + " ActiveQueries:" + that.ActiveQueryes);      
                document.addEventListener("FileCompl", that.FileComplHandler, false);
                var event = new CustomEvent("FileCompl", { detail: { Item: e.detail.Item }, bubbles: true, cancelable: true });
                document.dispatchEvent(event);
        }
        

    }
    this.FileComplHandler=function(e)
    {
        $("#FileUploaderItem" + e.detail.Item.FileId).remove();
        
        //console.log("file complite message receivedfile Id:" + e.detail.Item.FileId + " chunkId:" + e.detail.Item.ChunkNumber + " ActiveQueries:" + that.ActiveQueryes);

    }
    this.UpdateProgressBar = function (Item) {
        var send = that.FileProgress[Item.FileId];
        var total = Item.file.size;
        var percent = parseInt((send / total) * 100);
        if (percent > 100)
            percent = 100;
        $('#FileUploaderProgress' + Item.FileId).css("width", percent + "%");
        $("#FileUploaderProgress" + Item.FileId).removeClass("progress-bar-danger");

        var d = new Date();
        var msFromStart = d.getTime() - Item.StartTime.getTime();
        var TimeFromStart = msToTime(parseInt((total - send) / (send / msFromStart)));

        var filename=Item.file.name 
        if(filename.length>25)
            filename=filename.substring(0,23)+"...";
        $('#FileUploaderTitle' + Item.FileId).html("<small>" + filename + " " + percent + "%, -" + TimeFromStart + "</small>");
    }
    

}

////////////

function UploadBlockEditFiles(files, BlockId)
{
  //  debugger;
    FU.AddFiles(files, guid(), BlockId);
    return;
}