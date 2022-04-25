using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using LWFramework.Core;

namespace LWFramework.UI
{
    /// <summary>
    /// 可绑定的数据类型
    /// </summary>
    public class BindableType : IPoolObject
    {
        public UIBehaviour m_UI;
        protected TableData m_ViewData;
        protected object m_Key;

        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="control">绑定的目标控件</param>
        /// <param name="viewData">ViewData</param>
        /// <param name="key">绑定的Key</param>
        public virtual void Binding(UIBehaviour control, TableData viewData,object key)
        {
            m_UI = control;
            m_ViewData = viewData;
            m_Key = key;
        }
        /// <summary>
        /// 数据改变
        /// </summary>
        public virtual void Change()
        {

        }

        public void Release()
        {
            
        }

        public void UnSpawn()
        {
            m_UI = null;
            m_ViewData = null;
            m_Key = null;
        }
    }
}
