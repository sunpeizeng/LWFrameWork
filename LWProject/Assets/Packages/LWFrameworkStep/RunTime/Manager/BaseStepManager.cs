using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Step
{
    public abstract class BaseStepManager : IStepManager
    {
        private Dictionary<string, GameObject> m_GameObjectDict = new Dictionary<string, GameObject>();
        public abstract Action OnStepAllCompleted { get; set; }
        public abstract Action<object> OnStepChange { get; set; }
        public abstract object CurrStep { get;  }
        public abstract IStep CurrStepNode { get; set; }

        public abstract void SetData(object data);
        public abstract void StartStep();
        public abstract void StopStep();
        public abstract void JumpStep(int index);

        public abstract void MoveNext();

        public abstract void MovePrev();

    
    }
}