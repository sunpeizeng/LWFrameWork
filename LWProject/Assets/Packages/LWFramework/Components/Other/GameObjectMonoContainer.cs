using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-500)]
public class GameObjectMonoContainer : MonoBehaviour
{
    public List<GameObjectData> m_GameObjectList;
    void OnEnable()
    {
        for (int i = 0; i < m_GameObjectList.Count; i++)
        {
            GameObjectData objectData = m_GameObjectList[i];
            for (int j = 0; j < objectData.m_GameObjectList.Count; j++)
            {
                GameObjectContainer.Instance.AddObject(objectData.Name, objectData.m_GameObjectList[j]);
            }
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < m_GameObjectList.Count; i++)
        {
            GameObjectData objectData = m_GameObjectList[i];
            for (int j = 0; j < objectData.m_GameObjectList.Count; j++)
            {
                GameObjectContainer.Instance.DestroyObject(objectData.Name, objectData.m_GameObjectList[j]);
            }
        }
    }
}
