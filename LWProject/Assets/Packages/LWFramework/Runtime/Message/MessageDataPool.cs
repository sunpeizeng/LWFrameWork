using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Message {
    /// <summary>
    /// 消息池数据管理
    /// </summary>
    public class MessageDataPool
    {
        public static List<MessageData> messageList = new List<MessageData>();
        public static MessageData GetMessage()
        {
            MessageData ret;
            if (messageList.Count <= 0)
            {
                ret = new MessageData();
                //  Debug.Log("创建一个消息对象");
            }
            else
            {
                ret = messageList[0];
                messageList.Remove(ret);
            }
            return ret;
        }
        public static MessageData GetMessage(string type)
        {
            MessageData ret = GetMessage();
            //ret.data = data;
            ret.type = type;
            return ret;
        }
        public static MessageData GetMessage(string type, GameObject sender)
        {
            MessageData ret = GetMessage(type);
            ret.sender = sender;
            return ret;
        }
        public static MessageData GetMessage(string type, GameObject sender, float delay)
        {
            MessageData ret = GetMessage(type, sender);
            ret.delay = delay;
            return ret;
        }
        public static void AddMessage(MessageData msg)
        {
            //Debug.Log("添加一个消息对象");
            msg.Clear();
            messageList.Add(msg);
        }
    }
}
