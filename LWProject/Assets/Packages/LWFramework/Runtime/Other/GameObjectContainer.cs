using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectContainer : Singleton<GameObjectContainer>
{
    private Dictionary<string, List<GameObject>> m_Dict = new Dictionary<string, List<GameObject>>();
    private string stepGraphKeyName = "StepGraph";
    /// <summary>
    /// 根据名称查找StepObj对象
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <returns>返回对象</returns>
    public GameObject FindStepObjByName(string name) {
        GameObject ret = FindByName(stepGraphKeyName, name);
       
        return ret;
    }
    /// <summary>
    /// 根据名称查找对象
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <returns>返回对象</returns>
    public GameObject FindByName(string keyName,  string name)
    {
        GameObject ret = null;
        if (m_Dict.ContainsKey(keyName))
        {
            List<GameObject> list = m_Dict[keyName];
            ret = list.Find((GameObject go) =>
            {
                return go.name == name;
            });
        }

        return ret;
    }
    public bool CheckStepObject( GameObject go)
    {
       
        return CheckObject(stepGraphKeyName,go);
    }
    public bool CheckObject(string keyName,GameObject go) {
        bool ret = false;
        if (m_Dict.ContainsKey(keyName))
        {
            ret =  m_Dict[keyName].Contains(go);
        }
        return ret;
    }
    public bool CheckObject(string keyName, string goName)
    {
        bool ret = false;
        if (m_Dict.ContainsKey(keyName))
        {
            ret = m_Dict[keyName].Contains(FindByName(keyName, goName));
        }
        return ret;
    }
    public void AddStepObject( GameObject go)
    {
        AddObject(stepGraphKeyName, go);
    }
    public void AddObject(string keyName, GameObject go) {
        if (m_Dict.ContainsKey(keyName))
        {
            m_Dict[keyName].Add(go);
        }
        else {
            List<GameObject> list = new List<GameObject>();
            m_Dict.Add(keyName, list);
            list.Add(go);
        }
    }
    public void RemoveStepObject(GameObject go)
    {
        RemoveObject(stepGraphKeyName, go);
    }
    public void RemoveObject(string keyName, GameObject go)
    {
        if (m_Dict.ContainsKey(keyName) && go != null)
        {
            m_Dict[keyName].Remove(go);
        }
    }
    public void DestroyStepObject(GameObject go)
    {
        DestroyObject(stepGraphKeyName, go);
    }
    public void DestroyObject(string keyName, GameObject go) {
        if (m_Dict.ContainsKey(keyName) && go!=null)
        {
            m_Dict[keyName].Remove(go);
            GameObject.Destroy(go);
        }
    }
    public void ClearStepObjects() {
        ClearObjects(stepGraphKeyName);
    }
    public void ClearObjects(string keyName) {
        if (m_Dict.ContainsKey(keyName))
        {
            List<GameObject> list = m_Dict[keyName];
            for (int i = 0; i < list.Count; i++)
            {
                GameObject.Destroy(list[i]);
                list.RemoveAt(i);
                i--;
            }
            m_Dict.Remove(keyName);
        }
    }

    public List<GameObject> GetList(string v)
    {
        return m_Dict.ContainsKey(v) ? m_Dict[v] : null;
    }
}
