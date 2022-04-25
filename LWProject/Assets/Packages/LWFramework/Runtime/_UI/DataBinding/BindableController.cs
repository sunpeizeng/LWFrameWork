using LWFramework;
using LWFramework.Core;
using LWFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum BindableEnum { 
    String,Float,Bool,Texture
}
public class BindableController
{
    private ObjectPool<BindableString> m_BindableStringPool;
    private ObjectPool<BindableBool> m_BindableBoolPool;
    private ObjectPool<BindableFloat> m_BindableFloatPool;
    private ObjectPool<BindableTexture> m_BindableTexturePool;
    //管理所有的绑定数据 key-一般字符串 value-BindableType的集合，用于一个key绑定多个控件
    private Dictionary<object,List<BindableType>> m_keyUIDict;
    //UIManager中的ViewData，共用一个
    private TableData m_ViewData;
    public BindableController(TableData viewData) {
        m_BindableStringPool = new ObjectPool<BindableString>(20);
        m_BindableBoolPool = new ObjectPool<BindableBool>(20);
        m_BindableFloatPool = new ObjectPool<BindableFloat>(20);
        m_BindableTexturePool = new ObjectPool<BindableTexture>(20);

        m_keyUIDict = new Dictionary<object, List<BindableType>>();
        m_ViewData = viewData;
        viewData.OnDataChange += OnViewDataChange;
    }
    public void Binding(object key, UIBehaviour ui, BindableEnum bindableEnum) {

        List<BindableType> bindableList;
        //判断当前key是否已经添加到字典中
        if (!m_keyUIDict.TryGetValue(key, out bindableList))
        {
            bindableList = new List<BindableType>();           
            m_keyUIDict.Add(key, bindableList);          
        }

        BindableType bindableType = CreateBindableType(bindableEnum);
        bindableType.Binding(ui, m_ViewData, key);
        bindableList.Add(bindableType);
        //绑定的时候设置一次change,可以获取未绑定前已经加入的数据
        bindableType.Change();
    }
    private void OnViewDataChange(object key)
    {
        List<BindableType> bindableList;
        if (m_keyUIDict.TryGetValue(key, out bindableList))
        {
            for (int i = 0; i < bindableList.Count; i++)
            {
                BindableType bindableType = bindableList[i];
                //移除空UI的绑定
                if (bindableType.m_UI == null)
                {
                    DeleteBindableType(bindableList[i]);
                    bindableList.RemoveAt(i);
                    i--;
                    if (bindableList.Count == 0)
                    {
                        m_keyUIDict.Remove(key);
                    }
                    continue;
                }
                bindableType.Change();
            }
        }
       
    }
    BindableType CreateBindableType(BindableEnum bindableEnum) {
        BindableType ret = null;
        switch (bindableEnum)
        {
            case BindableEnum.String:
                ret = m_BindableStringPool.Spawn();
                break;
            case BindableEnum.Bool:
                ret = m_BindableBoolPool.Spawn();
                break;
            case BindableEnum.Float:
                ret = m_BindableFloatPool.Spawn();
                break;
            case BindableEnum.Texture:
                ret = m_BindableTexturePool.Spawn();
                break;
            default:
                break;
        }
        return ret;
    }
    void DeleteBindableType(BindableType bindableType)
    {
        if (bindableType is BindableString)
        {
            m_BindableStringPool.Unspawn(bindableType as BindableString);
        }else if (bindableType is BindableBool)
        {
            m_BindableBoolPool.Unspawn(bindableType as BindableBool);
        }
        else if (bindableType is BindableFloat)
        {
            m_BindableFloatPool.Unspawn(bindableType as BindableFloat);
        }
        else if (bindableType is BindableTexture)
        {
            m_BindableTexturePool.Unspawn(bindableType as BindableTexture);
        }

    }
}
