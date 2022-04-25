using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤控制器，主要用于处理各种步骤中的变化效果
    /// </summary>
    [Serializable]
    public abstract class BaseStepController : IStepController
    {
        [LabelText("备注"), GUIColor(0, 1, 0)]
        public string m_Remark;

        protected Action m_ControllerExecuteCompleted;


        /// <summary>
        /// 当前控制器执行完成的回调
        /// </summary>
        public Action ControllerCompleted { get => m_ControllerExecuteCompleted; set => m_ControllerExecuteCompleted = value; }
        public string Remark { get => m_Remark; }

        public abstract void Start();
        public abstract void Stop();
        public abstract void Execute();

        public virtual void SetXml(XElement xElement)
        {
        }
        public virtual XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));

            return control;
        }
    }

   
}