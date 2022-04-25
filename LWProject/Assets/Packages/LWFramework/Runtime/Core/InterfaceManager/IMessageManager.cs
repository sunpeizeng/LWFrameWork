using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Message
{
    public class MessageDelegate
    {
        public delegate void MessageHandler(MessageData msg);
    }
    /// <summary>
    /// 消息管理接口
    /// </summary>
    public interface IMessageManager
    {
        /// <summary>
        /// 添加一个消息监听
        /// </summary>
        /// <param name="type">消息的标识符</param>
        /// <param name="handler"></param>
        void AddListener(string type, MessageDelegate.MessageHandler handler);

        /// <summary>
        /// 检测是否包含这种类型的监听
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        bool CheckListener(string type);
        /// <summary>
        /// 添加唯一一个消息监听
        /// </summary>
        /// <param name="type">消息的标识符</param>
        /// <param name="handler"></param>
        void AddListenerSingle(string type, MessageDelegate.MessageHandler handler);
        /// <summary>
        /// 移除消息监听
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="handler">消息监听的委托，当为空时移除所有的type类型</param>
        /// <returns></returns>
        void RemoveListener(string type, MessageDelegate.MessageHandler handler = null);
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg"></param>
        void Dispatcher(MessageData msg);
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="type">不传参数的情况下使用</param>
        void Dispatcher(string type);

        void UpdateMsg();

        void Destory();
    }

}
