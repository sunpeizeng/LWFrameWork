using Cysharp.Threading.Tasks;
using UnityEngine;
using LWFramework.Core;

namespace LWFramework.UI
{
    public interface IUIManager
    {
        /// <summary>
        /// View的数据
        /// </summary>
        BindableController BindableController { get; }
        /// <summary>
        /// View的数据
        /// </summary>
        TableData ViewData { get;}
        /// <summary>
        /// UI画布
        /// </summary>
        Canvas UICanvas { get; }
        /// <summary>
        /// UI相机
        /// </summary>
        Camera UICamera { get; }
        /// <summary>
        /// 绑定viewName跟UI路径，替换掉UIView种的特性路径，绑定的优先级更高
        /// </summary>
        /// <param name="viewName">ViewName</param>
        /// <param name="uiGameObjectPath">对象的路径</param>
        void BindView(string viewName, string uiGameObjectPath);
        /// <summary>
        /// 预加载UI资源
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        void PreLoadView<T>();
        /// <summary>
        /// 获取View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        /// <param name="viewName">View的名称选填</param>
        /// <returns></returns>
        T GetView<T>(string viewName = null);
        /// <summary>
        /// 获取所有的View
        /// </summary>
        /// <returns></returns>
        IUIView[] GetAllView();
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        T OpenView<T>(bool isFirstSibling = false );
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">View的类型</typeparam>
        /// <param name="viewName">View的名称</param>
        /// <param name="uiGameObject">View的实体对象</param>
        T OpenView<T>(string viewName, GameObject uiGameObject = null,bool isFirstSibling = false);
        /// <summary>
        /// 打开已经缓存过的View
        /// </summary>
        /// <param name="viewName">view的typeof</param>
        /// <param name="isLastSibling">是否放置在最前面</param>
        /// <returns></returns>
        IUIView OpenView(string viewName, bool isLastSibling = false);
        /// <summary>
        /// 使用异步的方式打开UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        UniTask<T> OpenViewAsync<T>(bool isFirstSibling = false);   
             
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <typeparam name="T">保留的View类型（转换成使用typeOf转换）</typeparam>
        void CloseOtherView<T>();
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <typeparam name="T">保留的View类型（转换成使用typeOf转换）</typeparam>
        /// <typeparam name="K">保留的View类型（转换成使用typeOf转换）</typeparam>
        void CloseOtherView<T,K>();
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <typeparam name="T">保留的View类型（转换成使用typeOf转换）</typeparam>
        /// <typeparam name="K">保留的View类型（转换成使用typeOf转换）</typeparam>
        /// <typeparam name="M">保留的View类型（转换成使用typeOf转换）</typeparam>
        void CloseOtherView<T, K, M>();
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <typeparam name="T">保留的View类型（转换成使用typeOf转换）</typeparam>
        /// <typeparam name="K">保留的View类型（转换成使用typeOf转换）</typeparam>
        /// <typeparam name="M">保留的View类型（转换成使用typeOf转换）</typeparam>
        /// <typeparam name="N">保留的View类型（转换成使用typeOf转换）</typeparam>
        void CloseOtherView<T, K, M,N>();
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <param name="viewName">保留的ViewName</param>
        void CloseOtherView(string viewName);
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <param name="viewNameArray">保留的多个ViewName</param>
        void CloseOtherView(string[] viewNameArray);
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <param name="viewName">View的名称</param>
        void CloseView(string viewName);
        /// <summary>
        /// 关闭指定的View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        void CloseView<T>();
        /// <summary>
        /// 关闭所有的View
        /// </summary>
        void CloseAllView();
        /// <summary>
        /// 清空除数组以外的View
        /// </summary>
        /// <param name="viewNameArray"></param>
        void ClearOtherView(string[] viewNameArray);
        /// <summary>
        /// 清空除泛型以外的View
        /// </summary>
        void ClearOtherView<T>();
        /// <summary>
        /// 清空除泛型以外的View
        /// </summary>
        void ClearOtherView<T,K>();
        /// <summary>
        /// 清空除泛型以外的View
        /// </summary>
        void ClearOtherView<T,K,M>();
        /// <summary>
        /// 清空除泛型以外的View
        /// </summary>
        void ClearOtherView<T,K,M,N>();
        /// <summary>
        /// 清空View
        /// </summary>
        /// <param name="viewName">View的名称</param>
        void ClearView(string viewName);
        /// <summary>
        /// 清空View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        void ClearView<T>();
        /// <summary>
        /// 清空所有的View
        /// </summary>
        void ClearAllView();
        
    }
}