using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LWFramework.Core;

namespace LWFramework.UI
{
    public class BindableFloat : BindableType
    {
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="control">绑定的目标控件</param>
        public override void Binding(UIBehaviour control, TableData viewData, object key)
        {
            base.Binding(control, viewData, key);
            if (control is Slider)
            {
                Slider slider = control as Slider;
                slider.onValueChanged.AddListener((value) => { m_ViewData[m_Key] = value; });
            }
            else if (control is InputField)
            {
                InputField inputField = control as InputField;
                inputField.onValueChanged.AddListener((value) => { 
                    m_ViewData[m_Key] = value; 
                });
            }
            else if (control is Scrollbar)
            {
                Scrollbar scrollbar = control as Scrollbar;
                m_ViewData[m_Key] = scrollbar.value;
                scrollbar.onValueChanged.AddListener((value) => { m_ViewData[m_Key] = value; });
            }
            


        }
        public override void Change()
        {
            base.Change();
            

            if (m_ViewData.Get(m_Key)==null ) {
                return;
            }
            string value = m_ViewData.Get<object>(m_Key).ToString();
            if (m_UI is InputField)
            {
                InputField inputField = m_UI as InputField;
                inputField.text = value;
            }
            else if(m_UI is Text)
            {
                Text text = m_UI as Text;
                text.text = value;
            }            
            else if(m_UI is Slider)
            {
                Slider slider = m_UI as Slider;
                float floatValue = 0;
                if (float.TryParse(value, out floatValue)) {
                    slider.value = floatValue;
                }
              
            }
            else if (m_UI is Scrollbar)
            {
                Scrollbar scrollbar = m_UI as Scrollbar;
                float floatValue = 0;
                if (float.TryParse(value, out floatValue))
                {
                    scrollbar.value = floatValue;
                }
            }
            else if (m_UI is Image)
            {
                Image img = m_UI as Image;
                float floatValue = 0;
                if (float.TryParse(value, out floatValue))
                {
                    img.fillAmount = floatValue;
                }
            }
        }
       
    }
}
   
