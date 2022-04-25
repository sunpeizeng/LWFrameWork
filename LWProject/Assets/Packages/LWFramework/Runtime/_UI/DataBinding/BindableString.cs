using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LWFramework.Core;

namespace LWFramework.UI
{
    public class BindableString : BindableType
    {

        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="control">绑定的目标控件</param>
        public override void Binding(UIBehaviour control, TableData viewData, object key)
        {
            base.Binding(control, viewData, key);
            if (control is InputField)
            {
                InputField inputField = m_UI as InputField;
                inputField.onValueChanged.AddListener((value) => { m_ViewData[m_Key] = value; });
            }

        }
        public override void Change()
        {
            base.Change();
            var value = m_ViewData.Get<string>(m_Key);
          
           
            if (value == null) {
                return;
            }
            value = value.Replace("\\n", "\n");
            value = value.Replace("\\u3000", "\u3000");
            if (m_UI is InputField)
            {
                InputField inputField = m_UI as InputField;
                inputField.text = value;
            }
            else if (m_UI is Text)
            {
                Text text = m_UI as Text;              
                text.text = value;
            }
        }
      
    }
}
   
