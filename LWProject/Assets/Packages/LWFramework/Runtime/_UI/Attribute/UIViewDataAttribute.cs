using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWFramework.UI {
    public enum UILayer
    {
        bottom, normal, top, //基础三层次
        world,    //世界坐标
        view,      //父级View节点
        local     //当前已经存在View对象
    }
    public enum FindType
    {
        Tag = 0,
        Name = 1
    }

    public class UIViewDataAttribute : Attribute
    {
        public string m_LoadPath;
        public UILayer m_Layer;
        public FindType m_FindType;
        public string m_Param;
        [Obsolete("use FindType")]
        public UIViewDataAttribute(string p_LoadPath,UILayer p_UILayer)
        {
            this.m_LoadPath = p_LoadPath;
            this.m_Layer = p_UILayer;
        }
        public UIViewDataAttribute(string p_LoadPath, FindType p_FindType,string p_Param)
        {
            this.m_LoadPath = p_LoadPath;
            this.m_FindType = p_FindType;
            this.m_Param = p_Param;
        }
    }
}


