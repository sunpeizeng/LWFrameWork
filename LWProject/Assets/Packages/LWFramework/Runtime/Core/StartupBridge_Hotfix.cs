using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LWFramework.Core;
using LWFramework.Message;
using LWFramework.UI;
using UnityEngine;

public class StartupBridge_Hotfix
{
    private static List<Type> m_TypeList = new List<Type>();
    public static void StartCode() {
        //获取DLL ALLtype
        var assembly = Assembly.Load("Assembly-CSharp");
        StartReflection(assembly);
       
    }
    public static void StartReflection(Assembly assembly) {
        //获取DLL ALLtype
        if (assembly == null)
        {
            Debug.Log("当前dll is null");
        }
        m_TypeList = assembly.GetTypes().ToList();
        MainManager.Instance.InitHotfixManager(m_TypeList);
        MainManager.Instance.StartProcedure();
    }
   
    private static void LateUpdate(MessageData msg)
    {
        LWDebug.Log("LateUpdate");
        //iManager.LateUpdate();
    }

    private static void Update(MessageData msg)
    {
        LWDebug.Log("Update");
    }
}
