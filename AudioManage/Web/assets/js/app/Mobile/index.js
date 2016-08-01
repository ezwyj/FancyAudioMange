define(['common', 'bootstrap' ], function ($) {

    var AppId = '';

    //wx.config({
    //    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
    //    appId: AppId, // 必填，公众号的唯一标识
    //    timestamp: '@Model.JsSdkPackage.Timestamp', // 必填，生成签名的时间戳
    //    nonceStr: '@Model.JsSdkPackage.NonceStr', // 必填，生成签名的随机串
    //    signature: '@Model.JsSdkPackage.Signature',// 必填，签名
    //    jsApiList: [
    //            'checkJsApi',
    //            'onMenuShareTimeline',
    //            'onMenuShareAppMessage',
    //            'onMenuShareQQ',
    //            'onMenuShareWeibo',
    //            'hideMenuItems',
    //            'showMenuItems',
    //            'hideAllNonBaseMenuItem',
    //            'showAllNonBaseMenuItem',
    //            'chooseImage',
    //            'previewImage',
    //            'uploadImage',
    //            'downloadImage',
    //            'getNetworkType',
    //            'hideOptionMenu',
    //            'showOptionMenu',
    //            'closeWindow',
    //            'scanQRCode',
    //            'chooseWXPay',
    //            'openProductSpecificView'
    //    ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2。详见：http://mp.weixin.qq.com/wiki/7/aaa137b55fb2e0456bf8dd9148dd613f.html
    //});

    wx.error(function (res) {
        console.log(res);
        alert('验证失败');
    });

    wx.ready(function () {
        
        document.getElementById('bgmusic').play();
    
    });

    


});