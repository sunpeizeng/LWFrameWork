using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "UIEditConfig", menuName = "LWFramework/UIEditConfig", order = 0)]
public class UIEditConfig //: ScriptableObject
{

    [LabelText("启动快速更换图片"), OnValueChanged("ChangeFastSelectImage")]
    public bool IsEnableFastSelectImage;

    [LabelText("快速更换图片自动设置大小"), OnValueChanged("ChangeIsAutoSize")]
    public bool IsAutoSizeOnFastSelectImg;

    public UIEditConfig() {
        IsEnableFastSelectImage = EditorPrefs.GetBool("IsEnableFastSelectImage", false);
        IsAutoSizeOnFastSelectImg = EditorPrefs.GetBool("IsAutoSizeOnFastSelectImg", false);
        ImageSize = EditorPrefs.GetString("ImageSize", "1");
        if (IsEnableFastSelectImage) {
            Selection.selectionChanged += UIEditorHelper.OnSelectChange;
        }
    }
    void ChangeFastSelectImage() {
        //选中Image节点并点击图片后即帮它赋上图片
        if (IsEnableFastSelectImage)
        {
            Selection.selectionChanged += UIEditorHelper.OnSelectChange;
        }
        else {
            Selection.selectionChanged -= UIEditorHelper.OnSelectChange;
        }
        EditorPrefs.SetBool("IsEnableFastSelectImage", IsEnableFastSelectImage);
    }
    void ChangeIsAutoSize(){
        EditorPrefs.SetBool("IsAutoSizeOnFastSelectImg", IsAutoSizeOnFastSelectImg);
    }

    [LabelText("缩放系数"),OnValueChanged("OnChangeSize"), FoldoutGroup("图片大小")]
    public string ImageSize = "1";

    void OnChangeSize() {
        EditorPrefs.SetString("ImageSize", ImageSize);
    }
    [Button("重设大小"), FoldoutGroup("图片大小")]
    public void ResetSize()
    {
        float sizeValue = float.Parse(ImageSize);
        GameObject[] goArr = Selection.gameObjects;
        for (int i = 0; i < goArr.Length; i++)
        {
            RectTransform rectTransform = goArr[i].GetComponent<RectTransform>();
            if (rectTransform != null) {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * sizeValue, rectTransform.sizeDelta.y * sizeValue);
               // Undo.re
            }
            else
            {
                EditorWindow editorWindow = EditorWindow.GetWindow(typeof(SceneView));
                editorWindow.ShowNotification(new GUIContent("请先选择UI"));
            }
        }
    }
    [LabelText("Root"), FoldoutGroup("位置")]
    public Transform editParent;
    [LabelText("锚点"),FoldoutGroup("位置")]
    public AnchorType anchorType;
    [LabelText("X边距"), FoldoutGroup("位置")]
    public float offsetX;
    [LabelText("Y边距"), FoldoutGroup("位置")]
    public float offsetY;
    [Button("设置位置"), FoldoutGroup("位置")]
    public void ChangePosi() {
        
        RectTransform rectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
        if (!editParent) {
            editParent = GetParent();
        }
        if (rectTransform != null&& editParent) {
            //记录原来的位置
            int index = rectTransform.GetSiblingIndex();
            //记录原来父物体
            Transform selfParent = rectTransform.parent;
            rectTransform.parent = editParent;
            Vector2 size = rectTransform.sizeDelta;
            Vector2 screen = new Vector2(1920, 1200);
          
            switch (anchorType)
            {
                case AnchorType.LeftDown:
                    rectTransform.localPosition = new Vector3(offsetX + size.x * 0.5f - screen.x * 0.5f, offsetY + size.y * 0.5f - screen.y * 0.5f, 0);
                    break;
                case AnchorType.RightDown:
                    rectTransform.localPosition = new Vector3(-offsetX - size.x * 0.5f + screen.x * 0.5f, offsetY + size.y * 0.5f - screen.y * 0.5f, 0);
                    break;
                case AnchorType.LeftUp:
                    rectTransform.localPosition = new Vector3(offsetX + size.x * 0.5f - screen.x * 0.5f, -offsetY - size.y * 0.5f + screen.y * 0.5f, 0);
                    break;
                case AnchorType.RightUp:
                    rectTransform.localPosition = new Vector3(-offsetX - size.x * 0.5f + screen.x * 0.5f, -offsetY - size.y * 0.5f + screen.y * 0.5f, 0);
                    break;
                default:
                    break;
            }
            //还原
            rectTransform.parent = selfParent;
            rectTransform.SetSiblingIndex(index);
        }
    }
    Transform GetParent() {
      return  Selection.activeGameObject.GetComponentInParent<Canvas>().transform;
    }


    public enum AnchorType { 
        LeftDown,RightDown,LeftUp,RightUp
    }
}
