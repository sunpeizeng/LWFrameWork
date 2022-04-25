using libx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.Core;
using Cysharp.Threading.Tasks;

public class ABAssetsManger : IAssetsManager,IManager
{
    private ABInitUpdate _abInitUpdate;
    public System.Action<bool> OnInitUpdateComplete { set => _abInitUpdate.OnInitUpdateComplete = value; }
    /// <summary>
    /// 自定义初始化更新器
    /// </summary>
    //public ABInitUpdate ABInitUpdate { set => _abInitUpdate = value; get => _abInitUpdate; }

    //所有的Request
    private Dictionary<string, AssetRequest> m_RequestDic;
    //快速清理的缓存
    private List<AssetRequest> m_AutoClearRequestList;

    public void Init()
    {
        if (_abInitUpdate == null) {
            _abInitUpdate = new ABInitUpdate();
        }       
        _abInitUpdate.SetConfig();        
        m_RequestDic = new Dictionary<string, AssetRequest>();
        m_AutoClearRequestList = new List<AssetRequest>();
        AssetsInitialize().Forget();
    }

    public void Update()
    {
    }
    public T Load<T>(string path, bool autoClearCache = true)
    {
        AssetRequest request = GetRequest<AssetRequest, T>(path, autoClearCache);//Assets.LoadAsset(path, typeof(T));
                                                                 // AddRequest(path, request);      
        return (T)(object)request.asset;
    }
 
    public async UniTask<T> LoadAsync<T>(string path, bool autoClearCache = true)
    {
        AssetRequest request = GetRequestAsync<AssetRequest, T>(path, autoClearCache);//Assets.LoadAssetAsync(path, typeof(T));
        //AddRequest(path, request);
        await UniTask.WaitUntil(() => request.isDone);
      
        return (T)(object)request.asset;
    }

   
    public void LoadSceneAsync(string scenePath,bool additive, System.Action loadComplete = null, bool releaseScene = true)
    {
        SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(scenePath, additive, releaseScene);
        sceneAssetRequest.completed = (asset) =>
        {
            loadComplete?.Invoke();
        };
       
    }
    
