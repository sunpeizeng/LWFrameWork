using LWFramework.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class StepAssetManager : StepXmlManager, IManager
    {
        
        public override void SetData(object data)
        {
           
            m_BaseStepXmlList = ((StepAsset)data).m_StepNodeList;
            CollectChapter();


        }
        void CollectChapter() {
            List<string> chapterRemarkList = new List<string>();
            for (int i = 0; i < m_BaseStepXmlList.Count; i++)
            {
                if (m_BaseStepXmlList[i] is ChapterStep)
                {
                    ChapterStep chapterStep = m_BaseStepXmlList[i] as ChapterStep;
                    chapterRemarkList.Add(chapterStep.m_Remark);
                    //LWDebug.Log(chapterStep.m_Remark);
                    
                }
            }

            MessageData msg = MessageDataPool.GetMessage(nameof(StepCommonMessage.StepChapter));
            msg[nameof(StepCommonMessageKey.ChapterKey)] = chapterRemarkList;
            ManagerUtility.MessageMgr.Dispatcher(msg);
        }


    }
}