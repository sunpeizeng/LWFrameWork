using LWFramework.Core;
using LWFramework.WebRequest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebRequesetExample : MonoBehaviour
{
   
    public RawImage rawImage;
    // Start is called before the first frame update
    private void OnEnable()
    {
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        MainManager.Instance.AddManager(typeof(IWebRequestManager).ToString(), new WebRequestManager());

        //MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface("ABC", "http://192.168.100.112:8089/ToolsProject/Bundles/abc.txt", RespABC);
        //MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface("ABC2", "http://192.168.100.112:8089/ToolsProject/Bundles/tt.png", RespABC2);
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/ToolsProject/Bundles/abc.txt", RespABC);
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/ToolsProject/Bundles/tt.png", RespABC2);
    }
    private void OnDisable()
    {
        //MainManager.Instance.GetManager<IWebRequestManager>().UnRegisterInterface("ABC");
       // MainManager.Instance.GetManager<IWebRequestManager>().UnRegisterInterface("ABC2");
    }
    private void RespABC(string obj)
    {
        LWDebug.Log(obj);
        JsonTestData jtd = LitJson.JsonMapper.ToObject<JsonTestData>(obj);
        LWDebug.Log(jtd.name);
    }
    private void RespABC2(Texture2D obj)
    {
        rawImage.texture = obj;
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
    class JsonTestData
    {
        public string name = "";
        public string url = "";
        public int page = 0;
    }
}