    public T GetRequest<T, K>(string path, bool autoClearCache = true)
    {
        AssetRequest request = Assets.LoadAsset(path, typeof(K));
        if (autoClearCache)
        {
            m_AutoClearRequestList.Add(request);
        }

        AddRequest(path, request);
        return (T)(object)request;
    }
    //在外部使用awati处理异步 可以参考LoadAsync<T>(string path)
    public T GetRequestAsync<T,K>(string path, bool autoClearCache = true)
    {
        AssetRequest request = Assets.LoadAssetAsync(path, typeof(K));
        AddRequest(path, request);
        if (autoClearCache)
        {
            m_AutoClearRequestList.Add(request);
        }
        return (T)(object)request;
    }
    //在外部使用awati处理异步
    public T GetSceneRequestAsync<T>(string scenePath, bool additive)
    {
        SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(scenePath, additive);
        return (T)(object)sceneAssetRequest;
    }
    public void Unload(object param)
    {
        AssetRequest request = ((AssetRequest)param);
        Unload(request.url);
    }
    public void Unload(string path) {
        AssetRequest request;
        if (m_RequestDic.TryGetValue(path, out request))
        {            
            request.Release();
            if (request.refCount <= 0) {
                m_RequestDic.Remove(path);
            }           
        }
    }
    public void UnloadAll(string path)
    {
        AssetRequest request;
        if (m_RequestDic.TryGetValue(path, out request))
        {
            while (request.refCount >0) {
                request.Release();               
            }
            m_RequestDic.Remove(path);
        }
    }
    public void ClearAllAsset()
    {
        foreach (var request in m_RequestDic.Values)
        {
            while (request.refCount > 0)
            {
                request.Release();
            }
        }
        m_RequestDic.Clear();
    }
    public void ClearAutoCacheAsset()
    {
        for (int i = 0; i < m_AutoClearRequestList.Count; i++)
        {
            while (m_AutoClearRequestList[i].refCount > 0)
            {
               
                m_AutoClearRequestList[i].Release();
            }
            m_RequestDic.Remove(m_AutoClearRequestList[i].name);
        }
      
        m_AutoClearRequestList.Clear();
    }
    public void UpdatePatchAndLoadSceneAsync(string scenePath, bool additive, System.Action loadComplete = null)
    {
        string patchName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, (scenePath.LastIndexOf(".") - scenePath.LastIndexOf("/") - 1));
        _abInitUpdate.UpdateAsset(new[] { patchName }, "更新提示", () =>
        {
            SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(scenePath, additive);
            sceneAssetRequest.completed = (asset) =>
            {
                loadComplete?.Invoke();
            };          
        });
    }
    public void UpdatePatchAsset(string patchName, System.Action loadComplete = null)
    {
        _abInitUpdate.UpdateAsset(new[] { patchName }, "更新提示", () =>
        {
            loadComplete?.Invoke();
        });
    }
    public void UpdatePatchAsset(string[] patchNameArray, System.Action loadComplete = null)
    {
        _abInitUpdate.UpdateAsset(patchNameArray, "更新提示", () =>
        {
            loadComplete?.Invoke();
        });
    }
    /// <summary>
    /// 管理所有的Request
    /// </summary>
    /// <param name="path">加载的路径</param>
    /// <param name="request"></param>
    void AddRequest(string path, AssetRequest request)
    {
        if (!m_RequestDic.ContainsKey(path)) {
            m_RequestDic.Add(path, request);
        }       
    }

    /// <summary>
    /// 通过AB路径创建GameObject对象
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <returns>实例化的对象</returns>
    public GameObject InstanceGameObject(string path)
    {
        AssetRequest request = GetRequest<AssetRequest, GameObject>(path,false);
        GameObject go = Object.Instantiate(request.asset) as GameObject;
        request.Require(go);
        return go;
    }
    /// <summary>
    /// 通过AB路径创建GameObject对象
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <param name="parent">父物体</param>
    /// <param name="isWorld">是否世界坐标</param>
    /// <returns>实例化的对象</returns>
    public GameObject InstanceGameObject(string path, Transform parent, bool isWorld)
    {
        AssetRequest request = GetRequest<AssetRequest, GameObject>(path, false);
        GameObject go = Object.Instantiate(request.asset, parent, isWorld) as GameObject;
        request.Require(go);
        return go;
    }

    /// <summary>
    /// 异步通过AB路径创建GameObject对象
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <returns>实例化的对象</returns>
    public async UniTask<GameObject> InstanceGameObjectAsync(string path)
    {
        AssetRequest request = GetRequestAsync<AssetRequest, GameObject>(path);
        await UniTask.WaitUntil(() => request.isDone);
        GameObject go = Object.Instantiate(request.asset) as GameObject;
        request.Require(go);
       
        return go;
    }
    /// <summary>
    /// 异步通过AB路径创建GameObject对象
    /// </summary>
    /// <param name="path">加载AB的路径</param>
    /// <param name="parent">父物体</param>
    /// <param name="isWorld">是否世界坐标</param>
    /// <returns>实例化的对象</returns>
    public async UniTask<GameObject> InstanceGameObjectAsync(string path, Transform parent, bool isWorld)
    {
        AssetRequest request = GetRequestAsync<AssetRequest, GameObject>(path);
        await UniTask.WaitUntil(() => request.isDone);
        GameObject go = Object.Instantiate(request.asset, parent, isWorld) as GameObject;
        request.Require(go);
        return go;
    }
    //延迟5帧初始化，避免出现没有回调
    async UniTaskVoid AssetsInitialize()
    {       
        await UniTask.DelayFrame(5);
        _abInitUpdate.AssetsInitialize();
    }

    public void EnableCheckUnUsedAssets()
    {
        Assets.RemoveUnusedAssets();
    }

   
}
