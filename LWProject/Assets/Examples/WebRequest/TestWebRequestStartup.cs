using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using LWFramework.WebRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWebRequestStartup : MonoBehaviour
{
    public RawImage rawImage;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        MainManager.Instance.AddManager(typeof(IWebRequestManager).ToString(), new WebRequestManager());

        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/LWProject/Bundles/Windows/WebRequest/Req.txt", RespQueryScene);
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/LWProject/Bundles/Windows/WebRequest/abc.png", RespImage);
        //通过form传参获取接口
        WWWForm form = new WWWForm();
        CourseData courseData = new CourseData { courseId = "20" };
        form.AddField("jsonParam", LitJson.JsonMapper.ToJson(courseData));
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.124:8288/psych/app/queryCourseById", RespABC, form);


        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://47.99.212.190:7001/api/sys-manager-web/virtual/examination?type=0", RespQueryScene);


        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrlJson("http://47.99.212.190:7001/api/sys-manager-web/virtual/uploadScore", RespQueryScene, "{\"examId\":\"0\",\"stuId\":\"\",\"score\":\"24\"}");
    }

    private void RespQueryScene(string obj)
    {
        LWDebug.Log(obj);
    }

    private void RespABC(string obj)
    {
        LWDebug.Log(obj);
        JsonTestData jtd = LitJson.JsonMapper.ToObject<JsonTestData>(obj);
        LWDebug.Log(jtd.name);
    }
    private void RespImage(Texture2D obj)
    {
        rawImage.texture = obj;
    }
    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();        
    }

   
}

public class JsonTestData : NetMsg.BaseJsonData
{
    public string name;
    public string url;
    public int page;
}
public class SceneData
{
    public string sceneId;
}
public class CourseData
{
    public string courseId;
}