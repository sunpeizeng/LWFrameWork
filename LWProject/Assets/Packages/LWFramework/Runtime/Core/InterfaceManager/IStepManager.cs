using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStepManager
{ 
    Action OnStepAllCompleted { get; set; }
    Action<object> OnStepChange { get; set; }
    object CurrStep { get; }
    //IStep GetNextStepByIndex(int index);
    void StartStep();
    void StopStep();
    void MoveNext();
    void MovePrev();
    void JumpStep(int index);
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    void SetData(object data);

    //void AddGameObject(string name, GameObject go);

    //GameObject GetGameObject(string name);

    //void ClearGameObject();
}
