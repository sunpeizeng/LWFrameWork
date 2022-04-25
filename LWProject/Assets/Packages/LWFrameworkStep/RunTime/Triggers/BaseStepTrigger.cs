
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class BaseStepTrigger : IStepTrigger
    {
        // 是否触发，避免多次触发效果
        protected bool m_IsTrigger;
        protected Action<int> m_TiggerActionCompleted;
        protected IStepManager m_CurrStepManager;
        [LabelText("触发结果序号"),Tooltip("-1自动往下走"), LabelWidth(100)]
        public int m_NextNum = -1;
        /// <summary>
        /// TiggerCompleted触发完成
        /// </summary>
        public Action<int> TiggerCompleted { get => m_TiggerActionCompleted; set => m_TiggerActionCompleted = value; }
        /// <summary>
        /// 当前的Graph
        /// </summary>
        public IStepManager CurrStepManager { get => m_CurrStepManager; set => m_CurrStepManager = value; }

        public virtual void Start()
        {
            m_IsTrigger = false;
        }
        public virtual void Finished()
        {
            m_IsTrigger = true;
            m_TiggerActionCompleted?.Invoke(m_NextNum);
        }
        public virtual void Stop()
        {

        }
   


        public virtual XElement GetXml()
        {
            return null;
        }

        public virtual void SetXml(XElement xElement)
        {
        }
    }
}