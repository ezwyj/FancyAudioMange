define(['common', 'util', 'template', 'wysiwyg','uploadify', 'ztree', 'plugins', 'datepicker', 'bootstrap', ], function ($, util, template) {
    var rootUrl = OP_CONFIG.rootUrl;


    //////////////////////
    //页面初始化
    //////////////////////
    //////////////////////
    $('.datepicker').datepicker({
        format: 'yyyy/m/d',
        autoclose: true,
        clearBtn: true,
        todayHighlight: true,
        language: 'zh-CN'
    });

    //$('#editor').wysiwyg();

    //////////////////////
    //事件绑定
    //////////////////////
    $('#buildQrCode').on('click', function () {
        $.post(rootUrl + 'Home/SubmitQrcode', { audioId: $("#audioId").val(), savefile: $("#QrcodeFile").val() }, function (res) {
            if (res.state) {

                $.tips('生成功能', 3);
                window.location.reload();
            } else {
                $.tips(res.msg, 0);
            }
        });
    });

    $('#cancel').on('click', function () {
        window.close();
    });

    $('#deletAudio').on('click', function () {
        //删除
        $.confirm('是否删除本条记录？', function (result) {
            if (result) {

                $.post(rootUrl + 'Home/submitDelete', {
                    audioId: $("#audioId").val()
                }, function (res) {
                    if (res.state) {
                        $.tips('删除成功！', 0);
                        window.location.reload();
                        
                        
                    } else {
                        $.tips(res.msg, 0);
                    }
                });
                //删除附件

            }
        });
    });

    $('#save').on('click', function () {
        var panel = $('.panel-body');
        var attachments = [];
        var attachmentsImg = [];
        var attachmentNames = [];

        panel.find('.finish-queue-item').each(function () {
            if ($(this).data('fileid')) {
                attachments.push($(this).data('fileid'));
            }
        });

        panel.find('.finish-queue-itemImg').each(function () {
            if ($(this).data('fileid')) {
                attachmentsImg.push($(this).data('fileid'));
            }
        });
        //if (attachments.length == 0) {
        //    $.tips("没有音乐文件上传", 3);
        //    return false;
        //}

        var audioEntity = {
            Id: $("#audioId").val(),
            Title: $("#title").val(),
            Content: $("#editor").val(),
            Location: $("#location").val(),
            Remark: $("#Remark").val(),
            AudioFile: $("#AudioFile").val(),
            AudioFileId: attachments.join(),
            img: attachmentsImg.join(),
            CreateTime: $("#createTime").val(),
            ClassONE: $("#ClassOne").val(),
            ClassTWO: $("#ClassTwo").val(),
            orderNum: $("#order").val(),
            State:$("#State").val()
        }

        $.post(rootUrl + 'Home/detial', { valueSetJson: JSON.stringify(audioEntity) }, function (res) {
            if (res.state) {

                $.tips(res.msg, 3);
                window.location.reload();
            } else {
                $.tips(res.msg, 0);
            }
        });
    });

    $('#select-attachment').uploadify({
        swf: rootUrl + 'assets/js/lib/uploadify.swf',
        uploader: rootUrl + 'Common/UploadAttachment',
        fileSizeLimit: '100 MB',
        onSelect: function (file) {
            if (file.type != '.mp3' ) {
                $.tips('限制类文件，请重新上传！', 0);
                return false;
            }

            var queue = $('#' + this.settings.queueID);
            var html = '<div id="#{fileId}" class="uploadify-queue-item">' +
                            '<span class="icon #{fileIcon}"></span>' +
                            '<span class="file-name" title="#{fileName}">#{fileName}</span>' +
                            '<div class="uploadify-progress">' +
                                '<div class="uploadify-progress-bar">&nbsp;</div>' +
                            '</div>' +
                            '<span class="data">Waiting</span>' +
                        '</div>';

            var fileData = {
                fileId: file.id,
                fileName: file.name,
                fileIcon: util.getFileIcon(file.name)
            }


            queue.append(util.parseTpl(html, fileData));
        },
        onUploadSuccess: function (file, res) {
            res = JSON.parse(res);

            if (res.state) {
                var data = res.data[0];
                var html = '<div id="#{fileId}" class="finish-queue-item" data-fileid="#{id}">' +
                                '<span class="icon #{fileIcon}"></span>' +
                                '<span class="file-name" title="#{fileName}">#{fileName}</span>' +
                                '<span class="file-size">#{fileSize}</span>' +
                                '<a class="file-operate file-del" href="#">删除</a>' +
                            '</div>';
                var fileData = {
                    id: data.Id,
                    fileId: file.id,
                    fileIcon: util.getFileIcon(file.name),
                    fileName: file.name,
                    fileSize: util.getFileSize(file.size)
                }

                $('#' + file.id).replaceWith(util.parseTpl(html, fileData));

                //删除
                $('#' + file.id).on('click', '.file-del', function () {
                    $('#' + file.id).remove();
                    return false;
                });
            } else {
                $.tips(res.msg);
            }
        },
        onUploadStart: function (file) {
            $("#select-attachment").uploadify("settings", "formData", { 'extra': 1 });
            //在onUploadStart事件中，也就是上传之前，把参数写好传递到后台。  
        }
    });

    $('#select-attachmentPic').uploadify({
        swf: rootUrl + 'assets/js/lib/uploadify.swf',
        uploader: rootUrl + 'Common/UploadAttachment',
        fileSizeLimit: '100 MB',
        onSelect: function (file) {
            if (file.type != '.jpg' && file.type != '.png' && file.type != '.gif' && file.type != '.bmp') {
                $.tips('限制类文件，请重新上传！', 0);
                return false;
            }

            var queue = $('#' + this.settings.queueID);
            var html = '<div id="#{fileId}" class="finish-queue-itemImg">' +
                            '<span class="icon #{fileIcon}"></span>' +
                            '<span class="file-name" title="#{fileName}">#{fileName}</span>' +
                            '<div class="uploadify-progress">' +
                                '<div class="uploadify-progress-bar">&nbsp;</div>' +
                            '</div>' +
                            '<span class="data">Waiting</span>' +
                        '</div>';

            var fileData = {
                fileId: file.id,
                fileName: file.name,
                fileIcon: util.getFileIcon(file.name)
            }


            queue.append(util.parseTpl(html, fileData));
        },
        onUploadSuccess: function (file, res) {
            res = JSON.parse(res);

            if (res.state) {
                var data = res.data[0];
                var html = '<div id="#{fileId}" class="finish-queue-itemImg" data-fileid="#{id}">' +
                                '<span class="icon #{fileIcon}"></span>' +
                                '<span class="file-name" title="#{fileName}">#{fileName}</span>' +
                                '<span class="file-size">#{fileSize}</span>' +
                                '<a class="file-operate file-del" href="#">删除</a>' +
                            '</div>';
                var fileData = {
                    id: data.Id,
                    fileId: file.id,
                    fileIcon: util.getFileIcon(file.name),
                    fileName: file.name,
                    fileSize: util.getFileSize(file.size)
                }

                $('#' + file.id).replaceWith(util.parseTpl(html, fileData));

                //删除
                $('#' + file.id).on('click', '.file-del', function () {
                    $('#' + file.id).remove();
                    return false;
                });
            } else {
                $.tips(res.msg);
            }
        },
        onUploadStart: function (file) {
           
            //在onUploadStart事件中，也就是上传之前，把参数写好传递到后台。  
        }
    });





});