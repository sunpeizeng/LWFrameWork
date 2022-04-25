using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace LWFramework.UI
{
    public class UIUtility : Singleton<UIUtility>
    {
        private int m_ViewId;
        public int ViewId
        {
            get => m_ViewId++;
        }
        /// <summary>
        /// 所有UI的父节点缓存，每次使用的都记录一次避免多次查找
        /// </summary>
        private Dictionary<string, Transform> m_UIParentDicCache = new Dictionary<string, Transform>();

        /// <summary>
        /// 创建一个VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BaseUIView CreateView<T>(GameObject uiGameObject = null, string loadPathParam = null)
        {
            BaseUIView uiView = Activator.CreateInstance(typeof(T)) as BaseUIView;

            //获取UIViewDataAttribute特性
            var attr = (UIViewDataAttribute)typeof(T).GetCustomAttributes(typeof(UIViewDataAttribute), true).FirstOrDefault();
            if (attr == null)
            {
                LWDebug.LogError("没有找到UIViewDataAttribute这个特性");
                return null;
            }

            if (uiGameObject == null)
            {
                string loadPath = attr.m_LoadPath;
                //创建UI对象
                if (loadPathParam != null)
                {
                    loadPath = loadPathParam;
                }
                uiGameObject = ManagerUtility.AssetsMgr.InstanceGameObject(loadPath);//InstanceUIGameObject(loadPath);
            }

            SetParent(uiGameObject, attr.m_FindType, attr.m_Param);
            //初始化UI
            uiView.CreateView(uiGameObject);
            //LWDebug.Log("UIManager：" + typeof(T).ToString());
            return uiView;
        }

        /// <summary>
        /// 异步创建一个VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<BaseUIView> CreateViewAsync<T>(GameObject uiGameObject = null, string loadPathParam = null)
        {
            BaseUIView uiView = Activator.CreateInstance(typeof(T)) as BaseUIView;
            //获取UIViewDataAttribute特性
            var attr = (UIViewDataAttribute)typeof(T).GetCustomAttributes(typeof(UIViewDataAttribute), true).FirstOrDefault();
            if (attr == null)
            {
                LWDebug.LogError("没有找到UIViewDataAttribute这个特性");
                return null;
            }
            if (uiGameObject == null)
            {
                string loadPath = attr.m_LoadPath;
                //创建UI对象
                if (loadPathParam != null)
                {
                    loadPath = loadPathParam;
                }
                UIWidgetHelp.Instance.OpenLoadingView();
                uiGameObject = await ManagerUtility.AssetsMgr.InstanceGameObjectAsync(loadPath);
                UIWidgetHelp.Instance.CloseLoadingView();
            }
            SetParent(uiGameObject, attr.m_FindType, attr.m_Param);
            //初始化UI
            uiView.CreateView(uiGameObject);
            LWDebug.Log("UIManager：" + typeof(T).ToString());
            return uiView;
        }
        /// <summary>
        /// 根据特性 获取父级
        /// </summary>
        /// <param name="findType">查找的类型</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public Transform GetParent(FindType findType, string param)
        {
            Transform ret = null;
            if (m_UIParentDicCache.ContainsKey(param))
            {
                ret = m_UIParentDicCache[param];
            }
            else if (findType == FindType.Name)
            {
                GameObject gameObject = GameObject.Find(param);
                if (gameObject == null)
                {
                    LWDebug.LogError(string.Format("当前没有找到{0}这个GameObject对象", param));
                }
                ret = gameObject.transform;
                m_UIParentDicCache.Add(param, ret);
            }
            else if (findType == FindType.Tag)
            {
                GameObject gameObject = GameObject.FindGameObjectWithTag(param);
                if (gameObject == null)
                {
                    LWDebug.LogError(string.Format("当前没有找到{0}这个Tag GameObject对象", param));
                }
                ret = gameObject.transform;
                m_UIParentDicCache.Add(param, ret);
            }
            return ret;
        }


        /// <summary>
        /// 根据特性设置父节点
        /// </summary>
        /// <param name="go"></param>
        /// <param name="findType"></param>
        /// <param name="rootPath"></param>
        private void SetParent(GameObject go, FindType findType, string rootPath)
        {
            Transform parent = UIUtility.Instance.GetParent(findType, rootPath);
            if (parent == null)
            {
                LWDebug.LogError($"没有找到这个{ rootPath}路径的对象节点" );
            }
            else
            {
                go.transform.SetParent(parent, false);
            }

        }

        /// <summary>
        /// 根据ab路径获取精灵图片
        /// </summary>
        /// <param name="abPath">ab的路径</param>
        /// <returns></returns>
        public Sprite GetSprite(string abPath)
        {
            return ManagerUtility.AssetsMgr.Load<UnityEngine.Sprite>(abPath);
        }

        /// <summary>
        /// 根据特性获取UI对象
        /// </summary>
        /// <param name="entity"></param>
        public void SetViewElement(object entity, GameObject uiGameObject)
        {
            Type type = entity.GetType();
            //获取字段属性
            FieldInfo[] infos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //遍历字段属性
            for (int i = 0; i < infos.Length; i++)
            {
                //获取属性上的特性
                object[] attributes = infos[i].GetCustomAttributes(true);
                for (int j = 0; j < attributes.Length; j++)
                {
                    var attribute = attributes[j];
                    if (attribute is UIElementAttribute) {
                        SetUIElementAttribute(entity, infos[i], uiGameObject, attribute as UIElementAttribute);                      
                    }
                    if (attribute is DataBindingAttribute)
                    {
                        SetDataBindingAttribute(entity, infos[i], uiGameObject, attribute as DataBindingAttribute);
                    }
                }
              
            }
        }

        /// <summary>
        /// 设置UIElementAttribute 特性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="objectField"></param>
        /// <param name="uiGameObject"></param>
        /// <param name="uiElement"></param>
        void SetUIElementAttribute( object entity, FieldInfo objectField, GameObject uiGameObject,UIElementAttribute uiElement) {
            try
            {

                UnityEngine.Object obj = uiGameObject.transform.Find(uiElement.m_RootPath).GetComponent(objectField.FieldType);
                //给当前的字段赋值
                objectField.SetValue(entity, obj);
                //处理初始化动态图片
                if (uiElement.m_ResPath != "")
                {
                    if (objectField.FieldType == typeof(UnityEngine.UI.Image))
                    {
                        ((UnityEngine.UI.Image)obj).sprite = GetSprite(uiElement.m_ResPath);
                    }
                    else if (objectField.FieldType == typeof(UnityEngine.UI.Button))
                    {
                        ((UnityEngine.UI.Button)obj).GetComponent<UnityEngine.UI.Image>().sprite = GetSprite(uiElement.m_ResPath);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("当前: {0} 路径上没有找到对应的物体   {1} {2}", uiElement.m_RootPath,e.ToString(), e.StackTrace));

            }
        }

        /// <summary>
        /// 设置DataBindingAttribute特性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="infos"></param>
        /// <param name="uiGameObject"></param>
        /// <param name="dataBinding"></param>
        /**
        void SetDataBindingAttribute(object entity, FieldInfo info, GameObject uiGameObject, DataBindingAttribute dataBinding)
        {
            Type type = entity.GetType();
            PropertyInfo dataInfo = type.GetProperty("Data", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Type dataType = dataInfo.PropertyType;
            object dataValue = dataInfo.GetValue(entity);
            if (dataValue == null)
            {
                dataValue = Activator.CreateInstance(dataType);
                dataInfo.SetValue(entity, dataValue);
            }

            object[] args = new object[1];

            if (!info.FieldType.IsSubclassOf(typeof(UIBehaviour)))
            {
                LWDebug.LogError(string.Format("数据驱动器：数据绑定失败，字段 {0}.{1} 的类型不支持数据绑定，只有 UnityEngine.EventSystems.UIBehaviour 类型支持数据绑定！", type.FullName, info.Name));
                return;
            }
            //绑定的名称
            string target = dataBinding.Target;
            //根据名称获取数据类的字段
            FieldInfo targetField = dataType.GetField(target, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //判断字段是否为空
            if (targetField == null)
            {
                LWDebug.LogError(string.Format("数据驱动器：数据绑定失败，未找到字段 {0}.{1} 绑定的目标数据字段 {2}.{3}！", type.FullName, info.Name, dataType.FullName, target));
                return;
            }
            //判断数据类型
            if (!(targetField.FieldType.BaseType.IsGenericType && targetField.FieldType.BaseType.GetGenericTypeDefinition() == typeof(BindableType<>)))
            {
                LWDebug.LogError(string.Format("数据驱动器：数据绑定失败，目标数据字段 {0}.{1} 并不是可绑定的数据类型 BindableType！", dataType.FullName, target));
                return;
            }

            object targetValue = targetField.GetValue(dataValue);
            if (targetValue == null)
            {
                targetValue = Activator.CreateInstance(targetField.FieldType);
                targetField.SetValue(dataValue, targetValue);
            }

            object controlValue = info.GetValue(entity);
            if (controlValue == null)
            {
                LWDebug.LogError(string.Format("数据驱动器：数据绑定失败，字段 {0}.{1} 是个空引用！", type.FullName, info.Name));
                return;
            }

            MethodInfo binding = targetField.FieldType.GetMethod("Binding", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            args[0] = controlValue;
            binding.Invoke(targetValue, args);
        }
    */

        void SetDataBindingAttribute(object entity, FieldInfo info, GameObject uiGameObject, DataBindingAttribute dataBinding)
        {
            Type type = entity.GetType();
            if (!info.FieldType.IsSubclassOf(typeof(UIBehaviour)))
            {
                LWDebug.LogError(string.Format("数据驱动器：数据绑定失败，字段 {0}.{1} 的类型不支持数据绑定，只有 UnityEngine.EventSystems.UIBehaviour 类型支持数据绑定！", type.FullName, info.Name));
                return;
            }
            object key = dataBinding.Key;
            object controlValue = info.GetValue(entity);
            if (controlValue==null)
            {
                LWDebug.LogError(string.Format("数据驱动器：数据绑定失败，在{0}中找不到对应的组件{1}", entity, info.Name));
                return;
            }
            ManagerUtility.UIMgr.BindableController.Binding(key, (UIBehaviour)controlValue,dataBinding.Bandable);
        }
    }
    
}

