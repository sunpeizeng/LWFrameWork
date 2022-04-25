using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Xml.Linq;
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤控制器，主要用于处理各种步骤中的变化效果
    /// </summary>
    public class SC_Active : BaseStepObjectController
    {

        [LabelText("开始Active"), LabelWidth(90)]
        public bool m_BeginActive;
        [LabelText("结束Active"), LabelWidth(90)]
        public bool m_EndActive;
        private GameObject m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
            if (m_Target == null) {
                LWDebug.Log($"找不到 {m_ObjName} 对象");
            }
            m_Target.SetActive(m_BeginActive);
        }
        public override void Stop()
        {
            m_Target.SetActive(m_EndActive);
        }
        public override void Execute()
        {
            m_ControllerExecuteCompleted?.Invoke();
        }
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("BeginActive", $"{m_BeginActive}"));
            control.Add(new XAttribute("EndActive", $"{m_EndActive}"));
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_BeginActive = xElement.Attribute("BeginActive").Value == "True" ? true : false;
            m_EndActive = xElement.Attribute("EndActive").Value == "True" ? true : false;

        }
    }
}