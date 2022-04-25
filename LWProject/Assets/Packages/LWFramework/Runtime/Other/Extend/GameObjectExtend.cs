using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.Core;

public static class GameObjectExtend
{
    /// <summary>
    /// 获取Hierarchy中的结构路径
    /// </summary>
    /// <param name="gameObject">当前的游戏对象</param>
    /// <returns></returns>
    public static string GetHierarchyPath(this GameObject gameObject)
    {
        return GetParentPath(gameObject, "");
    }
    static string GetParentPath(GameObject child, string str)
    {
        if (child.transform.parent == null)
        {
            str = child.name + str;
            return str;
        }
        else
        {
            str = "/" + child.name + str;
            return GetParentPath(child.transform.parent.gameObject, str);
        }

    }


  
}
