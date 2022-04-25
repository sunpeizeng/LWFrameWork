using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIEditorHelper
{
    static Object LastSelectObj = null;//用来记录上次选中的GameObject，只有它带有Image组件时才把图片赋值给它
    static Object CurSelectObj = null;
    /// <summary>
    /// 选中Image
    /// </summary>
    public static void OnSelectChange()
    {
        if(CurSelectObj!=null)
            LastSelectObj = CurSelectObj;
        
        CurSelectObj = Selection.activeGameObject;
        //如果要遍历目录，修改为SelectionMode.DeepAssets
        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
        if (arr != null && arr.Length > 0)
        {
            GameObject selectObj = LastSelectObj as GameObject;
            if (selectObj != null && (arr[0] is Sprite || arr[0] is Texture2D))
            {
                string assetPath = AssetDatabase.GetAssetPath(arr[0]);
                Image image = selectObj.GetComponent<Image>();
                bool isImgWidget = false;
                if (image != null)
                {
                    isImgWidget = true;
                    SetImageByPath(assetPath, image);
                }

                if (isImgWidget)
                {
                    
                    //赋完图后把焦点还给Image节点
                    EditorApplication.delayCall = delegate
                    {
                        Debug.Log(LastSelectObj);
                        Selection.activeGameObject = LastSelectObj as GameObject;
                        Debug.Log(Selection.activeGameObject);
                    };
                }
            }
        }
    }

    ///// <summary>
    /////  获取配置文件
    ///// </summary>
    ///// <returns></returns>
    //public static UIEditConfig GetUIEditConfig()
    //{
    //    return Resources.Load<UIEditConfig>("UIEditConfig");
    //}
    /// <summary>
    /// 给Image设置图片
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="image"></param>
    public static void SetImageByPath(string assetPath, Image image)
    {
        Object newImg = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));
        Undo.RecordObject(image, "Change Image");//有了这句才可以用ctrl+z撤消此赋值操作
        image.sprite = newImg as Sprite;
       
        if (EditorPrefs.GetBool("IsAutoSizeOnFastSelectImg", false))
            image.SetNativeSize();
        EditorUtility.SetDirty(image);
    }
}
