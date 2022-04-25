
using UnityEngine;

namespace LWFramework.UI
{
    public  class BaseUIView: IUIView
    {
        /// <summary>
        /// UIGameObject
        /// </summary>
        protected GameObject m_Entity;
        protected CanvasGroup m_CanvasGroup;
        /// <summary>
        /// ViewId 动态生成
        /// </summary>
        private int m_ViewId;
        public int ViewId { get => m_ViewId; set => m_ViewId = value; }
        private bool m_IsOpen = false;
        public bool IsOpen {
            get => m_IsOpen;
            set => m_IsOpen = value;
        }
        public virtual void CreateView(GameObject gameObject) {
            m_Entity = gameObject;
            //view上的组件
            UIUtility.Instance.SetViewElement(this, gameObject);
            m_CanvasGroup = m_Entity.GetComponent<CanvasGroup>();
            if (m_CanvasGroup == null) {
                LWDebug.LogError(string.Format("{0}上没有CanvasGroup这个组件", m_Entity.name));
            }
            ViewId = UIUtility.Instance.ViewId;
           
        }
       
        /// <summary>
        /// 打开view
        /// </summary>
        public virtual void OpenView()
        {
            m_CanvasGroup.SetActive(true);
            m_IsOpen = true;         
        }
       
        /// <summary>
        ///关闭view 
        /// </summary>
        public virtual void CloseView() {
            m_CanvasGroup.SetActive(false);
            m_IsOpen = false;
        }
       
        //更新VIEW
        public virtual void UpdateView()
        {
            
        }             
        //删除VIEW
        public virtual void ClearView()
        {
           
            Object.Destroy(m_Entity);
        }
        /// <summary>
        /// 设置view层级
        /// </summary>
        /// <param name="isLastSibling">是否置于最前 默认false</param>
        public virtual void SetViewLastSibling(bool isLastSibling = false)
        {
            if (isLastSibling)
            {
                m_Entity.transform.SetAsLastSibling();
            }
        }
        public virtual void ResetView() { 
        }
    }    
}

