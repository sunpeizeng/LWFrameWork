using LWFramework.Step;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "StepAsset", menuName = "LWFramework/Step/StepAsset", order = 0)]
public class StepAsset : SerializedScriptableObject// SerializedMonoBehaviour //ScriptableObject   //Serializable   
{
    [LabelText("步骤集合"),Searchable,ListDrawerSettings(CustomAddFunction = "CustomAddFunction",OnTitleBarGUI = "RefreshBtn", ShowPaging = false),OnValueChanged("ChangeValue")]
    public List<IStep> m_StepNodeList;

#if UNITY_EDITOR
    [Button("导出xml文件")]
    public void ExportXml()
    {
        string path = UnityEditor.EditorUtility.SaveFilePanel("新增步骤配置文件", Application.dataPath, "Step", "xml");
        XElement config = new XElement("Manager");
        foreach (var item in m_StepNodeList)
        {
            config.Add(item.GetXml());
        }


        config.Save(path);
        UnityEditor.AssetDatabase.Refresh();
    }
    [Button("导入xml文件")]
    public void ImprotXml()
    {
        //
        //System.Reflection.Assembly[]aaa =  System.AppDomain.CurrentDomain.GetAssemblies();
        ConverHelp.Instance.CallAssembly = System.Reflection.Assembly.Load("Assembly-CSharp");
        string path = UnityEditor.EditorUtility.OpenFilePanel("选择Xml配置文件", Application.dataPath, "xml");
        if (path.Contains(".xml"))
        {
            m_StepNodeList.Clear();
            string xmlStr = FileTool.ReadFromFile(path);
            XElement root = XElement.Parse(xmlStr);
            List<XElement> stepList = root.Elements("Step").ToList();
            for (int i = 0; i < stepList.Count; i++)
            {
                string str = stepList[i].Attribute("StepScript").Value;
                Debug.Log(str);
                BaseStep baseStepXml = ConverHelp.Instance.CreateInstance<BaseStep>(str);
                baseStepXml.SetXml(stepList[i]);
                m_StepNodeList.Add(baseStepXml);
                //m_BaseStepXmlList.Add(baseStepXml);
            }
        }
        else {
            Debug.LogWarning("请选择xml配置文件");
        }
       
        
    }
    //添加一行
    public void CustomAddFunction() {
        NodeStep stepNode = new NodeStep();
        stepNode.m_Num = m_StepNodeList.Count;
        stepNode.m_UniqueId = MD5Tool.CalcMD5(stepNode.m_Num+DateTool.GetNowStr());
        m_StepNodeList.Add(stepNode);
    }
    public void ChangeValue()
    {
        ResetNum();
    }
    public void RefreshBtn()
    {
        if (Sirenix.Utilities.Editor.SirenixEditorGUI.ToolbarButton(Sirenix.Utilities.Editor.EditorIcons.Refresh)) {
            ResetNum();
        }else if (Sirenix.Utilities.Editor.SirenixEditorGUI.ToolbarButton(Sirenix.Utilities.Editor.EditorIcons.File))
        {
            ResetNum();
        }
    }
    void ResetNum() {
        //记录所有的触发器
        List<BaseStepTrigger> stepTriggers = new List<BaseStepTrigger>();
        //记录触发器下一步进入的Num
        List<int> nextNumList = new List<int>();
        //记录触发器下一步进入的UniqueId
        List<string> nextUniqueIdList = new List<string>();
        for (int i = 0; i < m_StepNodeList.Count; i++)
        {
            IStep step = m_StepNodeList[i];
            if (step is NodeStep) {
                NodeStep stepNode = (NodeStep)step;              
                for (int j = 0; stepNode.m_StepTriggerList!=null&& j < stepNode.m_StepTriggerList.Count; j++)
                {
                    BaseStepTrigger stepTrigger = (BaseStepTrigger)stepNode.m_StepTriggerList[j];
                    stepTriggers.Add(stepTrigger);
                    nextNumList.Add(stepTrigger.m_NextNum);                   
                }
            }
            
        }
        //记录UniqueId
        for (int i = 0; i < nextNumList.Count; i++)
        {
            if (nextNumList[i] == -1) {
                nextUniqueIdList.Add("-1");
                continue;
            }
            for (int j = 0; j < m_StepNodeList.Count; j++)
            {
                IStep step = m_StepNodeList[j];
                if (step is NodeStep)
                {
                    NodeStep stepNode = (NodeStep)step;
                    if (nextNumList[i] == stepNode.m_Num) {
                        nextUniqueIdList.Add(stepNode.m_UniqueId);
                    }
                }

            }
        }
        //重置编号
        for (int i = 0; i < m_StepNodeList.Count; i++)
        {
            m_StepNodeList[i].Num = i;
            if (m_StepNodeList[i].UniqueId == ""|| m_StepNodeList[i].UniqueId == null) {
                m_StepNodeList[i].UniqueId = MD5Tool.CalcMD5(m_StepNodeList[i].Num + DateTool.GetNowStr());
            }
        }
        //还原所有的NextNum
        for (int i = 0; i < stepTriggers.Count; i++)
        {
            if (nextUniqueIdList[i] == "-1")
            {
                stepTriggers[i].m_NextNum = -1;
                continue;
            }
            for (int j = 0; j < m_StepNodeList.Count; j++)
            {
                IStep step = m_StepNodeList[j];
                if (step is NodeStep)
                {
                    NodeStep stepNode = (NodeStep)step;
                    if (nextUniqueIdList[i] == stepNode.UniqueId)
                    {
                        stepTriggers[i].m_NextNum = stepNode.m_Num;
                    }
                }

            }
        }
    }
#endif
}
