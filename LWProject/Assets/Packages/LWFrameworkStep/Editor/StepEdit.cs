using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
namespace LWFramework.Step
{
    public class StepEdit : OdinEditorWindow
    {
        #region 编辑器
        public static StepEdit window;
        [MenuItem("LWFramework/步骤", priority = 5)]
        public static void OpenWindow()
        {
            window = GetWindow<StepEdit>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        }
        #endregion

        [OnValueChanged("ChangeValue")]
        public List<NodeStep> m_StepTriggerList;
        
        [Button("导出xml文件")]
        public void ExportXml()
        {
            string path = UnityEditor.EditorUtility.SaveFilePanel("新增步骤配置文件", Application.dataPath, "Step", "xml");
            XElement config = new XElement("Manager");
            foreach (var item in m_StepTriggerList)
            {
                if (item.GetType() == typeof(NodeStep))
                {
                    NodeStep stepNode = (NodeStep)item;
                    config.Add(stepNode.GetXml());
                }
            }


            config.Save(path);
            UnityEditor.AssetDatabase.Refresh();
        }
        [Button("Test")]
        public void Test()
        {
            NodeStep s = new NodeStep();
            s.m_StepControllerList = new List<IStepController>();
            s.m_StepControllerList.Add(new SC_Active());
            m_StepTriggerList.Add(s);
        }
        public void ChangeValue() {
            Debug.Log("aaaa");
        }
    }
}