using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.UI {
    /// <summary>
    /// 用于拆分View Logic的View基类
    /// </summary>
    /// <typeparam name="TUILogic"></typeparam>
    public class BaseLogicUIView <TUILogic>: BaseUIView where TUILogic : class,IUILogic
    {
        protected TUILogic m_Logic;
        public override void CreateView(GameObject gameObject)
        {
            m_Logic = Activator.CreateInstance(typeof(TUILogic),this) as TUILogic;
            base.CreateView(gameObject);
            m_Logic.OnCreateView();
        }
        public override void OpenView()
        {
            m_Logic.OnOpenView();
            base.OpenView();         
        }
        public override void UpdateView()
        {
            base.UpdateView();
            m_Logic.OnUpdateView();
        }
        public override void CloseView()
        {
            base.CloseView();
            m_Logic.OnCloseView();
        }
       
        public override void ClearView()
        {
            base.ClearView();
            m_Logic.OnClearView();
        }
    }

}
