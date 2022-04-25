
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResAssetsManger : IAssetsManager,IManager
{
    private Action<bool> m_OnUpdateCallback;
    public Action<bool> OnInitUpdateComplete {  set => m_OnUpdateCallback= value; }

    public void Init()
    {
        _ = TaskUpdateAsync();
    }
   
    async UniTaskVoid TaskUpdateAsync()
    {
        await UniTask.WaitForEndOfFrame();
        m_OnUpdateCallback?.Invoke(true);
    }
    public void Update()
    {
    }
    public T Load<T>(string path, bool autoClearCache = true)
    {      
        return (T)(object)Resources.Load(ConverResPath(path), typeof(T));
    }
   
    public async UniTask<T> LoadAsync<T>(string path, bool autoClearCache = true)
    {
        ResourceRequest request = Resources.LoadAsync(ConverResPath(path));
        await UniTask.WaitUntil(() => request.isDone);
        return (T)(object)request.asset;
    }
    
    public void LoadSceneAsync(string scenePath,bool additive, Action loadComplete = null, bool releaseScene = true )
    {
        string sceneName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, (scenePath.LastIndexOf(".") - scenePath.LastIndexOf("/") - 1));
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneName, additive?LoadSceneMode.Additive: LoadSceneMode.Single);
        asyncOperation.completed += (a) =>
        {
            loadComplete?.Invoke();
        };
        
    }
   
    public T GetRequestAsync<T,K>(string path, bool autoClearCache = true)
    {
        ResourceRequest request = Resources.LoadAsync(ConverResPath(path),typeof(K));
        return (T)(object)request;
    }
    public T GetSceneRequestAsync<T>(string scenePath, bool additive)
    {
        string sceneName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, (scenePath.LastIndexOf(".") - scenePath.LastIndexOf("/") - 1));
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        return (T)(object)asyncOperation;
    }
    private string ConverResPath(string path) {
        int startIndex = path.IndexOf("Resources") + "Resources".Length + 1;
        int length = path.LastIndexOf(".") - startIndex;
        string resPath = path.Substring(startIndex, length);
        return resPath;
    }

    public void UpdatePatchAsset(string patchName, Action loadComplete = null)
    {
        LWDebug.LogWarning("Res模式下没用UpdatePatchAsset");
    }
    public void UpdatePatchAsset(string[]patchNameArray, Action loadComplete = null)
    {
        LWDebug.LogWarning("Res模式下没用UpdatePatchAsset");
    }

    public void UpdatePatchAndLoadSceneAsync(string scenePath, bool additive, Action loadComplete = null)
    {
        LWDebug.LogWarning("Res模式下没用UpdatePatchAndLoadSceneAsync");
    }
    public void Unload(object param)
    {
        Resources.UnloadAsset((UnityEngine.Object)param);
    }

    public void Unload(string path)
    {
        LWDebug.LogWarning("Res模式下没用Unload(string path)");
    }
    public void UnloadAll(string path)
    {
        LWDebug.LogWarning("Res模式下没用UnloadAll(string path)");
    }
    public T GetRequest<T, K>(string path, bool autoClearCache = true)
    {
        LWDebug.LogWarning("Res模式下没用GetRequest<T, K>");
        throw new NotImplementedException();
    }

    public GameObject InstanceGameObject(string path)
    {
        GameObject go = UnityEngine.Object.Instantiate(Load<GameObject>(path)) as GameObject;
        return go;
    }

    public GameObject InstanceGameObject(string path, Transform parent, bool isWorld)
    {
        GameObject go = UnityEngine.Object.Instantiate(Load<GameObject>(path),parent,isWorld) as GameObject;
        return go;
    }

    public async UniTask<GameObject> InstanceGameObjectAsync(string path)
    {
        GameObject temp = await LoadAsync<GameObject>(path);
        GameObject go = UnityEngine.Object.Instantiate(temp) as GameObject;
        return go;
    }

    public async UniTask<GameObject> InstanceGameObjectAsync(string path, Transform parent, bool isWorld)
    {
        GameObject temp = await LoadAsync<GameObject>(path);
        GameObject go = UnityEngine.Object.Instantiate(temp,parent, isWorld) as GameObject;
        return go;
    }

    public void EnableCheckUnUsedAssets()
    {
        throw new NotImplementedException();
    }

    public void ClearAllAsset()
    {
        throw new NotImplementedException();
    }

    public void ClearAutoCacheAsset()
    {
        throw new NotImplementedException();
    }
}
