﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LWFramework.Editor.EditorLife
{
    [UnityEditor.InitializeOnLoad]
    public static class EditorLife
    {
        static EditorLife()
        {
            EditorApplication.playModeStateChanged += OnPlayStateChanged;
            EditorApplication.delayCall += OnCompileCode;
            EditorApplication.projectWindowItemOnGUI += ProjectWindowOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowOnGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
           
        }
        private static void ProjectWindowOnGUI(string guid, Rect selectionRect)
        {
            
            //string assetDir = AssetDatabase.GUIDToAssetPath(guid);
            //if (assetDir == "Assets/LWFramework")
            //{
            //    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/LWFramework/Editor/EditorRes/Texture/icon3.png");               
            //    var width = texture.width;
            //    var pos = selectionRect;
            //    pos.x = pos.xMax - width;
            //    pos.width = width;
            //    pos.height = texture.height;
            //    GUI.DrawTexture(pos, texture);
            //}
        }
        private static void HierarchyWindowOnGUI(int instanceid, Rect selectionRect)
        {
            //var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            //if (obj != null)
            //{
            //    if (obj.GetComponent<Startup>() != null)
            //    {
            //        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/LWFramework/Editor/EditorRes/Texture/icon4.png");
            //        var width = texture.width;
            //        var pos = selectionRect;
            //        pos.x = pos.xMax - width + 15;
            //        pos.width = width;
            //        pos.height = texture.height;
            //        GUI.DrawTexture(pos, texture);
            //    }
            //}
        }


        private static void OnCompileCode()
        {

        }
        [UnityEditor.Callbacks.DidReloadScripts]
        static void AllScriptsReloaded()
        {
            //Debug.Log("编译完成");


        }
      

        public static string playSceneName;
        private static void OnPlayStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                
            }
            else if (state == PlayModeStateChange.ExitingEditMode)
            {
               
                LWGlobalConfigTool.Instance.CreateConfig();
                if (FileTool.ExistsFile(Application.dataPath + "/Resources/LWGlobalAsset.asset"))
                {
                    FileTool.DeleteFile(Application.dataPath + "/Resources/LWGlobalAsset.asset");
                }
                var createAsset = ScriptableObject.CreateInstance(typeof(LWGlobalAsset));
                AssetDatabase.CreateAsset(createAsset, "Assets/Resources/LWGlobalAsset.asset");
                AssetDatabase.Refresh();
                //if (!FileTool.ExistsFile(Application.dataPath + "/Resources/LWGlobalAsset.asset"))
                //{
                //    var asset = ScriptableObject.CreateInstance(typeof(LWGlobalAsset));
                //    AssetDatabase.CreateAsset(asset, "Assets/Resources/LWGlobalAsset.asset");
                //}
                //LWDebug.Log(asset);
                //Resources.Load<LWGlobalAsset>("LWGlobalAsset").CreateJson();
            }
        }


    }
}