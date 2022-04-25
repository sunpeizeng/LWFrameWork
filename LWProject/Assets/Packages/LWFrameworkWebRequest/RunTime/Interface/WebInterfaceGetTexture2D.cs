using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LWFramework.WebRequest {
    /// <summary>
    /// 网络接口：获取Texture2D
    /// </summary>
    public sealed class WebInterfaceGetTexture2D : BaseWebInterface
    {
        public Action<Texture2D> Handler;

        public override void OnRequestFinished(DownloadHandler handler)
        {
            if (handler == null)
            {
                Handler?.Invoke(null);
            }
            else
            {
                DownloadHandlerTexture downloadHandler = handler as DownloadHandlerTexture;
                Handler?.Invoke(downloadHandler.texture);
            }
        }

        public override void OnSetDownloadHandler(UnityWebRequest request)
        {
            request.downloadHandler = new DownloadHandlerTexture(true);
        }

        public override string OnGetDownloadString(DownloadHandler handler)
        {
            return "";
        }

        public override void Reset()
        {
            OfflineHandler = null;
            Handler = null;
        }
    }
}
