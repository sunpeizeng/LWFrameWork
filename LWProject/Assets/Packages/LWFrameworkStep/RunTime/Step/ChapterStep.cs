using LWFramework.Message;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step {
    public class ChapterStep : BaseStep
    {
        public override void StartControllerList(bool isBack = false)
        {
            base.StartControllerList(isBack);
            m_CurrState = StepNodeState.Execute;
            m_StepManager.MoveNext();
            MessageData msg = MessageDataPool.GetMessage(nameof(StepCommonMessage.StepChapterState));
           
            if (!isBack)
            {
                msg[nameof(StepCommonMessageKey.ChapterIndexOffsetKey)] = 0;
                msg[nameof(StepCommonMessageKey.ChapterStateKey)] = StepNodeState.Complete;
            }
            else
            {
                msg[nameof(StepCommonMessageKey.ChapterIndexOffsetKey)] = 0;
                msg[nameof(StepCommonMessageKey.ChapterStateKey)] = StepNodeState.Wait;
            }
            ManagerUtility.MessageMgr.Dispatcher(msg);

            msg = MessageDataPool.GetMessage(nameof(StepCommonMessage.StepChapterState));
            if (!isBack)
            {
                msg[nameof(StepCommonMessageKey.ChapterIndexOffsetKey)] = 1;
                msg[nameof(StepCommonMessageKey.ChapterStateKey)] =  StepNodeState.Execute;
            }
            else {
                msg[nameof(StepCommonMessageKey.ChapterIndexOffsetKey)] = -1;
                msg[nameof(StepCommonMessageKey.ChapterStateKey)] = StepNodeState.Execute;
            }
            ManagerUtility.MessageMgr.Dispatcher(msg);
        }
        public override void StopControllerList()
        {
            base.StopControllerList();
            //MessageData msg = MessageDataPool.GetMessage(nameof(StepCommonMessage.StepChapterState));
            //msg[nameof(StepCommonMessageKey.ChapterIndexOffsetKey)] = 0;
            //msg[nameof(StepCommonMessageKey.ChapterStateKey)] = StepNodeState.Complete;
            //ManagerUtility.MessageMgr.Dispatcher(msg);
        }
    }
}

