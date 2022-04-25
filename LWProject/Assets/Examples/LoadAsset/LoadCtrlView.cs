using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using LWFramework.Core;
using libx;
using System.Collections.Generic;

[UIViewData("Assets/@Resources/Prefabs/LoadCtrlView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class LoadCtrlView : BaseUIView 
{

	[UIElement("BtnRequireDequire")]
	private Button m_BtnRequireDequire = null;
    [UIElement("BtnLoadUnloadRequest")]
    private Button m_LoadUnloadRequest = null;
    [UIElement("BtnLoadUnloadPath")]
    private Button m_LoadUnloadPath = null;
    [UIElement("BtnLoadUnloadScene")]
    private Button m_BtnLoadScene = null;
    [UIElement("BtnLoad")]
    private Button m_BtnLoad = null;
    [UIElement("BtnUnload")]
    private Button m_BtnUnload = null;

    [UIElement("BtnInstance")]
    private Button m_BtnInstance = null;
    [UIElement("BtnDestory")]
    private Button m_BtnDestory = null;
    [UIElement("BtnUnLoadAll")]
    private Button m_BtnUnLoadAll = null;
    public override  void CreateView(GameObject gameObject)
	{
		base.CreateView(gameObject);
        m_BtnRequireDequire.onClick.AddListener(() => 		{
            RequireDequire();
        });
        m_LoadUnloadPath.onClick.AddListener(() => {
            LoadUnloadPath();
        });
        m_LoadUnloadRequest.onClick.AddListener(() => {
            LoadUnloadRequest();
        });
        m_BtnLoadScene.onClick.AddListener(() => {
            LoadScene();
        });

        m_BtnLoad.onClick.AddListener(() => {
            Load();
        });
        m_BtnUnload.onClick.AddListener(() => {
            UnLoad();
        });

        m_BtnInstance.onClick.AddListener(() => {
            Instance();
        });
        m_BtnDestory.onClick.AddListener(() => {
            Destory();
        });
        m_BtnUnLoadAll.onClick.AddListener(() => {
            ManagerUtility.AssetsMgr.UnloadAll("Assets/@Resources/Prefabs/Cube.prefab");
        });
        Assets.updateUnusedAssetsImmediate = true;
    }

  

    //基于对XAsset的二次封装,基本不使用Require  Dequire
    void RequireDequire() {
        AssetRequest assetRequest = ManagerUtility.AssetsMgr.GetRequest<AssetRequest, GameObject>("Assets/@Resources/Prefabs/Cube.prefab");
        GameObject cube = GameObject.Instantiate(assetRequest.asset, Vector3.zero, Quaternion.identity) as GameObject;
        //将对象加入到Require中,在UpdateRequires的时候会将对象为null的清理,全部清理完成则自动释放资源   
        assetRequest.Require(cube);
        //主动清理掉一个对象,注意使用
        assetRequest.Dequire(cube);
       // Assets.RemoveUnusedAssets();
    }
    //通过路径卸载资源
    void LoadUnloadPath() {
        //assetRequest.Release();
        //Assets.RemoveUnusedAssets();
        AssetRequest assetRequest = ManagerUtility.AssetsMgr.GetRequestAsync<AssetRequest, GameObject>("Assets/@Resources/Prefabs/Cube.prefab");      
        ManagerUtility.AssetsMgr.Unload("Assets/@Resources/Prefabs/Cube.prefab");

        AssetRequest assetRequest2 = ManagerUtility.AssetsMgr.GetRequestAsync<AssetRequest, Texture2D>("Assets/@Resources/Sprites/UnityLogo.png");
        ManagerUtility.AssetsMgr.Unload("Assets/@Resources/Sprites/UnityLogo.png");
    }
    //通过Request卸载资源
    void LoadUnloadRequest()
    {
        AssetRequest assetRequest = ManagerUtility.AssetsMgr.GetRequestAsync<AssetRequest, GameObject>("Assets/@Resources/Prefabs/Cube.prefab");
        ManagerUtility.AssetsMgr.Unload(assetRequest);

        AssetRequest assetRequest2 = ManagerUtility.AssetsMgr.GetRequestAsync<AssetRequest, Texture2D>("Assets/@Resources/Sprites/UnityLogo.png");
        ManagerUtility.AssetsMgr.Unload(assetRequest2);
    }
    //加载场景
    void LoadScene()
    {        
        SceneAssetRequest request = ManagerUtility.AssetsMgr.GetSceneRequestAsync<SceneAssetRequest>("Assets/@Resources/Scenes/HotfixPatch.unity",true);
    }
    //加载资源
    void Load()
    {
        AssetRequest assetRequest = ManagerUtility.AssetsMgr.GetRequest<AssetRequest, GameObject>("Assets/@Resources/Prefabs/Cube.prefab");
        GameObject cube = GameObject.Instantiate(assetRequest.asset, Vector3.zero, Quaternion.identity) as GameObject;
    }
    //释放资源
    void UnLoad() {
        ManagerUtility.AssetsMgr.Unload("Assets/@Resources/Prefabs/Cube.prefab");
    }
    private List<GameObject> gameObjects = new List<GameObject>();
    //实例化对象,并处理引用
    async void Instance()
    {
        GameObject cube = await ManagerUtility.AssetsMgr.InstanceGameObjectAsync("Assets/@Resources/Prefabs/Cube.prefab");
        cube.transform.position = Vector3.zero;
        gameObjects.Add(cube);
    }
    //Destory对象,并处理引用
    void Destory()
    {
        if (gameObjects.Count > 0) {
            Object.Destroy(gameObjects[0]);
            gameObjects.RemoveAt(0);
        }
        
    }
}
