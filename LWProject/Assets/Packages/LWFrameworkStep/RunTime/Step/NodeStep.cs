using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class NodeStep:BaseStep
    {
    
        [LabelText("触发器集合")]
        public List<IStepTrigger> m_StepTriggerList;
        [LabelText("控制器集合")]
        public List<IStepController> m_StepControllerList;


        /// <summary>
        /// 执行完成的数量
        /// </summary>
        private int m_CompletedCount;
        public override void StartTriggerList()
        {
          
            for (int i = 0; m_StepTriggerList != null && i < m_StepTriggerList.Count; i++)
            {
                m_StepTriggerList[i].Start();
                m_StepTriggerList[i].TiggerCompleted = OnTiggerCompleted;
                m_StepTriggerList[i].CurrStepManager = m_StepManager;
                m_HaveTrigger = true;
            }
        }

        public override void StopTriggerList()
        {
            for (int i = 0; m_StepTriggerList != null && i < m_StepTriggerList.Count; i++)
            {
                m_StepTriggerList[i].TiggerCompleted = null;
                m_StepTriggerList[i].CurrStepManager = null;
                m_StepTriggerList[i].Stop();
            }
        }

        public override void StartControllerList(bool isBack = false)
        {
            base.StartControllerList();
            m_CompletedCount = 0;
            for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
            {
                m_StepControllerList[i].Start();
                m_StepControllerList[i].ControllerCompleted = OnControllerCompleted;
            }
            //如果没有触发器直接开始执行控制器
            if (m_StepTriggerList == null || m_StepTriggerList.Count == 0)
            {                
                OnTiggerCompleted(-1);
            }
        }

        public override void StopControllerList()
        {
            base.StopControllerList();
            for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
            {
                m_StepControllerList[i].ControllerCompleted = null;
                m_StepControllerList[i].Stop();
            }
        }
        protected override void OnTiggerCompleted(int index)
        {
            base.OnTiggerCompleted(index);
            m_CurrState = StepNodeState.Execute;
            for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
            {
                m_StepControllerList[i].Execute();
            }
            //如果没有控制器直接进入下一步
            if (m_StepControllerList == null || m_StepControllerList.Count == 0)
            {
                m_StepManager.MoveNext();
            }
        }

        private void OnControllerCompleted()
        {
            m_CompletedCount++;
            if (m_CompletedCount == m_StepControllerList.Count)
            {
                m_StepManager.MoveNext();
            }
        }
      

        public override  XElement GetXml()
        {
            XElement node = base.GetXml();
            XElement triggers = new XElement("Triggers");
            XElement controls = new XElement("Controls");
            node.Add(triggers);
            node.Add(controls);
            for (int i = 0; m_StepTriggerList!=null&& i < m_StepTriggerList.Count; i++)
            {
                triggers.Add(m_StepTriggerList[i].GetXml());
            }
            for (int i = 0; m_StepControllerList!=null&& i < m_StepControllerList.Count; i++)
            {
                controls.Add(m_StepControllerList[i].GetXml());
            }
          
            return node;
        }
        public override void SetXml(XElement xElement)
        {
            base.SetXml(xElement);
            List<XElement> triggerList = xElement.Element("Triggers").Elements("Trigger").ToList();
            List<XElement> controlList = xElement.Element("Controls").Elements("Control").ToList();
            m_StepTriggerList = new List<IStepTrigger>();
            m_StepControllerList = new List<IStepController>();
            for (int i = 0; i < triggerList.Count; i++)
            {
                IStepTrigger trigger = ConverHelp.Instance.CreateInstance<IStepTrigger>(triggerList[i].Attribute("ScriptName").Value);
                trigger.SetXml(triggerList[i]);
                m_StepTriggerList.Add(trigger);
            }
            for (int i = 0; i < controlList.Count; i++)
            {
                IStepController control = ConverHelp.Instance.CreateInstance<IStepController>(controlList[i].Attribute("ScriptName").Value);
                control.SetXml(controlList[i]);
                m_StepControllerList.Add(control);
            }
        }

    }
}