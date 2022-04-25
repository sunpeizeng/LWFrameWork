using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step {
    public class SC_SubStep : BaseStepController
    {
        
        [LabelText("子资源路径")]
        public string m_SubAssetPath;
        private IStepManager stepManager;
        public override void Start()
        {
            stepManager = new StepAssetManager();
            StepAsset stepAsset = ManagerUtility.AssetsMgr.Load<StepAsset>(m_SubAssetPath);
            stepManager.SetData(stepAsset);
          
            stepManager.OnStepAllCompleted = OnSubStepCompleted;
            LWDebug.Log("启动子节点");
        }

        private void OnSubStepCompleted()
        {
            m_ControllerExecuteCompleted?.Invoke();
        }

        public override void Stop()
        {
            stepManager = null;
        }

        public override void Execute()
        {
            stepManager.StartStep();
        }
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("SubAssetPath", $"{m_SubAssetPath}"));         
            control.Add(new XAttribute("Remark", $"{m_Remark}"));

            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_SubAssetPath = xElement.Attribute("SubAssetPath").Value;
            m_Remark = xElement.Attribute("Remark").Value;          
        }

    }
 
}
