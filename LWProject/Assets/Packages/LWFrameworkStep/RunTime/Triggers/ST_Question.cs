using LWFramework.Message;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step {

    public class ST_Question : BaseStepTrigger
    {
        public const string OPENQUESTION = "OPENQUESTION";
        public const string CLOSEQUESTION = "CLOSEQUESTION";
        public const string ANSWERQUESTION = "ANSWERQUESTION";
        public const string FINISHED = "FINISHED";
        [LabelText("问题")]
        public string m_Question;
        [LabelText("选项")]
        public AnswerData[] m_AnswerDataArray;
        private string[] m_AnswerArray;

        //增加随机性
        private List<AnswerData> m_RandomAnswerDataList;
        public override void Start()
        {
            base.Start();
            //增加随机数据缓存
            m_RandomAnswerDataList = new List<AnswerData>(m_AnswerDataArray);                    
            //处理数据
            m_AnswerArray = new string[m_AnswerDataArray.Length];
            List<int> rightIndexList = new List<int>();
            for (int i = 0; i < m_AnswerArray.Length; i++)
            {
                AnswerData answerData = m_RandomAnswerDataList[UnityEngine.Random.Range(0, m_RandomAnswerDataList.Count)];
                m_AnswerArray[i] = answerData.answer;
                if (answerData.isRight) {
                    rightIndexList.Add(i);
                }
                m_RandomAnswerDataList.Remove(answerData);
            }
           

            ManagerUtility.MessageMgr.AddListener(FINISHED, OnAnswerQuestion);
            MessageData message = MessageDataPool.GetMessage(OPENQUESTION);
            message["Question"] = m_Question;
            message["AnswerArray"] = m_AnswerArray;
            message["RightIndexArray"] = rightIndexList.ToArray();
            ManagerUtility.MessageMgr.Dispatcher(message);
        }

        private void OnAnswerQuestion(Message.MessageData msg)
        {
            Finished();
        }

        public override void Stop()
        {
            base.Stop();
            //ManagerUtility.UIMgr.CloseView<QuestionView>();
            ManagerUtility.MessageMgr.Dispatcher(CLOSEQUESTION);
            ManagerUtility.MessageMgr.RemoveListener(ANSWERQUESTION, OnAnswerQuestion);
        }
        public override XElement GetXml()
        {
            XElement trigger = new XElement("Trigger");
            trigger.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            trigger.Add(new XAttribute("Qustion", $"{m_Question}"));
            trigger.Add(new XAttribute("ResultIndex", $"{m_NextNum}"));
            XElement answers = new XElement("Datas");
            trigger.Add(answers);
            for (int i = 0; i < m_AnswerDataArray.Length; i++)
            {
                XElement answer = new XElement("Data");
                answer.Add(new XAttribute("Answer", $"{m_AnswerDataArray[i].answer}"));
                answer.Add(new XAttribute("IsRight", $"{m_AnswerDataArray[i].isRight}"));
                answers.Add(answer);
            }
            return trigger;
        }
        public override void SetXml(XElement xElement)
        {
            m_Question = xElement.Attribute("Qustion").Value;
           // m_RightIndex = int.Parse(xElement.Attribute("RightIndex").Value);
            m_NextNum = int.Parse(xElement.Attribute("ResultIndex").Value);
            List<XElement> list = xElement.Element("Datas").Elements("Data").ToList();
            m_AnswerDataArray = new AnswerData[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                AnswerData answerData = new AnswerData
                {
                    answer = list[i].Attribute("Answer").Value,
                    isRight = list[i].Attribute("IsRight").Value== "True" ? true:false,
                };
                m_AnswerDataArray[i] = answerData;
            }
        }
    }
    [Serializable]
    public class AnswerData
    {
        [HorizontalGroup(LabelWidth = 50,Width = 300)]
        public string answer;
        [HorizontalGroup(LabelWidth = 50)]
        public bool isRight;
       
    }
}


