using Sirenix.OdinInspector;
using System.Xml.Linq;
namespace LWFramework.Step
{
    public class ST_Message : BaseStepTrigger
    {

        [LabelText("消息名称"), LabelWidth(70)]
        public string m_Message;

        public override void Start()
        {
            base.Start();
            ManagerUtility.MessageMgr.AddListener(m_Message, OnMessage);
        }
        
        private void OnMessage(LWFramework.Message.MessageData msg)
        {
            Finished();
        }

        public override void Stop()
        {
            base.Stop();
            ManagerUtility.MessageMgr.RemoveListener(m_Message);
        }

        public override XElement GetXml()
        {
            XElement trigger = new XElement("Trigger");
            trigger.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            trigger.Add(new XAttribute("Message", $"{m_Message}"));
            trigger.Add(new XAttribute("ResultIndex", $"{m_NextNum}"));
            return trigger;
        }
        public override void SetXml(XElement xElement)
        {
            m_Message = xElement.Attribute("Message").Value;
            m_NextNum = int.Parse(xElement.Attribute("ResultIndex").Value);
        }
    }
}