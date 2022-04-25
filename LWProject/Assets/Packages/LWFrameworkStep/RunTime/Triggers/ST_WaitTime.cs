using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Xml.Linq;
namespace LWFramework.Step
{
    public class ST_WaitTime : BaseStepTrigger
    {

        [LabelText("等待时间"), LabelWidth(70)]
        public float m_WaitTime;

        public override void Start()
        {
            base.Start();
            _ = WaitTimeAsync();
        }
        /// <summary>
        //使用Task处理等待时间
        /// </summary>
        async UniTaskVoid WaitTimeAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_WaitTime), ignoreTimeScale: false);
            Finished();
        }
        public override void Stop()
        {
            base.Stop();
        }
        public override XElement GetXml()
        {
            XElement trigger = new XElement("Trigger");
            trigger.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            trigger.Add(new XAttribute("WaitTime", $"{m_WaitTime}"));
            trigger.Add(new XAttribute("ResultIndex", $"{m_NextNum}"));
            return trigger;
        }
        public override void SetXml(XElement xElement)
        {
            m_WaitTime = float.Parse(xElement.Attribute("WaitTime").Value);
            m_NextNum = int.Parse(xElement.Attribute("ResultIndex").Value);
        }
    }
}