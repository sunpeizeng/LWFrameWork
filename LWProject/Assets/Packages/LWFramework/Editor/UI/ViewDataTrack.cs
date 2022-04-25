using LWFramework.Core;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ViewDataTrack : OdinEditorWindow
{
    public static ViewDataTrack window;
    [LabelText("开启数据追踪"), OnValueChanged("ChangeTrack")]
    public bool m_IsTrackViewData = false;

    private bool m_OpenState = false;
    [TableList(ShowPaging = true, NumberOfItemsPerPage = 20, ShowIndexLabels = true, IsReadOnly = true)]
    public List<ViewDataTrackData> DataList = new List<ViewDataTrackData>();


    [MenuItem("LWFramework/ViewData追踪", priority = -100)]
    public static void OpenWindow()
    {
        window = GetWindow<ViewDataTrack>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);

    }
    protected override void OnGUI()
    {
        base.OnGUI();
        if (m_IsTrackViewData != m_OpenState)
        {
            ChangeTrack();
        }
    }
    void ChangeTrack()
    {
        if (m_IsTrackViewData)
        {
            OpenTrack();
        }
        else
        {
            CloseTrack();
        }
    }
    void OpenTrack()
    {
        DataList.Clear();
        if (ManagerUtility.UIMgr != null)
        {
            ManagerUtility.UIMgr.ViewData.OnDataChange += ViewData_OnViewDataChange;


            TableData tableData = ManagerUtility.UIMgr.ViewData;
            foreach (var item in tableData.Data.Keys)
            {
                ViewDataTrackData data = new ViewDataTrackData() { name = item.ToString(), value = tableData.Data[item].ToString() };
                DataList.Add(data);
            }
            m_OpenState = true;
        }
    }
    void CloseTrack()
    {
        DataList.Clear();
        if (ManagerUtility.UIMgr != null)
        {
            ManagerUtility.UIMgr.ViewData.OnDataChange -= ViewData_OnViewDataChange;
            m_OpenState = false;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        CloseTrack();
        m_IsTrackViewData = false;
    }

    private void ViewData_OnViewDataChange(object obj)
    {
        ViewDataTrackData ret = DataList.Find((ViewDataTrackData find) => {
            return find.name == obj.ToString();
        });
        if (ret != null)
        {
            ret.value = ManagerUtility.UIMgr.ViewData[obj].ToString();
        }
        else
        {
            ViewDataTrackData data = new ViewDataTrackData() { name = obj.ToString(), value = ManagerUtility.UIMgr.ViewData[obj].ToString() };
            DataList.Add(data);
        }

    }


}
[Serializable]
public class ViewDataTrackData
{
    [ReadOnly]
    public string name;
    [ReadOnly]
    public string value;
}
