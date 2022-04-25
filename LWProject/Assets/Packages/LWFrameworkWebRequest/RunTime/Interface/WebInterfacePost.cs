using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LWFramework.WebRequest {
    /// <summary>
    /// 网络接口：提交表单
    /// </summary>
    public sealed class WebInterfacePost : BaseWebInterface
    {
        public override void OnRequestFinished(DownloadHandler handler)
        {

        }

        public override void OnSetDownloadHandler(UnityWebRequest request)
        {

        }

        public override string OnGetDownloadString(DownloadHandler handler)
        {
            return "";
        }

        public override void Reset()
        {

        }
    }

}
