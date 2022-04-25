using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LWFramework.UI {
    /// <summary>
    /// 所有的UI管理器
    /// </summary>
    //[ManagerClass(ManagerType.Normal)]
    public class UIManager : IManager, IUIManager
    {
        /// <summary>
        /// 所有的view字典
        /// </summary>
        protected Dictionary<string, IUIView> m_UIViewDic;
        /// <summary>
        /// 所有的view集合
        /// </summary>
        protected List<IUIView> m_UIList;

        /// <summary>
        /// 所有的绑定数据
        /// </summary>
        protected Dictionary<string, string> m_UIBindViewPath;

        private BindableController m_BindableController;
        //绑定数据的控制器

        public BindableController BindableController
        {
            get { return m_BindableController; }

        }
        private TableData m_ViewData;
        //View的数据

        public TableData ViewData {
            get { return m_ViewData; }
            
        }
        private Canvas m_UICanvas;
        public Canvas UICanvas
        {
            get
            {
                if (m_UICanvas == null)
                {
                    m_UICanvas = GameObject.Find("LWFramework/Canvas").GetComponent<Canvas>();
                }
                return m_UICanvas;
            }
        }
        private Camera m_UICamera;
        public Camera UICamera
        {
            get
            {
                if (m_UICamera == null)
                {
                    m_UICamera = GameObject.Find("LWFramework/Canvas/UICamera").GetComponent<Camera>();
                }
                return m_UICamera;
            }
        }
        #region 获取Canvas编辑节点
        private Transform _editTransform;
        private Transform EditTransform
        {
            get
            {
                if (_editTransform == null)
                {
                    _editTransform = GameObject.Find("LWFramework/Canvas/Edit").transform;
                }
                return _editTransform;
            }
        }

        #endregion
        public virtual void Init()
        {
            m_UIViewDic = new Dictionary<string, IUIView>();
            m_UIList = new List<IUIView>();
            m_UIBindViewPath = new Dictionary<string, string>();          
            m_ViewData = new TableData();
            m_BindableController = new BindableController(m_ViewData);
            //启动之后隐藏编辑层
            EditTransform.gameObject.SetActive(false);
        }
        /// <summary>
        /// 更新所有的View
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < m_UIList.Count; i++)
            {
                if(m_UIList[i].IsOpen)
                    m_UIList[i].UpdateView();
            }
        }
      
        public void BindView(string viewName, string uiGameObjectPath)
        {
            if (!m_UIBindViewPath.ContainsKey(viewName)) {
                m_UIBindViewPath.Add(viewName, uiGameObjectPath);
            }
        }
        /// <summary>
        /// 预加载View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public virtual void PreLoadView<T>() {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(typeof(T).ToString(), out uiViewBase))
            {
                uiViewBase = UIUtility.Instance.CreateView<T>();
                m_UIViewDic.Add(typeof(T).ToString(), uiViewBase);
                m_UIList.Add(uiViewBase);
            }
            uiViewBase.CloseView();
        }
       /// <summary>
       /// 打开已经缓存过的View
       /// </summary>
       /// <param name="viewName">view的typeof</param>
       /// <param name="isLastSibling">是否放置在最前面</param>
       /// <returns></returns>
        public IUIView OpenView(string viewName,bool isLastSibling = false)
        {
            IUIView uiViewBase;
            if (m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                uiViewBase.OpenView();

                uiViewBase.SetViewLastSibling(isLastSibling);
            }
            else {
                Debug.LogError($"目前没有{viewName}的View");
            }          
            return uiViewBase;
        }
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">view的控制类</typeparam>
        /// <param name="isLastSibling">是否放置在最前面</param>
        public T OpenView<T>(bool isLastSibling = false)
        {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(typeof(T).ToString(), out uiViewBase))
            {
                uiViewBase = UIUtility.Instance.CreateView<T>();
                m_UIViewDic.Add(typeof(T).ToString(), uiViewBase);
                m_UIList.Add(uiViewBase);
            }
            uiViewBase.OpenView();

            uiViewBase.SetViewLastSibling(isLastSibling);
            return (T)uiViewBase;
        }
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">view的控制类</typeparam>
        /// <param name="viewName">view的名字，用于一个多个页面共用一个类</param>
        /// <param name="uiGameObject">view的对象，提前创建，优先级高于自己创建</param>
        /// <param name="isLastSibling">是否放置在最前面</param>
        public T OpenView<T>(string viewName, GameObject uiGameObject = null , bool isLastSibling = false)
        {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                if (m_UIBindViewPath.ContainsKey(viewName))
                {
                    uiViewBase = UIUtility.Instance.CreateView<T>(uiGameObject, m_UIBindViewPath[viewName]);
                }
                else {
                    uiViewBase = UIUtility.Instance.CreateView<T>(uiGameObject);
                }               
                m_UIViewDic.Add(viewName, uiViewBase);
                m_UIList.Add(uiViewBase);
            }
            uiViewBase.OpenView();
            uiViewBase.SetViewLastSibling(isLastSibling);
            return (T)uiViewBase;
        }
     
        /// <summary>
        /// 获取VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetView<T>(string p_ViewName = null)
        {
            IUIView ret = null;
            string viewName = typeof(T).ToString();
            if (p_ViewName != null)
            {
                viewName = p_ViewName;
            }          
            m_UIViewDic.TryGetValue(viewName, out ret);
            return (T)ret;
        }
        /// <summary>
        /// 获取所有的VIEW
        /// </summary>
        /// <returns></returns>
        public IUIView[] GetAllView() {
            return m_UIList.ToArray<IUIView>();
        }
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseView<T>()
        {
            CloseView(typeof(T).ToString());
        }
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseView(string viewName)
        {
            IUIView uiViewBase;
            if (m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                CloseView(uiViewBase);
            }
        }
        void CloseView(IUIView uiViewBase) {
            uiViewBase.CloseView();
        }
        /// <summary>
        /// 关闭其他所有的View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseOtherView<T>()
        {
            CloseOtherView(typeof(T).ToString());
        }
        public void CloseOtherView<T,K>()
        {
            CloseOtherView(new string[] { typeof(T).ToString(), typeof(K).ToString() });
        }
        public void CloseOtherView<T, K, M>()
        {
            CloseOtherView(new string[] { typeof(T).ToString(), typeof(K).ToString(), typeof(M).ToString() });
        }
        public void CloseOtherView<T, K, M , N>()
        {
            CloseOtherView(new string[] { typeof(T).ToString(), typeof(K).ToString(), typeof(M).ToString(), typeof(N).ToString() });
        }
        public void CloseOtherView(string viewName) {
            foreach (var item in m_UIViewDic.Keys)
            {
                if (item != viewName)
                {
                    CloseView(m_UIViewDic[item]);
                }
            }
        }
        public void CloseOtherView(string[] viewNameArray)
        {
            foreach (var item in m_UIViewDic.Keys)
            {
                bool canClose = true;
                for (int i = 0; i < viewNameArray.Length; i++)
                {
                    if (item == viewNameArray[i])
                    {
                        canClose = false;
                        break;
                          // m_UIViewDic[item].CloseView();
                    }
                }
                if(canClose)
                    CloseView(m_UIViewDic[item]);
            }
        }
        /// <summary>
        /// 关闭所有的view
        /// </summary>
        public void CloseAllView()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                CloseView(item);
            }
        }
        public void ClearView(string viewName) {
            IUIView uiViewBase;
            if (m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                uiViewBase.CloseView();
                uiViewBase.ClearView();
                m_UIViewDic.Remove(viewName);
                if (m_UIList.Contains(uiViewBase)) {
                    m_UIList.Remove(uiViewBase);
                }
            }         
        }
        public void ClearView<T>() {
            ClearView(typeof(T).ToString());
        }
        /// <summary>
        /// 清理所有的view
        /// </summary>
        public void ClearAllView()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                item.CloseView();
                item.ClearView();
            }
            m_UIViewDic.Clear();
            m_UIList.Clear();
        }
        public void ClearOtherView(string [] viewNameArray) {
            List<string> clearList = new List<string>();
            foreach (var item in m_UIViewDic.Keys)
            {
                bool canClear = true;
                for (int i = 0; i < viewNameArray.Length; i++)
                {
                    if (item == viewNameArray[i])
                    {
                        canClear = false;
                        break;
                    }
                }
                if (canClear) {
                    clearList.Add(item);
                }
            }
            for (int i = 0; i < clearList.Count; i++)
            {
                ClearView(clearList[i]);
            }
        }
        public void ClearOtherView<T>()
        {
            ClearOtherView(new string[] { typeof(T).ToString()});
        }
        public void ClearOtherView<T,K>()
        {
            ClearOtherView(new string[] { typeof(T).ToString(), typeof(K).ToString()});
        }
        public void ClearOtherView<T,K,M>()
        {
            ClearOtherView(new string[] { typeof(T).ToString(), typeof(K).ToString(), typeof(M).ToString()});
        }
        public void ClearOtherView<T,K,M,N>()
        {
            ClearOtherView(new string[] { typeof(T).ToString(), typeof(K).ToString(), typeof(M).ToString(), typeof(N).ToString() });
        }
        public virtual Cysharp.Threading.Tasks.UniTask<T> OpenViewAsync<T>(bool isFirstSibling = false)
        {
            LWDebug.LogError("不支持异步打开View");
            throw new NotImplementedException();
        }
        
    }
}

