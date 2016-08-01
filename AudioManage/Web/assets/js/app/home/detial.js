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
        $.post(rootUrl + 'Home/BuildQrCode', {
            audioId: $('#AudioId').val()
        }, function (res) {
            if (res.state) {


                $.tips(res.msg, 3);
            } else {
                $.tips(res.msg, 0);
            }
        });
    });

    $('#cancel').on('click', function () {
        window.close();
    });

    $('#save').on('click', function () {
        var panel = $('.panel-body');
        var attachments = [];

        panel.find('.finish-queue-item').each(function () {
            if ($(this).data('filepath')) {
                attachments.push($(this).data('filepath'));
            }
        });

        var audioEntity = {
            Id: $("#id").val(),
            Title: $("#title").val(),
            Content: $("#content").val(),
            Location: $("#location").val(),
            Remark: $("#Remark").val(),
            AudioFile: $("#AudioFile").val(),
            AudioFileId: attachments.join(),
            CreateTime: $("#CreateTime").val(),
            Order: $("#Order").val(),
            State:$("#State").val()
        }

        $.post(rootUrl + 'Home/detial', { valueSetJson: JSON.stringify(audioEntity) }, function (res) {
            if (res.state) {

                $.tips(res.msg, 3);
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
            //if (file.type != '.mp3' ) {
            //    $.tips('限制类文件，请重新上传！', 0);
            //    return false;
            //}

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





});