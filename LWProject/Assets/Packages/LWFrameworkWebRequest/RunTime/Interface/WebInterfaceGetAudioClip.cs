using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LWFramework.WebRequest {
    /// <summary>
    /// 网络接口：获取AudioClip
    /// </summary>
    public sealed class WebInterfaceGetAudioClip : BaseWebInterface
    {
        public Action<AudioClip> Handler;

        public override void OnRequestFinished(DownloadHandler handler)
        {
            if (handler == null)
            {
                Handler?.Invoke(null);
            }
            else
            {
                DownloadHandlerAudioClip downloadHandler = handler as DownloadHandlerAudioClip;
                Handler?.Invoke(downloadHandler.audioClip);
            }
        }

        public override void OnSetDownloadHandler(UnityWebRequest request)
        {
            request.downloadHandler = new DownloadHandlerAudioClip(request.url, AudioType.WAV);
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
