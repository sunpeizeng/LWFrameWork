using LWFramework.Core;
using LWFramework.Message;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public abstract class BaseStep : IStep
    {
        
        [ReadOnly, LabelText("序号")]
        public int m_Num;
        [ReadOnly, LabelText("唯一ID")]
        public string m_UniqueId;
        [LabelText("备注")]
        public string m_Remark;

        private int m_NextIndex;
        private IStep m_PrevNode;
        private IStep m_NextNode;
        protected bool m_HaveTrigger = false;
        /// <summary>
        /// 步骤管理器
        /// </summary>
        protected IStepManager m_StepManager;
        public IStepManager StepManager
        {
            get => m_StepManager; set => m_StepManager = value;
        }
        public int Num
        {
            get => m_Num; set => m_Num = value;
        }
        public string UniqueId
        {
            get => m_UniqueId; set => m_UniqueId = value;
        }
        /// <summary>
        /// 下一步骤的脚标
        /// </summary>
        public int NextIndex
        {
            get => m_NextIndex; 
        }
        protected StepNodeState m_CurrState;
        /// <summary>
        /// 当前的状态
        /// </summary>
        public StepNodeState CurrState { 
            get => m_CurrState; 
            set => m_CurrState = value; 
        } 
        /// <summary>
        /// 上一节点
        /// </summary>
        public IStep PrevNode { 
            get => m_PrevNode; 
            set => m_PrevNode = value; 
        }
        /// <summary>
        /// 下一节点
        /// </summary>
        public IStep NextNode
        {
            set => m_NextNode = value;
            get => m_NextNode;
        }
        public bool HaveTrigger {
            get => m_HaveTrigger;
        }
        public void SetSelfCurrent()
        {

            MessageData msg = MessageDataPool.GetMessage(nameof(StepCommonMessage.StepHelp));
            msg[nameof(StepCommonMessageKey.StepHelpKey)] = m_Remark;
            ManagerUtility.MessageMgr.Dispatcher(msg);
        }

        public virtual void StartTriggerList() { 
        
        }
        public virtual void StopTriggerList() { 
        
        }
        public virtual void StartControllerList(bool isBack = false)
        {
            m_NextIndex = m_Num + 1;
            m_CurrState = StepNodeState.Wait;
        }
        public virtual void StopControllerList()
        {
            m_CurrState = StepNodeState.Complete;
        }

        public virtual XElement GetXml() {
            XElement node = new XElement("Step");
            node.Add(new XAttribute("Num", $"{m_Num}"));
            node.Add(new XAttribute("UniqueId", $"{m_UniqueId}"));
            node.Add(new XAttribute("StepScript", $"{this.GetType()}"));
            node.Add(new XAttribute("Remark", $"{m_Remark}"));
           
            return node;
        }

        public virtual void SetXml(XElement xElement) {
            m_Remark = xElement.Attribute("Remark").Value;
            m_Num = int.Parse(xElement.Attribute("Num").Value);
            m_UniqueId = xElement.Attribute("UniqueId").Value;
        }

        protected virtual void OnTiggerCompleted(int index)
        {
            if (index != -1)
            {
                m_NextIndex = index;
            }

                     
        }
    }
}