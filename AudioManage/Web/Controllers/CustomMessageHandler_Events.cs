﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Configuration;
using Senparc.Weixin.MP.Entities;

namespace Web.Controllers
{
    public partial class CustomMessageHandler
    {
        //public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        //{
        //    // 预处理文字或事件类型请求。
        //    // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
        //    // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
        //    // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
        //    // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
        //    // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

        //    if (requestMessage.Content == "OneClick")
        //    {
        //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
        //        strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
        //        return strongResponseMessage;
        //    }
        //    return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        //}

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            string msg = string.Empty;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
               
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这里写什么都无所谓，比如：上帝爱你！";
            return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
        }
        private string Scan = ConfigurationManager.AppSettings["Scan"];
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {

            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            //通过扫描关注
            try
            {
                string url = requestMessage.EventKey.Replace("qrscene_", "");
                CreateResponseMessage<ResponseMessageNews>();
                string mobileUrl = System.Configuration.ConfigurationManager.AppSettings["mobileUrl"];

                string id = url.Replace(mobileUrl, "");
                var item = AudioCore.Entity.AudioEntity.GetSingle(id);
                if (item != null)
                {
                    responseMessage.Articles.Add(new Article()
                    {
                        Title = item.Title,
                        Description = item.ContentMini,
                        Url = url
                    });

                }
                else
                {
                    responseMessage.Articles.Add(new Article()
                    {
                        Title = "未找到文件",
                        Description = url,
                        Url = url
                    });

                }

                return responseMessage;
               


            }
            catch (Exception e)
            {
                responseMessage.Articles.Add(new Article()
                {
                    Title = e.Message,
                    Description = ""

                }
                );
            }
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "接收到了群发完成的信息。";
            return responseMessage;
        }


        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            //通过订阅关注
            try
            {
                

                bool state = false;
                string msg = string.Empty;
                if (!string.IsNullOrEmpty(requestMessage.EventKey))
                {
                    string url = requestMessage.EventKey.Replace("qrscene_", "");
                    CreateResponseMessage<ResponseMessageNews>();
                    string mobileUrl = System.Configuration.ConfigurationManager.AppSettings["mobileUrl"];
                    string id = url.Replace(mobileUrl, "");
                    var item = AudioCore.Entity.AudioEntity.GetSingle(id);
                    if (item != null)
                    {
                        responseMessage.Articles.Add(new Article()
                        {
                            Title = item.Title,
                            Description = item.ContentMini,
                            Url = url
                        });
                        
                    }
                    else
                    {
                        responseMessage.Articles.Add(new Article()
                        {
                            Title = "未找到文件",
                            Description = url,
                            Url = url
                        });
                        
                    }

                    return responseMessage;
                }
                else
                {

                }


            }
            catch (Exception e)
            {
                responseMessage.Articles.Add(new Article()
                    {
                        Title = e.Message,
                        Description = ""

                    }
                );
            }
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件(scancode_push)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之扫码推事件";
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
            responseMessage.Image.MediaId = requestMessage.ScanCodeInfo.ScanResult;

            return responseMessage;

        }

        /// <summary>
        /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出拍照或者相册发图";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出系统拍照发图(pic_sysphoto)
        /// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出系统拍照发图";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出微信相册发图器(pic_weixin)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出微信相册发图器";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出地理位置选择器";
            return responseMessage;
        }
    }
}