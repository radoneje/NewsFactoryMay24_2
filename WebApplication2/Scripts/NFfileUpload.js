$.fn.NfstatusBar = function () {
    var _ctrl = this;
    $(_ctrl).append($(div).addClass("NFfileUploadItemStatus"));
    $.fn.NfstatusBar.Percent=function(perc){
      
        $(_ctrl).find(".NFfileUploadItemStatus").css("width", perc + "%");
       
        $(_ctrl).parent().find(".NFfileUploadItemPrecent").html("<img src='/images/ajax-loader.gif' style='heught=1.2em'/>");
        return _ctrl;
    }
    return _ctrl;
}
$.fn.NfFileUpload = function () {

    
    var _ctrl = this;
    var _fileStack = new Array();

    const MAX_QUERYES = 8;
    const BYTES_PER_CHUNK = 1024 * 1024; // 1MB chunk sizes.

    var chunkStack = new Array();
    var ActiveQueryes = 0;

    var fileStack = [];
    var counterId = 0;

   var BlobBuilder = window.MozBlobBuilder || window.WebKitBlobBuilder ||
        window.BlobBuilder;

    uploadWorker();

    function uploadWorker() {

        if (fileStack.length == 0) {
            setTimeout(uploadWorker,  500);
            return;
        }

        var doing = fileStack.filter(function (item) { return item.started == true })
        var candidates = fileStack.filter(function (item) { return item.started == false });
        while (doing.length < 1 && candidates.length >0 )
        {
            console.log("doing", doing.length, candidates.length);
            
            if (candidates.length > 0) {
                var candidate = candidates[0];
                for (var i = 0; i < fileStack.length; i++) {
                    if (fileStack[i].id == candidate.id)
                        fileStack[i].started = true;
                }
               
                uploadProcess(candidate);

            }
            
            doing = fileStack.filter(function (item) { return item.started == true })
            candidates = fileStack.filter(function (item) { return item.started == false });
        }

        setTimeout(uploadWorker, 500);

    }
    function uploadProcess(item) {
        console.log("start upload file by chank", item.file);
        var file = item.file;
        var $div = item.$div;
       

        const chunkSize = 1024* 1024 * 100;//(100 MBt)

        sendFile(0, item.file.name, 0 ,0)

        function sendFile(start, filename, totalLoaded, taskId) {
            console.log("sendFile", start, file.size, taskId );
            if (start < file.size) {
                const chunk = file.slice(start, start + chunkSize );
                var xhr = new XMLHttpRequest();
                xhr.open("POST", serverRoot + "fileUploadChankSimple/" + item.blockId + "/" + item.i.toString() + "/", true);
                xhr.setRequestHeader("x-blockId", item.blockId);
                xhr.setRequestHeader("x-filename", encodeURIComponent(filename));
                xhr.setRequestHeader("x-firstChank", start == 0 ? 1 : 0);
                xhr.setRequestHeader("x-lastChank", ((start + chunkSize) >= file.size) ? 1 : 0);
                xhr.setRequestHeader("x-taskId", taskId);
                xhr.upload.addEventListener('progress', function (e) {
                
                    if (e.lengthComputable) {
                        item.$div.css("color", "#eee");          
                        item.$div.attr("complited", totalLoaded);
                        var percent = parseInt(parseFloat((totalLoaded+e.loaded) / file.size) * 100);                     
                        if (e.loaded == e.total) {          
                            totalLoaded += e.total;
                        }
                        item.$div.find(".NFfileUploadItemStatus").css("width", percent + "%");
                        item.$div.find(".NFfileUploadItemPrecent").html(percent + "%");
                    }
                });
                xhr.onreadystatechange = function (e) {
                    console.log({ cmd: "onreadystatechange" });
                    if (xhr.readyState === 4) {
                        if (xhr.status == 200) {
                            console.log({ cmd: "success chunk upload", flename:xhr.responseText });
                            var ret = JSON.parse(xhr.responseText)
                            sendFile(start + chunkSize, ret.fileName, totalLoaded, ret.taskId);
                        }
                        else {
                            console.warn({ cmd: "error file upload" });
                            item.$div.addClass("progressBarDanger");
                            item.$div.css("color", "red");
                            fileStack = fileStack.filter(function (a) { return a.id != item.id });

                        }
                    }
                }
                xhr.setRequestHeader('Content-Type', "application/octet-stream");
                // xhr.setRequestHeader('Content-Disposition', 'attachment; filename="' + file.name + '"');
                xhr.send(chunk);
                console.log("start send chank")

            }
            else {
                
                console.log({ cmd: "success file upload" });
                fileStack = fileStack.filter(function (a) { return a.id != item.id });
                setTimeout(function () {
                    item.$div.fadeOut(500, function () {
                        $div.remove();
                        if ($(".NFfileUploadItem").length == 0)
                            $(".NFfileUploadWrBox").addClass("hidden")
                    });
                    fileStack = fileStack.filter(function (a) { return a.id != item.id });
                }, 10000);
               
            }
        }
    }
    function old_uploadProcess(item) {
        try {
            console.log("start upload file", item.file);

            var file = item.file;
            var $div = item.$div;
            var xhr = new XMLHttpRequest();
            xhr.open("POST", serverRoot + "FileUploadSimple/" + item.blockId + "/" + item.i.toString() + "/", true);
            xhr.setRequestHeader("x-blockId", item.blockId);
            xhr.setRequestHeader("x-filename", encodeURIComponent(item.file.name));
            xhr.upload.addEventListener('progress', function (e) {
                if (e.lengthComputable) {

                    item.$div.css("color", "#eee");
                    var now = parseInt(e.loaded);
                    item.$div.attr("complited", now);
                    var percent = parseInt(parseFloat(e.loaded / e.total) * 100);
                    item.$div.find(".NFfileUploadItemStatus").css("width", percent + "%");
                    item.$div.find(".NFfileUploadItemPrecent").html(percent + "%");
                }
            }, false);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status == 200) {

                        console.log({ cmd: "success file upload" });
                        fileStack = fileStack.filter(function (a) { return a.id != item.id });
                        setTimeout(function () {
                            item.$div.fadeOut(500, function () {
                                $div.remove();
                                if ($(".NFfileUploadItem").length==0)
                                 $(".NFfileUploadWrBox").addClass("hidden")
                            });
                            fileStack = fileStack.filter(function (a) { return a.id != item.id });
                        }, 10000);
                    }
                    else {
                        console.warn({ cmd: "error file upload" });
                        item.$div.addClass("progressBarDanger");
                        item.$div.css("color", "red");
                        fileStack = fileStack.filter(function (a) { return a.id != item.id });
                    }
                }

            }
            setTimeout(function () {
                xhr.setRequestHeader('Content-Type', "application/octet-stream");
                // xhr.setRequestHeader('Content-Disposition', 'attachment; filename="' + file.name + '"');
                xhr.send(item.file);
                console.log("start send")
            }, 10);
        }
        catch (e) {
            console.warn({ cmd: "error file upload" });
            item.$div.addClass("progressBarDanger");
            item.$div.css("color", "red");
            fileStack = fileStack.filter(function (a) { return a.id != item.id });
        }

    }

    $.fn.NfFileUpload.addFileList = function (items, blockId) {

        console.log("items", items);
        for (var i = 0; i < items.length; i++) {
            var $div = $(div).addClass("NFfileUploadItem").attr("size", items[i].size).attr("complited", 0);
            $div.append(
                $(div)
                    .addClass("NFfileUploadItemStatusBar")
                    .append($(div).addClass("NFfileUploadItemStatus"))
            )
                .append(
                    $(div)
                    .append($(div).addClass("NFfileUploadItemTitle").html(RemoveHTMLTag(items[i].name)))
                        .append($(div).addClass("NFfileUploadItemPrecent"))
                        .append($(div).css("clear", "both"))
                );
            $(_ctrl).append($div);
            $div.css("color", "#eee");
           
            $div.attr("complited", 0);
            $div.find(".NFfileUploadItemStatus").css("width", 0 + "%");
            $div.find(".NFfileUploadItemPrecent").html(0 + "%");
            $(".NFfileUploadWrBox").removeClass("hidden")

            fileStack.push({ id: counterId, file: items[i], i: i, blockId: blockId, started: false, $div:$div })
            counterId++;
            
        }

        return;



        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serverRoot + "testservice.asmx/fileUploadGetNewFolder",
            data:JSON.stringify( { blockId: blockId }),
            dataType: "json",
            success: function (data) {
                var folderId = JSON.parse(data.d).id;
                filesAdd(items, folderId);

            },
            error: function (data) { console.warn("ERROR addFileList"); console.warn(data); }
        });
        return _ctrl;
    }
    function filesAdd(items, folderId) {
        console.log("filesAdd");
        console.log(items);
        for (var i = 0; i < items.length; i++) {
          
            var fileInfo = { name: items[i].name, size: items[i].size, file: items[i] };
         
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: serverRoot + "testservice.asmx/fileUploadGetNewFile",
                async: false,
                data: JSON.stringify({ folderId: folderId, fileId:0, fileInfo: fileInfo, i:i }),
                dataType: "json",
                success: function (data) {
                    var fileId = JSON.parse(data.d).id;
                    var j = JSON.parse(data.d).i;
                    var fileInfo2 = { name: items[j].name, size: items[j].size, file: items[j] };
                    filesAddToControl(fileInfo2, JSON.parse(data.d).folderId, JSON.parse(data.d).id);
                    
                    setTimeout(function () { 
                        buildChankStack(fileInfo2, JSON.parse(data.d).id);
                    }, 500)

                },
                error: function (data) { console.warn("ERROR addFileList"); console.warn(data); }
            });
          
        }
       
    }
    function buildChankStack(fileInfo, fileId, start) {
      
        if (typeof(start)=="undefined")
        start = 0;
     //   console.log(start);

       // while (start < fileInfo.size) {
            var end = start + BYTES_PER_CHUNK;
            var chunk;
            if (typeof fileInfo.file.slice === 'function') {
                chunk = fileInfo.file.slice(start, end);
            } else if (typeof fileInfo.file.webkitSlice === 'function') {
                chunk = fileInfo.file.webkitSlice(start, end);
            }
            chunkStack.push({ data: chunk, fileId: fileId, statByte: start, isActive: false });
            start += chunk.size;
       // }
        if (start < fileInfo.size)
            setTimeout(function () { buildChankStack(fileInfo, fileId, start) }, 20);
        else
        query();
    }
    function filesAddToControl(fileInfo, folderId, fileId)
    {
        $(_ctrl).append(
               $(div).addClass("NFfileUploadItem").attr("id", fileId).attr("folderId", folderId).attr("size", fileInfo.file.size).attr("complited", 0)
                       .append(
                       $(div)
                        .addClass("NFfileUploadItemStatusBar")
                        .append($(div).addClass("NFfileUploadItemStatus"))
                       )
               .append(
                   $(div)
                   .append($(div).addClass("NFfileUploadItemTitle").html(RemoveHTMLTag(fileInfo.name)))
                   .append($(div).addClass("NFfileUploadItemPrecent"))
                   .append($(div).css("clear", "both"))
               )
               );

        $("#" + fileId).find(".NFfileUploadItemStatus").css("width", 0 + "%");

        $("#" + fileId).find(".NFfileUploadItemPrecent").html("<img src='/images/loading.gif' style='height:1.2em'/>");

    }

    function query() {
        
       
        if (ActiveQueryes > 0)
            return;
        queryWorker();

    }
    function queryWorker() {
        for (var i = 0; i <= chunkStack.length && ActiveQueryes < MAX_QUERYES; i++) {
            uploadChunk(chunkStack.shift());
        }
        if (chunkStack.length > 0)
            setTimeout(queryWorker, 200);

    }
    function uploadChunk(chunkInfo)
    {
        if (typeof (chunkInfo) == "undefined")
            return;
        ActiveQueryes++;
        $.ajax({
            type: "POST",
            processData: false,
            contentType: null,
            url: serverRoot + "handlers/fileUploadGetNewChunk.ashx",
            headers: { 
                'Content-Range': 'bytes ' + chunkInfo.statByte + '-' + parseInt(chunkInfo.statByte + chunkInfo.data.size) + '/' + chunkInfo.data.size,
                'Origin-fileId':encodeURIComponent(chunkInfo.fileId)
            },
            data:chunkInfo.data,
            error: function (e) {
                console.warn(" error send chunk " + e.message);
                $("#" + chunkInfo.fileId).addClass("progressBarDanger");
                $("#" + chunkInfo.fileId).css("color", "red");
                chunkStack.push(chunkInfo);
                ActiveQueryes--;
            },
            success:function(e){
                console.log("send chunk success");
       
                if (e.bytes <= 0)
                {
                    chunkStack.push(chunkInfo);
                    console.warn(" error send chunk " + e.message);
                    $("#" + chunkInfo.fileId).css("color", "red");
                }
                else (updatePercent(e.fileId, e.bytes))
                ActiveQueryes--;
         }
        });
     
       
    }
    function updatePercent(fileId, bytes)
    {
        var total = $("#" + fileId).attr("size");
        var now = $("#" + fileId).attr("complited");
        $("#" + fileId).css("color", "#eee");
        now = parseInt(now) + parseInt(bytes);
        $("#" + fileId).attr("complited", now);
        var percent = parseInt(parseFloat(now / total) * 100);
        $("#" + fileId).find(".NFfileUploadItemStatus").css("width", percent + "%");
        $("#" + fileId).find(".NFfileUploadItemPrecent").html(percent + "%");


        if (percent >= 100) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: serverRoot + "testservice.asmx/fileUploadFileComplite",
                data: JSON.stringify({ fileId: fileId }),
                dataType: "json",
                success: function (data) {
                    var folderId = JSON.parse(data.d).id;
                    setTimeout(function () {
                        $("#" + fileId).fadeOut(500, function () { $("#" + fileId).remove() });
                    }, 10000);

                },
                error: function (data) { console.warn("ERROR addFileList"); console.warn(data); }
            });
        }
    }
}

