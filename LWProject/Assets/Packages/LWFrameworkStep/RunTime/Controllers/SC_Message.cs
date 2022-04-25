using Cysharp.Threading.Tasks;
using LWFramework.Message;
using LWFramework.Step;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class SC_Message : BaseStepController
    {
        [LabelText("等待发送时间"), LabelWidth(90)]
        public float m_EndWaitTime;
        [LabelText("消息数据"), LabelWidth(90)]
        public SC_MessageData[] m_MessageDataArray;
        public override void Start()
        {
            if (m_MessageDataArray.Length>0)
            {
                MessageData msg = MessageDataPool.GetMessage(m_MessageDataArray[0].messageType);
                SetMsg(m_MessageDataArray[0].messageValue, msg);
                ManagerUtility.MessageMgr.Dispatcher(msg);
            }
        }
        public override void Stop()
        {
            if (m_MessageDataArray.Length > 1)
            {
                MessageData msg = MessageDataPool.GetMessage(m_MessageDataArray[1].messageType);
                SetMsg(m_MessageDataArray[1].messageValue, msg);
                ManagerUtility.MessageMgr.Dispatcher(msg);
            }
        }
        void SetMsg(string xmlStr, MessageData msg)
        {
            if (xmlStr == "") return;
            string[] xmlValues = xmlStr.Split(';');
            for (int i = 0; i < xmlValues.Length; i++)
            {

                string[] filed = xmlValues[i].Split('=');
                msg[filed[0]] = filed[1];
            }
        }
        public override void Execute()
        {
            _ = WaitTimeAsync();
        }

        //使用Task处理等待时间
        /// </summary>
        async UniTaskVoid WaitTimeAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_EndWaitTime), ignoreTimeScale: false);
            m_ControllerExecuteCompleted?.Invoke();
        }

        public override XElement GetXml()
        {
            XElement control = base.GetXml();
            control.Add(new XAttribute("EndWaitTime", $"{m_EndWaitTime}"));

            XElement datas = new XElement("Datas");
            control.Add(datas);
            for (int i = 0; i < m_MessageDataArray.Length; i++)
            {
                XElement data = new XElement("Data");
                data.Add(new XAttribute("MessageType", $"{m_MessageDataArray[i].messageType}"));
                data.Add(new XAttribute("MessageValue", $"{m_MessageDataArray[i].messageValue}"));
                datas.Add(data);
            }
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_EndWaitTime = float.Parse(xElement.Attribute("EndWaitTime").Value);
            List<XElement> dataList = xElement.Element("Datas").Elements("Data").ToList();
            m_MessageDataArray = new SC_MessageData[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_MessageDataArray[i] = new SC_MessageData()
                {
                    messageType = dataList[i].Attribute("MessageType").Value,
                    messageValue = dataList[i].Attribute("MessageValue").Value
                };
            }
        }
    }
    [Serializable]
    public class SC_MessageData
    {
        [HorizontalGroup(LabelWidth = 90)]
        public string messageType;
        [HorizontalGroup(LabelWidth = 90, Width = 300)]
        public string messageValue;

    }
}