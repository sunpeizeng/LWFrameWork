using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IAssetsManager
{  
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    T Load<T>(string path,bool autoClearCache = true);

    /// <summary>
    /// UniTask异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    UniTask<T> LoadAsync<T>(string path, bool autoClearCache = true);

    /// <summary>
    /// 获取Request
    /// </summary>
    /// <typeparam name="T">返回异步操作的Request对象</typeparam>
    /// <typeparam name="K">返回的资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    T GetRequest<T, K>(string path, bool autoClearCache = true);

    /// <summary>
    /// 异步获取Request,在外部处理回调    
    /// </summary>
    /// <typeparam name="T">返回异步操作的Request对象</typeparam>
    /// <typeparam name="K">返回的资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    T GetRequestAsync<T,K>(string path, bool autoClearCache = true);

    /// <summary>
    /// 回调式异步加载场景 建议直接使用SceneAssetUtility
    /// </summary>
    /// <param name="scenePath"></param>
    /// <param name="additive">是否为添加</param>
    /// <param name="loadComplete">加载完成的回调</param>
    /// <param name="releaseScene">是否释放当前场景 默认释放</param>
    void LoadSceneAsync(string scenePath, bool additive, Action loadComplete = null, bool releaseScene = true);

    /// <summary>
    /// 获取SceneRequest,在外部处理回调    
    /// </summary>
    /// <typeparam name="T">返回异步操作的Request对象</typeparam>
    /// <param name="scenePath">场景的全路径</param>
    /// <param name="additive">是否为添加</param>
    /// <returns></returns>
    T GetSceneRequestAsync<T>(string scenePath, bool additive);

    /// <summary>
    /// 更新资源及加载场景（以场景名进行资源分包）
    /// </summary>
    /// <param name="scenePath"></param>
    /// <param name="additive">是否为添加</param>
    /// <param name="loadComplete">加载完成的回调</param>
    void UpdatePatchAndLoadSceneAsync(string scenePath, bool additive, Action loadComplete = null);

    /// <summary>
    /// 更新资源
    /// </summary>
    /// <param name="patchName">分包名称</param>
    /// <param name="loadComplete">加载完成的回调</param>
    void UpdatePatchAsset(string patchName, Action loadComplete = null);
    /// <summary>
    /// 更新资源
    /// </summary>
    /// <param name="patchName">分包名称</param>
    /// <param name="loadComplete">加载完成的回调</param>
    void UpdatePatchAsset(string[] patchNameArray, Action loadComplete = null);
    /// <summary>
    /// 减少资源引用
    /// </summary>
    /// <param name="param">Res-Object AB-Request</param>
    void Unload(object param);
    /// <summary>
    /// 减少资源引用
    /// </summary>
    /// <param name="param">加载资源的路径</param>
    void Unload(string path);
    /// <summary>
    /// 释放当前资源所有的引用，回收资源
    /// </summary>
    /// <param name="param">加载资源的路径</param>
    void UnloadAll(string path);


    /// <summary>
    /// 释放所有资源的引用 ,慎用
    /// </summary>
    void ClearAllAsset();
    /// <summary>
    /// 释放自动释放的资源的引用 ，可以通过load的autoClearCache进行控制缓存
    /// </summary>
    void ClearAutoCacheAsset();
    /// <summary>
    /// 开启检测无效的资源,检测完毕之后会自动关闭
    /// </summary>
    void EnableCheckUnUsedAssets();
    /// <summary>
    /// 初始化更新回调 Res模式下为空
    /// </summary>
    Action<bool> OnInitUpdateComplete {  set; }


    /// <summary>
    /// 通过AB路径创建GameObject对象， InstanceGameObject的资源会进行监视所以不会进入AutoCacheAsset中
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <returns>实例化的对象</returns>
    GameObject InstanceGameObject(string path);
    /// <summary>
    /// 通过AB路径创建GameObject对象，InstanceGameObject的资源会进行监视所以不会进入AutoCacheAsset中
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <param name="parent">父物体</param>
    /// <param name="isWorld">是否世界坐标</param>
    /// <returns>实例化的对象</returns>
    GameObject InstanceGameObject(string path, Transform parent, bool isWorld);
    /// <summary>
    /// 异步通过AB路径创建GameObject对象，InstanceGameObject的资源会进行监视所以不会进入AutoCacheAsset中
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <returns>实例化的对象</returns>
    UniTask<GameObject> InstanceGameObjectAsync(string path);
    /// <summary>
    /// 异步通过AB路径创建GameObject对象
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <param name="parent">父物体</param>
    /// <param name="isWorld">是否世界坐标</param>
    /// <returns>实例化的对象</returns>
    UniTask<GameObject> InstanceGameObjectAsync(string path, Transform parent, bool isWorld);
}
