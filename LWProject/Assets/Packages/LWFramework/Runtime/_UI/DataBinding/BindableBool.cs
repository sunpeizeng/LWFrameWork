using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LWFramework.Core;

namespace LWFramework.UI
{
    public class BindableBool : BindableType
    {
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="control">绑定的目标控件</param>
        public override void Binding(UIBehaviour control, TableData viewData, object key)
        {
            base.Binding(control, viewData, key);
            if (control is Toggle)
            {
                Toggle toggle = m_UI as Toggle;
                toggle.onValueChanged.AddListener((value) => { m_ViewData[m_Key] = value; });
            }

        }
        public override void Change()
        {
            base.Change();
            var value = m_ViewData.Get(m_Key);
            if (value == null)
            {
                return;
            }
            if (m_UI is Toggle)
            {
                Toggle toggle = m_UI as Toggle;
                toggle.isOn = (bool)value;
            }
            else if (m_UI is Button)
            {
                Button button = m_UI as Button;
                button.interactable = (bool)value; ;
            }
        }
       
    }
}
   
