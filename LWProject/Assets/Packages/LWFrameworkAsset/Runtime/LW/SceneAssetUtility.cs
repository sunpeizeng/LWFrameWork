using Cysharp.Threading.Tasks;
using libx;
using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssetUtility 
{
    public static async UniTask<object> LoadScene_ShowBar(string scenePath, bool additive,float waitTime=0)
    {
        UIWidgetHelp.Instance.OpenLoadingBarView();
        UIWidgetHelp.Instance.SetAppVer(LWUtility.GlobalConfig.appVer);
        UIWidgetHelp.Instance.SetAssetVer(Assets.currentVersions.ver);
        UIWidgetHelp.Instance.SetLoadingBarValue(0);
       // await UniTaskExtend.Delay(waitTime);
        SceneAssetAsyncRequest request =
            MainManager.Instance.GetManager<IAssetsManager>().GetSceneRequestAsync<SceneAssetAsyncRequest>(scenePath, additive);
        float progress = 0;
        float timeProgress = 0;
        while (progress<1)
        {
            await UniTask.WaitForEndOfFrame();           
            progress = request.progress;
            timeProgress = (timeProgress * waitTime + Time.deltaTime) / waitTime;
            if (progress > timeProgress) {
                progress = timeProgress;
            }
           // LWDebug.Log(progress+"  "+ timeProgress);
            //UIWidgetHelp.Instance.SetLoadingBarValue(Mathf.Clamp(request.progress / 0.32f, 0, 1));
            UIWidgetHelp.Instance.SetLoadingBarValue(progress);
        }   
        //打开UI界面
        UIWidgetHelp.Instance.CloseLoadingBarView();
        return null;
    }
    public static async UniTask<object> LoadScene_ShowLoading(string scenePath, bool additive, float waitTime = 0)
    {
        UIWidgetHelp.Instance.OpenLoadingView();
        SceneAssetAsyncRequest request =
            MainManager.Instance.GetManager<IAssetsManager>().GetSceneRequestAsync<SceneAssetAsyncRequest>(scenePath, additive);
        float progress = 0;
        float timeProgress = 0;
        while (progress < 1)
        {
            await UniTask.WaitForEndOfFrame();
            progress = request.progress;
            timeProgress = (timeProgress * waitTime + Time.deltaTime) / waitTime;
            if (progress > timeProgress)
            {
                progress = timeProgress;
            }
        }
        //await UniTask.WaitUntil(() => request.isDone);
      
        UIWidgetHelp.Instance.CloseLoadingView();
        return null;
    }
}
