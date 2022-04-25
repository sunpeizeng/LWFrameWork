using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UGUISceneEditor
{
   
    [InitializeOnLoadMethod]
    static void Init()
    {
#if UNITY_2018
        SceneView.onSceneGUIDelegate += OnSceneGUI;
#elif UNITY_2019
        SceneView.duringSceneGui += OnSceneGUI;
#endif

       
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
       
    }


    

   
}
