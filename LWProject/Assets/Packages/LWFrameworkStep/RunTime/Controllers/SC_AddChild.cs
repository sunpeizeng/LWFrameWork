using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step {
    public class SC_AddChild : BaseStepObjectController
    {
        
        [LabelText("子物体路径")]
        public string[] m_ChildPath;

        private GameObject m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
            for (int i = 0; i < m_ChildPath.Length; i++)
            {
                string objName = OtherUtility.GetFileNameFromPath(m_ChildPath[i]);
                GameObject go = GameObjectContainer.Instance.FindStepObjByName(objName);
                GameObjectContainer.Instance.RemoveStepObject(go);
            }
        }

        public override void Stop()
        {
            if (m_Target == null)
                return;
            for (int i = 0; i < m_ChildPath.Length; i++)
            {
                Transform child = m_Target.transform.Find(m_ChildPath[i]);
                if (child) {
                    GameObjectContainer.Instance.AddStepObject(child.gameObject);
                }
            }
        }

        public override void Execute()
        {
            m_ControllerExecuteCompleted?.Invoke();
        }
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            XElement datas = new XElement("Datas");
            for (int i = 0; i < m_ChildPath.Length; i++)
            {
                XElement data = new XElement("Data");
                data.Add(new XAttribute("ChildPath", $"{m_ChildPath[i]}"));
                datas.Add(data);
            }
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            control.Add(datas);

            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_Remark = xElement.Attribute("Remark").Value;
            List<XElement> dataList = xElement.Element("Datas").Elements("Data").ToList();
            m_ChildPath = new string[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_ChildPath[i] = dataList[i].Attribute("ChildPath").Value;
            }
        }

    }
 
}
