require(['common', 'util', 'plugins', 'bootstrap', 'datepicker'], function ($, util) {
    var rootUrl = OP_CONFIG.rootUrl;

    //////////////////////
    //页面初始化
    //////////////////////



    var resultTable = $('#result-list').table({
        url: rootUrl + 'Home/SearchList',
        maxHeight: $('#main').height() - 270,
        data: function () {
            
            return {
                pageIndex: 0,
                pageSize: 20,
                keyword: $.trim($('#title').val())
                
            };
        },
        tableClass: 'table-condensed',
        colOptions: [{
            name: '序号',
            field: 'Id',
            width: 50,
            align: 'center'
        }, {
            name: '标题',
            field: 'Title',
            handler: function (value, data) {
                return '<a href="' + rootUrl + 'Home/Detail?Id=' + data.Id + '" title=' + data.Title + '" target="_blank">' + data.TitleMini + '</a>';

            }
        }, {
            name: '位置',
            field: 'Location',
            width: 80
        }, {
            name: '状态',
            field: 'StateExp',
            width: 80
        }, {
            name: '提交日期',
            field: 'CreateTimeExp',
            width: 130
        }, {
            name: '备注',
            field: 'Remark',
            width: 70
        }]
    });

    //////////////////////
    //事件绑定
    //////////////////////


    $('#query').on('click', function () {
        resultTable.table('reload');
    });

    $('#add').on('click', function () {
        window.open('/home/Detial?id=0', 'newwindow', 'height=800x, width=600px, scrollbarsno, resizable=no')
    });

    

    

});