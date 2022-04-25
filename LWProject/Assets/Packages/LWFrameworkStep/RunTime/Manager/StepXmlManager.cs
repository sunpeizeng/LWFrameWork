using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class StepXmlManager : BaseStepManager, IManager
    {
        protected List<IStep> m_BaseStepXmlList;
        /// <summary>
        /// 当前Graph全部执行完成
        /// </summary>
        public override Action OnStepAllCompleted { get; set; }
        /// <summary>
        /// 当阶段改变时
        /// </summary>
        public override Action<object> OnStepChange { get; set; }

        public override object CurrStep { get => CurrStepNode; }
        /// <summary>
        /// 当前进行中的步骤
        /// </summary>
        public override IStep CurrStepNode { get; set; }
        protected int m_CurrIndex;
        //当前是否未最后一步
        protected bool m_LastStep = false;
        public override void SetData(object data)
        {
            ConverHelp.Instance.CallAssembly = System.Reflection.Assembly.GetCallingAssembly();
            m_BaseStepXmlList = new List<IStep>();
            XElement root = XElement.Parse(data.ToString());
            List<XElement> stepList = root.Elements("Step").ToList();
            for (int i = 0; i < stepList.Count; i++)
            {
                string str = stepList[i].Attribute("StepScript").Value;
                
                BaseStep baseStepXml = ConverHelp.Instance.CreateInstance<BaseStep>(str);
                baseStepXml.SetXml(stepList[i]);
                m_BaseStepXmlList.Add(baseStepXml);
            }
        }
        public void Init()
        {
        }

        public void Update()
        {
        }

        public override void StartStep()
        {
            m_CurrIndex = 0;
            SetCurrStepNode(m_BaseStepXmlList[0]);
            CurrStepNode.StartTriggerList();
            CurrStepNode.StartControllerList();
        }
        public override void StopStep() {
            CurrStepNode.StopControllerList();
            CurrStepNode.StopTriggerList();
            CurrStepNode = null;
            GameObjectContainer.Instance.ClearStepObjects();
            OnStepChange = null;
            OnStepAllCompleted = null;
        }
        public override void JumpStep(int index)
        {
            while (m_CurrIndex != index)
            {
                if (m_CurrIndex > index && m_CurrIndex >= 0)
                {
                    if (m_CurrIndex - index == 1)
                    {
                        MovePrev(true);
                    }
                    else {
                        MovePrev(false);
                    }
                    
                }
                else if (m_CurrIndex < index && m_LastStep == false)
                {
                    MoveNext();
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        ///  下一节点
        /// </summary>
        public override void MoveNext()
        {
            CurrStepNode.StopTriggerList();
            CurrStepNode.StopControllerList();
            if (CurrStepNode.NextNode == null)
            {
                int index = CurrStepNode.NextIndex;
                if (index < m_BaseStepXmlList.Count)
                {
                    CurrStepNode.NextNode = m_BaseStepXmlList[index];
                }

            }
            IStep stepNode = CurrStepNode.NextNode;
            if (stepNode != null)
            {
                stepNode.PrevNode = CurrStepNode;
                LWDebug.Log($"开启第{stepNode.Num}步节点");
                SetCurrStepNode(stepNode);
                CurrStepNode.StartTriggerList();
                CurrStepNode.StartControllerList();
                m_CurrIndex++;
            }
            else
            {
                OnStepAllCompleted?.Invoke();
                m_LastStep = true;
                LWDebug.Log("最后一步");
            }
        }
        /// <summary>
        ///  上一节点
        /// </summary>
        public override void MovePrev()
        {
            MovePrev(true);
        }
        void MovePrev(bool StartTrigger) {
            CurrStepNode.StartControllerList(true);
            CurrStepNode.StopTriggerList();
            IStep stepNode = GetPrevNode(CurrStepNode);
            if (stepNode != null)
            {
                SetCurrStepNode(stepNode);
                CurrStepNode.StopControllerList();
                if (StartTrigger) {

                    CurrStepNode.StartTriggerList();
                }
                CurrStepNode.StartControllerList();
                
                m_LastStep = false;
            }
        }
        IStep GetPrevNode(IStep stepNode) {
            m_CurrIndex--;
            IStep ret = stepNode.PrevNode;
            if (ret.PrevNode == null) {
                return ret;
            }
            if (ret is ChapterStep)
            {
                ret = GetPrevNode(ret);
            }
            else if (!ret.HaveTrigger) {
                ret = GetPrevNode(ret);
            }
            return ret;
        }
        private void SetCurrStepNode(IStep step)
        {
            CurrStepNode = step;
            CurrStepNode.StepManager = this;
            CurrStepNode.SetSelfCurrent();
            OnStepChange?.Invoke(step);
        }


    }
}