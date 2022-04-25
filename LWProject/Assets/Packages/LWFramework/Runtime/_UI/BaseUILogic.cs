using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.Core;
using System;

namespace LWFramework.UI {
    /// <summary>
    /// 处理UI的逻辑,在View CreateView的时候处理的逻辑依次是 Logic构造函数->View获取UI组件->Logic OnCreate->View 自定义代码
    /// </summary>
    /// <typeparam name="TUIView"></typeparam>
    public class BaseUILogic <TUIView>: IUILogic where TUIView : class, IUIView
    {
        protected TUIView m_View;
        /// <summary>
        /// View的数据
        /// </summary>
        protected TableData m_ViewData;

        private Dictionary<object, Action<object>> m_ListenerDict;
        public BaseUILogic(TUIView p_View) {
            m_View = p_View;
            m_ViewData = ManagerUtility.UIMgr.ViewData;//new ViewData();
        }
        public virtual void OnCreateView() { }
        public virtual void OnOpenView()
        {
            m_ViewData.OnDataChange += OnViewDataChange;
            m_ListenerDict = new Dictionary<object, Action<object>>();
        }
        public virtual void OnUpdateView()
        {

        }
        public virtual void OnCloseView()
        {
            m_ViewData.OnDataChange -= OnViewDataChange;
            if (m_ListenerDict != null) {
                m_ListenerDict.Clear();
                m_ListenerDict = null;
            }
        }

        public virtual void OnClearView()
        {
            OnCloseView();
            m_ViewData = null;
        }
        protected void AddListener(object key,Action<object>action) {
            m_ListenerDict.Add(key, action);
        }
      
        /// <summary>
        /// 内部处理ViewData中数据改变后的逻辑
        /// </summary>
        /// <param name="dataKey">改变的数值的key</param>
        private void OnViewDataChange(object dataKey) {
            Action<object> action;
            if (m_ListenerDict.TryGetValue(dataKey,out action)) {
                action.Invoke(m_ViewData[dataKey]);
            }
        }
    }
}

