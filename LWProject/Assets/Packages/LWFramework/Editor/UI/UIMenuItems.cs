using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuItems  
{
    [MenuItem("GameObject/UIFramework/创建View", false, -101)]
    static void CreateViewObject()
    {
        if (EditorApplication.isCompiling)
        {
            return;
        }
        GameObject viewObj = new GameObject("View", typeof(RectTransform), typeof(CanvasGroup));      
        viewObj.transform.SetParent((Selection.activeObject as GameObject).transform, false);       
        Selection.activeObject = viewObj;
        viewObj.layer = LayerMask.NameToLayer("UI");
    }
    //重写Create->UI->Text事件  
    [MenuItem("GameObject/UI/Text2")]
    static void CreatText()
    {
        if (Selection.activeTransform)
        {
            //如果选中的是列表里的Canvas  
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                //新建Text对象  
                GameObject go = new GameObject("Text", typeof(Text));
                //将raycastTarget置为false  
                go.GetComponent<Text>().raycastTarget = false;
                //设置其父物体  
                go.transform.SetParent(Selection.activeTransform,false);
                Selection.activeObject = go;
            }
        }
    }
    //重写Create->UI->Text事件  
    [MenuItem("GameObject/UI/Raw Image2")]
    static void CreatRawImage()
    {
        if (Selection.activeTransform)
        {
            //如果选中的是列表里的Canvas  
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                //新建Text对象  
                GameObject go = new GameObject("RawImage", typeof(RawImage));
                //将raycastTarget置为false  
                go.GetComponent<RawImage>().raycastTarget = false;
                //设置其父物体  
                go.transform.SetParent(Selection.activeTransform,false);
                Selection.activeObject = go;
            }
        }
    }
    [MenuItem("GameObject/UIFramework/复制路径(Shift+C) #c", false, -101)]
    static void CopyParents()
    {
        if (EditorApplication.isCompiling)
        {
            return;
        }
        string path = (Selection.activeObject as GameObject).GetHierarchyPath();//GetParentPath(Selection.activeObject as GameObject, "");
        if (path.Contains("View/")) {
            int index = path.IndexOf("View/")+5;
            path = path.Substring(index, path.Length - index);
        }
        GUIUtility.systemCopyBuffer = path;
        Debug.Log(string.Format("systemCopyBuffer: {0}", path));
    }
    [MenuItem("GameObject/UIFramework/改变界面状态(Shift+T)  #t", false, -101)]
    static void ChangeViewState()
    {
        GameObject view = Selection.activeObject as GameObject;
        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();
        canvasGroup.SetActive(canvasGroup.alpha == 0);
    }


    [MenuItem("GameObject/UIFramework/创建脚本/View", false, -100)]
    static void CreateScriptBtn()
    {
        Object[] array = Selection.objects;
        if (!CheckView(array[0]))
        {
            return;
        }
        List<GameObject> chooseGameObjectList;
        GameObject gameObject;
        GetGameObject(array, out gameObject, out chooseGameObjectList);
        string savePath = EditorUtility.SaveFolderPanel("保存目录", "", "");
        string viewName = gameObject.name;
        CreateView(viewName, "", savePath, gameObject, chooseGameObjectList);
        /**
        Object[] array = Selection.objects;
        
        if (!CheckView(array[0])) {
            return;
        }
        List<GameObject> chooseGameObjectList;
        GameObject gameObject;
        GetGameObject(array, out gameObject, out chooseGameObjectList);

        string savePath = EditorUtility.SaveFolderPanel("保存目录", "", "");        
        string viewName = gameObject.name;
        string generateFilePath = savePath + "/" + viewName + ".cs";
        var sw = new StreamWriter(generateFilePath, false, Encoding.UTF8);
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine("using LWFramework.UI;");
        strBuilder.AppendLine("using UnityEngine.UI;");
        strBuilder.AppendLine("using UnityEngine;");
        strBuilder.AppendLine();
        strBuilder.AppendFormat("[UIViewData(\"\",FindType.Name,\"\")]");
        strBuilder.AppendLine();
        strBuilder.AppendFormat("public class {0} : BaseUIView ", viewName);
        strBuilder.AppendLine();
        strBuilder.AppendLine("{");
        strBuilder.AppendLine();

        //定义控件属性
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);
            strBuilder.AppendFormat("\t[UIElement(\"{0}\")]", GetParentPath(gameObject, item, ""));
            strBuilder.AppendLine();
            strBuilder.AppendFormat("\tprivate {0} {1} = null;", componentName, ConvertName(childName));
            strBuilder.AppendLine();
        }
        strBuilder.AppendLine("\tpublic override  void CreateView(GameObject gameObject)");
        strBuilder.AppendLine("\t{");
        strBuilder.AppendLine("\t\tbase.CreateView(gameObject);");
        List<string> buttons = new List<string>();
        //获取ui控件
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);
            //添加按钮点击事件监听
            if (componentName == "Button")
            {
                strBuilder.AppendFormat("\t\t{0}.onClick.AddListener(() => ", ConvertName(childName));
                strBuilder.AppendLine("\t\t{");
                strBuilder.AppendLine();
                strBuilder.AppendLine("\t\t});");
                strBuilder.AppendLine();
                buttons.Add(childName);
            }
        }
        strBuilder.AppendLine("\t}");


        strBuilder.AppendLine("}");
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    */
    }
    [MenuItem("GameObject/UIFramework/创建脚本/ViewLogic", false, -100)]
    static void CreateViewLogicScriptBtn()
    {
        Object[] array = Selection.objects;
        if (!CheckView(array[0]))
        {
            return;
        }
        List<GameObject> chooseGameObjectList;
        GameObject gameObject;
        GetGameObject(array, out gameObject, out chooseGameObjectList);
        string savePath = EditorUtility.SaveFolderPanel("保存目录", "", "");
        string viewName = gameObject.name;
        string logicName = viewName + "Logic";
        CreateView(viewName, logicName, savePath, gameObject, chooseGameObjectList);
        CreateLogic(viewName, logicName,savePath);
    }
    static void CreateView(string viewName, string logicName, string savePath,GameObject gameObject, List<GameObject> chooseGameObjectList)
    {
       
        string generateFilePath = savePath + "/" + viewName + ".cs";
        var sw = new StreamWriter(generateFilePath, false, Encoding.UTF8);
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine("using LWFramework.UI;");
        strBuilder.AppendLine("using UnityEngine.UI;");
        strBuilder.AppendLine("using UnityEngine;");
        strBuilder.AppendLine();
        strBuilder.AppendFormat("[UIViewData(\"\",FindType.Name,\"\")]");
        strBuilder.AppendLine();
        if (logicName == null || logicName == "")
        {
            strBuilder.AppendFormat("public class {0} : BaseUIView ", viewName);
        }
        else {
            strBuilder.AppendFormat("public class {0} : BaseLogicUIView<{1}>  ", viewName, logicName);
        }       
        strBuilder.AppendLine();
        strBuilder.AppendLine("{");
        strBuilder.AppendLine();
        //获取view上的组建
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);
            strBuilder.AppendFormat("\t[UIElement(\"{0}\")]", GetParentPath(gameObject, item, ""));
            strBuilder.AppendLine();
            strBuilder.AppendFormat("\tprivate {0} {1};", componentName, ConvertName(childName));
            strBuilder.AppendLine();
        }
        strBuilder.AppendLine("\tpublic override  void CreateView(GameObject gameObject)");
        strBuilder.AppendLine("\t{");
        strBuilder.AppendLine("\t\tbase.CreateView(gameObject);");
        List<string> buttons = new List<string>();
        //获取ui控件
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);

            //添加按钮点击事件监听
            if (componentName == "Button")
            {
                strBuilder.AppendFormat("\t\t{0}.onClick.AddListener(() => ", ConvertName(childName));
                strBuilder.AppendLine("\t\t{");
                strBuilder.AppendLine();
                strBuilder.AppendLine("\t\t});");
                strBuilder.AppendLine();
                buttons.Add(childName);
            }
        }
        strBuilder.AppendLine("\t}");


        strBuilder.AppendLine("}");
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void CreateLogic(string viewName, string logicName, string savePath)
    {
        string generateFilePath = savePath + "/" + logicName + ".cs";
        var sw = new StreamWriter(generateFilePath, false, Encoding.UTF8);
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine("using LWFramework.UI;");
        strBuilder.AppendLine("using UnityEngine;");
        strBuilder.AppendLine();
        strBuilder.AppendFormat("public class {0} : BaseUILogic<{1}>  ", logicName, viewName);
        strBuilder.AppendLine();
        strBuilder.AppendLine("{");
        strBuilder.AppendLine();

        strBuilder.AppendFormat("\tpublic {0}({1} view): base(view)", logicName, viewName);
        strBuilder.AppendLine();
        strBuilder.AppendLine("\t{");
        //strBuilder.AppendLine("\t\tbase.CreateView(gameObject);");
        strBuilder.AppendLine("\t}");
        strBuilder.AppendLine("}");
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("GameObject/UIFramework/创建脚本/Node", false, -100)]
    static void CreateNodeScriptBtn()
    {
        Object[] array = Selection.objects;
        if (!CheckNode(array[0]))
        {
            return;
        }
        List<GameObject> chooseGameObjectList;
        GameObject gameObject ;
        GetGameObject(array, out gameObject, out chooseGameObjectList);

        string savePath = EditorUtility.SaveFolderPanel("保存目录", "", "");
        string behaviourName = gameObject.name;
        string generateFilePath = savePath + "/" + behaviourName + ".cs";
        var sw = new StreamWriter(generateFilePath, false, Encoding.UTF8);
        var strBuilder = new StringBuilder();

        strBuilder.AppendLine("using LWFramework.UI;");
        strBuilder.AppendLine("using UnityEngine.UI;");
        strBuilder.AppendLine("using UnityEngine;");
        strBuilder.AppendLine();
        strBuilder.AppendFormat("public class {0} : BaseUINode ", behaviourName);
        strBuilder.AppendLine();
        strBuilder.AppendLine("{");
        strBuilder.AppendLine();
        //获取view上的组建
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);
            strBuilder.AppendFormat("\t[UIElement(\"{0}\")]", GetParentPath(gameObject, item, ""));
            strBuilder.AppendLine();
            strBuilder.AppendFormat("\tprivate {0} {1};", componentName, ConvertName(childName));
            strBuilder.AppendLine();
        }
        strBuilder.AppendLine("\tpublic override  void Create(GameObject gameObject)");
        strBuilder.AppendLine("\t{");
        strBuilder.AppendLine("\t\tbase.Create(gameObject);");
        List<string> buttons = new List<string>();
        //获取ui控件
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);

            //添加按钮点击事件监听
            if (componentName == "Button")
            {
                strBuilder.AppendFormat("\t\t{0}.onClick.AddListener(() => ", ConvertName(childName));
                strBuilder.AppendLine("\t\t{");
                strBuilder.AppendLine();
                strBuilder.AppendLine("\t\t});");
                strBuilder.AppendLine();
                buttons.Add(childName);
            }
        }

        strBuilder.AppendLine("\t}");
        strBuilder.AppendLine("\tpublic override void UnSpawn()");
        strBuilder.AppendLine("\t{");
        strBuilder.AppendLine("\t\tbase.UnSpawn();");
        strBuilder.AppendLine("\t}");
        strBuilder.AppendLine("\tpublic override void Release()");
        strBuilder.AppendLine("\t{");
        strBuilder.AppendLine("\t\tbase.Release();");
        strBuilder.AppendLine("\t}");

        strBuilder.AppendLine("}");
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("GameObject/UIFramework/创建脚本/CopyComponet", false, -100)]
    static void CopyComponet() {
        Object[] array = Selection.objects;
        List<GameObject> chooseGameObjectList;
        GameObject gameObject;
        GetGameObject(array, out gameObject, out chooseGameObjectList);
        var strBuilder = new StringBuilder();
        //获取view上的组建
        foreach (var item in chooseGameObjectList)
        {
            string childName = item.name;
            string componentName = GetComponetName(item);
            strBuilder.AppendFormat("\t[UIElement(\"{0}\")]", GetParentPath(gameObject, item, ""));
            strBuilder.AppendLine();
            strBuilder.AppendFormat("\tprivate {0} {1};", componentName, ConvertName(childName));
            strBuilder.AppendLine();
        }
        GUIUtility.systemCopyBuffer = strBuilder.ToString();
        Debug.Log(strBuilder.ToString());
    }
    static bool CheckView(Object obj) {
        bool ret = true;
        if (!obj.name.Contains("View"))
        {
            Debug.LogError($"{obj} 名称中必须包含View 否则不能创建为View");
            ret = false;
        }
        else if(!(obj is GameObject)){
            Debug.LogError($"{obj} 不是GameObject对象");
            ret = false;
        }
        else if ((obj as GameObject).GetComponent<CanvasGroup>() == null)
        {
            Debug.LogError($"{obj} 没有CanvasGroup组件");
            ret = false;
        }
        return ret;
    }
    static bool CheckNode(Object obj)
    {
        bool ret = true;
        if (!obj.name.Contains("Node"))
        {
            Debug.LogError($"{obj} 名称中必须包含Node 否则不能创建为Node");
            ret = false;
        }
        return ret;
    }
    static void GetGameObject(Object[] array, out GameObject gameObject, out List<GameObject> chooseGameObjectList) {
        if (array.Length <= 1) {
            Debug.LogError($"选中的物体必须两个以上");
        }
        chooseGameObjectList = new List<GameObject>();
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] is GameObject)
            {
                chooseGameObjectList.Add(array[i] as GameObject);
            }
        }
        gameObject = array[0] as GameObject;
    }
    static string GetComponetName(GameObject gameObject)
    {
        if (null != gameObject.GetComponent<ScrollRect>())
            return "ScrollRect";
        else if (null != gameObject.GetComponent<InputField>())
            return "InputField";
        else if (null != gameObject.GetComponent<Button>())
            return "Button";
        else if (null != gameObject.GetComponent<Dropdown>())
            return "Dropdown";
        else if (null != gameObject.GetComponent<Image>())
            return "Image";
        else if (null != gameObject.GetComponent<Text>())
            return "Text";
        else if (null != gameObject.GetComponent<Toggle>())
            return "Toggle";
        else if (null != gameObject.GetComponent<RawImage>())
            return "RawImage";
        else if (null != gameObject.GetComponent<CanvasGroup>())
            return "CanvasGroup";
        return "Transform";
    }
    static string GetParentPath(GameObject gameObject, GameObject child, string str)
    {
        if (child.transform.parent == gameObject.transform)
        {
            str = child.name + str;
            return str;
        }
        else
        {
            str = "/" + child.name + str;
            return GetParentPath(gameObject, child.transform.parent.gameObject, str);
        }

    }
    static string ConvertName(string name)
    {
        return "m_" + name;
    }
}
